using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication1.Migrations
{
    public partial class mig6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Account_Expenses_ExpensesId",
                table: "Account");

            migrationBuilder.DropForeignKey(
                name: "FK_Account_Incomes_IncomesId",
                table: "Account");

            migrationBuilder.DropIndex(
                name: "IX_Account_ExpensesId",
                table: "Account");

            migrationBuilder.DropIndex(
                name: "IX_Account_IncomesId",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "ExpensesId",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "IncomesId",
                table: "Account");

            migrationBuilder.AddColumn<int>(
                name: "AccountId",
                table: "Incomes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AccountId",
                table: "Expenses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "FuturePayment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GoalCost = table.Column<double>(type: "float", nullable: false),
                    PaymentCost = table.Column<double>(type: "float", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Frequency = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FuturePayment", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AccountFuturePayment",
                columns: table => new
                {
                    AccountsListId = table.Column<int>(type: "int", nullable: false),
                    FuturePaymentListId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountFuturePayment", x => new { x.AccountsListId, x.FuturePaymentListId });
                    table.ForeignKey(
                        name: "FK_AccountFuturePayment_Account_AccountsListId",
                        column: x => x.AccountsListId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccountFuturePayment_FuturePayment_FuturePaymentListId",
                        column: x => x.FuturePaymentListId,
                        principalTable: "FuturePayment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Incomes_AccountId",
                table: "Incomes",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_AccountId",
                table: "Expenses",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountFuturePayment_FuturePaymentListId",
                table: "AccountFuturePayment",
                column: "FuturePaymentListId");

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_Account_AccountId",
                table: "Expenses",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Incomes_Account_AccountId",
                table: "Incomes",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_Account_AccountId",
                table: "Expenses");

            migrationBuilder.DropForeignKey(
                name: "FK_Incomes_Account_AccountId",
                table: "Incomes");

            migrationBuilder.DropTable(
                name: "AccountFuturePayment");

            migrationBuilder.DropTable(
                name: "FuturePayment");

            migrationBuilder.DropIndex(
                name: "IX_Incomes_AccountId",
                table: "Incomes");

            migrationBuilder.DropIndex(
                name: "IX_Expenses_AccountId",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "Incomes");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "Expenses");

            migrationBuilder.AddColumn<int>(
                name: "ExpensesId",
                table: "Account",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IncomesId",
                table: "Account",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Account_ExpensesId",
                table: "Account",
                column: "ExpensesId");

            migrationBuilder.CreateIndex(
                name: "IX_Account_IncomesId",
                table: "Account",
                column: "IncomesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Account_Expenses_ExpensesId",
                table: "Account",
                column: "ExpensesId",
                principalTable: "Expenses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Account_Incomes_IncomesId",
                table: "Account",
                column: "IncomesId",
                principalTable: "Incomes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
