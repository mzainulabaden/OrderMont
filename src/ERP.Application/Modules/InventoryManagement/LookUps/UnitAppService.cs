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
using System.Linq;
using System.Threading.Tasks;

namespace ERP.Modules.InventoryManagement.LookUps
{
    [AbpAuthorize(PermissionNames.LookUps_Unit)]
    public class UnitAppService : GenericSimpleAppService<UnitDto, UnitInfo, SimpleSearchDtoBase>
    {
        public IRepository<ItemInfo, long> Item_Repo { get; set; }

        public override PagedResultDto<UnitDto> GetAll(SimpleSearchDtoBase search)
        {
            return base.GetAll(search);
        }

        public override async Task<UnitDto> Create(UnitDto input)
        {
            return await base.Create(input);
        }

        public override UnitDto Get(long Id)
        {
            return base.Get(Id);
        }

        public override async Task<UnitDto> Update(UnitDto input)
        {
            return await base.Update(input);
        }

        public override async Task<string> Delete(EntityDto<long> input)
        {
            var has_linked = await Item_Repo
                .GetAllIncluding(i => i.ItemDetails).AnyAsync(i => i.ItemDetails.Any(d => d.UnitId == input.Id));

            if (has_linked)
                throw new UserFriendlyException("This Item is linked to ItemDetails and cannot be deleted.");

            return await base.Delete(input);
        }
    }

    [AutoMap(typeof(UnitInfo))]
    public class UnitDto : SimpleDtoBase
    {
        public decimal ConversionFactor { get; set; }
        public string Description { get; set; }
    }
}
