using System.Collections.Generic;

namespace ERP.Modules.Finance.ChartOfAccount.COALevel03
{
    public class COALevel03BulkUploadDto
    {
        public string Name { get; set; }
        public string SerialNumber { get; set; }
        public string COALevel02Name { get; set; }
        public string AccountTypeName { get; set; }
    }

    public class COALevel03BulkUploadRequestDto
    {
        public List<COALevel03BulkUploadDto> Items { get; set; }
    }

    public class COALevel03BulkUploadResultDto
    {
        public int TotalItems { get; set; }
        public int SuccessCount { get; set; }
        public int FailureCount { get; set; }
        public List<string> Errors { get; set; }

        public COALevel03BulkUploadResultDto()
        {
            Errors = new List<string>();
        }
    }
} 