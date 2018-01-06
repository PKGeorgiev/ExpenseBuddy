using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ExpenseBuddy.Web.Data.Migrations
{
    public partial class Expenses2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expense_ExpenseElements_ElementId",
                table: "Expense");

            migrationBuilder.DropForeignKey(
                name: "FK_Expense_AspNetUsers_OwnerId",
                table: "Expense");

            migrationBuilder.DropForeignKey(
                name: "FK_ExpensePayer_Expense_ExpenseId",
                table: "ExpensePayer");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Expense",
                table: "Expense");

            migrationBuilder.RenameTable(
                name: "Expense",
                newName: "Expenses");

            migrationBuilder.RenameIndex(
                name: "IX_Expense_OwnerId",
                table: "Expenses",
                newName: "IX_Expenses_OwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Expense_ElementId",
                table: "Expenses",
                newName: "IX_Expenses_ElementId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Expenses",
                table: "Expenses",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExpensePayer_Expenses_ExpenseId",
                table: "ExpensePayer",
                column: "ExpenseId",
                principalTable: "Expenses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_ExpenseElements_ElementId",
                table: "Expenses",
                column: "ElementId",
                principalTable: "ExpenseElements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_AspNetUsers_OwnerId",
                table: "Expenses",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExpensePayer_Expenses_ExpenseId",
                table: "ExpensePayer");

            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_ExpenseElements_ElementId",
                table: "Expenses");

            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_AspNetUsers_OwnerId",
                table: "Expenses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Expenses",
                table: "Expenses");

            migrationBuilder.RenameTable(
                name: "Expenses",
                newName: "Expense");

            migrationBuilder.RenameIndex(
                name: "IX_Expenses_OwnerId",
                table: "Expense",
                newName: "IX_Expense_OwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Expenses_ElementId",
                table: "Expense",
                newName: "IX_Expense_ElementId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Expense",
                table: "Expense",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Expense_ExpenseElements_ElementId",
                table: "Expense",
                column: "ElementId",
                principalTable: "ExpenseElements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Expense_AspNetUsers_OwnerId",
                table: "Expense",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExpensePayer_Expense_ExpenseId",
                table: "ExpensePayer",
                column: "ExpenseId",
                principalTable: "Expense",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
