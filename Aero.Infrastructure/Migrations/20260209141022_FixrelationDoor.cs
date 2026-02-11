using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Aero.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixrelationDoor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_door_sensor_sensor_id",
                table: "door");

            migrationBuilder.DropForeignKey(
                name: "FK_door_strike_strike_id",
                table: "door");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_strike_component_id",
                table: "strike");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_sensor_component_id",
                table: "sensor");

            migrationBuilder.DropIndex(
                name: "IX_door_sensor_id",
                table: "door");

            migrationBuilder.DropIndex(
                name: "IX_door_strike_id",
                table: "door");

            migrationBuilder.DropColumn(
                name: "sensor_id",
                table: "door");

            migrationBuilder.DropColumn(
                name: "strike_id",
                table: "door");

            migrationBuilder.AddColumn<short>(
                name: "door_id",
                table: "strike",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "door_id",
                table: "sensor",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "sio_id",
                table: "module",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.CreateIndex(
                name: "IX_strike_door_id",
                table: "strike",
                column: "door_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_sensor_door_id",
                table: "sensor",
                column: "door_id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_sensor_door_door_id",
                table: "sensor",
                column: "door_id",
                principalTable: "door",
                principalColumn: "component_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_strike_door_door_id",
                table: "strike",
                column: "door_id",
                principalTable: "door",
                principalColumn: "component_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_sensor_door_door_id",
                table: "sensor");

            migrationBuilder.DropForeignKey(
                name: "FK_strike_door_door_id",
                table: "strike");

            migrationBuilder.DropIndex(
                name: "IX_strike_door_id",
                table: "strike");

            migrationBuilder.DropIndex(
                name: "IX_sensor_door_id",
                table: "sensor");

            migrationBuilder.DropColumn(
                name: "door_id",
                table: "strike");

            migrationBuilder.DropColumn(
                name: "door_id",
                table: "sensor");

            migrationBuilder.DropColumn(
                name: "sio_id",
                table: "module");

            migrationBuilder.AddColumn<short>(
                name: "sensor_id",
                table: "door",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "strike_id",
                table: "door",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_strike_component_id",
                table: "strike",
                column: "component_id");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_sensor_component_id",
                table: "sensor",
                column: "component_id");

            migrationBuilder.CreateIndex(
                name: "IX_door_sensor_id",
                table: "door",
                column: "sensor_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_door_strike_id",
                table: "door",
                column: "strike_id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_door_sensor_sensor_id",
                table: "door",
                column: "sensor_id",
                principalTable: "sensor",
                principalColumn: "component_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_door_strike_strike_id",
                table: "door",
                column: "strike_id",
                principalTable: "strike",
                principalColumn: "component_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
