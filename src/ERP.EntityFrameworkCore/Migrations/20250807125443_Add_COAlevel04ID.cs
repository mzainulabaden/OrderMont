using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Migrations
{
    /// <inheritdoc />
    public partial class Add_COAlevel04ID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "DayBookDetailsInfo",
                newName: "COAName");

            migrationBuilder.AddColumn<long>(
                name: "COAlevel04Id",
                table: "DayBookDetailsInfo",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "COAlevel04Id",
                table: "DayBookDetailsInfo");

            migrationBuilder.RenameColumn(
                name: "COAName",
                table: "DayBookDetailsInfo",
                newName: "Name");
        }
    }
}
