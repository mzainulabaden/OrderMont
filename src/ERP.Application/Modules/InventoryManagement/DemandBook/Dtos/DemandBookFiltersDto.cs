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
    public class DemandBookFiltersDto : BaseFiltersDto
    {
        public string WarehouseId { get; set; }
        public string ItemId { get; set; }
    }
}
