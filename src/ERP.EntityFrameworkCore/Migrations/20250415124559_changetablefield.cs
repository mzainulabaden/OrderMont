using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Migrations
{
    /// <inheritdoc />
    public partial class changetablefield : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CommisionAmount",
                table: "CommissionPolicyDetailsInfo",
                newName: "SalesCommisionAmount");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SalesCommisionAmount",
                table: "CommissionPolicyDetailsInfo",
                newName: "CommisionAmount");
        }
    }
}
