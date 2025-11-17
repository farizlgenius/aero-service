using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HIDAeroService.Migrations
{
    /// <inheritdoc />
    public partial class _171120253 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MultiOccupancyOptions",
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
                    table.PrimaryKey("PK_MultiOccupancyOptions", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "MultiOccupancyOptions",
                columns: new[] { "Id", "Description", "Name", "Value" },
                values: new object[,]
                {
                    { 1, "Two or more not required in area", "Two or more not required in area", (short)0 },
                    { 2, "Two or more required", "Two or more required", (short)1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MultiOccupancyOptions");
        }
    }
}
