using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Migrations
{
    /// <inheritdoc />
    public partial class Added_Sales_Tables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SALES_SalesInvoiceInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReferenceNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentModeId = table.Column<long>(type: "bigint", nullable: false),
                    CustomerCOALevel04Id = table.Column<long>(type: "bigint", nullable: false),
                    TaxCOALevel04Id = table.Column<long>(type: "bigint", nullable: false),
                    SalesOrderIds = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_SALES_SalesInvoiceInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SALES_SalesOrderInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReferenceNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentModeId = table.Column<long>(type: "bigint", nullable: false),
                    CustomerCOALevel04Id = table.Column<long>(type: "bigint", nullable: false),
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
                    table.PrimaryKey("PK_SALES_SalesOrderInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SALES_SalesReturnInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReferenceNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerCOALevel04Id = table.Column<long>(type: "bigint", nullable: false),
                    IsReturnAgainstSalesInvoice = table.Column<bool>(type: "bit", nullable: false),
                    SalesInvoiceIds = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_SALES_SalesReturnInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SALES_SalesInvoiceDetailsInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<long>(type: "bigint", nullable: false),
                    UnitId = table.Column<long>(type: "bigint", nullable: false),
                    Rate = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    ProfitPercentage = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    ProfitAmount = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    InvoiceQty = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    LastSaleRate = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    GrandTotal = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    SalesOrderDetailId = table.Column<long>(type: "bigint", nullable: false),
                    SalesInvoiceInfoId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SALES_SalesInvoiceDetailsInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SALES_SalesInvoiceDetailsInfo_SALES_SalesInvoiceInfo_SalesInvoiceInfoId",
                        column: x => x.SalesInvoiceInfoId,
                        principalTable: "SALES_SalesInvoiceInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SALES_SalesOrderDetailsInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<long>(type: "bigint", nullable: false),
                    UnitId = table.Column<long>(type: "bigint", nullable: false),
                    Rate = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    OrderedQty = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    GrandTotal = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    SalesOrderInfoId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SALES_SalesOrderDetailsInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SALES_SalesOrderDetailsInfo_SALES_SalesOrderInfo_SalesOrderInfoId",
                        column: x => x.SalesOrderInfoId,
                        principalTable: "SALES_SalesOrderInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SalesReturnDetailsInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<long>(type: "bigint", nullable: false),
                    UnitId = table.Column<long>(type: "bigint", nullable: false),
                    Rate = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    ReturnedQty = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    LastSaleRate = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    GrandTotal = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    SalesInvoiceDetailId = table.Column<long>(type: "bigint", nullable: false),
                    SalesReturnInfoId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesReturnDetailsInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalesReturnDetailsInfo_SALES_SalesReturnInfo_SalesReturnInfoId",
                        column: x => x.SalesReturnInfoId,
                        principalTable: "SALES_SalesReturnInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SALES_SalesInvoiceDetailsInfo_ItemId",
                table: "SALES_SalesInvoiceDetailsInfo",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_SALES_SalesInvoiceDetailsInfo_SalesInvoiceInfoId",
                table: "SALES_SalesInvoiceDetailsInfo",
                column: "SalesInvoiceInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_SALES_SalesInvoiceInfo_CustomerCOALevel04Id_TaxCOALevel04Id",
                table: "SALES_SalesInvoiceInfo",
                columns: new[] { "CustomerCOALevel04Id", "TaxCOALevel04Id" });

            migrationBuilder.CreateIndex(
                name: "IX_SALES_SalesOrderDetailsInfo_ItemId",
                table: "SALES_SalesOrderDetailsInfo",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_SALES_SalesOrderDetailsInfo_SalesOrderInfoId",
                table: "SALES_SalesOrderDetailsInfo",
                column: "SalesOrderInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_SALES_SalesOrderInfo_CustomerCOALevel04Id",
                table: "SALES_SalesOrderInfo",
                column: "CustomerCOALevel04Id");

            migrationBuilder.CreateIndex(
                name: "IX_SalesReturnDetailsInfo_SalesReturnInfoId",
                table: "SalesReturnDetailsInfo",
                column: "SalesReturnInfoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SALES_SalesInvoiceDetailsInfo");

            migrationBuilder.DropTable(
                name: "SALES_SalesOrderDetailsInfo");

            migrationBuilder.DropTable(
                name: "SalesReturnDetailsInfo");

            migrationBuilder.DropTable(
                name: "SALES_SalesInvoiceInfo");

            migrationBuilder.DropTable(
                name: "SALES_SalesOrderInfo");

            migrationBuilder.DropTable(
                name: "SALES_SalesReturnInfo");
        }
    }
}
