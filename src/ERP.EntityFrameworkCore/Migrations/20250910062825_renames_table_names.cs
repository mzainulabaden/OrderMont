using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Migrations
{
    /// <inheritdoc />
    public partial class renames_table_names : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IMS_PurchaseInvoiceDetailsInfo_IMS_PurchaseInvoiceInfo_PurchaseInvoiceInfoId",
                table: "IMS_PurchaseInvoiceDetailsInfo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IMS_WarehouseStockLedgerInfo",
                table: "IMS_WarehouseStockLedgerInfo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IMS_VendorInfo",
                table: "IMS_VendorInfo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IMS_PurchaseInvoiceInfo",
                table: "IMS_PurchaseInvoiceInfo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IMS_LocationInfo",
                table: "IMS_LocationInfo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IMS_ItemInfo",
                table: "IMS_ItemInfo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IMS_ItemCategoryInfo",
                table: "IMS_ItemCategoryInfo");

            migrationBuilder.RenameTable(
                name: "IMS_WarehouseStockLedgerInfo",
                newName: "WarehouseStockLedgerInfo");

            migrationBuilder.RenameTable(
                name: "IMS_VendorInfo",
                newName: "VendorInfo");

            migrationBuilder.RenameTable(
                name: "IMS_PurchaseInvoiceInfo",
                newName: "PurchaseInvoiceInfo");

            migrationBuilder.RenameTable(
                name: "IMS_LocationInfo",
                newName: "LocationInfo");

            migrationBuilder.RenameTable(
                name: "IMS_ItemInfo",
                newName: "ItemInfo");

            migrationBuilder.RenameTable(
                name: "IMS_ItemCategoryInfo",
                newName: "ItemCategoryInfo");

            migrationBuilder.RenameIndex(
                name: "IX_IMS_WarehouseStockLedgerInfo_ItemId",
                table: "WarehouseStockLedgerInfo",
                newName: "IX_WarehouseStockLedgerInfo_ItemId");

            migrationBuilder.RenameIndex(
                name: "IX_IMS_PurchaseInvoiceInfo_VendorId",
                table: "PurchaseInvoiceInfo",
                newName: "IX_PurchaseInvoiceInfo_VendorId");

            migrationBuilder.RenameIndex(
                name: "IX_IMS_ItemInfo_CategoryId_VendorId",
                table: "ItemInfo",
                newName: "IX_ItemInfo_CategoryId_VendorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WarehouseStockLedgerInfo",
                table: "WarehouseStockLedgerInfo",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VendorInfo",
                table: "VendorInfo",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PurchaseInvoiceInfo",
                table: "PurchaseInvoiceInfo",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LocationInfo",
                table: "LocationInfo",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ItemInfo",
                table: "ItemInfo",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ItemCategoryInfo",
                table: "ItemCategoryInfo",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_IMS_PurchaseInvoiceDetailsInfo_PurchaseInvoiceInfo_PurchaseInvoiceInfoId",
                table: "IMS_PurchaseInvoiceDetailsInfo",
                column: "PurchaseInvoiceInfoId",
                principalTable: "PurchaseInvoiceInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IMS_PurchaseInvoiceDetailsInfo_PurchaseInvoiceInfo_PurchaseInvoiceInfoId",
                table: "IMS_PurchaseInvoiceDetailsInfo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WarehouseStockLedgerInfo",
                table: "WarehouseStockLedgerInfo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VendorInfo",
                table: "VendorInfo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PurchaseInvoiceInfo",
                table: "PurchaseInvoiceInfo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LocationInfo",
                table: "LocationInfo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ItemInfo",
                table: "ItemInfo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ItemCategoryInfo",
                table: "ItemCategoryInfo");

            migrationBuilder.RenameTable(
                name: "WarehouseStockLedgerInfo",
                newName: "IMS_WarehouseStockLedgerInfo");

            migrationBuilder.RenameTable(
                name: "VendorInfo",
                newName: "IMS_VendorInfo");

            migrationBuilder.RenameTable(
                name: "PurchaseInvoiceInfo",
                newName: "IMS_PurchaseInvoiceInfo");

            migrationBuilder.RenameTable(
                name: "LocationInfo",
                newName: "IMS_LocationInfo");

            migrationBuilder.RenameTable(
                name: "ItemInfo",
                newName: "IMS_ItemInfo");

            migrationBuilder.RenameTable(
                name: "ItemCategoryInfo",
                newName: "IMS_ItemCategoryInfo");

            migrationBuilder.RenameIndex(
                name: "IX_WarehouseStockLedgerInfo_ItemId",
                table: "IMS_WarehouseStockLedgerInfo",
                newName: "IX_IMS_WarehouseStockLedgerInfo_ItemId");

            migrationBuilder.RenameIndex(
                name: "IX_PurchaseInvoiceInfo_VendorId",
                table: "IMS_PurchaseInvoiceInfo",
                newName: "IX_IMS_PurchaseInvoiceInfo_VendorId");

            migrationBuilder.RenameIndex(
                name: "IX_ItemInfo_CategoryId_VendorId",
                table: "IMS_ItemInfo",
                newName: "IX_IMS_ItemInfo_CategoryId_VendorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IMS_WarehouseStockLedgerInfo",
                table: "IMS_WarehouseStockLedgerInfo",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IMS_VendorInfo",
                table: "IMS_VendorInfo",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IMS_PurchaseInvoiceInfo",
                table: "IMS_PurchaseInvoiceInfo",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IMS_LocationInfo",
                table: "IMS_LocationInfo",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IMS_ItemInfo",
                table: "IMS_ItemInfo",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IMS_ItemCategoryInfo",
                table: "IMS_ItemCategoryInfo",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_IMS_PurchaseInvoiceDetailsInfo_IMS_PurchaseInvoiceInfo_PurchaseInvoiceInfoId",
                table: "IMS_PurchaseInvoiceDetailsInfo",
                column: "PurchaseInvoiceInfoId",
                principalTable: "IMS_PurchaseInvoiceInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
