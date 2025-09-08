using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Migrations
{
    /// <inheritdoc />
    public partial class addjournalvoucher : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FINANCE_JournalVoucherInfo",
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
                    IssueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VoucherNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FINANCE_JournalVoucherInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JournalVoucherDetailsInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    COAlvl4Id = table.Column<long>(type: "bigint", nullable: false),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    Credit = table.Column<decimal>(type: "decimal(19,2)", precision: 19, scale: 2, nullable: false),
                    Debit = table.Column<decimal>(type: "decimal(19,2)", precision: 19, scale: 2, nullable: false),
                    JournalVoucherInfoId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JournalVoucherDetailsInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JournalVoucherDetailsInfo_FINANCE_JournalVoucherInfo_JournalVoucherInfoId",
                        column: x => x.JournalVoucherInfoId,
                        principalTable: "FINANCE_JournalVoucherInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JournalVoucherDetailsInfo_COAlvl4Id",
                table: "JournalVoucherDetailsInfo",
                column: "COAlvl4Id");

            migrationBuilder.CreateIndex(
                name: "IX_JournalVoucherDetailsInfo_EmployeeId",
                table: "JournalVoucherDetailsInfo",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_JournalVoucherDetailsInfo_JournalVoucherInfoId",
                table: "JournalVoucherDetailsInfo",
                column: "JournalVoucherInfoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JournalVoucherDetailsInfo");

            migrationBuilder.DropTable(
                name: "FINANCE_JournalVoucherInfo");
        }
    }
}
