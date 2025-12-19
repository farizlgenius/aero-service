using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HIDAeroService.Migrations
{
    /// <inheritdoc />
    public partial class _191220251 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OperatorLocation_Locations_LocationId",
                table: "OperatorLocation");

            migrationBuilder.DropForeignKey(
                name: "FK_OperatorLocation_Operators_OperatorId",
                table: "OperatorLocation");

            migrationBuilder.DropForeignKey(
                name: "FK_Operators_Roles_RoleId",
                table: "Operators");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OperatorLocation",
                table: "OperatorLocation");

            migrationBuilder.RenameTable(
                name: "OperatorLocation",
                newName: "OperatorLocations");

            migrationBuilder.RenameIndex(
                name: "IX_OperatorLocation_OperatorId",
                table: "OperatorLocations",
                newName: "IX_OperatorLocations_OperatorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OperatorLocations",
                table: "OperatorLocations",
                columns: new[] { "LocationId", "OperatorId" });

            migrationBuilder.AddForeignKey(
                name: "FK_OperatorLocations_Locations_LocationId",
                table: "OperatorLocations",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "ComponentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OperatorLocations_Operators_OperatorId",
                table: "OperatorLocations",
                column: "OperatorId",
                principalTable: "Operators",
                principalColumn: "ComponentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Operators_Roles_RoleId",
                table: "Operators",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "ComponentId",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OperatorLocations_Locations_LocationId",
                table: "OperatorLocations");

            migrationBuilder.DropForeignKey(
                name: "FK_OperatorLocations_Operators_OperatorId",
                table: "OperatorLocations");

            migrationBuilder.DropForeignKey(
                name: "FK_Operators_Roles_RoleId",
                table: "Operators");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OperatorLocations",
                table: "OperatorLocations");

            migrationBuilder.RenameTable(
                name: "OperatorLocations",
                newName: "OperatorLocation");

            migrationBuilder.RenameIndex(
                name: "IX_OperatorLocations_OperatorId",
                table: "OperatorLocation",
                newName: "IX_OperatorLocation_OperatorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OperatorLocation",
                table: "OperatorLocation",
                columns: new[] { "LocationId", "OperatorId" });

            migrationBuilder.AddForeignKey(
                name: "FK_OperatorLocation_Locations_LocationId",
                table: "OperatorLocation",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "ComponentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OperatorLocation_Operators_OperatorId",
                table: "OperatorLocation",
                column: "OperatorId",
                principalTable: "Operators",
                principalColumn: "ComponentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Operators_Roles_RoleId",
                table: "Operators",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "ComponentId");
        }
    }
}
