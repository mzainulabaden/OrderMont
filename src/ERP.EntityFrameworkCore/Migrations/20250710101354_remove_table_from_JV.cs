using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Migrations
{
    /// <inheritdoc />
    public partial class remove_table_from_JV : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_JournalVoucherDetailsInfo_EmployeeId",
                table: "JournalVoucherDetailsInfo");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "JournalVoucherDetailsInfo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "EmployeeId",
                table: "JournalVoucherDetailsInfo",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_JournalVoucherDetailsInfo_EmployeeId",
                table: "JournalVoucherDetailsInfo",
                column: "EmployeeId");
        }
    }
}
