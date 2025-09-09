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
    public class IMS_PurchaseInvoiceFiltersDto : BaseDocumentFiltersDto
    {
        public string VendorId { get; set; }
        public string StatusId { get; set; }
    }
}
