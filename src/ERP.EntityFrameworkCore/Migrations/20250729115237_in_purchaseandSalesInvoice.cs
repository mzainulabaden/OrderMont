using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Migrations
{
    /// <inheritdoc />
    public partial class in_purchaseandSalesInvoice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "RemainingQty",
                table: "SALES_SalesInvoiceDetailsInfo",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "RemainingQty",
                table: "IMS_PurchaseInvoiceDetailsInfo",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RemainingQty",
                table: "SALES_SalesInvoiceDetailsInfo");

            migrationBuilder.DropColumn(
                name: "RemainingQty",
                table: "IMS_PurchaseInvoiceDetailsInfo");
        }
    }
}
