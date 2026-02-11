using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Aero.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "hardware_access_level");

            migrationBuilder.AddColumn<short>(
                name: "door_id",
                table: "access_level_door_component",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.CreateIndex(
                name: "IX_access_level_door_component_door_id",
                table: "access_level_door_component",
                column: "door_id");

            migrationBuilder.CreateIndex(
                name: "IX_access_level_door_component_timezone_id",
                table: "access_level_door_component",
                column: "timezone_id");

            migrationBuilder.CreateIndex(
                name: "IX_access_level_component_mac",
                table: "access_level_component",
                column: "mac");

            migrationBuilder.AddForeignKey(
                name: "FK_access_level_component_hardware_mac",
                table: "access_level_component",
                column: "mac",
                principalTable: "hardware",
                principalColumn: "mac",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_access_level_door_component_door_door_id",
                table: "access_level_door_component",
                column: "door_id",
                principalTable: "door",
                principalColumn: "component_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_access_level_door_component_timezone_timezone_id",
                table: "access_level_door_component",
                column: "timezone_id",
                principalTable: "timezone",
                principalColumn: "component_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_access_level_component_hardware_mac",
                table: "access_level_component");

            migrationBuilder.DropForeignKey(
                name: "FK_access_level_door_component_door_door_id",
                table: "access_level_door_component");

            migrationBuilder.DropForeignKey(
                name: "FK_access_level_door_component_timezone_timezone_id",
                table: "access_level_door_component");

            migrationBuilder.DropIndex(
                name: "IX_access_level_door_component_door_id",
                table: "access_level_door_component");

            migrationBuilder.DropIndex(
                name: "IX_access_level_door_component_timezone_id",
                table: "access_level_door_component");

            migrationBuilder.DropIndex(
                name: "IX_access_level_component_mac",
                table: "access_level_component");

            migrationBuilder.DropColumn(
                name: "door_id",
                table: "access_level_door_component");

            migrationBuilder.CreateTable(
                name: "hardware_access_level",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    access_levelid = table.Column<int>(type: "integer", nullable: false),
                    hardware_mac = table.Column<string>(type: "text", nullable: false),
                    hardware_accesslevel_id = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_hardware_access_level", x => x.id);
                    table.ForeignKey(
                        name: "FK_hardware_access_level_access_level_access_levelid",
                        column: x => x.access_levelid,
                        principalTable: "access_level",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_hardware_access_level_hardware_hardware_mac",
                        column: x => x.hardware_mac,
                        principalTable: "hardware",
                        principalColumn: "mac",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_hardware_access_level_access_levelid",
                table: "hardware_access_level",
                column: "access_levelid");

            migrationBuilder.CreateIndex(
                name: "IX_hardware_access_level_hardware_mac",
                table: "hardware_access_level",
                column: "hardware_mac");
        }
    }
}
