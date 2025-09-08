using Abp.Domain.Entities;
using ERP.Enums;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace ERP.Modules.Finance.GeneralNote;

public class GeneralNoteInfo : ERPDocumentBaseEntity
{
    public string? Title { get; set; }
    public bool? IsCreditNature { get; set; }
    public string? NoteIndex { get; set; }
    public string? AccountClass { get; set; }
    public GeneralNoteType? GeneralNoteType { get; set; }
    public List<GeneralNoteDetailsInfo>? GeneralNoteDetails { get; set; }
}

[Index(
    nameof(COALvl1Id),
    nameof(COALvl2Id),
    nameof(COALvl3Id),
    nameof(COALvl4Id)
)]
public class GeneralNoteDetailsInfo : Entity<long>
{
    public long? COALvl1Id { get; set; }
    public long? COALvl2Id { get; set; }
    public long? COALvl3Id { get; set; }
    public long? COALvl4Id { get; set; }
    public string? GeneralNoteVoucherPrefix { get; set; }
    public string? GeneralNoteVoucherIndex { get; set; }
    public string? StockVoucherPrefix { get; set; }
    public string? StockVoucherIndex { get; set; }
    public string? FixedAssetVoucherPrefix { get; set; }
    public string? FixedAssetVoucherIndex { get; set; }
    public TransactionType? TransactionType { get; set; }
    public string? VoucherNumber { get; set; }
    public long? GeneralNoteInfoId { get; set; }
}
