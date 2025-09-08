using Abp.AutoMapper;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;

namespace ERP.Modules.Finance.JournalVoucher
{
    [AutoMap(typeof(JournalVoucherInfo))]
    public class FINANCE_JournalVoucherDto : Entity<long>
    {
        public DateTime IssueDate { get; set; }
        public string Remarks { get; set; }
        public List<JournalVoucherDetailsDto> JournalVoucherDetails { get; set; }
    }

    [AutoMap(typeof(JournalVoucherDetailsInfo))]
    public class JournalVoucherDetailsDto : Entity<long>
    {
        public long COAlvl4Id { get; set; }
        public decimal Credit { get; set; }
        public decimal Debit { get; set; }
        public string Remarks { get; set; }
    }
}
