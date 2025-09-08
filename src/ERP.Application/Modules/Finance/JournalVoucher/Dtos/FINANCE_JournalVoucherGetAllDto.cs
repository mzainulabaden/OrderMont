using System;
using Abp.AutoMapper;
using Abp.Authorization;
using ERP.Authorization;
using ERP.Generics;
using Abp.Domain.Entities;
using Abp.Application.Services.Dto;
using ERP.Generics.Simple;

namespace ERP.Modules.Finance.JournalVoucher
{
    [AutoMap(typeof(JournalVoucherInfo))]
    public class FINANCE_JournalVoucherGetAllDto : Entity<long>
    {
        public DateTime IssueDate { get; set; }
        public string VoucherNumber { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
    }
}
