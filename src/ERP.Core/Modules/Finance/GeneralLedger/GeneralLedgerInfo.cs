using ERP.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Modules.Finance.GeneralLedger
{
    public class GeneralLedgerInfo : ERPProjBaseEntity
    {
        public DateTime IssueDate { get; set; }
        public string VoucherNumber { get; set; }
        public long ChartOfAccountId { get; set; }

        [Precision(16, 2)]
        public decimal? Credit { get; set; }

        [Precision(15, 2)]
        public decimal? Debit { get; set; }

        public bool IsAdjustmentEntry { get; set; }
        public string Status { get; set; }
        public long EmployeeId { get; set; }
        public long LinkedDocumentId { get; set; }
        public GeneralLedgerLinkedDocument LinkedDocument { get; set; }
        public long ReferenceDocumentId { get; set; }
        public string ReferenceVoucherNumber { get; set; }
        public GeneralLedgerLinkedDocument? ReferenceDocument { get; set; }
        public string Remarks { get; set; }

        [NotMapped]
        public int Days => (DateTime.Now.Date - IssueDate.Date).Days;
    }
}
