using System;
using Abp.AutoMapper;
using Abp.Authorization;
using ERP.Authorization;
using ERP.Generics;
using Abp.Domain.Entities;
using Abp.Application.Services.Dto;
using ERP.Generics.Simple;

namespace ERP.Modules.Finance.LookUps
{
    [AutoMap(typeof(DefaultIntegrationsInfo))]
    public class FINANCE_DefaultIntegrationsDto : Entity<long>
    {
        public long ChartOfAccountId { get; set; }
        public string Remarks { get; set; }
        public string Name { get; set; }
    }
}
