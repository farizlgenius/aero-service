using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Aero.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialDatabase2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<short>(
                name: "location_id",
                table: "commnad_audit",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.CreateIndex(
                name: "IX_commnad_audit_location_id",
                table: "commnad_audit",
                column: "location_id");

            migrationBuilder.AddForeignKey(
                name: "FK_commnad_audit_location_location_id",
                table: "commnad_audit",
                column: "location_id",
                principalTable: "location",
                principalColumn: "component_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_commnad_audit_location_location_id",
                table: "commnad_audit");

            migrationBuilder.DropIndex(
                name: "IX_commnad_audit_location_id",
                table: "commnad_audit");

            migrationBuilder.DropColumn(
                name: "location_id",
                table: "commnad_audit");
        }
    }
}
