using ERP.Enums;
using System;

namespace ERP.Extensions.Common;

public class GeneralLedgerTransactionBaseDto
{
    public DateTime IssueDate { get; set; }
    public string VoucherNumber { get; set; }
    public bool IsAdjustmentEntry { get; set; }
    public long LinkedDocumentId { get; set; }
    public GeneralLedgerLinkedDocument LinkedDocument { get; set; }
    public string Remarks { get; set; }
    public int? TenantId { get; set; }
}
