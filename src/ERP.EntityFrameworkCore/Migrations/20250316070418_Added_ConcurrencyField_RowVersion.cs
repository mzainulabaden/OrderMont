using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Migrations
{
    /// <inheritdoc />
    public partial class Added_ConcurrencyField_RowVersion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "SALES_SalesReturnInfo",
                type: "rowversion",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "SALES_SalesOrderInfo",
                type: "rowversion",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "SALES_SalesInvoiceInfo",
                type: "rowversion",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "IMS_WarehouseStockLedgerInfo",
                type: "rowversion",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "IMS_WarehouseInfo",
                type: "rowversion",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "IMS_UnitInfo",
                type: "rowversion",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "IMS_PurchaseReturnInfo",
                type: "rowversion",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "IMS_PurchaseOrderInfo",
                type: "rowversion",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "IMS_PurchaseInvoiceInfo",
                type: "rowversion",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "IMS_ItemInfo",
                type: "rowversion",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "IMS_ItemCategoryInfo",
                type: "rowversion",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "HRM_GazettedHolidayInfo",
                type: "rowversion",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "HRM_EmployeeSalaryInfo",
                type: "rowversion",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "HRM_EmployeeInfo",
                type: "rowversion",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "HRM_DesignationInfo",
                type: "rowversion",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "HRM_AttendanceInfo",
                type: "rowversion",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "FINANCE_PaymentModeInfo",
                type: "rowversion",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "FINANCE_LinkWithInfo",
                type: "rowversion",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "FINANCE_GeneralLedgerInfo",
                type: "rowversion",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "FINANCE_CurrencyInfo",
                type: "rowversion",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "FINANCE_COALevel04Info",
                type: "rowversion",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "FINANCE_COALevel03Info",
                type: "rowversion",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "FINANCE_COALevel02Info",
                type: "rowversion",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "FINANCE_COALevel01Info",
                type: "rowversion",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "FINANCE_AccountTypeInfo",
                type: "rowversion",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "SALES_SalesReturnInfo");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "SALES_SalesOrderInfo");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "SALES_SalesInvoiceInfo");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "IMS_WarehouseStockLedgerInfo");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "IMS_WarehouseInfo");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "IMS_UnitInfo");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "IMS_PurchaseReturnInfo");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "IMS_PurchaseOrderInfo");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "IMS_PurchaseInvoiceInfo");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "IMS_ItemInfo");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "IMS_ItemCategoryInfo");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "HRM_GazettedHolidayInfo");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "HRM_EmployeeSalaryInfo");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "HRM_EmployeeInfo");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "HRM_DesignationInfo");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "HRM_AttendanceInfo");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "FINANCE_PaymentModeInfo");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "FINANCE_LinkWithInfo");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "FINANCE_GeneralLedgerInfo");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "FINANCE_CurrencyInfo");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "FINANCE_COALevel04Info");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "FINANCE_COALevel03Info");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "FINANCE_COALevel02Info");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "FINANCE_COALevel01Info");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "FINANCE_AccountTypeInfo");
        }
    }
}
