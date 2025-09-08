using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Migrations
{
    /// <inheritdoc />
    public partial class Added_Unit_Designation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HRM_EmployeeInfo_DesignationInfo_DesignationId",
                table: "HRM_EmployeeInfo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DesignationInfo",
                table: "DesignationInfo");

            migrationBuilder.RenameTable(
                name: "DesignationInfo",
                newName: "HIERARCHY_Designation");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HIERARCHY_Designation",
                table: "HIERARCHY_Designation",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "HIERARCHY_Unit",
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
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HIERARCHY_Unit", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_HRM_EmployeeInfo_HIERARCHY_Designation_DesignationId",
                table: "HRM_EmployeeInfo",
                column: "DesignationId",
                principalTable: "HIERARCHY_Designation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HRM_EmployeeInfo_HIERARCHY_Designation_DesignationId",
                table: "HRM_EmployeeInfo");

            migrationBuilder.DropTable(
                name: "HIERARCHY_Unit");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HIERARCHY_Designation",
                table: "HIERARCHY_Designation");

            migrationBuilder.RenameTable(
                name: "HIERARCHY_Designation",
                newName: "DesignationInfo");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DesignationInfo",
                table: "DesignationInfo",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_HRM_EmployeeInfo_DesignationInfo_DesignationId",
                table: "HRM_EmployeeInfo",
                column: "DesignationId",
                principalTable: "DesignationInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
