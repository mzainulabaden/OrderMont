using System;
using Abp.AutoMapper;
using Abp.Authorization;
using ERP.Authorization;
using ERP.Generics;
using Abp.Domain.Entities;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using ERP.Generics.Simple;

namespace ERP.Modules.Finance.GeneralNote
{
    [AutoMap(typeof(GeneralNoteInfo))]
    public class FINANCE_GeneralNoteGetForEditDto : Entity<long>
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
        public List<GeneralNoteDetailsGetForEditDto>? GeneralNoteDetails { get; set; }
    }

    [AutoMap(typeof(GeneralNoteDetailsInfo))]
    public class GeneralNoteDetailsGetForEditDto : Entity<long>
    {
        public long? COALvl1Id { get; set; }
        public string? COALvl1Name { get; set; }
        public long? COALvl2Id { get; set; }
        public string? COALvl2Name { get; set; }
        public long? COALvl3Id { get; set; }
        public string? COALvl3Name { get; set; }
        public long? COALvl4Id { get; set; }
        public string? COALvl4Name { get; set; }
        public string? GeneralNoteVoucherPrefix { get; set; }
        public string? GeneralNoteVoucherIndex { get; set; }
        public string? StockVoucherPrefix { get; set; }
        public string? StockVoucherIndex { get; set; }
        public string? FixedAssetVoucherPrefix { get; set; }
        public string? FixedAssetVoucherIndex { get; set; }
        public int? TransactionType { get; set; }
        public string? VoucherNumber { get; set; }
    }
}
