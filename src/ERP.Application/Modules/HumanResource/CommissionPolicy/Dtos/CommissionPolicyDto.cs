using Abp.AutoMapper;
using ERP.Enums;
using ERP.Generics;
using ERP.Modules.HumanResource.CommisionPolicy;
using System.Collections.Generic;

namespace ERP.Modules.HumanResource.CommissionPolicy
{
    [AutoMap(typeof(CommissionPolicyInfo))]
    public class CommissionPolicyDto : SimpleDtoBase
    {
        public PolicyType PolicyType { get; set; }
        public decimal? CommisionAmount { get; set; }
        public decimal? CommisionPercentage { get; set; }

        public List<CommissionPolicyDetailsInfo> CommissionPolicyDetails { get; set; } = new();
    }
}