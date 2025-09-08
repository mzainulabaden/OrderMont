using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Migrations
{
    /// <inheritdoc />
    public partial class Added_PurchaseReturn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IMS_PurchaseReturnInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SupplierCOALevel04Id = table.Column<long>(type: "bigint", nullable: false),
                    ReferenceNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PurchaseInvoiceIds = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    IssueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VoucherNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IMS_PurchaseReturnInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IMS_PurchaseReturnDetailsInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<long>(type: "bigint", nullable: false),
                    UnitId = table.Column<long>(type: "bigint", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    QuantityReturned = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    LastPurchaseRate = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    GrandTotal = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    PurchaseInvoiceDetailId = table.Column<long>(type: "bigint", nullable: false),
                    PurchaseReturnInfoId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IMS_PurchaseReturnDetailsInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IMS_PurchaseReturnDetailsInfo_IMS_PurchaseReturnInfo_PurchaseReturnInfoId",
                        column: x => x.PurchaseReturnInfoId,
                        principalTable: "IMS_PurchaseReturnInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IMS_PurchaseReturnDetailsInfo_ItemId",
                table: "IMS_PurchaseReturnDetailsInfo",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_IMS_PurchaseReturnDetailsInfo_PurchaseReturnInfoId",
                table: "IMS_PurchaseReturnDetailsInfo",
                column: "PurchaseReturnInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_IMS_PurchaseReturnInfo_SupplierCOALevel04Id",
                table: "IMS_PurchaseReturnInfo",
                column: "SupplierCOALevel04Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IMS_PurchaseReturnDetailsInfo");

            migrationBuilder.DropTable(
                name: "IMS_PurchaseReturnInfo");
        }
    }
}
