using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace StockApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeparateUserBalance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CashBalance",
                table: "Users");

            migrationBuilder.CreateTable(
                name: "UserBalances",
                columns: table => new
                {
                    UserID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(18,4)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBalances", x => x.UserID);
                    table.ForeignKey(
                        name: "FK_UserBalances_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "UserBalances",
                columns: new[] { "UserID", "Balance" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000001"), 1000m },
                    { new Guid("00000000-0000-0000-0000-000000000002"), 1000m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserBalances");

            migrationBuilder.AddColumn<decimal>(
                name: "CashBalance",
                table: "Users",
                type: "decimal(18,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserID",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                column: "CashBalance",
                value: 1000m);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserID",
                keyValue: new Guid("00000000-0000-0000-0000-000000000002"),
                column: "CashBalance",
                value: 1000m);
        }
    }
}
