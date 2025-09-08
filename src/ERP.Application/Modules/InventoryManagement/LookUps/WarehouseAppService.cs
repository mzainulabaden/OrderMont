using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.UI;
using ERP.Authorization;
using ERP.Generics;
using ERP.Generics.Simple;
using ERP.Modules.InventoryManagement.PurchaseInvoice;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ERP.Modules.InventoryManagement.LookUps
{
    [AbpAuthorize(PermissionNames.LookUps_Warehouse)]
    public class WarehouseAppService : GenericSimpleAppService<WarehouseDto, WarehouseInfo, SimpleSearchDtoBase>
    {
        public IRepository<PurchaseInvoiceInfo, long> PurchaseInvoice_Repo { get; set; }

        public override PagedResultDto<WarehouseDto> GetAll(SimpleSearchDtoBase search)
        {
            return base.GetAll(search);
        }

        public override async Task<WarehouseDto> Create(WarehouseDto input)
        {
            return await base.Create(input);
        }

        public override WarehouseDto Get(long Id)
        {
            return base.Get(Id);
        }

        public override async Task<WarehouseDto> Update(WarehouseDto input)
        {
            return await base.Update(input);
        }

        public override async Task<string> Delete(EntityDto<long> input)
        {
            var has_linked = await PurchaseInvoice_Repo.GetAll(this).AnyAsync(pi => pi.WarehouseId == input.Id);
            if (has_linked)
                throw new UserFriendlyException("This warehouse is linked to PurchaseInvoices and cannot be deleted.");

            return await base.Delete(input);
        }
    }

    [AutoMap(typeof(WarehouseInfo))]
    public class WarehouseDto : SimpleDtoBase
    {
        public string Address { get; set; }
        public string WarehouseCode { get; set; }
        public string Manager { get; set; }
        public string PhoneNumber { get; set; }
    }
}
