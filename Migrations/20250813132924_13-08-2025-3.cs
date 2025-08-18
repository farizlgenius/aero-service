using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HIDAeroService.Migrations
{
    /// <inheritdoc />
    public partial class _130820253 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ar_alvl_no",
                columns: new[] { "id", "alvl_number", "is_available" },
                values: new object[,]
                {
                    { 1, (short)0, false },
                    { 2, (short)1, false }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ar_alvl_no",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ar_alvl_no",
                keyColumn: "id",
                keyValue: 2);
        }
    }
}
