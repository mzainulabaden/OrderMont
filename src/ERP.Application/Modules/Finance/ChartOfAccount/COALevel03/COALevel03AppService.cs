using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.UI;
using ERP.Authorization;
using ERP.Modules.Finance.ChartOfAccount.COALevel02;
using ERP.Modules.Finance.ChartOfAccount.COALevel04;
using ERP.Modules.Finance.LookUps;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace ERP.Modules.Finance.ChartOfAccount.COALevel03
{
    public class COALevel03AppService : ApplicationService
    {
        public IRepository<COALevel02Info, long> COALevel02_Repo { get; set; }
        public IRepository<COALevel03Info, long> COALevel03_Repo { get; set; }
        public IRepository<COALevel04Info, long> COALevel04_Repo { get; set; }
        public IRepository<AccountTypeInfo, long> AccountType_Repo { get; set; }

        public async Task<PagedResultDto<COALevel03GetAllDto>> GetAll(COALevel03FiltersDto filters)
        {
            var coa_level03_query = COALevel03_Repo.GetAll(this).ApplyBaseFilters(filters);
            if (!string.IsNullOrWhiteSpace(filters.COALevel02Id))
                coa_level03_query = coa_level03_query.Where(i => i.COALevel02Id == filters.COALevel02Id.TryToLong());
            if (!string.IsNullOrWhiteSpace(filters.AccountTypeId))
                coa_level03_query = coa_level03_query.Where(i => i.AccountTypeId == filters.AccountTypeId.TryToLong());
            var coa_level03s = await coa_level03_query.ToPagedListAsync(filters);

            var c_oa_level02_ids = coa_level03s.Select(i => i.COALevel02Id).ToList();
            var account_type_ids = coa_level03s.Select(i => i.AccountTypeId).ToList();

            var total_count = coa_level03_query.DeferredCount().FutureValue();
            var coa_level02s = COALevel02_Repo.GetAll(this, i => c_oa_level02_ids.Contains(i.Id)).Select(i => new { i.Id, i.Name, i.SerialNumber }).Future();
            var account_types = AccountType_Repo.GetAll(this, i => account_type_ids.Contains(i.Id)).Select(i => new { i.Id, i.Name }).Future();
            await account_types.ToListAsync();

            var dict_coa_level02s = coa_level02s.ToDictionary(i => i.Id);
            var dict_account_types = account_types.ToDictionary(i => i.Id);

            var output = new List<COALevel03GetAllDto>();
            foreach (var c_oa_level03 in coa_level03s)
            {
                dict_coa_level02s.TryGetValue(c_oa_level03.COALevel02Id, out var coa_level02);
                dict_account_types.TryGetValue(c_oa_level03.AccountTypeId, out var account_type);

                var dto = ObjectMapper.Map<COALevel03GetAllDto>(c_oa_level03);
                dto.COALevel02Name = coa_level02?.Name ?? "";
                dto.COALevel02SerialNumber = coa_level02?.SerialNumber ?? "";
                dto.AccountTypeName = account_type?.Name ?? "";
                output.Add(dto);
            }

            return new PagedResultDto<COALevel03GetAllDto>(total_count.Value, output);
        }

        public async Task<string> Create(COALevel03Dto input)
        {
            var coa_level02 = COALevel02_Repo.GetAll(this, i => i.Id == input.COALevel02Id).DeferredFirstOrDefault().FutureValue();
            var account_type = AccountType_Repo.GetAll(this, i => i.Id == input.AccountTypeId).DeferredFirstOrDefault().FutureValue();
            await account_type.ValueAsync();

            if (coa_level02.Value == null)
                throw new UserFriendlyException($"COALevel02Id: '{input.COALevel02Id}' is invalid.");
            if (account_type.Value == null)
                throw new UserFriendlyException($"AccountTypeId: '{input.AccountTypeId}' is invalid.");

            var entity = ObjectMapper.Map<COALevel03Info>(input);
            entity.TenantId = AbpSession.TenantId;
            await COALevel03_Repo.InsertAsync(entity);
            await CurrentUnitOfWork.SaveChangesAsync();
            return "COALevel03 Created Successfully.";
        }

        private async Task<COALevel03Info> GetById(long Id)
        {
            var coa_level03 = await COALevel03_Repo.GetAll(this, i => i.Id == Id).FirstOrDefaultAsync();
            if (coa_level03 != null)
                return coa_level03;
            else
                throw new UserFriendlyException($"COALevel03Id: '{Id}' is invalid.");
        }

        public async Task<COALevel03GetAllDto> Get(long Id)
        {
            var coa_level03 = await GetById(Id);
            var coa_level02 = COALevel02_Repo.GetAll(this, i => i.Id == coa_level03.COALevel02Id).DeferredFirstOrDefault().FutureValue();
            var account_type = AccountType_Repo.GetAll(this, i => i.Id == coa_level03.AccountTypeId).DeferredFirstOrDefault().FutureValue();
            await account_type.ValueAsync();

            var output = ObjectMapper.Map<COALevel03GetAllDto>(coa_level03);
            output.COALevel02Name = coa_level02?.Value?.Name ?? "";
            output.COALevel02SerialNumber = coa_level02?.Value?.SerialNumber ?? "";
            output.AccountTypeName = account_type?.Value?.Name ?? "";
            return output;
        }

        public async Task<string> Update(COALevel03Dto input)
        {
            var coa_level02 = COALevel02_Repo.GetAll(this, i => i.Id == input.COALevel02Id).DeferredFirstOrDefault().FutureValue();
            var account_type = AccountType_Repo.GetAll(this, i => i.Id == input.AccountTypeId).DeferredFirstOrDefault().FutureValue();
            await account_type.ValueAsync();

            if (coa_level02.Value == null)
                throw new UserFriendlyException($"COALevel02Id: '{input.COALevel02Id}' is invalid.");
            if (account_type.Value == null)
                throw new UserFriendlyException($"AccountTypeId: '{input.AccountTypeId}' is invalid.");

            var old_coa_level_03 = await GetById(input.Id);
            var entity = ObjectMapper.Map(input, old_coa_level_03);
            await COALevel03_Repo.UpdateAsync(entity);
            await CurrentUnitOfWork.SaveChangesAsync();
            return "COALevel03 Updated Successfully.";
        }

        public async Task<string> Delete(long Id)
        {
            var coa_level03 = await GetById(Id);

            var has_linked = await COALevel04_Repo.GetAll(this).AnyAsync(i => i.COALevel03Id == Id);
            if (has_linked)
                throw new UserFriendlyException($"Cannot delete COALevel03 with SerialNumber '{coa_level03.SerialNumber}' because it has related COALevel04 records.");

            await COALevel03_Repo.DeleteAsync(coa_level03);
            await CurrentUnitOfWork.SaveChangesAsync();
            return "COALevel03 Deleted Successfully.";
        }

        public async Task<string> GetSerialNumber(long COALevel02Id)
        {
            var count = await COALevel03_Repo.GetAll(this, i => i.COALevel02Id == COALevel02Id).CountAsync() + 1;

            if (count > 999)
                throw new UserFriendlyException($"Cannot create a new COALevel03 entry. The maximum allowed limit of 999 entries has been reached for COALevel02 with Id {COALevel02Id}.");

            return count.ToString().PadLeft(3, '0');
        }

        public async Task<COALevel03BulkUploadResultDto> BulkUpload(COALevel03BulkUploadRequestDto input)
        {
            var result = new COALevel03BulkUploadResultDto
            {
                TotalItems = input.Items?.Count ?? 0
            };

            if (input.Items == null || input.Items.Count == 0)
            {
                result.Errors.Add("No items provided for bulk upload");
                return result;
            }

            var accountTypesQuery = await AccountType_Repo.GetAll(this).Select(at => new { at.Id, at.Name }).ToListAsync();
            var accountTypes = accountTypesQuery.GroupBy(at => at.Name.ToLower()).ToDictionary(g => g.Key, g => g.First().Id);
            var coaLevel02sQuery = await COALevel02_Repo.GetAll(this).Select(c => new { c.Id, c.Name }).ToListAsync();
            var coaLevel02s = coaLevel02sQuery.GroupBy(c => c.Name.ToLower()).ToDictionary(g => g.Key, g => g.First().Id);
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
                    if (string.IsNullOrWhiteSpace(item.COALevel02Name))
                    {
                        result.Errors.Add($"Item '{item.Name}' has no COALevel02Name");
                        result.FailureCount++;
                        continue;
                    }

                    if (string.IsNullOrWhiteSpace(item.AccountTypeName))
                    {
                        result.Errors.Add($"Item '{item.Name}' has no AccountTypeName");
                        result.FailureCount++;
                        continue;
                    }

                    var coaLevel02NameLower = item.COALevel02Name.ToLower();
                    if (!coaLevel02s.TryGetValue(coaLevel02NameLower, out var coaLevel02Id))
                    {
                        result.Errors.Add($"COALevel02Name '{item.COALevel02Name}' for item '{item.Name}' does not exist");
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

                    var existingWithSameSerialNumber = await COALevel03_Repo.GetAll(this)
                        .AnyAsync(c => c.SerialNumber == item.SerialNumber && c.COALevel02Id == coaLevel02Id);

                    if (existingWithSameSerialNumber)
                    {
                        result.Errors.Add($"SerialNumber '{item.SerialNumber}' for item '{item.Name}' already exists for the given COALevel02");
                        result.FailureCount++;
                        continue;
                    }

                    var entity = new COALevel03Info
                    {
                        Name = item.Name,
                        SerialNumber = item.SerialNumber,
                        COALevel02Id = coaLevel02Id,
                        AccountTypeId = accountTypeId,
                        TenantId = AbpSession.TenantId
                    };

                    await COALevel03_Repo.InsertAsync(entity);
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
