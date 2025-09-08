using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Migrations
{
    /// <inheritdoc />
    public partial class changeColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastPurchaseRate",
                table: "IMS_PurchaseOrderInfo");

            migrationBuilder.AddColumn<decimal>(
                name: "LastPurchaseRate",
                table: "IMS_PurchaseOrderDetailsInfo",
                type: "decimal(16,2)",
                precision: 16,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "LastPurchaseRate",
                table: "IMS_PurchaseInvoiceInfo",
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
                name: "LastPurchaseRate",
                table: "IMS_PurchaseOrderDetailsInfo");

            migrationBuilder.DropColumn(
                name: "LastPurchaseRate",
                table: "IMS_PurchaseInvoiceInfo");

            migrationBuilder.AddColumn<decimal>(
                name: "LastPurchaseRate",
                table: "IMS_PurchaseOrderInfo",
                type: "decimal(16,2)",
                precision: 16,
                scale: 2,
                nullable: false,
                defaultValue: 0m);
        }
    }
}
