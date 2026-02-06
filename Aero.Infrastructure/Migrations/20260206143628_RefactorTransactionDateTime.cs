using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Aero.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RefactorTransactionDateTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "date",
                table: "transaction");

            migrationBuilder.DropColumn(
                name: "time",
                table: "transaction");

            migrationBuilder.AddColumn<DateTime>(
                name: "date_time",
                table: "transaction",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "date_time",
                table: "transaction");

            migrationBuilder.AddColumn<string>(
                name: "date",
                table: "transaction",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "time",
                table: "transaction",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
