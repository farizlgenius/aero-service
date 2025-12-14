using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HIDAeroService.Migrations
{
    /// <inheritdoc />
    public partial class _141220251 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ActionTypes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ActionTypes",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "ActionTypes",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "ActionTypes",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "ActionTypes",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "ActionTypes",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "ActionTypes",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "ActionTypes",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "ActionTypes",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "ActionTypes",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "ActionTypes",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "ActionTypes",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "ActionTypes",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "ActionTypes",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.UpdateData(
                table: "ActionTypes",
                keyColumn: "Id",
                keyValue: 4,
                column: "Name",
                value: "Door Mode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ActionTypes",
                keyColumn: "Id",
                keyValue: 4,
                column: "Name",
                value: "ACR Mode");

            migrationBuilder.InsertData(
                table: "ActionTypes",
                columns: new[] { "Id", "Description", "Name", "Value" },
                values: new object[,]
                {
                    { 1, "Deletes all configured actions", "Delete all actions", (short)0 },
                    { 8, "Command 312: Procedure Control Command", "Procedure Control Command", (short)7 },
                    { 9, "Command 313: Trigger Variable Control Command", "Trigger Variable Control", (short)8 },
                    { 11, "Command 315: Reader LED Mode Control", "Reader LED Mode Control", (short)10 },
                    { 12, "Command 3319: Anti-passback Control (free pass only)", "Anti-passback Control", (short)11 },
                    { 16, "Command 322: Modify Access Area Configuration", "Modify Access Area Configuration", (short)17 },
                    { 17, "Abort pending access request (turnstile mode)", "Abort Wait For Door Open", (short)18 },
                    { 18, "Command 325: Temporary Reader LED Control", "Temporary Reader LED Control", (short)19 },
                    { 19, "Command 326: Text Output to LCD Terminal", "LCD Text Output", (short)20 },
                    { 20, "Command 334: Temporary ACR Mode", "Temporary ACR Mode", (short)24 },
                    { 23, "Command 335: Set Operating Mode", "Set Operating Mode", (short)27 },
                    { 24, "Command 339: Host Simulated Key Press", "Host Simulated Key Press", (short)28 },
                    { 25, "Filter transaction in calling trigger", "Filter Trigger Transaction", (short)29 },
                    { 26, "Command 1820: Trigger Activation Summary", "Trigger Activation Summary", (short)30 }
                });
        }
    }
}
