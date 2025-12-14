using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HIDAeroService.Migrations
{
    /// <inheritdoc />
    public partial class _141220251021 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Doors_AccessAreas_AntiPassBackIn",
                table: "Doors");

            migrationBuilder.DropForeignKey(
                name: "FK_Doors_AccessAreas_AntiPassBackOut",
                table: "Doors");

            migrationBuilder.AlterColumn<short>(
                name: "AntiPassBackOut",
                table: "Doors",
                type: "smallint",
                nullable: true,
                oldClrType: typeof(short),
                oldType: "smallint");

            migrationBuilder.AlterColumn<short>(
                name: "AntiPassBackIn",
                table: "Doors",
                type: "smallint",
                nullable: true,
                oldClrType: typeof(short),
                oldType: "smallint");

            migrationBuilder.AddForeignKey(
                name: "FK_Doors_AccessAreas_AntiPassBackIn",
                table: "Doors",
                column: "AntiPassBackIn",
                principalTable: "AccessAreas",
                principalColumn: "ComponentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Doors_AccessAreas_AntiPassBackOut",
                table: "Doors",
                column: "AntiPassBackOut",
                principalTable: "AccessAreas",
                principalColumn: "ComponentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Doors_AccessAreas_AntiPassBackIn",
                table: "Doors");

            migrationBuilder.DropForeignKey(
                name: "FK_Doors_AccessAreas_AntiPassBackOut",
                table: "Doors");

            migrationBuilder.AlterColumn<short>(
                name: "AntiPassBackOut",
                table: "Doors",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0,
                oldClrType: typeof(short),
                oldType: "smallint",
                oldNullable: true);

            migrationBuilder.AlterColumn<short>(
                name: "AntiPassBackIn",
                table: "Doors",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0,
                oldClrType: typeof(short),
                oldType: "smallint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Doors_AccessAreas_AntiPassBackIn",
                table: "Doors",
                column: "AntiPassBackIn",
                principalTable: "AccessAreas",
                principalColumn: "ComponentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Doors_AccessAreas_AntiPassBackOut",
                table: "Doors",
                column: "AntiPassBackOut",
                principalTable: "AccessAreas",
                principalColumn: "ComponentId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
