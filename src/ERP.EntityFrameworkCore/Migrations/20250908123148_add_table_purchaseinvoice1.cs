using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Migrations
{
    /// <inheritdoc />
    public partial class add_table_purchaseinvoice1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IMS_PurchaseInvoiceInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PONumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExternalRefNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VendorId = table.Column<long>(type: "bigint", nullable: false),
                    ExFactoryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PlacedOrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StatusId = table.Column<long>(type: "bigint", nullable: false),
                    Memo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DepositTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BilledTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UnBilledTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DepositAppliedTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
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
                    table.PrimaryKey("PK_IMS_PurchaseInvoiceInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IMS_PurchaseInvoiceDetailsInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<long>(type: "bigint", nullable: false),
                    Rate = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    ExFactoryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OrderQty = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RecievedQty = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OnShipment = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    PurchaseOrderDetailId = table.Column<long>(type: "bigint", nullable: false),
                    PurchaseInvoiceInfoId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IMS_PurchaseInvoiceDetailsInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IMS_PurchaseInvoiceDetailsInfo_IMS_PurchaseInvoiceInfo_PurchaseInvoiceInfoId",
                        column: x => x.PurchaseInvoiceInfoId,
                        principalTable: "IMS_PurchaseInvoiceInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IMS_PurchaseInvoiceDetailsInfo_ItemId",
                table: "IMS_PurchaseInvoiceDetailsInfo",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_IMS_PurchaseInvoiceDetailsInfo_PurchaseInvoiceInfoId",
                table: "IMS_PurchaseInvoiceDetailsInfo",
                column: "PurchaseInvoiceInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_IMS_PurchaseInvoiceInfo_VendorId",
                table: "IMS_PurchaseInvoiceInfo",
                column: "VendorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IMS_PurchaseInvoiceDetailsInfo");

            migrationBuilder.DropTable(
                name: "IMS_PurchaseInvoiceInfo");
        }
    }
}
