using ERP.Generics;
using Microsoft.EntityFrameworkCore;
using System;

namespace ERP.Modules.Finance.LookUps
{
    [Index(nameof(ChartOfAccountId))]
    public class DefaultIntegrationsInfo : SimpleEntityBase
    {
        public long ChartOfAccountId { get; set; }
        public string Remarks { get; set; }
    }
}
