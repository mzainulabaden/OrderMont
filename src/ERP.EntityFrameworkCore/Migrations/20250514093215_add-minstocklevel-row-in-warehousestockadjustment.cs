using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Migrations
{
    /// <inheritdoc />
    public partial class addminstocklevelrowinwarehousestockadjustment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "MinStockLevel",
                table: "WarehouseStockAdjustmentDetailsInfo",
                type: "decimal(19,2)",
                precision: 19,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<long>(
                name: "UnitId",
                table: "WarehouseStockAdjustmentDetailsInfo",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MinStockLevel",
                table: "WarehouseStockAdjustmentDetailsInfo");

            migrationBuilder.DropColumn(
                name: "UnitId",
                table: "WarehouseStockAdjustmentDetailsInfo");
        }
    }
}
