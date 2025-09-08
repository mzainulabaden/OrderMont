using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.UI;
using ERP.Authorization;
using ERP.Modules.InventoryManagement.Item;
using ERP.Modules.InventoryManagement.LookUps;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace ERP.Modules.InventoryManagement.WarehouseStockAdjustment
{
    [AbpAuthorize(PermissionNames.LookUps_IMS_WarehouseStockAdjustment)]
    public class WarehouseStockAdjustment : ERPDocumentService<WarehouseStockAdjustmentInfo>
    {
        public IRepository<WarehouseStockAdjustmentInfo, long> IMS_WarehouseStockAdjustment_Repo { get; set; }
        public IRepository<ItemInfo, long> InventoryItem_Repo { get; set; }
        public IRepository<UnitInfo, long> Unit_Repo { get; set; }
        public IRepository<WarehouseInfo, long> Warehouse_Repo { get; set; }

        public async Task<PagedResultDto<IMS_WarehouseStockAdjustmentGetAllDto>> GetAll(IMS_WarehouseStockAdjustmentFiltersDto filters)
        {
            var i_ms_warehouse_stock_adjustment_query = IMS_WarehouseStockAdjustment_Repo.GetAllIncluding(i => i.WarehouseStockAdjustmentDetails);
            if (AbpSession.TenantId.HasValue)
                i_ms_warehouse_stock_adjustment_query = i_ms_warehouse_stock_adjustment_query.Where(i => i.TenantId == AbpSession.TenantId);
            if (!string.IsNullOrWhiteSpace(filters.Id))
                i_ms_warehouse_stock_adjustment_query = i_ms_warehouse_stock_adjustment_query.Where(i => i.Id == filters.Id.TryToLong());
            var i_ms_warehouse_stock_adjustments = await i_ms_warehouse_stock_adjustment_query.ToPagedListAsync(filters);

            var total_count = i_ms_warehouse_stock_adjustment_query.DeferredCount().FutureValue();
            var output = new List<IMS_WarehouseStockAdjustmentGetAllDto>();
            foreach (var i_ms_warehouse_stock_adjustment in i_ms_warehouse_stock_adjustments)
            {
                var dto = ObjectMapper.Map<IMS_WarehouseStockAdjustmentGetAllDto>(i_ms_warehouse_stock_adjustment);
                if (i_ms_warehouse_stock_adjustment.WarehouseStockAdjustmentDetails != null && i_ms_warehouse_stock_adjustment.WarehouseStockAdjustmentDetails.Any())
                {
                    var firstDetail = i_ms_warehouse_stock_adjustment.WarehouseStockAdjustmentDetails.First();
                    dto.UnitId = firstDetail.UnitId;
                    dto.MinStockLevel = firstDetail.MinStockLevel;
                    dto.WarehouseId = firstDetail.WarehouseId;
                   
                }
                output.Add(dto);
            }
           
            return new PagedResultDto<IMS_WarehouseStockAdjustmentGetAllDto>(total_count.Value, output);
        }

        [AbpAuthorize(PermissionNames.LookUps_IMS_WarehouseStockAdjustment_Create)]
        public async Task<string> Create(IMS_WarehouseStockAdjustmentDto input)
        {
            var entity = ObjectMapper.Map<WarehouseStockAdjustmentInfo>(input);
            entity.Status = "PENDING";
            entity.VoucherNumber = await GetVoucherNumber("WSA", input.IssueDate);

            var inventory_item_ids = input.WarehouseStockAdjustmentDetails.Select(i => i.InventoryItemId).ToList();
            var unit_ids = input.WarehouseStockAdjustmentDetails.Select(i => i.UnitId).ToList();
            var warehouse_ids = input.WarehouseStockAdjustmentDetails.Select(i => i.WarehouseId).ToList();

            var inventory_items = InventoryItem_Repo.GetAll(this, i => inventory_item_ids.Contains(i.Id)).Select(i => i.Id).Future();
            var units = Unit_Repo.GetAll(this, i => unit_ids.Contains(i.Id)).Select(i => i.Id).Future();
            var warehouses = Warehouse_Repo.GetAll(this, i => warehouse_ids.Contains(i.Id)).Select(i => i.Id).Future();
            
            _ = await units.ToListAsync();
            _ = await warehouses.ToListAsync();

            for (int i = 0; i < entity.WarehouseStockAdjustmentDetails.Count; i++)
            {
                var detail = entity.WarehouseStockAdjustmentDetails[i];

                if (!inventory_items.Contains(detail.InventoryItemId))
                    throw new UserFriendlyException($"InventoryItemId: '{detail.InventoryItemId}' is invalid at Row: '{i + 1}'.");
                if (!units.Contains(detail.UnitId))
                    throw new UserFriendlyException($"UnitId: '{detail.UnitId}' is invalid at Row: '{i + 1}'.");
                if (!warehouses.Contains(detail.WarehouseId))
                    throw new UserFriendlyException($"WarehouseId: '{detail.WarehouseId}' is invalid at Row: '{i + 1}'.");

                // Update the unit price in ItemDetailsInfo for the item and unit
                var itemInfo = await InventoryItem_Repo.GetAllIncluding(x => x.ItemDetails)
                    .Where(x => x.Id == detail.InventoryItemId)
                    .FirstOrDefaultAsync();
                if (itemInfo != null && itemInfo.ItemDetails != null)
                {
                    var itemDetail = itemInfo.ItemDetails.FirstOrDefault(d => d.UnitId == detail.UnitId);
                    if (itemDetail != null)
                    {
                        itemDetail.UnitPrice = detail.CostRate;
                        itemDetail.MinSalePrice = detail.CostRate + 5;
                        itemDetail.MaxSalePrice = detail.CostRate + 10;
                    }
                }
            }

            entity.TenantId = AbpSession.TenantId;
            await IMS_WarehouseStockAdjustment_Repo.InsertAsync(entity);
            await CurrentUnitOfWork.SaveChangesAsync();
            return "IMS_WarehouseStockAdjustment Created Successfully.";
        }

        private async Task<WarehouseStockAdjustmentInfo> Get(long Id)
        {
            var i_ms_warehouse_stock_adjustment = await IMS_WarehouseStockAdjustment_Repo.GetAllIncluding(i => i.WarehouseStockAdjustmentDetails).Where(i => i.Id == Id).FirstOrDefaultAsync();
            if (i_ms_warehouse_stock_adjustment != null)
                return i_ms_warehouse_stock_adjustment;
            else
                throw new UserFriendlyException($"IMS_WarehouseStockAdjustmentId: '{Id}' is invalid.");
        }

        public async Task<IMS_WarehouseStockAdjustmentGetForEditDto> GetForEdit(long Id)
        {
            var i_ms_warehouse_stock_adjustment = await Get(Id);
            var output = ObjectMapper.Map<IMS_WarehouseStockAdjustmentGetForEditDto>(i_ms_warehouse_stock_adjustment);
            var inventory_item_ids = i_ms_warehouse_stock_adjustment.WarehouseStockAdjustmentDetails.Select(i => i.InventoryItemId).ToList();
            var unit_ids = i_ms_warehouse_stock_adjustment.WarehouseStockAdjustmentDetails.Select(i => i.UnitId).ToList();
            var warehouse_ids = i_ms_warehouse_stock_adjustment.WarehouseStockAdjustmentDetails.Select(i => i.WarehouseId).ToList();

            var inventory_items = InventoryItem_Repo.GetAll(this, i => inventory_item_ids.Contains(i.Id)).Future();
            var units = Unit_Repo.GetAll(this, i => unit_ids.Contains(i.Id)).Future();
            var warehouses = Warehouse_Repo.GetAll(this, i => warehouse_ids.Contains(i.Id)).Future();

            var dict_inventory_items = inventory_items.ToDictionary(i => i.Id, i => i.Name);
            var dict_units = units.ToDictionary(i => i.Id, i => i.Name);
            var dict_warehouses = warehouses.ToDictionary(i => i.Id, i => i.Name);

            output.WarehouseStockAdjustmentDetails = new();
            foreach (var detail in i_ms_warehouse_stock_adjustment.WarehouseStockAdjustmentDetails)
            {
                var mapped_detail = ObjectMapper.Map<WarehouseStockAdjustmentDetailsGetForEditDto>(detail);
                mapped_detail.InventoryItemName = dict_inventory_items.GetValueOrDefault(detail.InventoryItemId);
                mapped_detail.UnitName = dict_units.GetValueOrDefault(detail.UnitId);
                mapped_detail.WarehouseName = dict_warehouses.GetValueOrDefault(detail.WarehouseId);
                output.WarehouseStockAdjustmentDetails.Add(mapped_detail);
            }

            return output;
        }

        [AbpAuthorize(PermissionNames.LookUps_IMS_WarehouseStockAdjustment_Edit)]
        public async Task<string> Update(IMS_WarehouseStockAdjustmentGetForEditDto input)
        {
            var old_ims_warehousestockadjustment = await Get(input.Id);
            if (old_ims_warehousestockadjustment.Status != "PENDING")
                throw new UserFriendlyException($"Only entries with a status of 'PENDING' can be edited. The current status is '" + old_ims_warehousestockadjustment.Status + "'.");

            var entity = ObjectMapper.Map(input, old_ims_warehousestockadjustment);
            entity.Status = old_ims_warehousestockadjustment.Status;
            entity.VoucherNumber = await GetVoucherNumber("WSA", input.IssueDate, entity.VoucherNumber.GetVoucherIndex());

            var inventory_item_ids = input.WarehouseStockAdjustmentDetails.Select(i => i.InventoryItemId).ToList();
            var unit_ids = input.WarehouseStockAdjustmentDetails.Select(i => i.UnitId).ToList();
            var warehouse_ids = input.WarehouseStockAdjustmentDetails.Select(i => i.WarehouseId).ToList();

            var inventory_items = InventoryItem_Repo.GetAll(this, i => inventory_item_ids.Contains(i.Id)).Select(i => i.Id).Future();
            var units = Unit_Repo.GetAll(this, i => unit_ids.Contains(i.Id)).Select(i => i.Id).Future();
            var warehouses = Warehouse_Repo.GetAll(this, i => warehouse_ids.Contains(i.Id)).Select(i => i.Id).Future();
            
            _ = await units.ToListAsync();
            _ = await warehouses.ToListAsync();

            for (int i = 0; i < entity.WarehouseStockAdjustmentDetails.Count; i++)
            {
                var detail = entity.WarehouseStockAdjustmentDetails[i];

                if (!inventory_items.Contains(detail.InventoryItemId))
                    throw new UserFriendlyException($"InventoryItemId: '{detail.InventoryItemId}' is invalid at Row: '{i + 1}'.");
                if (!units.Contains(detail.UnitId))
                    throw new UserFriendlyException($"UnitId: '{detail.UnitId}' is invalid at Row: '{i + 1}'.");
                if (!warehouses.Contains(detail.WarehouseId))
                    throw new UserFriendlyException($"WarehouseId: '{detail.WarehouseId}' is invalid at Row: '{i + 1}'.");

                // Update the unit price in ItemDetailsInfo for the item and unit (same as Create)
                var itemInfo = await InventoryItem_Repo.GetAllIncluding(x => x.ItemDetails)
                    .Where(x => x.Id == detail.InventoryItemId)
                    .FirstOrDefaultAsync();
                if (itemInfo != null && itemInfo.ItemDetails != null)
                {
                    var itemDetail = itemInfo.ItemDetails.FirstOrDefault(d => d.UnitId == detail.UnitId);
                    if (itemDetail != null)
                    {
                        itemDetail.UnitPrice = detail.CostRate;
                        itemDetail.MinSalePrice = detail.CostRate + 5;
                        itemDetail.MaxSalePrice = detail.CostRate + 10;
                    }
                }
            }
            await IMS_WarehouseStockAdjustment_Repo.UpdateAsync(entity);
            await CurrentUnitOfWork.SaveChangesAsync();
            return "IMS_WarehouseStockAdjustment Updated Successfully.";
        }
       

        [AbpAuthorize(PermissionNames.LookUps_IMS_WarehouseStockAdjustment_Delete)]
        public async Task<string> Delete(long Id)
        {
            var i_ms_warehouse_stock_adjustment = await IMS_WarehouseStockAdjustment_Repo.GetAll(this, i => i.Id == Id).FirstOrDefaultAsync();
            if (i_ms_warehouse_stock_adjustment == null)
                throw new UserFriendlyException($"IMS_WarehouseStockAdjustmentId: '{Id}' is invalid.");
            if (i_ms_warehouse_stock_adjustment.Status != "PENDING")
                throw new UserFriendlyException($"Unable to delete IMS_WarehouseStockAdjustment: only records with a 'PENDING' status can be deleted.");

            await IMS_WarehouseStockAdjustment_Repo.DeleteAsync(i_ms_warehouse_stock_adjustment);
            await CurrentUnitOfWork.SaveChangesAsync();
            return "IMS_WarehouseStockAdjustment Deleted Successfully.";
        }

        public async Task<decimal> GetMinStockLevel(long itemId, long unitId)
        {
            var item = await InventoryItem_Repo.GetAllIncluding(i => i.ItemDetails)
                .Where(i => i.Id == itemId && i.TenantId == AbpSession.TenantId)
                .FirstOrDefaultAsync();

            if (item == null)
                throw new UserFriendlyException($"ItemId: '{itemId}' is invalid.");

            var itemDetail = item.ItemDetails.FirstOrDefault(d => d.UnitId == unitId);
            if (itemDetail == null)
                throw new UserFriendlyException($"UnitId: '{unitId}' is not associated with the specified item.");

            return itemDetail?.MinStockLevel?? 0;
        }

    }
}
