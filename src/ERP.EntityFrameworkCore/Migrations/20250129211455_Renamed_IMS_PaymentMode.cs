using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Migrations
{
    /// <inheritdoc />
    public partial class Renamed_IMS_PaymentMode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_IMS_PaymentModeInfo",
                table: "IMS_PaymentModeInfo");

            migrationBuilder.RenameTable(
                name: "IMS_PaymentModeInfo",
                newName: "FINANCE_PaymentModeInfo");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FINANCE_PaymentModeInfo",
                table: "FINANCE_PaymentModeInfo",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_FINANCE_PaymentModeInfo",
                table: "FINANCE_PaymentModeInfo");

            migrationBuilder.RenameTable(
                name: "FINANCE_PaymentModeInfo",
                newName: "IMS_PaymentModeInfo");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IMS_PaymentModeInfo",
                table: "IMS_PaymentModeInfo",
                column: "Id");
        }
    }
}
