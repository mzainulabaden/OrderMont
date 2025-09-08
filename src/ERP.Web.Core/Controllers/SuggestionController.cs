using Abp.Application.Services.Dto;
using ERP.Modules.Suggestion;
using ERP.Modules.Suggestion.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.Controllers
{
    [Route("api/services/app/[controller]")]
    public class SuggestionController : ERPControllerBase
    {
        private readonly SuggestionAppService _suggestionAppService;

        public SuggestionController(SuggestionAppService suggestionAppService)
        {
            _suggestionAppService = suggestionAppService;
        }

        [HttpGet("suppliers-and-clients")]
        public async Task<object> GetAllSuppliersAndClients([FromQuery] SuggestionFilterDto filters)
        {
            // Get suppliers
            var suppliers = await _suggestionAppService.GetSuggestions("Supplier", filters);
            
            // Get clients
            var clients = await _suggestionAppService.GetSuggestions("Client", filters);

            // Add type information to each item
            var suppliersWithType = suppliers.Items.Select(s => new 
            {
                s.Id,
                s.Name,
                s.Additional,
                Type = "Supplier"
            });

            var clientsWithType = clients.Items.Select(c => new 
            {
                c.Id,
                c.Name,
                c.Additional,
                Type = "Client"
            });

            // Combine both lists
            var combined = suppliersWithType.Concat(clientsWithType).ToList();

            return new
            {
                Items = combined,
                Suppliers = suppliersWithType,
                Clients = clientsWithType,
                TotalSuppliers = suppliers.TotalCount,
                TotalClients = clients.TotalCount,
                TotalCount = suppliers.TotalCount + clients.TotalCount
            };
        }

        [HttpGet("suppliers")]
        public async Task<object> GetAllSuppliers([FromQuery] SuggestionFilterDto filters)
        {
            var result = await _suggestionAppService.GetSuggestions("Supplier", filters);
            
            var suppliersWithType = result.Items.Select(s => new 
            {
                s.Id,
                s.Name,
                s.Additional,
                Type = "Supplier"
            }).ToList();

            return new
            {
                Items = suppliersWithType,
                TotalCount = result.TotalCount
            };
        }

        [HttpGet("clients")]
        public async Task<object> GetAllClients([FromQuery] SuggestionFilterDto filters)
        {
            var result = await _suggestionAppService.GetSuggestions("Client", filters);
            
            var clientsWithType = result.Items.Select(c => new 
            {
                c.Id,
                c.Name,
                c.Additional,
                Type = "Client"
            }).ToList();

            return new
            {
                Items = clientsWithType,
                TotalCount = result.TotalCount
            };
        }
    }
} 