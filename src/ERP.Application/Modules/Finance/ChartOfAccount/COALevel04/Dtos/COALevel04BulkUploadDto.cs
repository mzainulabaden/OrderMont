using System.Collections.Generic;
using ERP.Enums;

namespace ERP.Modules.Finance.ChartOfAccount.COALevel04
{
    public class COALevel04BulkUploadDto
    {
        public string Name { get; set; }
        public string SerialNumber { get; set; }
        public string COALevel03Name { get; set; }
        public string AccountTypeName { get; set; }
        public string CurrencyName { get; set; }
        public string LinkWithName { get; set; }
        public string NatureOfAccount { get; set; }
        
        // Properties specifically for Client and Supplier
        public string CNIC { get; set; }
        public string EmailAddress { get; set; }
        public string ContactNumber { get; set; }
        public string PhysicalAddress { get; set; }
        public string SalesTaxNumber { get; set; }
        public string NationalTaxNumber { get; set; }
    }

    public class COALevel04BulkUploadRequestDto
    {
        public List<COALevel04BulkUploadDto> Items { get; set; }
    }

    public class COALevel04BulkUploadResultDto
    {
        public int TotalItems { get; set; }
        public int SuccessCount { get; set; }
        public int FailureCount { get; set; }
        public List<string> Errors { get; set; }

        public COALevel04BulkUploadResultDto()
        {
            Errors = new List<string>();
        }
    }
} 