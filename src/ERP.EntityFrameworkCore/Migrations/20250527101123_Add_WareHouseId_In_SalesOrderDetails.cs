using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Migrations
{
    /// <inheritdoc />
    public partial class Add_WareHouseId_In_SalesOrderDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WarehouseId",
                table: "SALES_SalesOrderInfo");

            migrationBuilder.AddColumn<decimal>(
                name: "WarehouseId",
                table: "SALES_SalesOrderDetailsInfo",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WarehouseId",
                table: "SALES_SalesOrderDetailsInfo");

            migrationBuilder.AddColumn<long>(
                name: "WarehouseId",
                table: "SALES_SalesOrderInfo",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
