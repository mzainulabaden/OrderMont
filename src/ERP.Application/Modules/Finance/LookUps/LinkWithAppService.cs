using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.UI;
using ERP.Authorization;
using ERP.Generics;
using ERP.Generics.Simple;
using ERP.Modules.Finance.ChartOfAccount.COALevel04;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ERP.Modules.Finance.LookUps
{
    [AbpAuthorize(PermissionNames.LookUps_LinkWith)]
    public class LinkWithAppService : GenericSimpleAppService<LinkWithDto, LinkWithInfo, SimpleSearchDtoBase>
    {
        public IRepository<COALevel04Info, long> COALevel04_Repo { get; set; }

        public override PagedResultDto<LinkWithDto> GetAll(SimpleSearchDtoBase search)
        {
            return base.GetAll(search);
        }

        public override async Task<LinkWithDto> Create(LinkWithDto input)
        {
            return await base.Create(input);
        }

        public override LinkWithDto Get(long Id)
        {
            return base.Get(Id);
        }

        public override async Task<LinkWithDto> Update(LinkWithDto input)
        {
            return await base.Update(input);
        }

        public override async Task<string> Delete(EntityDto<long> input)
        {
            var has_linked = await COALevel04_Repo.GetAll(this).AnyAsync(i => i.CurrencyId == input.Id);
            if (has_linked)
                throw new UserFriendlyException($"This LinkWith is linked to COALevel04 and cannot be deleted.");

            return await base.Delete(input);
        }
    }

    [AutoMap(typeof(LinkWithInfo))]
    public class LinkWithDto : SimpleDtoBase
    {

    }
}
