using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.UI;
using ERP;
using ERP.Authorization;
using ERP.Modules.Finance.ChartOfAccount.COALevel04;
using ERP.Modules.InventoryManagement.Item.Dtos;
using ERP.Modules.InventoryManagement.LookUps;
using ERP.Modules.InventoryManagement.PurchaseInvoice;
using ERP.Modules.InventoryManagement.PurchaseOrder;
using ERP.Modules.SalesManagement.SalesInvoice.Dtos;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace ERP.Modules.InventoryManagement.Item
{
    [AbpAuthorize(PermissionNames.LookUps_Item)]
    public class ItemAppService : ApplicationService
    {
        public IRepository<ItemInfo, long> Item_Repo { get; set; }
        public IRepository<ItemCategoryInfo, long> ItemCategory_Repo { get; set; }
        public IRepository<UnitInfo, long> Unit_Repo { get; set; }
        public IRepository<PurchaseOrderInfo, long> PurchaseOrder_Repo { get; set; }
        public IRepository<PurchaseInvoiceInfo, long> PurchaseInvoice_Repo { get; set; }
        public IRepository<COALevel04Info, long> COALevel04_Repo { get; set; }

        private readonly IWebHostEnvironment _webHostEnvironment;

        public ItemAppService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<PagedResultDto<ItemGetAllDto>> GetAll(ItemFiltersDto filters)
        {
            var item_query = Item_Repo.GetAll(this).ApplyBaseFilters(filters);
            if (!string.IsNullOrWhiteSpace(filters.ItemCategoryId))
                item_query = item_query.Where(i => i.ItemCategoryId == filters.ItemCategoryId.TryToLong());
            var items = await item_query.ToPagedListAsync(filters);

            var coa_level04_ids = items.SelectMany(i => new[] { i.SalesCOALevel04Id, i.PurchaseCOALevel04Id }).Distinct().ToList();
            var item_category_ids = items.Select(i => i.ItemCategoryId).ToList();

            var total_count = item_query.DeferredCount().FutureValue();
            var coa_level04_s = COALevel04_Repo.GetAll(this, i => coa_level04_ids.Contains(i.Id)).Select(i => new { i.Id, i.SerialNumber, i.Name }).Future();
            var item_categories = ItemCategory_Repo.GetAll(this, i => item_category_ids.Contains(i.Id)).Select(i => new { i.Id, i.Name }).Future();
            _ = await item_categories.ToListAsync();

            var dict_item_categories = item_categories.ToDictionary(i => i.Id);
            var dict_coa_level04_s = coa_level04_s.ToDictionary(i => i.Id);

            var output = new List<ItemGetAllDto>();
            foreach (var item in items)
            {
                dict_item_categories.TryGetValue(item.ItemCategoryId, out var item_category);
                dict_coa_level04_s.TryGetValue(item.SalesCOALevel04Id, out var sales_coa_level04);
                dict_coa_level04_s.TryGetValue(item.PurchaseCOALevel04Id, out var purchase_coa_level04);

                var dto = ObjectMapper.Map<ItemGetAllDto>(item);
                dto.ItemCategoryName = item_category?.Name ?? "";
                dto.SalesCOALevel04SerialNumber = sales_coa_level04?.SerialNumber ?? "";
                dto.SalesCOALevel04Name = sales_coa_level04?.Name ?? "";
                dto.PurchaseCOALevel04SerialNumber = purchase_coa_level04?.SerialNumber ?? "";
                dto.PurchaseCOALevel04Name = purchase_coa_level04?.Name ?? "";
                output.Add(dto);
            }

            return new PagedResultDto<ItemGetAllDto>(total_count.Value, output);
        }

        [AbpAuthorize(PermissionNames.LookUps_Item_Create)]
        public async Task<string> Create(ItemDto input)
        {
            var entity = ObjectMapper.Map<ItemInfo>(input);
            var unit_ids = input.ItemDetails.Select(i => i.UnitId).ToList();

            var duplicate = Item_Repo.GetAll(this, i => i.Name == input.Name).DeferredFirstOrDefault().FutureValue();
            var item_category = ItemCategory_Repo.GetAll(this, i => i.Id == input.ItemCategoryId).DeferredFirstOrDefault().FutureValue();
            var units = Unit_Repo.GetAll(this, i => unit_ids.Contains(i.Id)).Select(i => i.Id).Future();
            _ = await units.ToListAsync();

            var duplicate_units = unit_ids.GroupBy(i => i).Where(i => i.Count() > 1).Select(i => i.Key).ToList();
            if (duplicate_units.Any())
                throw new UserFriendlyException($"The following UnitId(s) are selected multiple times: {string.Join(", ", duplicate_units)}. Each unit should be selected only once.");
            if (duplicate.Value != null)
                throw new UserFriendlyException($"An Item with same Name: {input.Name} already exists.");
            if (item_category.Value == null)
                throw new UserFriendlyException($"ItemCategoryId: '{input.ItemCategoryId}' is invalid.");

            for (int i = 0; i < entity.ItemDetails.Count; i++)
            {
                var detail = entity.ItemDetails[i];

                if (!units.Contains(detail.UnitId))
                    throw new UserFriendlyException($"UnitId: '{detail.UnitId}' is invalid at Row: '{i + 1}'.");
            }

            entity.TenantId = AbpSession.TenantId;
            await Item_Repo.InsertAsync(entity);
            await CurrentUnitOfWork.SaveChangesAsync();
            return "Item Created Successfully.";
        }

        private async Task<ItemInfo> GetById(long Id)
        {
            var item = await Item_Repo.GetAllIncluding(i => i.ItemDetails).Where(i => i.Id == Id).FirstOrDefaultAsync();
            if (item != null)
                return item;
            else
                throw new UserFriendlyException($"ItemId: '{Id}' is invalid.");
        }

        public async Task<ItemGetForEditDto> Get(long Id)
        {
            var item = await GetById(Id);
            var unit_ids = item.ItemDetails.Select(i => i.UnitId).ToList();

            var item_category = ItemCategory_Repo.GetAll(this, i => i.Id == item.ItemCategoryId).DeferredFirstOrDefault().FutureValue();
            var sales_coa_level04 = COALevel04_Repo.GetAll(this, i => i.Id == item.SalesCOALevel04Id).DeferredFirstOrDefault().FutureValue();
            var purchase_coa_level04 = COALevel04_Repo.GetAll(this, i => i.Id == item.PurchaseCOALevel04Id).DeferredFirstOrDefault().FutureValue();
            var units = Unit_Repo.GetAll(this, i => unit_ids.Contains(i.Id)).Future();
            _ = await units.ToListAsync();

            var dict_units = units.ToDictionary(i => i.Id, i => i.Name);

            var output = ObjectMapper.Map<ItemGetForEditDto>(item);
            output.ItemCategoryName = item_category?.Value?.Name ?? "";
            output.SalesCOALevel04SerialNumber = sales_coa_level04?.Value?.SerialNumber ?? "";
            output.SalesCOALevel04Name = sales_coa_level04?.Value?.Name ?? "";
            output.PurchaseCOALevel04SerialNumber = purchase_coa_level04?.Value?.SerialNumber ?? "";
            output.PurchaseCOALevel04Name = purchase_coa_level04?.Value?.Name ?? "";
            output.ItemDetails = new();

            foreach (var detail in item.ItemDetails)
            {
                var mapped_detail = ObjectMapper.Map<ItemDetailsGetForEditDto>(detail);
                mapped_detail.UnitName = dict_units.GetValueOrDefault(detail.UnitId);
                output.ItemDetails.Add(mapped_detail);
            }

            return output;
        }

        public async Task<List<GetItemDetailsByCategoryDto>> GetItemDetailsByCategory(long ItemCategoryId)
        {
            var items = Item_Repo.GetAllIncluding(i => i.ItemDetails).Where(i => i.ItemCategoryId == ItemCategoryId && i.TenantId == AbpSession.TenantId).Future();
            var units = Unit_Repo.GetAll(this).Future();
            _ = await units.ToListAsync();

            var dict_units = units.ToDictionary(i => i.Id, i => i.Name);

            var output = new List<GetItemDetailsByCategoryDto>();

            return items.SelectMany(item => item.ItemDetails.Select(item_detail =>
            {
                var dto = ObjectMapper.Map<GetItemDetailsByCategoryDto>(item_detail);
                dto.ItemId = item.Id;
                dto.ItemName = item.Name;
                dto.UnitName = dict_units.GetValueOrDefault(item_detail.UnitId);
                return dto;
            })).ToList();
        }

        [AbpAuthorize(PermissionNames.LookUps_Item_Update)]
        public async Task<string> Update(ItemDto input)
        {
            var old_item = await GetById(input.Id);
            var entity = ObjectMapper.Map(input, old_item);
            var unit_ids = input.ItemDetails.Select(i => i.UnitId).ToList();

            var duplicate = Item_Repo.GetAll(this, i => i.Id != input.Id && i.Name == input.Name).DeferredFirstOrDefault().FutureValue();
            var item_category = ItemCategory_Repo.GetAll(this, i => i.Id == input.ItemCategoryId).DeferredFirstOrDefault().FutureValue();
            var units = Unit_Repo.GetAll(this, i => unit_ids.Contains(i.Id)).Select(i => i.Id).Future();
            _ = await units.ToListAsync();

            var duplicate_units = unit_ids.GroupBy(i => i).Where(g => g.Count() > 1).Select(g => g.Key).ToList();
            if (duplicate_units.Any())
                throw new UserFriendlyException($"The following UnitId(s) are selected multiple times: {string.Join(", ", duplicate_units)}. Each unit should be selected only once.");
            if (duplicate.Value != null)
                throw new UserFriendlyException($"An Item with same Name: {input.Name} already exists.");
            if (item_category.Value == null)
                throw new UserFriendlyException($"ItemCategoryId: '{input.ItemCategoryId}' is invalid.");

            for (int i = 0; i < entity.ItemDetails.Count; i++)
            {
                var detail = entity.ItemDetails[i];

                if (!units.Contains(detail.UnitId))
                    throw new UserFriendlyException($"UnitId: '{detail.UnitId}' is invalid at Row: '{i + 1}'.");
            }

            await Item_Repo.UpdateAsync(entity);
            await CurrentUnitOfWork.SaveChangesAsync();
            return "Item Updated Successfully.";
        }

        public async Task<string> BulkUpdateItemDetails([FromBody] List<BulkUpdateItemDetailsDto> inputs)
        {
            var item_detail_ids = inputs.Select(i => i.Id).ToList();
            var items = await Item_Repo.GetAllIncluding(i => i.ItemDetails).Where(i => i.TenantId == AbpSession.TenantId && i.ItemDetails.Any(d => item_detail_ids.Contains(d.Id))).ToListAsync();

            if (!items.Any())
                throw new UserFriendlyException("No items found for the given details.");

            var dict_item_details = items.SelectMany(i => i.ItemDetails).ToDictionary(i => i.Id);

            foreach (var update_dto in inputs)
            {
                if (dict_item_details.TryGetValue(update_dto.Id, out var item_detail))
                {
                    item_detail.UnitId = update_dto.UnitId;
                    item_detail.UnitPrice = update_dto.UnitPrice;
                    item_detail.MinSalePrice = update_dto.MinSalePrice;
                    item_detail.MaxSalePrice = update_dto.MaxSalePrice;
                    item_detail.MinStockLevel = update_dto.MinStockLevel;
                    item_detail.Barcode = update_dto.Barcode;
                    item_detail.PerBagPrice = update_dto.PerBagPrice;
                }
            }

            await UnitOfWorkManager.Current.SaveChangesAsync();

            return "Item details updated successfully.";
        }

        [AbpAuthorize(PermissionNames.LookUps_Item_Delete)]
        public async Task<string> Delete(long Id)
        {
            var item = await Item_Repo.GetAll(this, i => i.Id == Id).FirstOrDefaultAsync();
            if (item == null)
                throw new UserFriendlyException($"ItemId: '{Id}' is invalid.");

            var has_purchase_orders = PurchaseOrder_Repo.GetAllIncluding(i => i.PurchaseOrderDetails).SelectMany(i => i.PurchaseOrderDetails).DeferredAny(i => i.ItemId == Id).FutureValue();
            var has_purchase_invoices = PurchaseInvoice_Repo.GetAllIncluding(i => i.PurchaseInvoiceDetails).SelectMany(i => i.PurchaseInvoiceDetails).DeferredAny(i => i.ItemId == Id).FutureValue();
            _ = await has_purchase_invoices.ValueAsync();

            if (has_purchase_orders.Value)
                throw new UserFriendlyException("This Item is linked to PurchaseOrders and cannot be deleted.");
            if (has_purchase_invoices.Value)
                throw new UserFriendlyException("This Item is linked to PurchaseInvoices and cannot be deleted.");

            await Item_Repo.DeleteAsync(item);
            await CurrentUnitOfWork.SaveChangesAsync();
            return "Item Deleted Successfully.";
        }

        [AbpAuthorize(PermissionNames.LookUps_Item_Create)]
        public async Task<ItemBulkUploadResultDto> BulkUpload(ItemBulkUploadRequestDto input)
        {
            var result = new ItemBulkUploadResultDto
            {
                TotalItems = input?.Items?.Count ?? 0
            };

            if (input?.Items == null || input.Items.Count == 0)
            {
                result.Errors.Add("No items provided for bulk upload");
                await WriteErrorsToFileAsync(result);
                return result;
            }

            // Check duplicates inside the provided list (by Name)
            var duplicateNamesInList = input.Items
                .Where(i => !string.IsNullOrWhiteSpace(i.Name))
                .GroupBy(i => i.Name.Trim().ToLower())
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();

            foreach (var dup in duplicateNamesInList)
                result.Errors.Add($"Duplicate item '{dup}' found in the upload list.");

            // Fetch all required lookup data for efficient processing
            var itemCategoriesQuery = await ItemCategory_Repo.GetAll(this)
                .Select(ic => new { ic.Id, ic.Name })
                .ToListAsync();

            var itemCategories = itemCategoriesQuery
                .GroupBy(ic => ic.Name.ToLower())
                .ToDictionary(g => g.Key, g => g.First().Id);

            var unitsQuery = await Unit_Repo.GetAll(this)
                .Select(u => new { u.Id, u.Name })
                .ToListAsync();

            var units = unitsQuery
                .GroupBy(u => u.Name.ToLower())
                .ToDictionary(g => g.Key, g => g.First().Id);

            var coaLevel04sQuery = await COALevel04_Repo.GetAll(this)
                .Select(c => new { c.Id, c.Name })
                .ToListAsync();

            var coaLevel04s = coaLevel04sQuery
                .GroupBy(c => c.Name.ToLower())
                .ToDictionary(g => g.Key, g => g.First().Id);

            var existingItemNames = await Item_Repo.GetAll(this)
                .Where(i => i.TenantId == AbpSession.TenantId)
                .Select(i => i.Name.ToLower())
                .ToListAsync();

            var preparedNewItems = new List<ItemInfo>();

            // Validate and prepare entities (no DB writes yet)
            for (int index = 0; index < input.Items.Count; index++)
            {
                var item = input.Items[index];
                var rowNumber = index + 1;

                // Required fields
                if (string.IsNullOrWhiteSpace(item.Name))
                    result.Errors.Add($"Row {rowNumber}: Name is required");
                if (string.IsNullOrWhiteSpace(item.ItemCategoryName))
                    result.Errors.Add($"Row {rowNumber}: ItemCategoryName is required");
                if (string.IsNullOrWhiteSpace(item.SalesCOALevel04Name))
                    result.Errors.Add($"Row {rowNumber}: SalesCOALevel04Name is required");
                if (string.IsNullOrWhiteSpace(item.PurchaseCOALevel04Name))
                    result.Errors.Add($"Row {rowNumber}: PurchaseCOALevel04Name is required");

                if (item.ItemDetails == null || item.ItemDetails.Count == 0)
                    result.Errors.Add($"Row {rowNumber}: ItemDetails are required");

                // If any of the above are missing, skip further validation for this row
                if (string.IsNullOrWhiteSpace(item.Name) ||
                    string.IsNullOrWhiteSpace(item.ItemCategoryName) ||
                    string.IsNullOrWhiteSpace(item.SalesCOALevel04Name) ||
                    string.IsNullOrWhiteSpace(item.PurchaseCOALevel04Name) ||
                    item.ItemDetails == null || item.ItemDetails.Count == 0)
                    continue;

                // Duplicates in DB
                if (existingItemNames.Contains(item.Name.Trim().ToLower()))
                    result.Errors.Add($"Row {rowNumber}: Item '{item.Name}' already exists in the database");

                // Resolve references
                if (!itemCategories.TryGetValue(item.ItemCategoryName.Trim().ToLower(), out var itemCategoryId))
                    result.Errors.Add($"Row {rowNumber}: ItemCategory '{item.ItemCategoryName}' not found");

                if (!coaLevel04s.TryGetValue(item.SalesCOALevel04Name.Trim().ToLower(), out var salesCOALevel04Id))
                    result.Errors.Add($"Row {rowNumber}: SalesCOALevel04 '{item.SalesCOALevel04Name}' not found");

                if (!coaLevel04s.TryGetValue(item.PurchaseCOALevel04Name.Trim().ToLower(), out var purchaseCOALevel04Id))
                    result.Errors.Add($"Row {rowNumber}: PurchaseCOALevel04 '{item.PurchaseCOALevel04Name}' not found");

                // Duplicate unit names within details
                var unitNames = item.ItemDetails.Select(d => (d.UnitName ?? string.Empty).Trim().ToLower()).ToList();
                var duplicateUnits = unitNames.GroupBy(u => u).Where(g => !string.IsNullOrWhiteSpace(g.Key) && g.Count() > 1).Select(g => g.Key).ToList();
                if (duplicateUnits.Any())
                    result.Errors.Add($"Row {rowNumber}: Duplicate units found: {string.Join(", ", duplicateUnits)}");

                // Validate each detail unit
                bool detailsValid = true;
                var preparedDetails = new List<ItemDetailsInfo>();
                foreach (var detail in item.ItemDetails)
                {
                    if (string.IsNullOrWhiteSpace(detail.UnitName))
                    {
                        result.Errors.Add($"Row {rowNumber}: A detail has empty UnitName");
                        detailsValid = false;
                        break;
                    }

                    if (!units.TryGetValue(detail.UnitName.Trim().ToLower(), out var unitId))
                    {
                        result.Errors.Add($"Row {rowNumber}: Unit '{detail.UnitName}' not found");
                        detailsValid = false;
                        break;
                    }

                    preparedDetails.Add(new ItemDetailsInfo
                    {
                        UnitId = unitId,
                        UnitPrice = detail.UnitPrice,
                        MinSalePrice = detail.MinSalePrice,
                        MaxSalePrice = detail.MaxSalePrice,
                        MinStockLevel = detail.MinStockLevel,
                        Barcode = detail.Barcode,
                        PerBagPrice = detail.PerBagPrice
                    });
                }

                // If this row had any mapping errors, skip preparing entity
                if (!detailsValid ||
                    !itemCategories.ContainsKey(item.ItemCategoryName.Trim().ToLower()) ||
                    !coaLevel04s.ContainsKey(item.SalesCOALevel04Name.Trim().ToLower()) ||
                    !coaLevel04s.ContainsKey(item.PurchaseCOALevel04Name.Trim().ToLower()))
                    continue;

                // Prepare entity for insertion
                var newItem = new ItemInfo
                {
                    Name = item.Name,
                    Description = item.Description,
                    ItemCategoryId = itemCategories[item.ItemCategoryName.Trim().ToLower()],
                    IsDiscountable = item.IsDiscountable,
                    DiscountAmount = item.DiscountAmount,
                    DiscountPercentage = item.DiscountPercentage,
                    SalesCOALevel04Id = coaLevel04s[item.SalesCOALevel04Name.Trim().ToLower()],
                    PurchaseCOALevel04Id = coaLevel04s[item.PurchaseCOALevel04Name.Trim().ToLower()],
                    TenantId = AbpSession.TenantId,
                    ItemDetails = preparedDetails
                };

                preparedNewItems.Add(newItem);
            }

            if (result.Errors.Count > 0)
            {
                result.SuccessCount = 0;
                result.FailureCount = result.TotalItems;
                await WriteErrorsToFileAsync(result);
                return result;
            }

            // All validations passed -> transactionally insert all items
            try
            {
                using (var uow = UnitOfWorkManager.Begin())
                {
                    foreach (var newItem in preparedNewItems)
                    {
                        await Item_Repo.InsertAsync(newItem);
                    }
                    await uow.CompleteAsync();
                }

                result.SuccessCount = result.TotalItems;
                result.FailureCount = 0;
                return result;
            }
            catch (System.Exception ex)
            {
                result.Errors.Add($"Bulk upload failed: {ex.Message}");
                result.SuccessCount = 0;
                result.FailureCount = result.TotalItems;
                await WriteErrorsToFileAsync(result);
                return result;
            }
        }

        private static async Task WriteErrorsToFileAsync(ItemBulkUploadResultDto result)
        {
            try
            {
                var baseDirectory = Directory.GetCurrentDirectory();
                var errorsDirectory = Path.Combine(baseDirectory, "wwwroot", "bulk-upload-errors");
                if (!Directory.Exists(errorsDirectory))
                    Directory.CreateDirectory(errorsDirectory);

                var fileName = $"item-errors-{DateTime.Now:yyyyMMddHHmmssfff}.txt";
                var fullPath = Path.Combine(errorsDirectory, fileName);
                await File.WriteAllLinesAsync(fullPath, result.Errors ?? new List<string> { "Unknown error" });

                result.ErrorFilePath = $"/bulk-upload-errors/{fileName}";
            }
            catch
            {
                // Ignore file write errors
            }
        }

        public async Task<decimal> GetMinStockLevel(long itemId, long unitId)
        {
            var item = await Item_Repo.GetAllIncluding(i => i.ItemDetails)
                .Where(i => i.Id == itemId && i.TenantId == AbpSession.TenantId)
                .FirstOrDefaultAsync();

            if (item == null)
                throw new UserFriendlyException($"ItemId: '{itemId}' is invalid.");

            var itemDetail = item.ItemDetails.FirstOrDefault(d => d.UnitId == unitId);

            return itemDetail?.MinStockLevel ?? 0;
        }

        public async Task<List<ItemUnitDto>> GetItemUnits(long itemId)
        {
            var item = await Item_Repo.GetAllIncluding(i => i.ItemDetails)
                .Where(i => i.Id == itemId && i.TenantId == AbpSession.TenantId)
                .FirstOrDefaultAsync();

            if (item == null)
                throw new UserFriendlyException($"ItemId: '{itemId}' is invalid.");

            var unitIds = item.ItemDetails.Select(d => d.UnitId).ToList();
            var units = await Unit_Repo.GetAll(this)
                .Where(u => unitIds.Contains(u.Id))
                .Select(u => new { u.Id, u.Name })
                .ToListAsync();

            var unitDict = units.ToDictionary(u => u.Id, u => u.Name);

            var result = new List<ItemUnitDto>();
            foreach (var detail in item.ItemDetails)
            {
                result.Add(new ItemUnitDto
                {
                    UnitId = detail.UnitId,
                    UnitName = unitDict.GetValueOrDefault(detail.UnitId, ""),
                    
                });
            }
            return result;
        }

        [AbpAuthorize(PermissionNames.LookUps_Item_Update)]
        public async Task<DocumentUploadResultDto> UploadDocuments(DocumentUploadDto input)
        {
            if (input.Base64Images == null || input.Base64Images.Count == 0)
                throw new UserFriendlyException("No images were provided for upload.");

            string documentDirectory = "product-images";
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
    }
}
