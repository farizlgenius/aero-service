using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HIDAeroService.Migrations
{
    /// <inheritdoc />
    public partial class _131220251 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ActionTypes",
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
                    table.PrimaryKey("PK_ActionTypes", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "ActionTypes",
                columns: new[] { "Id", "Description", "Name", "Value" },
                values: new object[,]
                {
                    { 1, "Deletes all configured actions", "Delete all actions", (short)0 },
                    { 2, "Command 306: Monitor Point Mask", "Monitor Point Mask", (short)1 },
                    { 3, "Command 307: Control Point Command", "Control Point Command", (short)2 },
                    { 4, "Command 308: ACR Mode", "ACR Mode", (short)3 },
                    { 5, "Command 309: Forced Open Mask", "Forced Open Mask", (short)4 },
                    { 6, "Command 310: Held Open Mask Control", "Held Open Mask", (short)5 },
                    { 7, "Command 311: Momentary Unlock", "Momentary Unlock", (short)6 },
                    { 8, "Command 312: Procedure Control Command", "Procedure Control Command", (short)7 },
                    { 9, "Command 313: Trigger Variable Control Command", "Trigger Variable Control", (short)8 },
                    { 10, "Command 314: Time Zone Control", "Time Zone Control", (short)9 },
                    { 11, "Command 315: Reader LED Mode Control", "Reader LED Mode Control", (short)10 },
                    { 12, "Command 3319: Anti-passback Control (free pass only)", "Anti-passback Control", (short)11 },
                    { 13, "Command 321: Monitor Point Group Arm/Disarm", "Monitor Point Group Arm/Disarm", (short)14 },
                    { 14, "Set action_type prefix based on mask_count", "Set Action Type by Mask Count", (short)15 },
                    { 15, "Set action_type prefix based on active points", "Set Action Type by Active Points", (short)16 },
                    { 16, "Command 322: Modify Access Area Configuration", "Modify Access Area Configuration", (short)17 },
                    { 17, "Abort pending access request (turnstile mode)", "Abort Wait For Door Open", (short)18 },
                    { 18, "Command 325: Temporary Reader LED Control", "Temporary Reader LED Control", (short)19 },
                    { 19, "Command 326: Text Output to LCD Terminal", "LCD Text Output", (short)20 },
                    { 20, "Command 334: Temporary ACR Mode", "Temporary ACR Mode", (short)24 },
                    { 21, "Command 331: Host Simulated Card Read", "Host Simulated Card Read", (short)25 },
                    { 22, "Command 3323: Set Cardholder Use Limit (all only)", "Set Cardholder Use Limit", (short)26 },
                    { 23, "Command 335: Set Operating Mode", "Set Operating Mode", (short)27 },
                    { 24, "Command 339: Host Simulated Key Press", "Host Simulated Key Press", (short)28 },
                    { 25, "Filter transaction in calling trigger", "Filter Trigger Transaction", (short)29 },
                    { 26, "Command 1820: Trigger Activation Summary", "Trigger Activation Summary", (short)30 },
                    { 27, "Delay in 0.1 second increments", "Delay (0.1 Second)", (short)126 },
                    { 28, "Delay in seconds", "Delay (Seconds)", (short)127 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActionTypes");
        }
    }
}
