using System;
using Abp.AutoMapper;
using Abp.Authorization;
using ERP.Authorization;
using ERP.Generics;
using Abp.Domain.Entities;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using ERP.Generics.Simple;

namespace ERP.Modules.InventoryManagement.WarehouseStockAdjustment
{
    [AutoMap(typeof(WarehouseStockAdjustmentInfo))]
    public class IMS_WarehouseStockAdjustmentGetForEditDto : Entity<long>
    {
        public DateTime IssueDate { get; set; }
        public string VoucherNumber { get; set; }
        public string Remarks { get; set; }
        public List<WarehouseStockAdjustmentDetailsGetForEditDto> WarehouseStockAdjustmentDetails { get; set; }
    }

    [AutoMap(typeof(WarehouseStockAdjustmentDetailsInfo))]
    public class WarehouseStockAdjustmentDetailsGetForEditDto : Entity<long>
    {
        public long InventoryItemId { get; set; }
        public string InventoryItemName { get; set; }
        public decimal Credit { get; set; }
        public decimal Debit { get; set; }
        public decimal CostRate { get; set; }
        public decimal MinStockLevel { get; set; }
        public long UnitId { get; set; }
        public string UnitName { get; set; }
        public long WarehouseId { get; set; }
        public string WarehouseName { get; set; }
        public string Remarks { get; set; }
    }
}
