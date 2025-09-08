using System.Collections.Generic;

namespace ERP.Modules.Finance.ChartOfAccount.COALevel01
{
    public class COALevel01BulkUploadDto
    {
        public string Name { get; set; }
        public string SerialNumber { get; set; }
        public string AccountTypeName { get; set; }
    }

    public class COALevel01BulkUploadRequestDto
    {
        public List<COALevel01BulkUploadDto> Items { get; set; }
    }

    public class COALevel01BulkUploadResultDto
    {
        public int TotalItems { get; set; }
        public int SuccessCount { get; set; }
        public int FailureCount { get; set; }
        public List<string> Errors { get; set; }

        public COALevel01BulkUploadResultDto()
        {
            Errors = new List<string>();
        }
    }
} 