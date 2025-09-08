using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Migrations
{
    /// <inheritdoc />
    public partial class Added_Min_Max_Rate_SalesOrder_SalesInvoice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ItemMaxRate",
                table: "SALES_SalesOrderDetailsInfo",
                type: "decimal(16,2)",
                precision: 16,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ItemMinRate",
                table: "SALES_SalesOrderDetailsInfo",
                type: "decimal(16,2)",
                precision: 16,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ItemMaxRate",
                table: "SALES_SalesInvoiceDetailsInfo",
                type: "decimal(16,2)",
                precision: 16,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ItemMinRate",
                table: "SALES_SalesInvoiceDetailsInfo",
                type: "decimal(16,2)",
                precision: 16,
                scale: 2,
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ItemMaxRate",
                table: "SALES_SalesOrderDetailsInfo");

            migrationBuilder.DropColumn(
                name: "ItemMinRate",
                table: "SALES_SalesOrderDetailsInfo");

            migrationBuilder.DropColumn(
                name: "ItemMaxRate",
                table: "SALES_SalesInvoiceDetailsInfo");

            migrationBuilder.DropColumn(
                name: "ItemMinRate",
                table: "SALES_SalesInvoiceDetailsInfo");
        }
    }
}
