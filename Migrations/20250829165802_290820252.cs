using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HIDAeroService.Migrations
{
    /// <inheritdoc />
    public partial class _290820252 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ArTzs",
                table: "ArTzs");

            migrationBuilder.RenameTable(
                name: "ArTzs",
                newName: "ArTimeZones");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ArTimeZones",
                table: "ArTimeZones",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ArTimeZones",
                table: "ArTimeZones");

            migrationBuilder.RenameTable(
                name: "ArTimeZones",
                newName: "ArTzs");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ArTzs",
                table: "ArTzs",
                column: "Id");
        }
    }
}
