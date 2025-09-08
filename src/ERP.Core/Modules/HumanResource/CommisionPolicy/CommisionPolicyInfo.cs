using Abp.Domain.Entities;
using ERP.Enums;
using ERP.Generics;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace ERP.Modules.HumanResource.CommisionPolicy
{
    //ERPDocumentBaseEntity
    public class CommissionPolicyInfo : SimpleEntityBase
    {
        public PolicyType PolicyType { get; set; }

        [Precision(16, 2)]
        public decimal? CommisionAmount { get; set; }

        [Precision(16, 2)]
        public decimal? CommisionPercentage { get; set; }

        public List<CommissionPolicyDetailsInfo> CommissionPolicyDetails { get; set; }
    }

    public class CommissionPolicyDetailsInfo : Entity<long>
    {
        public long FromAmount { get; set; }
        public long ToAmount { get; set; }
        public long SalesCommisionAmount { get; set; }
    }
}
