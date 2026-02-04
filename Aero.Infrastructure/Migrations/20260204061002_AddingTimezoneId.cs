using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Aero.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddingTimezoneId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<short>(
                name: "timezone_id",
                table: "timezone",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.UpdateData(
                table: "timezone",
                keyColumn: "id",
                keyValue: 1,
                column: "timezone_id",
                value: (short)0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "timezone_id",
                table: "timezone");
        }
    }
}
