using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.UI;
using ERP.Authorization;
using ERP.Generics;
using ERP.Generics.Simple;
using ERP.Modules.InventoryManagement.Item;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.Modules.InventoryManagement.LookUps
{
    [AbpAuthorize(PermissionNames.LookUps_ItemCategory)]
    public class ItemCategoryAppService : GenericSimpleAppService<ItemCategoryDto, ItemCategoryInfo, SimpleSearchDtoBase>
    {
        public IRepository<ItemInfo, long> Item_Repo { get; set; }

        public override PagedResultDto<ItemCategoryDto> GetAll(SimpleSearchDtoBase search)
        {
            return base.GetAll(search);
        }

        public override async Task<ItemCategoryDto> Create(ItemCategoryDto input)
        {
            return await base.Create(input);
        }

        public override ItemCategoryDto Get(long Id)
        {
            return base.Get(Id);
        }

        public override async Task<ItemCategoryDto> Update(ItemCategoryDto input)
        {
            return await base.Update(input);
        }

        public override async Task<string> Delete(EntityDto<long> input)
        {
            var has_linked = await Item_Repo.GetAll(this).AnyAsync(i => i.ItemCategoryId == input.Id);

            if (has_linked)
                throw new UserFriendlyException("This category is linked to Items and cannot be deleted.");

            return await base.Delete(input);
        }

        [AbpAuthorize(PermissionNames.LookUps_ItemCategory_Create)]
        public async Task<BulkUploadResultDto> BulkUpload(List<ItemCategoryDto> inputs)
        {
            var result = new BulkUploadResultDto
            {
                TotalCount = inputs?.Count ?? 0,
                SuccessCount = 0,
                FailedCount = 0,
                Errors = new List<string>()
            };

            if (inputs == null || inputs.Count == 0)
            {
                result.Errors.Add("No data provided for bulk upload.");
                result.FailedCount = result.TotalCount;
                await WriteErrorsToFileAsync(result);
                return result;
            }

            // Validate entries first (all-or-nothing behavior)
            // 1) Empty/invalid names
            var emptyNameRows = inputs
                .Select((x, idx) => new { x.Name, Index = idx + 1 })
                .Where(x => string.IsNullOrWhiteSpace(x.Name))
                .ToList();

            if (emptyNameRows.Any())
            {
                foreach (var row in emptyNameRows)
                {
                    result.Errors.Add($"Row {row.Index}: Name is required.");
                }
            }

            // 2) Duplicates within the input list
            var duplicatesInList = inputs
                .Where(x => !string.IsNullOrWhiteSpace(x.Name))
                .GroupBy(x => x.Name.Trim().ToLower())
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();

            if (duplicatesInList.Any())
            {
                foreach (var duplicate in duplicatesInList)
                {
                    result.Errors.Add($"Duplicate category '{duplicate}' found in the upload list.");
                }
            }

            // 3) Duplicates in database for current tenant
            var existingNames = MainRepository.GetAll()
                .Where(x => x.TenantId == AbpSession.TenantId)
                .Select(x => x.Name.ToLower())
                .ToList();

            var duplicatesInDb = inputs
                .Where(x => !string.IsNullOrWhiteSpace(x.Name))
                .Where(x => existingNames.Contains(x.Name.Trim().ToLower()))
                .Select(x => x.Name)
                .ToList();

            if (duplicatesInDb.Any())
            {
                foreach (var duplicate in duplicatesInDb)
                {
                    result.Errors.Add($"Category '{duplicate}' already exists in the database.");
                }
            }

            // If any errors were collected, fail the entire batch and return an error file
            if (result.Errors.Count > 0)
            {
                result.SuccessCount = 0;
                result.FailedCount = result.TotalCount;
                await WriteErrorsToFileAsync(result);
                return result;
            }

            // No validation errors -> perform transactional insert (all-or-nothing)
            try
            {
                using (var uow = UnitOfWorkManager.Begin())
                {
                    foreach (var input in inputs)
                    {
                        var entity = ObjectMapper.Map<ItemCategoryInfo>(input);
                        entity.TenantId = AbpSession.TenantId;
                        await MainRepository.InsertAsync(entity);
                    }

                    await uow.CompleteAsync();
                }

                result.SuccessCount = result.TotalCount;
                result.FailedCount = 0;
                return result;
            }
            catch (Exception ex)
            {
                // Roll back (uow not completed) and return errors file
                result.Errors.Add($"Bulk upload failed: {ex.Message}");
                result.SuccessCount = 0;
                result.FailedCount = result.TotalCount;
                await WriteErrorsToFileAsync(result);
                return result;
            }
        }

        private static async Task WriteErrorsToFileAsync(BulkUploadResultDto result)
        {
            try
            {
                var baseDirectory = Directory.GetCurrentDirectory();
                var errorsDirectory = Path.Combine(baseDirectory, "wwwroot", "bulk-upload-errors");
                if (!Directory.Exists(errorsDirectory))
                    Directory.CreateDirectory(errorsDirectory);

                var fileName = $"item-category-errors-{DateTime.Now:yyyyMMddHHmmssfff}.txt";
                var fullPath = Path.Combine(errorsDirectory, fileName);
                await File.WriteAllLinesAsync(fullPath, result.Errors ?? new List<string> { "Unknown error" });

                result.ErrorFilePath = $"/bulk-upload-errors/{fileName}";
            }
            catch
            {
                // Ignore file write errors
            }
        }
    }

    [AutoMap(typeof(ItemCategoryInfo))]
    public class ItemCategoryDto : SimpleDtoBase
    {

    }

    public class BulkUploadResultDto
    {
        public int TotalCount { get; set; }
        public int SuccessCount { get; set; }
        public int FailedCount { get; set; }
        public List<string> Errors { get; set; }
        public string ErrorFilePath { get; set; }
    }
}
