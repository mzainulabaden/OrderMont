using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Migrations
{
    /// <inheritdoc />
    public partial class addgeneralnote : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FINANCE_GeneralNoteInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsCreditNature = table.Column<bool>(type: "bit", nullable: false),
                    NoteIndex = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountClass = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GeneralNoteType = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_FINANCE_GeneralNoteInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GeneralNoteDetailsInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    COALvl1Id = table.Column<long>(type: "bigint", nullable: false),
                    COALvl2Id = table.Column<long>(type: "bigint", nullable: false),
                    COALvl3Id = table.Column<long>(type: "bigint", nullable: false),
                    COALvl4Id = table.Column<long>(type: "bigint", nullable: false),
                    GeneralNoteVoucherPrefix = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GeneralNoteVoucherIndex = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StockVoucherPrefix = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StockVoucherIndex = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FixedAssetVoucherPrefix = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FixedAssetVoucherIndex = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransactionType = table.Column<int>(type: "int", nullable: false),
                    VoucherNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GeneralNoteInfoId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneralNoteDetailsInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GeneralNoteDetailsInfo_FINANCE_GeneralNoteInfo_GeneralNoteInfoId",
                        column: x => x.GeneralNoteInfoId,
                        principalTable: "FINANCE_GeneralNoteInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GeneralNoteDetailsInfo_COALvl1Id_COALvl2Id_COALvl3Id_COALvl4Id",
                table: "GeneralNoteDetailsInfo",
                columns: new[] { "COALvl1Id", "COALvl2Id", "COALvl3Id", "COALvl4Id" });

            migrationBuilder.CreateIndex(
                name: "IX_GeneralNoteDetailsInfo_GeneralNoteInfoId",
                table: "GeneralNoteDetailsInfo",
                column: "GeneralNoteInfoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GeneralNoteDetailsInfo");

            migrationBuilder.DropTable(
                name: "FINANCE_GeneralNoteInfo");
        }
    }
}
