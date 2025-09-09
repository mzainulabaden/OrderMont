using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Migrations
{
    /// <inheritdoc />
    public partial class add_initial_Migration_for_newDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommissionPolicyDetailsInfo");

            migrationBuilder.DropTable(
                name: "CompanyProfileInfo");

            migrationBuilder.DropTable(
                name: "DayBookDetailsInfo");

            migrationBuilder.DropTable(
                name: "DemandBookInfo");

            migrationBuilder.DropTable(
                name: "FINANCE_AccountGroupsInfo");

            migrationBuilder.DropTable(
                name: "FINANCE_AccountTypeInfo");

            migrationBuilder.DropTable(
                name: "FINANCE_COALevel01Info");

            migrationBuilder.DropTable(
                name: "FINANCE_COALevel02Info");

            migrationBuilder.DropTable(
                name: "FINANCE_COALevel03Info");

            migrationBuilder.DropTable(
                name: "FINANCE_COALevel04Info");

            migrationBuilder.DropTable(
                name: "FINANCE_CurrencyInfo");

            migrationBuilder.DropTable(
                name: "FINANCE_DefaultIntegrationsInfo");

            migrationBuilder.DropTable(
                name: "FINANCE_GeneralLedgerInfo");

            migrationBuilder.DropTable(
                name: "FINANCE_LinkWithInfo");

            migrationBuilder.DropTable(
                name: "FINANCE_PaymentModeInfo");

            migrationBuilder.DropTable(
                name: "GeneralNoteDetailsInfo");

            migrationBuilder.DropTable(
                name: "GeneralPaymentDetailsInfo");

            migrationBuilder.DropTable(
                name: "HRM_AttendanceInfo");

            migrationBuilder.DropTable(
                name: "HRM_DesignationInfo");

            migrationBuilder.DropTable(
                name: "HRM_EmployeeInfo");

            migrationBuilder.DropTable(
                name: "HRM_EmployeeSalaryDetailsInfo");

            migrationBuilder.DropTable(
                name: "HRM_GazettedHolidayInfo");

            migrationBuilder.DropTable(
                name: "HRM_TodoInfo");

            migrationBuilder.DropTable(
                name: "IMS_ItemCategoryInfo");

            migrationBuilder.DropTable(
                name: "IMS_ItemDetailsInfo");

            migrationBuilder.DropTable(
                name: "IMS_PurchaseInvoiceDetailsInfo");

            migrationBuilder.DropTable(
                name: "IMS_PurchaseOrderDetailsInfo");

            migrationBuilder.DropTable(
                name: "IMS_PurchaseReturnDetailsInfo");

            migrationBuilder.DropTable(
                name: "IMS_UnitInfo");

            migrationBuilder.DropTable(
                name: "IMS_WarehouseInfo");

            migrationBuilder.DropTable(
                name: "IMS_WarehouseStockLedgerInfo");

            migrationBuilder.DropTable(
                name: "JournalVoucherDetailsInfo");

            migrationBuilder.DropTable(
                name: "ProfitLoseNoteDetailsInfo");

            migrationBuilder.DropTable(
                name: "SALES_SalesInvoiceDetailsInfo");

            migrationBuilder.DropTable(
                name: "SALES_SalesOrderDetailsInfo");

            migrationBuilder.DropTable(
                name: "SALES_SalesReturnDetailsInfo");

            migrationBuilder.DropTable(
                name: "WarehouseStockAdjustmentDetailsInfo");

            migrationBuilder.DropTable(
                name: "CommissionPolicyInfo");

            migrationBuilder.DropTable(
                name: "DayBookInfo");

            migrationBuilder.DropTable(
                name: "FINANCE_GeneralNoteInfo");

            migrationBuilder.DropTable(
                name: "FINANCE_GeneralPaymentInfo");

            migrationBuilder.DropTable(
                name: "HRM_EmployeeSalaryInfo");

            migrationBuilder.DropTable(
                name: "IMS_ItemInfo");

            migrationBuilder.DropTable(
                name: "IMS_PurchaseInvoiceInfo");

            migrationBuilder.DropTable(
                name: "IMS_PurchaseOrderInfo");

            migrationBuilder.DropTable(
                name: "IMS_PurchaseReturnInfo");

            migrationBuilder.DropTable(
                name: "FINANCE_JournalVoucherInfo");

            migrationBuilder.DropTable(
                name: "FINANCE_ProfitLoseNoteInfo");

            migrationBuilder.DropTable(
                name: "SALES_SalesInvoiceInfo");

            migrationBuilder.DropTable(
                name: "SALES_SalesOrderInfo");

            migrationBuilder.DropTable(
                name: "SALES_SalesReturnInfo");

            migrationBuilder.DropTable(
                name: "IMS_WarehouseStockAdjustmentInfo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CommissionPolicyInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CommisionAmount = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: true),
                    CommisionPercentage = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PolicyType = table.Column<int>(type: "int", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommissionPolicyInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CompanyProfileInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    Logo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NTN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyProfileInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DayBookInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IssueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DayBookInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DemandBookInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ItemId = table.Column<long>(type: "bigint", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Qty = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    WarehouseId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DemandBookInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FINANCE_AccountGroupsInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountTypeIds = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FINANCE_AccountGroupsInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FINANCE_AccountTypeInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountGroupId = table.Column<int>(type: "int", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShortName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FINANCE_AccountTypeInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FINANCE_COALevel01Info",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountTypeId = table.Column<long>(type: "bigint", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SerialNumber = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FINANCE_COALevel01Info", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FINANCE_COALevel02Info",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountTypeId = table.Column<long>(type: "bigint", nullable: false),
                    COALevel01Id = table.Column<long>(type: "bigint", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SerialNumber = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FINANCE_COALevel02Info", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FINANCE_COALevel03Info",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountTypeId = table.Column<long>(type: "bigint", nullable: false),
                    COALevel02Id = table.Column<long>(type: "bigint", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SerialNumber = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FINANCE_COALevel03Info", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FINANCE_COALevel04Info",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountTypeId = table.Column<long>(type: "bigint", nullable: false),
                    CNIC = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    COALevel03Id = table.Column<long>(type: "bigint", nullable: false),
                    ContactNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    CurrencyId = table.Column<long>(type: "bigint", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EmailAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    LinkWithId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NationalTaxNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NatureOfAccount = table.Column<int>(type: "int", nullable: false),
                    OpeningBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PhysicalAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SalesTaxNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SerialNumber = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    StopEntryBefore = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FINANCE_COALevel04Info", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FINANCE_CurrencyInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Symbol = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FINANCE_CurrencyInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FINANCE_DefaultIntegrationsInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChartOfAccountId = table.Column<long>(type: "bigint", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FINANCE_DefaultIntegrationsInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FINANCE_GeneralLedgerInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChartOfAccountId = table.Column<long>(type: "bigint", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    Credit = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: true),
                    Debit = table.Column<decimal>(type: "decimal(15,2)", precision: 15, scale: 2, nullable: true),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    IsAdjustmentEntry = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IssueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    LinkedDocument = table.Column<int>(type: "int", nullable: false),
                    LinkedDocumentId = table.Column<long>(type: "bigint", nullable: false),
                    ReferenceDocument = table.Column<int>(type: "int", nullable: true),
                    ReferenceDocumentId = table.Column<long>(type: "bigint", nullable: false),
                    ReferenceVoucherNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    VoucherNumber = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FINANCE_GeneralLedgerInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FINANCE_GeneralNoteInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountClass = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GeneralNoteType = table.Column<int>(type: "int", nullable: true),
                    IsCreditNature = table.Column<bool>(type: "bit", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IssueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    NoteIndex = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VoucherNumber = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FINANCE_GeneralNoteInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FINANCE_GeneralPaymentInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BankCOALevel04Id = table.Column<long>(type: "bigint", nullable: false),
                    ChequeNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChequeTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsCheque = table.Column<bool>(type: "bit", nullable: false),
                    IsCrossedCheque = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IssueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    LinkedDocument = table.Column<int>(type: "int", nullable: false),
                    MaturityDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReferenceDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReferenceNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    VoucherNumber = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FINANCE_GeneralPaymentInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FINANCE_JournalVoucherInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IssueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    VoucherNumber = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FINANCE_JournalVoucherInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FINANCE_LinkWithInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FINANCE_LinkWithInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FINANCE_PaymentModeInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FINANCE_PaymentModeInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FINANCE_ProfitLoseNoteInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountType = table.Column<int>(type: "int", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NoteNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FINANCE_ProfitLoseNoteInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HRM_AttendanceInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AttendanceDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CheckIn_Time = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CheckOut_Time = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HRM_AttendanceInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HRM_DesignationInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HRM_DesignationInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HRM_EmployeeInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CNIC = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CommissionPolicyId = table.Column<long>(type: "bigint", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    DailyWageRate = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DesignationId = table.Column<long>(type: "bigint", nullable: false),
                    EmployeeType = table.Column<int>(type: "int", nullable: true),
                    ErpId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    FatherName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    JoiningDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    MonthlySalary = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RestDays = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HRM_EmployeeInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HRM_EmployeeSalaryInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EmployeeType = table.Column<int>(type: "int", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IssueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    VoucherNumber = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HRM_EmployeeSalaryInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HRM_GazettedHolidayInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EventEndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EventStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsRecurring = table.Column<bool>(type: "bit", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HRM_GazettedHolidayInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HRM_TodoInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HRM_TodoInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IMS_ItemCategoryInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IMS_ItemCategoryInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IMS_ItemInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DiscountAmount = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    DiscountPercentage = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsDiscountable = table.Column<bool>(type: "bit", nullable: false),
                    ItemCategoryId = table.Column<long>(type: "bigint", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PurchaseCOALevel04Id = table.Column<long>(type: "bigint", nullable: false),
                    ReOrderQty = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SalesCOALevel04Id = table.Column<long>(type: "bigint", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IMS_ItemInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IMS_PurchaseInvoiceInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdvanceAmount = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    AdvanceAmountBankCOALevl04Id = table.Column<long>(type: "bigint", nullable: true),
                    AttachedDocuments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BrokerAmount = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    BrokerCOALevel04Id = table.Column<long>(type: "bigint", nullable: false),
                    BrokerPercentage = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    BuiltyExpense = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    CommissionAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DiscountAmount = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    DiscountPercentage = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    GrandTotal = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsStockup = table.Column<bool>(type: "bit", nullable: false),
                    IssueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastPurchaseRate = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    LocalExpense = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    NetTotal = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    PaymentModeId = table.Column<long>(type: "bigint", nullable: false),
                    PurchaseOrderIds = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferenceNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SupplierCOALevel04Id = table.Column<long>(type: "bigint", nullable: false),
                    Tax = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    TaxCOALevel04Id = table.Column<long>(type: "bigint", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Total = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    VEAmount = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    VIAmount = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    VoucherNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WarehouseId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IMS_PurchaseInvoiceInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IMS_PurchaseOrderInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AttachedDocuments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BuiltyExpense = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IssueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    LocalExpense = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    NetTotal = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    PaymentModeId = table.Column<long>(type: "bigint", nullable: false),
                    ReferenceNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SupplierCOALevel04Id = table.Column<long>(type: "bigint", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Total = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    VoucherNumber = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IMS_PurchaseOrderInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IMS_PurchaseReturnInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IssueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    PurchaseInvoiceIds = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferenceNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SupplierCOALevel04Id = table.Column<long>(type: "bigint", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    VoucherNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WarehouseId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IMS_PurchaseReturnInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IMS_UnitInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConversionFactor = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IMS_UnitInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IMS_WarehouseInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    Manager = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    WarehouseCode = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IMS_WarehouseInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IMS_WarehouseStockAdjustmentInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IssueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    VoucherNumber = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IMS_WarehouseStockAdjustmentInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IMS_WarehouseStockLedgerInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActualQty = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    COALevel04Id = table.Column<long>(type: "bigint", precision: 16, scale: 2, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    Credit = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    Debit = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DocumentId = table.Column<long>(type: "bigint", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IssueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ItemId = table.Column<long>(type: "bigint", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    Rate = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    TotalAmount = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    VoucherNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WarehouseId = table.Column<long>(type: "bigint", nullable: false),
                    WarehouseStockLedgerLinkedDocument = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IMS_WarehouseStockLedgerInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SALES_SalesInvoiceInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdvanceAmount = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    AdvanceAmountBankCOALevl04Id = table.Column<long>(type: "bigint", nullable: true),
                    AttachedDocuments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CommissionAmount = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    CustomerCOALevel04Id = table.Column<long>(type: "bigint", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DiscountAmount = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    DiscountPercentage = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    EmployeeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FreightAmount = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    GrandTotal = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IssueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    NetTotal = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    PaymentModeId = table.Column<long>(type: "bigint", nullable: false),
                    ReferenceNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SalesOrderIds = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TaxAmount = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    TaxCOALevel04Id = table.Column<long>(type: "bigint", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    VoucherNumber = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SALES_SalesInvoiceInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SALES_SalesOrderInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CommissionAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    CustomerCOALevel04Id = table.Column<long>(type: "bigint", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IssueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    PaymentModeId = table.Column<long>(type: "bigint", nullable: false),
                    ReferenceNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    TotalAmount = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    VoucherNumber = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SALES_SalesOrderInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SALES_SalesReturnInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    CustomerCOALevel04Id = table.Column<long>(type: "bigint", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsReturnAgainstSalesInvoice = table.Column<bool>(type: "bit", nullable: false),
                    IssueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    ReferenceNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SalesInvoiceIds = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    TotalAmount = table.Column<decimal>(type: "decimal(19,2)", precision: 19, scale: 2, nullable: false),
                    VoucherNumber = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SALES_SalesReturnInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CommissionPolicyDetailsInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CommissionPolicyInfoId = table.Column<long>(type: "bigint", nullable: true),
                    FromAmount = table.Column<long>(type: "bigint", nullable: false),
                    SalesCommisionAmount = table.Column<long>(type: "bigint", nullable: false),
                    ToAmount = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommissionPolicyDetailsInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommissionPolicyDetailsInfo_CommissionPolicyInfo_CommissionPolicyInfoId",
                        column: x => x.CommissionPolicyInfoId,
                        principalTable: "CommissionPolicyInfo",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DayBookDetailsInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    COAName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    COAlevel04Id = table.Column<long>(type: "bigint", nullable: false),
                    DayBookInfoId = table.Column<long>(type: "bigint", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DayBookDetailsInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DayBookDetailsInfo_DayBookInfo_DayBookInfoId",
                        column: x => x.DayBookInfoId,
                        principalTable: "DayBookInfo",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "GeneralNoteDetailsInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    COALvl1Id = table.Column<long>(type: "bigint", nullable: true),
                    COALvl2Id = table.Column<long>(type: "bigint", nullable: true),
                    COALvl3Id = table.Column<long>(type: "bigint", nullable: true),
                    COALvl4Id = table.Column<long>(type: "bigint", nullable: true),
                    FixedAssetVoucherIndex = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FixedAssetVoucherPrefix = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GeneralNoteInfoId = table.Column<long>(type: "bigint", nullable: true),
                    GeneralNoteVoucherIndex = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GeneralNoteVoucherPrefix = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StockVoucherIndex = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StockVoucherPrefix = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransactionType = table.Column<int>(type: "int", nullable: true),
                    VoucherNumber = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneralNoteDetailsInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GeneralNoteDetailsInfo_FINANCE_GeneralNoteInfo_GeneralNoteInfoId",
                        column: x => x.GeneralNoteInfoId,
                        principalTable: "FINANCE_GeneralNoteInfo",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "GeneralPaymentDetailsInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    COALevel04Id = table.Column<long>(type: "bigint", nullable: false),
                    GeneralLedgerId = table.Column<long>(type: "bigint", nullable: true),
                    GeneralPaymentInfoId = table.Column<long>(type: "bigint", nullable: false),
                    GrossAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    InvoiceNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NetAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    OtherTaxAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    OtherTaxCOALevel04Id = table.Column<long>(type: "bigint", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TaxAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TaxCOALevel04Id = table.Column<long>(type: "bigint", nullable: true),
                    VoucherNumber = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneralPaymentDetailsInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GeneralPaymentDetailsInfo_FINANCE_GeneralPaymentInfo_GeneralPaymentInfoId",
                        column: x => x.GeneralPaymentInfoId,
                        principalTable: "FINANCE_GeneralPaymentInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JournalVoucherDetailsInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    COAlvl4Id = table.Column<long>(type: "bigint", nullable: false),
                    Credit = table.Column<decimal>(type: "decimal(19,2)", precision: 19, scale: 2, nullable: false),
                    Debit = table.Column<decimal>(type: "decimal(19,2)", precision: 19, scale: 2, nullable: false),
                    JournalVoucherInfoId = table.Column<long>(type: "bigint", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JournalVoucherDetailsInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JournalVoucherDetailsInfo_FINANCE_JournalVoucherInfo_JournalVoucherInfoId",
                        column: x => x.JournalVoucherInfoId,
                        principalTable: "FINANCE_JournalVoucherInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProfitLoseNoteDetailsInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    COALevel03Id = table.Column<long>(type: "bigint", nullable: false),
                    COAlevel03Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfitLoseNoteInfoId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfitLoseNoteDetailsInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProfitLoseNoteDetailsInfo_FINANCE_ProfitLoseNoteInfo_ProfitLoseNoteInfoId",
                        column: x => x.ProfitLoseNoteInfoId,
                        principalTable: "FINANCE_ProfitLoseNoteInfo",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "HRM_EmployeeSalaryDetailsInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AttendanceDays = table.Column<int>(type: "int", nullable: false),
                    DailyWageRate = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    EmployeeSalaryInfoId = table.Column<long>(type: "bigint", nullable: false),
                    GazettedHolidays = table.Column<int>(type: "int", nullable: false),
                    LeaveDays = table.Column<int>(type: "int", nullable: false),
                    NetPayable = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    PayableDays = table.Column<int>(type: "int", nullable: false),
                    RestDays = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HRM_EmployeeSalaryDetailsInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HRM_EmployeeSalaryDetailsInfo_HRM_EmployeeSalaryInfo_EmployeeSalaryInfoId",
                        column: x => x.EmployeeSalaryInfoId,
                        principalTable: "HRM_EmployeeSalaryInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IMS_ItemDetailsInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Barcode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ItemInfoId = table.Column<long>(type: "bigint", nullable: false),
                    MaxSalePrice = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    MinSalePrice = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    MinStockLevel = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    PerBagPrice = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    UnitId = table.Column<long>(type: "bigint", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IMS_ItemDetailsInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IMS_ItemDetailsInfo_IMS_ItemInfo_ItemInfoId",
                        column: x => x.ItemInfoId,
                        principalTable: "IMS_ItemInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IMS_PurchaseInvoiceDetailsInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActualQuantity = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    Adjustment = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    CostRate = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    GrandTotal = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    ItemId = table.Column<long>(type: "bigint", nullable: false),
                    LastPurchaseQty = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    LastPurchaseRate = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    PricePerBag = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    PricePerBag40Kg = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    PricePerKg = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    PurchaseInvoiceInfoId = table.Column<long>(type: "bigint", nullable: false),
                    PurchaseOrderDetailId = table.Column<long>(type: "bigint", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    RemainingQty = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalWeight = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    UnitId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IMS_PurchaseInvoiceDetailsInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IMS_PurchaseInvoiceDetailsInfo_IMS_PurchaseInvoiceInfo_PurchaseInvoiceInfoId",
                        column: x => x.PurchaseInvoiceInfoId,
                        principalTable: "IMS_PurchaseInvoiceInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IMS_PurchaseOrderDetailsInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActualQuantity = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    GrandTotal = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    ItemId = table.Column<long>(type: "bigint", nullable: false),
                    LastPurchaseRate = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    PricePerBag = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    PricePerBag40Kg = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    PricePerKg = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    PurchaseOrderInfoId = table.Column<long>(type: "bigint", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    UnitId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IMS_PurchaseOrderDetailsInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IMS_PurchaseOrderDetailsInfo_IMS_PurchaseOrderInfo_PurchaseOrderInfoId",
                        column: x => x.PurchaseOrderInfoId,
                        principalTable: "IMS_PurchaseOrderInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IMS_PurchaseReturnDetailsInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActualQuantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GrandTotal = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    ItemId = table.Column<long>(type: "bigint", nullable: false),
                    LastPurchaseRate = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    PricePerBag = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    PricePerKg = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    PurchaseInvoiceDetailId = table.Column<long>(type: "bigint", nullable: false),
                    PurchaseReturnInfoId = table.Column<long>(type: "bigint", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    QuantityReturned = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    UnitId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IMS_PurchaseReturnDetailsInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IMS_PurchaseReturnDetailsInfo_IMS_PurchaseReturnInfo_PurchaseReturnInfoId",
                        column: x => x.PurchaseReturnInfoId,
                        principalTable: "IMS_PurchaseReturnInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WarehouseStockAdjustmentDetailsInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CostRate = table.Column<decimal>(type: "decimal(19,2)", precision: 19, scale: 2, nullable: false),
                    Credit = table.Column<decimal>(type: "decimal(19,2)", precision: 19, scale: 2, nullable: false),
                    Debit = table.Column<decimal>(type: "decimal(19,2)", precision: 19, scale: 2, nullable: false),
                    InventoryItemId = table.Column<long>(type: "bigint", nullable: false),
                    MinStockLevel = table.Column<decimal>(type: "decimal(19,2)", precision: 19, scale: 2, nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnitId = table.Column<long>(type: "bigint", nullable: false),
                    WarehouseId = table.Column<long>(type: "bigint", nullable: false),
                    WarehouseStockAdjustmentInfoId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarehouseStockAdjustmentDetailsInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WarehouseStockAdjustmentDetailsInfo_IMS_WarehouseStockAdjustmentInfo_WarehouseStockAdjustmentInfoId",
                        column: x => x.WarehouseStockAdjustmentInfoId,
                        principalTable: "IMS_WarehouseStockAdjustmentInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SALES_SalesInvoiceDetailsInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GrandTotal = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    InvoiceQty = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    ItemId = table.Column<long>(type: "bigint", nullable: false),
                    ItemMaxRate = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    ItemMinRate = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    LastSaleRate = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    MinStockLevel = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    PricePerKg = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    ProfitAmount = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    ProfitPercentage = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    Rate = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    RemainingQty = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SalesInvoiceInfoId = table.Column<long>(type: "bigint", nullable: false),
                    SalesOrderDetailId = table.Column<long>(type: "bigint", nullable: false),
                    UnitId = table.Column<long>(type: "bigint", nullable: false),
                    WarehouseId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SALES_SalesInvoiceDetailsInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SALES_SalesInvoiceDetailsInfo_SALES_SalesInvoiceInfo_SalesInvoiceInfoId",
                        column: x => x.SalesInvoiceInfoId,
                        principalTable: "SALES_SalesInvoiceInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SALES_SalesOrderDetailsInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BagQty = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    GrandTotal = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    ItemId = table.Column<long>(type: "bigint", nullable: false),
                    ItemMaxRate = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    ItemMinRate = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    LastSaleRate = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    MinStockLevel = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    OrderedQty = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    PricePerKg = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    Rate = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    SalesOrderInfoId = table.Column<long>(type: "bigint", nullable: false),
                    UnitId = table.Column<long>(type: "bigint", nullable: false),
                    WarehouseId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SALES_SalesOrderDetailsInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SALES_SalesOrderDetailsInfo_SALES_SalesOrderInfo_SalesOrderInfoId",
                        column: x => x.SalesOrderInfoId,
                        principalTable: "SALES_SalesOrderInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SALES_SalesReturnDetailsInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GrandTotal = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    ItemId = table.Column<long>(type: "bigint", nullable: false),
                    LastSaleRate = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    PricePerKg = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    Rate = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    ReturnedQty = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    SalesInvoiceDetailId = table.Column<long>(type: "bigint", nullable: false),
                    SalesInvoiceQty = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    SalesReturnInfoId = table.Column<long>(type: "bigint", nullable: false),
                    UnitId = table.Column<long>(type: "bigint", nullable: false),
                    WarehouseId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SALES_SalesReturnDetailsInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SALES_SalesReturnDetailsInfo_SALES_SalesReturnInfo_SalesReturnInfoId",
                        column: x => x.SalesReturnInfoId,
                        principalTable: "SALES_SalesReturnInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommissionPolicyDetailsInfo_CommissionPolicyInfoId",
                table: "CommissionPolicyDetailsInfo",
                column: "CommissionPolicyInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_DayBookDetailsInfo_DayBookInfoId",
                table: "DayBookDetailsInfo",
                column: "DayBookInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_FINANCE_COALevel01Info_SerialNumber_AccountTypeId",
                table: "FINANCE_COALevel01Info",
                columns: new[] { "SerialNumber", "AccountTypeId" });

            migrationBuilder.CreateIndex(
                name: "IX_FINANCE_COALevel02Info_SerialNumber_COALevel01Id_AccountTypeId",
                table: "FINANCE_COALevel02Info",
                columns: new[] { "SerialNumber", "COALevel01Id", "AccountTypeId" });

            migrationBuilder.CreateIndex(
                name: "IX_FINANCE_COALevel03Info_SerialNumber_COALevel02Id_AccountTypeId",
                table: "FINANCE_COALevel03Info",
                columns: new[] { "SerialNumber", "COALevel02Id", "AccountTypeId" });

            migrationBuilder.CreateIndex(
                name: "IX_FINANCE_COALevel04Info_SerialNumber_COALevel03Id_AccountTypeId_CurrencyId_LinkWithId",
                table: "FINANCE_COALevel04Info",
                columns: new[] { "SerialNumber", "COALevel03Id", "AccountTypeId", "CurrencyId", "LinkWithId" });

            migrationBuilder.CreateIndex(
                name: "IX_FINANCE_DefaultIntegrationsInfo_ChartOfAccountId",
                table: "FINANCE_DefaultIntegrationsInfo",
                column: "ChartOfAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralNoteDetailsInfo_COALvl1Id_COALvl2Id_COALvl3Id_COALvl4Id",
                table: "GeneralNoteDetailsInfo",
                columns: new[] { "COALvl1Id", "COALvl2Id", "COALvl3Id", "COALvl4Id" });

            migrationBuilder.CreateIndex(
                name: "IX_GeneralNoteDetailsInfo_GeneralNoteInfoId",
                table: "GeneralNoteDetailsInfo",
                column: "GeneralNoteInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralPaymentDetailsInfo_GeneralPaymentInfoId",
                table: "GeneralPaymentDetailsInfo",
                column: "GeneralPaymentInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_HRM_AttendanceInfo_EmployeeId_AttendanceDate",
                table: "HRM_AttendanceInfo",
                columns: new[] { "EmployeeId", "AttendanceDate" });

            migrationBuilder.CreateIndex(
                name: "IX_HRM_EmployeeInfo_Name_ErpId_DesignationId",
                table: "HRM_EmployeeInfo",
                columns: new[] { "Name", "ErpId", "DesignationId" });

            migrationBuilder.CreateIndex(
                name: "IX_HRM_EmployeeSalaryDetailsInfo_EmployeeId",
                table: "HRM_EmployeeSalaryDetailsInfo",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_HRM_EmployeeSalaryDetailsInfo_EmployeeSalaryInfoId",
                table: "HRM_EmployeeSalaryDetailsInfo",
                column: "EmployeeSalaryInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_HRM_EmployeeSalaryInfo_StartDate_EndDate",
                table: "HRM_EmployeeSalaryInfo",
                columns: new[] { "StartDate", "EndDate" });

            migrationBuilder.CreateIndex(
                name: "IX_HRM_GazettedHolidayInfo_EventStartDate_EventEndDate",
                table: "HRM_GazettedHolidayInfo",
                columns: new[] { "EventStartDate", "EventEndDate" });

            migrationBuilder.CreateIndex(
                name: "IX_IMS_ItemDetailsInfo_ItemInfoId",
                table: "IMS_ItemDetailsInfo",
                column: "ItemInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_IMS_ItemInfo_SalesCOALevel04Id_PurchaseCOALevel04Id",
                table: "IMS_ItemInfo",
                columns: new[] { "SalesCOALevel04Id", "PurchaseCOALevel04Id" });

            migrationBuilder.CreateIndex(
                name: "IX_IMS_PurchaseInvoiceDetailsInfo_ItemId",
                table: "IMS_PurchaseInvoiceDetailsInfo",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_IMS_PurchaseInvoiceDetailsInfo_PurchaseInvoiceInfoId",
                table: "IMS_PurchaseInvoiceDetailsInfo",
                column: "PurchaseInvoiceInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_IMS_PurchaseInvoiceInfo_SupplierCOALevel04Id_TaxCOALevel04Id_BrokerCOALevel04Id",
                table: "IMS_PurchaseInvoiceInfo",
                columns: new[] { "SupplierCOALevel04Id", "TaxCOALevel04Id", "BrokerCOALevel04Id" });

            migrationBuilder.CreateIndex(
                name: "IX_IMS_PurchaseOrderDetailsInfo_ItemId",
                table: "IMS_PurchaseOrderDetailsInfo",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_IMS_PurchaseOrderDetailsInfo_PurchaseOrderInfoId",
                table: "IMS_PurchaseOrderDetailsInfo",
                column: "PurchaseOrderInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_IMS_PurchaseOrderInfo_SupplierCOALevel04Id",
                table: "IMS_PurchaseOrderInfo",
                column: "SupplierCOALevel04Id");

            migrationBuilder.CreateIndex(
                name: "IX_IMS_PurchaseReturnDetailsInfo_ItemId",
                table: "IMS_PurchaseReturnDetailsInfo",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_IMS_PurchaseReturnDetailsInfo_PurchaseReturnInfoId",
                table: "IMS_PurchaseReturnDetailsInfo",
                column: "PurchaseReturnInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_IMS_PurchaseReturnInfo_SupplierCOALevel04Id",
                table: "IMS_PurchaseReturnInfo",
                column: "SupplierCOALevel04Id");

            migrationBuilder.CreateIndex(
                name: "IX_IMS_WarehouseStockLedgerInfo_ItemId",
                table: "IMS_WarehouseStockLedgerInfo",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_JournalVoucherDetailsInfo_COAlvl4Id",
                table: "JournalVoucherDetailsInfo",
                column: "COAlvl4Id");

            migrationBuilder.CreateIndex(
                name: "IX_JournalVoucherDetailsInfo_JournalVoucherInfoId",
                table: "JournalVoucherDetailsInfo",
                column: "JournalVoucherInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfitLoseNoteDetailsInfo_ProfitLoseNoteInfoId",
                table: "ProfitLoseNoteDetailsInfo",
                column: "ProfitLoseNoteInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_SALES_SalesInvoiceDetailsInfo_ItemId",
                table: "SALES_SalesInvoiceDetailsInfo",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_SALES_SalesInvoiceDetailsInfo_SalesInvoiceInfoId",
                table: "SALES_SalesInvoiceDetailsInfo",
                column: "SalesInvoiceInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_SALES_SalesInvoiceInfo_CustomerCOALevel04Id_TaxCOALevel04Id",
                table: "SALES_SalesInvoiceInfo",
                columns: new[] { "CustomerCOALevel04Id", "TaxCOALevel04Id" });

            migrationBuilder.CreateIndex(
                name: "IX_SALES_SalesOrderDetailsInfo_ItemId",
                table: "SALES_SalesOrderDetailsInfo",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_SALES_SalesOrderDetailsInfo_SalesOrderInfoId",
                table: "SALES_SalesOrderDetailsInfo",
                column: "SalesOrderInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_SALES_SalesOrderInfo_CustomerCOALevel04Id",
                table: "SALES_SalesOrderInfo",
                column: "CustomerCOALevel04Id");

            migrationBuilder.CreateIndex(
                name: "IX_SALES_SalesReturnDetailsInfo_ItemId",
                table: "SALES_SalesReturnDetailsInfo",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_SALES_SalesReturnDetailsInfo_SalesReturnInfoId",
                table: "SALES_SalesReturnDetailsInfo",
                column: "SalesReturnInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_SALES_SalesReturnInfo_CustomerCOALevel04Id",
                table: "SALES_SalesReturnInfo",
                column: "CustomerCOALevel04Id");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseStockAdjustmentDetailsInfo_InventoryItemId",
                table: "WarehouseStockAdjustmentDetailsInfo",
                column: "InventoryItemId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseStockAdjustmentDetailsInfo_UnitId",
                table: "WarehouseStockAdjustmentDetailsInfo",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseStockAdjustmentDetailsInfo_WarehouseStockAdjustmentInfoId",
                table: "WarehouseStockAdjustmentDetailsInfo",
                column: "WarehouseStockAdjustmentInfoId");
        }
    }
}
