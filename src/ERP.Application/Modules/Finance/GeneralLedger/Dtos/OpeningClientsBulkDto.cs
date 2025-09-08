using System;
using System.Collections.Generic;

namespace ERP.Modules.Finance.GeneralLedger.Dtos
{
    public class OpeningClientsBulkItemDto
    {
        public DateTime IssueDate { get; set; }
        public string VoucherNumber { get; set; }
        public string COALevel04Name { get; set; }
        public decimal? Credit { get; set; }
        public decimal? Debit { get; set; }
        public string Remarks { get; set; }
    }

    public class OpeningClientsBulkRequestDto
    {
        public List<OpeningClientsBulkItemDto> Items { get; set; }
    }

    public class OpeningClientsBulkResultDto
    {
        public int TotalItems { get; set; }
        public int SuccessCount { get; set; }
        public int FailureCount { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public string ErrorFilePath { get; set; }
    }
}


