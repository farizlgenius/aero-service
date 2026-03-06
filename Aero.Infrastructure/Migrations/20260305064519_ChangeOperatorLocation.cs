using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Aero.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeOperatorLocation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "operator_location",
                keyColumns: new[] { "location_id", "operator_id" },
                keyValues: new object[] { 1, 1 });

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

            migrationBuilder.InsertData(
                table: "operator_location",
                columns: new[] { "location_id", "operator_id", "id" },
                values: new object[] { 2, 1, 1 });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "operator_location",
                keyColumns: new[] { "location_id", "operator_id" },
                keyValues: new object[] { 2, 1 });

            migrationBuilder.UpdateData(
                table: "access_level",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_date", "updated_date" },
                values: new object[] { new DateTime(2026, 3, 5, 5, 27, 35, 586, DateTimeKind.Utc).AddTicks(8830), new DateTime(2026, 3, 5, 5, 27, 35, 586, DateTimeKind.Utc).AddTicks(8832) });

            migrationBuilder.UpdateData(
                table: "access_level",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "created_date", "updated_date" },
                values: new object[] { new DateTime(2026, 3, 5, 5, 27, 35, 586, DateTimeKind.Utc).AddTicks(8837), new DateTime(2026, 3, 5, 5, 27, 35, 586, DateTimeKind.Utc).AddTicks(8837) });

            migrationBuilder.UpdateData(
                table: "card_format",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_date", "updated_date" },
                values: new object[] { new DateTime(2026, 3, 5, 5, 27, 35, 595, DateTimeKind.Utc).AddTicks(9806), new DateTime(2026, 3, 5, 5, 27, 35, 595, DateTimeKind.Utc).AddTicks(9808) });

            migrationBuilder.UpdateData(
                table: "location",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_date", "updated_date" },
                values: new object[] { new DateTime(2026, 3, 5, 5, 27, 35, 569, DateTimeKind.Utc).AddTicks(1345), new DateTime(2026, 3, 5, 5, 27, 35, 569, DateTimeKind.Utc).AddTicks(1353) });

            migrationBuilder.UpdateData(
                table: "location",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "created_date", "updated_date" },
                values: new object[] { new DateTime(2026, 3, 5, 5, 27, 35, 569, DateTimeKind.Utc).AddTicks(1356), new DateTime(2026, 3, 5, 5, 27, 35, 569, DateTimeKind.Utc).AddTicks(1356) });

            migrationBuilder.UpdateData(
                table: "operator",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_date", "updated_date" },
                values: new object[] { new DateTime(2026, 3, 5, 5, 27, 35, 608, DateTimeKind.Utc).AddTicks(4379), new DateTime(2026, 3, 5, 5, 27, 35, 608, DateTimeKind.Utc).AddTicks(4380) });

            migrationBuilder.InsertData(
                table: "operator_location",
                columns: new[] { "location_id", "operator_id", "id" },
                values: new object[] { 1, 1, 1 });

            migrationBuilder.UpdateData(
                table: "role",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_date", "updated_date" },
                values: new object[] { new DateTime(2026, 3, 5, 5, 27, 35, 607, DateTimeKind.Utc).AddTicks(304), new DateTime(2026, 3, 5, 5, 27, 35, 607, DateTimeKind.Utc).AddTicks(307) });

            migrationBuilder.UpdateData(
                table: "timezone",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_date", "updated_date" },
                values: new object[] { new DateTime(2026, 3, 5, 5, 27, 35, 589, DateTimeKind.Utc).AddTicks(746), new DateTime(2026, 3, 5, 5, 27, 35, 589, DateTimeKind.Utc).AddTicks(748) });
        }
    }
}
