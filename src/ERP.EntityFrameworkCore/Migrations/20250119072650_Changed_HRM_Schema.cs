using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Migrations
{
    /// <inheritdoc />
    public partial class Changed_HRM_Schema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HRM_EmployeeInfo_HRM_Designation_DesignationId",
                table: "HRM_EmployeeInfo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IMS_Unit",
                table: "IMS_Unit");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HRM_Designation",
                table: "HRM_Designation");

            migrationBuilder.RenameTable(
                name: "IMS_Unit",
                newName: "IMS_UnitInfo");

            migrationBuilder.RenameTable(
                name: "HRM_Designation",
                newName: "HRM_DesignationInfo");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IMS_UnitInfo",
                table: "IMS_UnitInfo",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HRM_DesignationInfo",
                table: "HRM_DesignationInfo",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeSalaryDetailsInfo_HRM_EmployeeInfo_EmployeeId",
                table: "EmployeeSalaryDetailsInfo",
                column: "EmployeeId",
                principalTable: "HRM_EmployeeInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HRM_EmployeeInfo_HRM_DesignationInfo_DesignationId",
                table: "HRM_EmployeeInfo",
                column: "DesignationId",
                principalTable: "HRM_DesignationInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeSalaryDetailsInfo_HRM_EmployeeInfo_EmployeeId",
                table: "EmployeeSalaryDetailsInfo");

            migrationBuilder.DropForeignKey(
                name: "FK_HRM_EmployeeInfo_HRM_DesignationInfo_DesignationId",
                table: "HRM_EmployeeInfo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IMS_UnitInfo",
                table: "IMS_UnitInfo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HRM_DesignationInfo",
                table: "HRM_DesignationInfo");

            migrationBuilder.RenameTable(
                name: "IMS_UnitInfo",
                newName: "IMS_Unit");

            migrationBuilder.RenameTable(
                name: "HRM_DesignationInfo",
                newName: "HRM_Designation");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IMS_Unit",
                table: "IMS_Unit",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HRM_Designation",
                table: "HRM_Designation",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_HRM_EmployeeInfo_HRM_Designation_DesignationId",
                table: "HRM_EmployeeInfo",
                column: "DesignationId",
                principalTable: "HRM_Designation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
