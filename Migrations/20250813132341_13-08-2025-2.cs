using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HIDAeroService.Migrations
{
    /// <inheritdoc />
    public partial class _130820252 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "scp_id",
                table: "ar_alvl_no");

            migrationBuilder.DropColumn(
                name: "scp_ip",
                table: "ar_alvl_no");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<short>(
                name: "scp_id",
                table: "ar_alvl_no",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<string>(
                name: "scp_ip",
                table: "ar_alvl_no",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
