using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Migrations
{
    /// <inheritdoc />
    public partial class Added_IsActive_Employee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_HRM_EmployeeInfo_Name_DesignationId",
                table: "HRM_EmployeeInfo");

            migrationBuilder.RenameColumn(
                name: "StartEventDate",
                table: "HRM_GauztedHolidayInfo",
                newName: "EventStartDate");

            migrationBuilder.RenameColumn(
                name: "EndEventoDate",
                table: "HRM_GauztedHolidayInfo",
                newName: "EventEndDate");

            migrationBuilder.AlterColumn<string>(
                name: "ErpId",
                table: "HRM_EmployeeInfo",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "HRM_EmployeeInfo",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_HRM_EmployeeInfo_Name_ErpId_DesignationId",
                table: "HRM_EmployeeInfo",
                columns: new[] { "Name", "ErpId", "DesignationId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_HRM_EmployeeInfo_Name_ErpId_DesignationId",
                table: "HRM_EmployeeInfo");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "HRM_EmployeeInfo");

            migrationBuilder.RenameColumn(
                name: "EventStartDate",
                table: "HRM_GauztedHolidayInfo",
                newName: "StartEventDate");

            migrationBuilder.RenameColumn(
                name: "EventEndDate",
                table: "HRM_GauztedHolidayInfo",
                newName: "EndEventoDate");

            migrationBuilder.AlterColumn<string>(
                name: "ErpId",
                table: "HRM_EmployeeInfo",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_HRM_EmployeeInfo_Name_DesignationId",
                table: "HRM_EmployeeInfo",
                columns: new[] { "Name", "DesignationId" });
        }
    }
}
