using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ExpenseBuddy.Web.Data.Migrations
{
    public partial class t : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExpensePayer_Expenses_ExpenseId",
                table: "ExpensePayer");

            migrationBuilder.DropForeignKey(
                name: "FK_ExpensePayer_AspNetUsers_PayerId",
                table: "ExpensePayer");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExpensePayer",
                table: "ExpensePayer");

            migrationBuilder.RenameTable(
                name: "ExpensePayer",
                newName: "Payers");

            migrationBuilder.RenameIndex(
                name: "IX_ExpensePayer_PayerId",
                table: "Payers",
                newName: "IX_Payers_PayerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Payers",
                table: "Payers",
                columns: new[] { "ExpenseId", "PayerId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Payers_Expenses_ExpenseId",
                table: "Payers",
                column: "ExpenseId",
                principalTable: "Expenses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Payers_AspNetUsers_PayerId",
                table: "Payers",
                column: "PayerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payers_Expenses_ExpenseId",
                table: "Payers");

            migrationBuilder.DropForeignKey(
                name: "FK_Payers_AspNetUsers_PayerId",
                table: "Payers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Payers",
                table: "Payers");

            migrationBuilder.RenameTable(
                name: "Payers",
                newName: "ExpensePayer");

            migrationBuilder.RenameIndex(
                name: "IX_Payers_PayerId",
                table: "ExpensePayer",
                newName: "IX_ExpensePayer_PayerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExpensePayer",
                table: "ExpensePayer",
                columns: new[] { "ExpenseId", "PayerId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ExpensePayer_Expenses_ExpenseId",
                table: "ExpensePayer",
                column: "ExpenseId",
                principalTable: "Expenses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExpensePayer_AspNetUsers_PayerId",
                table: "ExpensePayer",
                column: "PayerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
