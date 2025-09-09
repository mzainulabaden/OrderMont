using Abp.Application.Services;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.UI;
using ERP.Authorization;
using ERP.Modules.Finance.AccountGroups.Dtos;
using ERP.Modules.Finance.LookUps;
using ERP.Modules.Suggestion.Dtos;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.Modules.Finance.AccountGroups
{
    public class AccountGroupsAppService : ApplicationService
    {
        public IRepository<AccountGroupsInfo, long> AccountGroups_Repo { get; set; }
        public IRepository<AccountTypeInfo, long> AccountType_Repo { get; set; }

        public async Task<List<AccountGroupsGetAllDto>> GetAll(AccountGroupsFiltersDto filters)
        {
            var entities = await AccountGroups_Repo.GetAll(this).ApplyBaseFilters(filters).ToPagedListAsync(filters);

            var all_account_type_ids = entities.SelectMany(i => i.AccountTypeIds).Distinct().ToList();
            var account_types = await AccountType_Repo.GetAll().Where(at => all_account_type_ids.Contains(at.Id)).Select(at => new { at.Id, at.Name }).ToListAsync();
            var dict_account_types = account_types.ToDictionary(at => at.Id, at => at.Name);

            var output = entities.Select(i => new AccountGroupsGetAllDto
            {
                Id = i.Id,
                Name = i.Name,
                AccountTypes = i.AccountTypeIds
                    .Where(id => dict_account_types.ContainsKey(id))
                    .Select(id => new SuggestionDto
                    {
                        Id = id,
                        Name = dict_account_types[id]
                    }).ToList()
            }).ToList();

            return output;
        }

        public async Task<string> Create(AccountGroupsDto input)
        {
            if (input.AccountTypeIds == null || !input.AccountTypeIds.Any())
                throw new UserFriendlyException("At least one Account Type must be selected.");

            var valid_account_type_count = await AccountType_Repo.GetAll().Where(at => input.AccountTypeIds.Contains(at.Id)).Select(at => at.Id).CountAsync();
            if (valid_account_type_count != input.AccountTypeIds.Count)
                throw new UserFriendlyException("One or more selected Account Types are invalid.");

            var entity = ObjectMapper.Map<AccountGroupsInfo>(input);
            entity.TenantId = AbpSession.TenantId;
            await AccountGroups_Repo.InsertAsync(entity);
            await CurrentUnitOfWork.SaveChangesAsync();
            return "Account Group Created Successfully.";
        }

        private async Task<AccountGroupsInfo> Get(long Id)
        {
            var entity = await AccountGroups_Repo.FirstOrDefaultAsync(Id);
            if (entity == null)
                throw new UserFriendlyException($"AccountGroupId '{Id}' is invalid.");
            return entity;
        }

        public async Task<AccountGroupsGetAllDto> GetForEdit(long id)
        {
            var entity = await AccountGroups_Repo.FirstOrDefaultAsync(id);
            if (entity == null)
                throw new UserFriendlyException($"AccountGroupId '{id}' is invalid.");

            var account_type_ids = entity.AccountTypeIds ?? new List<long>();

            var account_types = await AccountType_Repo.GetAll()
                .Where(at => account_type_ids.Contains(at.Id))
                .Select(at => new SuggestionDto
                {
                    Id = at.Id,
                    Name = at.Name
                })
                .ToListAsync();

            return new AccountGroupsGetAllDto
            {
                Id = entity.Id,
                Name = entity.Name,
                AccountTypes = account_types
            };
        }

        public async Task<string> Edit(AccountGroupsDto input)
        {
            var valid_account_type_count = await AccountType_Repo.GetAll().Where(at => input.AccountTypeIds.Contains(at.Id)).Select(at => at.Id).CountAsync();
            if (valid_account_type_count != input.AccountTypeIds.Count)
                throw new UserFriendlyException("One or more selected Account Types are invalid.");

            var entity = await Get(input.Id);
            ObjectMapper.Map(input, entity);
            await AccountGroups_Repo.UpdateAsync(entity);
            await CurrentUnitOfWork.SaveChangesAsync();
            return "Account Group Updated Successfully.";
        }

        public async Task<string> Delete(long Id)
        {
            var entity = await Get(Id);
            await AccountGroups_Repo.DeleteAsync(entity);
            await CurrentUnitOfWork.SaveChangesAsync();
            return "Account Group Deleted Successfully.";
        }

    }
}
