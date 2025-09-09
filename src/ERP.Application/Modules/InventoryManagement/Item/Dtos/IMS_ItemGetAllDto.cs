using System;
using Abp.AutoMapper;
using Abp.Authorization;
using ERP.Authorization;
using ERP.Generics;
using Abp.Domain.Entities;
using Abp.Application.Services.Dto;
using ERP.Generics.Simple;

namespace ERP.Modules.InventoryManagement.Item
{
    [AutoMap(typeof(ItemInfo))]
    public class IMS_ItemGetAllDto : Entity<long>
    {
        public string SKU { get; set; }
        public string Status { get; set; }
        public long StatusId { get; set; }
        public string StatusName { get; set; }
        public long CategoryId { get; set; }
        public string CategoryName { get; set; }
        public long VendorId { get; set; }
        public string VendorName { get; set; }
        public decimal QtyPerCase { get; set; }
        public decimal LandedCost { get; set; }
        public decimal ListPrice { get; set; }
        public decimal HSCode { get; set; }
        public string ManufacturingLeadTime { get; set; }
        public decimal LastPurchaseCost { get; set; }
        public decimal GTIN { get; set; }
        public decimal CartonLengthInches { get; set; }
        public decimal OceanLeadTime { get; set; }
        public decimal CartonHeightInches { get; set; }
        public decimal CartonWeigthLB { get; set; }
        public decimal CartonWidthInches { get; set; }
        public decimal UnitLengthInches { get; set; }
        public decimal UnitWidthInches { get; set; }
        public decimal UnitWeightLB { get; set; }
        public decimal UnitHeightInches { get; set; }
        public decimal ASIN { get; set; }
        public decimal Rate { get; set; }
        public string ManufacturingTime { get; set; }
        public string Name { get; set; }
    }
}
