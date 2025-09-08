using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Migrations
{
    /// <inheritdoc />
    public partial class add_column_PricePerBag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "PricePerBag",
                table: "IMS_PurchaseReturnDetailsInfo",
                type: "decimal(16,2)",
                precision: 16,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PricePerBag",
                table: "IMS_PurchaseOrderDetailsInfo",
                type: "decimal(16,2)",
                precision: 16,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PricePerBag",
                table: "IMS_PurchaseInvoiceDetailsInfo",
                type: "decimal(16,2)",
                precision: 16,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<DateTime>(
                name: "MaturityDate",
                table: "FINANCE_GeneralPaymentInfo",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<decimal>(
                name: "Debit",
                table: "FINANCE_GeneralLedgerInfo",
                type: "decimal(15,2)",
                precision: 15,
                scale: 2,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(15,2)",
                oldPrecision: 15,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "Credit",
                table: "FINANCE_GeneralLedgerInfo",
                type: "decimal(16,2)",
                precision: 16,
                scale: 2,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,2)",
                oldPrecision: 16,
                oldScale: 2);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PricePerBag",
                table: "IMS_PurchaseReturnDetailsInfo");

            migrationBuilder.DropColumn(
                name: "PricePerBag",
                table: "IMS_PurchaseOrderDetailsInfo");

            migrationBuilder.DropColumn(
                name: "PricePerBag",
                table: "IMS_PurchaseInvoiceDetailsInfo");

            migrationBuilder.AlterColumn<DateTime>(
                name: "MaturityDate",
                table: "FINANCE_GeneralPaymentInfo",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Debit",
                table: "FINANCE_GeneralLedgerInfo",
                type: "decimal(15,2)",
                precision: 15,
                scale: 2,
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(15,2)",
                oldPrecision: 15,
                oldScale: 2,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Credit",
                table: "FINANCE_GeneralLedgerInfo",
                type: "decimal(16,2)",
                precision: 16,
                scale: 2,
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,2)",
                oldPrecision: 16,
                oldScale: 2,
                oldNullable: true);
        }
    }
}
