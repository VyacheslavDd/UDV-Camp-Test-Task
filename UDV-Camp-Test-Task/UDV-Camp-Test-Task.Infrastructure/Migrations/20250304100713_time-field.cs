using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UDV_Camp_Test_Task.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class timefield : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CalculatedAt",
                table: "ParseResults",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CalculatedAt",
                table: "ParseResults");
        }
    }
}
