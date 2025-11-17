using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HIDAeroService.Migrations
{
    /// <inheritdoc />
    public partial class _171120252 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_DoorSpareFlags",
                table: "DoorSpareFlags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CredentialFlags",
                table: "CredentialFlags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AccessAreaCommands",
                table: "AccessAreaCommands");

            migrationBuilder.RenameTable(
                name: "DoorSpareFlags",
                newName: "DoorSpareFlagOption");

            migrationBuilder.RenameTable(
                name: "CredentialFlags",
                newName: "CredentialFlagOptions");

            migrationBuilder.RenameTable(
                name: "AccessAreaCommands",
                newName: "AccessAreaCommandOptions");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DoorSpareFlagOption",
                table: "DoorSpareFlagOption",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CredentialFlagOptions",
                table: "CredentialFlagOptions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AccessAreaCommandOptions",
                table: "AccessAreaCommandOptions",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "AccessAreaAccessControlOptions",
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
                    table.PrimaryKey("PK_AccessAreaAccessControlOptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AreaFlagOptions",
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
                    table.PrimaryKey("PK_AreaFlagOptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OccupancyControlOptions",
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
                    table.PrimaryKey("PK_OccupancyControlOptions", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "AccessAreaAccessControlOptions",
                columns: new[] { "Id", "Description", "Name", "Value" },
                values: new object[,]
                {
                    { 1, "No Operation", "NOP", (short)0 },
                    { 2, "No One Can Access", "Disable area", (short)1 },
                    { 3, "Enable Area", "Enable area", (short)2 }
                });

            migrationBuilder.InsertData(
                table: "AreaFlagOptions",
                columns: new[] { "Id", "Description", "Name", "Value" },
                values: new object[,]
                {
                    { 1, "Area can have open thresholds to only one other area", "Interlock", (short)1 },
                    { 2, "Just (O)ne (D)oor (O)nly is allowed to be open into this area (AREA_F_AIRLOCK must also be set)", "AirLock One Door Only", (short)2 }
                });

            migrationBuilder.InsertData(
                table: "OccupancyControlOptions",
                columns: new[] { "Id", "Description", "Name", "Value" },
                values: new object[,]
                {
                    { 1, "Do not change current occupancy count", "Do not change current occupancy count", (short)0 },
                    { 2, "Change current occupancy to occ_set", "Change current occupancy to occ_set", (short)1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccessAreaAccessControlOptions");

            migrationBuilder.DropTable(
                name: "AreaFlagOptions");

            migrationBuilder.DropTable(
                name: "OccupancyControlOptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DoorSpareFlagOption",
                table: "DoorSpareFlagOption");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CredentialFlagOptions",
                table: "CredentialFlagOptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AccessAreaCommandOptions",
                table: "AccessAreaCommandOptions");

            migrationBuilder.RenameTable(
                name: "DoorSpareFlagOption",
                newName: "DoorSpareFlags");

            migrationBuilder.RenameTable(
                name: "CredentialFlagOptions",
                newName: "CredentialFlags");

            migrationBuilder.RenameTable(
                name: "AccessAreaCommandOptions",
                newName: "AccessAreaCommands");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DoorSpareFlags",
                table: "DoorSpareFlags",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CredentialFlags",
                table: "CredentialFlags",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AccessAreaCommands",
                table: "AccessAreaCommands",
                column: "Id");
        }
    }
}
