using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HIDAeroService.Migrations
{
    /// <inheritdoc />
    public partial class _120920251 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.AddColumn<short>(
                name: "LocationId",
                table: "TimeZones",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "LocationId",
                table: "Intervals",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.UpdateData(
                table: "AccessLevels",
                keyColumn: "Id",
                keyValue: 1,
                column: "LocationId",
                value: (short)0);

            migrationBuilder.UpdateData(
                table: "AccessLevels",
                keyColumn: "Id",
                keyValue: 2,
                column: "LocationId",
                value: (short)0);

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "Id", "ComponentId", "CreatedDate", "Description", "IsActive", "LocationName", "Uuid" },
                values: new object[,]
                {
                    { 1, (short)0, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Central Location", true, "Central", "00000000-0000-0000-0000-000000000001" },
                    { 2, (short)1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Main Location", true, "Main", "00000000-0000-0000-0000-000000000001" }
                });

            migrationBuilder.UpdateData(
                table: "TimeZones",
                keyColumn: "Id",
                keyValue: 1,
                column: "LocationId",
                value: (short)0);

            migrationBuilder.CreateIndex(
                name: "IX_TimeZones_LocationId",
                table: "TimeZones",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Intervals_LocationId",
                table: "Intervals",
                column: "LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Intervals_Locations_LocationId",
                table: "Intervals",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "ComponentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TimeZones_Locations_LocationId",
                table: "TimeZones",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "ComponentId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Intervals_Locations_LocationId",
                table: "Intervals");

            migrationBuilder.DropForeignKey(
                name: "FK_TimeZones_Locations_LocationId",
                table: "TimeZones");

            migrationBuilder.DropIndex(
                name: "IX_TimeZones_LocationId",
                table: "TimeZones");

            migrationBuilder.DropIndex(
                name: "IX_Intervals_LocationId",
                table: "Intervals");

            migrationBuilder.DeleteData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "TimeZones");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "Intervals");

            migrationBuilder.UpdateData(
                table: "AccessLevels",
                keyColumn: "Id",
                keyValue: 1,
                column: "LocationId",
                value: (short)1);

            migrationBuilder.UpdateData(
                table: "AccessLevels",
                keyColumn: "Id",
                keyValue: 2,
                column: "LocationId",
                value: (short)1);

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "Id", "ComponentId", "CreatedDate", "Description", "IsActive", "LocationName", "Uuid" },
                values: new object[] { 1, (short)1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Main Location", true, "Main", "00000000-0000-0000-0000-000000000001" });
        }
    }
}
