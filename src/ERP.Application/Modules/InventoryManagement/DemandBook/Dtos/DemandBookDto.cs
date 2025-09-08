using System;
using Abp.AutoMapper;
using Abp.Authorization;
using ERP.Authorization;
using ERP.Generics;
using Abp.Domain.Entities;
using Abp.Application.Services.Dto;
using ERP.Generics.Simple;

namespace ERP.Modules.InventoryManagement.DemandBook
{
    [AutoMap(typeof(DemandBookInfo))]
    public class DemandBookDto : Entity<long>
    {
        public decimal Qty { get; set; }
        public long WarehouseId { get; set; }
        public string Name { get; set; }
        public long ItemId { get; set; }
    }
}
