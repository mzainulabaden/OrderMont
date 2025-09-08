using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Migrations
{
    /// <inheritdoc />
    public partial class Added_Totals_In_SalesManagement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "TotalAmount",
                table: "SALES_SalesReturnInfo",
                type: "decimal(19,2)",
                precision: 19,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalAmount",
                table: "SALES_SalesOrderInfo",
                type: "decimal(16,2)",
                precision: 16,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "DiscountAmount",
                table: "SALES_SalesInvoiceInfo",
                type: "decimal(16,2)",
                precision: 16,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "DiscountPercentage",
                table: "SALES_SalesInvoiceInfo",
                type: "decimal(16,2)",
                precision: 16,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "FreightAmount",
                table: "SALES_SalesInvoiceInfo",
                type: "decimal(16,2)",
                precision: 16,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "GrandTotal",
                table: "SALES_SalesInvoiceInfo",
                type: "decimal(16,2)",
                precision: 16,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "NetTotal",
                table: "SALES_SalesInvoiceInfo",
                type: "decimal(16,2)",
                precision: 16,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TaxAmount",
                table: "SALES_SalesInvoiceInfo",
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
                name: "TotalAmount",
                table: "SALES_SalesReturnInfo");

            migrationBuilder.DropColumn(
                name: "TotalAmount",
                table: "SALES_SalesOrderInfo");

            migrationBuilder.DropColumn(
                name: "DiscountAmount",
                table: "SALES_SalesInvoiceInfo");

            migrationBuilder.DropColumn(
                name: "DiscountPercentage",
                table: "SALES_SalesInvoiceInfo");

            migrationBuilder.DropColumn(
                name: "FreightAmount",
                table: "SALES_SalesInvoiceInfo");

            migrationBuilder.DropColumn(
                name: "GrandTotal",
                table: "SALES_SalesInvoiceInfo");

            migrationBuilder.DropColumn(
                name: "NetTotal",
                table: "SALES_SalesInvoiceInfo");

            migrationBuilder.DropColumn(
                name: "TaxAmount",
                table: "SALES_SalesInvoiceInfo");
        }
    }
}
