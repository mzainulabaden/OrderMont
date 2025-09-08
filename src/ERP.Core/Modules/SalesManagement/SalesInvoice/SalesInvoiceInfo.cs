using Abp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Modules.SalesManagement.SalesInvoice
{
    [Index(
        nameof(CustomerCOALevel04Id),
        nameof(TaxCOALevel04Id)
        )]
    public class SalesInvoiceInfo : ERPDocumentBaseEntity
    {
        public string ReferenceNumber { get; set; } 
        public long PaymentModeId { get; set; }
        public long CustomerCOALevel04Id { get; set; }
        public long? AdvanceAmountBankCOALevl04Id { get; set; }
        public long TaxCOALevel04Id { get; set; }
        public long EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        

        [Precision(16, 2)]
        public decimal CommissionAmount { get; set; }

        [Precision(16, 2)]
        public decimal GrandTotal { get; set; }

        [Precision(16, 2)]
        public decimal DiscountPercentage { get; set; }

        [Precision(16, 2)]
        public decimal DiscountAmount { get; set; }

        [Precision(16, 2)]
        public decimal FreightAmount { get; set; }

        [Precision(16, 2)]
        public decimal TaxAmount { get; set; }

        [Precision(16, 2)]
        public decimal NetTotal { get; set; }

        [Precision(16, 2)]
        public decimal AdvanceAmount { get; set; }
        public List<string> AttachedDocuments { get; set; }
        public List<long> SalesOrderIds { get; set; }
        public List<SalesInvoiceDetailsInfo> SalesInvoiceDetails { get; set; }
    }

    [Index(
        nameof(ItemId)
        )]
    [Table("SALES_SalesInvoiceDetailsInfo")]
    public class SalesInvoiceDetailsInfo : Entity<long>
    {
        public long ItemId { get; set; }

        [Precision(16, 2)]
        public decimal ItemMinRate { get; set; }

        [Precision(16, 2)]
        public decimal ItemMaxRate { get; set; }

        [Precision(16, 2)]
        public decimal MinStockLevel { get; set; }

        public long UnitId { get; set; }

        [Precision(16, 2)]
        public decimal Rate { get; set; }

        [Precision(16, 2)]
        public decimal PricePerKg { get; set; }

        [Precision(16, 2)]
        public decimal ProfitPercentage { get; set; }

        [Precision(16, 2)]
        public decimal ProfitAmount { get; set; }

        [Precision(16, 2)]
        public decimal InvoiceQty { get; set; }

        [Precision(16, 2)]
        public decimal LastSaleRate { get; set; }

        [Precision(16, 2)]
        public decimal GrandTotal { get; set; }
        public long WarehouseId { get; set; }
        public long SalesOrderDetailId { get; set; }
        public long SalesInvoiceInfoId { get; set; }
        public decimal RemainingQty { get; set; }
    }
}