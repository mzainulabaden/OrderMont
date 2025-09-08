using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Migrations
{
    /// <inheritdoc />
    public partial class Changes_In_PO_PI : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_IMS_PurchaseOrderInfo_SupplierCOALevel04Id",
                table: "IMS_PurchaseOrderInfo");

            migrationBuilder.RenameColumn(
                name: "TotalAmount",
                table: "IMS_PurchaseOrderInfo",
                newName: "VIAmount");

            migrationBuilder.RenameColumn(
                name: "FreightAmount",
                table: "IMS_PurchaseOrderInfo",
                newName: "VEAmount");

            migrationBuilder.RenameColumn(
                name: "GrandTotal",
                table: "IMS_PurchaseInvoiceInfo",
                newName: "NetTotal");

            migrationBuilder.AddColumn<decimal>(
                name: "BrokerAmount",
                table: "IMS_PurchaseOrderInfo",
                type: "decimal(16,2)",
                precision: 16,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<long>(
                name: "BrokerCOALevel04Id",
                table: "IMS_PurchaseOrderInfo",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<decimal>(
                name: "BrokerPercentage",
                table: "IMS_PurchaseOrderInfo",
                type: "decimal(16,2)",
                precision: 16,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "DiscountAmount",
                table: "IMS_PurchaseOrderInfo",
                type: "decimal(16,2)",
                precision: 16,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "DiscountPercentage",
                table: "IMS_PurchaseOrderInfo",
                type: "decimal(16,2)",
                precision: 16,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Freight",
                table: "IMS_PurchaseOrderInfo",
                type: "decimal(16,2)",
                precision: 16,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "NetTotal",
                table: "IMS_PurchaseOrderInfo",
                type: "decimal(16,2)",
                precision: 16,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Tax",
                table: "IMS_PurchaseOrderInfo",
                type: "decimal(16,2)",
                precision: 16,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<long>(
                name: "TaxCOALevel04Id",
                table: "IMS_PurchaseOrderInfo",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<decimal>(
                name: "Total",
                table: "IMS_PurchaseOrderInfo",
                type: "decimal(16,2)",
                precision: 16,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<long>(
                name: "WarehouseId",
                table: "IMS_PurchaseOrderInfo",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_IMS_PurchaseOrderInfo_SupplierCOALevel04Id_TaxCOALevel04Id_BrokerCOALevel04Id",
                table: "IMS_PurchaseOrderInfo",
                columns: new[] { "SupplierCOALevel04Id", "TaxCOALevel04Id", "BrokerCOALevel04Id" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_IMS_PurchaseOrderInfo_SupplierCOALevel04Id_TaxCOALevel04Id_BrokerCOALevel04Id",
                table: "IMS_PurchaseOrderInfo");

            migrationBuilder.DropColumn(
                name: "BrokerAmount",
                table: "IMS_PurchaseOrderInfo");

            migrationBuilder.DropColumn(
                name: "BrokerCOALevel04Id",
                table: "IMS_PurchaseOrderInfo");

            migrationBuilder.DropColumn(
                name: "BrokerPercentage",
                table: "IMS_PurchaseOrderInfo");

            migrationBuilder.DropColumn(
                name: "DiscountAmount",
                table: "IMS_PurchaseOrderInfo");

            migrationBuilder.DropColumn(
                name: "DiscountPercentage",
                table: "IMS_PurchaseOrderInfo");

            migrationBuilder.DropColumn(
                name: "Freight",
                table: "IMS_PurchaseOrderInfo");

            migrationBuilder.DropColumn(
                name: "NetTotal",
                table: "IMS_PurchaseOrderInfo");

            migrationBuilder.DropColumn(
                name: "Tax",
                table: "IMS_PurchaseOrderInfo");

            migrationBuilder.DropColumn(
                name: "TaxCOALevel04Id",
                table: "IMS_PurchaseOrderInfo");

            migrationBuilder.DropColumn(
                name: "Total",
                table: "IMS_PurchaseOrderInfo");

            migrationBuilder.DropColumn(
                name: "WarehouseId",
                table: "IMS_PurchaseOrderInfo");

            migrationBuilder.RenameColumn(
                name: "VIAmount",
                table: "IMS_PurchaseOrderInfo",
                newName: "TotalAmount");

            migrationBuilder.RenameColumn(
                name: "VEAmount",
                table: "IMS_PurchaseOrderInfo",
                newName: "FreightAmount");

            migrationBuilder.RenameColumn(
                name: "NetTotal",
                table: "IMS_PurchaseInvoiceInfo",
                newName: "GrandTotal");

            migrationBuilder.CreateIndex(
                name: "IX_IMS_PurchaseOrderInfo_SupplierCOALevel04Id",
                table: "IMS_PurchaseOrderInfo",
                column: "SupplierCOALevel04Id");
        }
    }
}
