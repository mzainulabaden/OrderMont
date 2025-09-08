using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Migrations
{
    /// <inheritdoc />
    public partial class changesinwarehousetable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_WarehouseStockAdjustmentDetailsInfo_InventoryItemId_ProjectId_CostCenterId",
                table: "WarehouseStockAdjustmentDetailsInfo");

            migrationBuilder.DropColumn(
                name: "CostCenterId",
                table: "WarehouseStockAdjustmentDetailsInfo");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "WarehouseStockAdjustmentDetailsInfo");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseStockAdjustmentDetailsInfo_InventoryItemId",
                table: "WarehouseStockAdjustmentDetailsInfo",
                column: "InventoryItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_WarehouseStockAdjustmentDetailsInfo_InventoryItemId",
                table: "WarehouseStockAdjustmentDetailsInfo");

            migrationBuilder.AddColumn<long>(
                name: "CostCenterId",
                table: "WarehouseStockAdjustmentDetailsInfo",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ProjectId",
                table: "WarehouseStockAdjustmentDetailsInfo",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseStockAdjustmentDetailsInfo_InventoryItemId_ProjectId_CostCenterId",
                table: "WarehouseStockAdjustmentDetailsInfo",
                columns: new[] { "InventoryItemId", "ProjectId", "CostCenterId" });
        }
    }
}
