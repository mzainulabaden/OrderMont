using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Migrations
{
    /// <inheritdoc />
    public partial class Added_HumanResource_Schema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DesignationInfo",
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
                    table.PrimaryKey("PK_DesignationInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HRM_EmployeeSalaryInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EmployeeType = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_HRM_EmployeeSalaryInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HRM_GauztedHolidayInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartEventDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndEventoDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsRecurring = table.Column<bool>(type: "bit", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_HRM_GauztedHolidayInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HRM_EmployeeInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CNIC = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeType = table.Column<int>(type: "int", nullable: false),
                    DesignationId = table.Column<long>(type: "bigint", nullable: false),
                    JoiningDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RestDays = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DailyWageRate = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    MonthlySalary = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
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
                    table.PrimaryKey("PK_HRM_EmployeeInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HRM_EmployeeInfo_DesignationInfo_DesignationId",
                        column: x => x.DesignationId,
                        principalTable: "DesignationInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeSalaryDetailsInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    AttendanceDays = table.Column<int>(type: "int", nullable: false),
                    DailyWageRate = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    NetPayable = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    EmployeeSalaryInfoId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeSalaryDetailsInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeSalaryDetailsInfo_HRM_EmployeeSalaryInfo_EmployeeSalaryInfoId",
                        column: x => x.EmployeeSalaryInfoId,
                        principalTable: "HRM_EmployeeSalaryInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HRM_AttendanceInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    CheckIn_Time = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CheckOut_Time = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AttendanceDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HRM_AttendanceInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HRM_AttendanceInfo_HRM_EmployeeInfo_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "HRM_EmployeeInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeSalaryDetailsInfo_EmployeeSalaryInfoId",
                table: "EmployeeSalaryDetailsInfo",
                column: "EmployeeSalaryInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_HRM_AttendanceInfo_EmployeeId",
                table: "HRM_AttendanceInfo",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_HRM_EmployeeInfo_DesignationId",
                table: "HRM_EmployeeInfo",
                column: "DesignationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeSalaryDetailsInfo");

            migrationBuilder.DropTable(
                name: "HRM_AttendanceInfo");

            migrationBuilder.DropTable(
                name: "HRM_GauztedHolidayInfo");

            migrationBuilder.DropTable(
                name: "HRM_EmployeeSalaryInfo");

            migrationBuilder.DropTable(
                name: "HRM_EmployeeInfo");

            migrationBuilder.DropTable(
                name: "DesignationInfo");
        }
    }
}
