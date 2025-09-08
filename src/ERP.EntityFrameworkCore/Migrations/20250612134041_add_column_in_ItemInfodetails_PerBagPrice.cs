using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Migrations
{
    /// <inheritdoc />
    public partial class add_column_in_ItemInfodetails_PerBagPrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "PerBagPrice",
                table: "IMS_ItemDetailsInfo",
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
                name: "PerBagPrice",
                table: "IMS_ItemDetailsInfo");
        }
    }
}
