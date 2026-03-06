using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Aero.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeRelationFeatureRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_feature_role",
                table: "feature_role");

            migrationBuilder.DeleteData(
                table: "feature_role",
                keyColumns: new[] { "feature_id", "role_id" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "feature_role",
                keyColumns: new[] { "feature_id", "role_id" },
                keyValues: new object[] { 2, 1 });

            migrationBuilder.DeleteData(
                table: "feature_role",
                keyColumns: new[] { "feature_id", "role_id" },
                keyValues: new object[] { 3, 1 });

            migrationBuilder.DeleteData(
                table: "feature_role",
                keyColumns: new[] { "feature_id", "role_id" },
                keyValues: new object[] { 4, 1 });

            migrationBuilder.DeleteData(
                table: "feature_role",
                keyColumns: new[] { "feature_id", "role_id" },
                keyValues: new object[] { 5, 1 });

            migrationBuilder.DeleteData(
                table: "feature_role",
                keyColumns: new[] { "feature_id", "role_id" },
                keyValues: new object[] { 6, 1 });

            migrationBuilder.DeleteData(
                table: "feature_role",
                keyColumns: new[] { "feature_id", "role_id" },
                keyValues: new object[] { 7, 1 });

            migrationBuilder.DeleteData(
                table: "feature_role",
                keyColumns: new[] { "feature_id", "role_id" },
                keyValues: new object[] { 8, 1 });

            migrationBuilder.DeleteData(
                table: "feature_role",
                keyColumns: new[] { "feature_id", "role_id" },
                keyValues: new object[] { 9, 1 });

            migrationBuilder.DeleteData(
                table: "feature_role",
                keyColumns: new[] { "feature_id", "role_id" },
                keyValues: new object[] { 10, 1 });

            migrationBuilder.DeleteData(
                table: "feature_role",
                keyColumns: new[] { "feature_id", "role_id" },
                keyValues: new object[] { 11, 1 });

            migrationBuilder.DeleteData(
                table: "feature_role",
                keyColumns: new[] { "feature_id", "role_id" },
                keyValues: new object[] { 12, 1 });

            migrationBuilder.DeleteData(
                table: "feature_role",
                keyColumns: new[] { "feature_id", "role_id" },
                keyValues: new object[] { 13, 1 });

            migrationBuilder.DeleteData(
                table: "feature_role",
                keyColumns: new[] { "feature_id", "role_id" },
                keyValues: new object[] { 14, 1 });

            migrationBuilder.DeleteData(
                table: "feature_role",
                keyColumns: new[] { "feature_id", "role_id" },
                keyValues: new object[] { 15, 1 });

            migrationBuilder.DeleteData(
                table: "feature_role",
                keyColumns: new[] { "feature_id", "role_id" },
                keyValues: new object[] { 16, 1 });

            migrationBuilder.DeleteData(
                table: "feature_role",
                keyColumns: new[] { "feature_id", "role_id" },
                keyValues: new object[] { 17, 1 });

            migrationBuilder.DeleteData(
                table: "feature_role",
                keyColumns: new[] { "feature_id", "role_id" },
                keyValues: new object[] { 18, 1 });

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "feature_role",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_feature_role",
                table: "feature_role",
                column: "id");

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

            migrationBuilder.InsertData(
                table: "feature_role",
                columns: new[] { "id", "feature_id", "is_action", "is_allow", "is_create", "is_delete", "is_modify", "role_id" },
                values: new object[,]
                {
                    { 1, 1, true, true, true, true, true, 1 },
                    { 2, 2, true, true, true, true, true, 1 },
                    { 3, 3, true, true, true, true, true, 1 },
                    { 4, 4, true, true, true, true, true, 1 },
                    { 5, 5, true, true, true, true, true, 1 },
                    { 6, 6, true, true, true, true, true, 1 },
                    { 7, 7, true, true, true, true, true, 1 },
                    { 8, 8, true, true, true, true, true, 1 },
                    { 9, 9, true, true, true, true, true, 1 },
                    { 10, 10, true, true, true, true, true, 1 },
                    { 11, 11, true, true, true, true, true, 1 },
                    { 12, 12, true, true, true, true, true, 1 },
                    { 13, 13, true, true, true, true, true, 1 },
                    { 14, 14, true, true, true, true, true, 1 },
                    { 15, 15, true, true, true, true, true, 1 },
                    { 16, 16, true, true, true, true, true, 1 },
                    { 17, 17, true, true, true, true, true, 1 },
                    { 18, 18, true, true, true, true, true, 1 }
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_feature_role_role_id",
                table: "feature_role",
                column: "role_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_feature_role",
                table: "feature_role");

            migrationBuilder.DropIndex(
                name: "IX_feature_role_role_id",
                table: "feature_role");

            migrationBuilder.DeleteData(
                table: "feature_role",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "feature_role",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "feature_role",
                keyColumn: "id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "feature_role",
                keyColumn: "id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "feature_role",
                keyColumn: "id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "feature_role",
                keyColumn: "id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "feature_role",
                keyColumn: "id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "feature_role",
                keyColumn: "id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "feature_role",
                keyColumn: "id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "feature_role",
                keyColumn: "id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "feature_role",
                keyColumn: "id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "feature_role",
                keyColumn: "id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "feature_role",
                keyColumn: "id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "feature_role",
                keyColumn: "id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "feature_role",
                keyColumn: "id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "feature_role",
                keyColumn: "id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "feature_role",
                keyColumn: "id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "feature_role",
                keyColumn: "id",
                keyValue: 18);

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "feature_role",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_feature_role",
                table: "feature_role",
                columns: new[] { "role_id", "feature_id" });

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

            migrationBuilder.InsertData(
                table: "feature_role",
                columns: new[] { "feature_id", "role_id", "id", "is_action", "is_allow", "is_create", "is_delete", "is_modify" },
                values: new object[,]
                {
                    { 1, 1, 1, true, true, true, true, true },
                    { 2, 1, 2, true, true, true, true, true },
                    { 3, 1, 3, true, true, true, true, true },
                    { 4, 1, 4, true, true, true, true, true },
                    { 5, 1, 5, true, true, true, true, true },
                    { 6, 1, 6, true, true, true, true, true },
                    { 7, 1, 7, true, true, true, true, true },
                    { 8, 1, 8, true, true, true, true, true },
                    { 9, 1, 9, true, true, true, true, true },
                    { 10, 1, 10, true, true, true, true, true },
                    { 11, 1, 11, true, true, true, true, true },
                    { 12, 1, 12, true, true, true, true, true },
                    { 13, 1, 13, true, true, true, true, true },
                    { 14, 1, 14, true, true, true, true, true },
                    { 15, 1, 15, true, true, true, true, true },
                    { 16, 1, 16, true, true, true, true, true },
                    { 17, 1, 17, true, true, true, true, true },
                    { 18, 1, 18, true, true, true, true, true }
                });

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
                table: "timezone",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_date", "updated_date" },
                values: new object[] { new DateTime(2026, 3, 5, 5, 15, 31, 458, DateTimeKind.Utc).AddTicks(3834), new DateTime(2026, 3, 5, 5, 15, 31, 458, DateTimeKind.Utc).AddTicks(3836) });
        }
    }
}
