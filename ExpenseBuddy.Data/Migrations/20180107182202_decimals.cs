using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ExpenseBuddy.Web.Data.Migrations
{
    public partial class decimals : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "Payers",
                type: "decimal(18, 3)",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "Fee",
                table: "Expenses",
                type: "decimal(18, 3)",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "Expenses",
                type: "decimal(18, 3)",
                nullable: false,
                oldClrType: typeof(decimal));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "Payers",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 3)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Fee",
                table: "Expenses",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 3)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "Expenses",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 3)");
        }
    }
}
