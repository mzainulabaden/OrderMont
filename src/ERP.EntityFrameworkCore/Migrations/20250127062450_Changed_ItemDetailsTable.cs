using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Migrations
{
    /// <inheritdoc />
    public partial class Changed_ItemDetailsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemDetailsInfo_IMS_ItemInfo_ItemInfoId",
                table: "ItemDetailsInfo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ItemDetailsInfo",
                table: "ItemDetailsInfo");

            migrationBuilder.RenameTable(
                name: "ItemDetailsInfo",
                newName: "IMS_ItemDetailsInfo");

            migrationBuilder.RenameIndex(
                name: "IX_ItemDetailsInfo_ItemInfoId",
                table: "IMS_ItemDetailsInfo",
                newName: "IX_IMS_ItemDetailsInfo_ItemInfoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IMS_ItemDetailsInfo",
                table: "IMS_ItemDetailsInfo",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_IMS_ItemDetailsInfo_IMS_ItemInfo_ItemInfoId",
                table: "IMS_ItemDetailsInfo",
                column: "ItemInfoId",
                principalTable: "IMS_ItemInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IMS_ItemDetailsInfo_IMS_ItemInfo_ItemInfoId",
                table: "IMS_ItemDetailsInfo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IMS_ItemDetailsInfo",
                table: "IMS_ItemDetailsInfo");

            migrationBuilder.RenameTable(
                name: "IMS_ItemDetailsInfo",
                newName: "ItemDetailsInfo");

            migrationBuilder.RenameIndex(
                name: "IX_IMS_ItemDetailsInfo_ItemInfoId",
                table: "ItemDetailsInfo",
                newName: "IX_ItemDetailsInfo_ItemInfoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ItemDetailsInfo",
                table: "ItemDetailsInfo",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemDetailsInfo_IMS_ItemInfo_ItemInfoId",
                table: "ItemDetailsInfo",
                column: "ItemInfoId",
                principalTable: "IMS_ItemInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
