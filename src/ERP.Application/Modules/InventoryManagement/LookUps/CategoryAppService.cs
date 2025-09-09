using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.AutoMapper;
using Abp.Domain.Entities;
using Abp.Authorization;
using ERP.Authorization;
using ERP.Generics;
using ERP.Generics.Simple;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace ERP.Modules.InventoryManagement.LookUps
{
    [AbpAuthorize(PermissionNames.LookUps_IMS_ItemCategory)]
    public class CategoryAppService : GenericSimpleAppService<IMS_ItemCategoryDto, ItemCategoryInfo, SimpleSearchDtoBase>
    {
        public async Task<IMS_ItemCategoryBulkUploadResultDto> BulkUpload(IMS_ItemCategoryBulkUploadRequestDto input)
        {
            CheckCreatePermission();

            var result = new IMS_ItemCategoryBulkUploadResultDto
            {
                TotalItems = input?.Items?.Count ?? 0
            };

            if (input?.Items == null || input.Items.Count == 0)
            {
                result.Errors.Add("No items provided for bulk upload");
                result.FailureCount = result.TotalItems;
                await WriteErrorsToFileAsync(result);
                return result;
            }

            var tenantId = AbpSession.TenantId;
            var existingNames = await MainRepository.GetAll()
                .Where(i => i.TenantId == tenantId)
                .Select(i => i.Name.ToLower())
                .ToListAsync();

            var existingNameSet = new HashSet<string>(existingNames);
            var inFileNameSet = new HashSet<string>();

            // Validate all rows first
            for (int index = 0; index < input.Items.Count; index++)
            {
                var item = input.Items[index];
                var rowNumber = index + 1;
                if (string.IsNullOrWhiteSpace(item.Name))
                {
                    result.Errors.Add($"Row {rowNumber}: Name is required");
                    result.FailureCount++;
                    continue;
                }

                var nameTrimmed = item.Name.Trim();
                var nameLower = nameTrimmed.ToLower();

                if (inFileNameSet.Contains(nameLower))
                {
                    result.Errors.Add($"Row {rowNumber}: Duplicate Name '{item.Name}' in file");
                    result.FailureCount++;
                    continue;
                }

                if (existingNameSet.Contains(nameLower))
                {
                    result.Errors.Add($"Row {rowNumber}: Name '{item.Name}' already exists");
                    result.FailureCount++;
                    continue;
                }

                inFileNameSet.Add(nameLower);
            }

            // If any validation error, write file and abort without inserting anything
            if (result.Errors.Count > 0)
            {
                await WriteErrorsToFileAsync(result);
                return result;
            }

            using (var uow = UnitOfWorkManager.Begin())
            {
                foreach (var item in input.Items)
                {
                    var nameTrimmed = item.Name.Trim();
                    var entity = new ItemCategoryInfo
                    {
                        Name = nameTrimmed,
                        TenantId = tenantId
                    };
                    await MainRepository.InsertAsync(entity);
                }

                await uow.CompleteAsync();
            }

            result.SuccessCount = result.TotalItems;
            result.FailureCount = 0;
            return result;
        }

        private static async Task WriteErrorsToFileAsync(IMS_ItemCategoryBulkUploadResultDto result)
        {
            try
            {
                var baseDirectory = Directory.GetCurrentDirectory();
                var errorsDirectory = Path.Combine(baseDirectory, "wwwroot", "bulk-upload-errors");
                if (!Directory.Exists(errorsDirectory))
                    Directory.CreateDirectory(errorsDirectory);

                var fileName = $"categories-errors-{DateTime.Now:yyyyMMddHHmmssfff}.txt";
                var fullPath = Path.Combine(errorsDirectory, fileName);
                await File.WriteAllLinesAsync(fullPath, result.Errors ?? new List<string>());

                result.ErrorFilePath = $"/bulk-upload-errors/{fileName}";
            }
            catch
            {
                // ignore file write errors; keep in-memory errors
            }
        }
    }

    [AutoMap(typeof(ItemCategoryInfo))]
    public class IMS_ItemCategoryDto : SimpleDtoBase
    {
    }

    public class IMS_ItemCategoryBulkUploadRequestDto
    {
        public List<IMS_ItemCategoryBulkItemDto> Items { get; set; }
    }

    public class IMS_ItemCategoryBulkItemDto
    {
        public string Name { get; set; }
    }

    public class IMS_ItemCategoryBulkUploadResultDto
    {
        public int TotalItems { get; set; }
        public int SuccessCount { get; set; }
        public int FailureCount { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public string ErrorFilePath { get; set; }
    }
}
