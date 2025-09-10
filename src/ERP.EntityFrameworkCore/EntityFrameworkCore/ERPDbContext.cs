using Abp.Zero.EntityFrameworkCore;
using ERP.Authorization.Roles;
using ERP.Authorization.Users;
using ERP.Modules.Finance.ChartOfAccount.COALevel01;
using ERP.Modules.Finance.ChartOfAccount.COALevel02;
using ERP.Modules.Finance.ChartOfAccount.COALevel03;
using ERP.Modules.Finance.ChartOfAccount.COALevel04;
using ERP.Modules.Finance.GeneralLedger;
using ERP.Modules.Finance.LookUps;
using ERP.Modules.InventoryManagement.Item;
using ERP.Modules.InventoryManagement.LookUps;
using ERP.Modules.InventoryManagement.PurchaseInvoice;
using ERP.Modules.InventoryManagement.StockLedger;
using ERP.Modules.InventoryManagement.Vendor;
using ERP.MultiTenancy;
using Microsoft.EntityFrameworkCore;

namespace ERP.EntityFrameworkCore;

public class ERPDbContext : AbpZeroDbContext<Tenant, Role, User, ERPDbContext>
{
    public ERPDbContext(DbContextOptions<ERPDbContext> options) : base(options)
    {

    }

    //// Finance - related DbSet properties
    public DbSet<AccountTypeInfo> FINANCE_AccountTypeInfo { get; set; }
    public DbSet<COALevel01Info> FINANCE_COALevel01Info { get; set; }
    public DbSet<COALevel02Info> FINANCE_COALevel02Info { get; set; }
    public DbSet<COALevel03Info> FINANCE_COALevel03Info { get; set; }
    public DbSet<COALevel04Info> FINANCE_COALevel04Info { get; set; }
    //public DbSet<CurrencyInfo> FINANCE_CurrencyInfo { get; set; }
    public DbSet<GeneralLedgerInfo> FINANCE_GeneralLedgerInfo { get; set; }
    //public DbSet<LinkWithInfo> FINANCE_LinkWithInfo { get; set; }
    //public DbSet<PaymentModeInfo> FINANCE_PaymentModeInfo { get; set; }
    //public DbSet<GeneralPaymentInfo> FINANCE_GeneralPaymentInfo { get; set; }
    //public DbSet<DefaultIntegrationsInfo> FINANCE_DefaultIntegrationsInfo { get; set; }
    //public DbSet<JournalVoucherInfo> FINANCE_JournalVoucherInfo { get; set; }
    //public DbSet<GeneralNoteInfo> FINANCE_GeneralNoteInfo { get; set; }
    //public DbSet<AccountGroupsInfo> FINANCE_AccountGroupsInfo { get; set; }
    //public DbSet<ProfitLoseNoteInfo> FINANCE_ProfitLoseNoteInfo { get; set; }



    //// InventoryManagement - related DbSet properties
    //public DbSet<ItemInfo> ItemInfo { get; set; }
    public DbSet<ItemCategoryInfo> ItemCategoryInfo { get; set; }
    public DbSet<LocationInfo> LocationInfo { get; set; }
    public DbSet<VendorInfo> VendorInfo { get; set; }
    public DbSet<ItemInfo> ItemInfo { get; set; }
    public DbSet<PurchaseInvoiceInfo> PurchaseInvoiceInfo { get; set; }
    //public DbSet<PurchaseInvoiceInfo> PurchaseInvoiceInfo { get; set; }
    //public DbSet<PurchaseOrderInfo> PurchaseOrderInfo { get; set; }
    //public DbSet<PurchaseReturnInfo> PurchaseReturnInfo { get; set; }
    //public DbSet<UnitInfo> UnitInfo { get; set; }

    public DbSet<WarehouseStockLedgerInfo> WarehouseStockLedgerInfo { get; set; }
    //public DbSet<WarehouseStockAdjustmentInfo> WarehouseStockAdjustmentInfo { get; set; }


}


