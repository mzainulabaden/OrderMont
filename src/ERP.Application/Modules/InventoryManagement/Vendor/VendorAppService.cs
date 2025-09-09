using System;
using Abp.AutoMapper;
using Abp.Domain.Entities;
using Abp.Authorization;
using ERP.Authorization;
using ERP.Generics;
using ERP.Generics.Simple;

namespace ERP.Modules.InventoryManagement.Vendor
{
    [AbpAuthorize(PermissionNames.LookUps_IMS_Vendor)]
    public class VendorAppService : GenericSimpleAppService<IMS_VendorDto, VendorInfo, SimpleSearchDtoBase>
    {
    }

    [AutoMap(typeof(VendorInfo))]
    public class IMS_VendorDto : SimpleDtoBase
    {
        public decimal Incoterm { get; set; }
        public decimal PaymentTerm { get; set; }
        public string ContactName { get; set; }
        public string ContactEmail { get; set; }
        public decimal PercentageAtPo { get; set; }
    }
}
