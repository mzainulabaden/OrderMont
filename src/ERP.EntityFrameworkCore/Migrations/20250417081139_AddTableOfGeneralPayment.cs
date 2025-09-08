using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Migrations
{
    /// <inheritdoc />
    public partial class AddTableOfGeneralPayment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FINANCE_GeneralPaymentInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BankCOALevel04Id = table.Column<long>(type: "bigint", nullable: false),
                    ReferenceNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferenceDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsCheque = table.Column<bool>(type: "bit", nullable: false),
                    IsCrossedCheque = table.Column<bool>(type: "bit", nullable: false),
                    ChequeTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChequeNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_FINANCE_GeneralPaymentInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GeneralPaymentDetailsInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    COALevel04Id = table.Column<long>(type: "bigint", nullable: false),
                    GrossAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TaxCOALevel04Id = table.Column<long>(type: "bigint", nullable: false),
                    TaxAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OtherTaxCOALevel04Id = table.Column<long>(type: "bigint", nullable: false),
                    OtherTaxAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NetAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    VoucherNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GeneralLedgerId = table.Column<long>(type: "bigint", nullable: false),
                    GeneralPaymentInfoId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneralPaymentDetailsInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GeneralPaymentDetailsInfo_FINANCE_GeneralPaymentInfo_GeneralPaymentInfoId",
                        column: x => x.GeneralPaymentInfoId,
                        principalTable: "FINANCE_GeneralPaymentInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GeneralPaymentDetailsInfo_GeneralPaymentInfoId",
                table: "GeneralPaymentDetailsInfo",
                column: "GeneralPaymentInfoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GeneralPaymentDetailsInfo");

            migrationBuilder.DropTable(
                name: "FINANCE_GeneralPaymentInfo");
        }
    }
}
