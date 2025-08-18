using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace HIDAeroService.Migrations
{
    /// <inheritdoc />
    public partial class _170820251 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ar_scp_structure_statuses",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ip = table.Column<string>(type: "text", nullable: false),
                    mac = table.Column<string>(type: "text", nullable: false),
                    rec_alloc_transaction = table.Column<short>(type: "smallint", nullable: false),
                    rec_alloc_timezone = table.Column<short>(type: "smallint", nullable: false),
                    rec_alloc_holiday = table.Column<short>(type: "smallint", nullable: false),
                    rec_alloc_sio_port = table.Column<short>(type: "smallint", nullable: false),
                    rec_alloc_mp = table.Column<short>(type: "smallint", nullable: false),
                    rec_alloc_cp = table.Column<short>(type: "smallint", nullable: false),
                    rec_alloc_acr = table.Column<short>(type: "smallint", nullable: false),
                    rec_alloc_alvl = table.Column<short>(type: "smallint", nullable: false),
                    rec_alloc_trig = table.Column<short>(type: "smallint", nullable: false),
                    rec_alloc_proc = table.Column<short>(type: "smallint", nullable: false),
                    rec_alloc_mpg = table.Column<short>(type: "smallint", nullable: false),
                    rec_alloc_area = table.Column<short>(type: "smallint", nullable: false),
                    rec_alloc_eal = table.Column<short>(type: "smallint", nullable: false),
                    rec_alloc_crdb = table.Column<short>(type: "smallint", nullable: false),
                    rec_alloc_card_active = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ar_scp_structure_statuses", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ar_scp_structure_statuses");
        }
    }
}
