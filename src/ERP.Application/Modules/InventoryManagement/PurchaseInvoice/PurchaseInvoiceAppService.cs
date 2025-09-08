using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.UI;
using ERP.Authorization;
using ERP.Enums;
using ERP.Extensions.Common;
using ERP.Modules.Finance.ChartOfAccount.COALevel04;
using ERP.Modules.Finance.GeneralLedger;
using ERP.Modules.Finance.LookUps;
using ERP.Modules.HumanResource.EmployeeManagement;
using ERP.Modules.InventoryManagement.Item;
using ERP.Modules.InventoryManagement.LookUps;
using ERP.Modules.InventoryManagement.PurchaseOrder;
using ERP.Modules.InventoryManagement.PurchaseReturn;
using ERP.Modules.InventoryManagement.StockLedger;
using ERP.Modules.SalesManagement.SalesInvoice.Dtos;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace ERP.Modules.InventoryManagement.PurchaseInvoice
{
    [AbpAuthorize(PermissionNames.LookUps_PurchaseInvoice)]
    public class PurchaseInvoiceAppService : ERPDocumentService<PurchaseInvoiceInfo>
    {
        public IRepository<PurchaseInvoiceInfo, long> PurchaseInvoice_Repo { get; set; }
        public IRepository<PaymentModeInfo, long> PaymentMode_Repo { get; set; }
        public IRepository<COALevel04Info, long> COALevel04_Repo { get; set; }
        public IRepository<WarehouseInfo, long> Warehouse_Repo { get; set; }
        public IRepository<ItemInfo, long> Item_Repo { get; set; }
        public IRepository<UnitInfo, long> Unit_Repo { get; set; }
        public IRepository<PurchaseOrderInfo, long> PurchaseOrder_Repo { get; set; }
        public IRepository<GeneralLedgerInfo, long> GeneralLedger_Repo { get; set; }
        public IRepository<EmployeeInfo, long> Employee_Repo { get; set; }
        public IRepository<WarehouseStockLedgerInfo, long> WarehouseStockLedger_Repo { get; set; }
        public IRepository<DefaultIntegrationsInfo, long> FINANCE_DefaultIntegrations_Repo { get; set; }
        public IRepository<PurchaseReturnInfo, long> PurchaseReturn_Repo { get; set; }


        private readonly IWebHostEnvironment _webHostEnvironment;
        public PurchaseInvoiceAppService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<PagedResultDto<PurchaseInvoiceGetAllDto>> GetAll(PurchaseInvoiceFiltersDto filters)
        {
            var purchase_invoice_query = PurchaseInvoice_Repo.GetAll(this).ApplyDocumentFilters(filters);
            if (!string.IsNullOrWhiteSpace(filters.SupplierCOALevel04Id))
                purchase_invoice_query = purchase_invoice_query.Where(i => i.SupplierCOALevel04Id == filters.SupplierCOALevel04Id.TryToLong());
            if (!string.IsNullOrWhiteSpace(filters.PaymentModeId))
                purchase_invoice_query = purchase_invoice_query.Where(i => i.PaymentModeId == filters.PaymentModeId.TryToLong());
            if (!string.IsNullOrWhiteSpace(filters.TaxCOALevel04Id))
                purchase_invoice_query = purchase_invoice_query.Where(i => i.TaxCOALevel04Id == filters.TaxCOALevel04Id.TryToLong());
            if (!string.IsNullOrWhiteSpace(filters.BrokerCOALevel04Id))
                purchase_invoice_query = purchase_invoice_query.Where(i => i.BrokerCOALevel04Id == filters.BrokerCOALevel04Id.TryToLong());
            if (!string.IsNullOrWhiteSpace(filters.AdvanceAmountBankCOALevl04Id))
                purchase_invoice_query = purchase_invoice_query.Where(i => i.AdvanceAmountBankCOALevl04Id == filters.AdvanceAmountBankCOALevl04Id.TryToLong());
            if (!string.IsNullOrWhiteSpace(filters.WarehouseId))
                purchase_invoice_query = purchase_invoice_query.Where(i => i.WarehouseId == filters.WarehouseId.TryToLong());
            var purchase_invoices = await purchase_invoice_query.ToPagedListAsync(filters);

            var coa_level04_ids = purchase_invoices.SelectMany(i => new[] { i.SupplierCOALevel04Id, i.AdvanceAmountBankCOALevl04Id, i.TaxCOALevel04Id, i.BrokerCOALevel04Id }).ToList();
            var payment_mode_ids = purchase_invoices.Select(i => i.PaymentModeId).ToList();
            var warehouse_ids = purchase_invoices.Select(i => i.WarehouseId).ToList();

            var total_count = purchase_invoice_query.DeferredCount().FutureValue();
            var supplier_coa_level04s = COALevel04_Repo.GetAll(this, i => coa_level04_ids.Contains(i.Id)).Select(i => new { i.Id, i.SerialNumber, i.Name }).Future();
            var payment_modes = PaymentMode_Repo.GetAll(this, i => payment_mode_ids.Contains(i.Id)).Select(i => new { i.Id, i.Name }).Future();
            var warehouses = Warehouse_Repo.GetAll(this, i => warehouse_ids.Contains(i.Id)).Select(i => new { i.Id, i.Name }).Future();
            _ = await warehouses.ToListAsync();

            var dict_coa_level04s = supplier_coa_level04s.ToDictionary(i => i.Id);
            var dict_payment_modes = payment_modes.ToDictionary(i => i.Id);
            var dict_warehouses = warehouses.ToDictionary(i => i.Id);

            var output = new List<PurchaseInvoiceGetAllDto>();
            foreach (var purchase_invoice in purchase_invoices)
            {
                dict_coa_level04s.TryGetValue(purchase_invoice.SupplierCOALevel04Id, out var supplier_coa_level04);
                dict_coa_level04s.TryGetValue(purchase_invoice.AdvanceAmountBankCOALevl04Id ?? 0, out var advance_bank_coa_level04);
                dict_payment_modes.TryGetValue(purchase_invoice.PaymentModeId, out var payment_mode);
                dict_coa_level04s.TryGetValue(purchase_invoice.BrokerCOALevel04Id, out var broker_coa_level04);
                dict_coa_level04s.TryGetValue(purchase_invoice.TaxCOALevel04Id, out var tax_coa_level04);
                dict_warehouses.TryGetValue(purchase_invoice.WarehouseId, out var warehouse);

                var dto = ObjectMapper.Map<PurchaseInvoiceGetAllDto>(purchase_invoice);
                dto.SupplierCOALevel04SerialNumber = supplier_coa_level04?.SerialNumber ?? "";
                dto.SupplierCOALevel04Name = supplier_coa_level04?.Name ?? "";
                dto.PaymentModeName = payment_mode?.Name ?? "";
                dto.AdvanceAmountBankCOALevl04Name = advance_bank_coa_level04?.Name ?? "";
                dto.TaxCOALevel04SerialNumber = tax_coa_level04?.SerialNumber ?? "";
                dto.TaxCOALevel04Name = tax_coa_level04?.Name ?? "";
                dto.BrokerCOALevel04SerialNumber = broker_coa_level04?.SerialNumber ?? "";
                dto.BrokerCOALevel04Name = broker_coa_level04?.Name ?? "";
                dto.WarehouseName = warehouse?.Name ?? "";
                output.Add(dto);
            }

            return new PagedResultDto<PurchaseInvoiceGetAllDto>(total_count.Value, output);
        }

        [AbpAuthorize(PermissionNames.LookUps_PurchaseInvoice)]
        public async Task<PagedResultDto<PurchaseInvoicePendingPaymentDto>> GetPendingPayments(PurchaseInvoiceFiltersDto filters)
        {
            var purchase_invoice_query = PurchaseInvoice_Repo.GetAll(this).ApplyDocumentFilters(filters);
            if (!string.IsNullOrWhiteSpace(filters.SupplierCOALevel04Id))
                purchase_invoice_query = purchase_invoice_query.Where(i => i.SupplierCOALevel04Id == filters.SupplierCOALevel04Id.TryToLong());

            // Consider only approved invoices for payment pending calculation
            purchase_invoice_query = purchase_invoice_query.Where(i => i.Status == "APPROVED");

            var purchase_invoices = await purchase_invoice_query.ToListAsync();

            var voucher_numbers = purchase_invoices
                .Select(i => i.VoucherNumber)
                .Where(v => !string.IsNullOrWhiteSpace(v))
                .Distinct()
                .ToList();

            var payment_docs = await GeneralLedger_Repo
                .Where(this, i =>
                    (i.LinkedDocument == GeneralLedgerLinkedDocument.GeneralPayment_BankPayment ||
                     i.LinkedDocument == GeneralLedgerLinkedDocument.GeneralPayment_CashPayment) &&
                    voucher_numbers.Contains(i.ReferenceVoucherNumber))
                .ToListAsync();

            var paid_by_ref = payment_docs
                .GroupBy(i => i.ReferenceVoucherNumber)
                .ToDictionary(g => g.Key, g => g.Sum(x => x.Credit ?? 0));

            var invoice_ids = purchase_invoices.Select(i => i.Id).ToList();
            var supplier_ids = purchase_invoices.Select(i => i.SupplierCOALevel04Id).Distinct().ToList();
            var suppliers = COALevel04_Repo.GetAll(this, i => supplier_ids.Contains(i.Id)).Select(i => new { i.Id, i.Name }).ToList();
            var supplier_name_by_id = suppliers.ToDictionary(i => i.Id, i => i.Name);

            // Sum approved purchase returns linked to each invoice
            var approved_returns = await PurchaseReturn_Repo
                .GetAllIncluding(r => r.PurchaseReturnDetails)
                .Where(r => r.TenantId == AbpSession.TenantId && r.Status == "APPROVED")
                .ToListAsync();

            var returns_by_invoice = new Dictionary<long, decimal>();
            foreach (var ret in approved_returns)
            {
                var ret_total = ret.PurchaseReturnDetails?.Sum(d => d.GrandTotal) ?? 0;
                foreach (var invId in ret.PurchaseInvoiceIds ?? new List<long>())
                {
                    if (!invoice_ids.Contains(invId)) continue;
                    returns_by_invoice[invId] = returns_by_invoice.GetValueOrDefault(invId) + ret_total;
                }
            }

            var pending_list = new List<PurchaseInvoicePendingPaymentDto>();
            foreach (var inv in purchase_invoices)
            {
                var advance = inv.AdvanceAmount;
                var returns = returns_by_invoice.GetValueOrDefault(inv.Id);
                var paid = string.IsNullOrWhiteSpace(inv.VoucherNumber) ? 0 : paid_by_ref.GetValueOrDefault(inv.VoucherNumber);
                var pending = inv.GrandTotal - returns - (advance) - paid;
                if (pending > 0)
                {
                    pending_list.Add(new PurchaseInvoicePendingPaymentDto
                    {
                        Id = inv.Id,
                        IssueDate = inv.IssueDate,
                        VoucherNumber = inv.VoucherNumber,
                        ReferenceNumber = inv.ReferenceNumber,
                        COALevel04Id = inv.SupplierCOALevel04Id,
                        SupplierName = supplier_name_by_id.GetValueOrDefault(inv.SupplierCOALevel04Id, string.Empty),
                        GrandTotal = inv.GrandTotal,
                        TaxAmount = inv.Tax,
                        AdvanceAmount = advance,
                        PaidAmount = paid,
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

            return new PagedResultDto<PurchaseInvoicePendingPaymentDto>(total_count, paged);
        }

        [AbpAuthorize(PermissionNames.LookUps_PurchaseInvoice_Create)]
        public async Task<PurchaseInvoiceCreateResultDto> Create(PurchaseInvoiceDto input)
        {
            var supplier_coa_level04 = COALevel04_Repo.GetAll(this, i => i.Id == input.SupplierCOALevel04Id).DeferredFirstOrDefault().FutureValue();
            var payment_mode = PaymentMode_Repo.GetAll(this, i => i.Id == input.PaymentModeId).DeferredFirstOrDefault().FutureValue();
            var warehouse = Warehouse_Repo.GetAll(this, i => i.Id == input.WarehouseId).DeferredFirstOrDefault().FutureValue();
            await warehouse.ValueAsync();

            if (supplier_coa_level04.Value == null)
                throw new UserFriendlyException($"SupplierCOALevel04Id: '{input.SupplierCOALevel04Id}' is invalid.");
            if (payment_mode.Value == null)
                throw new UserFriendlyException($"PaymentModeId: '{input.PaymentModeId}' is invalid.");
            if (warehouse.Value == null)
                throw new UserFriendlyException($"WarehouseId: '{input.WarehouseId}' is invalid.");

            var entity = ObjectMapper.Map<PurchaseInvoiceInfo>(input);
            entity.Status = "PENDING";
            entity.VoucherNumber = await GetVoucherNumber("PI", input.IssueDate);

            var item_ids = input.PurchaseInvoiceDetails.Select(i => i.ItemId).ToList();
            var unit_ids = input.PurchaseInvoiceDetails.Select(i => i.UnitId).ToList();

            var items = Item_Repo.GetAll(this, i => item_ids.Contains(i.Id)).Select(i => i.Id).Future();
            var units = Unit_Repo.GetAll(this, i => unit_ids.Contains(i.Id)).Select(i => i.Id).Future();
            _ = await units.ToListAsync();

            for (int i = 0; i < entity.PurchaseInvoiceDetails.Count; i++)
            {
                var detail = entity.PurchaseInvoiceDetails[i];

                if (!items.Contains(detail.ItemId))
                    throw new UserFriendlyException($"ItemId: '{detail.ItemId}' is invalid at Row: '{i + 1}'.");
                if (!units.Contains(detail.UnitId))
                    throw new UserFriendlyException($"UnitId: '{detail.UnitId}' is invalid at Row: '{i + 1}'.");
            }

            entity.TenantId = AbpSession.TenantId;
            await PurchaseInvoice_Repo.InsertAsync(entity);
            await CurrentUnitOfWork.SaveChangesAsync();

            foreach (var detail in entity.PurchaseInvoiceDetails)
            {
                var item = await Item_Repo.GetAllIncluding(i => i.ItemDetails)
                    .FirstOrDefaultAsync(i => i.Id == detail.ItemId);

                var unit = await Unit_Repo.GetAll()
                    .FirstOrDefaultAsync(i => i.Id == detail.UnitId);

                if (item != null)
                {
                    var itemDetail = item.ItemDetails.FirstOrDefault(d => d.UnitId == detail.UnitId);
                    if (itemDetail != null)
                    {
                        itemDetail.UnitPrice = detail.CostRate;
                        itemDetail.PerBagPrice = detail.CostRate * unit.ConversionFactor;
                        itemDetail.MinSalePrice = detail.CostRate + 5;
                        itemDetail.MaxSalePrice = detail.CostRate + 10;
                    }
                    else
                    {
                        if (item.ItemDetails == null)
                        {
                            item.ItemDetails = new List<ItemDetailsInfo>();
                        }
                        
                        item.ItemDetails.Add(new ItemDetailsInfo
                        {
                            UnitId = detail.UnitId,
                            UnitPrice = detail.CostRate,
                            PerBagPrice = detail.CostRate * unit.ConversionFactor,
                            MinSalePrice = detail.CostRate + 5,
                            MaxSalePrice = detail.CostRate + 10,
                            MinStockLevel = 0,
                            Barcode = string.Empty
                        });
                    }
                    
                    await Item_Repo.UpdateAsync(item);
                }
            }
            await CurrentUnitOfWork.SaveChangesAsync();

            return new PurchaseInvoiceCreateResultDto
            {
                Id = entity.Id,
                Message = "PurchaseInvoice Created Successfully."
            };
        }

        private async Task<PurchaseInvoiceInfo> GetById(long Id)
        {
            var purchase_invoice = await PurchaseInvoice_Repo.GetAllIncluding(i => i.PurchaseInvoiceDetails).Where(i => i.Id == Id).FirstOrDefaultAsync();
            if (purchase_invoice != null)
                return purchase_invoice;
            else
                throw new UserFriendlyException($"PurchaseInvoiceId: '{Id}' is invalid.");
        }

        public async Task<PurchaseInvoiceGetForEditDto> Get(long Id)
        {
            var purchase_invoice = await GetById(Id);
            var supplier_coa_level04 = COALevel04_Repo.GetAll(this, i => i.Id == purchase_invoice.SupplierCOALevel04Id).DeferredFirstOrDefault().FutureValue();
            var payment_mode = PaymentMode_Repo.GetAll(this, i => i.Id == purchase_invoice.PaymentModeId).DeferredFirstOrDefault().FutureValue();
            var advance_bank_coa_level04 = COALevel04_Repo.GetAll(this, i => i.Id == purchase_invoice.AdvanceAmountBankCOALevl04Id).DeferredFirstOrDefault().FutureValue();
            var tax_coa_level04 = COALevel04_Repo.GetAll(this, i => i.Id == purchase_invoice.TaxCOALevel04Id).DeferredFirstOrDefault().FutureValue();
            var broker_coa_level04 = COALevel04_Repo.GetAll(this, i => i.Id == purchase_invoice.BrokerCOALevel04Id).DeferredFirstOrDefault().FutureValue();
            var warehouse = Warehouse_Repo.GetAll(this, i => i.Id == purchase_invoice.WarehouseId).DeferredFirstOrDefault().FutureValue();
            await warehouse.ValueAsync();

            var output = ObjectMapper.Map<PurchaseInvoiceGetForEditDto>(purchase_invoice);
            output.SupplierCOALevel04Name = supplier_coa_level04?.Value?.Name ?? "";
            output.PaymentModeName = payment_mode?.Value?.Name ?? "";
            output.AdvanceAmountBankCOALevl04Name = advance_bank_coa_level04?.Value?.Name ?? "";
            output.TaxCOALevel04Name = tax_coa_level04?.Value?.Name ?? "";
            output.BrokerCOALevel04Name = broker_coa_level04?.Value?.Name ?? "";
            output.WarehouseName = warehouse?.Value?.Name ?? "";

            var item_ids = purchase_invoice.PurchaseInvoiceDetails.Select(i => i.ItemId).ToList();
            var unit_ids = purchase_invoice.PurchaseInvoiceDetails.Select(i => i.UnitId).ToList();

            var items = Item_Repo.GetAll(this, i => item_ids.Contains(i.Id)).Future();
            var units = Unit_Repo.GetAll(this, i => unit_ids.Contains(i.Id)).Future();
            _ = await units.ToListAsync();

            var dict_items = items.ToDictionary(i => i.Id, i => i.Name);
            var dict_units = units.ToDictionary(i => i.Id, i => i.Name);

            output.PurchaseInvoiceDetails = new();
            foreach (var detail in purchase_invoice.PurchaseInvoiceDetails)
            {
                var mapped_detail = ObjectMapper.Map<PurchaseInvoiceDetailsGetForEditDto>(detail);
                mapped_detail.ItemName = dict_items.GetValueOrDefault(detail.ItemId);
                mapped_detail.UnitName = dict_units.GetValueOrDefault(detail.UnitId);
                output.PurchaseInvoiceDetails.Add(mapped_detail);
            }

            return output;
        }

        [AbpAuthorize(PermissionNames.LookUps_PurchaseInvoice_Update)]
        public async Task<string> Update(PurchaseInvoiceDto input)
        {
            if (input.Id <= 0)
                throw new UserFriendlyException($"PurchaseInvoiceId must be provided. Current value: '{input.Id}' is invalid.");
                
            var old_purchaseinvoice = await GetById(input.Id);
            if (old_purchaseinvoice.Status != "PENDING")
                throw new UserFriendlyException($"Only entries with a status of 'PENDING' can be edited. The current status is '{old_purchaseinvoice.Status}'.");

            var supplier_coa_level04 = COALevel04_Repo.GetAll(this, i => i.Id == input.SupplierCOALevel04Id).DeferredFirstOrDefault().FutureValue();
            var payment_mode = PaymentMode_Repo.GetAll(this, i => i.Id == input.PaymentModeId).DeferredFirstOrDefault().FutureValue();
            var warehouse = Warehouse_Repo.GetAll(this, i => i.Id == input.WarehouseId).DeferredFirstOrDefault().FutureValue();
            await warehouse.ValueAsync();

            if (supplier_coa_level04.Value == null)
                throw new UserFriendlyException($"SupplierCOALevel04Id: '{input.SupplierCOALevel04Id}' is invalid.");
            if (payment_mode.Value == null)
                throw new UserFriendlyException($"PaymentModeId: '{input.PaymentModeId}' is invalid.");
            if (warehouse.Value == null)
                throw new UserFriendlyException($"WarehouseId: '{input.WarehouseId}' is invalid.");

            var entity = ObjectMapper.Map(input, old_purchaseinvoice);
            entity.VoucherNumber = await GetVoucherNumber("PI", input.IssueDate, entity.VoucherNumber.GetVoucherIndex());

            var item_ids = input.PurchaseInvoiceDetails.Select(i => i.ItemId).ToList();
            var unit_ids = input.PurchaseInvoiceDetails.Select(i => i.UnitId).ToList();

            var items = Item_Repo.GetAll(this, i => item_ids.Contains(i.Id)).Select(i => i.Id).Future();
            var units = Unit_Repo.GetAll(this, i => unit_ids.Contains(i.Id)).Select(i => i.Id).Future();
            _ = await units.ToListAsync();

            for (int i = 0; i < entity.PurchaseInvoiceDetails.Count; i++)
            {
                var detail = entity.PurchaseInvoiceDetails[i];

                if (!items.Contains(detail.ItemId))
                    throw new UserFriendlyException($"ItemId: '{detail.ItemId}' is invalid at Row: '{i + 1}'.");
                if (!units.Contains(detail.UnitId))
                    throw new UserFriendlyException($"UnitId: '{detail.UnitId}' is invalid at Row: '{i + 1}'.");
            }

            await PurchaseInvoice_Repo.UpdateAsync(entity);
            
            foreach (var detail in entity.PurchaseInvoiceDetails)
            {
                var item = await Item_Repo.GetAllIncluding(i => i.ItemDetails)
                    .FirstOrDefaultAsync(i => i.Id == detail.ItemId);
                
                var unit = await Unit_Repo.GetAll()
                    .FirstOrDefaultAsync(i => i.Id == detail.UnitId);
                    
                if (item != null)
                {
                    var itemDetail = item.ItemDetails.FirstOrDefault(d => d.UnitId == detail.UnitId);
                    if (itemDetail != null)
                    {
                        itemDetail.UnitPrice = detail.CostRate;
                        itemDetail.PerBagPrice = detail.CostRate * unit.ConversionFactor;
                        itemDetail.MinSalePrice = detail.CostRate + 5;
                        itemDetail.MaxSalePrice = detail.CostRate + 10;
                    }
                    else
                    {
                        if (item.ItemDetails == null)
                        {
                            item.ItemDetails = new List<ItemDetailsInfo>();
                        }
                        
                        item.ItemDetails.Add(new ItemDetailsInfo
                        {
                            UnitId = detail.UnitId,
                            UnitPrice = detail.CostRate,
                            PerBagPrice = detail.CostRate * unit.ConversionFactor,
                            MinSalePrice = detail.CostRate + 5,
                            MaxSalePrice = detail.CostRate + 10,
                            MinStockLevel = 0,
                            Barcode = string.Empty
                        });
                    }
                    
                    await Item_Repo.UpdateAsync(item);
                }
            }
            
            await CurrentUnitOfWork.SaveChangesAsync();
            return "PurchaseInvoice Updated Successfully.";
        }

        [AbpAuthorize(PermissionNames.LookUps_PurchaseInvoice_Delete)]
        public async Task<string> Delete(long Id)
        {
            var purchase_invoice = await PurchaseInvoice_Repo.GetAll(this, i => i.Id == Id).FirstOrDefaultAsync();
            if (purchase_invoice == null)
                throw new UserFriendlyException($"PurchaseInvoiceId: '{Id}' is invalid.");
            if (purchase_invoice.Status != "PENDING")
                throw new UserFriendlyException($"Unable to delete PurchaseInvoice: only records with a 'PENDING' status can be deleted.");

            await PurchaseInvoice_Repo.DeleteAsync(purchase_invoice);
            await CurrentUnitOfWork.SaveChangesAsync();
            return "PurchaseInvoice Deleted Successfully.";
        }

        public async Task<DocumentUploadResultDto> UploadDocuments(DocumentUploadDto input)
        {
            if (input.Base64Images == null || input.Base64Images.Count == 0)
                throw new UserFriendlyException("No images were provided for upload.");

            string documentDirectory = "purchase-invoices";
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

        public async override Task<string> ApproveDocument(long Id)
        {
            var document = await Get(Id);
            var item_ids = document.PurchaseInvoiceDetails.Select(i => i.ItemId).ToList();
            var items = Item_Repo.GetAll(this, i => item_ids.Contains(i.Id)).Future();
            var supplier = COALevel04_Repo.GetAll(this, i => i.Id == document.SupplierCOALevel04Id).DeferredFirstOrDefault().FutureValue();
            _ = await supplier.ValueAsync();

            var purchases_coa_id = await FINANCE_DefaultIntegrations_Repo.GetAll(this, i => i.Name == "PURCHASES A/C").Select(i => i.ChartOfAccountId).FirstOrDefaultAsync();
            if (purchases_coa_id == 0)
                throw new UserFriendlyException("Default integration 'PURCHASES A/C' is not configured.");

            var transaction_base = new GeneralLedgerTransactionBaseDto()
            {
                IssueDate = document.IssueDate,
                VoucherNumber = document.VoucherNumber,
                IsAdjustmentEntry = false,
                Remarks = document.Remarks,
                LinkedDocument = GeneralLedgerLinkedDocument.PurchaseInvoice,
                LinkedDocumentId = document.Id,
                TenantId = AbpSession.TenantId,
            };  

            foreach (var detail in document.PurchaseInvoiceDetails)
            {
                 await WarehouseStockLedger_Repo.AddLedgerTransactionAsync(
                    document.IssueDate,
                    document.VoucherNumber,
                    detail.ItemId,
                    document.WarehouseId,
                    detail.Quantity,
                    0,
                    detail.LastPurchaseRate,
                    detail.ActualQuantity,
                    detail.GrandTotal,
                    document.SupplierCOALevel04Id,
                    document.Remarks,
                    document.Id,
                    Enums.WarehouseStockLedgerLinkedDocument.PurchaseInvoice,
                    AbpSession.TenantId
                );
                await GeneralLedger_Repo.AddLedgerTransactionAsync(transaction_base, 0, detail.GrandTotal, purchases_coa_id, 0);
            }

            await GeneralLedger_Repo.AddLedgerTransactionAsync(transaction_base, document.GrandTotal, 0, document.SupplierCOALevel04Id, 0);
            await GeneralLedger_Repo.AddLedgerTransactionAsync(transaction_base, 0, document.AdvanceAmount ?? 0, document.SupplierCOALevel04Id, 0);
            await GeneralLedger_Repo.AddLedgerTransactionAsync(transaction_base,  document.AdvanceAmount ?? 0,0, document.AdvanceAmountBankCOALevl04Id??0, 0);

            CurrentUnitOfWork.SaveChanges();
            return await base.ApproveDocument(Id);
        }

        [AbpAuthorize(PermissionNames.LookUps_PurchaseInvoice_ApproveDocument)]
        public async Task<string> UnapproveDocument(long Id)
        {
            var purchase_invoice = await PurchaseInvoice_Repo.GetAll(this, i => i.Id == Id).FirstOrDefaultAsync();
            if (purchase_invoice == null)
                throw new UserFriendlyException($"Could not find PurchaseInvoice with ID: '{Id}'.");
            if (purchase_invoice.Status != "APPROVED")
                throw new UserFriendlyException($"Before Unapproving Document; Status must be 'APPROVED'");

            var general_ledger_entries = await GeneralLedger_Repo
                .Where(this, i => i.LinkedDocument == GeneralLedgerLinkedDocument.PurchaseInvoice && i.LinkedDocumentId == Id)
                .ToListAsync();
            if (general_ledger_entries.Count > 0)
                await GeneralLedger_Repo.DeleteRangeAsync(general_ledger_entries);

            var warehouse_stock_ledger_entries = await WarehouseStockLedger_Repo
                .Where(this, i => i.DocumentId == Id && i.WarehouseStockLedgerLinkedDocument == WarehouseStockLedgerLinkedDocument.PurchaseInvoice)
                .ToListAsync();
            if (warehouse_stock_ledger_entries.Count > 0)
                await WarehouseStockLedger_Repo.DeleteRangeAsync(warehouse_stock_ledger_entries);

            purchase_invoice.Status = "PENDING";
            await PurchaseInvoice_Repo.UpdateAsync(purchase_invoice);
            await CurrentUnitOfWork.SaveChangesAsync();

            return "PurchaseInvoice Status 'PENDING' Successfully.";
        }

        [AbpAuthorize(PermissionNames.LookUps_PurchaseInvoice_Update)]
        public async Task<string> UpdateWarehouseStockLedger(long Id)
        {
            var document = await GetById(Id);
            if (document.IsStockup)
            {
                throw new UserFriendlyException("The Stock is already up.");
            }
        
            foreach (var detail in document.PurchaseInvoiceDetails)
            {
                await WarehouseStockLedger_Repo.AddLedgerTransactionAsync(
                    document.IssueDate,
                    document.VoucherNumber,
                    detail.ItemId,
                    document.WarehouseId,
                    detail.Quantity,
                    0,
                    detail.LastPurchaseRate,
                    detail.ActualQuantity,
                    detail.GrandTotal,
                    document.Remarks,
                    document.Id,
                    Enums.WarehouseStockLedgerLinkedDocument.PurchaseInvoice,
                    AbpSession.TenantId
                );
            }
            document.IsStockup= true;
            await PurchaseInvoice_Repo.UpdateAsync(document);
            CurrentUnitOfWork.SaveChanges();
            return "Warehouse Stock Ledger entries created successfully.";
        }

        public async Task<decimal> GetLatestRate(long ItemId, long SupplierCOALevel04Id, long UnitId)
        {
            var latest_purchase_rate = await PurchaseInvoice_Repo
                .GetAllIncluding(i => i.PurchaseInvoiceDetails)
                .Where(i => i.SupplierCOALevel04Id == SupplierCOALevel04Id)
                .OrderByDescending(i => i.IssueDate)
                .ThenByDescending(i => i.CreationTime)
                .SelectMany(i => i.PurchaseInvoiceDetails)
                .Where(detail => detail.ItemId == ItemId && detail.UnitId == UnitId)
                .Select(i => i.CostRate)
                .FirstOrDefaultAsync();

            return latest_purchase_rate;
        }
    }
}

