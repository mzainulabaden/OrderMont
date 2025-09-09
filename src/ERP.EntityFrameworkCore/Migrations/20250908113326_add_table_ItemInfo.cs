using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Migrations
{
    /// <inheritdoc />
    public partial class add_table_ItemInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IMS_ItemInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SKU = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusId = table.Column<long>(type: "bigint", nullable: false),
                    CategoryId = table.Column<long>(type: "bigint", nullable: false),
                    VendorId = table.Column<long>(type: "bigint", nullable: false),
                    QtyPerCase = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LandedCost = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    ListPrice = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    HSCode = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ManufacturingLeadTime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastPurchaseCost = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    GTIN = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CartonLengthInches = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OceanLeadTime = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CartonHeightInches = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CartonWeigthLB = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CartonWidthInches = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UnitLengthInches = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UnitWidthInches = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UnitWeightLB = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UnitHeightInches = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ASIN = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Rate = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false),
                    ManufacturingTime = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_IMS_ItemInfo", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IMS_ItemInfo_CategoryId_VendorId",
                table: "IMS_ItemInfo",
                columns: new[] { "CategoryId", "VendorId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IMS_ItemInfo");
        }
    }
}
