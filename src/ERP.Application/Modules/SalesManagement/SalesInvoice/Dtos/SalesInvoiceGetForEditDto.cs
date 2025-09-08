using Abp.AutoMapper;
using System.Collections.Generic;

namespace ERP.Modules.SalesManagement.SalesInvoice
{
    [AutoMap(typeof(SalesInvoiceInfo))]
    public class SalesInvoiceGetForEditDto : SalesInvoiceGetAllDto
    {
        public List<SalesInvoiceDetailsGetForEditDto> SalesInvoiceDetails { get; set; }
     
    }

    [AutoMap(typeof(SalesInvoiceDetailsInfo))]
    public class SalesInvoiceDetailsGetForEditDto : SalesInvoiceDetailsDto
    {
        public string ItemName { get; set; }
        public string UnitName { get; set; }
       
    }
}
