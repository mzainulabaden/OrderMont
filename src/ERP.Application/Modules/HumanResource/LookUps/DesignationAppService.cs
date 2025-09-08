using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.UI;
using ERP.Authorization;
using ERP.Generics;
using ERP.Generics.Simple;
using ERP.Modules.HumanResource.EmployeeManagement;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.Modules.HumanResource.LookUps
{
    [AbpAuthorize(PermissionNames.LookUps_Designation)]
    public class DesignationAppService : GenericSimpleAppService<DesignationDto, DesignationInfo, SimpleSearchDtoBase>
    {
        public IRepository<EmployeeInfo, long> Employee_Repo { get; set; }

        public override PagedResultDto<DesignationDto> GetAll(SimpleSearchDtoBase search)
        {
            var query = MainRepository.GetAll().Where(i => i.TenantId == AbpSession.TenantId);
            query = ApplyFilters(query, search);
            query = query.OrderByDescending(i => i.CreationTime);

            var pagedEntities = query
                .Skip(search.SkipCount)
                .Take(search.MaxResultCount)
                .ToList();

            var items = ObjectMapper.Map<System.Collections.Generic.List<DesignationDto>>(pagedEntities) ?? new System.Collections.Generic.List<DesignationDto>();
            return new PagedResultDto<DesignationDto>(query.Count(), items);
        }

        public override async Task<DesignationDto> Create(DesignationDto input)
        {
            return await base.Create(input);
        }

        public override DesignationDto Get(long Id)
        {
            return base.Get(Id);
        }

        public override async Task<DesignationDto> Update(DesignationDto input)
        {
            return await base.Update(input);
        }

        public override async Task<string> Delete(EntityDto<long> input)
        {
            var has_employees = await Employee_Repo.GetAll(this, i => i.DesignationId == input.Id).AnyAsync();
            if (has_employees)
                throw new UserFriendlyException("This Designation is linked to Employees and cannot be deleted.");

            return await base.Delete(input);
        }
    }

    [AutoMap(typeof(DesignationInfo))]
    public class DesignationDto : SimpleDtoBase
    {

    }
}
