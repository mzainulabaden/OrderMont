using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Migrations
{
    /// <inheritdoc />
    public partial class Added_Indexes_RenamedDetailsTbl_SalesReturn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SalesReturnDetailsInfo_SALES_SalesReturnInfo_SalesReturnInfoId",
                table: "SalesReturnDetailsInfo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SalesReturnDetailsInfo",
                table: "SalesReturnDetailsInfo");

            migrationBuilder.RenameTable(
                name: "SalesReturnDetailsInfo",
                newName: "SALES_SalesReturnDetailsInfo");

            migrationBuilder.RenameIndex(
                name: "IX_SalesReturnDetailsInfo_SalesReturnInfoId",
                table: "SALES_SalesReturnDetailsInfo",
                newName: "IX_SALES_SalesReturnDetailsInfo_SalesReturnInfoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SALES_SalesReturnDetailsInfo",
                table: "SALES_SalesReturnDetailsInfo",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_SALES_SalesReturnInfo_CustomerCOALevel04Id",
                table: "SALES_SalesReturnInfo",
                column: "CustomerCOALevel04Id");

            migrationBuilder.CreateIndex(
                name: "IX_SALES_SalesReturnDetailsInfo_ItemId",
                table: "SALES_SalesReturnDetailsInfo",
                column: "ItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_SALES_SalesReturnDetailsInfo_SALES_SalesReturnInfo_SalesReturnInfoId",
                table: "SALES_SalesReturnDetailsInfo",
                column: "SalesReturnInfoId",
                principalTable: "SALES_SalesReturnInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SALES_SalesReturnDetailsInfo_SALES_SalesReturnInfo_SalesReturnInfoId",
                table: "SALES_SalesReturnDetailsInfo");

            migrationBuilder.DropIndex(
                name: "IX_SALES_SalesReturnInfo_CustomerCOALevel04Id",
                table: "SALES_SalesReturnInfo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SALES_SalesReturnDetailsInfo",
                table: "SALES_SalesReturnDetailsInfo");

            migrationBuilder.DropIndex(
                name: "IX_SALES_SalesReturnDetailsInfo_ItemId",
                table: "SALES_SalesReturnDetailsInfo");

            migrationBuilder.RenameTable(
                name: "SALES_SalesReturnDetailsInfo",
                newName: "SalesReturnDetailsInfo");

            migrationBuilder.RenameIndex(
                name: "IX_SALES_SalesReturnDetailsInfo_SalesReturnInfoId",
                table: "SalesReturnDetailsInfo",
                newName: "IX_SalesReturnDetailsInfo_SalesReturnInfoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SalesReturnDetailsInfo",
                table: "SalesReturnDetailsInfo",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesReturnDetailsInfo_SALES_SalesReturnInfo_SalesReturnInfoId",
                table: "SalesReturnDetailsInfo",
                column: "SalesReturnInfoId",
                principalTable: "SALES_SalesReturnInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
