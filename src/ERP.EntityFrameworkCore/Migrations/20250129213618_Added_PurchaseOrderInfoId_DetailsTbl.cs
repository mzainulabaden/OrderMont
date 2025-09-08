using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Migrations
{
    /// <inheritdoc />
    public partial class Added_PurchaseOrderInfoId_DetailsTbl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IMS_PurchaseOrderDetailsInfo_IMS_PurchaseOrderInfo_PurchaseOrderInfoId",
                table: "IMS_PurchaseOrderDetailsInfo");

            migrationBuilder.AlterColumn<long>(
                name: "PurchaseOrderInfoId",
                table: "IMS_PurchaseOrderDetailsInfo",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_IMS_PurchaseOrderDetailsInfo_IMS_PurchaseOrderInfo_PurchaseOrderInfoId",
                table: "IMS_PurchaseOrderDetailsInfo",
                column: "PurchaseOrderInfoId",
                principalTable: "IMS_PurchaseOrderInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IMS_PurchaseOrderDetailsInfo_IMS_PurchaseOrderInfo_PurchaseOrderInfoId",
                table: "IMS_PurchaseOrderDetailsInfo");

            migrationBuilder.AlterColumn<long>(
                name: "PurchaseOrderInfoId",
                table: "IMS_PurchaseOrderDetailsInfo",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_IMS_PurchaseOrderDetailsInfo_IMS_PurchaseOrderInfo_PurchaseOrderInfoId",
                table: "IMS_PurchaseOrderDetailsInfo",
                column: "PurchaseOrderInfoId",
                principalTable: "IMS_PurchaseOrderInfo",
                principalColumn: "Id");
        }
    }
}
