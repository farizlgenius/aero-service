using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HIDAeroService.Migrations
{
    /// <inheritdoc />
    public partial class _141220253 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TimeZoneCommands",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeZoneCommands", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "TimeZoneCommands",
                columns: new[] { "Id", "Description", "Name", "Value" },
                values: new object[,]
                {
                    { 1, "Temporary Clear - Deactivate Time Zone until it would normally change. Next interval change will clear the override.", "Temporary Clear", (short)1 },
                    { 2, "Temporary Set - Activate Time Zone until it would normally change. Next interval change will clear the override.", "Temporary Set", (short)2 },
                    { 3, "Override Clear - Deactivate Time Zone until next command 314", "Override Clear", (short)3 },
                    { 4, "Override Set - Activate Time Zone until next command 314", "Override Set", (short)4 },
                    { 5, "Release Time Zone (Return to Normal). Will take the time zone out of the temporary or override mode and put it in the proper state.", "Release", (short)5 },
                    { 6, "Refresh - causes time zone state to be logged to the transaction log. Commonly used for triggers.", "Refresh", (short)6 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TimeZoneCommands");
        }
    }
}
