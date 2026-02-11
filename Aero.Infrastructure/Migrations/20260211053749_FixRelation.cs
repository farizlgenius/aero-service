using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Aero.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_cardholder_access_level_AccessLevelid",
                table: "cardholder");

            migrationBuilder.DropForeignKey(
                name: "FK_cardholder_access_level_access_level_accessLevelid",
                table: "cardholder_access_level");

            migrationBuilder.DropIndex(
                name: "IX_cardholder_access_level_accessLevelid",
                table: "cardholder_access_level");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_cardholder_component_id",
                table: "cardholder");

            migrationBuilder.DropIndex(
                name: "IX_cardholder_AccessLevelid",
                table: "cardholder");

            migrationBuilder.DropColumn(
                name: "accessLevelid",
                table: "cardholder_access_level");

            migrationBuilder.DropColumn(
                name: "AccessLevelid",
                table: "cardholder");

            migrationBuilder.AddForeignKey(
                name: "FK_cardholder_access_level_access_level_accesslevel_id",
                table: "cardholder_access_level",
                column: "accesslevel_id",
                principalTable: "access_level",
                principalColumn: "component_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_cardholder_access_level_access_level_accesslevel_id",
                table: "cardholder_access_level");

            migrationBuilder.AddColumn<int>(
                name: "accessLevelid",
                table: "cardholder_access_level",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AccessLevelid",
                table: "cardholder",
                type: "integer",
                nullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_cardholder_component_id",
                table: "cardholder",
                column: "component_id");

            migrationBuilder.CreateIndex(
                name: "IX_cardholder_access_level_accessLevelid",
                table: "cardholder_access_level",
                column: "accessLevelid");

            migrationBuilder.CreateIndex(
                name: "IX_cardholder_AccessLevelid",
                table: "cardholder",
                column: "AccessLevelid");

            migrationBuilder.AddForeignKey(
                name: "FK_cardholder_access_level_AccessLevelid",
                table: "cardholder",
                column: "AccessLevelid",
                principalTable: "access_level",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_cardholder_access_level_access_level_accessLevelid",
                table: "cardholder_access_level",
                column: "accessLevelid",
                principalTable: "access_level",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
