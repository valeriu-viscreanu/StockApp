using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StockApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixShadowForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BuyOrders_Users_ApplicationUserUserID",
                table: "BuyOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_SellOrders_Users_ApplicationUserUserID",
                table: "SellOrders");

            migrationBuilder.DropIndex(
                name: "IX_SellOrders_ApplicationUserUserID",
                table: "SellOrders");

            migrationBuilder.DropIndex(
                name: "IX_BuyOrders_ApplicationUserUserID",
                table: "BuyOrders");

            migrationBuilder.DropColumn(
                name: "ApplicationUserUserID",
                table: "SellOrders");

            migrationBuilder.DropColumn(
                name: "ApplicationUserUserID",
                table: "BuyOrders");

            migrationBuilder.CreateIndex(
                name: "IX_SellOrders_UserID",
                table: "SellOrders",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_BuyOrders_UserID",
                table: "BuyOrders",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_BuyOrders_Users_UserID",
                table: "BuyOrders",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SellOrders_Users_UserID",
                table: "SellOrders",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BuyOrders_Users_UserID",
                table: "BuyOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_SellOrders_Users_UserID",
                table: "SellOrders");

            migrationBuilder.DropIndex(
                name: "IX_SellOrders_UserID",
                table: "SellOrders");

            migrationBuilder.DropIndex(
                name: "IX_BuyOrders_UserID",
                table: "BuyOrders");

            migrationBuilder.AddColumn<Guid>(
                name: "ApplicationUserUserID",
                table: "SellOrders",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ApplicationUserUserID",
                table: "BuyOrders",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SellOrders_ApplicationUserUserID",
                table: "SellOrders",
                column: "ApplicationUserUserID");

            migrationBuilder.CreateIndex(
                name: "IX_BuyOrders_ApplicationUserUserID",
                table: "BuyOrders",
                column: "ApplicationUserUserID");

            migrationBuilder.AddForeignKey(
                name: "FK_BuyOrders_Users_ApplicationUserUserID",
                table: "BuyOrders",
                column: "ApplicationUserUserID",
                principalTable: "Users",
                principalColumn: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_SellOrders_Users_ApplicationUserUserID",
                table: "SellOrders",
                column: "ApplicationUserUserID",
                principalTable: "Users",
                principalColumn: "UserID");
        }
    }
}
