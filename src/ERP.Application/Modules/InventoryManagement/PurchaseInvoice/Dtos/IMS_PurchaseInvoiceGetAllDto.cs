using System;
using Abp.AutoMapper;
using Abp.Authorization;
using ERP.Authorization;
using ERP.Generics;
using Abp.Domain.Entities;
using Abp.Application.Services.Dto;
using ERP.Generics.Simple;

namespace ERP.Modules.InventoryManagement.PurchaseInvoice
{
    [AutoMap(typeof(PurchaseInvoiceInfo))]
    public class IMS_PurchaseInvoiceGetAllDto : Entity<long>
    {
        public string PONumber { get; set; }
        public string ExternalRefNumber { get; set; }
        public long VendorId { get; set; }
        public string VendorName { get; set; }
        public DateTime ExFactoryDate { get; set; }
        public DateTime PlacedOrderDate { get; set; }
        public long StatusId { get; set; }
        public string StatusName { get; set; }
        public string Memo { get; set; }
        public decimal DepositTotal { get; set; }
        public decimal BilledTotal { get; set; }
        public decimal UnBilledTotal { get; set; }
        public decimal DepositAppliedTotal { get; set; }
        public DateTime IssueDate { get; set; }
        public string VoucherNumber { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
    }
}
