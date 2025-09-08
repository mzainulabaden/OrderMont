using System;
using Abp.AutoMapper;
using Abp.Domain.Entities;
using Abp.Authorization;
using ERP.Authorization;
using ERP.Generics;
using ERP.Generics.Simple;

namespace ERP.Modules.HumanResource.CompanyProfile
{
    [AbpAuthorize(PermissionNames.LookUps_CompanyProfile)]
    public class CompanyProfileAppService : GenericSimpleAppService<CompanyProfileDto, CompanyProfileInfo, SimpleSearchDtoBase>
    {
    }

    [AutoMap(typeof(CompanyProfileInfo))]
    public class CompanyProfileDto : SimpleDtoBase
    {
        public string Logo { get; set; }
        public string NTN { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
    }
}
