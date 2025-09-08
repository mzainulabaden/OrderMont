using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.UI;
using Abp.Authorization;
using ERP.Authorization;
using ERP.Modules.HumanResource.AttendanceManagement;
using ERP.Modules.HumanResource.EmployeeManagement;
using ERP.Modules.SalesManagement.SalesInvoice;
using ERP.Modules.SalesManagement.SalesOrder;
using ERP.Modules.InventoryManagement.PurchaseInvoice;
using ERP.Modules.InventoryManagement.PurchaseOrder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ERP.Modules.Finance.ChartOfAccount.COALevel04;
using ERP.Enums;
using ERP.Modules.Dashboard.Dtos;

namespace ERP.Modules.Dashboard
{
    [AbpAuthorize]
    public class DashboardAppService : ApplicationService
    {
        public IRepository<AttendanceInfo, long> Attendance_Repo { get; set; }
        public IRepository<SalesInvoiceInfo, long> SaleInvoice_Repo { get; set; }
        public IRepository<EmployeeInfo, long> Employee_Repo { get; set; }
        public IRepository<COALevel04Info, long> COALevel04_Repo { get; set; }
        public IRepository<PurchaseInvoiceInfo, long> PurchaseInvoice_Repo { get; set; }
        public IRepository<PurchaseOrderInfo, long> PurchaseOrder_Repo { get; set; }
        public IRepository<SalesOrderInfo, long> SalesOrder_Repo { get; set; }


        [AbpAuthorize(PermissionNames.LookUps_Attendance)]
        public async Task<EmployeeAttendanceStatsDto> GetEmployeeAttendance(DateTime? date = null)
        {
            var tenantId = AbpSession.TenantId;
            var targetDate = date?.Date ?? DateTime.Today;

            var allEmployees = await Employee_Repo.GetAllListAsync(x => 
                x.IsActive && !x.IsDeleted && x.TenantId == tenantId);
            var totalEmployees = allEmployees.Count;

            var presentEmployees = await Attendance_Repo.GetAllListAsync(x => 
                x.AttendanceDate.Date == targetDate && x.TenantId == tenantId);

            var presentCount = presentEmployees.Count;
            var absentCount = totalEmployees - presentCount;

            return new EmployeeAttendanceStatsDto
            {
                Date = targetDate,
                TotalEmployees = totalEmployees,
                PresentEmployees = presentCount,
                AbsentEmployees = absentCount,
                AttendanceDetails = presentEmployees.Select(x =>
                {
                    var employee = allEmployees.FirstOrDefault(e => e.Id == x.EmployeeId);
                    return new DashboardEmployeeDto
                    {
                        EmployeeId = x.EmployeeId,
                        EmployeeName = employee?.Name ?? "Unknown",
                        CheckInTime = x.CheckIn_Time
                    };
                }).ToList()
            };
        }

        [AbpAuthorize(PermissionNames.LookUps_COALevel04)]
        public async Task<BusinessPartnersDto> GetBusinessPartnersCount()
        {
            var currentTenantId = AbpSession.TenantId;

            if (currentTenantId == null)
            {
                throw new UserFriendlyException("Tenant ID is required to fetch business partners.");
            }

            var allAccounts = await COALevel04_Repo.GetAllListAsync(x =>
                x.TenantId == currentTenantId &&
                !x.IsDeleted &&
                (x.NatureOfAccount == NatureOfAccount.Supplier || x.NatureOfAccount == NatureOfAccount.Client ||x.NatureOfAccount==NatureOfAccount.Bank || x.NatureOfAccount==NatureOfAccount.Broker ));

            var suppliers = allAccounts.Where(x => x.NatureOfAccount == NatureOfAccount.Supplier).ToList();
            var clients = allAccounts.Where(x => x.NatureOfAccount == NatureOfAccount.Client).ToList();
            var Banks = allAccounts.Where(x =>x.NatureOfAccount==NatureOfAccount.Bank).ToList();
            var Brokers = allAccounts.Where(x =>x.NatureOfAccount==NatureOfAccount.Broker).ToList();

            return new BusinessPartnersDto
            {
                TotalSuppliers = suppliers.Count,
                TotalClients = clients.Count,
                TotalBanks=Banks.Count,
                TotalBrokers=Brokers.Count
                
            };
        }

        [AbpAuthorize(PermissionNames.LookUps_PurchaseInvoice, PermissionNames.LookUps_PurchaseOrder, 
                     PermissionNames.LookUps_SalesInvoice, PermissionNames.LookUps_SalesOrder)]
        public async Task<PurchaseSalesStatsDto> GetSalesStats(DateTime? startDate = null, DateTime? endDate = null)
        {
            var tenantId = AbpSession.TenantId;

            if (tenantId == null)
            {
                throw new UserFriendlyException("Tenant ID is required to fetch statistics.");
            }

            var purchaseInvoices = await PurchaseInvoice_Repo.GetAllListAsync(x => 
                x.TenantId == tenantId && !x.IsDeleted &&
                (!startDate.HasValue || x.IssueDate >= startDate.Value.Date) &&
                (!endDate.HasValue || x.IssueDate <= endDate.Value.Date));
            
            var purchaseOrders = await PurchaseOrder_Repo.GetAllListAsync(x => 
                x.TenantId == tenantId && !x.IsDeleted &&
                (!startDate.HasValue || x.IssueDate >= startDate.Value.Date) &&
                (!endDate.HasValue || x.IssueDate <= endDate.Value.Date));

            var salesInvoices = await SaleInvoice_Repo.GetAllListAsync(x => 
                x.TenantId == tenantId && !x.IsDeleted &&
                (!startDate.HasValue || x.IssueDate >= startDate.Value.Date) &&
                (!endDate.HasValue || x.IssueDate <= endDate.Value.Date));
            
            var salesOrders = await SalesOrder_Repo.GetAllListAsync(x => 
                x.TenantId == tenantId && !x.IsDeleted &&
                (!startDate.HasValue || x.IssueDate >= startDate.Value.Date) &&
                (!endDate.HasValue || x.IssueDate <= endDate.Value.Date));

            return new PurchaseSalesStatsDto
            {
                PurchaseStats = new PurchaseStatsDto
                {
                    TotalPurchaseInvoices = purchaseInvoices.Count,
                    TotalPurchaseOrders = purchaseOrders.Count,
                    TotalPurchaseInvoiceAmount = purchaseInvoices.Sum(x => x.GrandTotal),
                    TotalPurchaseOrderAmount = purchaseOrders.Sum(x => x.NetTotal)
                },
                SalesStats = new SalesStatsDto
                {
                    TotalSalesInvoices = salesInvoices.Count,
                    TotalSalesOrders = salesOrders.Count,
                    TotalSalesInvoiceAmount = salesInvoices.Sum(x => x.GrandTotal),
                    TotalSalesOrderAmount = salesOrders.Sum(x => x.TotalAmount)
                }
            };
        }
    }

   
   
    public class BusinessPartnersDto
    {
        public int TotalSuppliers { get; set; }
        public int TotalClients { get; set; }
        public int TotalBanks { get; set; }
        public int TotalBrokers { get; set; }
    
    }

    public class BusinessPartnerDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }

    public class PurchaseSalesStatsDto
    {
        public PurchaseStatsDto PurchaseStats { get; set; }
        public SalesStatsDto SalesStats { get; set; }
    }

    public class PurchaseStatsDto
    {
        public int TotalPurchaseInvoices { get; set; }
        public int TotalPurchaseOrders { get; set; }
        public decimal TotalPurchaseInvoiceAmount { get; set; }
        public decimal TotalPurchaseOrderAmount { get; set; }
    }

    public class SalesStatsDto
    {
        public int TotalSalesInvoices { get; set; }
        public int TotalSalesOrders { get; set; }
        public decimal TotalSalesInvoiceAmount { get; set; }
        public decimal TotalSalesOrderAmount { get; set; }
    }
}


