using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Migrations
{
    /// <inheritdoc />
    public partial class Add_ProfitLoseNoteTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FINANCE_ProfitLoseNoteInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NoteNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountType = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_FINANCE_ProfitLoseNoteInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProfitLoseNoteDetailsInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    COALevel03Id = table.Column<long>(type: "bigint", nullable: false),
                    COAlevel03Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfitLoseNoteInfoId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfitLoseNoteDetailsInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProfitLoseNoteDetailsInfo_FINANCE_ProfitLoseNoteInfo_ProfitLoseNoteInfoId",
                        column: x => x.ProfitLoseNoteInfoId,
                        principalTable: "FINANCE_ProfitLoseNoteInfo",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProfitLoseNoteDetailsInfo_ProfitLoseNoteInfoId",
                table: "ProfitLoseNoteDetailsInfo",
                column: "ProfitLoseNoteInfoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProfitLoseNoteDetailsInfo");

            migrationBuilder.DropTable(
                name: "FINANCE_ProfitLoseNoteInfo");
        }
    }
}
