using Abp.Application.Services.Dto;
using ERP.Controllers;
using ERP.Modules.SalesManagement.SalesTracking;
using ERP.Modules.SalesManagement.SalesTracking.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ERP.Web.Core.Controllers
{
    [Route("api/services/app/[controller]")]
    public class SalesTrackingController : ERPControllerBase
    {
        private readonly SalesTrackingAppService _salesTrackingAppService;

        public SalesTrackingController(SalesTrackingAppService salesTrackingAppService)
        {
            _salesTrackingAppService = salesTrackingAppService;
        }

        [HttpGet("summary")]
        public async Task<PagedResultDto<SalesTrackingDto>> GetSalesTrackingSummary([FromQuery] SalesTrackingFiltersDto filters)
        {
            return await _salesTrackingAppService.GetSalesTrackingSummary(filters);
        }

        [HttpGet("details/{customerId}")]
        public async Task<SalesTrackingDetailsDto> GetSalesTrackingDetails(long customerId)
        {
            return await _salesTrackingAppService.GetSalesTrackingDetails(customerId);
        }
    }
} 