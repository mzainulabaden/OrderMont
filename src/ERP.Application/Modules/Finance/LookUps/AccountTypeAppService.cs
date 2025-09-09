using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.UI;
using ERP.Authorization;
using ERP.Generics;
using ERP.Generics.Simple;
using ERP.Modules.Finance.ChartOfAccount.COALevel01;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ERP.Modules.Finance.LookUps
{
    public class AccountTypeAppService : GenericSimpleAppService<AccountTypeDto, AccountTypeInfo, SimpleSearchDtoBase>
    {
        public IRepository<COALevel01Info, long> COALevel01_Repo { get; set; }

        public override PagedResultDto<AccountTypeDto> GetAll(SimpleSearchDtoBase search)
        {
            return base.GetAll(search);
        }

        public override async Task<AccountTypeDto> Create(AccountTypeDto input)
        {
            return await base.Create(input);
        }

        public override AccountTypeDto Get(long Id)
        {
            return base.Get(Id);
        }

        public override async Task<AccountTypeDto> Update(AccountTypeDto input)
        {
            return await base.Update(input);
        }

        public override async Task<string> Delete(EntityDto<long> input)
        {
            var has_linked = await COALevel01_Repo.GetAll(this).AnyAsync(i => i.AccountTypeId == input.Id);
            if (has_linked)
                throw new UserFriendlyException($"This AccountType is linked to ChartOfAccounts and cannot be deleted.");

            return await base.Delete(input);
        }
    }

    [AutoMap(typeof(AccountTypeInfo))]
    public class AccountTypeDto : SimpleDtoBase
    {
        public string ShortName { get; set; }
    }
}
