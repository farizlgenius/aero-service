using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Aero.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUserIdFromOperator : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "user_id",
                table: "refresh_token");

            migrationBuilder.DropColumn(
                name: "user_id",
                table: "operator");

            migrationBuilder.UpdateData(
                table: "access_level",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_date", "updated_date" },
                values: new object[] { new DateTime(2026, 3, 5, 7, 25, 37, 525, DateTimeKind.Utc).AddTicks(5938), new DateTime(2026, 3, 5, 7, 25, 37, 525, DateTimeKind.Utc).AddTicks(5940) });

            migrationBuilder.UpdateData(
                table: "access_level",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "created_date", "updated_date" },
                values: new object[] { new DateTime(2026, 3, 5, 7, 25, 37, 525, DateTimeKind.Utc).AddTicks(5943), new DateTime(2026, 3, 5, 7, 25, 37, 525, DateTimeKind.Utc).AddTicks(5944) });

            migrationBuilder.UpdateData(
                table: "card_format",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_date", "updated_date" },
                values: new object[] { new DateTime(2026, 3, 5, 7, 25, 37, 532, DateTimeKind.Utc).AddTicks(1263), new DateTime(2026, 3, 5, 7, 25, 37, 532, DateTimeKind.Utc).AddTicks(1265) });

            migrationBuilder.UpdateData(
                table: "location",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_date", "updated_date" },
                values: new object[] { new DateTime(2026, 3, 5, 7, 25, 37, 505, DateTimeKind.Utc).AddTicks(801), new DateTime(2026, 3, 5, 7, 25, 37, 505, DateTimeKind.Utc).AddTicks(804) });

            migrationBuilder.UpdateData(
                table: "location",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "created_date", "updated_date" },
                values: new object[] { new DateTime(2026, 3, 5, 7, 25, 37, 505, DateTimeKind.Utc).AddTicks(807), new DateTime(2026, 3, 5, 7, 25, 37, 505, DateTimeKind.Utc).AddTicks(807) });

            migrationBuilder.UpdateData(
                table: "operator",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_date", "updated_date" },
                values: new object[] { new DateTime(2026, 3, 5, 7, 25, 37, 540, DateTimeKind.Utc).AddTicks(1707), new DateTime(2026, 3, 5, 7, 25, 37, 540, DateTimeKind.Utc).AddTicks(1710) });

            migrationBuilder.UpdateData(
                table: "role",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_date", "updated_date" },
                values: new object[] { new DateTime(2026, 3, 5, 7, 25, 37, 538, DateTimeKind.Utc).AddTicks(7183), new DateTime(2026, 3, 5, 7, 25, 37, 538, DateTimeKind.Utc).AddTicks(7186) });

            migrationBuilder.UpdateData(
                table: "timezone",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_date", "updated_date" },
                values: new object[] { new DateTime(2026, 3, 5, 7, 25, 37, 526, DateTimeKind.Utc).AddTicks(6048), new DateTime(2026, 3, 5, 7, 25, 37, 526, DateTimeKind.Utc).AddTicks(6050) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "user_id",
                table: "refresh_token",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "user_id",
                table: "operator",
                type: "text",
                nullable: false,
                defaultValue: "");

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
                columns: new[] { "created_date", "updated_date", "user_id" },
                values: new object[] { new DateTime(2026, 3, 5, 7, 10, 3, 489, DateTimeKind.Utc).AddTicks(9384), new DateTime(2026, 3, 5, 7, 10, 3, 489, DateTimeKind.Utc).AddTicks(9387), "Administrator" });

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
    }
}
