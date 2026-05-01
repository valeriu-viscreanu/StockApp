using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace StockApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFinancialGoals : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GoalTypes",
                columns: table => new
                {
                    GoalTypeID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoalTypes", x => x.GoalTypeID);
                });

            migrationBuilder.CreateTable(
                name: "FinancialGoals",
                columns: table => new
                {
                    FinancialGoalID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GoalTypeID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TargetAmount = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    InitialAmount = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    MonthlyContribution = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    CurrentAmount = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TargetDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancialGoals", x => x.FinancialGoalID);
                    table.ForeignKey(
                        name: "FK_FinancialGoals_GoalTypes_GoalTypeID",
                        column: x => x.GoalTypeID,
                        principalTable: "GoalTypes",
                        principalColumn: "GoalTypeID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FinancialGoals_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "GoalTypes",
                columns: new[] { "GoalTypeID", "Name" },
                values: new object[,]
                {
                    { new Guid("30000000-0000-0000-0000-000000000001"), "Retirement" },
                    { new Guid("30000000-0000-0000-0000-000000000002"), "University" },
                    { new Guid("30000000-0000-0000-0000-000000000003"), "Emergency Fund" },
                    { new Guid("30000000-0000-0000-0000-000000000004"), "Vacation" },
                    { new Guid("30000000-0000-0000-0000-000000000005"), "Custom" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_FinancialGoals_GoalTypeID",
                table: "FinancialGoals",
                column: "GoalTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialGoals_UserID",
                table: "FinancialGoals",
                column: "UserID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FinancialGoals");

            migrationBuilder.DropTable(
                name: "GoalTypes");
        }
    }
}
