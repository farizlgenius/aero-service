using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HIDAeroService.Migrations
{
    /// <inheritdoc />
    public partial class _14122025857 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Triggers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "TransactionSources",
                keyColumn: "Id",
                keyValue: 22,
                column: "Source",
                value: "Hardware");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Triggers");

            migrationBuilder.UpdateData(
                table: "TransactionSources",
                keyColumn: "Id",
                keyValue: 22,
                column: "Source",
                value: "");
        }
    }
}
