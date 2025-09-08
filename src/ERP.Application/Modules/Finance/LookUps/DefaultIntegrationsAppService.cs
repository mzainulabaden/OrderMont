using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.UI;
using ERP.Authorization;
using ERP.Modules.Finance.ChartOfAccount.COALevel04;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace ERP.Modules.Finance.LookUps
{
    [AbpAuthorize(PermissionNames.LookUps_FINANCE_DefaultIntegrations)]
    public class DefaultIntegrationsAppService : ApplicationService
    {
        public IRepository<DefaultIntegrationsInfo, long> FINANCE_DefaultIntegrations_Repo { get; set; }
        public IRepository<COALevel04Info, long> ChartOfAccount_Repo { get; set; }

        public async Task<PagedResultDto<FINANCE_DefaultIntegrationsGetAllDto>> GetAll(FINANCE_DefaultIntegrationsFiltersDto filters)
        {
            var f_inance_default_integrations_query = FINANCE_DefaultIntegrations_Repo.GetAll(this).ApplyBaseFilters(filters);
            if (!string.IsNullOrWhiteSpace(filters.Id))
                f_inance_default_integrations_query = f_inance_default_integrations_query.Where(i => i.Id == filters.Id.TryToLong());
            if (!string.IsNullOrWhiteSpace(filters.ChartOfAccountId))
                f_inance_default_integrations_query = f_inance_default_integrations_query.Where(i => i.ChartOfAccountId == filters.ChartOfAccountId.TryToLong());
            var default_integrations = await f_inance_default_integrations_query.ApplyDocumentFilters(filters).ToPagedListAsync(filters);

            var chart_of_account_ids = default_integrations.Select(i => i.ChartOfAccountId).ToList();

            var total_count = f_inance_default_integrations_query.DeferredCount().FutureValue();
            var chart_of_accounts = ChartOfAccount_Repo.GetAll(this, i => chart_of_account_ids.Contains(i.Id)).Future();
            await chart_of_accounts.ToListAsync();

            var dict_chart_of_accounts = chart_of_accounts.ToDictionary(i => i.Id);

            var output = new List<FINANCE_DefaultIntegrationsGetAllDto>();
            foreach (var default_integration in default_integrations)
            {
                dict_chart_of_accounts.TryGetValue(default_integration.ChartOfAccountId, out var chart_of_account);

                var dto = ObjectMapper.Map<FINANCE_DefaultIntegrationsGetAllDto>(default_integration);
                dto.ChartOfAccountName = chart_of_account?.Name ?? "";
                output.Add(dto);
            }

            return new PagedResultDto<FINANCE_DefaultIntegrationsGetAllDto>(total_count.Value, output);
        }

        [AbpAuthorize(PermissionNames.LookUps_FINANCE_DefaultIntegrations_Create)]
        public async Task<string> Create(FINANCE_DefaultIntegrationsDto input)
        {
            var chart_of_account = ChartOfAccount_Repo.GetAll(this, i => i.Id == input.ChartOfAccountId).DeferredFirstOrDefault().FutureValue();
            await chart_of_account.ValueAsync();

            if (chart_of_account.Value == null)
                throw new UserFriendlyException($"ChartOfAccountId: '{input.ChartOfAccountId}' is invalid.");

            var entity = ObjectMapper.Map<DefaultIntegrationsInfo>(input);
            entity.TenantId = AbpSession.TenantId;
            await FINANCE_DefaultIntegrations_Repo.InsertAsync(entity);
            await CurrentUnitOfWork.SaveChangesAsync();
            return "FINANCE_DefaultIntegrations Created Successfully.";
        }

        public async Task<DefaultIntegrationsInfo> Get(long Id)
        {
            var f_inance_default_integrations = await FINANCE_DefaultIntegrations_Repo.GetAll(this, i => i.Id == Id).FirstOrDefaultAsync();
            if (f_inance_default_integrations != null)
                return f_inance_default_integrations;
            else
                throw new UserFriendlyException($"FINANCE_DefaultIntegrationsId is '{Id}' invalid.");
        }

        public async Task<FINANCE_DefaultIntegrationsGetAllDto> GetForEdit(long Id)
        {
            var f_inance_default_integrations = await Get(Id);
            var chart_of_account = ChartOfAccount_Repo.GetAll(this, i => i.Id == f_inance_default_integrations.ChartOfAccountId).DeferredFirstOrDefault().FutureValue();
            await chart_of_account.ValueAsync();

            var output = ObjectMapper.Map<FINANCE_DefaultIntegrationsGetAllDto>(f_inance_default_integrations);
            output.ChartOfAccountName = chart_of_account?.Value?.Name ?? "";
            return output;
        }

        [AbpAuthorize(PermissionNames.LookUps_FINANCE_DefaultIntegrations_Edit)]
   
        public async Task<string> Update(FINANCE_DefaultIntegrationsDto input)
        {
            var chart_of_account = ChartOfAccount_Repo.GetAll(this, i => i.Id == input.ChartOfAccountId).DeferredFirstOrDefault().FutureValue();
            await chart_of_account.ValueAsync();

            if (chart_of_account.Value == null)
                throw new UserFriendlyException($"ChartOfAccountId: '{input.ChartOfAccountId}' is invalid.");

            var old_financedefaultintegrations = await Get(input.Id);
            var entity = ObjectMapper.Map(input, old_financedefaultintegrations);
            await FINANCE_DefaultIntegrations_Repo.UpdateAsync(entity);
            await CurrentUnitOfWork.SaveChangesAsync();
            return "FINANCE_DefaultIntegrations Updated Successfully.";
        }

        [AbpAuthorize(PermissionNames.LookUps_FINANCE_DefaultIntegrations_Delete)]
        public async Task<string> Delete(long Id)
        {
            var f_inance_default_integrations = await FINANCE_DefaultIntegrations_Repo.GetAll(this, i => i.Id == Id).FirstOrDefaultAsync();
            if (f_inance_default_integrations == null)
                throw new UserFriendlyException($"FINANCE_DefaultIntegrationsId is '{Id}' invalid.");

            await FINANCE_DefaultIntegrations_Repo.DeleteAsync(f_inance_default_integrations);
            await CurrentUnitOfWork.SaveChangesAsync();
            return "FINANCE_DefaultIntegrations Deleted Successfully.";
        }
    }
}
