using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Migrations
{
    /// <inheritdoc />
    public partial class Changes_In_PurchaseOrder_Item : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                name: "Tax",
                table: "IMS_PurchaseOrderInfo");

            migrationBuilder.DropColumn(
                name: "TaxCOALevel04Id",
                table: "IMS_PurchaseOrderInfo");

            migrationBuilder.DropColumn(
                name: "WarehouseId",
                table: "IMS_PurchaseOrderInfo");

            migrationBuilder.RenameColumn(
                name: "VIAmount",
                table: "IMS_PurchaseOrderInfo",
                newName: "LocalExpense");

            migrationBuilder.RenameColumn(
                name: "VEAmount",
                table: "IMS_PurchaseOrderInfo",
                newName: "BuiltyExpense");

            migrationBuilder.RenameColumn(
                name: "QuantityPerKg",
                table: "IMS_PurchaseOrderDetailsInfo",
                newName: "Quantity");

            migrationBuilder.RenameColumn(
                name: "QuantityBag40Kg",
                table: "IMS_PurchaseOrderDetailsInfo",
                newName: "ActualQuantity");

            migrationBuilder.AddColumn<long>(
                name: "PurchaseCOALevel04Id",
                table: "IMS_ItemInfo",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "SalesCOALevel04Id",
                table: "IMS_ItemInfo",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_IMS_PurchaseOrderInfo_SupplierCOALevel04Id",
                table: "IMS_PurchaseOrderInfo",
                column: "SupplierCOALevel04Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_IMS_PurchaseOrderInfo_SupplierCOALevel04Id",
                table: "IMS_PurchaseOrderInfo");

            migrationBuilder.DropColumn(
                name: "PurchaseCOALevel04Id",
                table: "IMS_ItemInfo");

            migrationBuilder.DropColumn(
                name: "SalesCOALevel04Id",
                table: "IMS_ItemInfo");

            migrationBuilder.RenameColumn(
                name: "LocalExpense",
                table: "IMS_PurchaseOrderInfo",
                newName: "VIAmount");

            migrationBuilder.RenameColumn(
                name: "BuiltyExpense",
                table: "IMS_PurchaseOrderInfo",
                newName: "VEAmount");

            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "IMS_PurchaseOrderDetailsInfo",
                newName: "QuantityPerKg");

            migrationBuilder.RenameColumn(
                name: "ActualQuantity",
                table: "IMS_PurchaseOrderDetailsInfo",
                newName: "QuantityBag40Kg");

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
    }
}
