using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Aero.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveOperatorLocationId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "id",
                table: "operator_location");

            migrationBuilder.UpdateData(
                table: "access_level",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_date", "updated_date" },
                values: new object[] { new DateTime(2026, 3, 5, 7, 10, 3, 472, DateTimeKind.Utc).AddTicks(1601), new DateTime(2026, 3, 5, 7, 10, 3, 472, DateTimeKind.Utc).AddTicks(1604) });

            migrationBuilder.UpdateData(
                table: "access_level",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "created_date", "updated_date" },
                values: new object[] { new DateTime(2026, 3, 5, 7, 10, 3, 472, DateTimeKind.Utc).AddTicks(1612), new DateTime(2026, 3, 5, 7, 10, 3, 472, DateTimeKind.Utc).AddTicks(1613) });

            migrationBuilder.UpdateData(
                table: "card_format",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_date", "updated_date" },
                values: new object[] { new DateTime(2026, 3, 5, 7, 10, 3, 478, DateTimeKind.Utc).AddTicks(9869), new DateTime(2026, 3, 5, 7, 10, 3, 478, DateTimeKind.Utc).AddTicks(9871) });

            migrationBuilder.UpdateData(
                table: "location",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_date", "updated_date" },
                values: new object[] { new DateTime(2026, 3, 5, 7, 10, 3, 450, DateTimeKind.Utc).AddTicks(2858), new DateTime(2026, 3, 5, 7, 10, 3, 450, DateTimeKind.Utc).AddTicks(2861) });

            migrationBuilder.UpdateData(
                table: "location",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "created_date", "updated_date" },
                values: new object[] { new DateTime(2026, 3, 5, 7, 10, 3, 450, DateTimeKind.Utc).AddTicks(2864), new DateTime(2026, 3, 5, 7, 10, 3, 450, DateTimeKind.Utc).AddTicks(2864) });

            migrationBuilder.UpdateData(
                table: "operator",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_date", "updated_date" },
                values: new object[] { new DateTime(2026, 3, 5, 7, 10, 3, 489, DateTimeKind.Utc).AddTicks(9384), new DateTime(2026, 3, 5, 7, 10, 3, 489, DateTimeKind.Utc).AddTicks(9387) });

            migrationBuilder.UpdateData(
                table: "role",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_date", "updated_date" },
                values: new object[] { new DateTime(2026, 3, 5, 7, 10, 3, 488, DateTimeKind.Utc).AddTicks(313), new DateTime(2026, 3, 5, 7, 10, 3, 488, DateTimeKind.Utc).AddTicks(316) });

            migrationBuilder.UpdateData(
                table: "timezone",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_date", "updated_date" },
                values: new object[] { new DateTime(2026, 3, 5, 7, 10, 3, 473, DateTimeKind.Utc).AddTicks(4455), new DateTime(2026, 3, 5, 7, 10, 3, 473, DateTimeKind.Utc).AddTicks(4458) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "id",
                table: "operator_location",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "access_level",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_date", "updated_date" },
                values: new object[] { new DateTime(2026, 3, 5, 6, 45, 16, 70, DateTimeKind.Utc).AddTicks(115), new DateTime(2026, 3, 5, 6, 45, 16, 70, DateTimeKind.Utc).AddTicks(117) });

            migrationBuilder.UpdateData(
                table: "access_level",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "created_date", "updated_date" },
                values: new object[] { new DateTime(2026, 3, 5, 6, 45, 16, 70, DateTimeKind.Utc).AddTicks(120), new DateTime(2026, 3, 5, 6, 45, 16, 70, DateTimeKind.Utc).AddTicks(127) });

            migrationBuilder.UpdateData(
                table: "card_format",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_date", "updated_date" },
                values: new object[] { new DateTime(2026, 3, 5, 6, 45, 16, 74, DateTimeKind.Utc).AddTicks(2170), new DateTime(2026, 3, 5, 6, 45, 16, 74, DateTimeKind.Utc).AddTicks(2170) });

            migrationBuilder.UpdateData(
                table: "location",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_date", "updated_date" },
                values: new object[] { new DateTime(2026, 3, 5, 6, 45, 16, 56, DateTimeKind.Utc).AddTicks(3326), new DateTime(2026, 3, 5, 6, 45, 16, 56, DateTimeKind.Utc).AddTicks(3329) });

            migrationBuilder.UpdateData(
                table: "location",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "created_date", "updated_date" },
                values: new object[] { new DateTime(2026, 3, 5, 6, 45, 16, 56, DateTimeKind.Utc).AddTicks(3331), new DateTime(2026, 3, 5, 6, 45, 16, 56, DateTimeKind.Utc).AddTicks(3331) });

            migrationBuilder.UpdateData(
                table: "operator",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_date", "updated_date" },
                values: new object[] { new DateTime(2026, 3, 5, 6, 45, 16, 87, DateTimeKind.Utc).AddTicks(6059), new DateTime(2026, 3, 5, 6, 45, 16, 87, DateTimeKind.Utc).AddTicks(6061) });

            migrationBuilder.UpdateData(
                table: "operator_location",
                keyColumns: new[] { "location_id", "operator_id" },
                keyValues: new object[] { 2, 1 },
                column: "id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "role",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_date", "updated_date" },
                values: new object[] { new DateTime(2026, 3, 5, 6, 45, 16, 81, DateTimeKind.Utc).AddTicks(8259), new DateTime(2026, 3, 5, 6, 45, 16, 81, DateTimeKind.Utc).AddTicks(8260) });

            migrationBuilder.UpdateData(
                table: "timezone",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_date", "updated_date" },
                values: new object[] { new DateTime(2026, 3, 5, 6, 45, 16, 70, DateTimeKind.Utc).AddTicks(7740), new DateTime(2026, 3, 5, 6, 45, 16, 70, DateTimeKind.Utc).AddTicks(7741) });
        }
    }
}
