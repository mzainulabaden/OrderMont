using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.UI;
using ERP.Authorization;
using ERP.Enums;
using ERP.Modules.InventoryManagement.Item;
using ERP.Modules.InventoryManagement.Vendor;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace ERP.Modules.InventoryManagement.PurchaseInvoice
{
    [AbpAuthorize(PermissionNames.LookUps_IMS_PurchaseInvoice)]
    public class PurchaseInvoiceAppService : ERPDocumentService<PurchaseInvoiceInfo>
    {
        public IRepository<PurchaseInvoiceInfo, long> IMS_PurchaseInvoice_Repo { get; set; }
        public IRepository<VendorInfo, long> Vendor_Repo { get; set; }
        public IRepository<ItemInfo, long> Item_Repo { get; set; }

        public async Task<PagedResultDto<IMS_PurchaseInvoiceGetAllDto>> GetAll(IMS_PurchaseInvoiceFiltersDto filters)
        {
            var i_ms_purchase_invoice_query = IMS_PurchaseInvoice_Repo.GetAll(this);
            if (!string.IsNullOrWhiteSpace(filters.Id))
                i_ms_purchase_invoice_query = i_ms_purchase_invoice_query.Where(i => i.Id == filters.Id.TryToLong());
            if (!string.IsNullOrWhiteSpace(filters.VendorId))
                i_ms_purchase_invoice_query = i_ms_purchase_invoice_query.Where(i => i.VendorId == filters.VendorId.TryToLong());
            if (!string.IsNullOrWhiteSpace(filters.StatusId))
                i_ms_purchase_invoice_query = i_ms_purchase_invoice_query.Where(i => i.StatusId == filters.StatusId.TryToLong());
            var i_ms_purchase_invoices = await i_ms_purchase_invoice_query.ToPagedListAsync(filters);

            var vendor_ids = i_ms_purchase_invoices.Select(i => i.VendorId).ToList();
            var status_ids = i_ms_purchase_invoices.Select(i => i.StatusId).ToList();

            var total_count = i_ms_purchase_invoice_query.DeferredCount().FutureValue();
            var vendors = Vendor_Repo.GetAll(this, i => vendor_ids.Contains(i.Id)).Future();

            var dict_vendors = vendors.ToDictionary(i => i.Id);
            // status names will be derived from enum `Status`

            var output = new List<IMS_PurchaseInvoiceGetAllDto>();
            foreach (var i_ms_purchase_invoice in i_ms_purchase_invoices)
            {
                dict_vendors.TryGetValue(i_ms_purchase_invoice.VendorId, out var vendor);
                // map status name from enum

                var dto = ObjectMapper.Map<IMS_PurchaseInvoiceGetAllDto>(i_ms_purchase_invoice);
                dto.VendorName = vendor?.Name ?? "";
                dto.StatusName = System.Enum.IsDefined(typeof(Status), (int)i_ms_purchase_invoice.StatusId)
                    ? ((Status)(int)i_ms_purchase_invoice.StatusId).ToString()
                    : "";
                output.Add(dto);
            }

            return new PagedResultDto<IMS_PurchaseInvoiceGetAllDto>(total_count.Value, output);
        }

        [AbpAuthorize(PermissionNames.LookUps_IMS_PurchaseInvoice_Create)]
        public async Task<string> Create(IMS_PurchaseInvoiceDto input)
        {
            var vendor = Vendor_Repo.GetAll(this, i => i.Id == input.VendorId).DeferredFirstOrDefault().FutureValue();
            await vendor.ValueAsync();

            if (vendor.Value == null)
                throw new UserFriendlyException($"VendorId: '{input.VendorId}' is invalid.");
            if (!System.Enum.IsDefined(typeof(Status), (int)input.StatusId))
                throw new UserFriendlyException($"StatusId: '{input.StatusId}' is invalid.");

            var entity = ObjectMapper.Map<PurchaseInvoiceInfo>(input);
            entity.Status = "PENDING";
            entity.VoucherNumber = await GetVoucherNumber("UA", input.IssueDate);

            var item_ids = input.IMS_PurchaseInvoiceDetails.Select(i => i.ItemId).ToList();

            var items = Item_Repo.GetAll(this, i => item_ids.Contains(i.Id)).Select(i => i.Id).Future();
            _ = await items.ToListAsync();

            for (int i = 0; i < entity.PurchaseInvoiceDetails.Count; i++)
            {
                var detail = entity.PurchaseInvoiceDetails[i];

                if (!items.Contains(detail.ItemId))
                    throw new UserFriendlyException($"ItemId: '{detail.ItemId}' is invalid at Row: '{i + 1}'.");

                entity.PurchaseInvoiceDetails[i].VoucherNumber = $"{entity.VoucherNumber}/{i + 1}";
            }

            entity.TenantId = AbpSession.TenantId;
            await IMS_PurchaseInvoice_Repo.InsertAsync(entity);
            await CurrentUnitOfWork.SaveChangesAsync();
            return "IMS_PurchaseInvoice Created Successfully.";
        }

        private async Task<PurchaseInvoiceInfo> Get(long Id)
        {
            var i_ms_purchase_invoice = await IMS_PurchaseInvoice_Repo.GetAllIncluding(i => i.PurchaseInvoiceDetails).Where(i => i.Id == Id).FirstOrDefaultAsync();
            if (i_ms_purchase_invoice != null)
                return i_ms_purchase_invoice;
            else
                throw new UserFriendlyException($"IMS_PurchaseInvoiceId: '{Id}' is invalid.");
        }

        public async Task<IMS_PurchaseInvoiceGetForEditDto> GetForEdit(long Id)
        {
            var i_ms_purchase_invoice = await Get(Id);
            var vendor = Vendor_Repo.GetAll(this, i => i.Id == i_ms_purchase_invoice.VendorId).DeferredFirstOrDefault().FutureValue();
            await vendor.ValueAsync();

            var output = ObjectMapper.Map<IMS_PurchaseInvoiceGetForEditDto>(i_ms_purchase_invoice);
            output.VendorName = vendor?.Value?.Name ?? "";
            output.StatusName = System.Enum.IsDefined(typeof(Status), (int)i_ms_purchase_invoice.StatusId)
                ? ((Status)(int)i_ms_purchase_invoice.StatusId).ToString()
                : "";

            var item_ids = i_ms_purchase_invoice.PurchaseInvoiceDetails.Select(i => i.ItemId).ToList();

            var items = Item_Repo.GetAll(this, i => item_ids.Contains(i.Id)).Future();

            var dict_items = items.ToDictionary(i => i.Id, i => i.Name);

            output.IMS_PurchaseInvoiceDetails = new();
            foreach (var detail in i_ms_purchase_invoice.PurchaseInvoiceDetails)
            {
                var mapped_detail = ObjectMapper.Map<IMS_PurchaseInvoiceDetailsGetForEditDto>(detail);
                mapped_detail.ItemName = dict_items.GetValueOrDefault(detail.ItemId);
                output.IMS_PurchaseInvoiceDetails.Add(mapped_detail);
            }

            return output;
        }

        [AbpAuthorize(PermissionNames.LookUps_IMS_PurchaseInvoice_Edit)]
        public async Task<string> Edit(IMS_PurchaseInvoiceDto input)
        {
            var old_ims_purchaseinvoice = await Get(input.Id);
            if (old_ims_purchaseinvoice.Status != "PENDING")
                throw new UserFriendlyException($"Only entries with a status of 'PENDING' can be edited. The current status is '" + old_ims_purchaseinvoice.Status + "'.");

            var vendor = Vendor_Repo.GetAll(this, i => i.Id == input.VendorId).DeferredFirstOrDefault().FutureValue();
            await vendor.ValueAsync();

            if (vendor.Value == null)
                throw new UserFriendlyException($"VendorId: '{input.VendorId}' is invalid.");
            if (!System.Enum.IsDefined(typeof(Status), (int)input.StatusId))
                throw new UserFriendlyException($"StatusId: '{input.StatusId}' is invalid.");

            var entity = ObjectMapper.Map(input, old_ims_purchaseinvoice);
            entity.VoucherNumber = await GetVoucherNumber("UA", input.IssueDate, entity.VoucherNumber.GetVoucherIndex());

            var item_ids = input.IMS_PurchaseInvoiceDetails.Select(i => i.ItemId).ToList();

            var items = Item_Repo.GetAll(this, i => item_ids.Contains(i.Id)).Select(i => i.Id).Future();
            _ = await items.ToListAsync();

            for (int i = 0; i < entity.PurchaseInvoiceDetails.Count; i++)
            {
                var detail = entity.PurchaseInvoiceDetails[i];

                if (!items.Contains(detail.ItemId))
                    throw new UserFriendlyException($"ItemId: '{detail.ItemId}' is invalid at Row: '{i + 1}'.");

                entity.PurchaseInvoiceDetails[i].VoucherNumber = $"{entity.VoucherNumber}/{i + 1}";
            }

            await IMS_PurchaseInvoice_Repo.UpdateAsync(entity);
            await CurrentUnitOfWork.SaveChangesAsync();
            return "IMS_PurchaseInvoice Updated Successfully.";
        }

        [AbpAuthorize(PermissionNames.LookUps_IMS_PurchaseInvoice_Delete)]
        public async Task<string> Delete(long Id)
        {
            var i_ms_purchase_invoice = await IMS_PurchaseInvoice_Repo.GetAll(this, i => i.Id == Id).FirstOrDefaultAsync();
            if (i_ms_purchase_invoice == null)
                throw new UserFriendlyException($"IMS_PurchaseInvoiceId: '{Id}' is invalid.");
            if (i_ms_purchase_invoice.Status != "PENDING")
                throw new UserFriendlyException($"Unable to delete IMS_PurchaseInvoice: only records with a 'PENDING' status can be deleted.");

            await IMS_PurchaseInvoice_Repo.DeleteAsync(i_ms_purchase_invoice);
            await CurrentUnitOfWork.SaveChangesAsync();
            return "IMS_PurchaseInvoice Deleted Successfully.";
        }
    }
}
