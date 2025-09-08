using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.UI;
using ERP.Enums;
using ERP.Modules.Finance.ChartOfAccount.COALevel04;
using ERP.Modules.Finance.GeneralLedger;
using ERP.Modules.Finance.GeneralPayments;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace ERP.Modules.Finance.GeneralPayment
{

    public class GeneralPaymentAppService : ERPDocumentService<GeneralPaymentInfo>
    {
        public IRepository<GeneralPaymentInfo, long> FINANCE_GeneralPayment_Repo { get; set; }
        public IRepository<COALevel04Info, long> COALevel04_Repo { get; set; }
        public IRepository<GeneralLedgerInfo, long> GeneralLedger_Repo { get; set; }

        public async Task<PagedResultDto<FINANCE_GeneralPaymentGetAllDto>> GetAll(FINANCE_GeneralPaymentFiltersDto filters)
        {
            var finance_general_payment_query = FINANCE_GeneralPayment_Repo.GetAll(this).Include(i => i.GeneralPaymentDetails).ApplyDocumentFilters(filters);
            if (!string.IsNullOrWhiteSpace(filters.BankCOALevel04Id))
                finance_general_payment_query = finance_general_payment_query.Where(i => i.BankCOALevel04Id == filters.BankCOALevel04Id.TryToLong());
            if (filters.LinkedDocument != null)
                finance_general_payment_query = finance_general_payment_query.Where(i => i.LinkedDocument == filters.LinkedDocument.Value);
            var finance_general_payments = await finance_general_payment_query.ToPagedListAsync(filters);

            var bank_coa_level04_ids = finance_general_payments.Select(i => i.BankCOALevel04Id).ToList();

            var total_count = finance_general_payment_query.DeferredCount().FutureValue();
            var bank_coa_level04s = COALevel04_Repo.GetAll(this, i => bank_coa_level04_ids.Contains(i.Id)).Future();
            await bank_coa_level04s.ToListAsync();

            var dict_bank_coa_level04s = bank_coa_level04s.ToDictionary(i => i.Id);

            var all_detail_coa_level04_ids = finance_general_payments
                .SelectMany(p => p.GeneralPaymentDetails ?? new List<GeneralPaymentDetailsInfo>())
                .Select(d => d.COALevel04Id)
                .Distinct()
                .ToList();
            var all_detail_coa_level04s = COALevel04_Repo.GetAll(this, i => all_detail_coa_level04_ids.Contains(i.Id)).ToList();
            var dict_detail_coa_level04s = all_detail_coa_level04s.ToDictionary(i => i.Id, i => i.Name);

            var output = new List<FINANCE_GeneralPaymentGetAllDto>();
            foreach (var finance_general_payment in finance_general_payments)
            {
                dict_bank_coa_level04s.TryGetValue(finance_general_payment?.BankCOALevel04Id ?? 0, out var bank_coa_level04);
                var dto = ObjectMapper.Map<FINANCE_GeneralPaymentGetAllDto>(finance_general_payment);
                dto.BankCOALevel04Name = bank_coa_level04?.Name ?? "";
                dto.ReferenceNumber = finance_general_payment.ReferenceNumber;
                dto.ReferenceDate = finance_general_payment.ReferenceDate;
                dto.MaturityDate = finance_general_payment.MaturityDate;
                dto.IsCheque = finance_general_payment.IsCheque;
                dto.IsCrossedCheque = finance_general_payment.IsCrossedCheque;
                dto.ChequeTitle = finance_general_payment.ChequeTitle;
                dto.ChequeNumber = finance_general_payment.ChequeNumber;
                dto.TotalAmount = finance_general_payment.TotalAmount;
                dto.LinkedDocument = finance_general_payment.LinkedDocument;
                dto.GeneralPaymentDetails = finance_general_payment.GeneralPaymentDetails != null
                    ? finance_general_payment.GeneralPaymentDetails.Select(detail => {
                        var detailDto = ObjectMapper.Map<GeneralPaymentDetailsDto>(detail);
                        dict_detail_coa_level04s.TryGetValue(detail.COALevel04Id, out var coaName);
                        detailDto.COALevel04Name = coaName ?? string.Empty;
                        detailDto.InvoiceNumber = detail.InvoiceNumber;
                        return detailDto;
                    }).ToList()
                    : new List<GeneralPaymentDetailsDto>();
                output.Add(dto);
            }

            return new PagedResultDto<FINANCE_GeneralPaymentGetAllDto>(total_count.Value, output);
        }

        public async Task<string> Create(FINANCE_GeneralPaymentDto input)
        {
            var bank_coa_level04 = COALevel04_Repo.GetAll(this, i => i.Id == input.BankCOALevel04Id).DeferredFirstOrDefault().FutureValue();
            await bank_coa_level04.ValueAsync();
            if (bank_coa_level04.Value == null)
                throw new UserFriendlyException($"BankCOALevel04Id: '{input.BankCOALevel04Id}' is invalid.");

            var entity = ObjectMapper.Map<GeneralPaymentInfo>(input);
            entity.Status = "PENDING";
            var voucher_prefix = GetVoucherPrefix(input.LinkedDocument);
            entity.VoucherNumber = await GetVoucherNumberWithDocument(voucher_prefix, input.IssueDate, input.LinkedDocument, 0);
            entity.ReferenceNumber = input.ReferenceNumber;
            entity.ReferenceDate = input.ReferenceDate;
            entity.MaturityDate = input.MaturityDate;
            entity.IsCheque = input.IsCheque;
            entity.IsCrossedCheque = input.IsCrossedCheque;
            entity.ChequeTitle = input.ChequeTitle;
            entity.ChequeNumber = input.ChequeNumber;
            entity.LinkedDocument = input.LinkedDocument;
            entity.TotalAmount = (input.GeneralPaymentDetails ?? new List<GeneralPaymentDetailsDto>())
                .Sum(d => d.NetAmount ?? d.GrossAmount);

            var coa_level04_ids = input.GeneralPaymentDetails.Select(i => i.COALevel04Id).ToList();
            var tax_coa_level04_ids = input.GeneralPaymentDetails.Select(i => (long?)i.TaxCOALevel04Id).Where(id => id.HasValue).Select(id => id.Value).ToList();
            var other_tax_coa_level04_ids = input.GeneralPaymentDetails.Select(i => (long?)i.OtherTaxCOALevel04Id).Where(id => id.HasValue).Select(id => id.Value).ToList();
            var general_ledger_ids = input.GeneralPaymentDetails.Select(i => (long?)i.GeneralLedgerId).Where(id => id.HasValue).Select(id => id.Value).ToList();

            var coa_level04s = COALevel04_Repo.GetAll(this, i => coa_level04_ids.Contains(i.Id)).Select(i => i.Id).Future();
            var tax_coa_level04s = COALevel04_Repo.GetAll(this, i => tax_coa_level04_ids.Contains(i.Id)).Select(i => i.Id).Future();
            var other_tax_coa_level04s = COALevel04_Repo.GetAll(this, i => other_tax_coa_level04_ids.Contains(i.Id)).Select(i => i.Id).Future();
            var general_ledgers = GeneralLedger_Repo.GetAll(this, i => general_ledger_ids.Contains(i.Id)).Select(i => i.Id).Future();
            _ = await general_ledgers.ToListAsync();

            for (int i = 0; i < entity.GeneralPaymentDetails.Count; i++)
            {
                var detail = entity.GeneralPaymentDetails[i];
                if (!coa_level04s.Contains(detail.COALevel04Id))
                    throw new UserFriendlyException($"COALevel04Id: '{detail.COALevel04Id}' is invalid at Row: '{i + 1}'.");
                if (detail.TaxCOALevel04Id.HasValue && !tax_coa_level04s.Contains(detail.TaxCOALevel04Id.Value))
                    throw new UserFriendlyException($"TaxCOALevel04Id: '{detail.TaxCOALevel04Id}' is invalid at Row: '{i + 1}'.");
                if (detail.OtherTaxCOALevel04Id.HasValue && !other_tax_coa_level04s.Contains(detail.OtherTaxCOALevel04Id.Value))
                    throw new UserFriendlyException($"OtherTaxCOALevel04Id: '{detail.OtherTaxCOALevel04Id}' is invalid at Row: '{i + 1}'.");
                detail.VoucherNumber = $"{entity.VoucherNumber}/{i + 1}";
                detail.GeneralPaymentInfoId = entity.Id;
                // Ensure InvoiceNumber flows from input to entity in case mapper isn't updated
                if (i < (input.GeneralPaymentDetails?.Count ?? 0))
                    detail.InvoiceNumber = input.GeneralPaymentDetails[i].InvoiceNumber;
            }

            entity.TenantId = AbpSession.TenantId;
            await FINANCE_GeneralPayment_Repo.InsertAsync(entity);
            await CurrentUnitOfWork.SaveChangesAsync();

            return "FINANCE_GeneralPayment Created Successfully.";
        }

        private async Task<GeneralPaymentInfo> Get(long Id)
        {
            var finance_general_payment = await FINANCE_GeneralPayment_Repo.GetAllIncluding(i => i.GeneralPaymentDetails).Where(i => i.Id == Id).FirstOrDefaultAsync();
            if (finance_general_payment != null)
                return finance_general_payment;
            else
                throw new UserFriendlyException($"FINANCE_GeneralPaymentId: '{Id}' is invalid.");
        }

        public async Task<FINANCE_GeneralPaymentGetForEditDto> GetForEdit(long Id)
        {
            var finance_general_payment = await Get(Id);
            var bank_coa_level04 = COALevel04_Repo.GetAll(this, i => i.Id == finance_general_payment.BankCOALevel04Id).DeferredFirstOrDefault().FutureValue();
            await bank_coa_level04.ValueAsync();

            var output = ObjectMapper.Map<FINANCE_GeneralPaymentGetForEditDto>(finance_general_payment);
            output.BankCOALevel04Name = bank_coa_level04?.Value?.Name ?? "";
            output.ReferenceNumber = finance_general_payment.ReferenceNumber;
            output.ReferenceDate = finance_general_payment.ReferenceDate;
            output.MaturityDate = finance_general_payment.MaturityDate;
            output.IsCheque = finance_general_payment.IsCheque;
            output.IsCrossedCheque = finance_general_payment.IsCrossedCheque;
            output.ChequeTitle = finance_general_payment.ChequeTitle;
            output.ChequeNumber = finance_general_payment.ChequeNumber;
            output.LinkedDocument = finance_general_payment.LinkedDocument;

            var coa_level04_ids = finance_general_payment.GeneralPaymentDetails.Select(i => i.COALevel04Id).ToList();
            var tax_coa_level04_ids = finance_general_payment.GeneralPaymentDetails.Select(i => i.TaxCOALevel04Id).Where(id => id.HasValue).Select(id => id.Value).ToList();
            var other_tax_coa_level04_ids = finance_general_payment.GeneralPaymentDetails.Select(i => i.OtherTaxCOALevel04Id).Where(id => id.HasValue).Select(id => id.Value).ToList();
            var general_ledger_ids = finance_general_payment.GeneralPaymentDetails.Select(i => i.GeneralLedgerId).Where(id => id.HasValue).Select(id => id.Value).ToList();

            var coa_level04s = COALevel04_Repo.GetAll(this, i => coa_level04_ids.Contains(i.Id)).Future();
            var tax_coa_level04s = COALevel04_Repo.GetAll(this, i => tax_coa_level04_ids.Contains(i.Id)).Future();
            var other_tax_coa_level04s = COALevel04_Repo.GetAll(this, i => other_tax_coa_level04_ids.Contains(i.Id)).Future();
            var general_ledgers = GeneralLedger_Repo.GetAll(this, i => general_ledger_ids.Contains(i.Id)).Future();

            var dict_coa_level04s = coa_level04s.ToDictionary(i => i.Id, i => i.Name);
            var dict_tax_coa_level04s = tax_coa_level04s.ToDictionary(i => i.Id, i => i.Name);
            var dict_other_tax_coa_level04s = other_tax_coa_level04s.ToDictionary(i => i.Id, i => i.Name);
            var dict_general_ledgers = general_ledgers.ToDictionary(i => i.Id);

            output.GeneralPaymentDetails = new();
            foreach (var detail in finance_general_payment.GeneralPaymentDetails)
            {
                var mapped_detail = ObjectMapper.Map<GeneralPaymentDetailsGetForEditDto>(detail);
                mapped_detail.COALevel04Name = dict_coa_level04s.GetValueOrDefault(detail.COALevel04Id);
                mapped_detail.TaxCOALevel04Name = detail.TaxCOALevel04Id.HasValue ? dict_tax_coa_level04s.GetValueOrDefault(detail.TaxCOALevel04Id.Value) : null;
                mapped_detail.OtherTaxCOALevel04Name = detail.OtherTaxCOALevel04Id.HasValue ? dict_other_tax_coa_level04s.GetValueOrDefault(detail.OtherTaxCOALevel04Id.Value) : null;
                mapped_detail.GeneralLedgerName = detail.GeneralLedgerId.HasValue ? dict_general_ledgers.GetValueOrDefault(detail.GeneralLedgerId.Value)?.ToString() : null;
                mapped_detail.InvoiceNumber = detail.InvoiceNumber;
                output.GeneralPaymentDetails.Add(mapped_detail);
            }

            return output;
        }

        public async Task<string> Update(FINANCE_GeneralPaymentDto input)
        {
            var old_finance_generalpayment = await Get(input.Id);
            if (old_finance_generalpayment.Status != "PENDING")
                throw new UserFriendlyException($"Only entries with a status of 'PENDING' can be edited. The current status is '" + old_finance_generalpayment.Status + "'.");

            var bank_coa_level04 = COALevel04_Repo.GetAll(this, i => i.Id == input.BankCOALevel04Id).DeferredFirstOrDefault().FutureValue();
            await bank_coa_level04.ValueAsync();

            if (bank_coa_level04.Value == null)
                throw new UserFriendlyException($"BankCOALevel04Id: '{input.BankCOALevel04Id}' is invalid.");

            var entity = ObjectMapper.Map(input, old_finance_generalpayment);
            var voucher_prefix = GetVoucherPrefix(input.LinkedDocument);
            entity.VoucherNumber = await GetVoucherNumberWithDocument(voucher_prefix, input.IssueDate, input.LinkedDocument, 0);
            entity.ReferenceNumber = input.ReferenceNumber;
            entity.ReferenceDate = input.ReferenceDate;
            entity.MaturityDate = input.MaturityDate;
            entity.IsCheque = input.IsCheque;
            entity.IsCrossedCheque = input.IsCrossedCheque;
            entity.ChequeTitle = input.ChequeTitle;
            entity.ChequeNumber = input.ChequeNumber;
            entity.LinkedDocument = input.LinkedDocument;
            entity.TotalAmount = (input.GeneralPaymentDetails ?? new List<GeneralPaymentDetailsDto>())
                .Sum(d => d.NetAmount ?? d.GrossAmount);

            var c_oa_level04_ids = input.GeneralPaymentDetails.Select(i => i.COALevel04Id).ToList();
            var tax_coa_level04_ids = input.GeneralPaymentDetails.Select(i => (long?)i.TaxCOALevel04Id).Where(id => id.HasValue).Select(id => id.Value).ToList();
            var other_tax_coa_level04_ids = input.GeneralPaymentDetails.Select(i => (long?)i.OtherTaxCOALevel04Id).Where(id => id.HasValue).Select(id => id.Value).ToList();
            var general_ledger_ids = input.GeneralPaymentDetails.Select(i => (long?)i.GeneralLedgerId).Where(id => id.HasValue).Select(id => id.Value).ToList();

            var c_oa_level04s = COALevel04_Repo.GetAll(this, i => c_oa_level04_ids.Contains(i.Id)).Select(i => i.Id).Future();
            var tax_coa_level04s = COALevel04_Repo.GetAll(this, i => tax_coa_level04_ids.Contains(i.Id)).Select(i => i.Id).Future();
            var other_tax_coa_level04s = COALevel04_Repo.GetAll(this, i => other_tax_coa_level04_ids.Contains(i.Id)).Select(i => i.Id).Future();
            var general_ledgers = GeneralLedger_Repo.GetAll(this, i => general_ledger_ids.Contains(i.Id)).Select(i => i.Id).Future();
            _ = await general_ledgers.ToListAsync();

            for (int i = 0; i < entity.GeneralPaymentDetails.Count; i++)
            {
                var detail = entity.GeneralPaymentDetails[i];
                if (!c_oa_level04s.Contains(detail.COALevel04Id))
                    throw new UserFriendlyException($"COALevel04Id: '{detail.COALevel04Id}' is invalid at Row: '{i + 1}'.");
                if (detail.TaxCOALevel04Id.HasValue && !tax_coa_level04s.Contains(detail.TaxCOALevel04Id.Value))
                    throw new UserFriendlyException($"TaxCOALevel04Id: '{detail.TaxCOALevel04Id}' is invalid at Row: '{i + 1}'.");
                if (detail.OtherTaxCOALevel04Id.HasValue && !other_tax_coa_level04s.Contains(detail.OtherTaxCOALevel04Id.Value))
                    throw new UserFriendlyException($"OtherTaxCOALevel04Id: '{detail.OtherTaxCOALevel04Id}' is invalid at Row: '{i + 1}'.");
                detail.VoucherNumber = $"{entity.VoucherNumber}/{i + 1}";
                detail.GeneralPaymentInfoId = entity.Id;
                // Ensure InvoiceNumber is preserved on update
                if (i < (input.GeneralPaymentDetails?.Count ?? 0))
                    detail.InvoiceNumber = input.GeneralPaymentDetails[i].InvoiceNumber;
            }

            await FINANCE_GeneralPayment_Repo.UpdateAsync(entity);
            await CurrentUnitOfWork.SaveChangesAsync();
            return "FINANCE_GeneralPayment Updated Successfully.";
        }

        public async Task<string> Delete(long Id)
        {
            var f_inance_general_payment = await FINANCE_GeneralPayment_Repo.GetAll(this, i => i.Id == Id).FirstOrDefaultAsync();
            if (f_inance_general_payment == null)
                throw new UserFriendlyException($"FINANCE_GeneralPaymentId: '{Id}' is invalid.");
            if (f_inance_general_payment.Status != "PENDING")
                throw new UserFriendlyException($"Unable to delete FINANCE_GeneralPayment: only records with a 'PENDING' status can be deleted.");

            await FINANCE_GeneralPayment_Repo.DeleteAsync(f_inance_general_payment);
            await CurrentUnitOfWork.SaveChangesAsync();
            return "FINANCE_GeneralPayment Deleted Successfully.";
        }

            public async Task<string> GetVoucherNumberWithDocument(string Prefix, DateTime IssueDate, GeneralPaymentLinkedDocument LinkedDocument, int Count = 0)
            {
                using (UnitOfWorkManager.Current.DisableFilter(AbpDataFilters.SoftDelete))
                {
                    var latest_voucher = await MainRepository
                        .GetAll(this, i =>
                            i.IssueDate.Month == IssueDate.Month &&
                            i.IssueDate.Year == IssueDate.Year &&
                            i.LinkedDocument == LinkedDocument)
                        .OrderByDescending(i => i.CreationTime)
                        .FirstOrDefaultAsync();
             

                    var current_voucher_index = (latest_voucher?.VoucherNumber?.GetVoucherIndex() ?? 0) + 1;

                    var voucher_number = $"{Prefix}-{IssueDate.Year}-{IssueDate.Month}-{current_voucher_index}";
                    return voucher_number;
                }
            }

        private GeneralLedgerLinkedDocument MapToLedgerLinkedDocument(GeneralPaymentLinkedDocument paymentDoc)
        {
            switch (paymentDoc)
            {
                case GeneralPaymentLinkedDocument.BANK_PAYMENT:
                    return GeneralLedgerLinkedDocument.GeneralPayment_BankPayment;
                case GeneralPaymentLinkedDocument.CASH_RECEIPT:
                    return GeneralLedgerLinkedDocument.GeneralPayment_CashReceipt;
                case GeneralPaymentLinkedDocument.BANK_RECEIPT:
                    return GeneralLedgerLinkedDocument.GeneralPayment_BankReceipt;
                case GeneralPaymentLinkedDocument.CASH_PAYMENT:
                    return GeneralLedgerLinkedDocument.GeneralPayment_CashPayment;
                default:
                    return 0; // Or throw exception if preferred
            }
        }

        public async override Task<string> ApproveDocument(long id)
        {
            var payment = await FINANCE_GeneralPayment_Repo.GetAsync(id);
            await FINANCE_GeneralPayment_Repo.EnsureCollectionLoadedAsync(payment, p => p.GeneralPaymentDetails);

            var ledgerLinkedDoc = MapToLedgerLinkedDocument(payment.LinkedDocument);
            var generalLedgerEntries = new List<GeneralLedgerInfo>();

            foreach (var detail in payment.GeneralPaymentDetails)
            {
                generalLedgerEntries.Add(new GeneralLedgerInfo
                {
                    IssueDate = payment.IssueDate,
                    VoucherNumber = payment.VoucherNumber,
                    ChartOfAccountId = detail.COALevel04Id,
                    Debit = detail.NetAmount,
                    Credit = 0,
                    Status = "PENDING",
                    LinkedDocumentId = payment.Id,
                    LinkedDocument = ledgerLinkedDoc,
                    ReferenceDocumentId = 0,
                    ReferenceVoucherNumber = detail.InvoiceNumber,
                    Remarks = detail.Remarks,
                    TenantId = AbpSession.TenantId
                });

                generalLedgerEntries.Add(new GeneralLedgerInfo
                {
                    IssueDate = payment.IssueDate,
                    VoucherNumber = payment.VoucherNumber,
                    ChartOfAccountId = payment.BankCOALevel04Id,
                    Debit = 0,
                    Credit = detail.NetAmount,
                    Status = "PENDING",
                    LinkedDocumentId = payment.Id,
                    LinkedDocument = ledgerLinkedDoc,
                    ReferenceDocumentId = 0,
                    ReferenceVoucherNumber = detail.InvoiceNumber,
                    Remarks =  detail.Remarks,
                    TenantId = AbpSession.TenantId
                });
            }

            foreach (var entry in generalLedgerEntries)
            {
                await GeneralLedger_Repo.InsertAsync(entry);
            }

            return await base.ApproveDocument(id);
        }

        private static string GetVoucherPrefix(GeneralPaymentLinkedDocument linked_document)
        {
            return linked_document switch
            {
                GeneralPaymentLinkedDocument.CASH_PAYMENT => "CP",
                GeneralPaymentLinkedDocument.BANK_PAYMENT => "BP",
                GeneralPaymentLinkedDocument.CASH_RECEIPT => "CR",
                GeneralPaymentLinkedDocument.BANK_RECEIPT => "BR",
                _ => string.Empty
            };
        }
    }
}
