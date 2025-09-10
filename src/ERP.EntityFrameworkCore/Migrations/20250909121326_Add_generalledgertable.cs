using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Migrations
{
    /// <inheritdoc />
    public partial class Add_generalledgertable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VoucherNumber",
                table: "IMS_PurchaseInvoiceDetailsInfo",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FINANCE_GeneralLedgerInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IssueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VoucherNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChartOfAccountId = table.Column<long>(type: "bigint", nullable: false),
                    Credit = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: true),
                    Debit = table.Column<decimal>(type: "decimal(15,2)", precision: 15, scale: 2, nullable: true),
                    IsAdjustmentEntry = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    LinkedDocumentId = table.Column<long>(type: "bigint", nullable: false),
                    LinkedDocument = table.Column<int>(type: "int", nullable: false),
                    ReferenceDocumentId = table.Column<long>(type: "bigint", nullable: false),
                    ReferenceVoucherNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferenceDocument = table.Column<int>(type: "int", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FINANCE_GeneralLedgerInfo", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FINANCE_GeneralLedgerInfo");

            migrationBuilder.DropColumn(
                name: "VoucherNumber",
                table: "IMS_PurchaseInvoiceDetailsInfo");
        }
    }
}
