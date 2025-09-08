using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Migrations
{
    /// <inheritdoc />
    public partial class add_COAlevel04id_in_warehouse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "COALevel04Id",
                table: "IMS_WarehouseStockLedgerInfo",
                type: "bigint",
                precision: 16,
                scale: 2,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "COALevel04Id",
                table: "IMS_WarehouseStockLedgerInfo");
        }
    }
}
