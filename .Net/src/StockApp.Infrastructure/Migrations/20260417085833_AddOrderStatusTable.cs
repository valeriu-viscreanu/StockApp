using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace StockApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderStatusTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrderStatuses",
                columns: table => new
                {
                    OrderStatusID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StatusName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderStatuses", x => x.OrderStatusID);
                });

            migrationBuilder.InsertData(
                table: "OrderStatuses",
                columns: new[] { "OrderStatusID", "StatusName" },
                values: new object[,]
                {
                    { new Guid("20000000-0000-0000-0000-000000000001"), "Pending" },
                    { new Guid("20000000-0000-0000-0000-000000000002"), "Authorized" },
                    { new Guid("20000000-0000-0000-0000-000000000003"), "Processed" },
                    { new Guid("20000000-0000-0000-0000-000000000004"), "Canceled" }
                });

            migrationBuilder.DropColumn(
                name: "Status",
                table: "SellOrders");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "BuyOrders");

            migrationBuilder.AddColumn<Guid>(
                name: "OrderStatusID",
                table: "SellOrders",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("20000000-0000-0000-0000-000000000001"));

            migrationBuilder.AddColumn<Guid>(
                name: "OrderStatusID",
                table: "BuyOrders",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("20000000-0000-0000-0000-000000000001"));

            migrationBuilder.CreateIndex(
                name: "IX_SellOrders_OrderStatusID",
                table: "SellOrders",
                column: "OrderStatusID");

            migrationBuilder.CreateIndex(
                name: "IX_BuyOrders_OrderStatusID",
                table: "BuyOrders",
                column: "OrderStatusID");

            migrationBuilder.AddForeignKey(
                name: "FK_BuyOrders_OrderStatuses_OrderStatusID",
                table: "BuyOrders",
                column: "OrderStatusID",
                principalTable: "OrderStatuses",
                principalColumn: "OrderStatusID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SellOrders_OrderStatuses_OrderStatusID",
                table: "SellOrders",
                column: "OrderStatusID",
                principalTable: "OrderStatuses",
                principalColumn: "OrderStatusID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BuyOrders_OrderStatuses_OrderStatusID",
                table: "BuyOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_SellOrders_OrderStatuses_OrderStatusID",
                table: "SellOrders");

            migrationBuilder.DropTable(
                name: "OrderStatuses");

            migrationBuilder.DropIndex(
                name: "IX_SellOrders_OrderStatusID",
                table: "SellOrders");

            migrationBuilder.DropIndex(
                name: "IX_BuyOrders_OrderStatusID",
                table: "BuyOrders");

            migrationBuilder.DropColumn(
                name: "OrderStatusID",
                table: "SellOrders");

            migrationBuilder.DropColumn(
                name: "OrderStatusID",
                table: "BuyOrders");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "SellOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "BuyOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
