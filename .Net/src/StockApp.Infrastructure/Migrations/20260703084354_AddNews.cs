using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace StockApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddNews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AdvisorID",
                table: "Users",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "NewsItems",
                columns: table => new
                {
                    NewsID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Headline = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Source = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PublishedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SourceUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewsItems", x => x.NewsID);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserID",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                column: "AdvisorID",
                value: null);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserID",
                keyValue: new Guid("00000000-0000-0000-0000-000000000002"),
                column: "AdvisorID",
                value: null);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserID", "AdvisorID", "Email", "RoleID" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000003"), null, "advisor@test.com", new Guid("10000000-0000-0000-0000-000000000003") },
                    { new Guid("00000000-0000-0000-0000-000000000010"), new Guid("00000000-0000-0000-0000-000000000003"), "john.carter@example.com", new Guid("10000000-0000-0000-0000-000000000001") },
                    { new Guid("00000000-0000-0000-0000-000000000011"), new Guid("00000000-0000-0000-0000-000000000003"), "sofia.hernandez@example.com", new Guid("10000000-0000-0000-0000-000000000001") },
                    { new Guid("00000000-0000-0000-0000-000000000012"), new Guid("00000000-0000-0000-0000-000000000003"), "marcus.lee@example.com", new Guid("10000000-0000-0000-0000-000000000001") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_AdvisorID",
                table: "Users",
                column: "AdvisorID");

            migrationBuilder.CreateIndex(
                name: "IX_NewsItems_Category",
                table: "NewsItems",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_NewsItems_PublishedDate",
                table: "NewsItems",
                column: "PublishedDate",
                descending: new bool[0]);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Users_AdvisorID",
                table: "Users",
                column: "AdvisorID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Users_AdvisorID",
                table: "Users");

            migrationBuilder.DropTable(
                name: "NewsItems");

            migrationBuilder.DropIndex(
                name: "IX_Users_AdvisorID",
                table: "Users");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserID",
                keyValue: new Guid("00000000-0000-0000-0000-000000000010"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserID",
                keyValue: new Guid("00000000-0000-0000-0000-000000000011"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserID",
                keyValue: new Guid("00000000-0000-0000-0000-000000000012"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserID",
                keyValue: new Guid("00000000-0000-0000-0000-000000000003"));

            migrationBuilder.DropColumn(
                name: "AdvisorID",
                table: "Users");
        }
    }
}
