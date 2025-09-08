using System;

namespace ERP;

public class ERPDocumentBaseEntity : ERPProjBaseEntity
{
    public DateTime IssueDate { get; set; }
    public string VoucherNumber { get; set; }
    public string  Status { get; set; }
    public string Remarks { get; set; }
}