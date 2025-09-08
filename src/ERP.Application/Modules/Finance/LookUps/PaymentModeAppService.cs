using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.UI;
using ERP.Authorization;
using ERP.Generics;
using ERP.Generics.Simple;
using ERP.Modules.InventoryManagement.PurchaseInvoice;
using ERP.Modules.InventoryManagement.PurchaseOrder;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace ERP.Modules.Finance.LookUps
{
    [AbpAuthorize(PermissionNames.LookUps_PaymentMode)]
    public class PaymentModeAppService : GenericSimpleAppService<PaymentModeDto, PaymentModeInfo, SimpleSearchDtoBase>
    {
        public IRepository<PurchaseOrderInfo, long> PurchaseOrder_Repo { get; set; }
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

        public override async Task<string> Delete(EntityDto<long> input)
        {
            var has_linked_purchase_order = PurchaseOrder_Repo.GetAll(this).DeferredAny(po => po.PaymentModeId == input.Id).FutureValue();
            var has_linked_purchase_invoice = PurchaseInvoice_Repo.GetAll(this).DeferredAny(pi => pi.PaymentModeId == input.Id).FutureValue();
            _ = await has_linked_purchase_invoice.ValueAsync();

            if (has_linked_purchase_order.Value)
                throw new UserFriendlyException("This payment mode is linked to a Purchase Order and cannot be deleted.");
            if (has_linked_purchase_invoice.Value)
                throw new UserFriendlyException("This payment mode is linked to a Purchase Invoice and cannot be deleted.");

            return await base.Delete(input);
        }
    }

    [AutoMap(typeof(PaymentModeInfo))]
    public class PaymentModeDto : SimpleDtoBase
    {

    }
}
