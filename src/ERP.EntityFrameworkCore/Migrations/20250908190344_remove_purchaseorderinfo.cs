using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Migrations
{
    /// <inheritdoc />
    public partial class remove_purchaseorderinfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PurchaseOrderDetailId",
                table: "IMS_PurchaseInvoiceDetailsInfo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "PurchaseOrderDetailId",
                table: "IMS_PurchaseInvoiceDetailsInfo",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
