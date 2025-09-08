using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Migrations
{
    /// <inheritdoc />
    public partial class Removed_AuditedProperties_ItemDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "IMS_ItemDetailsInfo");

            migrationBuilder.DropColumn(
                name: "CreatorUserId",
                table: "IMS_ItemDetailsInfo");

            migrationBuilder.DropColumn(
                name: "DeleterUserId",
                table: "IMS_ItemDetailsInfo");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                table: "IMS_ItemDetailsInfo");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "IMS_ItemDetailsInfo");

            migrationBuilder.DropColumn(
                name: "LastModificationTime",
                table: "IMS_ItemDetailsInfo");

            migrationBuilder.DropColumn(
                name: "LastModifierUserId",
                table: "IMS_ItemDetailsInfo");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "IMS_ItemDetailsInfo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "IMS_ItemDetailsInfo",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "CreatorUserId",
                table: "IMS_ItemDetailsInfo",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DeleterUserId",
                table: "IMS_ItemDetailsInfo",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionTime",
                table: "IMS_ItemDetailsInfo",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "IMS_ItemDetailsInfo",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModificationTime",
                table: "IMS_ItemDetailsInfo",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LastModifierUserId",
                table: "IMS_ItemDetailsInfo",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "IMS_ItemDetailsInfo",
                type: "int",
                nullable: true);
        }
    }
}
