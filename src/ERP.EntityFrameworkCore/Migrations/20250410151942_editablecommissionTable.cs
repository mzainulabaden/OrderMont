using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Migrations
{
    /// <inheritdoc />
    public partial class editablecommissionTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CommissionPolicyInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PolicyType = table.Column<int>(type: "int", nullable: false),
                    CommisionAmount = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: true),
                    CommisionPercentage = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: true),
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
                    table.PrimaryKey("PK_CommissionPolicyInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CommissionPolicyDetailsInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FromAmount = table.Column<long>(type: "bigint", nullable: false),
                    ToAmount = table.Column<long>(type: "bigint", nullable: false),
                    CommisionAmount = table.Column<long>(type: "bigint", nullable: false),
                    CommissionPolicyInfoId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommissionPolicyDetailsInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommissionPolicyDetailsInfo_CommissionPolicyInfo_CommissionPolicyInfoId",
                        column: x => x.CommissionPolicyInfoId,
                        principalTable: "CommissionPolicyInfo",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommissionPolicyDetailsInfo_CommissionPolicyInfoId",
                table: "CommissionPolicyDetailsInfo",
                column: "CommissionPolicyInfoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommissionPolicyDetailsInfo");

            migrationBuilder.DropTable(
                name: "CommissionPolicyInfo");
        }
    }
}
