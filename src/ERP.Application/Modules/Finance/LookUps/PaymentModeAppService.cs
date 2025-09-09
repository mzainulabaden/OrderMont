using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.UI;
using ERP.Generics;
using ERP.Generics.Simple;
using ERP.Modules.InventoryManagement.PurchaseInvoice;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace ERP.Modules.Finance.LookUps
{
    public class PaymentModeAppService : GenericSimpleAppService<PaymentModeDto, PaymentModeInfo, SimpleSearchDtoBase>
    {
        public IRepository<PurchaseInvoiceInfo, long> PurchaseInvoice_Repo { get; set; }

        public override PagedResultDto<PaymentModeDto> GetAll(SimpleSearchDtoBase search)
        {
            return base.GetAll(search);
        }

        public override async Task<PaymentModeDto> Create(PaymentModeDto input)
        {
            return await base.Create(input);
        }

        public override PaymentModeDto Get(long Id)
        {
            return base.Get(Id);
        }

        public override async Task<PaymentModeDto> Update(PaymentModeDto input)
        {
            return await base.Update(input);
        }

    }

    [AutoMap(typeof(PaymentModeInfo))]
    public class PaymentModeDto : SimpleDtoBase
    {

    }
}
