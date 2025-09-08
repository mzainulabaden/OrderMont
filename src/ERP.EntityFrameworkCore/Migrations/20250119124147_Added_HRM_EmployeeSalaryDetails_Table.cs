using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Migrations
{
    /// <inheritdoc />
    public partial class Added_HRM_EmployeeSalaryDetails_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeSalaryDetailsInfo_HRM_EmployeeSalaryInfo_EmployeeSalaryInfoId",
                table: "EmployeeSalaryDetailsInfo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EmployeeSalaryDetailsInfo",
                table: "EmployeeSalaryDetailsInfo");

            migrationBuilder.RenameTable(
                name: "EmployeeSalaryDetailsInfo",
                newName: "HRM_EmployeeSalaryDetailsInfo");

            migrationBuilder.RenameIndex(
                name: "IX_EmployeeSalaryDetailsInfo_EmployeeSalaryInfoId",
                table: "HRM_EmployeeSalaryDetailsInfo",
                newName: "IX_HRM_EmployeeSalaryDetailsInfo_EmployeeSalaryInfoId");

            migrationBuilder.RenameIndex(
                name: "IX_EmployeeSalaryDetailsInfo_EmployeeId",
                table: "HRM_EmployeeSalaryDetailsInfo",
                newName: "IX_HRM_EmployeeSalaryDetailsInfo_EmployeeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HRM_EmployeeSalaryDetailsInfo",
                table: "HRM_EmployeeSalaryDetailsInfo",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_HRM_EmployeeSalaryDetailsInfo_HRM_EmployeeSalaryInfo_EmployeeSalaryInfoId",
                table: "HRM_EmployeeSalaryDetailsInfo",
                column: "EmployeeSalaryInfoId",
                principalTable: "HRM_EmployeeSalaryInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HRM_EmployeeSalaryDetailsInfo_HRM_EmployeeSalaryInfo_EmployeeSalaryInfoId",
                table: "HRM_EmployeeSalaryDetailsInfo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HRM_EmployeeSalaryDetailsInfo",
                table: "HRM_EmployeeSalaryDetailsInfo");

            migrationBuilder.RenameTable(
                name: "HRM_EmployeeSalaryDetailsInfo",
                newName: "EmployeeSalaryDetailsInfo");

            migrationBuilder.RenameIndex(
                name: "IX_HRM_EmployeeSalaryDetailsInfo_EmployeeSalaryInfoId",
                table: "EmployeeSalaryDetailsInfo",
                newName: "IX_EmployeeSalaryDetailsInfo_EmployeeSalaryInfoId");

            migrationBuilder.RenameIndex(
                name: "IX_HRM_EmployeeSalaryDetailsInfo_EmployeeId",
                table: "EmployeeSalaryDetailsInfo",
                newName: "IX_EmployeeSalaryDetailsInfo_EmployeeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmployeeSalaryDetailsInfo",
                table: "EmployeeSalaryDetailsInfo",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeSalaryDetailsInfo_HRM_EmployeeSalaryInfo_EmployeeSalaryInfoId",
                table: "EmployeeSalaryDetailsInfo",
                column: "EmployeeSalaryInfoId",
                principalTable: "HRM_EmployeeSalaryInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
