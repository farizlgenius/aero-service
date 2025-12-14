using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HIDAeroService.Migrations
{
    /// <inheritdoc />
    public partial class _141220252 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ActionTypes",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "ActionTypes",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "ActionTypes",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "ActionTypes",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "ActionTypes",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "ActionTypes",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.InsertData(
                table: "ActionTypes",
                columns: new[] { "Id", "Description", "Name", "Value" },
                values: new object[] { 20, "Command 334: Temporary ACR Mode", "Temporary Door Mode", (short)24 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ActionTypes",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.InsertData(
                table: "ActionTypes",
                columns: new[] { "Id", "Description", "Name", "Value" },
                values: new object[,]
                {
                    { 5, "Command 309: Forced Open Mask", "Forced Open Mask", (short)4 },
                    { 6, "Command 310: Held Open Mask Control", "Held Open Mask", (short)5 },
                    { 14, "Set action_type prefix based on mask_count", "Set Action Type by Mask Count", (short)15 },
                    { 15, "Set action_type prefix based on active points", "Set Action Type by Active Points", (short)16 },
                    { 22, "Command 3323: Set Cardholder Use Limit (all only)", "Set Cardholder Use Limit", (short)26 },
                    { 27, "Delay in 0.1 second increments", "Delay (0.1 Second)", (short)126 }
                });
        }
    }
}
