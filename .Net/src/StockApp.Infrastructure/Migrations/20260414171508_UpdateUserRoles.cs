using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace StockApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumn: "RoleID",
                keyValue: new Guid("10000000-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumn: "RoleID",
                keyValue: new Guid("10000000-0000-0000-0000-000000000005"));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumn: "RoleID",
                keyValue: new Guid("10000000-0000-0000-0000-000000000001"),
                column: "RoleName",
                value: "Customer");

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumn: "RoleID",
                keyValue: new Guid("10000000-0000-0000-0000-000000000003"),
                column: "RoleName",
                value: "Advisor");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumn: "RoleID",
                keyValue: new Guid("10000000-0000-0000-0000-000000000001"),
                column: "RoleName",
                value: "User");

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumn: "RoleID",
                keyValue: new Guid("10000000-0000-0000-0000-000000000003"),
                column: "RoleName",
                value: "Analyst");

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "RoleID", "RoleName" },
                values: new object[,]
                {
                    { new Guid("10000000-0000-0000-0000-000000000004"), "Moderator" },
                    { new Guid("10000000-0000-0000-0000-000000000005"), "Viewer" }
                });
        }
    }
}
