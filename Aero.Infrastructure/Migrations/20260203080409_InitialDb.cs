using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Aero.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "access_area_command",
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
                    table.PrimaryKey("PK_access_area_command", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "action_type",
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
                    table.PrimaryKey("PK_action_type", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "antipassback_mode",
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
                    table.PrimaryKey("PK_antipassback_mode", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "area_access_control",
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
                    table.PrimaryKey("PK_area_access_control", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "area_flag",
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
                    table.PrimaryKey("PK_area_flag", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "commnad_log",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    tag_no = table.Column<int>(type: "integer", nullable: false),
                    hardware_id = table.Column<int>(type: "integer", nullable: false),
                    hardware_mac = table.Column<string>(type: "text", nullable: true),
                    command = table.Column<string>(type: "text", nullable: true),
                    status = table.Column<char>(type: "character(1)", nullable: false),
                    nak_reason = table.Column<string>(type: "text", nullable: true),
                    nake_desc_code = table.Column<int>(type: "integer", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_commnad_log", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "credential_flag",
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
                    table.PrimaryKey("PK_credential_flag", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "door_access_control_flag",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    value = table.Column<int>(type: "integer", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_door_access_control_flag", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "door_mode",
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
                    table.PrimaryKey("PK_door_mode", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "door_spare_flag",
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
                    table.PrimaryKey("PK_door_spare_flag", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "feature",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    component_id = table.Column<short>(type: "smallint", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    path = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_feature", x => x.id);
                    table.UniqueConstraint("AK_feature_component_id", x => x.component_id);
                });

            migrationBuilder.CreateTable(
                name: "file_type",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    value = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_file_type", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "hardware_component",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    model_no = table.Column<short>(type: "smallint", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    n_input = table.Column<short>(type: "smallint", nullable: false),
                    n_output = table.Column<short>(type: "smallint", nullable: false),
                    n_reader = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_hardware_component", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "hardware_type",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    component_id = table.Column<short>(type: "smallint", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_hardware_type", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "id_report",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    device_id = table.Column<short>(type: "smallint", nullable: false),
                    device_ver = table.Column<short>(type: "smallint", nullable: false),
                    software_rev_major = table.Column<short>(type: "smallint", nullable: false),
                    software_rev_minor = table.Column<short>(type: "smallint", nullable: false),
                    firmware = table.Column<string>(type: "text", nullable: false),
                    serial_number = table.Column<int>(type: "integer", nullable: false),
                    ram_size = table.Column<int>(type: "integer", nullable: false),
                    ram_free = table.Column<int>(type: "integer", nullable: false),
                    e_sec = table.Column<int>(type: "integer", nullable: false),
                    db_max = table.Column<int>(type: "integer", nullable: false),
                    db_active = table.Column<int>(type: "integer", nullable: false),
                    dip_switch_powerup = table.Column<byte>(type: "smallint", nullable: false),
                    dip_switch_current = table.Column<byte>(type: "smallint", nullable: false),
                    scp_id = table.Column<short>(type: "smallint", nullable: false),
                    firmware_advisory = table.Column<short>(type: "smallint", nullable: false),
                    scp_in1 = table.Column<short>(type: "smallint", nullable: false),
                    scp_in2 = table.Column<short>(type: "smallint", nullable: false),
                    n_oem_code = table.Column<short>(type: "smallint", nullable: false),
                    config_flag = table.Column<byte>(type: "smallint", nullable: false),
                    mac = table.Column<string>(type: "text", nullable: false),
                    tls_status = table.Column<byte>(type: "smallint", nullable: false),
                    oper_mode = table.Column<byte>(type: "smallint", nullable: false),
                    scp_in3 = table.Column<short>(type: "smallint", nullable: false),
                    cumulative_bld_cnt = table.Column<int>(type: "integer", nullable: false),
                    ip = table.Column<string>(type: "text", nullable: false),
                    port = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_id_report", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "input_mode",
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
                    table.PrimaryKey("PK_input_mode", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "location",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    component_id = table.Column<short>(type: "smallint", nullable: false),
                    location_name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_location", x => x.id);
                    table.UniqueConstraint("AK_location_component_id", x => x.component_id);
                });

            migrationBuilder.CreateTable(
                name: "module_baudrate",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    value = table.Column<int>(type: "integer", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_module_baudrate", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "module_protocol",
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
                    table.PrimaryKey("PK_module_protocol", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "monitor_group_command",
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
                    table.PrimaryKey("PK_monitor_group_command", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "monitor_group_type",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    value = table.Column<short>(type: "smallint", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_monitor_group_type", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "monitor_point_log_function",
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
                    table.PrimaryKey("PK_monitor_point_log_function", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "monitor_point_mode",
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
                    table.PrimaryKey("PK_monitor_point_mode", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "multi_occupancy",
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
                    table.PrimaryKey("PK_multi_occupancy", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "occupancy_control",
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
                    table.PrimaryKey("PK_occupancy_control", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "osdp_address",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    value = table.Column<short>(type: "smallint", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_osdp_address", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "osdp_baudrate",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    value = table.Column<short>(type: "smallint", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_osdp_baudrate", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "output_mode",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    value = table.Column<short>(type: "smallint", nullable: false),
                    offline_mode = table.Column<short>(type: "smallint", nullable: false),
                    relay_mode = table.Column<short>(type: "smallint", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_output_mode", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "password_rule",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    len = table.Column<int>(type: "integer", nullable: false),
                    is_lower = table.Column<bool>(type: "boolean", nullable: false),
                    is_upper = table.Column<bool>(type: "boolean", nullable: false),
                    is_digit = table.Column<bool>(type: "boolean", nullable: false),
                    is_symbol = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_password_rule", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "reader_configuration_mode",
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
                    table.PrimaryKey("PK_reader_configuration_mode", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "reader_out_configuration",
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
                    table.PrimaryKey("PK_reader_out_configuration", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "refresh_token",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    uuid = table.Column<Guid>(type: "uuid", nullable: false),
                    hashed_token = table.Column<string>(type: "text", nullable: false),
                    user_id = table.Column<string>(type: "text", nullable: false),
                    user_name = table.Column<string>(type: "text", nullable: false),
                    action = table.Column<string>(type: "text", nullable: false),
                    info = table.Column<string>(type: "text", nullable: true),
                    expire_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_refresh_token", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "relay_mode",
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
                    table.PrimaryKey("PK_relay_mode", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "relay_offline_mode",
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
                    table.PrimaryKey("PK_relay_offline_mode", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "role",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    component_id = table.Column<short>(type: "smallint", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_role", x => x.id);
                    table.UniqueConstraint("AK_role_component_id", x => x.component_id);
                });

            migrationBuilder.CreateTable(
                name: "scp_setting",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    n_msp1_port = table.Column<short>(type: "smallint", nullable: false),
                    n_transaction = table.Column<int>(type: "integer", nullable: false),
                    n_sio = table.Column<short>(type: "smallint", nullable: false),
                    n_mp = table.Column<short>(type: "smallint", nullable: false),
                    n_cp = table.Column<short>(type: "smallint", nullable: false),
                    n_acr = table.Column<short>(type: "smallint", nullable: false),
                    n_alvl = table.Column<short>(type: "smallint", nullable: false),
                    n_trgr = table.Column<short>(type: "smallint", nullable: false),
                    n_proc = table.Column<short>(type: "smallint", nullable: false),
                    gmt_offset = table.Column<short>(type: "smallint", nullable: false),
                    n_tz = table.Column<short>(type: "smallint", nullable: false),
                    n_hol = table.Column<short>(type: "smallint", nullable: false),
                    n_mpg = table.Column<short>(type: "smallint", nullable: false),
                    n_card = table.Column<short>(type: "smallint", nullable: false),
                    n_area = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_scp_setting", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "strike_mode",
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
                    table.PrimaryKey("PK_strike_mode", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "system_configuration",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    n_ports = table.Column<short>(type: "smallint", nullable: false),
                    n_scp = table.Column<short>(type: "smallint", nullable: false),
                    n_channel_id = table.Column<short>(type: "smallint", nullable: false),
                    c_type = table.Column<short>(type: "smallint", nullable: false),
                    c_port = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_system_configuration", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "timezone_command",
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
                    table.PrimaryKey("PK_timezone_command", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "timezone_mode",
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
                    table.PrimaryKey("PK_timezone_mode", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "transaction_source",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    value = table.Column<short>(type: "smallint", nullable: false),
                    source = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transaction_source", x => x.id);
                    table.UniqueConstraint("AK_transaction_source_value", x => x.value);
                });

            migrationBuilder.CreateTable(
                name: "transaction_type",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    value = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transaction_type", x => x.id);
                    table.UniqueConstraint("AK_transaction_type_value", x => x.value);
                });

            migrationBuilder.CreateTable(
                name: "trigger_command",
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
                    table.PrimaryKey("PK_trigger_command", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sub_feature",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    component_id = table.Column<short>(type: "smallint", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    path = table.Column<string>(type: "text", nullable: false),
                    feature_id = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sub_feature", x => x.id);
                    table.ForeignKey(
                        name: "FK_sub_feature_feature_feature_id",
                        column: x => x.feature_id,
                        principalTable: "feature",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "access_level",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    location_id = table.Column<short>(type: "smallint", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    component_id = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_access_level", x => x.id);
                    table.UniqueConstraint("AK_access_level_component_id", x => x.component_id);
                    table.ForeignKey(
                        name: "FK_access_level_location_location_id",
                        column: x => x.location_id,
                        principalTable: "location",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "area",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    multi_occ = table.Column<short>(type: "smallint", nullable: false),
                    access_control = table.Column<short>(type: "smallint", nullable: false),
                    occ_control = table.Column<short>(type: "smallint", nullable: false),
                    occ_set = table.Column<short>(type: "smallint", nullable: false),
                    occ_max = table.Column<short>(type: "smallint", nullable: false),
                    occ_up = table.Column<short>(type: "smallint", nullable: false),
                    occ_down = table.Column<short>(type: "smallint", nullable: false),
                    area_flag = table.Column<short>(type: "smallint", nullable: false),
                    location_id = table.Column<short>(type: "smallint", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    component_id = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_area", x => x.id);
                    table.UniqueConstraint("AK_area_component_id", x => x.component_id);
                    table.ForeignKey(
                        name: "FK_area_location_location_id",
                        column: x => x.location_id,
                        principalTable: "location",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "card_format",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
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
                    ic_loc = table.Column<short>(type: "smallint", nullable: false),
                    location_id = table.Column<short>(type: "smallint", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    component_id = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_card_format", x => x.id);
                    table.ForeignKey(
                        name: "FK_card_format_location_location_id",
                        column: x => x.location_id,
                        principalTable: "location",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "hardware",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    hardware_type = table.Column<int>(type: "integer", nullable: false),
                    hardware_type_desc = table.Column<string>(type: "text", nullable: false),
                    ip = table.Column<string>(type: "text", nullable: false),
                    mac = table.Column<string>(type: "text", nullable: false),
                    port = table.Column<string>(type: "text", nullable: false),
                    firmware = table.Column<string>(type: "text", nullable: false),
                    serial_number = table.Column<string>(type: "text", nullable: false),
                    port_one = table.Column<bool>(type: "boolean", nullable: false),
                    protocol_one = table.Column<short>(type: "smallint", nullable: false),
                    protocol_one_desc = table.Column<string>(type: "text", nullable: false),
                    baudrate_one = table.Column<short>(type: "smallint", nullable: false),
                    port_two = table.Column<bool>(type: "boolean", nullable: false),
                    protocol_two = table.Column<short>(type: "smallint", nullable: false),
                    protocol_two_desc = table.Column<string>(type: "text", nullable: false),
                    baudrate_two = table.Column<short>(type: "smallint", nullable: false),
                    is_upload = table.Column<bool>(type: "boolean", nullable: false),
                    is_reset = table.Column<bool>(type: "boolean", nullable: false),
                    last_sync = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    component_id = table.Column<short>(type: "smallint", nullable: false),
                    location_id = table.Column<short>(type: "smallint", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_hardware", x => x.id);
                    table.UniqueConstraint("AK_hardware_component_id", x => x.component_id);
                    table.UniqueConstraint("AK_hardware_mac", x => x.mac);
                    table.ForeignKey(
                        name: "FK_hardware_location_location_id",
                        column: x => x.location_id,
                        principalTable: "location",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "holiday",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    component_id = table.Column<short>(type: "smallint", nullable: false),
                    year = table.Column<short>(type: "smallint", nullable: false),
                    month = table.Column<short>(type: "smallint", nullable: false),
                    day = table.Column<short>(type: "smallint", nullable: false),
                    extend = table.Column<short>(type: "smallint", nullable: false),
                    type_mask = table.Column<short>(type: "smallint", nullable: false),
                    location_id = table.Column<short>(type: "smallint", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_holiday", x => x.id);
                    table.ForeignKey(
                        name: "FK_holiday_location_location_id",
                        column: x => x.location_id,
                        principalTable: "location",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "interval",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    component_id = table.Column<short>(type: "smallint", nullable: false),
                    days_desc = table.Column<string>(type: "text", nullable: false),
                    start_time = table.Column<string>(type: "text", nullable: false),
                    end_time = table.Column<string>(type: "text", nullable: false),
                    location_id = table.Column<short>(type: "smallint", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_interval", x => x.id);
                    table.UniqueConstraint("AK_interval_component_id", x => x.component_id);
                    table.ForeignKey(
                        name: "FK_interval_location_location_id",
                        column: x => x.location_id,
                        principalTable: "location",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "timezone",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    component_id = table.Column<short>(type: "smallint", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    mode = table.Column<short>(type: "smallint", nullable: false),
                    active_time = table.Column<string>(type: "text", nullable: false),
                    deactive_time = table.Column<string>(type: "text", nullable: false),
                    location_id = table.Column<short>(type: "smallint", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_timezone", x => x.id);
                    table.UniqueConstraint("AK_timezone_component_id", x => x.component_id);
                    table.ForeignKey(
                        name: "FK_timezone_location_location_id",
                        column: x => x.location_id,
                        principalTable: "location",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "transaction",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    date = table.Column<string>(type: "text", nullable: false),
                    time = table.Column<string>(type: "text", nullable: false),
                    serial_number = table.Column<int>(type: "integer", nullable: false),
                    actor = table.Column<string>(type: "text", nullable: false),
                    source = table.Column<double>(type: "double precision", nullable: false),
                    source_desc = table.Column<string>(type: "text", nullable: false),
                    origin = table.Column<string>(type: "text", nullable: false),
                    source_module = table.Column<string>(type: "text", nullable: false),
                    type = table.Column<double>(type: "double precision", nullable: false),
                    type_desc = table.Column<string>(type: "text", nullable: false),
                    tran_code = table.Column<double>(type: "double precision", nullable: false),
                    image_path = table.Column<string>(type: "text", nullable: false),
                    tran_code_desc = table.Column<string>(type: "text", nullable: false),
                    extend_desc = table.Column<string>(type: "text", nullable: false),
                    remark = table.Column<string>(type: "text", nullable: false),
                    hardware_mac = table.Column<string>(type: "text", nullable: false),
                    hardware_name = table.Column<string>(type: "text", nullable: false),
                    component_id = table.Column<short>(type: "smallint", nullable: false),
                    mac = table.Column<string>(type: "text", nullable: false),
                    location_id = table.Column<short>(type: "smallint", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transaction", x => x.id);
                    table.ForeignKey(
                        name: "FK_transaction_location_location_id",
                        column: x => x.location_id,
                        principalTable: "location",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "weak_password",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    pattern = table.Column<string>(type: "text", nullable: false),
                    password_rule_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_weak_password", x => x.id);
                    table.ForeignKey(
                        name: "FK_weak_password_password_rule_password_rule_id",
                        column: x => x.password_rule_id,
                        principalTable: "password_rule",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "feature_role",
                columns: table => new
                {
                    feature_id = table.Column<short>(type: "smallint", nullable: false),
                    role_id = table.Column<short>(type: "smallint", nullable: false),
                    is_allow = table.Column<bool>(type: "boolean", nullable: false),
                    is_create = table.Column<bool>(type: "boolean", nullable: false),
                    is_modify = table.Column<bool>(type: "boolean", nullable: false),
                    is_delete = table.Column<bool>(type: "boolean", nullable: false),
                    is_action = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_feature_role", x => new { x.role_id, x.feature_id });
                    table.ForeignKey(
                        name: "FK_feature_role_feature_feature_id",
                        column: x => x.feature_id,
                        principalTable: "feature",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_feature_role_role_role_id",
                        column: x => x.role_id,
                        principalTable: "role",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "operator",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    component_id = table.Column<short>(type: "smallint", nullable: false),
                    user_id = table.Column<string>(type: "text", nullable: false),
                    user_name = table.Column<string>(type: "text", nullable: false),
                    password = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    title = table.Column<string>(type: "text", nullable: false),
                    first_name = table.Column<string>(type: "text", nullable: false),
                    middle_name = table.Column<string>(type: "text", nullable: false),
                    last_name = table.Column<string>(type: "text", nullable: false),
                    phone = table.Column<string>(type: "text", nullable: false),
                    image_path = table.Column<string>(type: "text", nullable: false),
                    role_id = table.Column<short>(type: "smallint", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_operator", x => x.id);
                    table.UniqueConstraint("AK_operator_component_id", x => x.component_id);
                    table.ForeignKey(
                        name: "FK_operator_role_role_id",
                        column: x => x.role_id,
                        principalTable: "role",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "transaction_code",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    value = table.Column<short>(type: "smallint", nullable: false),
                    transaction_type_value = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transaction_code", x => x.id);
                    table.ForeignKey(
                        name: "FK_transaction_code_transaction_type_transaction_type_value",
                        column: x => x.transaction_type_value,
                        principalTable: "transaction_type",
                        principalColumn: "value",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "transaction_source_type",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    transction_source_value = table.Column<short>(type: "smallint", nullable: false),
                    transction_type_value = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transaction_source_type", x => x.id);
                    table.ForeignKey(
                        name: "FK_transaction_source_type_transaction_source_transction_sourc~",
                        column: x => x.transction_source_value,
                        principalTable: "transaction_source",
                        principalColumn: "value",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_transaction_source_type_transaction_type_transction_type_va~",
                        column: x => x.transction_type_value,
                        principalTable: "transaction_type",
                        principalColumn: "value",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "access_level_component",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    mac = table.Column<string>(type: "text", nullable: false),
                    access_level_id = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_access_level_component", x => x.id);
                    table.ForeignKey(
                        name: "FK_access_level_component_access_level_access_level_id",
                        column: x => x.access_level_id,
                        principalTable: "access_level",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "cardholder",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<string>(type: "text", nullable: false),
                    title = table.Column<string>(type: "text", nullable: false),
                    first_name = table.Column<string>(type: "text", nullable: false),
                    middle_name = table.Column<string>(type: "text", nullable: false),
                    last_name = table.Column<string>(type: "text", nullable: false),
                    sex = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    phone = table.Column<string>(type: "text", nullable: false),
                    company = table.Column<string>(type: "text", nullable: false),
                    department = table.Column<string>(type: "text", nullable: false),
                    position = table.Column<string>(type: "text", nullable: false),
                    flag = table.Column<short>(type: "smallint", nullable: false),
                    image_path = table.Column<string>(type: "text", nullable: false),
                    AccessLevelid = table.Column<int>(type: "integer", nullable: true),
                    location_id = table.Column<short>(type: "smallint", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    component_id = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cardholder", x => x.id);
                    table.UniqueConstraint("AK_cardholder_component_id", x => x.component_id);
                    table.UniqueConstraint("AK_cardholder_user_id", x => x.user_id);
                    table.ForeignKey(
                        name: "FK_cardholder_access_level_AccessLevelid",
                        column: x => x.AccessLevelid,
                        principalTable: "access_level",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_cardholder_location_location_id",
                        column: x => x.location_id,
                        principalTable: "location",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "hardware_access_level",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    hardware_accesslevel_id = table.Column<short>(type: "smallint", nullable: false),
                    access_levelid = table.Column<int>(type: "integer", nullable: false),
                    hardware_mac = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_hardware_access_level", x => x.id);
                    table.ForeignKey(
                        name: "FK_hardware_access_level_access_level_access_levelid",
                        column: x => x.access_levelid,
                        principalTable: "access_level",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_hardware_access_level_hardware_hardware_mac",
                        column: x => x.hardware_mac,
                        principalTable: "hardware",
                        principalColumn: "mac",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "module",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    model = table.Column<short>(type: "smallint", nullable: false),
                    model_desc = table.Column<string>(type: "text", nullable: false),
                    revision = table.Column<string>(type: "text", nullable: false),
                    serial_number = table.Column<string>(type: "text", nullable: false),
                    n_hardware_id = table.Column<int>(type: "integer", nullable: false),
                    n_hardware_id_desc = table.Column<string>(type: "text", nullable: false),
                    n_hardware_rev = table.Column<int>(type: "integer", nullable: false),
                    n_product_id = table.Column<int>(type: "integer", nullable: false),
                    n_product_ver = table.Column<int>(type: "integer", nullable: false),
                    n_enc_config = table.Column<short>(type: "smallint", nullable: false),
                    n_enc_config_desc = table.Column<string>(type: "text", nullable: false),
                    n_enc_key_status = table.Column<short>(type: "smallint", nullable: false),
                    n_enc_key_status_desc = table.Column<string>(type: "text", nullable: false),
                    hardware_mac = table.Column<string>(type: "text", nullable: false),
                    address = table.Column<short>(type: "smallint", nullable: false),
                    address_desc = table.Column<string>(type: "text", nullable: false),
                    port = table.Column<short>(type: "smallint", nullable: false),
                    n_input = table.Column<short>(type: "smallint", nullable: false),
                    n_output = table.Column<short>(type: "smallint", nullable: false),
                    n_reader = table.Column<short>(type: "smallint", nullable: false),
                    msp1_no = table.Column<short>(type: "smallint", nullable: false),
                    baudrate = table.Column<short>(type: "smallint", nullable: false),
                    n_protocol = table.Column<short>(type: "smallint", nullable: false),
                    n_dialect = table.Column<short>(type: "smallint", nullable: false),
                    component_id = table.Column<short>(type: "smallint", nullable: false),
                    mac = table.Column<string>(type: "text", nullable: false),
                    location_id = table.Column<short>(type: "smallint", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_module", x => x.id);
                    table.UniqueConstraint("AK_module_component_id", x => x.component_id);
                    table.ForeignKey(
                        name: "FK_module_hardware_hardware_mac",
                        column: x => x.hardware_mac,
                        principalTable: "hardware",
                        principalColumn: "mac",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_module_location_location_id",
                        column: x => x.location_id,
                        principalTable: "location",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "monitor_group",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    n_mp_count = table.Column<short>(type: "smallint", nullable: false),
                    hardware_mac = table.Column<string>(type: "text", nullable: false),
                    component_id = table.Column<short>(type: "smallint", nullable: false),
                    mac = table.Column<string>(type: "text", nullable: false),
                    location_id = table.Column<short>(type: "smallint", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_monitor_group", x => x.id);
                    table.UniqueConstraint("AK_monitor_group_component_id", x => x.component_id);
                    table.ForeignKey(
                        name: "FK_monitor_group_hardware_hardware_mac",
                        column: x => x.hardware_mac,
                        principalTable: "hardware",
                        principalColumn: "mac",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_monitor_group_location_location_id",
                        column: x => x.location_id,
                        principalTable: "location",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "procedure",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    proc_id = table.Column<short>(type: "smallint", nullable: false),
                    component_id = table.Column<short>(type: "smallint", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    location_id = table.Column<short>(type: "smallint", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    Hardwareid = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_procedure", x => x.Id);
                    table.UniqueConstraint("AK_procedure_component_id", x => x.component_id);
                    table.ForeignKey(
                        name: "FK_procedure_hardware_Hardwareid",
                        column: x => x.Hardwareid,
                        principalTable: "hardware",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_procedure_location_location_id",
                        column: x => x.location_id,
                        principalTable: "location",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "days_in_week",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    component_id = table.Column<short>(type: "smallint", nullable: false),
                    sunday = table.Column<bool>(type: "boolean", nullable: false),
                    monday = table.Column<bool>(type: "boolean", nullable: false),
                    tuesday = table.Column<bool>(type: "boolean", nullable: false),
                    wednesday = table.Column<bool>(type: "boolean", nullable: false),
                    thursday = table.Column<bool>(type: "boolean", nullable: false),
                    friday = table.Column<bool>(type: "boolean", nullable: false),
                    saturday = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_days_in_week", x => x.id);
                    table.ForeignKey(
                        name: "FK_days_in_week_interval_component_id",
                        column: x => x.component_id,
                        principalTable: "interval",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "timezone_interval",
                columns: table => new
                {
                    timezone_id = table.Column<short>(type: "smallint", nullable: false),
                    interval_id = table.Column<short>(type: "smallint", nullable: false),
                    id = table.Column<int>(type: "integer", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_timezone_interval", x => new { x.timezone_id, x.interval_id });
                    table.ForeignKey(
                        name: "FK_timezone_interval_interval_interval_id",
                        column: x => x.interval_id,
                        principalTable: "interval",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_timezone_interval_timezone_timezone_id",
                        column: x => x.timezone_id,
                        principalTable: "timezone",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "transaction_flag",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    topic = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    transactionid = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transaction_flag", x => x.id);
                    table.ForeignKey(
                        name: "FK_transaction_flag_transaction_transactionid",
                        column: x => x.transactionid,
                        principalTable: "transaction",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "operator_location",
                columns: table => new
                {
                    location_id = table.Column<short>(type: "smallint", nullable: false),
                    operator_id = table.Column<short>(type: "smallint", nullable: false),
                    id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_operator_location", x => new { x.location_id, x.operator_id });
                    table.ForeignKey(
                        name: "FK_operator_location_location_location_id",
                        column: x => x.location_id,
                        principalTable: "location",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_operator_location_operator_operator_id",
                        column: x => x.operator_id,
                        principalTable: "operator",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "access_level_door_component",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    acr_id = table.Column<short>(type: "smallint", nullable: false),
                    timezone_id = table.Column<short>(type: "smallint", nullable: false),
                    access_level_component_id = table.Column<int>(type: "integer", nullable: false),
                    access_level_componentid = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_access_level_door_component", x => x.id);
                    table.ForeignKey(
                        name: "FK_access_level_door_component_access_level_component_access_l~",
                        column: x => x.access_level_componentid,
                        principalTable: "access_level_component",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "cardholder_access_level",
                columns: table => new
                {
                    holder_id = table.Column<string>(type: "text", nullable: false),
                    accesslevel_id = table.Column<short>(type: "smallint", nullable: false),
                    accessLevelid = table.Column<int>(type: "integer", nullable: false),
                    id = table.Column<int>(type: "integer", nullable: false),
                    location_id = table.Column<short>(type: "smallint", nullable: false),
                    locationid = table.Column<int>(type: "integer", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    component_id = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cardholder_access_level", x => new { x.accesslevel_id, x.holder_id });
                    table.ForeignKey(
                        name: "FK_cardholder_access_level_access_level_accessLevelid",
                        column: x => x.accessLevelid,
                        principalTable: "access_level",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_cardholder_access_level_cardholder_holder_id",
                        column: x => x.holder_id,
                        principalTable: "cardholder",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_cardholder_access_level_location_locationid",
                        column: x => x.locationid,
                        principalTable: "location",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "cardholder_additional",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    holder_id = table.Column<string>(type: "text", nullable: false),
                    additional = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cardholder_additional", x => x.id);
                    table.ForeignKey(
                        name: "FK_cardholder_additional_cardholder_holder_id",
                        column: x => x.holder_id,
                        principalTable: "cardholder",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "credential",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    bits = table.Column<int>(type: "integer", nullable: false),
                    issue_code = table.Column<int>(type: "integer", nullable: false),
                    fac_code = table.Column<int>(type: "integer", nullable: false),
                    card_no = table.Column<long>(type: "bigint", nullable: false),
                    pin = table.Column<string>(type: "text", nullable: true),
                    active_date = table.Column<string>(type: "text", nullable: false),
                    deactive_date = table.Column<string>(type: "text", nullable: false),
                    cardholder_id = table.Column<string>(type: "text", nullable: false),
                    cardholderid = table.Column<int>(type: "integer", nullable: false),
                    location_id = table.Column<short>(type: "smallint", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    component_id = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credential", x => x.id);
                    table.ForeignKey(
                        name: "FK_credential_cardholder_cardholderid",
                        column: x => x.cardholderid,
                        principalTable: "cardholder",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_credential_location_location_id",
                        column: x => x.location_id,
                        principalTable: "location",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "control_point",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    cp_id = table.Column<short>(type: "smallint", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    module_id = table.Column<short>(type: "smallint", nullable: false),
                    module_desc = table.Column<string>(type: "text", nullable: false),
                    output_no = table.Column<short>(type: "smallint", nullable: false),
                    relay_mode = table.Column<short>(type: "smallint", nullable: false),
                    relay_mode_desc = table.Column<string>(type: "text", nullable: false),
                    offline_mode = table.Column<short>(type: "smallint", nullable: false),
                    offline_mode_desc = table.Column<string>(type: "text", nullable: false),
                    default_pulse = table.Column<short>(type: "smallint", nullable: false),
                    component_id = table.Column<short>(type: "smallint", nullable: false),
                    mac = table.Column<string>(type: "text", nullable: false),
                    location_id = table.Column<short>(type: "smallint", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_control_point", x => x.id);
                    table.ForeignKey(
                        name: "FK_control_point_location_location_id",
                        column: x => x.location_id,
                        principalTable: "location",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_control_point_module_module_id",
                        column: x => x.module_id,
                        principalTable: "module",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "monitor_point",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    mp_id = table.Column<short>(type: "smallint", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    module_id = table.Column<short>(type: "smallint", nullable: false),
                    input_no = table.Column<short>(type: "smallint", nullable: false),
                    input_mode = table.Column<short>(type: "smallint", nullable: false),
                    input_mode_desc = table.Column<string>(type: "text", nullable: false),
                    debounce = table.Column<short>(type: "smallint", nullable: false),
                    holdtime = table.Column<short>(type: "smallint", nullable: false),
                    log_function = table.Column<short>(type: "smallint", nullable: false),
                    log_function_desc = table.Column<string>(type: "text", nullable: false),
                    monitor_point_mode = table.Column<short>(type: "smallint", nullable: false),
                    monitor_point_mode_desc = table.Column<string>(type: "text", nullable: false),
                    delay_entry = table.Column<short>(type: "smallint", nullable: false),
                    delay_exit = table.Column<short>(type: "smallint", nullable: false),
                    is_mask = table.Column<bool>(type: "boolean", nullable: false),
                    component_id = table.Column<short>(type: "smallint", nullable: false),
                    mac = table.Column<string>(type: "text", nullable: false),
                    location_id = table.Column<short>(type: "smallint", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_monitor_point", x => x.id);
                    table.ForeignKey(
                        name: "FK_monitor_point_location_location_id",
                        column: x => x.location_id,
                        principalTable: "location",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_monitor_point_module_module_id",
                        column: x => x.module_id,
                        principalTable: "module",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "sensor",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    module_id = table.Column<short>(type: "smallint", nullable: false),
                    input_no = table.Column<short>(type: "smallint", nullable: false),
                    input_mode = table.Column<short>(type: "smallint", nullable: false),
                    debounce = table.Column<short>(type: "smallint", nullable: false),
                    holdtime = table.Column<short>(type: "smallint", nullable: false),
                    dc_held = table.Column<short>(type: "smallint", nullable: false),
                    component_id = table.Column<short>(type: "smallint", nullable: false),
                    mac = table.Column<string>(type: "text", nullable: false),
                    location_id = table.Column<short>(type: "smallint", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sensor", x => x.id);
                    table.UniqueConstraint("AK_sensor_component_id", x => x.component_id);
                    table.ForeignKey(
                        name: "FK_sensor_location_location_id",
                        column: x => x.location_id,
                        principalTable: "location",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_sensor_module_module_id",
                        column: x => x.module_id,
                        principalTable: "module",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "strike",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    module_id = table.Column<short>(type: "smallint", nullable: false),
                    output_no = table.Column<short>(type: "smallint", nullable: false),
                    relay_mode = table.Column<short>(type: "smallint", nullable: false),
                    offline_mode = table.Column<short>(type: "smallint", nullable: false),
                    strike_max = table.Column<short>(type: "smallint", nullable: false),
                    strike_min = table.Column<short>(type: "smallint", nullable: false),
                    strike_mode = table.Column<short>(type: "smallint", nullable: false),
                    component_id = table.Column<short>(type: "smallint", nullable: false),
                    mac = table.Column<string>(type: "text", nullable: false),
                    location_id = table.Column<short>(type: "smallint", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_strike", x => x.id);
                    table.UniqueConstraint("AK_strike_component_id", x => x.component_id);
                    table.ForeignKey(
                        name: "FK_strike_location_location_id",
                        column: x => x.location_id,
                        principalTable: "location",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_strike_module_module_id",
                        column: x => x.module_id,
                        principalTable: "module",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "monitor_group_list",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    point_type = table.Column<short>(type: "smallint", nullable: false),
                    point_type_desc = table.Column<string>(type: "text", nullable: false),
                    point_number = table.Column<short>(type: "smallint", nullable: false),
                    monitor_group_id = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_monitor_group_list", x => x.id);
                    table.ForeignKey(
                        name: "FK_monitor_group_list_monitor_group_monitor_group_id",
                        column: x => x.monitor_group_id,
                        principalTable: "monitor_group",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "action",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    scp_id = table.Column<short>(type: "smallint", nullable: false),
                    action_type = table.Column<short>(type: "smallint", nullable: false),
                    action_type_desc = table.Column<string>(type: "text", nullable: false),
                    arg1 = table.Column<short>(type: "smallint", nullable: false),
                    arg2 = table.Column<short>(type: "smallint", nullable: false),
                    arg3 = table.Column<short>(type: "smallint", nullable: false),
                    arg4 = table.Column<short>(type: "smallint", nullable: false),
                    arg5 = table.Column<short>(type: "smallint", nullable: false),
                    arg6 = table.Column<short>(type: "smallint", nullable: false),
                    arg7 = table.Column<short>(type: "smallint", nullable: false),
                    str_arg = table.Column<string>(type: "text", nullable: false),
                    delay_time = table.Column<short>(type: "smallint", nullable: false),
                    procedure_id = table.Column<short>(type: "smallint", nullable: false),
                    component_id = table.Column<short>(type: "smallint", nullable: false),
                    mac = table.Column<string>(type: "text", nullable: false),
                    location_id = table.Column<short>(type: "smallint", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_action", x => x.id);
                    table.ForeignKey(
                        name: "FK_action_location_location_id",
                        column: x => x.location_id,
                        principalTable: "location",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_action_procedure_procedure_id",
                        column: x => x.procedure_id,
                        principalTable: "procedure",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "trigger",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    trig_id = table.Column<short>(type: "smallint", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    command = table.Column<short>(type: "smallint", nullable: false),
                    procedure_id = table.Column<short>(type: "smallint", nullable: false),
                    source_type = table.Column<short>(type: "smallint", nullable: false),
                    source_number = table.Column<short>(type: "smallint", nullable: false),
                    tran_type = table.Column<short>(type: "smallint", nullable: false),
                    hardware_mac = table.Column<string>(type: "text", nullable: false),
                    hardwareid = table.Column<int>(type: "integer", nullable: false),
                    timezone = table.Column<short>(type: "smallint", nullable: false),
                    component_id = table.Column<short>(type: "smallint", nullable: false),
                    mac = table.Column<string>(type: "text", nullable: false),
                    location_id = table.Column<short>(type: "smallint", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_trigger", x => x.id);
                    table.UniqueConstraint("AK_trigger_component_id", x => x.component_id);
                    table.ForeignKey(
                        name: "FK_trigger_hardware_hardwareid",
                        column: x => x.hardwareid,
                        principalTable: "hardware",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_trigger_location_location_id",
                        column: x => x.location_id,
                        principalTable: "location",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_trigger_procedure_procedure_id",
                        column: x => x.procedure_id,
                        principalTable: "procedure",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "hardware_credential",
                columns: table => new
                {
                    hardware_mac = table.Column<string>(type: "text", nullable: false),
                    hardware_credential_id = table.Column<short>(type: "smallint", nullable: false),
                    id = table.Column<int>(type: "integer", nullable: false),
                    credentialid = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_hardware_credential", x => new { x.hardware_mac, x.hardware_credential_id });
                    table.ForeignKey(
                        name: "FK_hardware_credential_credential_credentialid",
                        column: x => x.credentialid,
                        principalTable: "credential",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_hardware_credential_hardware_hardware_credential_id",
                        column: x => x.hardware_credential_id,
                        principalTable: "hardware",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "door",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    acr_id = table.Column<short>(type: "smallint", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    access_config = table.Column<short>(type: "smallint", nullable: false),
                    pair_door_no = table.Column<short>(type: "smallint", nullable: false),
                    hardware_mac = table.Column<string>(type: "text", nullable: false),
                    reader_out_config = table.Column<short>(type: "smallint", nullable: false),
                    strike_id = table.Column<short>(type: "smallint", nullable: false),
                    sensor_id = table.Column<short>(type: "smallint", nullable: false),
                    card_format = table.Column<short>(type: "smallint", nullable: false),
                    antipassback_mode = table.Column<short>(type: "smallint", nullable: false),
                    antipassback_in = table.Column<short>(type: "smallint", nullable: true),
                    area_in_id = table.Column<short>(type: "smallint", nullable: false),
                    antipassback_out = table.Column<short>(type: "smallint", nullable: true),
                    area_out_id = table.Column<short>(type: "smallint", nullable: false),
                    spare_tag = table.Column<short>(type: "smallint", nullable: false),
                    access_control_flag = table.Column<short>(type: "smallint", nullable: false),
                    mode = table.Column<short>(type: "smallint", nullable: false),
                    mode_desc = table.Column<string>(type: "text", nullable: false),
                    offline_mode = table.Column<short>(type: "smallint", nullable: false),
                    offline_mode_desc = table.Column<string>(type: "text", nullable: false),
                    default_mode = table.Column<short>(type: "smallint", nullable: false),
                    default_mode_desc = table.Column<string>(type: "text", nullable: false),
                    default_led_mode = table.Column<short>(type: "smallint", nullable: false),
                    pre_alarm = table.Column<short>(type: "smallint", nullable: false),
                    antipassback_delay = table.Column<short>(type: "smallint", nullable: false),
                    strike_t2 = table.Column<short>(type: "smallint", nullable: false),
                    dc_held2 = table.Column<short>(type: "smallint", nullable: false),
                    strike_follow_pulse = table.Column<short>(type: "smallint", nullable: false),
                    strike_follow_delay = table.Column<short>(type: "smallint", nullable: false),
                    n_ext_feature_type = table.Column<short>(type: "smallint", nullable: false),
                    i_lpb_sio = table.Column<short>(type: "smallint", nullable: false),
                    i_lpb_number = table.Column<short>(type: "smallint", nullable: false),
                    i_lpb_long_press = table.Column<short>(type: "smallint", nullable: false),
                    i_lpb_out_sio = table.Column<short>(type: "smallint", nullable: false),
                    i_lpb_out_num = table.Column<short>(type: "smallint", nullable: false),
                    df_filter_time = table.Column<short>(type: "smallint", nullable: false),
                    is_held_mask = table.Column<bool>(type: "boolean", nullable: false),
                    is_force_mask = table.Column<bool>(type: "boolean", nullable: false),
                    component_id = table.Column<short>(type: "smallint", nullable: false),
                    mac = table.Column<string>(type: "text", nullable: false),
                    location_id = table.Column<short>(type: "smallint", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_door", x => x.id);
                    table.UniqueConstraint("AK_door_component_id", x => x.component_id);
                    table.ForeignKey(
                        name: "FK_door_area_antipassback_in",
                        column: x => x.antipassback_in,
                        principalTable: "area",
                        principalColumn: "component_id");
                    table.ForeignKey(
                        name: "FK_door_area_antipassback_out",
                        column: x => x.antipassback_out,
                        principalTable: "area",
                        principalColumn: "component_id");
                    table.ForeignKey(
                        name: "FK_door_hardware_hardware_mac",
                        column: x => x.hardware_mac,
                        principalTable: "hardware",
                        principalColumn: "mac",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_door_location_location_id",
                        column: x => x.location_id,
                        principalTable: "location",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_door_sensor_sensor_id",
                        column: x => x.sensor_id,
                        principalTable: "sensor",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_door_strike_strike_id",
                        column: x => x.strike_id,
                        principalTable: "strike",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "trigger_tran_code",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    value = table.Column<short>(type: "smallint", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    trigger_id = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_trigger_tran_code", x => x.id);
                    table.ForeignKey(
                        name: "FK_trigger_tran_code_trigger_value",
                        column: x => x.value,
                        principalTable: "trigger",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "reader",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    module_id = table.Column<short>(type: "smallint", nullable: false),
                    reader_no = table.Column<short>(type: "smallint", nullable: false),
                    data_format = table.Column<short>(type: "smallint", nullable: false),
                    keypad_mode = table.Column<short>(type: "smallint", nullable: false),
                    led_drive_mode = table.Column<short>(type: "smallint", nullable: false),
                    direction = table.Column<int>(type: "integer", nullable: false),
                    osdp_flag = table.Column<bool>(type: "boolean", nullable: false),
                    osdp_baudrate = table.Column<short>(type: "smallint", nullable: false),
                    osdp_discover = table.Column<short>(type: "smallint", nullable: false),
                    osdp_tracing = table.Column<short>(type: "smallint", nullable: false),
                    osdp_address = table.Column<short>(type: "smallint", nullable: false),
                    osdp_secure_channel = table.Column<short>(type: "smallint", nullable: false),
                    component_id = table.Column<short>(type: "smallint", nullable: false),
                    mac = table.Column<string>(type: "text", nullable: false),
                    location_id = table.Column<short>(type: "smallint", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reader", x => x.id);
                    table.ForeignKey(
                        name: "FK_reader_door_component_id",
                        column: x => x.component_id,
                        principalTable: "door",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_reader_location_location_id",
                        column: x => x.location_id,
                        principalTable: "location",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_reader_module_module_id",
                        column: x => x.module_id,
                        principalTable: "module",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "request_exit",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    module_id = table.Column<short>(type: "smallint", nullable: false),
                    input_no = table.Column<short>(type: "smallint", nullable: false),
                    input_mode = table.Column<short>(type: "smallint", nullable: false),
                    debounce = table.Column<short>(type: "smallint", nullable: false),
                    holdtime = table.Column<short>(type: "smallint", nullable: false),
                    mask_timezone = table.Column<short>(type: "smallint", nullable: false),
                    component_id = table.Column<short>(type: "smallint", nullable: false),
                    mac = table.Column<string>(type: "text", nullable: false),
                    location_id = table.Column<short>(type: "smallint", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_request_exit", x => x.id);
                    table.ForeignKey(
                        name: "FK_request_exit_door_component_id",
                        column: x => x.component_id,
                        principalTable: "door",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_request_exit_location_location_id",
                        column: x => x.location_id,
                        principalTable: "location",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_request_exit_module_module_id",
                        column: x => x.module_id,
                        principalTable: "module",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "access_area_command",
                columns: new[] { "id", "description", "name", "value" },
                values: new object[,]
                {
                    { 1, "Disable Area", "Disable Area", (short)1 },
                    { 2, "Enable area", "Enable area", (short)2 },
                    { 3, "Set current occupancy to occ_set value", "Set current occupancy to occ_set value", (short)3 },
                    { 4, "Clear occupancy counts of the “standard” and “special” users", "Clear occupancy counts of the “standard” and “special” users", (short)5 },
                    { 5, "Disable multi-occupancy rules", "Disable multi-occupancy rules", (short)6 },
                    { 6, "Enable standard multi-occupancy processing", "Enable standard multi-occupancy processing", (short)7 }
                });

            migrationBuilder.InsertData(
                table: "action_type",
                columns: new[] { "id", "description", "name", "value" },
                values: new object[,]
                {
                    { 2, "command 306: Monitor Point Mask", "Monitor Point Mask", (short)1 },
                    { 3, "command 307: Control Point command", "Control Point command", (short)2 },
                    { 4, "command 308: ACR mode", "door mode", (short)3 },
                    { 7, "command 311: Momentary Unlock", "Momentary Unlock", (short)6 },
                    { 10, "command 314: time Zone Control", "time Zone Control", (short)9 },
                    { 13, "command 321: Monitor Point Group Arm/Disarm", "Monitor Point Group Arm/Disarm", (short)14 },
                    { 20, "command 334: Temporary ACR mode", "Temporary door mode", (short)24 },
                    { 21, "command 331: Host Simulated Card Read", "Host Simulated Card Read", (short)25 },
                    { 28, "Delay in seconds", "Delay (Seconds)", (short)127 }
                });

            migrationBuilder.InsertData(
                table: "antipassback_mode",
                columns: new[] { "id", "description", "name", "value" },
                values: new object[,]
                {
                    { 1, "Do not check or alter anti-passback location. No antipassback rules.", "None", (short)0 },
                    { 2, "Soft anti-passback: Accept any new location, change the user’s location to current reader, and generate an antipassback violation for an invalid entry.", "Soft Anti-passback", (short)1 },
                    { 3, "Hard anti-passback: Check user location, if a valid entry is made, change user’s location to new location. If an invalid entry is attempted, do not grant access.", "Hard Anti-passback", (short)2 },
                    { 4, "Reader-based anti-passback using the ACR’s last valid user. Verify it’s not the same user within the time parameter specified within apb_delay.", "time-base Anti-passback - Last (Second)", (short)3 },
                    { 5, "Reader-based anti-passback using the access history from the cardholder database: Check user’s last ACR used, checks for same reader within a specified time (apb_delay). This requires the bSupportTimeApb flag be set in command 1105: Access Database Specification.", "time-base Anti-passback - History (Second)", (short)4 },
                    { 6, "Area based anti-passback: Check user’s current location, if it does not match the expected location then check the delay time (apb_delay). Change user’s location on entry. This requires the bSupportTimeApb flag be set in command 1105: Access Database Specification.", "Area-base Anti-passback (Second)", (short)5 },
                    { 7, "Same as \"time-base Anti-passback - Last (Second)\" but the apb_delay value is treated as minutes instead of seconds.", "time-base Anti-passback - Last (Minute)", (short)6 },
                    { 8, "Same as \"time-base Anti-passback - History (Second)\" but the apb_delay value is treated as minutes instead of seconds.", "time-base Anti-passback - History (Minute)", (short)7 },
                    { 9, "Same as \"Area-base Anti-passback (Second)\" but the apb_delay value is treated as minutes instead of seconds.", "Area-base Anti-passback (Minute)", (short)8 }
                });

            migrationBuilder.InsertData(
                table: "area_access_control",
                columns: new[] { "id", "description", "name", "value" },
                values: new object[,]
                {
                    { 1, "No Operation", "NOP", (short)0 },
                    { 2, "No One Can Access", "Disable area", (short)1 },
                    { 3, "Enable Area", "Enable area", (short)2 }
                });

            migrationBuilder.InsertData(
                table: "area_flag",
                columns: new[] { "id", "description", "name", "value" },
                values: new object[,]
                {
                    { 1, "Area can have open thresholds to only one other area", "Interlock", (short)1 },
                    { 2, "Just (O)ne (D)oor (O)nly is allowed to be open into this area (AREA_F_AIRLOCK must also be set)", "AirLock One door Only", (short)2 }
                });

            migrationBuilder.InsertData(
                table: "credential_flag",
                columns: new[] { "id", "description", "name", "value" },
                values: new object[,]
                {
                    { 1, "Active credential Record", "Active credential Record", (short)1 },
                    { 2, "Allow one free anti-passback pass", "Free One Antipassback", (short)2 },
                    { 3, "", "Anti-passback exempt", (short)4 },
                    { 4, "Use timing parameters for the disabled (ADA)", "Timing for disbled (ADA)", (short)8 },
                    { 5, "PIN Exempt for \"Card & PIN\" ACR mode", "pin Exempt", (short)16 },
                    { 6, "Do not change apb_loc", "No Change APB location", (short)32 },
                    { 7, "Do not alter either the \"original\" or the \"current\" use count values", "No UpdateAsync Current Use Count", (short)64 },
                    { 8, "Do not alter the \"current\" use count but change the original use limit stored in the cardholder database", "No UpdateAsync Current Use Count but Change Use Limit", (short)128 }
                });

            migrationBuilder.InsertData(
                table: "door_access_control_flag",
                columns: new[] { "id", "description", "name", "value" },
                values: new object[,]
                {
                    { 1, "ACR_F_DCR	0x0001	\r\n🔹 Purpose: Decrements a user’s “use counter” when they successfully access.\r\n🔹 Effect: Each valid access reduces their remaining allowed uses.\r\n🔹 Use Case: Temporary or limited-access credential (e.g., one-time use visitor cards or tickets).", "Decrease Counter", 1 },
                    { 2, "ACR_F_CUL	0x0002	\r\n🔹 Purpose: Requires that the use limit is non-zero before granting access.\r\n🔹 Effect: If the use counter reaches zero, access is denied.\r\n🔹 Use Case: Works together with ACR_F_DCR for managing credential with limited usage.", "Deny Zero", 2 },
                    { 3, "ACR_F_DRSS	0x0004	\r\n🔹 Purpose: Deny duress access requests.\r\n🔹 Effect: Normally, a duress PIN (like a special emergency PIN) grants access but logs a “duress” event. This flag changes that behavior — access is denied instead, but still logged.\r\n🔹 Use Case: High-security environments where duress entries should never open the door (only alert security).", "Denu Duress", 4 },
                    { 4, "ACR_F_ALLUSED	0x0008	\r\n🔹 Purpose: Treat all access grants as “used” immediately — don’t wait for door contact feedback.\r\n🔹 Effect: When access is granted, the system immediately logs it as used, even if the door sensor doesn’t open.\r\n🔹 Use Case: For systems with no door contact sensor, or for virtual reader (logical access).", "No sensor door", 8 },
                    { 5, "ACR_F_QEXIT	0x0010	\r\n🔹 Purpose: “Quiet Exit” — disables strike relay activation on REX (Request to Exit).\r\n🔹 Effect: When someone exits, the strike is not pulsed — useful for magnetic locks or silent egress door.\r\n🔹 Use Case: Hospital wards, offices, or area where audible clicks must be minimized.", "Quit Exit", 16 },
                    { 6, "ACR_F_FILTER	0x0020	\r\n🔹 Purpose: Filter out detailed door state change transactions (like every open/close event).\r\n🔹 Effect: Reduces event log noise — only key events (grants, denies) are logged.\r\n🔹 Use Case: Typically enabled in production. Disable only if you need fine-grained door state diagnostics.", "door State Filter", 32 },
                    { 7, "ACR_F_2CARD	0x0040	\r\n🔹 Purpose: Enables two-card control — requires two different valid cards before access is granted.\r\n🔹 Effect: The system waits for a second credential (often within a timeout period).\r\n🔹 Use Case: High-security door or vaults where two people must be present (dual authentication).", "Two man rules", 64 },
                    { 8, "ACR_F_HOST_CBG	0x0400	\r\n🔹 Purpose: If online, check with the host server before granting access.\r\n🔹 Effect: The controller sends the access request to the host; the host can grant or deny.\r\n🔹 Use Case: Centralized decision-making — e.g., dynamic permissions, host-based rules, or temporary card revocations.\r\n🔹 Note: Often used together with ACR_FE_HOST_BYPASS in the extended flags.", "Host Decision", 1024 },
                    { 9, "ACR_F_HOST_SFT	0x0800	\r\n🔹 Purpose: Defines offline failover behavior.\r\n🔹 Effect: If the host is unreachable or times out, the controller proceeds to grant access using local data instead of denying.\r\n🔹 Use Case: Ensures continuity of access during temporary network outages.\r\n🔹 Note: Use with caution — enables access even when host verification fails.", "Offline Grant", 2048 },
                    { 10, "ACR_F_CIPHER	0x1000	\r\n🔹 Purpose: Enables Cipher mode (numeric keypad emulates card input).\r\n🔹 Effect: Allows the user to type their card number on a keypad instead of presenting a physical card.\r\n🔹 Use Case: For environments with numeric-only access or backup credential entry.\r\n🔹 Reference: See command 1117 (trigger Specification) for keypad mapping.", "Cipher mode", 4096 },
                    { 11, "ACR_F_LOG_EARLY	0x4000	\r\n🔹 Purpose: Log access transactions immediately upon grant — before door usage is confirmed.\r\n🔹 Effect: Creates an instant “Access Granted” event, then later logs “Used” or “Not Used.”\r\n🔹 Constraint: Automatically disabled if ACR_F_ALLUSED (0x0008) is set.\r\n🔹 Use Case: Real-time systems that require immediate event logging (e.g., monitoring dashboards).", "Log Early", 16384 },
                    { 12, "ACR_F_CNIF_WAIT	0x8000	\r\n🔹 Purpose: Changes “Card Not in File” behavior to show ‘Wait’ pattern instead of “Denied.”\r\n🔹 Effect: The reader shows a temporary wait indication (e.g., blinking LED) — useful when waiting for host validation.\r\n🔹 Use Case: Online reader with host delay — improves user feedback for cards that might soon be recognized after sync.\r\n🔹 Reference: See command 122 (Reader LED/Buzzer Function Specs).", "Wait for Card in file", 32768 }
                });

            migrationBuilder.InsertData(
                table: "door_mode",
                columns: new[] { "id", "description", "name", "value" },
                values: new object[,]
                {
                    { 1, "Disable the ACR, no REX", "Disable", (short)1 },
                    { 2, "Unlock (unlimited access)", "Unlock", (short)2 },
                    { 3, "Locked (no access, REX active)", "Locked", (short)3 },
                    { 4, "facility code only", "facility code only", (short)4 },
                    { 5, "Card only", "Card only", (short)5 },
                    { 6, "PIN only", "PIN only", (short)6 },
                    { 7, "Card and PIN required", "Card and PIN", (short)7 },
                    { 8, "Card or PIN required", "Card or PIN", (short)8 }
                });

            migrationBuilder.InsertData(
                table: "door_spare_flag",
                columns: new[] { "id", "description", "name", "value" },
                values: new object[,]
                {
                    { 1, "ACR_FE_NOEXTEND	0x0001	\r\n🔹 Purpose: Prevents the “Extended door Held Open Timer” from being restarted when a new access is granted.\r\n🔹 Effect: If someone presents a valid credential while the door is already open, the extended hold timer won’t reset — the timer continues to count down.\r\n🔹 Use Case: High-traffic door where you don’t want repeated badge reads to keep the door open indefinitely.", "No extend", (short)1 },
                    { 2, "ACR_FE_NOPINCARD	0x0002	\r\n🔹 Purpose: Forces CARD-before-PIN entry sequence in “Card and PIN” mode.\r\n🔹 Effect: PIN entered before a card will be rejected.\r\n🔹 Use Case: Ensures consistent user behavior and security (e.g., requiring card tap first, then PIN entry).", "Card Before pin", (short)2 },
                    { 3, "ACR_FE_DFO_FLTR	0x0008	\r\n🔹 Purpose: Enables door Forced Open Filter.\r\n🔹 Effect: If the door opens within 3 seconds after it was last closed, the system will not treat it as a “door Forced Open” alarm.\r\n🔹 Use Case: Prevents nuisance alarms caused by door bounce, air pressure, or slow latch operation.", "door Force Filter", (short)8 },
                    { 4, "ACR_FE_NO_ARQ	0x0010	\r\n🔹 Purpose: Blocks all access requests.\r\n🔹 Effect: Every access attempt is automatically reported as “Access Denied – door Locked.”\r\n🔹 Use Case: Temporarily disables access (e.g., during lockdown, maintenance, or controlled shutdown).", "Blocked All Request", (short)16 },
                    { 5, "ACR_FE_SHNTRLY	0x0020	\r\n🔹 Purpose: Defines a Shunt Relay used for suppressing door alarms during unlock events.\r\n🔹 Effect: When the door is unlocked:\r\n • The shunt relay activates 5 ms before the strike relay.\r\n • It deactivates 1 second after the door closes or the held-open timer expires.\r\n🔹 Note: The dc_held field (door-held timer) must be > 1 for this to function.\r\n🔹 Use Case: Used when connecting to alarm panels or to bypass door contacts during unlocks.", "Shunt Relay", (short)32 },
                    { 6, "ACR_FE_FLOOR_PIN	0x0040	\r\n🔹 Purpose: Enables Floor Selection via PIN for elevators in “Card + PIN” mode.\r\n🔹 Effect: Instead of entering a PIN code, users enter the floor number after presenting a card.\r\n🔹 Use Case: Simplifies elevator access when using a single reader for multiple floors.", "Floor pin", (short)64 },
                    { 7, "ACR_FE_LINK_MODE	0x0080	\r\n🔹 Purpose: Indicates that the reader is in linking mode (pairing with another device or reader).\r\n🔹 Effect: Set when acr_mode = 29 (start linking) and cleared when:\r\n • The reader is successfully linked, or\r\n • acr_mode = 30 (abort) or timeout occurs.\r\n🔹 Use Case: Used for configuring dual-reader systems (e.g., in/out reader or linked elevator panels).", "Link mode", (short)128 },
                    { 8, "ACR_FE_DCARD	0x0100	\r\n🔹 Purpose: Enables Double Card mode.\r\n🔹 Effect: If the same valid card is presented twice within 5 seconds, it generates a double card event.\r\n🔹 Use Case: Used for dual-authentication or special functions (e.g., manager override, arming/disarming security zones).", "Double Card transaction", (short)256 },
                    { 9, "ACR_FE_OVERRIDE	0x0200	\r\n🔹 Purpose: Indicates that the reader is operating in a Temporary ACR mode Override.\r\n🔹 Effect: Typically means that a temporary mode (e.g., unlocked, lockdown) has been forced manually or by schedule.\r\n🔹 Use Case: Allows temporary override of normal access control logic without changing the base configuration.", "Allow mode Override", (short)512 },
                    { 10, "ACR_FE_CRD_OVR_EN	0x0400	\r\n🔹 Purpose: Enables Override credential.\r\n🔹 Effect: Specific credential (set in FFRM_FLD_ACCESSFLGS) can unlock the door even when it’s locked or access is disabled.\r\n🔹 Use Case: For emergency or master access cards (security, admin, fire personnel).", "Allow Super Card", (short)1024 },
                    { 11, "ACR_FE_ELV_DISABLE	0x0800	\r\n🔹 Purpose: Enables the ability to disable elevator floors using the offline_mode field.\r\n🔹 Effect: Only applies to Elevator type_desc 1 and 2 ACRs.\r\n🔹 Use Case: Temporarily disables access to certain floors when the elevator or reader is in offline or restricted mode.", "Disable Elevator", (short)2048 },
                    { 12, "ACR_FE_LINK_MODE_ALT	0x1000	\r\n🔹 Purpose: Similar to ACR_FE_LINK_MODE but for Alternate Reader Linking.\r\n🔹 Effect: Set when acr_mode = 32 (start link) and cleared when:\r\n  • Link successful, or\r\n  • acr_mode = 33 (abort) or timeout reached.\r\n🔹 Use Case: Used for alternate or backup reader pairing configurations.", "Alternate Reader Link", (short)4096 },
                    { 13, "🔹 Purpose: Extends the REX (Request-to-Exit) grant time while REX input is active.\r\n🔹 Effect: As long as the REX signal remains active (button pressed or motion detected), the door remains unlocked.\r\n🔹 Use Case: Ideal for long exit paths, large door, or slow-moving personnel.", "HOLD REX", (short)8192 },
                    { 14, "ACR_FE_HOST_BYPASS	0x4000	\r\n🔹 Purpose: Enables host decision bypass for online authorization.\r\n🔹 Effect: Requires ACR_F_HOST_CBG to also be enabled.\r\n 1. Controller sends credential to host for decision.\r\n 2. If host replies in time → uses host’s decision.\r\n 3. If no reply (timeout): controller checks its local database.\r\n  • If credential valid → grant.\r\n  • If not → deny.\r\n🔹 Use Case: For real-time validation in networked systems but with local fallback during communication loss.\r\n🔹 Supports: Card + PIN reader, online decision making, hybrid access control.", "HOST Decision", (short)16384 }
                });

            migrationBuilder.InsertData(
                table: "feature",
                columns: new[] { "id", "component_id", "name", "path" },
                values: new object[,]
                {
                    { 1, (short)1, "Dashboard", "/" },
                    { 2, (short)2, "transaction", "/event" },
                    { 3, (short)3, "location", "/location" },
                    { 4, (short)4, "Alerts", "/alert" },
                    { 5, (short)5, "operator", "" },
                    { 6, (short)6, "Devices", "" },
                    { 7, (short)7, "door", "/door" },
                    { 8, (short)8, "Card Holder", "/cardholder" },
                    { 9, (short)9, "Access Level", "/level" },
                    { 10, (short)10, "Access Area", "/area" },
                    { 11, (short)11, "time", "" },
                    { 12, (short)12, "trigger & procedure", "" },
                    { 13, (short)13, "Reports", "" },
                    { 14, (short)14, "Settings", "/setting" },
                    { 15, (short)15, "Maps", "/map" },
                    { 16, (short)16, "ControlPoint", "/control" },
                    { 17, (short)17, "MonitorPoint", "/monitor" },
                    { 18, (short)18, "monitor_group", "/monitorgroup" }
                });

            migrationBuilder.InsertData(
                table: "file_type",
                columns: new[] { "id", "name", "value" },
                values: new object[,]
                {
                    { 1, "Host Comm certificate file. The file should be in the same format currently used by the default certificate (PEM).", (short)0 },
                    { 2, "User defined file. This file can contain any type of data, up to one block in size. This file can have a name on the SCP up to 259 bytes.", (short)1 },
                    { 3, "License file. This file will be generated by HID and needed on only those products that require a license.", (short)2 },
                    { 4, "Peer certificate", (short)3 },
                    { 5, "OSDP file transfer files", (short)4 },
                    { 6, "Linq certificate", (short)7 },
                    { 7, "Over-Watch certificate", (short)8 },
                    { 8, "Web server certificate", (short)9 },
                    { 9, "HID Origo™ certificate", (short)10 },
                    { 10, "Aperio certificate", (short)11 },
                    { 11, "Host translator service for OEM cloud certificate", (short)12 },
                    { 12, "Driver trust store", (short)13 },
                    { 13, "802.1x TLS authentication", (short)16 },
                    { 14, "HTS OEM cloud authentication", (short)18 }
                });

            migrationBuilder.InsertData(
                table: "hardware_component",
                columns: new[] { "id", "model_no", "n_input", "n_output", "n_reader", "name" },
                values: new object[,]
                {
                    { 1, (short)196, (short)7, (short)4, (short)4, "HID Aero X1100" },
                    { 2, (short)193, (short)7, (short)4, (short)4, "HID Aero X100" },
                    { 3, (short)194, (short)19, (short)2, (short)0, "HID Aero X200" },
                    { 4, (short)195, (short)5, (short)12, (short)0, "HID Aero X300" },
                    { 5, (short)190, (short)7, (short)4, (short)2, "VertX V100" },
                    { 6, (short)191, (short)19, (short)2, (short)0, "VertX V200" },
                    { 7, (short)192, (short)5, (short)12, (short)0, "VertX V300" }
                });

            migrationBuilder.InsertData(
                table: "hardware_type",
                columns: new[] { "id", "component_id", "description", "name" },
                values: new object[,]
                {
                    { 1, (short)1, "HID Intelligent Controller", "HID Aero" },
                    { 2, (short)2, "HID Face Terminal", "HID Amico" }
                });

            migrationBuilder.InsertData(
                table: "input_mode",
                columns: new[] { "id", "description", "name", "value" },
                values: new object[,]
                {
                    { 1, "Normally closed, no End-Of-Line (EOL)", "Normally closed", (short)0 },
                    { 2, "Normally open, no EOL", "Normally open", (short)1 },
                    { 3, "Standard (ROM’ed) EOL: 1 kΩ normal, 2 kΩ active", "Standard EOL 1", (short)2 },
                    { 4, "Standard (ROM’ed) EOL: 2 kΩ normal, 1 kΩ active", "Standard EOL 2", (short)3 }
                });

            migrationBuilder.InsertData(
                table: "location",
                columns: new[] { "id", "component_id", "created_date", "description", "is_active", "location_name", "updated_date" },
                values: new object[] { 1, (short)1, new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Main location", true, "Main", new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.InsertData(
                table: "module_baudrate",
                columns: new[] { "id", "description", "name", "value" },
                values: new object[,]
                {
                    { 1, "9600", "9600", 9600 },
                    { 2, "19200", "19200", 19200 },
                    { 3, "38400", "38400", 38400 },
                    { 4, "115200", "115200", 115200 }
                });

            migrationBuilder.InsertData(
                table: "module_protocol",
                columns: new[] { "id", "description", "name", "value" },
                values: new object[,]
                {
                    { 1, "HID Aero X100, X200 and X300 protocol", "Aero", (short)0 },
                    { 2, "VertX™ V100, V200 and V300 protocol", "VertX", (short)15 },
                    { 3, "Aperio", "Aperio", (short)16 }
                });

            migrationBuilder.InsertData(
                table: "monitor_group_command",
                columns: new[] { "id", "description", "name", "value" },
                values: new object[,]
                {
                    { 1, "If the mask count is zero, mask all monitor points and increment the mask count by one", "Access", (short)1 },
                    { 2, "Set mask count to arg1. If arg1 is zero, then all points get unmasked. If arg1 is not zero, then all points get masked.", "Override", (short)2 },
                    { 3, "Force Arm: If the mask count > 1 then decrement the mask count by 1. Otherwise, if the mask count is equal to 1, unmask all non-active monitor points and set the mask count to zero.", "Force Arm", (short)3 },
                    { 4, "If the mask count > 1 then decrement the mask count by one. Otherwise, if the mask count is equal to 1 and no monitor points are active, unmask all monitor points. and set the mask count to zero.", "Arm", (short)4 },
                    { 5, "If the mask count > 1 then decrement the mask count by one, otherwise if the mask count is 1 unmask all monitor points and set the mask count to zero", "Override arm", (short)5 }
                });

            migrationBuilder.InsertData(
                table: "monitor_group_type",
                columns: new[] { "id", "description", "name", "value" },
                values: new object[,]
                {
                    { 1, "", "Monitor Point", (short)1 },
                    { 2, "", "Forced Open", (short)2 },
                    { 3, "", "Held Open", (short)3 }
                });

            migrationBuilder.InsertData(
                table: "monitor_point_log_function",
                columns: new[] { "id", "description", "name", "value" },
                values: new object[,]
                {
                    { 1, "Logs all changes", "Logs all", (short)0 },
                    { 2, "Do not log contact change-of-state if masked", "No Masked", (short)1 },
                    { 3, "Do not log contact change-of-state if masked and no fault-to-fault changes", "No Mask & Fault to Fault", (short)2 }
                });

            migrationBuilder.InsertData(
                table: "monitor_point_mode",
                columns: new[] { "id", "description", "name", "value" },
                values: new object[,]
                {
                    { 1, "", "Normal mode (no exit or entry delay)", (short)0 },
                    { 2, "", "Non-latching mode", (short)1 },
                    { 3, "", "Latching mode", (short)2 }
                });

            migrationBuilder.InsertData(
                table: "multi_occupancy",
                columns: new[] { "id", "description", "name", "value" },
                values: new object[,]
                {
                    { 1, "Two or more not required in area", "Two or more not required in area", (short)0 },
                    { 2, "Two or more required", "Two or more required", (short)1 }
                });

            migrationBuilder.InsertData(
                table: "occupancy_control",
                columns: new[] { "id", "description", "name", "value" },
                values: new object[,]
                {
                    { 1, "Do not change current occupancy count", "Do not change current occupancy count", (short)0 },
                    { 2, "Change current occupancy to occ_set", "Change current occupancy to occ_set", (short)1 }
                });

            migrationBuilder.InsertData(
                table: "osdp_address",
                columns: new[] { "id", "description", "name", "value" },
                values: new object[,]
                {
                    { 1, "", "address 0", (short)0 },
                    { 2, "", "address 1", (short)32 },
                    { 3, "", "address 2", (short)64 },
                    { 4, "", "address 3", (short)96 }
                });

            migrationBuilder.InsertData(
                table: "osdp_baudrate",
                columns: new[] { "id", "description", "name", "value" },
                values: new object[,]
                {
                    { 1, "", "9600", (short)1 },
                    { 2, "", "19200", (short)2 },
                    { 3, "", "38400", (short)3 },
                    { 4, "", "115200", (short)4 },
                    { 5, "", "57600", (short)5 },
                    { 6, "", "230400", (short)6 }
                });

            migrationBuilder.InsertData(
                table: "output_mode",
                columns: new[] { "id", "description", "offline_mode", "relay_mode", "value" },
                values: new object[,]
                {
                    { 1, "Normal mode with Offline: No change", (short)0, (short)0, (short)0 },
                    { 2, "Inverted mode Offline: No change", (short)0, (short)1, (short)1 },
                    { 3, "Normal mode Offline: Inactive", (short)1, (short)0, (short)16 },
                    { 4, "Inverted mode Offline: Inactive", (short)1, (short)1, (short)17 },
                    { 5, "Normal mode Offline: Active", (short)2, (short)0, (short)32 },
                    { 6, "Inverted mode Offline: Active", (short)2, (short)1, (short)33 }
                });

            migrationBuilder.InsertData(
                table: "password_rule",
                columns: new[] { "id", "is_digit", "is_lower", "is_symbol", "is_upper", "len" },
                values: new object[] { 1, false, false, false, false, 4 });

            migrationBuilder.InsertData(
                table: "reader_configuration_mode",
                columns: new[] { "id", "description", "name", "value" },
                values: new object[,]
                {
                    { 1, "Single reader, controlling the door", "Single Reader", (short)0 },
                    { 2, "Paired reader, Master - this reader controls the door", "Paired reader, Master", (short)1 },
                    { 3, "Paired reader, Slave - this reader does not control door", "Paired reader, Slave", (short)2 },
                    { 4, "Turnstile Reader. Two modes selected by: n strike_t_min != strike_t_max (original implementation - an access grant pulses the strike output for 1 second) n strike_t_min == strike_t_max (pulses the strike output after a door open/close signal for each additional access grant if several grants are waiting)", "Turnstile Reader", (short)3 },
                    { 5, "Elevator, no floor select feedback", "Elevator, no floor", (short)4 },
                    { 6, "Elevator with floor select feedback", "Elevator with floor", (short)5 }
                });

            migrationBuilder.InsertData(
                table: "reader_out_configuration",
                columns: new[] { "id", "description", "name", "value" },
                values: new object[,]
                {
                    { 1, "Ignore data from alternate reader", "Ignore", (short)0 },
                    { 2, "Normal Access Reader (two read heads to the same processor)", "Normal", (short)1 }
                });

            migrationBuilder.InsertData(
                table: "relay_mode",
                columns: new[] { "id", "description", "name", "value" },
                values: new object[,]
                {
                    { 1, "Active is energized", "Normal", (short)0 },
                    { 2, "Active is de-energized", "Inverted", (short)1 }
                });

            migrationBuilder.InsertData(
                table: "relay_offline_mode",
                columns: new[] { "id", "description", "name", "value" },
                values: new object[,]
                {
                    { 1, "No Change", "No Change", (short)0 },
                    { 2, "Relay de-energized", "Inactive", (short)1 },
                    { 3, "Relay energized", "Active", (short)2 }
                });

            migrationBuilder.InsertData(
                table: "role",
                columns: new[] { "id", "component_id", "created_date", "name", "updated_date" },
                values: new object[] { 1, (short)1, new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Administrator", new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.InsertData(
                table: "scp_setting",
                columns: new[] { "id", "gmt_offset", "n_acr", "n_alvl", "n_area", "n_card", "n_cp", "n_hol", "n_mp", "n_mpg", "n_msp1_port", "n_proc", "n_sio", "n_transaction", "n_trgr", "n_tz" },
                values: new object[] { 1, (short)-25200, (short)64, (short)32000, (short)127, (short)200, (short)388, (short)255, (short)615, (short)128, (short)3, (short)1024, (short)33, 60000, (short)1024, (short)255 });

            migrationBuilder.InsertData(
                table: "strike_mode",
                columns: new[] { "id", "description", "name", "value" },
                values: new object[,]
                {
                    { 1, "Do not use! This would allow the strike to stay active for the entire strike time allowing the door to be opened multiple times.", "Normal", (short)0 },
                    { 2, "Deactivate strike when door opens", "Deactivate On Open", (short)1 },
                    { 3, "Deactivate strike on door close or strike_t_max expires", "Deactivate On Close", (short)2 },
                    { 4, "Used with ACR_S_OPEN or ACR_S_CLOSE, to select tailgate mode: pulse (strk_sio:strk_number+1) relay for each user expected to enter", "Tailgate", (short)16 }
                });

            migrationBuilder.InsertData(
                table: "system_configuration",
                columns: new[] { "id", "c_port", "c_type", "n_channel_id", "n_ports", "n_scp" },
                values: new object[] { 1, (short)3333, (short)7, (short)1, (short)1, (short)100 });

            migrationBuilder.InsertData(
                table: "timezone_command",
                columns: new[] { "id", "description", "name", "value" },
                values: new object[,]
                {
                    { 1, "Temporary Clear - Deactivate time Zone until it would normally change. Next interval change will clear the override.", "Temporary Clear", (short)1 },
                    { 2, "Temporary Set - Activate time Zone until it would normally change. Next interval change will clear the override.", "Temporary Set", (short)2 },
                    { 3, "Override Clear - Deactivate time Zone until next command 314", "Override Clear", (short)3 },
                    { 4, "Override Set - Activate time Zone until next command 314", "Override Set", (short)4 },
                    { 5, "Release time Zone (Return to Normal). Will take the time zone out of the temporary or override mode and put it in the proper state.", "Release", (short)5 },
                    { 6, "Refresh - causes time zone state to be logged to the transaction log. Commonly used for triggers.", "Refresh", (short)6 }
                });

            migrationBuilder.InsertData(
                table: "timezone_mode",
                columns: new[] { "id", "description", "name", "value" },
                values: new object[,]
                {
                    { 1, "The time zone is always inactive, regardless of the time zone interval specified or the holiday in effect.", "Off", (short)0 },
                    { 2, "The time zone is always active, regardless of the time zone interval specified or the holiday in effect.", "On", (short)1 },
                    { 3, "The time Zone state is decided using either the day MaskAsync or the Holiday MaskAsync. If the current day is specified as a Holiday, the state relies only on whether the Holiday MaskAsync flag for that Holiday is set (if today is Holiday 1, and the Holiday MaskAsync sets flag H1, then the state is active. If today is Holiday 1, and the Holiday MaskAsync does not have flag H1 set, then the state is inactive). holiday override the standard accessibility rules. If the current day is not specified as a Holiday, the time Zone is active or inactive depending on whether the current day/time falls within the day MaskAsync. If day MaskAsync is M-F, 8-5, the time Zone is active during those times, and inactive on the weekend and outside working hours.", "Scan", (short)2 },
                    { 4, "Scan time zone interval list and apply only if the date string in expTest matches the current date", "OneTimeEvent", (short)3 },
                    { 5, "This mode is similar to mode Scan mode, but instead of only checking the Holiday MaskAsync if it is a Holiday, and only checking the day MaskAsync if not, this mode checks both. If it is not a Holiday, this mode functions normally, only checking the day MaskAsync. If it is a Holiday, this mode performs a logical OR on the Holiday and day Masks. If either or both are active, the time Zone is active, otherwise if neither is active, the time Zone is inactive.", "Scan, Always Honor day of Week", (short)4 },
                    { 6, "This mode is similar to mode \"Scan, Always Honor day of Week\", but it performs a logical AND instead of a logical OR. If it is not a Holiday, this mode functions normally, only checking the day MaskAsync. If it is a Holiday, this mode is only active if BOTH the day MaskAsync and Holiday MaskAsync are active. If either one is inactive, the entire time Zone is inactive.", "Scan, Always Holiday and day of Week", (short)5 }
                });

            migrationBuilder.InsertData(
                table: "transaction_source",
                columns: new[] { "id", "name", "source", "value" },
                values: new object[,]
                {
                    { 1, "SCP diagnostics", "hardware", (short)0 },
                    { 2, "SCP to HOST communication driver - not defined", "hardware", (short)1 },
                    { 3, "SCP local monitor points (tamper & power fault)", "hardware", (short)2 },
                    { 4, "SIO diagnostics", "modules", (short)3 },
                    { 5, "SIO communication driver", "modules", (short)4 },
                    { 6, "SIO cabinet tamper", "modules", (short)5 },
                    { 7, "SIO power monitor", "modules", (short)6 },
                    { 8, "Alarm monitor point", "Monitor Point", (short)7 },
                    { 9, "Output control point", "Control Point", (short)8 },
                    { 10, "Access Control Reader (ACR)", "door", (short)9 },
                    { 11, "ACR: reader tamper monitor", "door", (short)10 },
                    { 12, "ACR: door position sensor", "door", (short)11 },
                    { 13, "ACR: 1st \"Request to exit\" input", "door", (short)13 },
                    { 14, "ACR: 2nd \"Request to exit\" input", "door", (short)14 },
                    { 15, "time zone", "time zone", (short)15 },
                    { 16, "procedure (action list)", "procedure", (short)16 },
                    { 17, "trigger", "trigger", (short)17 },
                    { 18, "trigger variable", "trigger", (short)18 },
                    { 19, "Monitor point group", "Monitor point group", (short)19 },
                    { 20, "Access area", "Access area", (short)20 },
                    { 21, "ACR: the alternate reader's tamper monitor source_number", "door", (short)21 },
                    { 22, "Login Service", "hardware", (short)24 }
                });

            migrationBuilder.InsertData(
                table: "transaction_type",
                columns: new[] { "id", "name", "value" },
                values: new object[,]
                {
                    { 1, "System", (short)1 },
                    { 2, "SIO communication status report", (short)2 },
                    { 3, "Binary card data", (short)3 },
                    { 4, "Card data", (short)4 },
                    { 5, "Formatted card: facility code, card number ID, issue code", (short)5 },
                    { 6, "Formatted card: card number only", (short)6 },
                    { 7, "Change-of-state", (short)7 },
                    { 8, "Exit request", (short)8 },
                    { 9, "door status monitor change-of-state", (short)9 },
                    { 10, "procedure (command list) log", (short)10 },
                    { 11, "User command request report", (short)11 },
                    { 12, "Change of state: trigger variable, time zone, & triggers", (short)12 },
                    { 13, "door mode change", (short)13 },
                    { 14, "Monitor point group status change", (short)14 },
                    { 15, "Access area", (short)15 },
                    { 16, "Extended user command", (short)18 },
                    { 17, "Use limit report", (short)19 },
                    { 18, "Web activity", (short)20 },
                    { 19, "Specify tranTypeCardFull (0x05) instead", (short)21 },
                    { 20, "Specify tranTypeCardID (0x06) instead", (short)22 },
                    { 21, "Operating mode change", (short)24 },
                    { 22, "Elevator Floor status CoS", (short)26 },
                    { 23, "File Download status", (short)27 },
                    { 24, "Elevator Floor Access transaction", (short)29 },
                    { 25, "Specify tranTypeCardFull (0x05) instead", (short)37 },
                    { 26, "Specify tranTypeCardID (0x06) instead", (short)38 },
                    { 27, "Specify tranTypeCardFull (0x05) instead", (short)53 },
                    { 28, "door extended feature stateless transition", (short)64 },
                    { 29, "door extended feature change-of-state", (short)65 },
                    { 30, "Formatted card and user PIN was captured at an ACR", (short)66 }
                });

            migrationBuilder.InsertData(
                table: "trigger_command",
                columns: new[] { "id", "description", "name", "value" },
                values: new object[,]
                {
                    { 1, "Abort a delayed procedure", "Abort a delayed procedure", (short)1 },
                    { 2, "Execute actions with prefix 0", "Execute actions with prefix 0", (short)2 },
                    { 3, "Resume a delayed procedure and execute actions with prefix 0", "Resume a delayed procedure and execute actions with prefix 0", (short)3 },
                    { 4, "Execute actions with prefix 256", "Execute actions with prefix 256", (short)4 },
                    { 5, "Execute actions with prefix 512", "Execute actions with prefix 512", (short)5 },
                    { 6, "Execute actions with prefix 1024", "Execute actions with prefix 1024", (short)6 },
                    { 7, "Resume a delayed procedure and execute actions with prefix 256", "Resume a delayed procedure and execute actions with prefix 256", (short)7 },
                    { 8, "Resume a delayed procedure and execute actions with prefix 512", "Resume a delayed procedure and execute actions with prefix 512", (short)8 },
                    { 9, "Resume a delayed procedure and execute actions with prefix 1024", "Resume a delayed procedure and execute actions with prefix 1024", (short)9 }
                });

            migrationBuilder.InsertData(
                table: "access_level",
                columns: new[] { "id", "component_id", "created_date", "is_active", "location_id", "name", "updated_date" },
                values: new object[,]
                {
                    { 1, (short)1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, (short)1, "No Access", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, (short)2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, (short)1, "Full Access", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "area",
                columns: new[] { "id", "access_control", "area_flag", "component_id", "created_date", "is_active", "location_id", "multi_occ", "name", "occ_control", "occ_down", "occ_max", "occ_set", "occ_up", "updated_date" },
                values: new object[] { 1, (short)0, (short)0, (short)-1, new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, (short)1, (short)0, "Any Area", (short)0, (short)0, (short)0, (short)0, (short)0, new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.InsertData(
                table: "card_format",
                columns: new[] { "id", "bits", "ch_ln", "ch_loc", "component_id", "created_date", "facility", "fc_ln", "fc_loc", "flags", "function_id", "ic_ln", "ic_loc", "is_active", "location_id", "name", "offset", "pe_ln", "pe_loc", "po_ln", "po_loc", "updated_date" },
                values: new object[] { 1, (short)26, (short)16, (short)9, (short)0, new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), (short)-1, (short)0, (short)0, (short)0, (short)1, (short)0, (short)0, true, (short)1, "26 bits (No Fac)", (short)0, (short)13, (short)0, (short)13, (short)13, new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.InsertData(
                table: "feature_role",
                columns: new[] { "feature_id", "role_id", "is_action", "is_allow", "is_create", "is_delete", "is_modify" },
                values: new object[,]
                {
                    { (short)1, (short)1, true, true, true, true, true },
                    { (short)2, (short)1, true, true, true, true, true },
                    { (short)3, (short)1, true, true, true, true, true },
                    { (short)4, (short)1, true, true, true, true, true },
                    { (short)5, (short)1, true, true, true, true, true },
                    { (short)6, (short)1, true, true, true, true, true },
                    { (short)7, (short)1, true, true, true, true, true },
                    { (short)8, (short)1, true, true, true, true, true },
                    { (short)9, (short)1, true, true, true, true, true },
                    { (short)10, (short)1, true, true, true, true, true },
                    { (short)11, (short)1, true, true, true, true, true },
                    { (short)12, (short)1, true, true, true, true, true },
                    { (short)13, (short)1, true, true, true, true, true },
                    { (short)14, (short)1, true, true, true, true, true },
                    { (short)15, (short)1, true, true, true, true, true },
                    { (short)16, (short)1, true, true, true, true, true },
                    { (short)17, (short)1, true, true, true, true, true },
                    { (short)18, (short)1, true, true, true, true, true }
                });

            migrationBuilder.InsertData(
                table: "operator",
                columns: new[] { "id", "component_id", "created_date", "email", "first_name", "image_path", "is_active", "last_name", "middle_name", "password", "phone", "role_id", "title", "updated_date", "user_id", "user_name" },
                values: new object[] { 1, (short)1, new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "support@honorsupplying.com", "Administrator", "", true, "", "", "2439iBIqejYGcodz6j0vGvyeI25eOrjMX3QtIhgVyo0M4YYmWbS+NmGwo0LLByUY", "", (short)1, "Mr.", new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Administrator", "admin" });

            migrationBuilder.InsertData(
                table: "sub_feature",
                columns: new[] { "id", "component_id", "feature_id", "name", "path" },
                values: new object[,]
                {
                    { 1, (short)1, (short)5, "operator", "/operator" },
                    { 2, (short)2, (short)5, "role", "/role" },
                    { 3, (short)3, (short)6, "hardware", "/hardware" },
                    { 4, (short)4, (short)6, "modules", "/modules" },
                    { 5, (short)5, (short)11, "Timezone", "/timezone" },
                    { 6, (short)6, (short)11, "holiday", "/holiday" },
                    { 7, (short)7, (short)11, "interval", "/interval" },
                    { 8, (short)8, (short)12, "trigger", "/trigger" },
                    { 9, (short)9, (short)12, "procedure", "/action" },
                    { 10, (short)10, (short)13, "transaction", "/transaction" },
                    { 11, (short)11, (short)13, "Audit Trail", "/audit" }
                });

            migrationBuilder.InsertData(
                table: "timezone",
                columns: new[] { "id", "active_time", "component_id", "created_date", "deactive_time", "is_active", "location_id", "mode", "name", "updated_date" },
                values: new object[] { 1, "", (short)1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", true, (short)1, (short)1, "Always", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "transaction_code",
                columns: new[] { "id", "description", "name", "transaction_type_value", "value" },
                values: new object[,]
                {
                    { 1, "SCP power-up diagnostics", "SCP power-up diagnostics", (short)1, (short)1 },
                    { 2, "Host communications offline", "Host communications offline", (short)1, (short)2 },
                    { 3, "Host communications online", "Host communications online", (short)1, (short)3 },
                    { 4, "transaction count exceeds the preset limit", "transaction count exceeds the preset limit", (short)1, (short)4 },
                    { 5, "Configuration database save complete", "Configuration database save complete", (short)1, (short)5 },
                    { 6, "Card database save complete", "Card database save complete", (short)1, (short)6 },
                    { 7, "Card database cleared due to SRAM buffer overflow", "Card database cleared due to SRAM buffer overflow", (short)1, (short)7 },
                    { 8, "Communication disabled (result of host command)", "Disabled", (short)2, (short)1 },
                    { 9, "Timeout (no/bad response from unit)", "Offline", (short)2, (short)2 },
                    { 10, "Invalid identification from SIO", "Offline", (short)2, (short)3 },
                    { 11, "command too long", "Offline", (short)2, (short)4 },
                    { 12, "Normal connection", "Online", (short)2, (short)5 },
                    { 13, "ser_num is address loaded (-1 = last record)", "hexLoad report", (short)2, (short)6 },
                    { 14, "Invalid card format", "Access denied", (short)3, (short)1 },
                    { 15, "Invalid card format, forward read", "Access denied", (short)4, (short)1 },
                    { 16, "Invalid card format, reverse read", "Access denied", (short)4, (short)2 },
                    { 17, "Access point \"locked\"", "Request rejected", (short)5, (short)1 },
                    { 18, "Access point \"unlocked\"", "Request accepted", (short)5, (short)2 },
                    { 19, "Invalid facility code", "Request rejected", (short)5, (short)3 },
                    { 20, "Invalid facility code extension", "Request rejected", (short)5, (short)4 },
                    { 21, "Not in card file", "Request rejected", (short)5, (short)5 },
                    { 22, "Invalid issue code", "Request rejected", (short)5, (short)6 },
                    { 23, "facility code verified, not used", "Request granted", (short)5, (short)7 },
                    { 24, "facility code verified, door used", "Request granted", (short)5, (short)8 },
                    { 25, "Asked for host approval, then timed out", "Access denied", (short)5, (short)9 },
                    { 26, "Reporting that this card is \"about to get access granted\"", "Reporting that this card is \"about to get access granted\"", (short)5, (short)10 },
                    { 27, "Count exceeded", "Access denied", (short)5, (short)11 },
                    { 28, "Asked for host approval, then host denied", "Access denied", (short)5, (short)12 },
                    { 29, "Airlock is busy", "Request rejected", (short)5, (short)13 },
                    { 30, "Deactivated card", "Request rejected", (short)6, (short)1 },
                    { 31, "Before activation date", "Request rejected", (short)6, (short)2 },
                    { 32, "After expiration date", "Request rejected", (short)6, (short)3 },
                    { 33, "Invalid time", "Request rejected", (short)6, (short)4 },
                    { 34, "Invalid PIN", "Request rejected", (short)6, (short)5 },
                    { 35, "Anti-passback violation", "Request rejected", (short)6, (short)6 },
                    { 36, "APB violation, not used", "Request granted", (short)6, (short)7 },
                    { 37, "APB violation, used", "Request granted", (short)6, (short)8 },
                    { 38, "Duress code detected", "Request rejected", (short)6, (short)9 },
                    { 39, "Duress, used", "Request granted", (short)6, (short)10 },
                    { 40, "Duress, not used", "Request granted", (short)6, (short)11 },
                    { 41, "Full test, not used", "Request granted", (short)6, (short)12 },
                    { 42, "Full test, used", "Request granted", (short)6, (short)13 },
                    { 43, "Never allowed at this reader (all Tz's = 0)", "Request denied", (short)6, (short)14 },
                    { 44, "No second card presented", "Request denied", (short)6, (short)15 },
                    { 45, "Occupancy limit reached", "Request denied", (short)6, (short)16 },
                    { 46, "The area is NOT enabled", "Request denied", (short)6, (short)17 },
                    { 47, "Use limit", "Request denied", (short)6, (short)18 },
                    { 48, "Used/not used transaction will follow", "Granting access", (short)6, (short)21 },
                    { 49, "No escort card presented", "Request rejected", (short)6, (short)24 },
                    { 50, "Reserved", "Reserved", (short)6, (short)25 },
                    { 51, "Reserved", "Reserved", (short)6, (short)26 },
                    { 52, "Reserved", "Reserved", (short)6, (short)27 },
                    { 53, "Airlock is busy", "Request rejected", (short)6, (short)29 },
                    { 54, "Incomplete CARD & PIN sequence", "Request rejected", (short)6, (short)30 },
                    { 55, "Double-card event", "Request granted", (short)6, (short)31 },
                    { 56, "Double-card event while in uncontrolled state (locked/unlocked)", "Request granted", (short)6, (short)32 },
                    { 57, "Requires escort, pending escort card", "Granting access", (short)6, (short)39 },
                    { 58, "Violates minimum occupancy count", "Request rejected", (short)6, (short)40 },
                    { 59, "Card pending at another reader", "Request rejected", (short)6, (short)41 },
                    { 60, "Card pending at another reader", "Request rejected", (short)66, (short)41 },
                    { 62, "Disconnected (from an input point ID)", "Disconnected", (short)7, (short)1 },
                    { 63, "Unknown (offline): no report from the ID", "Offline", (short)7, (short)2 },
                    { 64, "Secure (or deactivate relay)", "Secure", (short)7, (short)3 },
                    { 65, "Alarm (or activated relay: perm or temp)", "Alarm", (short)7, (short)4 },
                    { 66, "Fault", "Fault", (short)7, (short)5 },
                    { 67, "Exit delay in progress", "Exit delay in progress", (short)7, (short)6 },
                    { 68, "Entry delay in progress", "Entry delay in progress", (short)7, (short)7 },
                    { 69, "door use not verified", "Exit cycle", (short)8, (short)1 },
                    { 70, "door not used", "Exit cycle", (short)8, (short)2 },
                    { 71, "door used", "Exit cycle", (short)8, (short)3 },
                    { 72, "door use not verified", "Host initiated request", (short)8, (short)4 },
                    { 73, "door not used", "Host initiated request", (short)8, (short)5 },
                    { 74, "door used", "Host initiated request", (short)8, (short)6 },
                    { 75, "Started", "Exit cycle", (short)8, (short)9 },
                    { 76, "Disconnected", "Disconnected", (short)9, (short)1 },
                    { 77, "Unknown _RS bits: last known status", "Unknown _RS bits: last known status", (short)9, (short)2 },
                    { 78, "Secure", "Secure", (short)9, (short)3 },
                    { 79, "Alarm (forced, held open or both)", "Alarm", (short)9, (short)4 },
                    { 80, "Fault (fault type is encoded in door_status byte)", "Fault", (short)9, (short)5 },
                    { 81, "Cancel procedure (abort delay)", "Cancel procedure (abort delay)", (short)10, (short)1 },
                    { 82, "Execute procedure (start new)", "Execute procedure (start new)", (short)10, (short)2 },
                    { 83, "Resume procedure, if paused", "Resume procedure, if paused", (short)10, (short)3 },
                    { 84, "Execute procedure with prefix 256 actions", "Execute procedure with prefix 256 actions", (short)10, (short)4 },
                    { 85, "Execute procedure with prefix 512 actions", "Execute procedure with prefix 512 actions", (short)10, (short)5 },
                    { 86, "Execute procedure with prefix 1024 actions", "Execute procedure with prefix 1024 actions", (short)10, (short)6 },
                    { 87, "Resume procedure with prefix 256 actions", "Resume procedure with prefix 256 actions", (short)10, (short)7 },
                    { 88, "Resume procedure with prefix 512 actions", "Resume procedure with prefix 512 actions", (short)10, (short)8 },
                    { 89, "Resume procedure with prefix 1024 actions", "Resume procedure with prefix 1024 actions", (short)10, (short)9 },
                    { 90, "command was issued to procedure with no actions - (NOP)", "command was issued to procedure with no actions - (NOP)", (short)10, (short)10 },
                    { 91, "command entered by the user", "command entered by the user", (short)11, (short)10 },
                    { 92, "Became inactive", "Became inactive", (short)12, (short)1 },
                    { 93, "Became active", "Became active", (short)12, (short)2 },
                    { 94, "Disabled", "Disabled", (short)13, (short)1 },
                    { 95, "Unlocked", "Unlocked", (short)13, (short)2 },
                    { 96, "Locked (exit request enabled)", "Locked", (short)13, (short)3 },
                    { 97, "facility code only", "facility code only", (short)13, (short)4 },
                    { 98, "Card only", "Card only", (short)13, (short)5 },
                    { 99, "PIN only", "PIN only", (short)13, (short)6 },
                    { 100, "Card and PIN", "Card and PIN", (short)13, (short)7 },
                    { 101, "PIN or card", "PIN or card", (short)13, (short)8 },
                    { 102, "First disarm command executed (mask_count was 0, all MPs got masked)", "First disarm command executed", (short)14, (short)1 },
                    { 103, "Subsequent disarm command executed (mask_count incremented, MPs already masked)", "Subsequent disarm command executed", (short)14, (short)2 },
                    { 104, "Override command: armed (mask_count cleared, all points unmasked)", "Override command: armed", (short)14, (short)3 },
                    { 105, "Override command: disarmed (mask_count set, unmasked all points)", "Override command: disarmed", (short)14, (short)4 },
                    { 106, "Force arm command, MPG armed, (may have active zones, mask_count is now zero)", "Force arm command, MPG armed", (short)14, (short)5 },
                    { 107, "Force arm command, MPG not armed (mask_count decremented)", "Force arm command, MPG not armed", (short)14, (short)6 },
                    { 108, "Standard arm command, MPG armed (did not have active zones, mask_count is now zero)", "Standard arm command, MPG armed", (short)14, (short)7 },
                    { 109, "Standard arm command, MPG did not arm, (had active zones, mask_count unchanged)d", "Standard arm command, MPG did not arm", (short)14, (short)8 },
                    { 110, "Standard arm command, MPG still armed, (mask_count decremented)", "Standard arm command, MPG still armed", (short)14, (short)9 },
                    { 111, "Override arm command, MPG armed (mask_count is now zero)", "Override arm command, MPG armed", (short)14, (short)10 },
                    { 112, "Override arm command, MPG did not arm, (mask_count decremented)", "Override arm command, MPG did not arm", (short)14, (short)11 },
                    { 113, "Area disabled", "Area disabled", (short)15, (short)1 },
                    { 114, "Area enabled", "Area enabled", (short)15, (short)2 },
                    { 115, "Occupancy count reached zero", "Occupancy count reached zero", (short)15, (short)3 },
                    { 116, "Occupancy count reached the \"downward-limit\"", "Occupancy count reached the \"downward-limit\"", (short)15, (short)4 },
                    { 117, "Occupancy count reached the \"upward-limit\"", "Occupancy count reached the \"upward-limit\"", (short)15, (short)5 },
                    { 118, "Occupancy count reached the \"max-occupancy-limit\"", "Occupancy count reached the \"max-occupancy-limit\"", (short)15, (short)6 },
                    { 119, "Multi-occupancy mode changed", "Multi-occupancy mode changed", (short)15, (short)7 },
                    { 120, "Save home notes", "Save home notes", (short)20, (short)1 },
                    { 121, "Save network settings", "Save network settings", (short)20, (short)2 },
                    { 122, "Save host communication settings", "Save host communication settings", (short)20, (short)3 },
                    { 123, "Add user", "Add user", (short)20, (short)4 },
                    { 124, "DeleteAsync user", "DeleteAsync user", (short)20, (short)5 },
                    { 125, "Modify user", "Modify user", (short)20, (short)6 },
                    { 126, "Save password strength and session timer", "Save password strength and session timer", (short)20, (short)7 },
                    { 127, "Save web server options", "Save web server options", (short)20, (short)8 },
                    { 128, "Save time server settings", "Save time server settings", (short)20, (short)9 },
                    { 129, "Auto save timer settings", "Auto save timer settings", (short)20, (short)10 },
                    { 130, "Load certificate", "Load certificate", (short)20, (short)11 },
                    { 131, "Logged out by link", "Logged out by link", (short)20, (short)12 },
                    { 132, "Logged out by timeout", "Logged out by timeout", (short)20, (short)13 },
                    { 133, "Logged out by user", "Logged out by user", (short)20, (short)14 },
                    { 134, "Logged out by apply", "Logged out by apply", (short)20, (short)15 },
                    { 135, "Invalid login", "Invalid login", (short)20, (short)16 },
                    { 136, "Successful login", "Successful login", (short)20, (short)17 },
                    { 137, "Network diagnostic saved", "Network diagnostic saved", (short)20, (short)18 },
                    { 138, "Card DB size saved", "Card DB size saved", (short)20, (short)19 },
                    { 139, "Diagnostic page saved", "Diagnostic page saved", (short)20, (short)21 },
                    { 140, "Security options page saved", "Security options page saved", (short)20, (short)22 },
                    { 141, "Add-on package page saved", "Add-on package page saved", (short)20, (short)23 },
                    { 145, "Invalid login limit reached", "Invalid login limit reached", (short)20, (short)27 },
                    { 146, "firmware download initiated", "firmware download initiated", (short)20, (short)28 },
                    { 147, "Advanced networking routes saved", "Advanced networking routes saved", (short)20, (short)29 },
                    { 148, "Advanced networking reversion timer started", "Advanced networking reversion timer started", (short)20, (short)30 },
                    { 149, "Advanced networking reversion timer elapsed", "Advanced networking reversion timer elapsed", (short)20, (short)31 },
                    { 150, "Advanced networking route changes reverted", "Advanced networking route changes reverted", (short)20, (short)32 },
                    { 151, "Advanced networking route changes cleared", "Advanced networking route changes cleared", (short)20, (short)33 },
                    { 152, "Certificate generation started", "Certificate generation started", (short)20, (short)34 },
                    { 153, "Operating mode changed to mode 0", "Operating mode changed to mode 0", (short)24, (short)1 },
                    { 154, "Operating mode changed to mode 1", "Operating mode changed to mode 1", (short)24, (short)2 },
                    { 155, "Operating mode changed to mode 2", "Operating mode changed to mode 2", (short)24, (short)3 },
                    { 156, "Operating mode changed to mode 3", "Operating mode changed to mode 3", (short)24, (short)4 },
                    { 157, "Operating mode changed to mode 4", "Operating mode changed to mode 4", (short)24, (short)5 },
                    { 158, "Operating mode changed to mode 5", "Operating mode changed to mode 5", (short)24, (short)6 },
                    { 159, "Operating mode changed to mode 6", "Operating mode changed to mode 6", (short)24, (short)7 },
                    { 160, "Operating mode changed to mode 7", "Operating mode changed to mode 7", (short)24, (short)8 },
                    { 161, "Floor status is secure", "Secure", (short)26, (short)1 },
                    { 162, "Floor status is public", "Public", (short)26, (short)2 },
                    { 163, "Floor status is disabled (override)", "Disabled (override)", (short)26, (short)3 },
                    { 164, "File transfer success", "File transfer success", (short)27, (short)1 },
                    { 165, "File transfer error", "File transfer error", (short)27, (short)2 },
                    { 166, "File delete successful", "File delete successful", (short)27, (short)3 },
                    { 167, "File delete unsuccessful", "File delete unsuccessful", (short)27, (short)4 },
                    { 168, "OSDP file transfer complete (primary ACR) - look at source number for ACR number", "OSDP file transfer complete (primary ACR)", (short)27, (short)5 },
                    { 169, "OSDP file transfer error (primary ACR) - look at source number for ACR number", "OSDP file transfer error (primary ACR)", (short)27, (short)6 },
                    { 170, "OSDP file transfer complete (alternate ACR) - look at source number for ACR number", "OSDP file transfer complete (alternate ACR)", (short)27, (short)7 },
                    { 171, "OSDP file transfer error (alternate ACR) - look at source number for ACR number", "OSDP file transfer error (alternate ACR)", (short)27, (short)8 },
                    { 172, "Elevator access", "Elevator access", (short)29, (short)1 },
                    { 173, "Extended status updated", "Extended status updated", (short)64, (short)1 },
                    { 174, "Secure / Inactive", "Secure / Inactive", (short)65, (short)3 },
                    { 175, "Alarm / Active", "Alarm / Active", (short)65, (short)4 }
                });

            migrationBuilder.InsertData(
                table: "transaction_source_type",
                columns: new[] { "id", "transction_source_value", "transction_type_value" },
                values: new object[,]
                {
                    { 1, (short)0, (short)1 },
                    { 2, (short)0, (short)20 },
                    { 3, (short)0, (short)24 },
                    { 4, (short)0, (short)27 },
                    { 5, (short)2, (short)7 },
                    { 7, (short)4, (short)2 },
                    { 8, (short)5, (short)7 },
                    { 9, (short)6, (short)7 },
                    { 10, (short)7, (short)7 },
                    { 11, (short)8, (short)7 },
                    { 12, (short)9, (short)3 },
                    { 13, (short)9, (short)4 },
                    { 14, (short)9, (short)5 },
                    { 15, (short)9, (short)6 },
                    { 16, (short)9, (short)8 },
                    { 17, (short)9, (short)11 },
                    { 18, (short)9, (short)13 },
                    { 19, (short)9, (short)18 },
                    { 20, (short)9, (short)19 },
                    { 21, (short)9, (short)26 },
                    { 22, (short)9, (short)29 },
                    { 23, (short)9, (short)64 },
                    { 24, (short)9, (short)65 },
                    { 25, (short)9, (short)19 },
                    { 26, (short)10, (short)7 },
                    { 27, (short)11, (short)9 },
                    { 28, (short)13, (short)7 },
                    { 29, (short)14, (short)7 },
                    { 30, (short)15, (short)12 },
                    { 31, (short)16, (short)10 },
                    { 32, (short)17, (short)12 },
                    { 33, (short)18, (short)12 },
                    { 34, (short)19, (short)14 },
                    { 35, (short)20, (short)15 },
                    { 36, (short)21, (short)7 },
                    { 37, (short)24, (short)7 }
                });

            migrationBuilder.InsertData(
                table: "weak_password",
                columns: new[] { "id", "password_rule_id", "pattern" },
                values: new object[,]
                {
                    { 1, 1, "P@ssw0rd" },
                    { 2, 1, "password" },
                    { 3, 1, "admin" },
                    { 4, 1, "123456" }
                });

            migrationBuilder.InsertData(
                table: "operator_location",
                columns: new[] { "location_id", "operator_id", "id" },
                values: new object[] { (short)1, (short)1, 1 });

            migrationBuilder.CreateIndex(
                name: "IX_access_level_location_id",
                table: "access_level",
                column: "location_id");

            migrationBuilder.CreateIndex(
                name: "IX_access_level_component_access_level_id",
                table: "access_level_component",
                column: "access_level_id");

            migrationBuilder.CreateIndex(
                name: "IX_access_level_door_component_access_level_componentid",
                table: "access_level_door_component",
                column: "access_level_componentid");

            migrationBuilder.CreateIndex(
                name: "IX_action_location_id",
                table: "action",
                column: "location_id");

            migrationBuilder.CreateIndex(
                name: "IX_action_procedure_id",
                table: "action",
                column: "procedure_id");

            migrationBuilder.CreateIndex(
                name: "IX_area_location_id",
                table: "area",
                column: "location_id");

            migrationBuilder.CreateIndex(
                name: "IX_card_format_location_id",
                table: "card_format",
                column: "location_id");

            migrationBuilder.CreateIndex(
                name: "IX_cardholder_AccessLevelid",
                table: "cardholder",
                column: "AccessLevelid");

            migrationBuilder.CreateIndex(
                name: "IX_cardholder_location_id",
                table: "cardholder",
                column: "location_id");

            migrationBuilder.CreateIndex(
                name: "IX_cardholder_access_level_accessLevelid",
                table: "cardholder_access_level",
                column: "accessLevelid");

            migrationBuilder.CreateIndex(
                name: "IX_cardholder_access_level_holder_id",
                table: "cardholder_access_level",
                column: "holder_id");

            migrationBuilder.CreateIndex(
                name: "IX_cardholder_access_level_locationid",
                table: "cardholder_access_level",
                column: "locationid");

            migrationBuilder.CreateIndex(
                name: "IX_cardholder_additional_holder_id",
                table: "cardholder_additional",
                column: "holder_id");

            migrationBuilder.CreateIndex(
                name: "IX_control_point_location_id",
                table: "control_point",
                column: "location_id");

            migrationBuilder.CreateIndex(
                name: "IX_control_point_module_id",
                table: "control_point",
                column: "module_id");

            migrationBuilder.CreateIndex(
                name: "IX_credential_cardholderid",
                table: "credential",
                column: "cardholderid");

            migrationBuilder.CreateIndex(
                name: "IX_credential_location_id",
                table: "credential",
                column: "location_id");

            migrationBuilder.CreateIndex(
                name: "IX_days_in_week_component_id",
                table: "days_in_week",
                column: "component_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_door_antipassback_in",
                table: "door",
                column: "antipassback_in");

            migrationBuilder.CreateIndex(
                name: "IX_door_antipassback_out",
                table: "door",
                column: "antipassback_out");

            migrationBuilder.CreateIndex(
                name: "IX_door_hardware_mac",
                table: "door",
                column: "hardware_mac");

            migrationBuilder.CreateIndex(
                name: "IX_door_location_id",
                table: "door",
                column: "location_id");

            migrationBuilder.CreateIndex(
                name: "IX_door_sensor_id",
                table: "door",
                column: "sensor_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_door_strike_id",
                table: "door",
                column: "strike_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_feature_role_feature_id",
                table: "feature_role",
                column: "feature_id");

            migrationBuilder.CreateIndex(
                name: "IX_hardware_location_id",
                table: "hardware",
                column: "location_id");

            migrationBuilder.CreateIndex(
                name: "IX_hardware_access_level_access_levelid",
                table: "hardware_access_level",
                column: "access_levelid");

            migrationBuilder.CreateIndex(
                name: "IX_hardware_access_level_hardware_mac",
                table: "hardware_access_level",
                column: "hardware_mac");

            migrationBuilder.CreateIndex(
                name: "IX_hardware_credential_credentialid",
                table: "hardware_credential",
                column: "credentialid");

            migrationBuilder.CreateIndex(
                name: "IX_hardware_credential_hardware_credential_id",
                table: "hardware_credential",
                column: "hardware_credential_id");

            migrationBuilder.CreateIndex(
                name: "IX_holiday_location_id",
                table: "holiday",
                column: "location_id");

            migrationBuilder.CreateIndex(
                name: "IX_interval_component_id",
                table: "interval",
                column: "component_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_interval_location_id",
                table: "interval",
                column: "location_id");

            migrationBuilder.CreateIndex(
                name: "IX_module_hardware_mac",
                table: "module",
                column: "hardware_mac");

            migrationBuilder.CreateIndex(
                name: "IX_module_location_id",
                table: "module",
                column: "location_id");

            migrationBuilder.CreateIndex(
                name: "IX_monitor_group_hardware_mac",
                table: "monitor_group",
                column: "hardware_mac");

            migrationBuilder.CreateIndex(
                name: "IX_monitor_group_location_id",
                table: "monitor_group",
                column: "location_id");

            migrationBuilder.CreateIndex(
                name: "IX_monitor_group_list_monitor_group_id",
                table: "monitor_group_list",
                column: "monitor_group_id");

            migrationBuilder.CreateIndex(
                name: "IX_monitor_point_location_id",
                table: "monitor_point",
                column: "location_id");

            migrationBuilder.CreateIndex(
                name: "IX_monitor_point_module_id",
                table: "monitor_point",
                column: "module_id");

            migrationBuilder.CreateIndex(
                name: "IX_operator_role_id",
                table: "operator",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "IX_operator_location_operator_id",
                table: "operator_location",
                column: "operator_id");

            migrationBuilder.CreateIndex(
                name: "IX_procedure_Hardwareid",
                table: "procedure",
                column: "Hardwareid");

            migrationBuilder.CreateIndex(
                name: "IX_procedure_location_id",
                table: "procedure",
                column: "location_id");

            migrationBuilder.CreateIndex(
                name: "IX_reader_component_id",
                table: "reader",
                column: "component_id");

            migrationBuilder.CreateIndex(
                name: "IX_reader_location_id",
                table: "reader",
                column: "location_id");

            migrationBuilder.CreateIndex(
                name: "IX_reader_module_id",
                table: "reader",
                column: "module_id");

            migrationBuilder.CreateIndex(
                name: "IX_request_exit_component_id",
                table: "request_exit",
                column: "component_id");

            migrationBuilder.CreateIndex(
                name: "IX_request_exit_location_id",
                table: "request_exit",
                column: "location_id");

            migrationBuilder.CreateIndex(
                name: "IX_request_exit_module_id",
                table: "request_exit",
                column: "module_id");

            migrationBuilder.CreateIndex(
                name: "IX_sensor_location_id",
                table: "sensor",
                column: "location_id");

            migrationBuilder.CreateIndex(
                name: "IX_sensor_module_id",
                table: "sensor",
                column: "module_id");

            migrationBuilder.CreateIndex(
                name: "IX_strike_location_id",
                table: "strike",
                column: "location_id");

            migrationBuilder.CreateIndex(
                name: "IX_strike_module_id",
                table: "strike",
                column: "module_id");

            migrationBuilder.CreateIndex(
                name: "IX_sub_feature_feature_id",
                table: "sub_feature",
                column: "feature_id");

            migrationBuilder.CreateIndex(
                name: "IX_timezone_location_id",
                table: "timezone",
                column: "location_id");

            migrationBuilder.CreateIndex(
                name: "IX_timezone_interval_interval_id",
                table: "timezone_interval",
                column: "interval_id");

            migrationBuilder.CreateIndex(
                name: "IX_transaction_location_id",
                table: "transaction",
                column: "location_id");

            migrationBuilder.CreateIndex(
                name: "IX_transaction_code_transaction_type_value",
                table: "transaction_code",
                column: "transaction_type_value");

            migrationBuilder.CreateIndex(
                name: "IX_transaction_flag_transactionid",
                table: "transaction_flag",
                column: "transactionid");

            migrationBuilder.CreateIndex(
                name: "IX_transaction_source_type_transction_source_value",
                table: "transaction_source_type",
                column: "transction_source_value");

            migrationBuilder.CreateIndex(
                name: "IX_transaction_source_type_transction_type_value",
                table: "transaction_source_type",
                column: "transction_type_value");

            migrationBuilder.CreateIndex(
                name: "IX_trigger_hardwareid",
                table: "trigger",
                column: "hardwareid");

            migrationBuilder.CreateIndex(
                name: "IX_trigger_location_id",
                table: "trigger",
                column: "location_id");

            migrationBuilder.CreateIndex(
                name: "IX_trigger_procedure_id",
                table: "trigger",
                column: "procedure_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_trigger_tran_code_value",
                table: "trigger_tran_code",
                column: "value");

            migrationBuilder.CreateIndex(
                name: "IX_weak_password_password_rule_id",
                table: "weak_password",
                column: "password_rule_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "access_area_command");

            migrationBuilder.DropTable(
                name: "access_level_door_component");

            migrationBuilder.DropTable(
                name: "action");

            migrationBuilder.DropTable(
                name: "action_type");

            migrationBuilder.DropTable(
                name: "antipassback_mode");

            migrationBuilder.DropTable(
                name: "area_access_control");

            migrationBuilder.DropTable(
                name: "area_flag");

            migrationBuilder.DropTable(
                name: "card_format");

            migrationBuilder.DropTable(
                name: "cardholder_access_level");

            migrationBuilder.DropTable(
                name: "cardholder_additional");

            migrationBuilder.DropTable(
                name: "commnad_log");

            migrationBuilder.DropTable(
                name: "control_point");

            migrationBuilder.DropTable(
                name: "credential_flag");

            migrationBuilder.DropTable(
                name: "days_in_week");

            migrationBuilder.DropTable(
                name: "door_access_control_flag");

            migrationBuilder.DropTable(
                name: "door_mode");

            migrationBuilder.DropTable(
                name: "door_spare_flag");

            migrationBuilder.DropTable(
                name: "feature_role");

            migrationBuilder.DropTable(
                name: "file_type");

            migrationBuilder.DropTable(
                name: "hardware_access_level");

            migrationBuilder.DropTable(
                name: "hardware_component");

            migrationBuilder.DropTable(
                name: "hardware_credential");

            migrationBuilder.DropTable(
                name: "hardware_type");

            migrationBuilder.DropTable(
                name: "holiday");

            migrationBuilder.DropTable(
                name: "id_report");

            migrationBuilder.DropTable(
                name: "input_mode");

            migrationBuilder.DropTable(
                name: "module_baudrate");

            migrationBuilder.DropTable(
                name: "module_protocol");

            migrationBuilder.DropTable(
                name: "monitor_group_command");

            migrationBuilder.DropTable(
                name: "monitor_group_list");

            migrationBuilder.DropTable(
                name: "monitor_group_type");

            migrationBuilder.DropTable(
                name: "monitor_point");

            migrationBuilder.DropTable(
                name: "monitor_point_log_function");

            migrationBuilder.DropTable(
                name: "monitor_point_mode");

            migrationBuilder.DropTable(
                name: "multi_occupancy");

            migrationBuilder.DropTable(
                name: "occupancy_control");

            migrationBuilder.DropTable(
                name: "operator_location");

            migrationBuilder.DropTable(
                name: "osdp_address");

            migrationBuilder.DropTable(
                name: "osdp_baudrate");

            migrationBuilder.DropTable(
                name: "output_mode");

            migrationBuilder.DropTable(
                name: "reader");

            migrationBuilder.DropTable(
                name: "reader_configuration_mode");

            migrationBuilder.DropTable(
                name: "reader_out_configuration");

            migrationBuilder.DropTable(
                name: "refresh_token");

            migrationBuilder.DropTable(
                name: "relay_mode");

            migrationBuilder.DropTable(
                name: "relay_offline_mode");

            migrationBuilder.DropTable(
                name: "request_exit");

            migrationBuilder.DropTable(
                name: "scp_setting");

            migrationBuilder.DropTable(
                name: "strike_mode");

            migrationBuilder.DropTable(
                name: "sub_feature");

            migrationBuilder.DropTable(
                name: "system_configuration");

            migrationBuilder.DropTable(
                name: "timezone_command");

            migrationBuilder.DropTable(
                name: "timezone_interval");

            migrationBuilder.DropTable(
                name: "timezone_mode");

            migrationBuilder.DropTable(
                name: "transaction_code");

            migrationBuilder.DropTable(
                name: "transaction_flag");

            migrationBuilder.DropTable(
                name: "transaction_source_type");

            migrationBuilder.DropTable(
                name: "trigger_command");

            migrationBuilder.DropTable(
                name: "trigger_tran_code");

            migrationBuilder.DropTable(
                name: "weak_password");

            migrationBuilder.DropTable(
                name: "access_level_component");

            migrationBuilder.DropTable(
                name: "credential");

            migrationBuilder.DropTable(
                name: "monitor_group");

            migrationBuilder.DropTable(
                name: "operator");

            migrationBuilder.DropTable(
                name: "door");

            migrationBuilder.DropTable(
                name: "feature");

            migrationBuilder.DropTable(
                name: "interval");

            migrationBuilder.DropTable(
                name: "timezone");

            migrationBuilder.DropTable(
                name: "transaction");

            migrationBuilder.DropTable(
                name: "transaction_source");

            migrationBuilder.DropTable(
                name: "transaction_type");

            migrationBuilder.DropTable(
                name: "trigger");

            migrationBuilder.DropTable(
                name: "password_rule");

            migrationBuilder.DropTable(
                name: "cardholder");

            migrationBuilder.DropTable(
                name: "role");

            migrationBuilder.DropTable(
                name: "area");

            migrationBuilder.DropTable(
                name: "sensor");

            migrationBuilder.DropTable(
                name: "strike");

            migrationBuilder.DropTable(
                name: "procedure");

            migrationBuilder.DropTable(
                name: "access_level");

            migrationBuilder.DropTable(
                name: "module");

            migrationBuilder.DropTable(
                name: "hardware");

            migrationBuilder.DropTable(
                name: "location");
        }
    }
}
