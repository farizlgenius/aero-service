using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Aero.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeDayinWeek : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_days_in_week_location_locationid",
                table: "days_in_week");

            migrationBuilder.DropIndex(
                name: "IX_days_in_week_locationid",
                table: "days_in_week");

            migrationBuilder.DropColumn(
                name: "created_date",
                table: "days_in_week");

            migrationBuilder.DropColumn(
                name: "is_active",
                table: "days_in_week");

            migrationBuilder.DropColumn(
                name: "location_id",
                table: "days_in_week");

            migrationBuilder.DropColumn(
                name: "locationid",
                table: "days_in_week");

            migrationBuilder.DropColumn(
                name: "updated_date",
                table: "days_in_week");

            migrationBuilder.UpdateData(
                table: "access_level",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_date", "updated_date" },
                values: new object[] { new DateTime(2026, 3, 4, 11, 0, 40, 178, DateTimeKind.Utc).AddTicks(7245), new DateTime(2026, 3, 4, 11, 0, 40, 178, DateTimeKind.Utc).AddTicks(7246) });

            migrationBuilder.UpdateData(
                table: "access_level",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "created_date", "updated_date" },
                values: new object[] { new DateTime(2026, 3, 4, 11, 0, 40, 178, DateTimeKind.Utc).AddTicks(7249), new DateTime(2026, 3, 4, 11, 0, 40, 178, DateTimeKind.Utc).AddTicks(7250) });

            migrationBuilder.UpdateData(
                table: "card_format",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_date", "updated_date" },
                values: new object[] { new DateTime(2026, 3, 4, 11, 0, 40, 183, DateTimeKind.Utc).AddTicks(2346), new DateTime(2026, 3, 4, 11, 0, 40, 183, DateTimeKind.Utc).AddTicks(2347) });

            migrationBuilder.UpdateData(
                table: "location",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_date", "updated_date" },
                values: new object[] { new DateTime(2026, 3, 4, 11, 0, 40, 165, DateTimeKind.Utc).AddTicks(2322), new DateTime(2026, 3, 4, 11, 0, 40, 165, DateTimeKind.Utc).AddTicks(2324) });

            migrationBuilder.UpdateData(
                table: "location",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "created_date", "updated_date" },
                values: new object[] { new DateTime(2026, 3, 4, 11, 0, 40, 165, DateTimeKind.Utc).AddTicks(2327), new DateTime(2026, 3, 4, 11, 0, 40, 165, DateTimeKind.Utc).AddTicks(2327) });

            migrationBuilder.UpdateData(
                table: "operator",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_date", "updated_date" },
                values: new object[] { new DateTime(2026, 3, 4, 11, 0, 40, 188, DateTimeKind.Utc).AddTicks(6882), new DateTime(2026, 3, 4, 11, 0, 40, 188, DateTimeKind.Utc).AddTicks(6883) });

            migrationBuilder.UpdateData(
                table: "role",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_date", "updated_date" },
                values: new object[] { new DateTime(2026, 3, 4, 11, 0, 40, 187, DateTimeKind.Utc).AddTicks(4513), new DateTime(2026, 3, 4, 11, 0, 40, 187, DateTimeKind.Utc).AddTicks(4514) });

            migrationBuilder.UpdateData(
                table: "timezone",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_date", "updated_date" },
                values: new object[] { new DateTime(2026, 3, 4, 11, 0, 40, 179, DateTimeKind.Utc).AddTicks(5435), new DateTime(2026, 3, 4, 11, 0, 40, 179, DateTimeKind.Utc).AddTicks(5436) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "created_date",
                table: "days_in_week",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "is_active",
                table: "days_in_week",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "location_id",
                table: "days_in_week",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "locationid",
                table: "days_in_week",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_date",
                table: "days_in_week",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "access_level",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_date", "updated_date" },
                values: new object[] { new DateTime(2026, 3, 4, 3, 35, 40, 416, DateTimeKind.Utc).AddTicks(4891), new DateTime(2026, 3, 4, 3, 35, 40, 416, DateTimeKind.Utc).AddTicks(4893) });

            migrationBuilder.UpdateData(
                table: "access_level",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "created_date", "updated_date" },
                values: new object[] { new DateTime(2026, 3, 4, 3, 35, 40, 416, DateTimeKind.Utc).AddTicks(4896), new DateTime(2026, 3, 4, 3, 35, 40, 416, DateTimeKind.Utc).AddTicks(4897) });

            migrationBuilder.UpdateData(
                table: "card_format",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_date", "updated_date" },
                values: new object[] { new DateTime(2026, 3, 4, 3, 35, 40, 422, DateTimeKind.Utc).AddTicks(5940), new DateTime(2026, 3, 4, 3, 35, 40, 422, DateTimeKind.Utc).AddTicks(5942) });

            migrationBuilder.UpdateData(
                table: "location",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_date", "updated_date" },
                values: new object[] { new DateTime(2026, 3, 4, 3, 35, 40, 382, DateTimeKind.Utc).AddTicks(4917), new DateTime(2026, 3, 4, 3, 35, 40, 382, DateTimeKind.Utc).AddTicks(4918) });

            migrationBuilder.UpdateData(
                table: "location",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "created_date", "updated_date" },
                values: new object[] { new DateTime(2026, 3, 4, 3, 35, 40, 382, DateTimeKind.Utc).AddTicks(4921), new DateTime(2026, 3, 4, 3, 35, 40, 382, DateTimeKind.Utc).AddTicks(4922) });

            migrationBuilder.UpdateData(
                table: "operator",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_date", "updated_date" },
                values: new object[] { new DateTime(2026, 3, 4, 3, 35, 40, 428, DateTimeKind.Utc).AddTicks(6776), new DateTime(2026, 3, 4, 3, 35, 40, 428, DateTimeKind.Utc).AddTicks(6777) });

            migrationBuilder.UpdateData(
                table: "role",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_date", "updated_date" },
                values: new object[] { new DateTime(2026, 3, 4, 3, 35, 40, 427, DateTimeKind.Utc).AddTicks(2839), new DateTime(2026, 3, 4, 3, 35, 40, 427, DateTimeKind.Utc).AddTicks(2841) });

            migrationBuilder.UpdateData(
                table: "timezone",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_date", "updated_date" },
                values: new object[] { new DateTime(2026, 3, 4, 3, 35, 40, 417, DateTimeKind.Utc).AddTicks(5216), new DateTime(2026, 3, 4, 3, 35, 40, 417, DateTimeKind.Utc).AddTicks(5217) });

            migrationBuilder.CreateIndex(
                name: "IX_days_in_week_locationid",
                table: "days_in_week",
                column: "locationid");

            migrationBuilder.AddForeignKey(
                name: "FK_days_in_week_location_locationid",
                table: "days_in_week",
                column: "locationid",
                principalTable: "location",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
