using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Migrations
{
    /// <inheritdoc />
    public partial class changesinPriceandqty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PurchaseInvoiceQuantity",
                table: "IMS_PurchaseReturnDetailsInfo",
                newName: "Quantity");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "IMS_PurchaseReturnDetailsInfo",
                newName: "PricePerKg");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "IMS_PurchaseReturnDetailsInfo",
                newName: "PurchaseInvoiceQuantity");

            migrationBuilder.RenameColumn(
                name: "PricePerKg",
                table: "IMS_PurchaseReturnDetailsInfo",
                newName: "Price");
        }
    }
}
