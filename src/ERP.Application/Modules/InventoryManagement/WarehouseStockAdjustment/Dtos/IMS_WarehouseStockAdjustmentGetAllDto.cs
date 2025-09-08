using System;
using Abp.AutoMapper;
using Abp.Authorization;
using ERP.Authorization;
using ERP.Generics;
using Abp.Domain.Entities;
using Abp.Application.Services.Dto;
using ERP.Generics.Simple;

namespace ERP.Modules.InventoryManagement.WarehouseStockAdjustment
{
    [AutoMap(typeof(WarehouseStockAdjustmentInfo))]
    public class IMS_WarehouseStockAdjustmentGetAllDto : Entity<long>
    {
        public DateTime IssueDate { get; set; }
        public string VoucherNumber { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
        public decimal MinStockLevel { get; set; }
        public long UnitId { get; set; }
        public long WarehouseId { get; set; }
        
    }
}
