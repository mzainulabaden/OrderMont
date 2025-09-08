using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.UI;
using ERP.Authorization;
using ERP.Modules.Finance.ChartOfAccount.COALevel04;
using ERP.Modules.Finance.LookUps;
using ERP.Modules.InventoryManagement.Item;
using ERP.Modules.InventoryManagement.LookUps;
using ERP.Modules.InventoryManagement.PurchaseInvoice;
using ERP.Modules.InventoryManagement.PurchaseOrder.Dtos;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace ERP.Modules.InventoryManagement.PurchaseOrder
{
    [AbpAuthorize(PermissionNames.LookUps_PurchaseOrder)]
    public class PurchaseOrderAppService : ERPDocumentService<PurchaseOrderInfo>
    {
        public IRepository<PurchaseOrderInfo, long> PurchaseOrder_Repo { get; set; }
        public IRepository<PurchaseInvoiceInfo, long> PurchaseInvoice_Repo { get; set; }
        public IRepository<COALevel04Info, long> COALevel04_Repo { get; set; }
        public IRepository<PaymentModeInfo, long> PaymentMode_Repo { get; set; }
        public IRepository<ItemInfo, long> Item_Repo { get; set; }
        public IRepository<UnitInfo, long> Unit_Repo { get; set; }
        public IRepository<WarehouseInfo, long> Warehouse_Repo { get; set; }

        public async Task<PagedResultDto<PurchaseOrderGetAllDto>> GetAll(PurchaseOrderFiltersDto filters)
        {
            var purchase_order_query = PurchaseOrder_Repo.GetAll(this).ApplyDocumentFilters(filters);
            if (!string.IsNullOrWhiteSpace(filters.SupplierCOALevel04Id))
                purchase_order_query = purchase_order_query.Where(i => i.SupplierCOALevel04Id == filters.SupplierCOALevel04Id.TryToLong());
            if (!string.IsNullOrWhiteSpace(filters.PaymentModeId))
                purchase_order_query = purchase_order_query.Where(i => i.PaymentModeId == filters.PaymentModeId.TryToLong());
            var purchase_orders = await purchase_order_query.ToPagedListAsync(filters);

            var coa_level04_ids = purchase_orders.Select(i => i.SupplierCOALevel04Id).ToList();
            var payment_mode_ids = purchase_orders.Select(i => i.PaymentModeId).ToList();

            var total_count = purchase_order_query.DeferredCount().FutureValue();
            var supplier_coa_level04s = COALevel04_Repo.GetAll(this, i => coa_level04_ids.Contains(i.Id)).Select(i => new { i.Id, i.SerialNumber, i.Name }).Future();
            var payment_modes = PaymentMode_Repo.GetAll(this, i => payment_mode_ids.Contains(i.Id)).Select(i => new { i.Id, i.Name }).Future();
            _ = await payment_modes.ToListAsync();

            var dict_coa_level04s = supplier_coa_level04s.ToDictionary(i => i.Id);
            var dict_payment_modes = payment_modes.ToDictionary(i => i.Id);

            var output = new List<PurchaseOrderGetAllDto>();
            foreach (var purchase_order in purchase_orders)
            {
                dict_coa_level04s.TryGetValue(purchase_order.SupplierCOALevel04Id, out var supplier_coa_level04);
                dict_payment_modes.TryGetValue(purchase_order.PaymentModeId, out var payment_mode);

                var dto = ObjectMapper.Map<PurchaseOrderGetAllDto>(purchase_order);
                dto.SupplierCOALevel04SerialNumber = supplier_coa_level04?.SerialNumber ?? "";
                dto.SupplierCOALevel04Name = supplier_coa_level04?.Name ?? "";
                dto.PaymentModeName = payment_mode?.Name ?? "";
                dto.AttachedDocuments = purchase_order.AttachedDocuments;
                output.Add(dto);
            }

            return new PagedResultDto<PurchaseOrderGetAllDto>(total_count.Value, output);
        }

        public async Task<PagedResultDto<PurchaseOrderGetAllDto>> GetAllPurchaseOrders(PurchaseOrderFiltersDto filters)
        {
            var purchase_order_query = PurchaseOrder_Repo.GetAll(this).ApplyDocumentFilters(filters);
            if (!string.IsNullOrWhiteSpace(filters.PaymentModeId))
                purchase_order_query = purchase_order_query.Where(i => i.PaymentModeId == filters.PaymentModeId.TryToLong());
            if (!string.IsNullOrWhiteSpace(filters.SupplierCOALevel04Id))
                purchase_order_query = purchase_order_query.Where(i => i.SupplierCOALevel04Id == filters.SupplierCOALevel04Id.TryToLong());

            var purchase_orders = await purchase_order_query.Include(i => i.PurchaseOrderDetails).ToPagedListAsync(filters);

            var allDetailIds = purchase_orders.SelectMany(o => o.PurchaseOrderDetails).Select(d => d.Id).ToList();
            var allInvoiceDetails = PurchaseInvoice_Repo.GetAll()
                .SelectMany(i => i.PurchaseInvoiceDetails)
                .Where(d => allDetailIds.Contains(d.PurchaseOrderDetailId))
                .ToList();

            var invoiceQtyByDetailId = allInvoiceDetails
                .GroupBy(d => d.PurchaseOrderDetailId)
                .ToDictionary(g => g.Key, g => g.Sum(x => x.Quantity));

            var payment_mode_ids = purchase_orders.Select(i => i.PaymentModeId).ToList();
            var supplier_coa_level04_ids = purchase_orders.Select(i => i.SupplierCOALevel04Id).ToList();
            var item_ids = purchase_orders.SelectMany(o => o.PurchaseOrderDetails).Select(d => d.ItemId).Distinct().ToList();
            var unit_ids = purchase_orders.SelectMany(o => o.PurchaseOrderDetails).Select(d => d.UnitId).Distinct().ToList();
           
            var total_count = purchase_orders.Count;
            var payment_modes = PaymentMode_Repo.GetAll(this, i => payment_mode_ids.Contains(i.Id)).Select(i => new { i.Id, i.Name }).ToList();
            var supplier_coa_level04s = COALevel04_Repo.GetAll(this, i => supplier_coa_level04_ids.Contains(i.Id)).Select(i => new { i.Id, i.Name, i.SerialNumber }).ToList();
            var items = Item_Repo.GetAll(this, i => item_ids.Contains(i.Id)).Select(i => new { i.Id, i.Name }).ToList();
            var units = Unit_Repo.GetAll(this, i => unit_ids.Contains(i.Id)).Select(i => new { i.Id, i.Name }).ToList();
          
            var dict_payment_modes = payment_modes.ToDictionary(i => i.Id);
            var dict_supplier_coa_level04s = supplier_coa_level04s.ToDictionary(i => i.Id);
            var dict_items = items.ToDictionary(i => i.Id);
            var dict_units = units.ToDictionary(i => i.Id);
           

            var output = new List<PurchaseOrderGetAllDto>();
            foreach (var purchase_order in purchase_orders)
            {
                dict_payment_modes.TryGetValue(purchase_order.PaymentModeId, out var payment_mode);
                dict_supplier_coa_level04s.TryGetValue(purchase_order.SupplierCOALevel04Id, out var supplier_coa_level04);

                var dto = ObjectMapper.Map<PurchaseOrderGetAllDto>(purchase_order);
                dto.PaymentModeName = payment_mode?.Name ?? "";
                dto.SupplierCOALevel04Name = supplier_coa_level04?.Name ?? "";
                dto.SupplierCOALevel04SerialNumber = supplier_coa_level04?.SerialNumber ?? "";
                dto.AttachedDocuments = purchase_order.AttachedDocuments;

                dto.PurchaseOrderDetails = new List<PurchaseOrderDetailsGetAllDto>();
                foreach (var detail in purchase_order.PurchaseOrderDetails)
                {
                    var invoicedQty = invoiceQtyByDetailId.ContainsKey(detail.Id) ? invoiceQtyByDetailId[detail.Id] : 0;
                    var remainingQty = detail.Quantity - invoicedQty;
                    if (remainingQty > 0)
                    {
                        dict_items.TryGetValue(detail.ItemId, out var item);
                        dict_units.TryGetValue(detail.UnitId, out var unit);
                        
                        var detailDto = ObjectMapper.Map<PurchaseOrderDetailsGetAllDto>(detail);
                        detailDto.ItemName = item?.Name ?? "";
                        detailDto.UnitName = unit?.Name ?? "";
                        detailDto.InvoicedQty = invoicedQty;
                        detailDto.RemainingQty = remainingQty;
                        dto.PurchaseOrderDetails.Add(detailDto);
                    }
                }
                if (dto.PurchaseOrderDetails.Count > 0)
                    output.Add(dto);
            }

            return new PagedResultDto<PurchaseOrderGetAllDto>(output.Count, output);
        }

        [AbpAuthorize(PermissionNames.LookUps_PurchaseOrder_Create)]
        public async Task<string> Create(PurchaseOrderDto input)
        {
            var supplier_coa_level04 = COALevel04_Repo.GetAll(this, i => i.Id == input.SupplierCOALevel04Id).DeferredFirstOrDefault().FutureValue();
            var payment_mode = PaymentMode_Repo.GetAll(this, i => i.Id == input.PaymentModeId).DeferredFirstOrDefault().FutureValue();
            _ = await payment_mode.ValueAsync();

            if (supplier_coa_level04.Value == null)
                throw new UserFriendlyException($"SupplierCOALevel04Id: '{input.SupplierCOALevel04Id}' is invalid.");
            if (payment_mode.Value == null)
                throw new UserFriendlyException($"PaymentModeId: '{input.PaymentModeId}' is invalid.");

            var entity = ObjectMapper.Map<PurchaseOrderInfo>(input);
            entity.Status = "PENDING";
            entity.VoucherNumber = await GetVoucherNumber("PO", input.IssueDate);
            entity.AttachedDocuments = input.AttachedDocuments;

            var item_ids = input.PurchaseOrderDetails.Select(i => i.ItemId).ToList();
            var unit_ids = input.PurchaseOrderDetails.Select(i => i.UnitId).ToList();

            var items = Item_Repo.GetAll(this, i => item_ids.Contains(i.Id)).Select(i => i.Id).Future();
            var units = Unit_Repo.GetAll(this, i => unit_ids.Contains(i.Id)).Future();
            var unitsList = await units.ToListAsync();

            for (int i = 0; i < entity.PurchaseOrderDetails.Count; i++)
            {
                var detail = entity.PurchaseOrderDetails[i];

                if (!items.Contains(detail.ItemId))
                    throw new UserFriendlyException($"ItemId: '{detail.ItemId}' is invalid at Row: '{i + 1}'.");
                if (!units.Select(u => u.Id).Contains(detail.UnitId))
                    throw new UserFriendlyException($"UnitId: '{detail.UnitId}' is invalid at Row: '{i + 1}'.");
                
                // Calculate PricePerBag based on PricePerKg and the unit's ConversionFactor
                var unit = unitsList.FirstOrDefault(u => u.Id == detail.UnitId);
                if (unit != null)
                {
                    detail.PricePerBag = detail.PricePerKg * unit.ConversionFactor;
                }
            }

            entity.TenantId = AbpSession.TenantId;
            await PurchaseOrder_Repo.InsertAsync(entity);
            await CurrentUnitOfWork.SaveChangesAsync();
            return "PurchaseOrder Created Successfully.";
        }

        private async Task<PurchaseOrderInfo> GetById(long Id)
        {
            var purchase_order = await PurchaseOrder_Repo.GetAllIncluding(i => i.PurchaseOrderDetails).Where(i => i.Id == Id).FirstOrDefaultAsync();
            if (purchase_order != null)
                return purchase_order;
            else
                throw new UserFriendlyException($"PurchaseOrderId: '{Id}' is invalid.");
        }

        public async Task<PurchaseOrderGetForEditDto> Get(long Id)
        {
            var purchase_order = await GetById(Id);
            var supplier_coa_level04 = COALevel04_Repo.GetAll(this, i => i.Id == purchase_order.SupplierCOALevel04Id).DeferredFirstOrDefault().FutureValue();
            var payment_mode = PaymentMode_Repo.GetAll(this, i => i.Id == purchase_order.PaymentModeId).DeferredFirstOrDefault().FutureValue();
            _ = await payment_mode.ValueAsync();

            var output = ObjectMapper.Map<PurchaseOrderGetForEditDto>(purchase_order);
            output.SupplierCOALevel04Name = supplier_coa_level04?.Value?.Name ?? "";
            output.SupplierCOALevel04SerialNumber = supplier_coa_level04?.Value?.SerialNumber ?? "";
            output.PaymentModeName = payment_mode?.Value?.Name ?? "";

            var item_ids = purchase_order.PurchaseOrderDetails.Select(i => i.ItemId).ToList();
            var unit_ids = purchase_order.PurchaseOrderDetails.Select(i => i.UnitId).ToList();

            var items = Item_Repo.GetAll(this, i => item_ids.Contains(i.Id)).Select(i => new { i.Id, i.Name }).Future();
            var units = Unit_Repo.GetAll(this, i => unit_ids.Contains(i.Id)).Select(i => new { i.Id, i.Name }).Future();
            _ = await units.ToListAsync();

            var dict_items = items.ToDictionary(i => i.Id, i => i.Name);
            var dict_units = units.ToDictionary(i => i.Id, i => i.Name);

            output.PurchaseOrderDetails = new();
            foreach (var detail in purchase_order.PurchaseOrderDetails)
            {
                var mapped_detail = ObjectMapper.Map<PurchaseOrderDetailsGetForEditDto>(detail);
                mapped_detail.ItemName = dict_items.GetValueOrDefault(detail.ItemId);
                mapped_detail.UnitName = dict_units.GetValueOrDefault(detail.UnitId);
                output.PurchaseOrderDetails.Add(mapped_detail);
            }

            return output;
        }

        [AbpAuthorize(PermissionNames.LookUps_PurchaseOrder_Update)]
        public async Task<string> Update(PurchaseOrderDto input)
        {
            var old_purchase_order = await GetById(input.Id);
            if (old_purchase_order.Status != "PENDING")
                throw new UserFriendlyException($"Only entries with a status of 'PENDING' can be edited. The current status is '{old_purchase_order.Status}'.");

            var supplier_coa_level04 = COALevel04_Repo.GetAll(this, i => i.Id == input.SupplierCOALevel04Id).DeferredFirstOrDefault().FutureValue();
            var payment_mode = PaymentMode_Repo.GetAll(this, i => i.Id == input.PaymentModeId).DeferredFirstOrDefault().FutureValue();
            await payment_mode.ValueAsync();

            if (supplier_coa_level04.Value == null)
                throw new UserFriendlyException($"SupplierCOALevel04Id: '{input.SupplierCOALevel04Id}' is invalid.");
            if (payment_mode.Value == null)
                throw new UserFriendlyException($"PaymentModeId: '{input.PaymentModeId}' is invalid.");

            var entity = ObjectMapper.Map(input, old_purchase_order);
            entity.VoucherNumber = await GetVoucherNumber("PO", input.IssueDate, entity.VoucherNumber.GetVoucherIndex());

            var item_ids = input.PurchaseOrderDetails.Select(i => i.ItemId).ToList();
            var unit_ids = input.PurchaseOrderDetails.Select(i => i.UnitId).ToList();

            var items = Item_Repo.GetAll(this, i => item_ids.Contains(i.Id)).Select(i => i.Id).Future();
            var units = Unit_Repo.GetAll(this, i => unit_ids.Contains(i.Id)).Future();
            var unitsList = await units.ToListAsync();

            for (int i = 0; i < entity.PurchaseOrderDetails.Count; i++)
            {
                var detail = entity.PurchaseOrderDetails[i];

                if (!items.Contains(detail.ItemId))
                    throw new UserFriendlyException($"ItemId: '{detail.ItemId}' is invalid at Row: '{i + 1}'.");
                if (!units.Select(u => u.Id).Contains(detail.UnitId))
                    throw new UserFriendlyException($"UnitId: '{detail.UnitId}' is invalid at Row: '{i + 1}'.");
                
                // Calculate PricePerBag based on PricePerKg and the unit's ConversionFactor
                var unit = unitsList.FirstOrDefault(u => u.Id == detail.UnitId);
                if (unit != null)
                {
                    detail.PricePerBag = detail.PricePerKg * unit.ConversionFactor;
                }
            }

            await PurchaseOrder_Repo.UpdateAsync(entity);
            await CurrentUnitOfWork.SaveChangesAsync();
            return "PurchaseOrder Updated Successfully.";
        }

        [AbpAuthorize(PermissionNames.LookUps_PurchaseOrder_Delete)]
        public async Task<string> Delete(long Id)
        {
            var purchase_order = await PurchaseOrder_Repo.GetAll(this, i => i.Id == Id).FirstOrDefaultAsync();
            if (purchase_order == null)
                throw new UserFriendlyException($"PurchaseOrderId: '{Id}' is invalid.");
            if (purchase_order.Status != "PENDING")
                throw new UserFriendlyException($"Unable to delete PurchaseOrder: only records with a 'PENDING' status can be deleted.");

            await PurchaseOrder_Repo.DeleteAsync(purchase_order);
            await CurrentUnitOfWork.SaveChangesAsync();
            return "PurchaseOrder Deleted Successfully.";
        }

        public async Task<decimal> GetLatestRate(long ItemId, long SupplierCOALevel04Id, long UnitId)
        {
            var latest_purchase_rate = await PurchaseOrder_Repo
                .GetAllIncluding(i => i.PurchaseOrderDetails)
                .Where(i => i.SupplierCOALevel04Id == SupplierCOALevel04Id)
                .OrderByDescending(i => i.IssueDate)
                .ThenByDescending(i => i.CreationTime)
                .SelectMany(i => i.PurchaseOrderDetails)
                .Where(detail => detail.ItemId == ItemId && detail.UnitId == UnitId)
                .Select(i => i.PricePerKg)
                .FirstOrDefaultAsync();

            return latest_purchase_rate;
        }
    }   
}