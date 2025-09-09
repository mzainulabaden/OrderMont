using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.UI;
using ERP.Authorization;
using ERP.Modules.Finance.ChartOfAccount.COALevel01;
using ERP.Modules.Finance.ChartOfAccount.COALevel03;
using ERP.Modules.Finance.LookUps;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace ERP.Modules.Finance.ChartOfAccount.COALevel02
{
    public class COALevel02AppService : ApplicationService
    {
        public IRepository<COALevel01Info, long> COALevel01_Repo { get; set; }
        public IRepository<COALevel02Info, long> COALevel02_Repo { get; set; }
        public IRepository<COALevel03Info, long> COALevel03_Repo { get; set; }
        public IRepository<AccountTypeInfo, long> AccountType_Repo { get; set; }

        public async Task<PagedResultDto<COALevel02GetAllDto>> GetAll(COALevel02FiltersDto filters)
        {
            var coa_level02_query = COALevel02_Repo.GetAll(this).ApplyBaseFilters(filters);
            if (!string.IsNullOrWhiteSpace(filters.COALevel01Id))
                coa_level02_query = coa_level02_query.Where(i => i.COALevel01Id == filters.COALevel01Id.TryToLong());
            if (!string.IsNullOrWhiteSpace(filters.AccountTypeId))
                coa_level02_query = coa_level02_query.Where(i => i.AccountTypeId == filters.AccountTypeId.TryToLong());
            var coa_level02s = await coa_level02_query.ToPagedListAsync(filters);

            var coa_level01_ids = coa_level02s.Select(i => i.COALevel01Id).ToList();
            var account_type_ids = coa_level02s.Select(i => i.AccountTypeId).ToList();

            var total_count = coa_level02_query.DeferredCount().FutureValue();
            var coa_level01s = COALevel01_Repo.GetAll(this, i => coa_level01_ids.Contains(i.Id)).Select(i => new { i.Id, i.Name, i.SerialNumber }).Future();
            var account_types = AccountType_Repo.GetAll(this, i => account_type_ids.Contains(i.Id)).Select(i => new { i.Id, i.Name }).Future();
            _ = await account_types.ToListAsync();

            var dict_coa_level01s = coa_level01s.ToDictionary(i => i.Id);
            var dict_account_types = account_types.ToDictionary(i => i.Id);

            var output = new List<COALevel02GetAllDto>();
            foreach (var coa_level02 in coa_level02s)
            {
                dict_coa_level01s.TryGetValue(coa_level02.COALevel01Id, out var coa_level01);
                dict_account_types.TryGetValue(coa_level02.AccountTypeId, out var account_type);

                var dto = ObjectMapper.Map<COALevel02GetAllDto>(coa_level02);
                dto.COALevel01Name = coa_level01?.Name ?? "";
                dto.COALevel01SerialNumber = coa_level01?.SerialNumber ?? "";
                dto.AccountTypeName = account_type?.Name ?? "";
                output.Add(dto);
            }

            return new PagedResultDto<COALevel02GetAllDto>(total_count.Value, output);
        }

        public async Task<string> Create(COALevel02Dto input)
        {
            var coa_level01 = COALevel01_Repo.GetAll(this, i => i.Id == input.COALevel01Id).DeferredFirstOrDefault().FutureValue();
            var account_type = AccountType_Repo.GetAll(this, i => i.Id == input.AccountTypeId).DeferredFirstOrDefault().FutureValue();
            _ = await account_type.ValueAsync();

            if (coa_level01.Value == null)
                throw new UserFriendlyException($"COALevel01Id: '{input.COALevel01Id}' is invalid.");
            if (account_type.Value == null)
                throw new UserFriendlyException($"AccountTypeId: '{input.AccountTypeId}' is invalid.");

            var entity = ObjectMapper.Map<COALevel02Info>(input);
            entity.TenantId = AbpSession.TenantId;
            await COALevel02_Repo.InsertAsync(entity);
            await CurrentUnitOfWork.SaveChangesAsync();
            return "COALevel02 Created Successfully.";
        }

        private async Task<COALevel02Info> GetById(long Id)
        {
            var coa_level02 = await COALevel02_Repo.GetAll(this, i => i.Id == Id).FirstOrDefaultAsync();
            if (coa_level02 != null)
                return coa_level02;
            else
                throw new UserFriendlyException($"COALevel02Id: '{Id}' is invalid.");
        }

        public async Task<COALevel02GetAllDto> Get(long Id)
        {
            var coa_level02 = await GetById(Id);
            var coa_level01 = COALevel01_Repo.GetAll(this, i => i.Id == coa_level02.COALevel01Id).DeferredFirstOrDefault().FutureValue();
            var account_type = AccountType_Repo.GetAll(this, i => i.Id == coa_level02.AccountTypeId).DeferredFirstOrDefault().FutureValue();
            _ = await account_type.ValueAsync();

            var output = ObjectMapper.Map<COALevel02GetAllDto>(coa_level02);
            output.COALevel01Name = coa_level01?.Value?.Name ?? "";
            output.COALevel01SerialNumber = coa_level01?.Value?.SerialNumber ?? "";
            output.AccountTypeName = account_type?.Value?.Name ?? "";
            return output;
        }

        public async Task<string> Update(COALevel02Dto input)
        {
            var coa_level01 = COALevel01_Repo.GetAll(this, i => i.Id == input.COALevel01Id).DeferredFirstOrDefault().FutureValue();
            var account_type = AccountType_Repo.GetAll(this, i => i.Id == input.AccountTypeId).DeferredFirstOrDefault().FutureValue();
            await account_type.ValueAsync();

            if (coa_level01.Value == null)
                throw new UserFriendlyException($"COALevel01Id: '{input.COALevel01Id}' is invalid.");
            if (account_type.Value == null)
                throw new UserFriendlyException($"AccountTypeId: '{input.AccountTypeId}' is invalid.");

            var old_coa_level_02 = await GetById(input.Id);
            var entity = ObjectMapper.Map(input, old_coa_level_02);
            await COALevel02_Repo.UpdateAsync(entity);
            await CurrentUnitOfWork.SaveChangesAsync();
            return "COALevel02 Updated Successfully.";
        }

        public async Task<string> Delete(long Id)
        {
            var coa_level02 = await GetById(Id);

            var has_linked = await COALevel03_Repo.GetAll(this, i => i.COALevel02Id == Id).AnyAsync();
            if (has_linked)
                throw new UserFriendlyException($"Cannot delete COALevel02 with SerialNumber '{coa_level02.SerialNumber}' because it has related COALevel03 records.");

            await COALevel02_Repo.DeleteAsync(coa_level02);
            await CurrentUnitOfWork.SaveChangesAsync();
            return "COALevel02 Deleted Successfully.";
        }

        public async Task<string> GetSerialNumber(long COALevel01Id)
        {
            var count = await COALevel02_Repo.GetAll(this, i => i.COALevel01Id == COALevel01Id).CountAsync() + 1;

            if (count > 99)
                throw new UserFriendlyException($"Cannot create a new COALevel02 entry. The maximum allowed limit of 99 entries has been reached for COALevel01 with Id {COALevel01Id}.");

            return count.ToString().PadLeft(2, '0');
        }

        public async Task<COALevel02BulkUploadResultDto> BulkUpload(COALevel02BulkUploadRequestDto input)
        {
            var result = new COALevel02BulkUploadResultDto
            {
                TotalItems = input.Items?.Count ?? 0
            };

            if (input.Items == null || input.Items.Count == 0)
            {
                result.Errors.Add("No items provided for bulk upload");
                return result;
            }

            var accountTypesQuery = await AccountType_Repo.GetAll(this)
                .Select(at => new { at.Id, at.Name })
                .ToListAsync();
                
            var accountTypes = accountTypesQuery
                .GroupBy(at => at.Name.ToLower())
                .ToDictionary(g => g.Key, g => g.First().Id);

            var coaLevel01sQuery = await COALevel01_Repo.GetAll(this)
                .Select(c => new { c.Id, c.Name })
                .ToListAsync();
                
            var coaLevel01s = coaLevel01sQuery
                .GroupBy(c => c.Name.ToLower())
                .ToDictionary(g => g.Key, g => g.First().Id);

            foreach (var item in input.Items)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(item.Name))
                    {
                        result.Errors.Add($"Item with SerialNumber '{item.SerialNumber}' has no Name");
                        result.FailureCount++;
                        continue;
                    }

                    if (string.IsNullOrWhiteSpace(item.SerialNumber))
                    {
                        result.Errors.Add($"Item with Name '{item.Name}' has no SerialNumber");
                        result.FailureCount++;
                        continue;
                    }

                    if (string.IsNullOrWhiteSpace(item.COALevel01Name))
                    {
                        result.Errors.Add($"Item '{item.Name}' has no COALevel01Name");
                        result.FailureCount++;
                        continue;
                    }

                    if (string.IsNullOrWhiteSpace(item.AccountTypeName))
                    {
                        result.Errors.Add($"Item '{item.Name}' has no AccountTypeName");
                        result.FailureCount++;
                        continue;
                    }

                    var coaLevel01NameLower = item.COALevel01Name.ToLower();
                    if (!coaLevel01s.TryGetValue(coaLevel01NameLower, out var coaLevel01Id))
                    {
                        result.Errors.Add($"COALevel01Name '{item.COALevel01Name}' for item '{item.Name}' does not exist");
                        result.FailureCount++;
                        continue;
                    }

                    var accountTypeNameLower = item.AccountTypeName.ToLower();
                    if (!accountTypes.TryGetValue(accountTypeNameLower, out var accountTypeId))
                    {
                        result.Errors.Add($"AccountTypeName '{item.AccountTypeName}' for item '{item.Name}' does not exist");
                        result.FailureCount++;
                        continue;
                    }

                    var existingWithSameSerialNumber = await COALevel02_Repo.GetAll(this)
                        .AnyAsync(c => c.SerialNumber == item.SerialNumber && c.COALevel01Id == coaLevel01Id);

                    if (existingWithSameSerialNumber)
                    {
                        result.Errors.Add($"SerialNumber '{item.SerialNumber}' for item '{item.Name}' already exists for the given COALevel01");
                        result.FailureCount++;
                        continue;
                    }

                    var entity = new COALevel02Info
                    {
                        Name = item.Name,
                        SerialNumber = item.SerialNumber,
                        COALevel01Id = coaLevel01Id,
                        AccountTypeId = accountTypeId,
                        TenantId = AbpSession.TenantId
                    };

                    await COALevel02_Repo.InsertAsync(entity);
                    result.SuccessCount++;
                }
                catch (System.Exception ex)
                {
                    result.Errors.Add($"Error processing item '{item.Name}': {ex.Message}");
                    result.FailureCount++;
                }
            }

            await CurrentUnitOfWork.SaveChangesAsync();
            return result;
        }
    }
}
