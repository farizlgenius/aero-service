using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HIDAeroService.Migrations
{
    /// <inheritdoc />
    public partial class firstinit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ar_access_lvls",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    access_lv_number = table.Column<short>(type: "smallint", nullable: false),
                    tz1 = table.Column<short>(type: "smallint", nullable: false),
                    tz2 = table.Column<short>(type: "smallint", nullable: false),
                    tz3 = table.Column<short>(type: "smallint", nullable: false),
                    tz4 = table.Column<short>(type: "smallint", nullable: false),
                    tz5 = table.Column<short>(type: "smallint", nullable: false),
                    tz6 = table.Column<short>(type: "smallint", nullable: false),
                    tz7 = table.Column<short>(type: "smallint", nullable: false),
                    tz8 = table.Column<short>(type: "smallint", nullable: false),
                    tz9 = table.Column<short>(type: "smallint", nullable: false),
                    tz10 = table.Column<short>(type: "smallint", nullable: false),
                    tz11 = table.Column<short>(type: "smallint", nullable: false),
                    tz12 = table.Column<short>(type: "smallint", nullable: false),
                    tz13 = table.Column<short>(type: "smallint", nullable: false),
                    tz14 = table.Column<short>(type: "smallint", nullable: false),
                    tz15 = table.Column<short>(type: "smallint", nullable: false),
                    tz16 = table.Column<short>(type: "smallint", nullable: false),
                    tz17 = table.Column<short>(type: "smallint", nullable: false),
                    tz18 = table.Column<short>(type: "smallint", nullable: false),
                    tz19 = table.Column<short>(type: "smallint", nullable: false),
                    tz20 = table.Column<short>(type: "smallint", nullable: false),
                    tz21 = table.Column<short>(type: "smallint", nullable: false),
                    tz22 = table.Column<short>(type: "smallint", nullable: false),
                    tz23 = table.Column<short>(type: "smallint", nullable: false),
                    tz24 = table.Column<short>(type: "smallint", nullable: false),
                    tz25 = table.Column<short>(type: "smallint", nullable: false),
                    tz26 = table.Column<short>(type: "smallint", nullable: false),
                    tz27 = table.Column<short>(type: "smallint", nullable: false),
                    tz28 = table.Column<short>(type: "smallint", nullable: false),
                    tz29 = table.Column<short>(type: "smallint", nullable: false),
                    tz30 = table.Column<short>(type: "smallint", nullable: false),
                    tz31 = table.Column<short>(type: "smallint", nullable: false),
                    tz32 = table.Column<short>(type: "smallint", nullable: false),
                    tz33 = table.Column<short>(type: "smallint", nullable: false),
                    tz34 = table.Column<short>(type: "smallint", nullable: false),
                    tz35 = table.Column<short>(type: "smallint", nullable: false),
                    tz36 = table.Column<short>(type: "smallint", nullable: false),
                    tz37 = table.Column<short>(type: "smallint", nullable: false),
                    tz38 = table.Column<short>(type: "smallint", nullable: false),
                    tz39 = table.Column<short>(type: "smallint", nullable: false),
                    tz40 = table.Column<short>(type: "smallint", nullable: false),
                    tz41 = table.Column<short>(type: "smallint", nullable: false),
                    tz42 = table.Column<short>(type: "smallint", nullable: false),
                    tz43 = table.Column<short>(type: "smallint", nullable: false),
                    tz44 = table.Column<short>(type: "smallint", nullable: false),
                    tz45 = table.Column<short>(type: "smallint", nullable: false),
                    tz46 = table.Column<short>(type: "smallint", nullable: false),
                    tz47 = table.Column<short>(type: "smallint", nullable: false),
                    tz48 = table.Column<short>(type: "smallint", nullable: false),
                    tz49 = table.Column<short>(type: "smallint", nullable: false),
                    tz50 = table.Column<short>(type: "smallint", nullable: false),
                    tz51 = table.Column<short>(type: "smallint", nullable: false),
                    tz52 = table.Column<short>(type: "smallint", nullable: false),
                    tz53 = table.Column<short>(type: "smallint", nullable: false),
                    tz54 = table.Column<short>(type: "smallint", nullable: false),
                    tz55 = table.Column<short>(type: "smallint", nullable: false),
                    tz56 = table.Column<short>(type: "smallint", nullable: false),
                    tz57 = table.Column<short>(type: "smallint", nullable: false),
                    tz58 = table.Column<short>(type: "smallint", nullable: false),
                    tz59 = table.Column<short>(type: "smallint", nullable: false),
                    tz60 = table.Column<short>(type: "smallint", nullable: false),
                    tz61 = table.Column<short>(type: "smallint", nullable: false),
                    tz62 = table.Column<short>(type: "smallint", nullable: false),
                    tz63 = table.Column<short>(type: "smallint", nullable: false),
                    tz64 = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ar_access_lvls", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ar_acr_modes",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    value = table.Column<short>(type: "smallint", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ar_acr_modes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ar_acr_no",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    scp_ip = table.Column<string>(type: "text", nullable: false),
                    sio_number = table.Column<short>(type: "smallint", nullable: false),
                    acr_number = table.Column<short>(type: "smallint", nullable: false),
                    is_available = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ar_acr_no", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ar_acrs",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    scp_ip = table.Column<string>(type: "text", nullable: false),
                    acr_number = table.Column<short>(type: "smallint", nullable: false),
                    access_cfg = table.Column<short>(type: "smallint", nullable: false),
                    rdr_sio = table.Column<short>(type: "smallint", nullable: false),
                    reader_number = table.Column<short>(type: "smallint", nullable: false),
                    strk_sio = table.Column<short>(type: "smallint", nullable: false),
                    strk_number = table.Column<short>(type: "smallint", nullable: false),
                    strike_t_min = table.Column<short>(type: "smallint", nullable: false),
                    strike_t_max = table.Column<short>(type: "smallint", nullable: false),
                    strike_mode = table.Column<short>(type: "smallint", nullable: false),
                    sensor_sio = table.Column<short>(type: "smallint", nullable: false),
                    sensor_number = table.Column<short>(type: "smallint", nullable: false),
                    dc_held = table.Column<short>(type: "smallint", nullable: false),
                    rex1_sio = table.Column<short>(type: "smallint", nullable: false),
                    rex1_number = table.Column<short>(type: "smallint", nullable: false),
                    rex2_sio = table.Column<short>(type: "smallint", nullable: false),
                    rex2_number = table.Column<short>(type: "smallint", nullable: false),
                    rex1_tzmask = table.Column<short>(type: "smallint", nullable: false),
                    rex2_tzmask = table.Column<short>(type: "smallint", nullable: false),
                    altrdr_sio = table.Column<short>(type: "smallint", nullable: false),
                    altrdr_number = table.Column<short>(type: "smallint", nullable: false),
                    altrdr_spec = table.Column<short>(type: "smallint", nullable: false),
                    cd_format = table.Column<short>(type: "smallint", nullable: false),
                    apb_mode = table.Column<short>(type: "smallint", nullable: false),
                    offline_mode = table.Column<short>(type: "smallint", nullable: false),
                    default_mode = table.Column<short>(type: "smallint", nullable: false),
                    default_led_mode = table.Column<short>(type: "smallint", nullable: false),
                    pre_alarm = table.Column<short>(type: "smallint", nullable: false),
                    apb_delay = table.Column<short>(type: "smallint", nullable: false),
                    strk_t2 = table.Column<short>(type: "smallint", nullable: false),
                    dc_held2 = table.Column<short>(type: "smallint", nullable: false),
                    strk_follow_pulse = table.Column<short>(type: "smallint", nullable: false),
                    strk_follow_delay = table.Column<short>(type: "smallint", nullable: false),
                    nExtFeatureType = table.Column<short>(type: "smallint", nullable: false),
                    ilPB_sio = table.Column<short>(type: "smallint", nullable: false),
                    ilPB_number = table.Column<short>(type: "smallint", nullable: false),
                    ilPB_long_press = table.Column<short>(type: "smallint", nullable: false),
                    ilPB_out_sio = table.Column<short>(type: "smallint", nullable: false),
                    ilPB_out_num = table.Column<short>(type: "smallint", nullable: false),
                    dfofFilterTime = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ar_acrs", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ar_apb_modes",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    value = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ar_apb_modes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ar_card_formats",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    number = table.Column<short>(type: "smallint", nullable: false),
                    facility = table.Column<short>(type: "smallint", nullable: false),
                    offset = table.Column<short>(type: "smallint", nullable: false),
                    function_id = table.Column<short>(type: "smallint", nullable: false),
                    flags = table.Column<short>(type: "smallint", nullable: false),
                    bits = table.Column<short>(type: "smallint", nullable: false),
                    pe_ln = table.Column<short>(type: "smallint", nullable: false),
                    pe_loc = table.Column<short>(type: "smallint", nullable: false),
                    po_ln = table.Column<short>(type: "smallint", nullable: false),
                    po_loc = table.Column<short>(type: "smallint", nullable: false),
                    fc_ln = table.Column<short>(type: "smallint", nullable: false),
                    fc_loc = table.Column<short>(type: "smallint", nullable: false),
                    ch_ln = table.Column<short>(type: "smallint", nullable: false),
                    ch_loc = table.Column<short>(type: "smallint", nullable: false),
                    ic_ln = table.Column<short>(type: "smallint", nullable: false),
                    ic_loc = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ar_card_formats", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ar_card_holders",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    card_holder_id = table.Column<string>(type: "text", nullable: false),
                    card_holder_refenrence_number = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    sex = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: true),
                    phone = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    holder_status = table.Column<string>(type: "text", nullable: true),
                    issue_code_running_number = table.Column<int>(type: "integer", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ar_card_holders", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ar_control_point",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    scp_ip = table.Column<string>(type: "text", nullable: false),
                    sio_number = table.Column<short>(type: "smallint", nullable: false),
                    cp_number = table.Column<short>(type: "smallint", nullable: false),
                    op_number = table.Column<short>(type: "smallint", nullable: false),
                    mode = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ar_control_point", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ar_cp_no",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    scp_ip = table.Column<string>(type: "text", nullable: false),
                    sio_number = table.Column<short>(type: "smallint", nullable: false),
                    cp_number = table.Column<short>(type: "smallint", nullable: false),
                    is_available = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ar_cp_no", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ar_credentials",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    card_holder_refenrence_number = table.Column<Guid>(type: "uuid", nullable: false),
                    bits = table.Column<int>(type: "integer", nullable: false),
                    issue_code = table.Column<int>(type: "integer", nullable: false),
                    facility_code = table.Column<int>(type: "integer", nullable: false),
                    card_number = table.Column<long>(type: "bigint", nullable: false),
                    pin = table.Column<string>(type: "text", nullable: true),
                    act_time = table.Column<int>(type: "integer", nullable: false),
                    deact_time = table.Column<int>(type: "integer", nullable: false),
                    access_level = table.Column<short>(type: "smallint", nullable: false),
                    image = table.Column<string>(type: "text", nullable: true),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ar_credentials", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ar_events",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    date = table.Column<string>(type: "text", nullable: false),
                    time = table.Column<string>(type: "text", nullable: false),
                    serial_number = table.Column<int>(type: "integer", nullable: false),
                    source = table.Column<string>(type: "text", nullable: false),
                    source_number = table.Column<string>(type: "text", nullable: false),
                    type = table.Column<string>(type: "text", nullable: false),
                    transaction_code = table.Column<double>(type: "double precision", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    additional = table.Column<string>(type: "text", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ar_events", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ar_ip_modes",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    value = table.Column<short>(type: "smallint", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ar_ip_modes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ar_monitor_point",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    scp_ip = table.Column<string>(type: "text", nullable: false),
                    sio_number = table.Column<short>(type: "smallint", nullable: false),
                    mp_number = table.Column<short>(type: "smallint", nullable: false),
                    ip_number = table.Column<short>(type: "smallint", nullable: false),
                    icvt_num = table.Column<short>(type: "smallint", nullable: false),
                    lf_code = table.Column<short>(type: "smallint", nullable: false),
                    delay_entry = table.Column<short>(type: "smallint", nullable: false),
                    delay_exit = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ar_monitor_point", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ar_mp_no",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    scp_ip = table.Column<string>(type: "text", nullable: false),
                    sio_number = table.Column<short>(type: "smallint", nullable: false),
                    mp_number = table.Column<short>(type: "smallint", nullable: false),
                    ip_number = table.Column<short>(type: "smallint", nullable: false),
                    is_available = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ar_mp_no", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ar_naks",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    tag_no = table.Column<int>(type: "integer", nullable: false),
                    nak_reason = table.Column<string>(type: "text", nullable: true),
                    nak_desc_code = table.Column<int>(type: "integer", nullable: false),
                    nak_desc = table.Column<string>(type: "text", nullable: true),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ar_naks", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ar_op_modes",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    value = table.Column<short>(type: "smallint", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ar_op_modes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ar_rdr_modes",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    value = table.Column<short>(type: "smallint", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ar_rdr_modes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ar_readers",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    scp_mac = table.Column<string>(type: "text", nullable: false),
                    sio_number = table.Column<short>(type: "smallint", nullable: false),
                    reader_number = table.Column<short>(type: "smallint", nullable: false),
                    led_drive_mode = table.Column<short>(type: "smallint", nullable: false),
                    osdp_flag = table.Column<short>(type: "smallint", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ar_readers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ar_scps",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    scp_id = table.Column<short>(type: "smallint", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    model = table.Column<string>(type: "text", nullable: false),
                    mac = table.Column<string>(type: "text", nullable: false),
                    ip_address = table.Column<string>(type: "text", nullable: false),
                    serial_number = table.Column<string>(type: "text", nullable: false),
                    n_sio = table.Column<short>(type: "smallint", nullable: false),
                    n_mp = table.Column<short>(type: "smallint", nullable: false),
                    n_cp = table.Column<short>(type: "smallint", nullable: false),
                    n_acr = table.Column<short>(type: "smallint", nullable: false),
                    n_alvl = table.Column<short>(type: "smallint", nullable: false),
                    n_trgr = table.Column<short>(type: "smallint", nullable: false),
                    n_proc = table.Column<short>(type: "smallint", nullable: false),
                    n_tz = table.Column<short>(type: "smallint", nullable: false),
                    n_hol = table.Column<short>(type: "smallint", nullable: false),
                    n_mpg = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ar_scps", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ar_sio_no",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    scp_ip = table.Column<string>(type: "text", nullable: false),
                    sio_number = table.Column<short>(type: "smallint", nullable: false),
                    is_available = table.Column<bool>(type: "boolean", nullable: false),
                    port = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ar_sio_no", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ar_sios",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    scp_name = table.Column<string>(type: "text", nullable: false),
                    scp_ip = table.Column<string>(type: "text", nullable: false),
                    sio_number = table.Column<short>(type: "smallint", nullable: false),
                    n_inputs = table.Column<short>(type: "smallint", nullable: false),
                    n_outputs = table.Column<short>(type: "smallint", nullable: false),
                    n_readers = table.Column<short>(type: "smallint", nullable: false),
                    model = table.Column<short>(type: "smallint", nullable: false),
                    model_desc = table.Column<string>(type: "text", nullable: false),
                    address = table.Column<short>(type: "smallint", nullable: false),
                    msp1_number = table.Column<short>(type: "smallint", nullable: false),
                    port_number = table.Column<short>(type: "smallint", nullable: false),
                    baud_rate = table.Column<short>(type: "smallint", nullable: false),
                    n_protocol = table.Column<short>(type: "smallint", nullable: false),
                    n_dialect = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ar_sios", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ar_strk_modes",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    value = table.Column<short>(type: "smallint", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ar_strk_modes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ar_tz_intervals",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    tz_number = table.Column<short>(type: "smallint", nullable: false),
                    intervals_number = table.Column<short>(type: "smallint", nullable: false),
                    i_days = table.Column<short>(type: "smallint", nullable: false),
                    i_start = table.Column<string>(type: "text", nullable: false),
                    i_end = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ar_tz_intervals", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ar_tz_no",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    tz_number = table.Column<short>(type: "smallint", nullable: false),
                    is_available = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ar_tz_no", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ar_tzs",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    tz_number = table.Column<short>(type: "smallint", nullable: false),
                    mode = table.Column<short>(type: "smallint", nullable: false),
                    act_time = table.Column<short>(type: "smallint", nullable: false),
                    deact_time = table.Column<short>(type: "smallint", nullable: false),
                    intervals = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ar_tzs", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ar_users",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<string>(type: "text", nullable: false),
                    user_name = table.Column<string>(type: "text", nullable: false),
                    password = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    title = table.Column<string>(type: "text", nullable: false),
                    first_name = table.Column<string>(type: "text", nullable: false),
                    middle_name = table.Column<string>(type: "text", nullable: false),
                    last_name = table.Column<string>(type: "text", nullable: false),
                    role = table.Column<string>(type: "text", nullable: false),
                    phone = table.Column<string>(type: "text", nullable: false),
                    photo = table.Column<string>(type: "text", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ar_users", x => x.id);
                });

            migrationBuilder.InsertData(
                table: "ar_access_lvls",
                columns: new[] { "id", "access_lv_number", "name", "tz1", "tz10", "tz11", "tz12", "tz13", "tz14", "tz15", "tz16", "tz17", "tz18", "tz19", "tz2", "tz20", "tz21", "tz22", "tz23", "tz24", "tz25", "tz26", "tz27", "tz28", "tz29", "tz3", "tz30", "tz31", "tz32", "tz33", "tz34", "tz35", "tz36", "tz37", "tz38", "tz39", "tz4", "tz40", "tz41", "tz42", "tz43", "tz44", "tz45", "tz46", "tz47", "tz48", "tz49", "tz5", "tz50", "tz51", "tz52", "tz53", "tz54", "tz55", "tz56", "tz57", "tz58", "tz59", "tz6", "tz60", "tz61", "tz62", "tz63", "tz64", "tz7", "tz8", "tz9" },
                values: new object[,]
                {
                    { 1, (short)0, "Full Access", (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1 },
                    { 2, (short)1, "No Access", (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0 }
                });

            migrationBuilder.InsertData(
                table: "ar_acr_modes",
                columns: new[] { "id", "description", "name", "value" },
                values: new object[,]
                {
                    { 1, "Disable the ACR, no REX", "Disable", (short)1 },
                    { 2, "Unlock (unlimited access)", "Unlock", (short)2 },
                    { 3, "Locked (no access, REX active)", "Locked", (short)3 },
                    { 4, "Facility code only", "Facility code only", (short)4 },
                    { 5, "Card only", "Card only", (short)5 },
                    { 6, "PIN only", "PIN only", (short)6 },
                    { 7, "Card and PIN required", "Card and PIN", (short)7 },
                    { 8, "Card or PIN required", "Card or PIN", (short)8 }
                });

            migrationBuilder.InsertData(
                table: "ar_apb_modes",
                columns: new[] { "id", "description", "name", "value" },
                values: new object[,]
                {
                    { 1, "Do not check or alter anti-passback location. No antipassback rules.", "None", (short)0 },
                    { 2, "Soft anti-passback: Accept any new location, change the user’s location to current reader, and generate an antipassback violation for an invalid entry.", "Soft anti-passback", (short)1 },
                    { 3, "Hard anti-passback: Check user location, if a valid entry is made, change user’s location to new location. If an invalid entry is attempted, do not grant access.", "Hard anti-passback", (short)2 }
                });

            migrationBuilder.InsertData(
                table: "ar_card_formats",
                columns: new[] { "id", "bits", "ch_ln", "ch_loc", "facility", "fc_ln", "fc_loc", "flags", "function_id", "ic_ln", "ic_loc", "number", "offset", "pe_ln", "pe_loc", "po_ln", "po_loc" },
                values: new object[] { 1, (short)26, (short)16, (short)9, (short)-1, (short)0, (short)0, (short)0, (short)1, (short)0, (short)0, (short)0, (short)0, (short)13, (short)0, (short)13, (short)13 });

            migrationBuilder.InsertData(
                table: "ar_ip_modes",
                columns: new[] { "id", "description", "name", "value" },
                values: new object[,]
                {
                    { 1, "Normally closed, no End-Of-Line (EOL)", "Normally closed", (short)0 },
                    { 2, "Normally open, no EOL", "Normally open", (short)1 }
                });

            migrationBuilder.InsertData(
                table: "ar_op_modes",
                columns: new[] { "id", "description", "value" },
                values: new object[,]
                {
                    { 1, "Normal Mode with Offline: No change", (short)0 },
                    { 2, "Inverted Mode Offline: No change", (short)1 },
                    { 3, "Normal Mode Offline: Inactive", (short)16 },
                    { 4, "Inverted Mode Offline: Inactive", (short)17 },
                    { 5, "Normal Mode Offline: Active", (short)32 },
                    { 6, "Inverted Mode Offline: Inactive", (short)33 }
                });

            migrationBuilder.InsertData(
                table: "ar_rdr_modes",
                columns: new[] { "id", "description", "name", "value" },
                values: new object[,]
                {
                    { 1, "Single reader, controlling the door", "Single Reader", (short)0 },
                    { 2, "Paired readers, Master - this reader controls the door", "Paired readers, Master", (short)1 },
                    { 3, "Paired readers, Slave - this reader does not control door", "Paired readers, Slave", (short)2 },
                    { 4, "Turnstile Reader. Two modes selected by: n strike_t_min != strike_t_max (original implementation - an access grant pulses the strike output for 1 second) n strike_t_min == strike_t_max (pulses the strike output after a door open/close signal for each additional access grant if several grants are waiting)", "Turnstile Reader", (short)3 },
                    { 5, "Elevator, no floor select feedback", "Elevator, no floor", (short)4 },
                    { 6, "Elevator with floor select feedback", "Elevator with floor", (short)5 }
                });

            migrationBuilder.InsertData(
                table: "ar_strk_modes",
                columns: new[] { "id", "description", "name", "value" },
                values: new object[,]
                {
                    { 1, "Do not use! This would allow the strike to stay active for the entire strike time allowing the door to be opened multiple times.", "Normal", (short)0 },
                    { 2, "Deactivate strike when door opens", "Deactivate On Open", (short)1 },
                    { 3, "Deactivate strike on door close or strike_t_max expires", "Deactivate On Close", (short)2 },
                    { 4, "Used with ACR_S_OPEN or ACR_S_CLOSE, to select tailgate mode: pulse (strk_sio:strk_number+1) relay for each user expected to enter", "Tailgate", (short)16 }
                });

            migrationBuilder.InsertData(
                table: "ar_tz_no",
                columns: new[] { "id", "is_available", "tz_number" },
                values: new object[] { 1, false, (short)1 });

            migrationBuilder.InsertData(
                table: "ar_tzs",
                columns: new[] { "id", "act_time", "deact_time", "intervals", "mode", "name", "tz_number" },
                values: new object[] { 1, (short)0, (short)0, (short)0, (short)1, "Always", (short)1 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ar_access_lvls");

            migrationBuilder.DropTable(
                name: "ar_acr_modes");

            migrationBuilder.DropTable(
                name: "ar_acr_no");

            migrationBuilder.DropTable(
                name: "ar_acrs");

            migrationBuilder.DropTable(
                name: "ar_apb_modes");

            migrationBuilder.DropTable(
                name: "ar_card_formats");

            migrationBuilder.DropTable(
                name: "ar_card_holders");

            migrationBuilder.DropTable(
                name: "ar_control_point");

            migrationBuilder.DropTable(
                name: "ar_cp_no");

            migrationBuilder.DropTable(
                name: "ar_credentials");

            migrationBuilder.DropTable(
                name: "ar_events");

            migrationBuilder.DropTable(
                name: "ar_ip_modes");

            migrationBuilder.DropTable(
                name: "ar_monitor_point");

            migrationBuilder.DropTable(
                name: "ar_mp_no");

            migrationBuilder.DropTable(
                name: "ar_naks");

            migrationBuilder.DropTable(
                name: "ar_op_modes");

            migrationBuilder.DropTable(
                name: "ar_rdr_modes");

            migrationBuilder.DropTable(
                name: "ar_readers");

            migrationBuilder.DropTable(
                name: "ar_scps");

            migrationBuilder.DropTable(
                name: "ar_sio_no");

            migrationBuilder.DropTable(
                name: "ar_sios");

            migrationBuilder.DropTable(
                name: "ar_strk_modes");

            migrationBuilder.DropTable(
                name: "ar_tz_intervals");

            migrationBuilder.DropTable(
                name: "ar_tz_no");

            migrationBuilder.DropTable(
                name: "ar_tzs");

            migrationBuilder.DropTable(
                name: "ar_users");
        }
    }
}
