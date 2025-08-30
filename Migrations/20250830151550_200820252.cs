using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HIDAeroService.Migrations
{
    /// <inheritdoc />
    public partial class _200820252 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ScpMac",
                table: "ArHolidays");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ScpMac",
                table: "ArHolidays",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
