using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Migrations
{
    /// <inheritdoc />
    public partial class Changes_PurchaseInvoice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Freight",
                table: "IMS_PurchaseInvoiceInfo");

            migrationBuilder.DropColumn(
                name: "VEAmount",
                table: "IMS_PurchaseInvoiceInfo");

            migrationBuilder.DropColumn(
                name: "VIAmount",
                table: "IMS_PurchaseInvoiceInfo");

            migrationBuilder.RenameColumn(
                name: "QuantityPerKg",
                table: "IMS_PurchaseInvoiceDetailsInfo",
                newName: "Quantity");

            migrationBuilder.RenameColumn(
                name: "QuantityBag40Kg",
                table: "IMS_PurchaseInvoiceDetailsInfo",
                newName: "ActualQuantity");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "IMS_PurchaseInvoiceDetailsInfo",
                newName: "QuantityPerKg");

            migrationBuilder.RenameColumn(
                name: "ActualQuantity",
                table: "IMS_PurchaseInvoiceDetailsInfo",
                newName: "QuantityBag40Kg");

            migrationBuilder.AddColumn<decimal>(
                name: "Freight",
                table: "IMS_PurchaseInvoiceInfo",
                type: "decimal(16,2)",
                precision: 16,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "VEAmount",
                table: "IMS_PurchaseInvoiceInfo",
                type: "decimal(16,2)",
                precision: 16,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "VIAmount",
                table: "IMS_PurchaseInvoiceInfo",
                type: "decimal(16,2)",
                precision: 16,
                scale: 2,
                nullable: false,
                defaultValue: 0m);
        }
    }
}
