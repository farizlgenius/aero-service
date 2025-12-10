using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HIDAeroService.Migrations
{
    /// <inheritdoc />
    public partial class _101220251 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CardHolderAccessLevel_AccessLevels_AccessLevelId",
                table: "CardHolderAccessLevel");

            migrationBuilder.DropForeignKey(
                name: "FK_CardHolderAccessLevel_CardHolders_CardHolderId",
                table: "CardHolderAccessLevel");

            migrationBuilder.DropForeignKey(
                name: "FK_CardHolderAdditional_CardHolders_CardHolderId",
                table: "CardHolderAdditional");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CardHolderAdditional",
                table: "CardHolderAdditional");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CardHolderAccessLevel",
                table: "CardHolderAccessLevel");

            migrationBuilder.RenameTable(
                name: "CardHolderAdditional",
                newName: "CardHolderAdditionals");

            migrationBuilder.RenameTable(
                name: "CardHolderAccessLevel",
                newName: "CardHolderAccessLevels");

            migrationBuilder.RenameIndex(
                name: "IX_CardHolderAdditional_CardHolderId",
                table: "CardHolderAdditionals",
                newName: "IX_CardHolderAdditionals_CardHolderId");

            migrationBuilder.RenameIndex(
                name: "IX_CardHolderAccessLevel_CardHolderId",
                table: "CardHolderAccessLevels",
                newName: "IX_CardHolderAccessLevels_CardHolderId");

            migrationBuilder.AddColumn<short>(
                name: "ModelNo",
                table: "Modules",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CardHolderAdditionals",
                table: "CardHolderAdditionals",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CardHolderAccessLevels",
                table: "CardHolderAccessLevels",
                columns: new[] { "AccessLevelId", "CardHolderId" });

            migrationBuilder.AddForeignKey(
                name: "FK_CardHolderAccessLevels_AccessLevels_AccessLevelId",
                table: "CardHolderAccessLevels",
                column: "AccessLevelId",
                principalTable: "AccessLevels",
                principalColumn: "ComponentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CardHolderAccessLevels_CardHolders_CardHolderId",
                table: "CardHolderAccessLevels",
                column: "CardHolderId",
                principalTable: "CardHolders",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CardHolderAdditionals_CardHolders_CardHolderId",
                table: "CardHolderAdditionals",
                column: "CardHolderId",
                principalTable: "CardHolders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CardHolderAccessLevels_AccessLevels_AccessLevelId",
                table: "CardHolderAccessLevels");

            migrationBuilder.DropForeignKey(
                name: "FK_CardHolderAccessLevels_CardHolders_CardHolderId",
                table: "CardHolderAccessLevels");

            migrationBuilder.DropForeignKey(
                name: "FK_CardHolderAdditionals_CardHolders_CardHolderId",
                table: "CardHolderAdditionals");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CardHolderAdditionals",
                table: "CardHolderAdditionals");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CardHolderAccessLevels",
                table: "CardHolderAccessLevels");

            migrationBuilder.DropColumn(
                name: "ModelNo",
                table: "Modules");

            migrationBuilder.RenameTable(
                name: "CardHolderAdditionals",
                newName: "CardHolderAdditional");

            migrationBuilder.RenameTable(
                name: "CardHolderAccessLevels",
                newName: "CardHolderAccessLevel");

            migrationBuilder.RenameIndex(
                name: "IX_CardHolderAdditionals_CardHolderId",
                table: "CardHolderAdditional",
                newName: "IX_CardHolderAdditional_CardHolderId");

            migrationBuilder.RenameIndex(
                name: "IX_CardHolderAccessLevels_CardHolderId",
                table: "CardHolderAccessLevel",
                newName: "IX_CardHolderAccessLevel_CardHolderId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CardHolderAdditional",
                table: "CardHolderAdditional",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CardHolderAccessLevel",
                table: "CardHolderAccessLevel",
                columns: new[] { "AccessLevelId", "CardHolderId" });

            migrationBuilder.AddForeignKey(
                name: "FK_CardHolderAccessLevel_AccessLevels_AccessLevelId",
                table: "CardHolderAccessLevel",
                column: "AccessLevelId",
                principalTable: "AccessLevels",
                principalColumn: "ComponentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CardHolderAccessLevel_CardHolders_CardHolderId",
                table: "CardHolderAccessLevel",
                column: "CardHolderId",
                principalTable: "CardHolders",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CardHolderAdditional_CardHolders_CardHolderId",
                table: "CardHolderAdditional",
                column: "CardHolderId",
                principalTable: "CardHolders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
