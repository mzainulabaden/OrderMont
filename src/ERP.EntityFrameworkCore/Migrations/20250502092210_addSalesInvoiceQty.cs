using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Migrations
{
    /// <inheritdoc />
    public partial class addSalesInvoiceQty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "SalesInvoiceQty",
                table: "SALES_SalesReturnDetailsInfo",
                type: "decimal(16,2)",
                precision: 16,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PurchaseInvoiceQuantity",
                table: "IMS_PurchaseReturnDetailsInfo",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SalesInvoiceQty",
                table: "SALES_SalesReturnDetailsInfo");

            migrationBuilder.DropColumn(
                name: "PurchaseInvoiceQuantity",
                table: "IMS_PurchaseReturnDetailsInfo");
        }
    }
}
