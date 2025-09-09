using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Migrations
{
    /// <inheritdoc />
    public partial class Add_lcoationnewtable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ItemCategoryInfo",
                table: "ItemCategoryInfo");

            migrationBuilder.RenameTable(
                name: "ItemCategoryInfo",
                newName: "IMS_ItemCategoryInfo");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IMS_ItemCategoryInfo",
                table: "IMS_ItemCategoryInfo",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "IMS_LocationInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_IMS_LocationInfo", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IMS_LocationInfo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IMS_ItemCategoryInfo",
                table: "IMS_ItemCategoryInfo");

            migrationBuilder.RenameTable(
                name: "IMS_ItemCategoryInfo",
                newName: "ItemCategoryInfo");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ItemCategoryInfo",
                table: "ItemCategoryInfo",
                column: "Id");
        }
    }
}
