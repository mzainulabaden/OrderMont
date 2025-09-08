using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Migrations
{
    /// <inheritdoc />
    public partial class AddTableWareHouseStockAdjusment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IMS_WarehouseStockAdjustmentInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    IssueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VoucherNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IMS_WarehouseStockAdjustmentInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WarehouseStockAdjustmentDetailsInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InventoryItemId = table.Column<long>(type: "bigint", nullable: false),
                    ProjectId = table.Column<long>(type: "bigint", nullable: false),
                    CostCenterId = table.Column<long>(type: "bigint", nullable: false),
                    Credit = table.Column<decimal>(type: "decimal(19,2)", precision: 19, scale: 2, nullable: false),
                    Debit = table.Column<decimal>(type: "decimal(19,2)", precision: 19, scale: 2, nullable: false),
                    CostRate = table.Column<decimal>(type: "decimal(19,2)", precision: 19, scale: 2, nullable: false),
                    WarehouseStockAdjustmentInfoId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarehouseStockAdjustmentDetailsInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WarehouseStockAdjustmentDetailsInfo_IMS_WarehouseStockAdjustmentInfo_WarehouseStockAdjustmentInfoId",
                        column: x => x.WarehouseStockAdjustmentInfoId,
                        principalTable: "IMS_WarehouseStockAdjustmentInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseStockAdjustmentDetailsInfo_InventoryItemId_ProjectId_CostCenterId",
                table: "WarehouseStockAdjustmentDetailsInfo",
                columns: new[] { "InventoryItemId", "ProjectId", "CostCenterId" });

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseStockAdjustmentDetailsInfo_WarehouseStockAdjustmentInfoId",
                table: "WarehouseStockAdjustmentDetailsInfo",
                column: "WarehouseStockAdjustmentInfoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WarehouseStockAdjustmentDetailsInfo");

            migrationBuilder.DropTable(
                name: "IMS_WarehouseStockAdjustmentInfo");
        }
    }
}
