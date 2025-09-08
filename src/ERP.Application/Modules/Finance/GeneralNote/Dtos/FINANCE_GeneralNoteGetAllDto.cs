using System;
using Abp.AutoMapper;
using Abp.Authorization;
using ERP.Authorization;
using ERP.Generics;
using Abp.Domain.Entities;
using Abp.Application.Services.Dto;
using ERP.Generics.Simple;

namespace ERP.Modules.Finance.GeneralNote
{
    [AutoMap(typeof(GeneralNoteInfo))]
    public class FINANCE_GeneralNoteGetAllDto : Entity<long>
    {
        public string? Title { get; set; }
        public bool? IsCreditNature { get; set; }
        public string? NoteIndex { get; set; }
        public string? AccountClass { get; set; }
        public int? GeneralNoteType { get; set; }
        public DateTime? IssueDate { get; set; }
        public string? VoucherNumber { get; set; }
        public string? Status { get; set; }
        public string? Remarks { get; set; }
    }
}
