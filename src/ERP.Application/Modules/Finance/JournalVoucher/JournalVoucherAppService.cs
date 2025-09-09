using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.UI;
using ERP.Authorization;
using ERP.Modules.Finance.ChartOfAccount.COALevel04;
using ERP.Modules.Finance.GeneralLedger;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace ERP.Modules.Finance.JournalVoucher
{
    public class JournalVoucherAppService : ERPDocumentService<JournalVoucherInfo>
    {
        public IRepository<JournalVoucherInfo, long> FINANCE_JournalVoucher_Repo { get; set; }
        public IRepository<COALevel04Info, long> COAlvl4_Repo { get; set; }
        public IRepository<GeneralLedgerInfo, long> GeneralLedger_Repo { get; set; }

        public async Task<PagedResultDto<FINANCE_JournalVoucherGetAllDto>> GetAll(FINANCE_JournalVoucherFiltersDto filters)
        {
            var f_inance_journal_voucher_query = FINANCE_JournalVoucher_Repo.GetAllIncluding(i => i.JournalVoucherDetails);
            if (AbpSession.TenantId.HasValue)
                f_inance_journal_voucher_query = f_inance_journal_voucher_query.Where(i => i.TenantId == AbpSession.TenantId);
            if (!string.IsNullOrWhiteSpace(filters.Id))
                f_inance_journal_voucher_query = f_inance_journal_voucher_query.Where(i => i.Id == filters.Id.TryToLong());
            var f_inance_journal_vouchers = await f_inance_journal_voucher_query.ToPagedListAsync(filters);

            var total_count = f_inance_journal_voucher_query.DeferredCount().FutureValue();
            var output = new List<FINANCE_JournalVoucherGetAllDto>();
            foreach (var f_inance_journal_voucher in f_inance_journal_vouchers)
            {
                var dto = ObjectMapper.Map<FINANCE_JournalVoucherGetAllDto>(f_inance_journal_voucher);
                
                if (f_inance_journal_voucher.JournalVoucherDetails != null && f_inance_journal_voucher.JournalVoucherDetails.Any())
                {
                    var firstDetail = f_inance_journal_voucher.JournalVoucherDetails.First();
                }
                
                output.Add(dto);
            }

            return new PagedResultDto<FINANCE_JournalVoucherGetAllDto>(total_count.Value, output);
        }

        public async Task<string> Create(FINANCE_JournalVoucherDto input)
        {
            var entity = ObjectMapper.Map<JournalVoucherInfo>(input);
            entity.Status = "PENDING";
            entity.VoucherNumber = await GetVoucherNumber("JV", input.IssueDate);

            var c_o_alvl4_ids = input.JournalVoucherDetails.Select(i => i.COAlvl4Id).ToList();

            var c_o_alvl4s = COAlvl4_Repo.GetAll(this, i => c_o_alvl4_ids.Contains(i.Id)).Select(i => i.Id).Future();
            _ = await c_o_alvl4s.ToListAsync();

            for (int i = 0; i < entity.JournalVoucherDetails.Count; i++)
            {
                var detail = entity.JournalVoucherDetails[i];

                if (!c_o_alvl4s.Contains(detail.COAlvl4Id))
                    throw new UserFriendlyException($"COAlvl4Id: '{detail.COAlvl4Id}' is invalid at Row: '{i + 1}'.");
            }

            entity.TenantId = AbpSession.TenantId;
            await FINANCE_JournalVoucher_Repo.InsertAsync(entity);
            await CurrentUnitOfWork.SaveChangesAsync();
            return "FINANCE_JournalVoucher Created Successfully.";
        }

        private async Task<JournalVoucherInfo> Get(long Id)
        {
            var f_inance_journal_voucher = await FINANCE_JournalVoucher_Repo.GetAllIncluding(i => i.JournalVoucherDetails).Where(i => i.Id == Id).FirstOrDefaultAsync();
            if (f_inance_journal_voucher != null)
                return f_inance_journal_voucher;
            else
                throw new UserFriendlyException($"FINANCE_JournalVoucherId: '{Id}' is invalid.");
        }

        public async Task<FINANCE_JournalVoucherGetForEditDto> GetForEdit(long Id)
        {
            var f_inance_journal_voucher = await Get(Id);
            var output = ObjectMapper.Map<FINANCE_JournalVoucherGetForEditDto>(f_inance_journal_voucher);
            var c_o_alvl4_ids = f_inance_journal_voucher.JournalVoucherDetails.Select(i => i.COAlvl4Id).ToList();

            var c_o_alvl4s = COAlvl4_Repo.GetAll(this, i => c_o_alvl4_ids.Contains(i.Id)).Future();

            var dict_c_o_alvl4s = c_o_alvl4s.ToDictionary(i => i.Id, i => i.Name);

            output.JournalVoucherDetails = new();
            foreach (var detail in f_inance_journal_voucher.JournalVoucherDetails)
            {
                var mapped_detail = ObjectMapper.Map<JournalVoucherDetailsGetForEditDto>(detail);
                mapped_detail.COAlvl4Name = dict_c_o_alvl4s.GetValueOrDefault(detail.COAlvl4Id);
                output.JournalVoucherDetails.Add(mapped_detail);
            }

            return output;
        }
        [HttpPut]
        public async Task<string> Edit(FINANCE_JournalVoucherDto input)
        {
            var old_finance_journalvoucher = await Get(input.Id);
            if (old_finance_journalvoucher.Status != "PENDING")
                throw new UserFriendlyException($"Only entries with a status of 'PENDING' can be edited. The current status is '" + old_finance_journalvoucher.Status + "'.");

            var entity = ObjectMapper.Map(input, old_finance_journalvoucher);
            entity.VoucherNumber = await GetVoucherNumber("JV", input.IssueDate, entity.VoucherNumber.GetVoucherIndex());

            var c_o_alvl4_ids = input.JournalVoucherDetails.Select(i => i.COAlvl4Id).ToList();

            var c_o_alvl4s = COAlvl4_Repo.GetAll(this, i => c_o_alvl4_ids.Contains(i.Id)).Select(i => i.Id).Future();
            _ = await c_o_alvl4s.ToListAsync();

            for (int i = 0; i < entity.JournalVoucherDetails.Count; i++)
            {
                var detail = entity.JournalVoucherDetails[i];

                if (!c_o_alvl4s.Contains(detail.COAlvl4Id))
                    throw new UserFriendlyException($"COAlvl4Id: '{detail.COAlvl4Id}' is invalid at Row: '{i + 1}'.");
            }

            await FINANCE_JournalVoucher_Repo.UpdateAsync(entity);
            await CurrentUnitOfWork.SaveChangesAsync();
            return "FINANCE_JournalVoucher Updated Successfully.";
        }

        public async Task<string> Delete(long Id)
        {
            var f_inance_journal_voucher = await FINANCE_JournalVoucher_Repo.GetAll(this, i => i.Id == Id).FirstOrDefaultAsync();
            if (f_inance_journal_voucher == null)
                throw new UserFriendlyException($"FINANCE_JournalVoucherId: '{Id}' is invalid.");
            if (f_inance_journal_voucher.Status != "PENDING")
                throw new UserFriendlyException($"Unable to delete FINANCE_JournalVoucher: only records with a 'PENDING' status can be deleted.");

            await FINANCE_JournalVoucher_Repo.DeleteAsync(f_inance_journal_voucher);
            await CurrentUnitOfWork.SaveChangesAsync();
            return "FINANCE_JournalVoucher Deleted Successfully.";
        }

        public async override Task<string> ApproveDocument(long id)
        {
            var voucher = await FINANCE_JournalVoucher_Repo.GetAllIncluding(i => i.JournalVoucherDetails).Where(i => i.Id == id).FirstOrDefaultAsync();
            if (voucher == null)
                throw new UserFriendlyException($"Could not find FINANCE_JournalVoucher with ID: '{id}'.");
            if (voucher.Status != "PENDING")
                throw new UserFriendlyException($"Before Approving Document; Status must be 'PENDING'");

            var ledgerEntries = new List<GeneralLedgerInfo>();
            foreach (var detail in voucher.JournalVoucherDetails)
            {
                if (detail.Debit > 0)
                {
                    ledgerEntries.Add(new GeneralLedgerInfo
                    {
                        IssueDate = voucher.IssueDate,
                        VoucherNumber = voucher.VoucherNumber,
                        ChartOfAccountId = detail.COAlvl4Id,
                        Debit = detail.Debit,
                        Credit = 0,
                        Status = "PENDING",
                        LinkedDocumentId = voucher.Id,
                        LinkedDocument = 0, 
                        ReferenceDocumentId = 0,
                        ReferenceVoucherNumber = voucher.VoucherNumber,
                        Remarks = !string.IsNullOrEmpty(detail.Remarks) ? detail.Remarks : voucher.Remarks,
                        TenantId = AbpSession.TenantId
                    });
                }
                if (detail.Credit > 0)
                {
                    ledgerEntries.Add(new GeneralLedgerInfo
                    {
                        IssueDate = voucher.IssueDate,
                        VoucherNumber = voucher.VoucherNumber,
                        ChartOfAccountId = detail.COAlvl4Id,
                        Debit = 0,
                        Credit = detail.Credit,
                        Status = "PENDING",
                        LinkedDocumentId = voucher.Id,
                        LinkedDocument = 0,
                        ReferenceDocumentId = 0,
                        ReferenceVoucherNumber = voucher.VoucherNumber,
                        Remarks = !string.IsNullOrEmpty(detail.Remarks) ? detail.Remarks : voucher.Remarks,
                        TenantId = AbpSession.TenantId
                    });
                }
            }
            foreach (var entry in ledgerEntries)
            {
                await GeneralLedger_Repo.InsertAsync(entry);
            }
            await CurrentUnitOfWork.SaveChangesAsync();
            return await base.ApproveDocument(id);
        }
    }
}
