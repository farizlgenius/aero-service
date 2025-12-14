using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HIDAeroService.Migrations
{
    /// <inheritdoc />
    public partial class _14122025237 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TriggerCommands",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<short>(type: "smallint", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TriggerCommands", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "TriggerCommands",
                columns: new[] { "Id", "Description", "Name", "Value" },
                values: new object[,]
                {
                    { 1, "Abort a delayed procedure", "Abort a delayed procedure", (short)1 },
                    { 2, "Execute actions with prefix 0", "Execute actions with prefix 0", (short)2 },
                    { 3, "Resume a delayed procedure and execute actions with prefix 0", "Resume a delayed procedure and execute actions with prefix 0", (short)3 },
                    { 4, "Execute actions with prefix 256", "Execute actions with prefix 256", (short)4 },
                    { 5, "Execute actions with prefix 512", "Execute actions with prefix 512", (short)5 },
                    { 6, "Execute actions with prefix 1024", "Execute actions with prefix 1024", (short)6 },
                    { 7, "Resume a delayed procedure and execute actions with prefix 256", "Resume a delayed procedure and execute actions with prefix 256", (short)7 },
                    { 8, "Resume a delayed procedure and execute actions with prefix 512", "Resume a delayed procedure and execute actions with prefix 512", (short)8 },
                    { 9, "Resume a delayed procedure and execute actions with prefix 1024", "Resume a delayed procedure and execute actions with prefix 1024", (short)9 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TriggerCommands");
        }
    }
}
