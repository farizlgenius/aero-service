using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Aero.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixRelation2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_cardholder_access_level_location_locationid",
                table: "cardholder_access_level");

            migrationBuilder.DropIndex(
                name: "IX_cardholder_access_level_locationid",
                table: "cardholder_access_level");

            migrationBuilder.DropColumn(
                name: "component_id",
                table: "cardholder_access_level");

            migrationBuilder.DropColumn(
                name: "created_date",
                table: "cardholder_access_level");

            migrationBuilder.DropColumn(
                name: "id",
                table: "cardholder_access_level");

            migrationBuilder.DropColumn(
                name: "is_active",
                table: "cardholder_access_level");

            migrationBuilder.DropColumn(
                name: "location_id",
                table: "cardholder_access_level");

            migrationBuilder.DropColumn(
                name: "locationid",
                table: "cardholder_access_level");

            migrationBuilder.DropColumn(
                name: "updated_date",
                table: "cardholder_access_level");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<short>(
                name: "component_id",
                table: "cardholder_access_level",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<DateTime>(
                name: "created_date",
                table: "cardholder_access_level",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "id",
                table: "cardholder_access_level",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "is_active",
                table: "cardholder_access_level",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<short>(
                name: "location_id",
                table: "cardholder_access_level",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<int>(
                name: "locationid",
                table: "cardholder_access_level",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_date",
                table: "cardholder_access_level",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_cardholder_access_level_locationid",
                table: "cardholder_access_level",
                column: "locationid");

            migrationBuilder.AddForeignKey(
                name: "FK_cardholder_access_level_location_locationid",
                table: "cardholder_access_level",
                column: "locationid",
                principalTable: "location",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
