using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Migrations
{
    /// <inheritdoc />
    public partial class HumanResource_Added_Indexes_SchemaChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "JoiningDate",
                table: "HRM_EmployeeInfo",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<int>(
                name: "EmployeeType",
                table: "HRM_EmployeeInfo",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_HRM_GazettedHolidayInfo_EventStartDate_EventEndDate",
                table: "HRM_GazettedHolidayInfo",
                columns: new[] { "EventStartDate", "EventEndDate" });

            migrationBuilder.CreateIndex(
                name: "IX_HRM_EmployeeSalaryInfo_StartDate_EndDate",
                table: "HRM_EmployeeSalaryInfo",
                columns: new[] { "StartDate", "EndDate" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_HRM_GazettedHolidayInfo_EventStartDate_EventEndDate",
                table: "HRM_GazettedHolidayInfo");

            migrationBuilder.DropIndex(
                name: "IX_HRM_EmployeeSalaryInfo_StartDate_EndDate",
                table: "HRM_EmployeeSalaryInfo");

            migrationBuilder.AlterColumn<DateTime>(
                name: "JoiningDate",
                table: "HRM_EmployeeInfo",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "EmployeeType",
                table: "HRM_EmployeeInfo",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
