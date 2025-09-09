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
    public class IMS_ItemFiltersDto : BaseFiltersDto
    {
        public string StatusId { get; set; }
        public string CategoryId { get; set; }
        public string VendorId { get; set; }
    }
}
