using Abp.Application.Services.Dto;
using System.Collections.Generic;

namespace ERP.Modules.SalesManagement.SalesTracking.Dtos
{
    public class SalesTrackingDto : EntityDto<long>
    {
        public string CustomerName { get; set; }
        public long CustomerId { get; set; }
        public int TotalSalesOrders { get; set; }
        public int TotalSalesInvoices { get; set; }
        public decimal TotalSalesOrderAmount { get; set; }
        public decimal TotalSalesInvoiceAmount { get; set; }
        public string SalesOrderStatus { get; set; }
        public string SalesInvoiceStatus { get; set; }
    }

    public class SalesTrackingDetailsDto : EntityDto<long>
    {
        public long CustomerId { get; set; }
        public string CustomerName { get; set; }
        public List<SalesOrderSummaryDto> SalesOrders { get; set; }
        public List<SalesInvoiceSummaryDto> SalesInvoices { get; set; }
    }

    public class SalesOrderSummaryDto
    {
        public long Id { get; set; }
        public string SalesOrderNumber { get; set; }
        public string SalesmanName { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public System.DateTime IssueDate { get; set; }
        public string VoucherNumber { get; set; }
    }

    public class SalesInvoiceSummaryDto
    {
        public long Id { get; set; }
        public string SalesInvoiceNumber { get; set; }
        public string SalesmanName { get; set; }
        public decimal GrandTotal { get; set; }
        public string Status { get; set; }
        public System.DateTime IssueDate { get; set; }
        public string VoucherNumber { get; set; }
    }

    public class SalesTrackingFiltersDto : PagedResultRequestDto
    {
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public System.DateTime? FromDate { get; set; }
        public System.DateTime? ToDate { get; set; }
        public string Status { get; set; }
    }
} 