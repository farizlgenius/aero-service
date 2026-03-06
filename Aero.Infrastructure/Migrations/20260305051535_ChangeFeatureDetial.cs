using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Aero.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeFeatureDetial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "access_level",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_date", "updated_date" },
                values: new object[] { new DateTime(2026, 3, 5, 5, 15, 31, 456, DateTimeKind.Utc).AddTicks(9856), new DateTime(2026, 3, 5, 5, 15, 31, 456, DateTimeKind.Utc).AddTicks(9858) });

            migrationBuilder.UpdateData(
                table: "access_level",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "created_date", "updated_date" },
                values: new object[] { new DateTime(2026, 3, 5, 5, 15, 31, 456, DateTimeKind.Utc).AddTicks(9864), new DateTime(2026, 3, 5, 5, 15, 31, 456, DateTimeKind.Utc).AddTicks(9876) });

            migrationBuilder.UpdateData(
                table: "card_format",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_date", "updated_date" },
                values: new object[] { new DateTime(2026, 3, 5, 5, 15, 31, 464, DateTimeKind.Utc).AddTicks(5809), new DateTime(2026, 3, 5, 5, 15, 31, 464, DateTimeKind.Utc).AddTicks(5812) });

            migrationBuilder.UpdateData(
                table: "feature",
                keyColumn: "id",
                keyValue: 2,
                column: "name",
                value: "Events");

            migrationBuilder.UpdateData(
                table: "feature",
                keyColumn: "id",
                keyValue: 3,
                column: "name",
                value: "Location");

            migrationBuilder.UpdateData(
                table: "feature",
                keyColumn: "id",
                keyValue: 5,
                column: "name",
                value: "Operator");

            migrationBuilder.UpdateData(
                table: "feature",
                keyColumn: "id",
                keyValue: 11,
                column: "name",
                value: "Time");

            migrationBuilder.UpdateData(
                table: "feature",
                keyColumn: "id",
                keyValue: 12,
                column: "name",
                value: "Trigger & procedure");

            migrationBuilder.UpdateData(
                table: "feature",
                keyColumn: "id",
                keyValue: 18,
                column: "name",
                value: "MonitorGroup");

            migrationBuilder.UpdateData(
                table: "location",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_date", "updated_date" },
                values: new object[] { new DateTime(2026, 3, 5, 5, 15, 31, 433, DateTimeKind.Utc).AddTicks(1342), new DateTime(2026, 3, 5, 5, 15, 31, 433, DateTimeKind.Utc).AddTicks(1346) });

            migrationBuilder.UpdateData(
                table: "location",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "created_date", "updated_date" },
                values: new object[] { new DateTime(2026, 3, 5, 5, 15, 31, 433, DateTimeKind.Utc).AddTicks(1350), new DateTime(2026, 3, 5, 5, 15, 31, 433, DateTimeKind.Utc).AddTicks(1350) });

            migrationBuilder.UpdateData(
                table: "operator",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_date", "updated_date" },
                values: new object[] { new DateTime(2026, 3, 5, 5, 15, 31, 474, DateTimeKind.Utc).AddTicks(1693), new DateTime(2026, 3, 5, 5, 15, 31, 474, DateTimeKind.Utc).AddTicks(1695) });

            migrationBuilder.UpdateData(
                table: "role",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_date", "updated_date" },
                values: new object[] { new DateTime(2026, 3, 5, 5, 15, 31, 471, DateTimeKind.Utc).AddTicks(8162), new DateTime(2026, 3, 5, 5, 15, 31, 471, DateTimeKind.Utc).AddTicks(8165) });

            migrationBuilder.UpdateData(
                table: "sub_feature",
                keyColumn: "id",
                keyValue: 1,
                column: "name",
                value: "Operator");

            migrationBuilder.UpdateData(
                table: "sub_feature",
                keyColumn: "id",
                keyValue: 2,
                column: "name",
                value: "Role");

            migrationBuilder.UpdateData(
                table: "sub_feature",
                keyColumn: "id",
                keyValue: 3,
                columns: new[] { "name", "path" },
                values: new object[] { "Device", "/device" });

            migrationBuilder.UpdateData(
                table: "sub_feature",
                keyColumn: "id",
                keyValue: 4,
                column: "name",
                value: "Modules");

            migrationBuilder.UpdateData(
                table: "sub_feature",
                keyColumn: "id",
                keyValue: 6,
                column: "name",
                value: "Holiday");

            migrationBuilder.UpdateData(
                table: "sub_feature",
                keyColumn: "id",
                keyValue: 7,
                column: "name",
                value: "Interval");

            migrationBuilder.UpdateData(
                table: "sub_feature",
                keyColumn: "id",
                keyValue: 8,
                column: "name",
                value: "Trigger");

            migrationBuilder.UpdateData(
                table: "sub_feature",
                keyColumn: "id",
                keyValue: 9,
                column: "name",
                value: "Procedure");

            migrationBuilder.UpdateData(
                table: "sub_feature",
                keyColumn: "id",
                keyValue: 10,
                column: "name",
                value: "Transaction");

            migrationBuilder.UpdateData(
                table: "timezone",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_date", "updated_date" },
                values: new object[] { new DateTime(2026, 3, 5, 5, 15, 31, 458, DateTimeKind.Utc).AddTicks(3834), new DateTime(2026, 3, 5, 5, 15, 31, 458, DateTimeKind.Utc).AddTicks(3836) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                table: "feature",
                keyColumn: "id",
                keyValue: 2,
                column: "name",
                value: "transaction");

            migrationBuilder.UpdateData(
                table: "feature",
                keyColumn: "id",
                keyValue: 3,
                column: "name",
                value: "location");

            migrationBuilder.UpdateData(
                table: "feature",
                keyColumn: "id",
                keyValue: 5,
                column: "name",
                value: "operator");

            migrationBuilder.UpdateData(
                table: "feature",
                keyColumn: "id",
                keyValue: 11,
                column: "name",
                value: "time");

            migrationBuilder.UpdateData(
                table: "feature",
                keyColumn: "id",
                keyValue: 12,
                column: "name",
                value: "trigger & procedure");

            migrationBuilder.UpdateData(
                table: "feature",
                keyColumn: "id",
                keyValue: 18,
                column: "name",
                value: "monitor_group");

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
                table: "sub_feature",
                keyColumn: "id",
                keyValue: 1,
                column: "name",
                value: "operator");

            migrationBuilder.UpdateData(
                table: "sub_feature",
                keyColumn: "id",
                keyValue: 2,
                column: "name",
                value: "role");

            migrationBuilder.UpdateData(
                table: "sub_feature",
                keyColumn: "id",
                keyValue: 3,
                columns: new[] { "name", "path" },
                values: new object[] { "hardware", "/hardware" });

            migrationBuilder.UpdateData(
                table: "sub_feature",
                keyColumn: "id",
                keyValue: 4,
                column: "name",
                value: "modules");

            migrationBuilder.UpdateData(
                table: "sub_feature",
                keyColumn: "id",
                keyValue: 6,
                column: "name",
                value: "holiday");

            migrationBuilder.UpdateData(
                table: "sub_feature",
                keyColumn: "id",
                keyValue: 7,
                column: "name",
                value: "interval");

            migrationBuilder.UpdateData(
                table: "sub_feature",
                keyColumn: "id",
                keyValue: 8,
                column: "name",
                value: "trigger");

            migrationBuilder.UpdateData(
                table: "sub_feature",
                keyColumn: "id",
                keyValue: 9,
                column: "name",
                value: "procedure");

            migrationBuilder.UpdateData(
                table: "sub_feature",
                keyColumn: "id",
                keyValue: 10,
                column: "name",
                value: "transaction");

            migrationBuilder.UpdateData(
                table: "timezone",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_date", "updated_date" },
                values: new object[] { new DateTime(2026, 3, 4, 11, 0, 40, 179, DateTimeKind.Utc).AddTicks(5435), new DateTime(2026, 3, 4, 11, 0, 40, 179, DateTimeKind.Utc).AddTicks(5436) });
        }
    }
}
