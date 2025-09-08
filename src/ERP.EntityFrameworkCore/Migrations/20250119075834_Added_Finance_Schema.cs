using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Migrations
{
    /// <inheritdoc />
    public partial class Added_Finance_Schema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeSalaryDetailsInfo_HRM_EmployeeInfo_EmployeeId",
                table: "EmployeeSalaryDetailsInfo");

            migrationBuilder.DropForeignKey(
                name: "FK_HRM_AttendanceInfo_HRM_EmployeeInfo_EmployeeId",
                table: "HRM_AttendanceInfo");

            migrationBuilder.DropForeignKey(
                name: "FK_HRM_EmployeeInfo_HRM_DesignationInfo_DesignationId",
                table: "HRM_EmployeeInfo");

            migrationBuilder.DropTable(
                name: "HRM_GauztedHolidayInfo");

            migrationBuilder.DropIndex(
                name: "IX_HRM_EmployeeInfo_DesignationId",
                table: "HRM_EmployeeInfo");

            migrationBuilder.AddColumn<int>(
                name: "GazettedHolidays",
                table: "EmployeeSalaryDetailsInfo",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LeaveDays",
                table: "EmployeeSalaryDetailsInfo",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PayableDays",
                table: "EmployeeSalaryDetailsInfo",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "FINANCE_AccountTypeInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShortName = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_FINANCE_AccountTypeInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FINANCE_COALevel01Info",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SerialNumber = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    AccountTypeId = table.Column<long>(type: "bigint", nullable: false),
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
                    table.PrimaryKey("PK_FINANCE_COALevel01Info", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FINANCE_COALevel02Info",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SerialNumber = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    COALevel01Id = table.Column<long>(type: "bigint", nullable: false),
                    AccountTypeId = table.Column<long>(type: "bigint", nullable: false),
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
                    table.PrimaryKey("PK_FINANCE_COALevel02Info", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FINANCE_COALevel03Info",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SerialNumber = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    COALevel02Id = table.Column<long>(type: "bigint", nullable: false),
                    AccountTypeId = table.Column<long>(type: "bigint", nullable: false),
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
                    table.PrimaryKey("PK_FINANCE_COALevel03Info", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FINANCE_COALevel04Info",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SerialNumber = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    COALevel03Id = table.Column<long>(type: "bigint", nullable: false),
                    StopEntryBefore = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NatureOfAccount = table.Column<int>(type: "int", nullable: false),
                    AccountTypeId = table.Column<long>(type: "bigint", nullable: false),
                    CurrencyId = table.Column<long>(type: "bigint", nullable: false),
                    LinkWithId = table.Column<long>(type: "bigint", nullable: false),
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
                    table.PrimaryKey("PK_FINANCE_COALevel04Info", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FINANCE_CurrencyInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Symbol = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_FINANCE_CurrencyInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FINANCE_LinkWithInfo",
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
                    table.PrimaryKey("PK_FINANCE_LinkWithInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HRM_GazettedHolidayInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EventEndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
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
                    table.PrimaryKey("PK_HRM_GazettedHolidayInfo", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FINANCE_COALevel01Info_SerialNumber_AccountTypeId",
                table: "FINANCE_COALevel01Info",
                columns: new[] { "SerialNumber", "AccountTypeId" });

            migrationBuilder.CreateIndex(
                name: "IX_FINANCE_COALevel02Info_SerialNumber_COALevel01Id_AccountTypeId",
                table: "FINANCE_COALevel02Info",
                columns: new[] { "SerialNumber", "COALevel01Id", "AccountTypeId" });

            migrationBuilder.CreateIndex(
                name: "IX_FINANCE_COALevel03Info_SerialNumber_COALevel02Id_AccountTypeId",
                table: "FINANCE_COALevel03Info",
                columns: new[] { "SerialNumber", "COALevel02Id", "AccountTypeId" });

            migrationBuilder.CreateIndex(
                name: "IX_FINANCE_COALevel04Info_SerialNumber_COALevel03Id_AccountTypeId_CurrencyId_LinkWithId",
                table: "FINANCE_COALevel04Info",
                columns: new[] { "SerialNumber", "COALevel03Id", "AccountTypeId", "CurrencyId", "LinkWithId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FINANCE_AccountTypeInfo");

            migrationBuilder.DropTable(
                name: "FINANCE_COALevel01Info");

            migrationBuilder.DropTable(
                name: "FINANCE_COALevel02Info");

            migrationBuilder.DropTable(
                name: "FINANCE_COALevel03Info");

            migrationBuilder.DropTable(
                name: "FINANCE_COALevel04Info");

            migrationBuilder.DropTable(
                name: "FINANCE_CurrencyInfo");

            migrationBuilder.DropTable(
                name: "FINANCE_LinkWithInfo");

            migrationBuilder.DropTable(
                name: "HRM_GazettedHolidayInfo");

            migrationBuilder.DropColumn(
                name: "GazettedHolidays",
                table: "EmployeeSalaryDetailsInfo");

            migrationBuilder.DropColumn(
                name: "LeaveDays",
                table: "EmployeeSalaryDetailsInfo");

            migrationBuilder.DropColumn(
                name: "PayableDays",
                table: "EmployeeSalaryDetailsInfo");

            migrationBuilder.CreateTable(
                name: "HRM_GauztedHolidayInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EventEndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EventStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsRecurring = table.Column<bool>(type: "bit", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HRM_GauztedHolidayInfo", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HRM_EmployeeInfo_DesignationId",
                table: "HRM_EmployeeInfo",
                column: "DesignationId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeSalaryDetailsInfo_HRM_EmployeeInfo_EmployeeId",
                table: "EmployeeSalaryDetailsInfo",
                column: "EmployeeId",
                principalTable: "HRM_EmployeeInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HRM_AttendanceInfo_HRM_EmployeeInfo_EmployeeId",
                table: "HRM_AttendanceInfo",
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
    }
}
