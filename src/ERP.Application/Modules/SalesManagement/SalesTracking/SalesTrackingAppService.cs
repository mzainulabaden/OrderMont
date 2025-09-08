using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using ERP.Authorization;
using ERP.Authorization.Users;
using ERP.Enums;
using ERP.Modules.Finance.ChartOfAccount.COALevel04;
using ERP.Modules.SalesManagement.SalesInvoice;
using ERP.Modules.SalesManagement.SalesOrder;
using ERP.Modules.SalesManagement.SalesTracking.Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.Modules.SalesManagement.SalesTracking
{
    [AbpAuthorize(PermissionNames.LookUps_SalesTracking)]
    public class SalesTrackingAppService : ERPApplicationService
    {
        public IRepository<SalesOrderInfo, long> SalesOrder_Repo { get; set; }
        public IRepository<SalesInvoiceInfo, long> SalesInvoice_Repo { get; set; }
        public IRepository<COALevel04Info, long> Customer_Repo { get; set; }
        public IRepository<User, long> User_Repo { get; set; }

        public async Task<PagedResultDto<SalesTrackingDto>> GetSalesTrackingSummary(SalesTrackingFiltersDto filters)
        {
            var query = Customer_Repo.GetAll(this)
                .Where(c => c.NatureOfAccount == NatureOfAccount.Client);

            // Apply customer filters
            if (!string.IsNullOrWhiteSpace(filters.CustomerId))
                query = query.Where(c => c.Id == filters.CustomerId.TryToLong());

            if (!string.IsNullOrWhiteSpace(filters.CustomerName))
                query = query.Where(c => c.Name.ToLower().Contains(filters.CustomerName.ToLower()));

            var customers = await query.ToListAsync();

            var result = new List<SalesTrackingDto>();

            foreach (var customer in customers)
            {
                var salesOrderQuery = SalesOrder_Repo.GetAll(this)
                    .Where(so => so.CustomerCOALevel04Id == customer.Id);

                var salesInvoiceQuery = SalesInvoice_Repo.GetAll(this)
                    .Where(si => si.CustomerCOALevel04Id == customer.Id);

                // Apply date filters
                if (filters.FromDate.HasValue)
                {
                    salesOrderQuery = salesOrderQuery.Where(so => so.IssueDate >= filters.FromDate.Value);
                    salesInvoiceQuery = salesInvoiceQuery.Where(si => si.IssueDate >= filters.FromDate.Value);
                }

                if (filters.ToDate.HasValue)
                {
                    salesOrderQuery = salesOrderQuery.Where(so => so.IssueDate <= filters.ToDate.Value);
                    salesInvoiceQuery = salesInvoiceQuery.Where(si => si.IssueDate <= filters.ToDate.Value);
                }

                // Apply status filters
                if (!string.IsNullOrWhiteSpace(filters.Status))
                {
                    salesOrderQuery = salesOrderQuery.Where(so => so.Status == filters.Status);
                    salesInvoiceQuery = salesInvoiceQuery.Where(si => si.Status == filters.Status);
                }

                var salesOrders = await salesOrderQuery.ToListAsync();
                var salesInvoices = await salesInvoiceQuery.ToListAsync();

                // Only include customers with sales data
                if (salesOrders.Any() || salesInvoices.Any())
                {
                    var trackingDto = new SalesTrackingDto
                    {
                        Id = customer.Id,
                        CustomerId = customer.Id,
                        CustomerName = customer.Name,
                        TotalSalesOrders = salesOrders.Count,
                        TotalSalesInvoices = salesInvoices.Count,
                        TotalSalesOrderAmount = salesOrders.Sum(so => so.TotalAmount),
                        TotalSalesInvoiceAmount = salesInvoices.Sum(si => si.GrandTotal),
                        SalesOrderStatus = GetMostCommonStatus(salesOrders.Select(so => so.Status).ToList()),
                        SalesInvoiceStatus = GetMostCommonStatus(salesInvoices.Select(si => si.Status).ToList())
                    };

                    result.Add(trackingDto);
                }
            }

            // Apply pagination
            var pagedResult = result
                .OrderBy(x => x.CustomerName)
                .Skip(filters.SkipCount)
                .Take(filters.MaxResultCount)
                .ToList();

            return new PagedResultDto<SalesTrackingDto>(result.Count, pagedResult);
        }

        public async Task<SalesTrackingDetailsDto> GetSalesTrackingDetails(long customerId)
        {
            var customer = await Customer_Repo.GetAll(this)
                .Where(c => c.Id == customerId && c.NatureOfAccount == NatureOfAccount.Client)
                .FirstOrDefaultAsync();

            if (customer == null)
                throw new Abp.UI.UserFriendlyException($"Customer with ID {customerId} not found.");

            var salesOrders = await SalesOrder_Repo.GetAll(this)
                .Where(so => so.CustomerCOALevel04Id == customerId)
                .OrderByDescending(so => so.IssueDate)
                .ToListAsync();

            var salesInvoices = await SalesInvoice_Repo.GetAll(this)
                .Where(si => si.CustomerCOALevel04Id == customerId)
                .OrderByDescending(si => si.IssueDate)
                .ToListAsync();

            // Get creator information from AbpUsers table
            var creatorUserIds = salesOrders.Where(so => so.CreatorUserId.HasValue)
                .Select(so => so.CreatorUserId.Value)
                .Union(salesInvoices.Where(si => si.CreatorUserId.HasValue).Select(si => si.CreatorUserId.Value))
                .Distinct()
                .ToList();

            var creators = await User_Repo.GetAll()
                .Where(u => creatorUserIds.Contains(u.Id))
                .ToListAsync();

            var creatorUserMap = creators.ToDictionary(
                c => c.Id,
                c => c.Name ?? "Unknown"
            );

            var result = new SalesTrackingDetailsDto
            {
                Id = customer.Id,
                CustomerId = customer.Id,
                CustomerName = customer.Name,
                SalesOrders = salesOrders.Select(so => new SalesOrderSummaryDto
                {
                    Id = so.Id,
                    SalesOrderNumber = so.ReferenceNumber,
                    SalesmanName = so.CreatorUserId.HasValue ? creatorUserMap.GetValueOrDefault(so.CreatorUserId.Value, "Unknown") : "Not Assigned",
                    TotalAmount = so.TotalAmount,
                    Status = so.Status,
                    IssueDate = so.IssueDate,
                    VoucherNumber = so.VoucherNumber
                }).ToList(),
                SalesInvoices = salesInvoices.Select(si => new SalesInvoiceSummaryDto
                {
                    Id = si.Id,
                    SalesInvoiceNumber = si.ReferenceNumber,
                    SalesmanName = si.CreatorUserId.HasValue ? creatorUserMap.GetValueOrDefault(si.CreatorUserId.Value, "Unknown") : "Not Assigned",
                    GrandTotal = si.GrandTotal,
                    Status = si.Status,
                    IssueDate = si.IssueDate,
                    VoucherNumber = si.VoucherNumber
                }).ToList()
            };

            return result;
        }

        private string GetMostCommonStatus(List<string> statuses)
        {
            if (!statuses.Any())
                return "Pending";

            return statuses
                .GroupBy(s => s)
                .OrderByDescending(g => g.Count())
                .FirstOrDefault()?.Key ?? "Pending";
        }
    }
} 