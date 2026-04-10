using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace StockApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "RoleID",
                table: "Users",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    RoleID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => x.RoleID);
                });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "RoleID", "RoleName" },
                values: new object[,]
                {
                    { new Guid("10000000-0000-0000-0000-000000000001"), "User" },
                    { new Guid("10000000-0000-0000-0000-000000000002"), "Admin" },
                    { new Guid("10000000-0000-0000-0000-000000000003"), "Analyst" },
                    { new Guid("10000000-0000-0000-0000-000000000004"), "Moderator" },
                    { new Guid("10000000-0000-0000-0000-000000000005"), "Viewer" }
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserID",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                column: "RoleID",
                value: null);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserID",
                keyValue: new Guid("00000000-0000-0000-0000-000000000002"),
                column: "RoleID",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleID",
                table: "Users",
                column: "RoleID");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_UserRoles_RoleID",
                table: "Users",
                column: "RoleID",
                principalTable: "UserRoles",
                principalColumn: "RoleID",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_UserRoles_RoleID",
                table: "Users");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropIndex(
                name: "IX_Users_RoleID",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "RoleID",
                table: "Users");
        }
    }
}
