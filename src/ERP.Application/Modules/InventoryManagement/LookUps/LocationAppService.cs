using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Abp.AutoMapper;
using Abp.Domain.Entities;
using Abp.Authorization;
using ERP.Authorization;
using ERP.Generics;
using ERP.Generics.Simple;
using Microsoft.EntityFrameworkCore;

namespace ERP.Modules.InventoryManagement.LookUps
{
    [AbpAuthorize(PermissionNames.LookUps_IMS_Location)]
    public class LocationAppService : GenericSimpleAppService<IMS_LocationDto, LocationInfo, SimpleSearchDtoBase>
    {
        public async Task<IMS_LocationBulkUploadResultDto> BulkUpload(IMS_LocationBulkUploadRequestDto input)
        {
            CheckCreatePermission();

            var result = new IMS_LocationBulkUploadResultDto
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
                    var entity = new LocationInfo
                    {
                        Name = nameTrimmed,
                        Address = item.Address,
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

        private static async Task WriteErrorsToFileAsync(IMS_LocationBulkUploadResultDto result)
        {
            try
            {
                var baseDirectory = Directory.GetCurrentDirectory();
                var errorsDirectory = Path.Combine(baseDirectory, "wwwroot", "bulk-upload-errors");
                if (!Directory.Exists(errorsDirectory))
                    Directory.CreateDirectory(errorsDirectory);

                var fileName = $"locations-errors-{DateTime.Now:yyyyMMddHHmmssfff}.txt";
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

    [AutoMap(typeof(LocationInfo))]
    public class IMS_LocationDto : SimpleDtoBase
    {
        public string Address { get; set; }
    }

    public class IMS_LocationBulkUploadRequestDto
    {
        public List<IMS_LocationBulkItemDto> Items { get; set; }
    }

    public class IMS_LocationBulkItemDto
    {
        public string Name { get; set; }
        public string Address { get; set; }
    }

    public class IMS_LocationBulkUploadResultDto
    {
        public int TotalItems { get; set; }
        public int SuccessCount { get; set; }
        public int FailureCount { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public string ErrorFilePath { get; set; }
    }
}
