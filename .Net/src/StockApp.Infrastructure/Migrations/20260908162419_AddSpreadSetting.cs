using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StockApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSpreadSetting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SpreadSettings",
                columns: table => new
                {
                    SpreadSettingID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SpreadPercentage = table.Column<decimal>(type: "decimal(9,4)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpreadSettings", x => x.SpreadSettingID);
                });

            migrationBuilder.InsertData(
                table: "SpreadSettings",
                columns: new[] { "SpreadSettingID", "SpreadPercentage", "UpdatedAt" },
                values: new object[] { new Guid("30000000-0000-0000-0000-000000000001"), 0.2m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SpreadSettings");
        }
    }
}
