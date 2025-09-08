using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Migrations
{
    /// <inheritdoc />
    public partial class Added_Indexes_Item : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_IMS_ItemInfo_SalesCOALevel04Id_PurchaseCOALevel04Id",
                table: "IMS_ItemInfo",
                columns: new[] { "SalesCOALevel04Id", "PurchaseCOALevel04Id" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_IMS_ItemInfo_SalesCOALevel04Id_PurchaseCOALevel04Id",
                table: "IMS_ItemInfo");
        }
    }
}
