using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Migrations
{
    /// <inheritdoc />
    public partial class add_WareHouseId_in_salesInvoice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "SALES_SalesInvoiceInfo");

            migrationBuilder.AlterColumn<long>(
                name: "WarehouseId",
                table: "SALES_SalesOrderDetailsInfo",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<long>(
                name: "WarehouseId",
                table: "SALES_SalesInvoiceDetailsInfo",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WarehouseId",
                table: "SALES_SalesInvoiceDetailsInfo");

            migrationBuilder.AlterColumn<decimal>(
                name: "WarehouseId",
                table: "SALES_SalesOrderDetailsInfo",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<long>(
                name: "EmployeeId",
                table: "SALES_SalesInvoiceInfo",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
