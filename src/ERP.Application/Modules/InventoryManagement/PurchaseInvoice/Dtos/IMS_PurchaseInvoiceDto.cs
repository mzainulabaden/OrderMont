using System;
using Abp.AutoMapper;
using Abp.Authorization;
using ERP.Authorization;
using ERP.Generics;
using Abp.Domain.Entities;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using ERP.Generics.Simple;

namespace ERP.Modules.InventoryManagement.PurchaseInvoice
{
    [AutoMap(typeof(IMS_PurchaseInvoiceInfo))]
    public class IMS_PurchaseInvoiceDto : Entity<long>
    {
        public string PONumber { get; set; }
        public string ExternalRefNumber { get; set; }
        public long VendorId { get; set; }
        public DateTime ExFactoryDate { get; set; }
        public DateTime PlacedOrderDate { get; set; }
        public long StatusId { get; set; }
        public string Memo { get; set; }
        public decimal DepositTotal { get; set; }
        public decimal BilledTotal { get; set; }
        public decimal UnBilledTotal { get; set; }
        public decimal DepositAppliedTotal { get; set; }
        public DateTime IssueDate { get; set; }
        public string Remarks { get; set; }
        public List<IMS_PurchaseInvoiceDetailsDto> IMS_PurchaseInvoiceDetails { get; set; }
    }

    [AutoMap(typeof(IMS_PurchaseInvoiceDetailsInfo))]
    public class IMS_PurchaseInvoiceDetailsDto : Entity<long>
    {
        public long ItemId { get; set; }
        public decimal Rate { get; set; }
        public DateTime ExFactoryDate { get; set; }
        public decimal OrderQty { get; set; }
        public decimal RecievedQty { get; set; }
        public decimal OnShipment { get; set; }
    }
}
