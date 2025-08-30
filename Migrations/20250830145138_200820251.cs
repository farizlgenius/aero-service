using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HIDAeroService.Migrations
{
    /// <inheritdoc />
    public partial class _200820251 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IStartTime",
                table: "ArIntervals",
                newName: "StartTime");

            migrationBuilder.RenameColumn(
                name: "IEndTime",
                table: "ArIntervals",
                newName: "Endtime");

            migrationBuilder.RenameColumn(
                name: "IDaysDays",
                table: "ArIntervals",
                newName: "Days");

            migrationBuilder.UpdateData(
                table: "ArTimeZones",
                keyColumn: "Id",
                keyValue: 1,
                column: "IsActive",
                value: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StartTime",
                table: "ArIntervals",
                newName: "IStartTime");

            migrationBuilder.RenameColumn(
                name: "Endtime",
                table: "ArIntervals",
                newName: "IEndTime");

            migrationBuilder.RenameColumn(
                name: "Days",
                table: "ArIntervals",
                newName: "IDaysDays");

            migrationBuilder.UpdateData(
                table: "ArTimeZones",
                keyColumn: "Id",
                keyValue: 1,
                column: "IsActive",
                value: false);
        }
    }
}
