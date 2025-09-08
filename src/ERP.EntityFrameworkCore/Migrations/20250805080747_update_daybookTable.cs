using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Migrations
{
    /// <inheritdoc />
    public partial class update_daybookTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "DayBookInfo");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "DayBookInfo");

            migrationBuilder.AddColumn<DateTime>(
                name: "IssueDate",
                table: "DayBookInfo",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "DayBookDetailsInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DayBookInfoId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DayBookDetailsInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DayBookDetailsInfo_DayBookInfo_DayBookInfoId",
                        column: x => x.DayBookInfoId,
                        principalTable: "DayBookInfo",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DayBookDetailsInfo_DayBookInfoId",
                table: "DayBookDetailsInfo",
                column: "DayBookInfoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DayBookDetailsInfo");

            migrationBuilder.DropColumn(
                name: "IssueDate",
                table: "DayBookInfo");

            migrationBuilder.AddColumn<int>(
                name: "Amount",
                table: "DayBookInfo",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "DayBookInfo",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
