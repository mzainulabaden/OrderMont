using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Migrations
{
    /// <inheritdoc />
    public partial class Added_PurchaseOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IMS_PaymentModeInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IMS_PaymentModeInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IMS_PurchaseOrderInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SupplierCOALevel04Id = table.Column<long>(type: "bigint", nullable: false),
                    ReferenceNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentModeId = table.Column<long>(type: "bigint", nullable: false),
                    FreightAmount = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
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
                    table.PrimaryKey("PK_IMS_PurchaseOrderInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IMS_PurchaseOrderDetailsInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<long>(type: "bigint", nullable: false),
                    UnitId = table.Column<long>(type: "bigint", nullable: false),
                    PricePerKg = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    PricePerBag40Kg = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    QuantityPerKg = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    QuantityBag40Kg = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    GrandTotal = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    PurchaseOrderInfoId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IMS_PurchaseOrderDetailsInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IMS_PurchaseOrderDetailsInfo_IMS_PurchaseOrderInfo_PurchaseOrderInfoId",
                        column: x => x.PurchaseOrderInfoId,
                        principalTable: "IMS_PurchaseOrderInfo",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_IMS_PurchaseOrderDetailsInfo_ItemId",
                table: "IMS_PurchaseOrderDetailsInfo",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_IMS_PurchaseOrderDetailsInfo_PurchaseOrderInfoId",
                table: "IMS_PurchaseOrderDetailsInfo",
                column: "PurchaseOrderInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_IMS_PurchaseOrderInfo_SupplierCOALevel04Id",
                table: "IMS_PurchaseOrderInfo",
                column: "SupplierCOALevel04Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IMS_PaymentModeInfo");

            migrationBuilder.DropTable(
                name: "IMS_PurchaseOrderDetailsInfo");

            migrationBuilder.DropTable(
                name: "IMS_PurchaseOrderInfo");
        }
    }
}
