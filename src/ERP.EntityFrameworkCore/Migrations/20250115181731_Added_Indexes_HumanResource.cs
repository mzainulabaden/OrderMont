using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Migrations
{
    /// <inheritdoc />
    public partial class Added_Indexes_HumanResource : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_HRM_AttendanceInfo_EmployeeId",
                table: "HRM_AttendanceInfo");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "HRM_EmployeeInfo",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_HRM_EmployeeInfo_Name_DesignationId",
                table: "HRM_EmployeeInfo",
                columns: new[] { "Name", "DesignationId" });

            migrationBuilder.CreateIndex(
                name: "IX_HRM_AttendanceInfo_EmployeeId_AttendanceDate",
                table: "HRM_AttendanceInfo",
                columns: new[] { "EmployeeId", "AttendanceDate" });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeSalaryDetailsInfo_EmployeeId",
                table: "EmployeeSalaryDetailsInfo",
                column: "EmployeeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_HRM_EmployeeInfo_Name_DesignationId",
                table: "HRM_EmployeeInfo");

            migrationBuilder.DropIndex(
                name: "IX_HRM_AttendanceInfo_EmployeeId_AttendanceDate",
                table: "HRM_AttendanceInfo");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeSalaryDetailsInfo_EmployeeId",
                table: "EmployeeSalaryDetailsInfo");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "HRM_EmployeeInfo",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_HRM_AttendanceInfo_EmployeeId",
                table: "HRM_AttendanceInfo",
                column: "EmployeeId");
        }
    }
}
