using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Aero.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialDatabase3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "timezone",
                keyColumn: "id",
                keyValue: 1,
                column: "timezone_id",
                value: (short)1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "timezone",
                keyColumn: "id",
                keyValue: 1,
                column: "timezone_id",
                value: (short)0);
        }
    }
}
