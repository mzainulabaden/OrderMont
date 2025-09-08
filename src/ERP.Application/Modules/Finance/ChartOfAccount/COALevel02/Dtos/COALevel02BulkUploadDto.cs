using System.Collections.Generic;

namespace ERP.Modules.Finance.ChartOfAccount.COALevel02
{
    public class COALevel02BulkUploadDto
    {
        public string Name { get; set; }
        public string SerialNumber { get; set; }
        public string COALevel01Name { get; set; }
        public string AccountTypeName { get; set; }
    }

    public class COALevel02BulkUploadRequestDto
    {
        public List<COALevel02BulkUploadDto> Items { get; set; }
    }

    public class COALevel02BulkUploadResultDto
    {
        public int TotalItems { get; set; }
        public int SuccessCount { get; set; }
        public int FailureCount { get; set; }
        public List<string> Errors { get; set; }

        public COALevel02BulkUploadResultDto()
        {
            Errors = new List<string>();
        }
    }
} 