using Abp.Zero.EntityFrameworkCore;
using ERP.Authorization.Roles;
using ERP.Authorization.Users;
using ERP.Modules.Finance.AccountGroups;
using ERP.Modules.Finance.ChartOfAccount.COALevel01;
using ERP.Modules.Finance.ChartOfAccount.COALevel02;
using ERP.Modules.Finance.ChartOfAccount.COALevel03;
using ERP.Modules.Finance.ChartOfAccount.COALevel04;
using ERP.Modules.Finance.GeneralLedger;
using ERP.Modules.Finance.GeneralNote;
using ERP.Modules.Finance.GeneralPayments;
using ERP.Modules.Finance.JournalVoucher;
using ERP.Modules.Finance.LookUps;
using ERP.Modules.Finance.ProfitLoseNote;
using ERP.Modules.HumanResource.AttendanceManagement;
using ERP.Modules.HumanResource.CommisionPolicy;
using ERP.Modules.HumanResource.CompanyProfile;
using ERP.Modules.HumanResource.EmployeeManagement;
using ERP.Modules.HumanResource.GazettedHoliday;
using ERP.Modules.HumanResource.LookUps;
using ERP.Modules.HumanResource.PayrollAdministration;
using ERP.Modules.HumanResource.Todo;
using ERP.Modules.InventoryManagement.DayBook;
using ERP.Modules.InventoryManagement.DemandBook;
using ERP.Modules.InventoryManagement.Item;
using ERP.Modules.InventoryManagement.LookUps;
using ERP.Modules.InventoryManagement.PurchaseInvoice;
using ERP.Modules.InventoryManagement.PurchaseOrder;
using ERP.Modules.InventoryManagement.PurchaseReturn;
using ERP.Modules.InventoryManagement.StockLedger;
using ERP.Modules.InventoryManagement.WarehouseStockAdjustment;
using ERP.Modules.SalesManagement.SalesInvoice;
using ERP.Modules.SalesManagement.SalesOrder;
using ERP.Modules.SalesManagement.SalesReturn;
using ERP.MultiTenancy;
using Microsoft.EntityFrameworkCore;

namespace ERP.EntityFrameworkCore;

public class ERPDbContext : AbpZeroDbContext<Tenant, Role, User, ERPDbContext>
{
    public ERPDbContext(DbContextOptions<ERPDbContext> options) : base(options)
    {

    }

    // Finance - related DbSet properties
    public DbSet<AccountTypeInfo> FINANCE_AccountTypeInfo { get; set; }
    public DbSet<COALevel01Info> FINANCE_COALevel01Info { get; set; }
    public DbSet<COALevel02Info> FINANCE_COALevel02Info { get; set; }
    public DbSet<COALevel03Info> FINANCE_COALevel03Info { get; set; }
    public DbSet<COALevel04Info> FINANCE_COALevel04Info { get  ; set; }
    public DbSet<CurrencyInfo> FINANCE_CurrencyInfo { get; set; }
    public DbSet<GeneralLedgerInfo> FINANCE_GeneralLedgerInfo { get; set; }
    public DbSet<LinkWithInfo> FINANCE_LinkWithInfo { get; set; }
    public DbSet<PaymentModeInfo> FINANCE_PaymentModeInfo { get; set; }
    public DbSet<GeneralPaymentInfo> FINANCE_GeneralPaymentInfo { get; set; }
    public DbSet<DefaultIntegrationsInfo> FINANCE_DefaultIntegrationsInfo { get; set; }
    public DbSet<JournalVoucherInfo> FINANCE_JournalVoucherInfo { get; set; }
    public DbSet<GeneralNoteInfo> FINANCE_GeneralNoteInfo { get; set; }
    public DbSet<AccountGroupsInfo> FINANCE_AccountGroupsInfo { get; set; }
    public DbSet<ProfitLoseNoteInfo> FINANCE_ProfitLoseNoteInfo { get; set; }

    // HumanResource - related DbSet properties
    public DbSet<AttendanceInfo> HRM_AttendanceInfo { get; set; }
    public DbSet<DesignationInfo> HRM_DesignationInfo { get; set; }
    public DbSet<EmployeeInfo> HRM_EmployeeInfo { get; set; }
    public DbSet<EmployeeSalaryInfo> HRM_EmployeeSalaryInfo { get; set; }
    public DbSet<GazettedHolidayInfo> HRM_GazettedHolidayInfo { get; set; }
    public DbSet<CommissionPolicyInfo> CommissionPolicyInfo { get; set; }
    public DbSet<CompanyProfileInfo> CompanyProfileInfo { get; set; }
    public DbSet<TodoInfo> HRM_TodoInfo{ get; set; }


    // InventoryManagement - related DbSet properties
    public DbSet<ItemInfo> IMS_ItemInfo { get; set; }
    public DbSet<ItemCategoryInfo> IMS_ItemCategoryInfo { get; set; }
    public DbSet<PurchaseInvoiceInfo> IMS_PurchaseInvoiceInfo { get; set; }
    public DbSet<PurchaseOrderInfo> IMS_PurchaseOrderInfo { get; set; }
    public DbSet<PurchaseReturnInfo> IMS_PurchaseReturnInfo { get; set; }
    public DbSet<UnitInfo> IMS_UnitInfo { get; set; }
    public DbSet<WarehouseInfo> IMS_WarehouseInfo { get; set; }
    public DbSet<WarehouseStockLedgerInfo> IMS_WarehouseStockLedgerInfo { get; set; }
    public DbSet<WarehouseStockAdjustmentInfo> IMS_WarehouseStockAdjustmentInfo { get; set; }
    public DbSet<DayBookInfo> DayBookInfo { get; set; }
    public DbSet<DemandBookInfo> DemandBookInfo { get; set; }



    // SalesManagement - related DbSet properties
    public DbSet<SalesInvoiceInfo> SALES_SalesInvoiceInfo { get; set; }
    public DbSet<SalesOrderInfo> SALES_SalesOrderInfo { get; set; }
    public DbSet<SalesReturnInfo> SALES_SalesReturnInfo { get; set; }
}


