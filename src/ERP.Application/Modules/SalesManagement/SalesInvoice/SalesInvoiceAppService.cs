using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.UI;
using ERP.Authorization;
using ERP.Authorization.Users;
using ERP.Enums;
using ERP.Extensions.Common;
using ERP.Modules.Finance.ChartOfAccount.COALevel04;
using ERP.Modules.Finance.GeneralLedger;
using ERP.Modules.Finance.LookUps;
using ERP.Modules.HumanResource.CommisionPolicy;
using ERP.Modules.HumanResource.EmployeeManagement;
using ERP.Modules.InventoryManagement.Item;
using ERP.Modules.InventoryManagement.LookUps;
using ERP.Modules.InventoryManagement.StockLedger;
using ERP.Modules.SalesManagement.SalesInvoice.Dtos;
using ERP.Modules.SalesManagement.SalesOrder;
using ERP.Modules.SalesManagement.SalesReturn;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace ERP.Modules.SalesManagement.SalesInvoice
{

    [AbpAuthorize(PermissionNames.LookUps_SalesInvoice)]    
    public class SalesInvoiceAppService : ERPDocumentService<SalesInvoiceInfo>
    {
        public IRepository<SalesInvoiceInfo, long> SalesInvoice_Repo { get; set; }
        public IRepository<PaymentModeInfo, long> PaymentMode_Repo { get; set; }
        public IRepository<COALevel04Info, long> COALevel04Repo { get; set; }
        public IRepository<ItemInfo, long> Item_Repo { get; set; }
        public IRepository<UnitInfo, long> Unit_Repo { get; set; }
        public IRepository<CommissionPolicyInfo, long> CommissionPolicy_Repo { get; set; }
        public IRepository<SalesOrderInfo, long> SalesOrder_Repo { get; set; }
        public IRepository<WarehouseInfo, long> Warehouse_Repo { get; set; }
        public IRepository<User, long> User_Repository { get; set; }
        public IRepository<EmployeeInfo, long> Employee_Repo { get; set; }
        public IRepository<GeneralLedgerInfo, long> GeneralLedger_Repo { get; set; }
        public IRepository<WarehouseStockLedgerInfo, long> WarehouseStockLedger_Repo { get; set; }
        public IRepository<DefaultIntegrationsInfo, long> FINANCE_DefaultIntegrations_Repo { get; set; }
        public IRepository<SalesReturnInfo, long> SalesReturn_Repo { get; set; }

        private readonly IWebHostEnvironment _webHostEnvironment;
        public SalesInvoiceAppService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment    ;
        }

        public async Task<PagedResultDto<SalesInvoiceGetAllDto>> GetAll(SalesInvoiceFiltersDto filters)
        {
            var sales_invoice_query = SalesInvoice_Repo.GetAll(this).ApplyDocumentFilters(filters);
            if (!string.IsNullOrWhiteSpace(filters.PaymentModeId))
                sales_invoice_query = sales_invoice_query.Where(i => i.PaymentModeId == filters.PaymentModeId.TryToLong());
            if (!string.IsNullOrWhiteSpace(filters.CustomerCOALevel04Id))
                sales_invoice_query = sales_invoice_query.Where(i => i.CustomerCOALevel04Id == filters.CustomerCOALevel04Id.TryToLong());
            if (!string.IsNullOrWhiteSpace(filters.AdvanceAmountBankCOALevl04Id))
                sales_invoice_query = sales_invoice_query.Where(i => i.AdvanceAmountBankCOALevl04Id == filters.AdvanceAmountBankCOALevl04Id.TryToLong());
            if (!string.IsNullOrWhiteSpace(filters.TaxCOALevel04Id))
                sales_invoice_query = sales_invoice_query.Where(i => i.TaxCOALevel04Id == filters.TaxCOALevel04Id.TryToLong());
            if (!string.IsNullOrWhiteSpace(filters.WarehouseId))
            {
                var warehouseId = filters.WarehouseId.TryToLong();
                sales_invoice_query = sales_invoice_query.Where(i => i.SalesInvoiceDetails.Any(d => d.WarehouseId == warehouseId));
            }
            var sales_invoices = await sales_invoice_query.ToPagedListAsync(filters);

            var payment_mode_ids = sales_invoices.Select(i => i.PaymentModeId).ToList();
            var coa_level04_ids = sales_invoices.SelectMany(i => new[] { i.CustomerCOALevel04Id, i.AdvanceAmountBankCOALevl04Id, i.TaxCOALevel04Id }).Distinct().ToList();
            var warehouse_ids = sales_invoices.SelectMany(i => i.SalesInvoiceDetails?.Select(d => d.WarehouseId) ?? Enumerable.Empty<long>()).Distinct().ToList();
            var employee_ids = sales_invoices.Select(i => i.EmployeeId).Distinct().ToList();

            var total_count = sales_invoice_query.DeferredCount().FutureValue();
            var payment_modes = PaymentMode_Repo.GetAll(this, i => payment_mode_ids.Contains(i.Id)).Select(i => new { i.Id, i.Name }).Future();
            var coa_level04s = COALevel04Repo.GetAll(this, i => coa_level04_ids.Contains(i.Id)).Select(i => new { i.Id, i.Name }).Future();
            var warehouses = Warehouse_Repo.GetAll(this, i => warehouse_ids.Contains(i.Id)).Select(i => new { i.Id, i.Name }).Future();
            var employees = Employee_Repo.GetAll(this, i => employee_ids.Contains(i.Id)).Select(i => new { i.Id, i.Name }).Future();
            await warehouses.ToListAsync();

            var dict_payment_modes = payment_modes.ToDictionary(i => i.Id);
            var dict_coa_level04s = coa_level04s.ToDictionary(i => i.Id);
            var dict_warehouses = warehouses.ToDictionary(i => i.Id);
            var dict_employees = employees.ToDictionary(i => i.Id);

            var output = new List<SalesInvoiceGetAllDto>();
            foreach (var sales_invoice in sales_invoices)
            {
                dict_payment_modes.TryGetValue(sales_invoice.PaymentModeId, out var payment_mode);
                dict_coa_level04s.TryGetValue(sales_invoice.CustomerCOALevel04Id, out var customer_coa_level04);
                dict_coa_level04s.TryGetValue(sales_invoice.AdvanceAmountBankCOALevl04Id??0, out var advance_bank_coa_level04);
                dict_coa_level04s.TryGetValue(sales_invoice.TaxCOALevel04Id, out var tax_coa_level04);
                dict_employees.TryGetValue(sales_invoice.EmployeeId, out var employee);

                var dto = ObjectMapper.Map<SalesInvoiceGetAllDto>(sales_invoice);
                dto.PaymentModeName = payment_mode?.Name ?? "";
                dto.CustomerCOALevel04Name = customer_coa_level04?.Name ?? "";
                dto.AdvanceAmountBankCOALevl04Name = advance_bank_coa_level04?.Name ?? "";
                dto.TaxCOALevel04Name = tax_coa_level04?.Name ?? "";
                dto.EmployeeName = employee?.Name ?? "";
                dto.CommissionAmount = sales_invoice.CommissionAmount;
                output.Add(dto);
            }

            return new PagedResultDto<SalesInvoiceGetAllDto>(total_count.Value, output);
        }

        [AbpAuthorize(PermissionNames.LookUps_SalesInvoice)]
        public async Task<PagedResultDto<SalesInvoicePendingPaymentDto>> GetPendingPayments(SalesInvoiceFiltersDto filters)
        {
            var sales_invoice_query = SalesInvoice_Repo.GetAll(this).ApplyDocumentFilters(filters);
            if (!string.IsNullOrWhiteSpace(filters.CustomerCOALevel04Id))
                sales_invoice_query = sales_invoice_query.Where(i => i.CustomerCOALevel04Id == filters.CustomerCOALevel04Id.TryToLong());

            sales_invoice_query = sales_invoice_query.Where(i => i.Status == "APPROVED");

            var sales_invoices = await sales_invoice_query.ToListAsync();

            var voucher_numbers = sales_invoices
                .Select(i => i.VoucherNumber)
                .Where(v => !string.IsNullOrWhiteSpace(v))
                .Distinct()
                .ToList();

            var receipt_docs = await GeneralLedger_Repo
                .Where(this, i =>
                    (i.LinkedDocument == GeneralLedgerLinkedDocument.GeneralPayment_BankReceipt ||
                     i.LinkedDocument == GeneralLedgerLinkedDocument.GeneralPayment_CashReceipt) &&
                    voucher_numbers.Contains(i.ReferenceVoucherNumber))
                .ToListAsync();

            var received_by_ref = receipt_docs
                .GroupBy(i => i.ReferenceVoucherNumber)
                .ToDictionary(g => g.Key, g => g.Sum(x => x.Debit ?? 0));

            var invoice_ids = sales_invoices.Select(i => i.Id).ToList();
            var customer_ids = sales_invoices.Select(i => i.CustomerCOALevel04Id).Distinct().ToList();
            var customers = COALevel04Repo.GetAll(this, i => customer_ids.Contains(i.Id)).Select(i => new { i.Id, i.Name }).ToList();
            var customer_name_by_id = customers.ToDictionary(i => i.Id, i => i.Name);

            // Sum approved sales returns linked to each invoice
            var approved_returns = await SalesReturn_Repo
                .GetAllIncluding(r => r.SalesReturnDetails)
                .Where(r => r.TenantId == AbpSession.TenantId && r.Status == "APPROVED")
                .ToListAsync();

            var returns_by_invoice = new Dictionary<long, decimal>();
            foreach (var ret in approved_returns)
            {
                var ret_total = ret.SalesReturnDetails?.Sum(d => d.GrandTotal) ?? 0;
                foreach (var invId in ret.SalesInvoiceIds ?? new List<long>())
                {
                    if (!invoice_ids.Contains(invId)) continue;
                    returns_by_invoice[invId] = returns_by_invoice.GetValueOrDefault(invId) + ret_total;
                }
            }

            var pending_list = new List<SalesInvoicePendingPaymentDto>();
            foreach (var inv in sales_invoices)
            {
                var advance = inv.AdvanceAmount;
                var returns = returns_by_invoice.GetValueOrDefault(inv.Id);
                var received = string.IsNullOrWhiteSpace(inv.VoucherNumber) ? 0 : received_by_ref.GetValueOrDefault(inv.VoucherNumber);
                var pending = inv.GrandTotal - returns - (advance) - received;
                if (pending > 0)
                {
                    pending_list.Add(new SalesInvoicePendingPaymentDto
                    {
                        Id = inv.Id,
                        IssueDate = inv.IssueDate,
                        VoucherNumber = inv.VoucherNumber,
                        ReferenceNumber = inv.ReferenceNumber,
                        CustomerCOALevel04Id = inv.CustomerCOALevel04Id,
                        CustomerName = customer_name_by_id.GetValueOrDefault(inv.CustomerCOALevel04Id, string.Empty),
                        GrandTotal = inv.GrandTotal,
                        TaxAmount = inv.TaxAmount,
                        AdvanceAmount = advance,
                        PaidAmount = received,
                        PendingAmount = pending,
                        Status = inv.Status
                    });
                }
            }

            var total_count = pending_list.Count;
            var paged = pending_list
                .OrderByDescending(i => i.IssueDate)
                .Skip(filters.SkipCount)
                .Take(filters.MaxResultCount)
                .ToList();

            return new PagedResultDto<SalesInvoicePendingPaymentDto>(total_count, paged);
        }

        [AbpAuthorize(PermissionNames.LookUps_SalesInvoice_Create)]
        public async Task<SalesInvoiceCreateResultDto> Create(SalesInvoiceDto input)
        {
            var payment_mode = PaymentMode_Repo.GetAll(this, i => i.Id == input.PaymentModeId).DeferredFirstOrDefault().FutureValue();
            await payment_mode.ValueAsync();

            if (payment_mode.Value == null)
                throw new UserFriendlyException($"PaymentModeId: '{input.PaymentModeId}' is invalid.");

            var entity = ObjectMapper.Map<SalesInvoiceInfo>(input);
            entity.Status = "PENDING";
            entity.VoucherNumber = await GetVoucherNumber("SI", input.IssueDate);

            var item_ids = input.SalesInvoiceDetails.Select(i => i.ItemId).ToList();
            var unit_ids = input.SalesInvoiceDetails.Select(i => i.UnitId).ToList();
            var warehouse_ids = input.SalesInvoiceDetails.Select(i => i.WarehouseId).ToList();

            var items = Item_Repo.GetAll(this, i => item_ids.Contains(i.Id)).Select(i => i.Id).Future();
            var units = Unit_Repo.GetAll(this, i => unit_ids.Contains(i.Id)).Select(i => i.Id).Future();
            var warehouses = Warehouse_Repo.GetAll(this, i => warehouse_ids.Contains(i.Id)).Select(i => i.Id).Future();
            _ = await warehouses.ToListAsync();

            for (int i = 0; i < entity.SalesInvoiceDetails.Count; i++)
            {
                var detail = entity.SalesInvoiceDetails[i];

                if (!items.Contains(detail.ItemId))
                    throw new UserFriendlyException($"ItemId: '{detail.ItemId}' is invalid at Row: '{i + 1}'.");
                if (!units.Contains(detail.UnitId))
                    throw new UserFriendlyException($"UnitId: '{detail.UnitId}' is invalid at Row: '{i + 1}'.");
                if (!warehouses.Contains(detail.WarehouseId))
                    throw new UserFriendlyException($"WarehouseId: '{detail.WarehouseId}' is invalid at Row: '{i + 1}'.");
            }

            entity.TenantId = AbpSession.TenantId;
            await SalesInvoice_Repo.InsertAsync(entity);
            await CurrentUnitOfWork.SaveChangesAsync();
            return new SalesInvoiceCreateResultDto
            {
                Id = entity.Id,
                Message = "SalesInvoice Created Successfully."
            };
        }

        private async Task<SalesInvoiceInfo> Get(long Id)
        {
            var sales_invoice = await SalesInvoice_Repo.GetAllIncluding(i => i.SalesInvoiceDetails).Where(i => i.Id == Id).FirstOrDefaultAsync();
            if (sales_invoice != null)
                return sales_invoice;
            else
                throw new UserFriendlyException($"SalesInvoiceId: '{Id}' is invalid.");
        }

        public async Task<SalesInvoiceGetForEditDto> GetForEdit(long Id)
        {
            var sales_invoice = await Get(Id);
            var payment_mode = PaymentMode_Repo.GetAll(this, i => i.Id == sales_invoice.PaymentModeId).DeferredFirstOrDefault().FutureValue();
            var customer_coa_level04 = COALevel04Repo.GetAll(this, i => i.Id == sales_invoice.CustomerCOALevel04Id).DeferredFirstOrDefault().FutureValue();
            var advance_bank_coa_level04 = COALevel04Repo.GetAll(this, i => i.Id == sales_invoice.AdvanceAmountBankCOALevl04Id).DeferredFirstOrDefault().FutureValue();
            var tax_coa_level04 = COALevel04Repo.GetAll(this, i => i.Id == sales_invoice.TaxCOALevel04Id).DeferredFirstOrDefault().FutureValue();
            var employee = Employee_Repo.GetAll(this, i => i.Id == sales_invoice.EmployeeId).DeferredFirstOrDefault().FutureValue();
            await payment_mode.ValueAsync();

            var output = ObjectMapper.Map<SalesInvoiceGetForEditDto>(sales_invoice);
            output.PaymentModeName = payment_mode?.Value?.Name ?? "";
            output.CustomerCOALevel04Name = customer_coa_level04?.Value?.Name ?? "";
            output.AdvanceAmountBankCOALevl04Name = advance_bank_coa_level04?.Value?.Name ?? "";
            output.TaxCOALevel04Name = tax_coa_level04?.Value?.Name ?? "";
            output.EmployeeName = employee?.Value?.Name ?? "";
            output.CommissionAmount = sales_invoice.CommissionAmount;

            var item_ids = sales_invoice.SalesInvoiceDetails.Select(i => i.ItemId).ToList();
            var unit_ids = sales_invoice.SalesInvoiceDetails.Select(i => i.UnitId).ToList();
            var warehouse_ids = sales_invoice.SalesInvoiceDetails.Select(i => i.WarehouseId).ToList();

            var items = Item_Repo.GetAll(this, i => item_ids.Contains(i.Id)).Select(i => new { i.Id, i.Name }).Future();
            var units = Unit_Repo.GetAll(this, i => unit_ids.Contains(i.Id)).Select(i => new { i.Id, i.Name }).Future();
            var warehouses = Warehouse_Repo.GetAll(this, i => warehouse_ids.Contains(i.Id)).Select(i => new { i.Id, i.Name }).Future();
            _ = await warehouses.ToListAsync();

            var dict_items = items.ToDictionary(i => i.Id, i => i.Name);
            var dict_units = units.ToDictionary(i => i.Id, i => i.Name);
            var dict_warehouses = warehouses.ToDictionary(i => i.Id, i => i.Name);

            output.SalesInvoiceDetails = new();
            foreach (var detail in sales_invoice.SalesInvoiceDetails)
            {
                var mapped_detail = ObjectMapper.Map<SalesInvoiceDetailsGetForEditDto>(detail);
                mapped_detail.ItemName = dict_items.GetValueOrDefault(detail.ItemId);
                mapped_detail.UnitName = dict_units.GetValueOrDefault(detail.UnitId);
                output.SalesInvoiceDetails.Add(mapped_detail);
            }

            return output;
        }

        [AbpAuthorize(PermissionNames.LookUps_SalesInvoice_Update)]
        public async Task<string> Update(SalesInvoiceDto input)
        {
            var old_sales_invoice = await Get(input.Id);
            if (old_sales_invoice.Status != "PENDING")
                throw new UserFriendlyException($"Only entries with a status of 'PENDING' can be edited. The current status is '{old_sales_invoice.Status}'.");

            var payment_mode = PaymentMode_Repo.GetAll(this, i => i.Id == input.PaymentModeId).DeferredFirstOrDefault().FutureValue();
            await payment_mode.ValueAsync();

            if (payment_mode.Value == null)
                throw new UserFriendlyException($"PaymentModeId: '{input.PaymentModeId}' is invalid.");

            var entity = ObjectMapper.Map(input, old_sales_invoice);
            entity.VoucherNumber = await GetVoucherNumber("SI", input.IssueDate, entity.VoucherNumber.GetVoucherIndex());

            var item_ids = input.SalesInvoiceDetails.Select(i => i.ItemId).ToList();
            var unit_ids = input.SalesInvoiceDetails.Select(i => i.UnitId).ToList();
            var warehouse_ids = input.SalesInvoiceDetails.Select(i => i.WarehouseId).ToList();

            var items = Item_Repo.GetAll(this, i => item_ids.Contains(i.Id)).Select(i => i.Id).Future();
            var units = Unit_Repo.GetAll(this, i => unit_ids.Contains(i.Id)).Select(i => i.Id).Future();
            var warehouses = Warehouse_Repo.GetAll(this, i => warehouse_ids.Contains(i.Id)).Select(i => i.Id).Future();
            _ = await warehouses.ToListAsync();

            for (int i = 0; i < entity.SalesInvoiceDetails.Count; i++)
            {
                var detail = entity.SalesInvoiceDetails[i];

                if (!items.Contains(detail.ItemId))
                    throw new UserFriendlyException($"ItemId: '{detail.ItemId}' is invalid at Row: '{i + 1}'.");
                if (!units.Contains(detail.UnitId))
                    throw new UserFriendlyException($"UnitId: '{detail.UnitId}' is invalid at Row: '{i + 1}'.");
                if (!warehouses.Contains(detail.WarehouseId))
                    throw new UserFriendlyException($"WarehouseId: '{detail.WarehouseId}' is invalid at Row: '{i + 1}'.");
                
                // Update RemainingQty to match InvoiceQty for pending sales invoice details
                detail.RemainingQty = detail.InvoiceQty;
            }

            await SalesInvoice_Repo.UpdateAsync(entity);
            await CurrentUnitOfWork.SaveChangesAsync();
            return "SalesInvoice Updated Successfully.";
        }

        [AbpAuthorize(PermissionNames.LookUps_SalesInvoice_Delete)]
        public async Task<string> Delete(long Id)
        {
            var sales_invoice = await SalesInvoice_Repo.GetAll(this, i => i.Id == Id).FirstOrDefaultAsync();
            if (sales_invoice == null)
                throw new UserFriendlyException($"SalesInvoiceId: '{Id}' is invalid.");
            if (sales_invoice.Status != "PENDING")
                throw new UserFriendlyException($"Unable to delete SalesInvoice: only records with a 'PENDING' status can be deleted.");

            await SalesInvoice_Repo.DeleteAsync(sales_invoice);
            await CurrentUnitOfWork.SaveChangesAsync();
            return "SalesInvoice Deleted Successfully.";
        }

        public async override Task<string> ApproveDocument(long Id)
        {
            var document = await GetForEdit(Id);

            var item_ids = document.SalesInvoiceDetails.Select(i => i.ItemId).ToList();
            var items = Item_Repo.GetAll(this, i => item_ids.Contains(i.Id)).Future();
            var customer = COALevel04Repo.GetAll(this, i => i.Id == document.CustomerCOALevel04Id).DeferredFirstOrDefault().FutureValue();
            _ = await customer.ValueAsync();

            var sales_coa_id = await FINANCE_DefaultIntegrations_Repo
                .GetAll(this, i => i.Name == "SalesCOA")
                .Select(i => i.ChartOfAccountId)
                .FirstOrDefaultAsync();
            if (sales_coa_id == 0)
                throw new UserFriendlyException("Default integration 'SalesCOA' is not configured.");

            var sales_invoice = await Get(Id);
            if (sales_invoice.ReferenceNumber != null)
            {
                var sales_order = await SalesOrder_Repo.GetAll(this, i => i.VoucherNumber == sales_invoice.ReferenceNumber).FirstOrDefaultAsync();
                if (sales_order != null)
                {
                    sales_invoice.EmployeeId = sales_order.CreatorUserId.Value;
                }
            }
            var user = await Employee_Repo.FirstOrDefaultAsync(i => i.Id == sales_invoice.EmployeeId) ?? new();

            if (user.CommissionPolicyId != 0)
            {
                var commissionPolicy = await CommissionPolicy_Repo.GetAllIncluding(i => i.CommissionPolicyDetails).FirstOrDefaultAsync(i => i.Id == user.CommissionPolicyId);
                if (commissionPolicy != null)
                {
                    if (commissionPolicy.PolicyType == Enums.PolicyType.Bill)
                    {
                        sales_invoice.CommissionAmount = (decimal)commissionPolicy.CommisionAmount;
                    }
                    else if (commissionPolicy.PolicyType == Enums.PolicyType.Percentage)
                    {
                        decimal invoiceAmount = sales_invoice.GrandTotal;
                        sales_invoice.CommissionAmount = invoiceAmount * ((commissionPolicy.CommisionPercentage ?? 0) / 100);
                    }
                    else if (commissionPolicy.PolicyType == Enums.PolicyType.Slabe)
                    {
                        decimal invoiceAmount = sales_invoice.GrandTotal;
                        var matchingSlab = commissionPolicy.CommissionPolicyDetails
                            .Where(s => invoiceAmount >= s.FromAmount && invoiceAmount <= s.ToAmount)
                            .OrderByDescending(s => s.FromAmount)
                            .FirstOrDefault();

                        if (matchingSlab != null)
                        {
                            sales_invoice.CommissionAmount = matchingSlab.SalesCommisionAmount;
                        }
                        else
                        {
                            sales_invoice.CommissionAmount = 0;
                        }
                    }
                }
            }

            var transaction_base = new GeneralLedgerTransactionBaseDto()
            {
                IssueDate = document.IssueDate,
                VoucherNumber = document.VoucherNumber,
                IsAdjustmentEntry = false,
                Remarks = document.Remarks,
                LinkedDocument = GeneralLedgerLinkedDocument.SalesInvoice,
                LinkedDocumentId = document.Id,
                TenantId = AbpSession.TenantId,
            };

            foreach (var detail in document.SalesInvoiceDetails)
            {
                var item_coa = sales_coa_id;

                await WarehouseStockLedger_Repo.AddLedgerTransactionAsync(
                    document.IssueDate,
                    document.VoucherNumber,
                    detail.ItemId,
                    detail.WarehouseId,
                    0,
                    detail.InvoiceQty,
                    detail.Rate,
                    detail.InvoiceQty,
                    detail.GrandTotal,
                    document.CustomerCOALevel04Id,
                    document.Remarks,
                    document.Id,
                    Enums.WarehouseStockLedgerLinkedDocument.SalesInvoice,
                    AbpSession.TenantId);

                await GeneralLedger_Repo.AddLedgerTransactionAsync(transaction_base, detail.GrandTotal, 0, item_coa, 0);
            }
            await GeneralLedger_Repo.AddLedgerTransactionAsync(transaction_base, 0, document.GrandTotal, document.CustomerCOALevel04Id, 0);
            await GeneralLedger_Repo.AddLedgerTransactionAsync(transaction_base, 0, document.AdvanceAmount ?? 0, document.AdvanceAmountBankCOALevl04Id ?? 0, 0);
            await GeneralLedger_Repo.AddLedgerTransactionAsync(transaction_base, document.AdvanceAmount??0, 0, document.CustomerCOALevel04Id, 0);
           
            CurrentUnitOfWork.SaveChanges();
            return await base.ApproveDocument(Id);
        }

        [AbpAuthorize(PermissionNames.LookUps_SalesInvoice_ApproveDocument)]
        public async Task<string> UnapproveDocument(long Id)
        {
            var sales_invoice = await SalesInvoice_Repo.GetAll(this, i => i.Id == Id).FirstOrDefaultAsync();
            if (sales_invoice == null)
                throw new UserFriendlyException($"Could not find SalesInvoice with ID: '{Id}'.");
            if (sales_invoice.Status != "APPROVED")
                throw new UserFriendlyException($"Before Unapproving Document; Status must be 'APPROVED'");

            var general_ledger_entries = await GeneralLedger_Repo
                .Where(this, i => i.LinkedDocument == GeneralLedgerLinkedDocument.SalesInvoice && i.LinkedDocumentId == Id)
                .ToListAsync();
            if (general_ledger_entries.Count > 0)
                await GeneralLedger_Repo.DeleteRangeAsync(general_ledger_entries);

            var warehouse_stock_ledger_entries = await WarehouseStockLedger_Repo
                .Where(this, i => i.DocumentId == Id && i.WarehouseStockLedgerLinkedDocument == WarehouseStockLedgerLinkedDocument.SalesInvoice)
                .ToListAsync();
            if (warehouse_stock_ledger_entries.Count > 0)
                await WarehouseStockLedger_Repo.DeleteRangeAsync(warehouse_stock_ledger_entries);

            sales_invoice.Status = "PENDING";
            await SalesInvoice_Repo.UpdateAsync(sales_invoice);
            await CurrentUnitOfWork.SaveChangesAsync();

            return "SalesInvoice Status 'PENDING' Successfully.";
        }

        public async Task<DocumentUploadResultDto> UploadDocuments(DocumentUploadDto input)
        {
            if (input.Base64Images == null || input.Base64Images.Count == 0)
                throw new UserFriendlyException("No images were provided for upload.");

            string documentDirectory = "sales-invoices";
            string uploadDirectory = Path.Combine(_webHostEnvironment.WebRootPath, documentDirectory);

            if (!Directory.Exists(uploadDirectory))
                Directory.CreateDirectory(uploadDirectory);

            var result = new DocumentUploadResultDto
            {
                ImagePaths = new List<string>()
            };

            foreach (var base64Image in input.Base64Images)
            {
                string base64Data = base64Image;
                string contentType = "image/jpeg";
                if (base64Image.Contains(";base64,"))
                {
                    var parts = base64Image.Split(";base64,");
                    if (parts.Length == 2)
                    {
                        contentType = parts[0].Replace("data:", "");
                        base64Data = parts[1];
                    }
                }
                byte[] imageBytes = Convert.FromBase64String(base64Data);
                string extension = ".jpg";
                if (contentType.Contains("png"))
                    extension = ".png";
                else if (contentType.Contains("gif"))
                    extension = ".gif";
                else if (contentType.Contains("pdf"))
                    extension = ".pdf";
                string fileName = $"{DateTime.Now.Ticks}{extension}";
                string filePath = Path.Combine(uploadDirectory, fileName);
                await File.WriteAllBytesAsync(filePath, imageBytes);
                string relativePath = $"/{documentDirectory}/{fileName}";
                result.ImagePaths.Add(relativePath);
            }
            result.Message = $"Successfully uploaded {result.ImagePaths.Count} document(s)";
            await CurrentUnitOfWork.SaveChangesAsync();
            return result;
        }
        public async Task<CommissionPolicyInfoDto> GetCommissionPolicyByUserId(long userId)
        {
            var employee = await Employee_Repo.GetAll()
                .Where(e => e.UserId == userId)
                .FirstOrDefaultAsync();

            if (employee == null)
                throw new UserFriendlyException($"No employee found for UserId: '{userId}'.");

            if (employee.CommissionPolicyId == 0)
                throw new UserFriendlyException($"No commission policy assigned to the employee.");

            var commissionPolicy = await CommissionPolicy_Repo.GetAllIncluding(i => i.CommissionPolicyDetails)
                .Where(cp => cp.Id == employee.CommissionPolicyId)
                .FirstOrDefaultAsync();

            if (commissionPolicy == null)
                throw new UserFriendlyException($"Commission policy with Id: '{employee.CommissionPolicyId}' not found.");

            var dto = new CommissionPolicyInfoDto
            {
                Id = commissionPolicy.Id,
                Name = commissionPolicy.Name,
                PolicyType = commissionPolicy.PolicyType.ToString(),
                CommisionAmount = commissionPolicy.CommisionAmount,
                CommisionPercentage = commissionPolicy.CommisionPercentage,
                EmployeeName = employee.Name,
                EmployeeId = employee.Id,
                CommissionPolicyDetails = commissionPolicy.CommissionPolicyDetails?.Select(d => new CommissionPolicyDetailDto
                {
                    Id = d.Id,
                    FromAmount = d.FromAmount,
                    ToAmount = d.ToAmount,
                    SalesCommisionAmount = d.SalesCommisionAmount
                }).ToList()
            };

            return dto;
        }

        public async Task<decimal> GetLatestRate(long ItemId, long CustomerCOALevel04Id, long UnitId)
        {
            var latest_sales_rate = await SalesInvoice_Repo
                .GetAllIncluding(i => i.SalesInvoiceDetails)
                .Where(i => i.CustomerCOALevel04Id == CustomerCOALevel04Id)
                .OrderByDescending(i => i.IssueDate)
                .ThenByDescending(i => i.CreationTime)
                .SelectMany(i => i.SalesInvoiceDetails)
                .Where(detail => detail.ItemId == ItemId && detail.UnitId == UnitId)
                .Select(i => i.Rate)
                .FirstOrDefaultAsync();

            return latest_sales_rate;
        }
    }
}
