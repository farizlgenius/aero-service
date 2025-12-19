using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HIDAeroService.Migrations
{
    /// <inheritdoc />
    public partial class _191220252 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PasswordRules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Len = table.Column<int>(type: "integer", nullable: false),
                    IsLower = table.Column<bool>(type: "boolean", nullable: false),
                    IsUpper = table.Column<bool>(type: "boolean", nullable: false),
                    IsDigit = table.Column<bool>(type: "boolean", nullable: false),
                    IsSymbol = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PasswordRules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WeakPasswords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Pattern = table.Column<string>(type: "text", nullable: false),
                    PasswordRuleId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeakPasswords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WeakPasswords_PasswordRules_PasswordRuleId",
                        column: x => x.PasswordRuleId,
                        principalTable: "PasswordRules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "PasswordRules",
                columns: new[] { "Id", "IsDigit", "IsLower", "IsSymbol", "IsUpper", "Len" },
                values: new object[] { 1, false, false, false, false, 4 });

            migrationBuilder.InsertData(
                table: "WeakPasswords",
                columns: new[] { "Id", "PasswordRuleId", "Pattern" },
                values: new object[,]
                {
                    { 1, 1, "P@ssw0rd" },
                    { 2, 1, "password" },
                    { 3, 1, "admin" },
                    { 4, 1, "123456" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_WeakPasswords_PasswordRuleId",
                table: "WeakPasswords",
                column: "PasswordRuleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WeakPasswords");

            migrationBuilder.DropTable(
                name: "PasswordRules");
        }
    }
}
