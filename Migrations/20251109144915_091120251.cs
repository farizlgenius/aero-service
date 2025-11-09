using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HIDAeroService.Migrations
{
    /// <inheritdoc />
    public partial class _091120251 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CardHolders_AccessLevels_AccessLevelId",
                table: "CardHolders");

            migrationBuilder.DropIndex(
                name: "IX_CardHolders_AccessLevelId",
                table: "CardHolders");

            migrationBuilder.DropColumn(
                name: "AccessLevelId",
                table: "CardHolders");

            migrationBuilder.CreateTable(
                name: "CardHolderAccessLevel",
                columns: table => new
                {
                    CardHolderId = table.Column<string>(type: "text", nullable: false),
                    AccessLevelId = table.Column<short>(type: "smallint", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardHolderAccessLevel", x => new { x.AccessLevelId, x.CardHolderId });
                    table.ForeignKey(
                        name: "FK_CardHolderAccessLevel_AccessLevels_AccessLevelId",
                        column: x => x.AccessLevelId,
                        principalTable: "AccessLevels",
                        principalColumn: "ComponentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CardHolderAccessLevel_CardHolders_CardHolderId",
                        column: x => x.CardHolderId,
                        principalTable: "CardHolders",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CardHolderAccessLevel_CardHolderId",
                table: "CardHolderAccessLevel",
                column: "CardHolderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CardHolderAccessLevel");

            migrationBuilder.AddColumn<short>(
                name: "AccessLevelId",
                table: "CardHolders",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.CreateIndex(
                name: "IX_CardHolders_AccessLevelId",
                table: "CardHolders",
                column: "AccessLevelId");

            migrationBuilder.AddForeignKey(
                name: "FK_CardHolders_AccessLevels_AccessLevelId",
                table: "CardHolders",
                column: "AccessLevelId",
                principalTable: "AccessLevels",
                principalColumn: "ComponentId");
        }
    }
}
