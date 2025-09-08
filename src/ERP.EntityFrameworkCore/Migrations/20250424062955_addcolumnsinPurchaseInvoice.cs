using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Migrations
{
    /// <inheritdoc />
    public partial class addcolumnsinPurchaseInvoice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "CommissionAmount",
                table: "IMS_PurchaseInvoiceInfo",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<long>(
                name: "EmployeeId",
                table: "IMS_PurchaseInvoiceInfo",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CommissionAmount",
                table: "IMS_PurchaseInvoiceInfo");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "IMS_PurchaseInvoiceInfo");
        }
    }
}
