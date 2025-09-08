using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Migrations
{
    /// <inheritdoc />
    public partial class remove_column_from_salesReturn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WarehouseId",
                table: "SALES_SalesReturnInfo");

            migrationBuilder.AddColumn<long>(
                name: "WarehouseId",
                table: "SALES_SalesReturnDetailsInfo",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WarehouseId",
                table: "SALES_SalesReturnDetailsInfo");

            migrationBuilder.AddColumn<long>(
                name: "WarehouseId",
                table: "SALES_SalesReturnInfo",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
