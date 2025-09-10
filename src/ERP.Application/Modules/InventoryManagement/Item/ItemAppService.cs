using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.UI;
using ERP.Authorization;
using ERP.Modules.InventoryManagement.LookUps;
using ERP.Modules.InventoryManagement.Vendor;
using Microsoft.EntityFrameworkCore;
using ERP.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace ERP.Modules.InventoryManagement.Item
{
    [AbpAuthorize(PermissionNames.LookUps_Item)]
    public class ItemAppService : ApplicationService
    {
        public IRepository<ItemInfo, long> IMS_Item_Repo { get; set; }
        public IRepository<ItemCategoryInfo, long> Category_Repo { get; set; }
        public IRepository<VendorInfo, long> Vendor_Repo { get; set; }

        public async Task<PagedResultDto<IMS_ItemGetAllDto>> GetAll(IMS_ItemFiltersDto filters)
        {
            var i_ms_item_query = IMS_Item_Repo.GetAll(this).ApplyBaseFilters(filters);
            if (!string.IsNullOrWhiteSpace(filters.Id))
                i_ms_item_query = i_ms_item_query.Where(i => i.Id == filters.Id.TryToLong());
            if (!string.IsNullOrWhiteSpace(filters.StatusId))
                i_ms_item_query = i_ms_item_query.Where(i => i.StatusId == filters.StatusId.TryToLong());
            if (!string.IsNullOrWhiteSpace(filters.CategoryId))
                i_ms_item_query = i_ms_item_query.Where(i => i.CategoryId == filters.CategoryId.TryToLong());
            if (!string.IsNullOrWhiteSpace(filters.VendorId))
                i_ms_item_query = i_ms_item_query.Where(i => i.VendorId == filters.VendorId.TryToLong());
            var i_ms_items = await i_ms_item_query.ToPagedListAsync(filters);

            var category_ids = i_ms_items.Select(i => i.CategoryId).ToList();
            var vendor_ids = i_ms_items.Select(i => i.VendorId).ToList();

            var total_count = i_ms_item_query.DeferredCount().FutureValue();
            var categories = Category_Repo.GetAll(this, i => category_ids.Contains(i.Id)).Future();
            var vendors = Vendor_Repo.GetAll(this, i => vendor_ids.Contains(i.Id)).Future();
            await vendors.ToListAsync();

            var dict_categories = categories.ToDictionary(i => i.Id);
            var dict_vendors = vendors.ToDictionary(i => i.Id);

            var output = new List<IMS_ItemGetAllDto>();
            foreach (var i_ms_item in i_ms_items)
            {
                dict_categories.TryGetValue(i_ms_item.CategoryId, out var category);
                dict_vendors.TryGetValue(i_ms_item.VendorId, out var vendor);

                var dto = ObjectMapper.Map<IMS_ItemGetAllDto>(i_ms_item);
                dto.StatusName = GetStatusName(i_ms_item.StatusId);
                dto.CategoryName = category?.Name ?? "";
                dto.VendorName = vendor?.Name ?? "";
                output.Add(dto);
            }

            return new PagedResultDto<IMS_ItemGetAllDto>(total_count.Value, output);
        }

        [AbpAuthorize(PermissionNames.LookUps_Item_Create)]
        public async Task<string> Create(IMS_ItemDto input)
        {
            var category = Category_Repo.GetAll(this, i => i.Id == input.CategoryId).DeferredFirstOrDefault().FutureValue();
            var vendor = Vendor_Repo.GetAll(this, i => i.Id == input.VendorId).DeferredFirstOrDefault().FutureValue();
            await vendor.ValueAsync();

            if (!System.Enum.IsDefined(typeof(Status), (int)input.StatusId))
                throw new UserFriendlyException($"StatusId: '{input.StatusId}' is invalid.");
            if (category.Value == null)
                throw new UserFriendlyException($"CategoryId: '{input.CategoryId}' is invalid.");
            if (vendor.Value == null)
                throw new UserFriendlyException($"VendorId: '{input.VendorId}' is invalid.");

            var entity = ObjectMapper.Map<ItemInfo>(input);
            entity.Status = GetStatusName(input.StatusId);
            entity.TenantId = AbpSession.TenantId;
            await IMS_Item_Repo.InsertAsync(entity);
            await CurrentUnitOfWork.SaveChangesAsync();
            return "IMS_Item Created Successfully.";
        }

        public async Task<ItemInfo> Get(long Id)
        {
            var i_ms_item = await IMS_Item_Repo.GetAll(this, i => i.Id == Id).FirstOrDefaultAsync();
            if (i_ms_item != null)
                return i_ms_item;
            else
                throw new UserFriendlyException($"IMS_ItemId is '{Id}' invalid.");
        }

        public async Task<IMS_ItemGetAllDto> GetForEdit(long Id)
        {
            var i_ms_item = await Get(Id);
            var category = Category_Repo.GetAll(this, i => i.Id == i_ms_item.CategoryId).DeferredFirstOrDefault().FutureValue();
            var vendor = Vendor_Repo.GetAll(this, i => i.Id == i_ms_item.VendorId).DeferredFirstOrDefault().FutureValue();
            await vendor.ValueAsync();

            var output = ObjectMapper.Map<IMS_ItemGetAllDto>(i_ms_item);
            output.StatusName = GetStatusName(i_ms_item.StatusId);
            output.CategoryName = category?.Value?.Name ?? "";
            output.VendorName = vendor?.Value?.Name ?? "";
            return output;
        }

        [AbpAuthorize(PermissionNames.LookUps_Item_Edit)]
        public async Task<string> Edit(IMS_ItemDto input)
        {
            var category = Category_Repo.GetAll(this, i => i.Id == input.CategoryId).DeferredFirstOrDefault().FutureValue();
            var vendor = Vendor_Repo.GetAll(this, i => i.Id == input.VendorId).DeferredFirstOrDefault().FutureValue();
            await vendor.ValueAsync();

            if (!System.Enum.IsDefined(typeof(Status), (int)input.StatusId))
                throw new UserFriendlyException($"StatusId: '{input.StatusId}' is invalid.");
            if (category.Value == null)
                throw new UserFriendlyException($"CategoryId: '{input.CategoryId}' is invalid.");
            if (vendor.Value == null)
                throw new UserFriendlyException($"VendorId: '{input.VendorId}' is invalid.");

            var olditem = await Get(input.Id);  
            var entity = ObjectMapper.Map(input, olditem);
            entity.Status = GetStatusName(input.StatusId);
            await IMS_Item_Repo.UpdateAsync(entity);
            await CurrentUnitOfWork.SaveChangesAsync();
            return "IMS_Item Updated Successfully.";
        }

        [AbpAuthorize(PermissionNames.LookUps_Item_Delete)]
        public async Task<string> Delete(long Id)
        {
            var i_ms_item = await IMS_Item_Repo.GetAll(this, i => i.Id == Id).FirstOrDefaultAsync();
            if (i_ms_item == null)
                throw new UserFriendlyException($"IMS_ItemId is '{Id}' invalid.");

            await IMS_Item_Repo.DeleteAsync(i_ms_item);
            await CurrentUnitOfWork.SaveChangesAsync();
            return "IMS_Item Deleted Successfully.";
        }

        private string GetStatusName(long statusId)
        {
            return System.Enum.IsDefined(typeof(Status), (int)statusId)
                ? ((Status)statusId).ToString()
                : string.Empty;
        }
    }
}
