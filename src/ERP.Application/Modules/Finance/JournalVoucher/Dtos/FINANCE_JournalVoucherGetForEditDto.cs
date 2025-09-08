using System;
using Abp.AutoMapper;
using Abp.Authorization;
using ERP.Authorization;
using ERP.Generics;
using Abp.Domain.Entities;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using ERP.Generics.Simple;

namespace ERP.Modules.Finance.JournalVoucher
{
    [AutoMap(typeof(JournalVoucherInfo))]
    public class FINANCE_JournalVoucherGetForEditDto : Entity<long>
    {
        public DateTime IssueDate { get; set; }
        public string VoucherNumber { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
        public List<JournalVoucherDetailsGetForEditDto> JournalVoucherDetails { get; set; }
    }

    [AutoMap(typeof(JournalVoucherDetailsInfo))]
    public class JournalVoucherDetailsGetForEditDto : Entity<long>
    {
        public long COAlvl4Id { get; set; }
        public string COAlvl4Name { get; set; }
        public decimal Credit { get; set; }
        public decimal Debit { get; set; }
        public string Remarks { get; set; }
    }
}
