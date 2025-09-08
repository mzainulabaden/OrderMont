using Abp.AutoMapper;
using ERP.Generics;
using System.Collections.Generic;

namespace ERP.Modules.SalesManagement.SalesReturn
{
    [AutoMap(typeof(SalesReturnInfo))]
    public class SalesReturnGetAllDto : BaseDocumentGetAllDto
    {
        public string ReferenceNumber { get; set; }
        public long CustomerCOALevel04Id { get; set; }
        public string CustomerCOALevel04Name { get; set; }
        public bool IsReturnAgainstSalesInvoice { get; set; }
        public decimal TotalAmount { get; set; }
        public List<string> SalesInvoiceIds { get; set; }
        // If needed, add: public List<SalesReturnDetailsDto> SalesReturnDetails { get; set; }
    }
}
