using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Migrations
{
    /// <inheritdoc />
    public partial class Added_PO_Reference_PI : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PurchaseOrderIds",
                table: "IMS_PurchaseInvoiceInfo",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "PurchaseOrderDetailId",
                table: "IMS_PurchaseInvoiceDetailsInfo",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PurchaseOrderIds",
                table: "IMS_PurchaseInvoiceInfo");

            migrationBuilder.DropColumn(
                name: "PurchaseOrderDetailId",
                table: "IMS_PurchaseInvoiceDetailsInfo");
        }
    }
}
