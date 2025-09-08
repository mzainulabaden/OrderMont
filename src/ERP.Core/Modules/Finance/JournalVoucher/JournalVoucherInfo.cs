using Abp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace ERP.Modules.Finance.JournalVoucher
{
    public class JournalVoucherInfo : ERPDocumentBaseEntity
    {
        public List<JournalVoucherDetailsInfo> JournalVoucherDetails { get; set; }
    }

    [Index(
        nameof(COAlvl4Id)
    )]
    public class JournalVoucherDetailsInfo : Entity<long>
    {
        public long COAlvl4Id { get; set; }

        [Precision(19, 2)]
        public decimal Credit { get; set; }

        [Precision(19, 2)]
        public decimal Debit { get; set; }
        public string Remarks { get; set; }

        public long JournalVoucherInfoId { get; set; }
    }
}
