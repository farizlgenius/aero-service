using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace HIDAeroService.Migrations
{
    /// <inheritdoc />
    public partial class _14122025928 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CodeMap",
                table: "Triggers");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Triggers_ComponentId",
                table: "Triggers",
                column: "ComponentId");

            migrationBuilder.CreateTable(
                name: "TriggerTranCodes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Value = table.Column<short>(type: "smallint", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    TriggerId = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TriggerTranCodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TriggerTranCodes_Triggers_Value",
                        column: x => x.Value,
                        principalTable: "Triggers",
                        principalColumn: "ComponentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TriggerTranCodes_Value",
                table: "TriggerTranCodes",
                column: "Value");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TriggerTranCodes");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Triggers_ComponentId",
                table: "Triggers");

            migrationBuilder.AddColumn<short>(
                name: "CodeMap",
                table: "Triggers",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);
        }
    }
}
