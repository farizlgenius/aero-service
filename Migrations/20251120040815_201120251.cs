using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HIDAeroService.Migrations
{
    /// <inheritdoc />
    public partial class _201120251 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Operators_Roles_RoleId",
                table: "Operators");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Operators");

            migrationBuilder.DropColumn(
                name: "MacAddress",
                table: "Operators");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Operators",
                newName: "ImagePath");

            migrationBuilder.AddForeignKey(
                name: "FK_Operators_Roles_RoleId",
                table: "Operators",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "ComponentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Operators_Roles_RoleId",
                table: "Operators");

            migrationBuilder.RenameColumn(
                name: "ImagePath",
                table: "Operators",
                newName: "UserId");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Operators",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MacAddress",
                table: "Operators",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Operators_Roles_RoleId",
                table: "Operators",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "ComponentId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
