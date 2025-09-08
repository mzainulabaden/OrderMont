using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Migrations
{
    /// <inheritdoc />
    public partial class makenullable_generalNote : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GeneralNoteDetailsInfo_FINANCE_GeneralNoteInfo_GeneralNoteInfoId",
                table: "GeneralNoteDetailsInfo");

            migrationBuilder.AlterColumn<int>(
                name: "TransactionType",
                table: "GeneralNoteDetailsInfo",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<long>(
                name: "GeneralNoteInfoId",
                table: "GeneralNoteDetailsInfo",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "COALvl4Id",
                table: "GeneralNoteDetailsInfo",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "COALvl3Id",
                table: "GeneralNoteDetailsInfo",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "COALvl2Id",
                table: "GeneralNoteDetailsInfo",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "COALvl1Id",
                table: "GeneralNoteDetailsInfo",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<bool>(
                name: "IsCreditNature",
                table: "FINANCE_GeneralNoteInfo",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<int>(
                name: "GeneralNoteType",
                table: "FINANCE_GeneralNoteInfo",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_GeneralNoteDetailsInfo_FINANCE_GeneralNoteInfo_GeneralNoteInfoId",
                table: "GeneralNoteDetailsInfo",
                column: "GeneralNoteInfoId",
                principalTable: "FINANCE_GeneralNoteInfo",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GeneralNoteDetailsInfo_FINANCE_GeneralNoteInfo_GeneralNoteInfoId",
                table: "GeneralNoteDetailsInfo");

            migrationBuilder.AlterColumn<int>(
                name: "TransactionType",
                table: "GeneralNoteDetailsInfo",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "GeneralNoteInfoId",
                table: "GeneralNoteDetailsInfo",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "COALvl4Id",
                table: "GeneralNoteDetailsInfo",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "COALvl3Id",
                table: "GeneralNoteDetailsInfo",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "COALvl2Id",
                table: "GeneralNoteDetailsInfo",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "COALvl1Id",
                table: "GeneralNoteDetailsInfo",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsCreditNature",
                table: "FINANCE_GeneralNoteInfo",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "GeneralNoteType",
                table: "FINANCE_GeneralNoteInfo",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_GeneralNoteDetailsInfo_FINANCE_GeneralNoteInfo_GeneralNoteInfoId",
                table: "GeneralNoteDetailsInfo",
                column: "GeneralNoteInfoId",
                principalTable: "FINANCE_GeneralNoteInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
