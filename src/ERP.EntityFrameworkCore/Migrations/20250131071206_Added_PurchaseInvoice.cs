using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Migrations
{
    /// <inheritdoc />
    public partial class Added_PurchaseInvoice : Migration
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
                    SupplierCOALevel04Id = table.Column<long>(type: "bigint", nullable: false),
                    ReferenceNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentModeId = table.Column<long>(type: "bigint", nullable: false),
                    TaxCOALevel04Id = table.Column<long>(type: "bigint", nullable: false),
                    BrokerCOALevel04Id = table.Column<long>(type: "bigint", nullable: false),
                    Warehouse = table.Column<int>(type: "int", nullable: false),
                    DiscountPercentage = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    VEAmount = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    VIAmount = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    Freight = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    Tax = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    Total = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    BrokerPercentage = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    BrokerAmount = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    GrandTotal = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    AttachedDocuments = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    UnitId = table.Column<long>(type: "bigint", nullable: false),
                    PricePerKg = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    PricePerBag40Kg = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    QuantityPerKg = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    QuantityBag40Kg = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    LastPurchaseRate = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    CostRate = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    Adjustment = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    TotalWeight = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    GrandTotal = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
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
                name: "IX_IMS_PurchaseInvoiceInfo_SupplierCOALevel04Id_TaxCOALevel04Id_BrokerCOALevel04Id",
                table: "IMS_PurchaseInvoiceInfo",
                columns: new[] { "SupplierCOALevel04Id", "TaxCOALevel04Id", "BrokerCOALevel04Id" });
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
