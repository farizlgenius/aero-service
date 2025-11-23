using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HIDAeroService.Migrations
{
    /// <inheritdoc />
    public partial class _231120251 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccessAreaAccessControlOptions",
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
                    table.PrimaryKey("PK_AccessAreaAccessControlOptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AccessAreaCommandOptions",
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
                    table.PrimaryKey("PK_AccessAreaCommandOptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AntipassbackModes",
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
                    table.PrimaryKey("PK_AntipassbackModes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ArCommandStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Uuid = table.Column<string>(type: "text", nullable: false),
                    TagNo = table.Column<int>(type: "integer", nullable: false),
                    ScpId = table.Column<int>(type: "integer", nullable: false),
                    ScpMac = table.Column<string>(type: "text", nullable: true),
                    Command = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<char>(type: "character(1)", nullable: false),
                    NakReason = table.Column<string>(type: "text", nullable: true),
                    NakDescCode = table.Column<int>(type: "integer", nullable: false),
                    MacAddress = table.Column<string>(type: "text", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArCommandStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AreaFlagOptions",
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
                    table.PrimaryKey("PK_AreaFlagOptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CardFormats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Uuid = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ComponentId = table.Column<short>(type: "smallint", nullable: false),
                    Facility = table.Column<short>(type: "smallint", nullable: false),
                    Offset = table.Column<short>(type: "smallint", nullable: false),
                    FunctionId = table.Column<short>(type: "smallint", nullable: false),
                    Flags = table.Column<short>(type: "smallint", nullable: false),
                    Bits = table.Column<short>(type: "smallint", nullable: false),
                    PeLn = table.Column<short>(type: "smallint", nullable: false),
                    PeLoc = table.Column<short>(type: "smallint", nullable: false),
                    PoLn = table.Column<short>(type: "smallint", nullable: false),
                    PoLoc = table.Column<short>(type: "smallint", nullable: false),
                    FcLn = table.Column<short>(type: "smallint", nullable: false),
                    FcLoc = table.Column<short>(type: "smallint", nullable: false),
                    ChLn = table.Column<short>(type: "smallint", nullable: false),
                    ChLoc = table.Column<short>(type: "smallint", nullable: false),
                    IcLn = table.Column<short>(type: "smallint", nullable: false),
                    IcLoc = table.Column<short>(type: "smallint", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardFormats", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Components",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ModelNo = table.Column<short>(type: "smallint", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    nInput = table.Column<short>(type: "smallint", nullable: false),
                    nOutput = table.Column<short>(type: "smallint", nullable: false),
                    nReader = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Components", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CredentialFlagOptions",
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
                    table.PrimaryKey("PK_CredentialFlagOptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DoorAccessControlFlags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoorAccessControlFlags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DoorModes",
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
                    table.PrimaryKey("PK_DoorModes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DoorSpareFlagOption",
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
                    table.PrimaryKey("PK_DoorSpareFlagOption", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Feature",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ComponentId = table.Column<short>(type: "smallint", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feature", x => x.Id);
                    table.UniqueConstraint("AK_Feature_ComponentId", x => x.ComponentId);
                });

            migrationBuilder.CreateTable(
                name: "InputModes",
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
                    table.PrimaryKey("PK_InputModes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Intervals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Uuid = table.Column<string>(type: "text", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    ComponentId = table.Column<short>(type: "smallint", nullable: false),
                    DaysDesc = table.Column<string>(type: "text", nullable: false),
                    StartTime = table.Column<string>(type: "text", nullable: false),
                    EndTime = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Intervals", x => x.Id);
                    table.UniqueConstraint("AK_Intervals_ComponentId", x => x.ComponentId);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Uuid = table.Column<string>(type: "text", nullable: false),
                    ComponentId = table.Column<short>(type: "smallint", nullable: false),
                    LocationName = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Id);
                    table.UniqueConstraint("AK_Locations_ComponentId", x => x.ComponentId);
                });

            migrationBuilder.CreateTable(
                name: "MonitorPointModes",
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
                    table.PrimaryKey("PK_MonitorPointModes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MultiOccupancyOptions",
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
                    table.PrimaryKey("PK_MultiOccupancyOptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OccupancyControlOptions",
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
                    table.PrimaryKey("PK_OccupancyControlOptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OsdpAddresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Value = table.Column<short>(type: "smallint", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OsdpAddresses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OsdpBaudrates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Value = table.Column<short>(type: "smallint", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OsdpBaudrates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OutputModes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Value = table.Column<short>(type: "smallint", nullable: false),
                    OfflineMode = table.Column<short>(type: "smallint", nullable: false),
                    RelayMode = table.Column<short>(type: "smallint", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutputModes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OutputOfflineModes",
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
                    table.PrimaryKey("PK_OutputOfflineModes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReaderConfigurationModes",
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
                    table.PrimaryKey("PK_ReaderConfigurationModes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReaderOutConfigurations",
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
                    table.PrimaryKey("PK_ReaderOutConfigurations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokenAudits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Uuid = table.Column<Guid>(type: "uuid", nullable: false),
                    HashedToken = table.Column<string>(type: "text", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: false),
                    Action = table.Column<string>(type: "text", nullable: false),
                    Info = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokenAudits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RelayModes",
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
                    table.PrimaryKey("PK_RelayModes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ComponentId = table.Column<short>(type: "smallint", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                    table.UniqueConstraint("AK_Roles_ComponentId", x => x.ComponentId);
                });

            migrationBuilder.CreateTable(
                name: "StrikeModes",
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
                    table.PrimaryKey("PK_StrikeModes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SystemConfigurations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nPorts = table.Column<short>(type: "smallint", nullable: false),
                    nScp = table.Column<short>(type: "smallint", nullable: false),
                    nChannelId = table.Column<short>(type: "smallint", nullable: false),
                    cType = table.Column<short>(type: "smallint", nullable: false),
                    cPort = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemConfigurations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SystemSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nMsp1Port = table.Column<short>(type: "smallint", nullable: false),
                    nTransaction = table.Column<int>(type: "integer", nullable: false),
                    nSio = table.Column<short>(type: "smallint", nullable: false),
                    nMp = table.Column<short>(type: "smallint", nullable: false),
                    nCp = table.Column<short>(type: "smallint", nullable: false),
                    nAcr = table.Column<short>(type: "smallint", nullable: false),
                    nAlvl = table.Column<short>(type: "smallint", nullable: false),
                    nTrgr = table.Column<short>(type: "smallint", nullable: false),
                    nProc = table.Column<short>(type: "smallint", nullable: false),
                    GmtOffset = table.Column<short>(type: "smallint", nullable: false),
                    nTz = table.Column<short>(type: "smallint", nullable: false),
                    nHol = table.Column<short>(type: "smallint", nullable: false),
                    nMpg = table.Column<short>(type: "smallint", nullable: false),
                    nCard = table.Column<short>(type: "smallint", nullable: false),
                    nArea = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TimeZoneModes",
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
                    table.PrimaryKey("PK_TimeZoneModes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TimeZones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Uuid = table.Column<string>(type: "text", nullable: false),
                    ComponentId = table.Column<short>(type: "smallint", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Mode = table.Column<short>(type: "smallint", nullable: false),
                    ActiveTime = table.Column<string>(type: "text", nullable: false),
                    DeactiveTime = table.Column<string>(type: "text", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeZones", x => x.Id);
                    table.UniqueConstraint("AK_TimeZones_ComponentId", x => x.ComponentId);
                });

            migrationBuilder.CreateTable(
                name: "TransactionSources",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionSources", x => x.Id);
                    table.UniqueConstraint("AK_TransactionSources_Value", x => x.Value);
                });

            migrationBuilder.CreateTable(
                name: "TransactionTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionTypes", x => x.Id);
                    table.UniqueConstraint("AK_TransactionTypes_Value", x => x.Value);
                });

            migrationBuilder.CreateTable(
                name: "DaysInWeek",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Uuid = table.Column<string>(type: "text", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    ComponentId = table.Column<short>(type: "smallint", nullable: false),
                    Sunday = table.Column<bool>(type: "boolean", nullable: false),
                    Monday = table.Column<bool>(type: "boolean", nullable: false),
                    Tuesday = table.Column<bool>(type: "boolean", nullable: false),
                    Wednesday = table.Column<bool>(type: "boolean", nullable: false),
                    Thursday = table.Column<bool>(type: "boolean", nullable: false),
                    Friday = table.Column<bool>(type: "boolean", nullable: false),
                    Saturday = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DaysInWeek", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DaysInWeek_Intervals_ComponentId",
                        column: x => x.ComponentId,
                        principalTable: "Intervals",
                        principalColumn: "ComponentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AccessAreas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ComponentId = table.Column<short>(type: "smallint", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    MultiOccupancy = table.Column<short>(type: "smallint", nullable: false),
                    AccessControl = table.Column<short>(type: "smallint", nullable: false),
                    OccControl = table.Column<short>(type: "smallint", nullable: false),
                    OccSet = table.Column<short>(type: "smallint", nullable: false),
                    OccMax = table.Column<short>(type: "smallint", nullable: false),
                    OccUp = table.Column<short>(type: "smallint", nullable: false),
                    OccDown = table.Column<short>(type: "smallint", nullable: false),
                    AreaFlag = table.Column<short>(type: "smallint", nullable: false),
                    Uuid = table.Column<string>(type: "text", nullable: false),
                    LocationId = table.Column<short>(type: "smallint", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessAreas", x => x.Id);
                    table.UniqueConstraint("AK_AccessAreas_ComponentId", x => x.ComponentId);
                    table.ForeignKey(
                        name: "FK_AccessAreas_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "ComponentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AccessLevels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ComponentId = table.Column<short>(type: "smallint", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Uuid = table.Column<string>(type: "text", nullable: false),
                    LocationId = table.Column<short>(type: "smallint", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessLevels", x => x.Id);
                    table.UniqueConstraint("AK_AccessLevels_ComponentId", x => x.ComponentId);
                    table.ForeignKey(
                        name: "FK_AccessLevels_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "ComponentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AeroStructureStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Uuid = table.Column<string>(type: "text", nullable: false),
                    IpAddress = table.Column<string>(type: "text", nullable: false),
                    RecAllocTransaction = table.Column<int>(type: "integer", nullable: false),
                    RecAllocTimezone = table.Column<int>(type: "integer", nullable: false),
                    RecAllocHoliday = table.Column<int>(type: "integer", nullable: false),
                    RecAllocSioPort = table.Column<int>(type: "integer", nullable: false),
                    RecAllocMp = table.Column<int>(type: "integer", nullable: false),
                    RecAllocCp = table.Column<int>(type: "integer", nullable: false),
                    RecAllocAcr = table.Column<int>(type: "integer", nullable: false),
                    RecAllocAlvl = table.Column<int>(type: "integer", nullable: false),
                    RecAllocTrig = table.Column<int>(type: "integer", nullable: false),
                    RecAllocProc = table.Column<int>(type: "integer", nullable: false),
                    RecAllocMpg = table.Column<int>(type: "integer", nullable: false),
                    RecAllocArea = table.Column<int>(type: "integer", nullable: false),
                    RecAllocEal = table.Column<int>(type: "integer", nullable: false),
                    RecAllocCrdb = table.Column<int>(type: "integer", nullable: false),
                    RecAllocCardActive = table.Column<int>(type: "integer", nullable: false),
                    MacAddress = table.Column<string>(type: "text", nullable: false),
                    LocationId = table.Column<short>(type: "smallint", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AeroStructureStatuses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AeroStructureStatuses_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "ComponentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CardHolders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    MiddleName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    Sex = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Phone = table.Column<string>(type: "text", nullable: false),
                    Company = table.Column<string>(type: "text", nullable: false),
                    Department = table.Column<string>(type: "text", nullable: false),
                    Position = table.Column<string>(type: "text", nullable: false),
                    ImagePath = table.Column<string>(type: "text", nullable: false),
                    Uuid = table.Column<string>(type: "text", nullable: false),
                    LocationId = table.Column<short>(type: "smallint", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardHolders", x => x.Id);
                    table.UniqueConstraint("AK_CardHolders_UserId", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_CardHolders_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "ComponentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Date = table.Column<string>(type: "text", nullable: false),
                    Time = table.Column<string>(type: "text", nullable: false),
                    Serialnumber = table.Column<int>(type: "integer", nullable: false),
                    Source = table.Column<string>(type: "text", nullable: false),
                    SourceNo = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    TransactionCode = table.Column<double>(type: "double precision", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Additional = table.Column<string>(type: "text", nullable: false),
                    Uuid = table.Column<string>(type: "text", nullable: false),
                    ComponentId = table.Column<short>(type: "smallint", nullable: false),
                    MacAddress = table.Column<string>(type: "text", nullable: false),
                    LocationId = table.Column<short>(type: "smallint", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Events_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "ComponentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Hardwares",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Model = table.Column<string>(type: "text", nullable: false),
                    IpAddress = table.Column<string>(type: "text", nullable: false),
                    SerialNumber = table.Column<string>(type: "text", nullable: false),
                    IsUpload = table.Column<bool>(type: "boolean", nullable: false),
                    IsReset = table.Column<bool>(type: "boolean", nullable: false),
                    LastSync = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    Uuid = table.Column<string>(type: "text", nullable: false),
                    ComponentId = table.Column<short>(type: "smallint", nullable: false),
                    MacAddress = table.Column<string>(type: "text", nullable: false),
                    LocationId = table.Column<short>(type: "smallint", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hardwares", x => x.Id);
                    table.UniqueConstraint("AK_Hardwares_ComponentId", x => x.ComponentId);
                    table.UniqueConstraint("AK_Hardwares_MacAddress", x => x.MacAddress);
                    table.ForeignKey(
                        name: "FK_Hardwares_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "ComponentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Holidays",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ComponentId = table.Column<short>(type: "smallint", nullable: false),
                    Year = table.Column<short>(type: "smallint", nullable: false),
                    Month = table.Column<short>(type: "smallint", nullable: false),
                    Day = table.Column<short>(type: "smallint", nullable: false),
                    Extend = table.Column<short>(type: "smallint", nullable: false),
                    TypeMask = table.Column<short>(type: "smallint", nullable: false),
                    Uuid = table.Column<string>(type: "text", nullable: false),
                    LocationId = table.Column<short>(type: "smallint", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Holidays", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Holidays_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "ComponentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MonitorPointGroup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Uuid = table.Column<string>(type: "text", nullable: false),
                    ComponentId = table.Column<short>(type: "smallint", nullable: false),
                    MacAddress = table.Column<string>(type: "text", nullable: false),
                    LocationId = table.Column<short>(type: "smallint", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonitorPointGroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MonitorPointGroup_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "ComponentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FeatureRole",
                columns: table => new
                {
                    FeatureId = table.Column<short>(type: "smallint", nullable: false),
                    RoleId = table.Column<short>(type: "smallint", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false),
                    IsAllow = table.Column<bool>(type: "boolean", nullable: false),
                    IsWritable = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeatureRole", x => new { x.RoleId, x.FeatureId });
                    table.ForeignKey(
                        name: "FK_FeatureRole_Feature_FeatureId",
                        column: x => x.FeatureId,
                        principalTable: "Feature",
                        principalColumn: "ComponentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FeatureRole_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "ComponentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Operators",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ComponentId = table.Column<short>(type: "smallint", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    MiddleName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    Phone = table.Column<string>(type: "text", nullable: false),
                    ImagePath = table.Column<string>(type: "text", nullable: false),
                    RoleId = table.Column<short>(type: "smallint", nullable: false),
                    Uuid = table.Column<string>(type: "text", nullable: false),
                    LocationId = table.Column<short>(type: "smallint", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operators", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Operators_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "ComponentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Operators_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "ComponentId");
                });

            migrationBuilder.CreateTable(
                name: "TimeZoneIntervals",
                columns: table => new
                {
                    TimeZoneId = table.Column<short>(type: "smallint", nullable: false),
                    IntervalId = table.Column<short>(type: "smallint", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Uuid = table.Column<string>(type: "text", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeZoneIntervals", x => new { x.TimeZoneId, x.IntervalId });
                    table.ForeignKey(
                        name: "FK_TimeZoneIntervals_Intervals_IntervalId",
                        column: x => x.IntervalId,
                        principalTable: "Intervals",
                        principalColumn: "ComponentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TimeZoneIntervals_TimeZones_TimeZoneId",
                        column: x => x.TimeZoneId,
                        principalTable: "TimeZones",
                        principalColumn: "ComponentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TransactionCodes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<short>(type: "smallint", nullable: false),
                    TransactionTypeValue = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionCodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransactionCodes_TransactionTypes_TransactionTypeValue",
                        column: x => x.TransactionTypeValue,
                        principalTable: "TransactionTypes",
                        principalColumn: "Value",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TransactionSourceType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TransactionSourceValue = table.Column<short>(type: "smallint", nullable: false),
                    TransactionTypeValue = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionSourceType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransactionSourceType_TransactionSources_TransactionSourceV~",
                        column: x => x.TransactionSourceValue,
                        principalTable: "TransactionSources",
                        principalColumn: "Value",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TransactionSourceType_TransactionTypes_TransactionTypeValue",
                        column: x => x.TransactionTypeValue,
                        principalTable: "TransactionTypes",
                        principalColumn: "Value",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CardHolderAccessLevel",
                columns: table => new
                {
                    CardHolderId = table.Column<string>(type: "text", nullable: false),
                    AccessLevelId = table.Column<short>(type: "smallint", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardHolderAccessLevel", x => new { x.AccessLevelId, x.CardHolderId });
                    table.ForeignKey(
                        name: "FK_CardHolderAccessLevel_AccessLevels_AccessLevelId",
                        column: x => x.AccessLevelId,
                        principalTable: "AccessLevels",
                        principalColumn: "ComponentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CardHolderAccessLevel_CardHolders_CardHolderId",
                        column: x => x.CardHolderId,
                        principalTable: "CardHolders",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CardHolderAdditional",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CardHolderId = table.Column<int>(type: "integer", nullable: false),
                    HolderId = table.Column<string>(type: "text", nullable: false),
                    Additional = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardHolderAdditional", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CardHolderAdditional_CardHolders_CardHolderId",
                        column: x => x.CardHolderId,
                        principalTable: "CardHolders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Credentials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ComponentId = table.Column<short>(type: "smallint", nullable: false),
                    Flag = table.Column<short>(type: "smallint", nullable: false),
                    Bits = table.Column<int>(type: "integer", nullable: false),
                    IssueCode = table.Column<int>(type: "integer", nullable: false),
                    FacilityCode = table.Column<int>(type: "integer", nullable: false),
                    CardNo = table.Column<long>(type: "bigint", nullable: false),
                    Pin = table.Column<string>(type: "text", nullable: true),
                    ActiveDate = table.Column<string>(type: "text", nullable: false),
                    DeactiveDate = table.Column<string>(type: "text", nullable: false),
                    CardHolderId = table.Column<string>(type: "text", nullable: false),
                    Uuid = table.Column<string>(type: "text", nullable: false),
                    LocationId = table.Column<short>(type: "smallint", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Credentials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Credentials_CardHolders_CardHolderId",
                        column: x => x.CardHolderId,
                        principalTable: "CardHolders",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Credentials_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "ComponentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HardwareAccessLevel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HardwareAccessLevelId = table.Column<short>(type: "smallint", nullable: false),
                    AccessLevelId = table.Column<int>(type: "integer", nullable: false),
                    MacAddress = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HardwareAccessLevel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HardwareAccessLevel_AccessLevels_AccessLevelId",
                        column: x => x.AccessLevelId,
                        principalTable: "AccessLevels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HardwareAccessLevel_Hardwares_MacAddress",
                        column: x => x.MacAddress,
                        principalTable: "Hardwares",
                        principalColumn: "MacAddress",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Modules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Model = table.Column<string>(type: "text", nullable: false),
                    Address = table.Column<short>(type: "smallint", nullable: false),
                    Port = table.Column<short>(type: "smallint", nullable: false),
                    nInput = table.Column<short>(type: "smallint", nullable: false),
                    nOutput = table.Column<short>(type: "smallint", nullable: false),
                    nReader = table.Column<short>(type: "smallint", nullable: false),
                    Msp1No = table.Column<short>(type: "smallint", nullable: false),
                    BaudRate = table.Column<short>(type: "smallint", nullable: false),
                    nProtocol = table.Column<short>(type: "smallint", nullable: false),
                    nDialect = table.Column<short>(type: "smallint", nullable: false),
                    Uuid = table.Column<string>(type: "text", nullable: false),
                    ComponentId = table.Column<short>(type: "smallint", nullable: false),
                    MacAddress = table.Column<string>(type: "text", nullable: false),
                    LocationId = table.Column<short>(type: "smallint", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modules", x => x.Id);
                    table.UniqueConstraint("AK_Modules_ComponentId", x => x.ComponentId);
                    table.ForeignKey(
                        name: "FK_Modules_Hardwares_MacAddress",
                        column: x => x.MacAddress,
                        principalTable: "Hardwares",
                        principalColumn: "MacAddress",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Modules_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "ComponentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HardwareCredential",
                columns: table => new
                {
                    MacAddress = table.Column<string>(type: "text", nullable: false),
                    HardwareCredentialId = table.Column<short>(type: "smallint", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false),
                    CredentialId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HardwareCredential", x => new { x.MacAddress, x.HardwareCredentialId });
                    table.ForeignKey(
                        name: "FK_HardwareCredential_Credentials_CredentialId",
                        column: x => x.CredentialId,
                        principalTable: "Credentials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HardwareCredential_Hardwares_HardwareCredentialId",
                        column: x => x.HardwareCredentialId,
                        principalTable: "Hardwares",
                        principalColumn: "ComponentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ControlPoints",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ModuleId = table.Column<short>(type: "smallint", nullable: false),
                    OutputNo = table.Column<short>(type: "smallint", nullable: false),
                    RelayMode = table.Column<short>(type: "smallint", nullable: false),
                    OfflineMode = table.Column<short>(type: "smallint", nullable: false),
                    DefaultPulse = table.Column<short>(type: "smallint", nullable: false),
                    Uuid = table.Column<string>(type: "text", nullable: false),
                    ComponentId = table.Column<short>(type: "smallint", nullable: false),
                    MacAddress = table.Column<string>(type: "text", nullable: false),
                    LocationId = table.Column<short>(type: "smallint", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ControlPoints", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ControlPoints_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "ComponentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ControlPoints_Modules_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Modules",
                        principalColumn: "ComponentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MonitorPoints",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ModuleId = table.Column<short>(type: "smallint", nullable: false),
                    InputNo = table.Column<short>(type: "smallint", nullable: false),
                    InputMode = table.Column<short>(type: "smallint", nullable: false),
                    Debounce = table.Column<short>(type: "smallint", nullable: false),
                    HoldTime = table.Column<short>(type: "smallint", nullable: false),
                    LogFunction = table.Column<short>(type: "smallint", nullable: false),
                    MonitorPointMode = table.Column<short>(type: "smallint", nullable: false),
                    DelayEntry = table.Column<short>(type: "smallint", nullable: false),
                    DelayExit = table.Column<short>(type: "smallint", nullable: false),
                    IsMask = table.Column<bool>(type: "boolean", nullable: false),
                    Uuid = table.Column<string>(type: "text", nullable: false),
                    ComponentId = table.Column<short>(type: "smallint", nullable: false),
                    MacAddress = table.Column<string>(type: "text", nullable: false),
                    LocationId = table.Column<short>(type: "smallint", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonitorPoints", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MonitorPoints_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "ComponentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MonitorPoints_Modules_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Modules",
                        principalColumn: "ComponentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sensors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ModuleId = table.Column<short>(type: "smallint", nullable: false),
                    InputNo = table.Column<short>(type: "smallint", nullable: false),
                    InputMode = table.Column<short>(type: "smallint", nullable: false),
                    Debounce = table.Column<short>(type: "smallint", nullable: false),
                    HoldTime = table.Column<short>(type: "smallint", nullable: false),
                    DcHeld = table.Column<short>(type: "smallint", nullable: false),
                    Uuid = table.Column<string>(type: "text", nullable: false),
                    ComponentId = table.Column<short>(type: "smallint", nullable: false),
                    MacAddress = table.Column<string>(type: "text", nullable: false),
                    LocationId = table.Column<short>(type: "smallint", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sensors", x => x.Id);
                    table.UniqueConstraint("AK_Sensors_ComponentId", x => x.ComponentId);
                    table.ForeignKey(
                        name: "FK_Sensors_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "ComponentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sensors_Modules_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Modules",
                        principalColumn: "ComponentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Strikes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ModuleId = table.Column<short>(type: "smallint", nullable: false),
                    OutputNo = table.Column<short>(type: "smallint", nullable: false),
                    RelayMode = table.Column<short>(type: "smallint", nullable: false),
                    OfflineMode = table.Column<short>(type: "smallint", nullable: false),
                    StrkMax = table.Column<short>(type: "smallint", nullable: false),
                    StrkMin = table.Column<short>(type: "smallint", nullable: false),
                    StrkMode = table.Column<short>(type: "smallint", nullable: false),
                    Uuid = table.Column<string>(type: "text", nullable: false),
                    ComponentId = table.Column<short>(type: "smallint", nullable: false),
                    MacAddress = table.Column<string>(type: "text", nullable: false),
                    LocationId = table.Column<short>(type: "smallint", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Strikes", x => x.Id);
                    table.UniqueConstraint("AK_Strikes_ComponentId", x => x.ComponentId);
                    table.ForeignKey(
                        name: "FK_Strikes_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "ComponentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Strikes_Modules_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Modules",
                        principalColumn: "ComponentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Doors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    AccessConfig = table.Column<short>(type: "smallint", nullable: false),
                    PairDoorNo = table.Column<short>(type: "smallint", nullable: false),
                    ReaderOutConfiguration = table.Column<short>(type: "smallint", nullable: false),
                    StrkComponentId = table.Column<short>(type: "smallint", nullable: false),
                    SensorComponentId = table.Column<short>(type: "smallint", nullable: false),
                    CardFormat = table.Column<short>(type: "smallint", nullable: false),
                    AntiPassbackMode = table.Column<short>(type: "smallint", nullable: false),
                    AntiPassBackIn = table.Column<short>(type: "smallint", nullable: false),
                    AntiPassBackOut = table.Column<short>(type: "smallint", nullable: false),
                    SpareTags = table.Column<short>(type: "smallint", nullable: false),
                    AccessControlFlags = table.Column<short>(type: "smallint", nullable: false),
                    Mode = table.Column<short>(type: "smallint", nullable: false),
                    ModeDesc = table.Column<string>(type: "text", nullable: false),
                    OfflineMode = table.Column<short>(type: "smallint", nullable: false),
                    OfflineModeDesc = table.Column<string>(type: "text", nullable: false),
                    DefaultMode = table.Column<short>(type: "smallint", nullable: false),
                    DefaultModeDesc = table.Column<string>(type: "text", nullable: false),
                    DefaultLEDMode = table.Column<short>(type: "smallint", nullable: false),
                    PreAlarm = table.Column<short>(type: "smallint", nullable: false),
                    AntiPassbackDelay = table.Column<short>(type: "smallint", nullable: false),
                    StrkT2 = table.Column<short>(type: "smallint", nullable: false),
                    DcHeld2 = table.Column<short>(type: "smallint", nullable: false),
                    StrkFollowPulse = table.Column<short>(type: "smallint", nullable: false),
                    StrkFollowDelay = table.Column<short>(type: "smallint", nullable: false),
                    nExtFeatureType = table.Column<short>(type: "smallint", nullable: false),
                    IlPBSio = table.Column<short>(type: "smallint", nullable: false),
                    IlPBNumber = table.Column<short>(type: "smallint", nullable: false),
                    IlPBLongPress = table.Column<short>(type: "smallint", nullable: false),
                    IlPBOutSio = table.Column<short>(type: "smallint", nullable: false),
                    IlPBOutNum = table.Column<short>(type: "smallint", nullable: false),
                    DfOfFilterTime = table.Column<short>(type: "smallint", nullable: false),
                    MaskHeldOpen = table.Column<bool>(type: "boolean", nullable: false),
                    MaskForceOpen = table.Column<bool>(type: "boolean", nullable: false),
                    Uuid = table.Column<string>(type: "text", nullable: false),
                    ComponentId = table.Column<short>(type: "smallint", nullable: false),
                    MacAddress = table.Column<string>(type: "text", nullable: false),
                    LocationId = table.Column<short>(type: "smallint", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doors", x => x.Id);
                    table.UniqueConstraint("AK_Doors_ComponentId", x => x.ComponentId);
                    table.ForeignKey(
                        name: "FK_Doors_AccessAreas_AntiPassBackIn",
                        column: x => x.AntiPassBackIn,
                        principalTable: "AccessAreas",
                        principalColumn: "ComponentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Doors_AccessAreas_AntiPassBackOut",
                        column: x => x.AntiPassBackOut,
                        principalTable: "AccessAreas",
                        principalColumn: "ComponentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Doors_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "ComponentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Doors_Sensors_SensorComponentId",
                        column: x => x.SensorComponentId,
                        principalTable: "Sensors",
                        principalColumn: "ComponentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Doors_Strikes_StrkComponentId",
                        column: x => x.StrkComponentId,
                        principalTable: "Strikes",
                        principalColumn: "ComponentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AccessLevelDoorTimeZones",
                columns: table => new
                {
                    AccessLevelId = table.Column<short>(type: "smallint", nullable: false),
                    TimeZoneId = table.Column<short>(type: "smallint", nullable: false),
                    DoorId = table.Column<short>(type: "smallint", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessLevelDoorTimeZones", x => new { x.AccessLevelId, x.TimeZoneId, x.DoorId });
                    table.ForeignKey(
                        name: "FK_AccessLevelDoorTimeZones_AccessLevels_AccessLevelId",
                        column: x => x.AccessLevelId,
                        principalTable: "AccessLevels",
                        principalColumn: "ComponentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccessLevelDoorTimeZones_Doors_DoorId",
                        column: x => x.DoorId,
                        principalTable: "Doors",
                        principalColumn: "ComponentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccessLevelDoorTimeZones_TimeZones_TimeZoneId",
                        column: x => x.TimeZoneId,
                        principalTable: "TimeZones",
                        principalColumn: "ComponentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Readers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ModuleId = table.Column<short>(type: "smallint", nullable: false),
                    ReaderNo = table.Column<short>(type: "smallint", nullable: false),
                    DataFormat = table.Column<short>(type: "smallint", nullable: false),
                    KeypadMode = table.Column<short>(type: "smallint", nullable: false),
                    LedDriveMode = table.Column<short>(type: "smallint", nullable: false),
                    Direction = table.Column<short>(type: "smallint", nullable: false),
                    OsdpFlag = table.Column<bool>(type: "boolean", nullable: false),
                    OsdpBaudrate = table.Column<short>(type: "smallint", nullable: false),
                    OsdpDiscover = table.Column<short>(type: "smallint", nullable: false),
                    OsdpTracing = table.Column<short>(type: "smallint", nullable: false),
                    OsdpAddress = table.Column<short>(type: "smallint", nullable: false),
                    OsdpSecureChannel = table.Column<short>(type: "smallint", nullable: false),
                    Uuid = table.Column<string>(type: "text", nullable: false),
                    ComponentId = table.Column<short>(type: "smallint", nullable: false),
                    MacAddress = table.Column<string>(type: "text", nullable: false),
                    LocationId = table.Column<short>(type: "smallint", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Readers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Readers_Doors_ComponentId",
                        column: x => x.ComponentId,
                        principalTable: "Doors",
                        principalColumn: "ComponentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Readers_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "ComponentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Readers_Modules_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Modules",
                        principalColumn: "ComponentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RequestExits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ModuleId = table.Column<short>(type: "smallint", nullable: false),
                    InputNo = table.Column<short>(type: "smallint", nullable: false),
                    InputMode = table.Column<short>(type: "smallint", nullable: false),
                    Debounce = table.Column<short>(type: "smallint", nullable: false),
                    HoldTime = table.Column<short>(type: "smallint", nullable: false),
                    MaskTimeZone = table.Column<short>(type: "smallint", nullable: false),
                    Uuid = table.Column<string>(type: "text", nullable: false),
                    ComponentId = table.Column<short>(type: "smallint", nullable: false),
                    MacAddress = table.Column<string>(type: "text", nullable: false),
                    LocationId = table.Column<short>(type: "smallint", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestExits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RequestExits_Doors_ComponentId",
                        column: x => x.ComponentId,
                        principalTable: "Doors",
                        principalColumn: "ComponentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RequestExits_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "ComponentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RequestExits_Modules_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Modules",
                        principalColumn: "ComponentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AccessAreaAccessControlOptions",
                columns: new[] { "Id", "Description", "Name", "Value" },
                values: new object[,]
                {
                    { 1, "No Operation", "NOP", (short)0 },
                    { 2, "No One Can Access", "Disable area", (short)1 },
                    { 3, "Enable Area", "Enable area", (short)2 }
                });

            migrationBuilder.InsertData(
                table: "AccessAreaCommandOptions",
                columns: new[] { "Id", "Description", "Name", "Value" },
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
                table: "AntipassbackModes",
                columns: new[] { "Id", "Description", "Name", "Value" },
                values: new object[,]
                {
                    { 1, "Do not check or alter anti-passback location. No antipassback rules.", "None", (short)0 },
                    { 2, "Soft anti-passback: Accept any new location, change the user’s location to current reader, and generate an antipassback violation for an invalid entry.", "Soft Anti-passback", (short)1 },
                    { 3, "Hard anti-passback: Check user location, if a valid entry is made, change user’s location to new location. If an invalid entry is attempted, do not grant access.", "Hard Anti-passback", (short)2 },
                    { 4, "Reader-based anti-passback using the ACR’s last valid user. Verify it’s not the same user within the time parameter specified within apb_delay.", "Time-base Anti-passback - Last (Second)", (short)3 },
                    { 5, "Reader-based anti-passback using the access history from the cardholder database: Check user’s last ACR used, checks for same reader within a specified time (apb_delay). This requires the bSupportTimeApb flag be set in Command 1105: Access Database Specification.", "Time-base Anti-passback - History (Second)", (short)4 },
                    { 6, "Area based anti-passback: Check user’s current location, if it does not match the expected location then check the delay time (apb_delay). Change user’s location on entry. This requires the bSupportTimeApb flag be set in Command 1105: Access Database Specification.", "Area-base Anti-passback (Second)", (short)5 },
                    { 7, "Same as \"Time-base Anti-passback - Last (Second)\" but the apb_delay value is treated as minutes instead of seconds.", "Time-base Anti-passback - Last (Minute)", (short)6 },
                    { 8, "Same as \"Time-base Anti-passback - History (Second)\" but the apb_delay value is treated as minutes instead of seconds.", "Time-base Anti-passback - History (Minute)", (short)7 },
                    { 9, "Same as \"Area-base Anti-passback (Second)\" but the apb_delay value is treated as minutes instead of seconds.", "Area-base Anti-passback (Minute)", (short)8 }
                });

            migrationBuilder.InsertData(
                table: "AreaFlagOptions",
                columns: new[] { "Id", "Description", "Name", "Value" },
                values: new object[,]
                {
                    { 1, "Area can have open thresholds to only one other area", "Interlock", (short)1 },
                    { 2, "Just (O)ne (D)oor (O)nly is allowed to be open into this area (AREA_F_AIRLOCK must also be set)", "AirLock One Door Only", (short)2 }
                });

            migrationBuilder.InsertData(
                table: "CardFormats",
                columns: new[] { "Id", "Bits", "ChLn", "ChLoc", "ComponentId", "CreatedDate", "Facility", "FcLn", "FcLoc", "Flags", "FunctionId", "IcLn", "IcLoc", "IsActive", "Name", "Offset", "PeLn", "PeLoc", "PoLn", "PoLoc", "Uuid" },
                values: new object[] { 1, (short)26, (short)16, (short)9, (short)0, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (short)-1, (short)0, (short)0, (short)0, (short)1, (short)0, (short)0, true, "26 Bits (No Fac)", (short)0, (short)13, (short)0, (short)13, (short)13, "00000000-0000-0000-0000-000000000001" });

            migrationBuilder.InsertData(
                table: "Components",
                columns: new[] { "Id", "ModelNo", "Name", "nInput", "nOutput", "nReader" },
                values: new object[,]
                {
                    { 1, (short)196, "HID Aero X1100", (short)7, (short)4, (short)4 },
                    { 2, (short)193, "HID Aero X100", (short)7, (short)4, (short)4 },
                    { 3, (short)194, "HID Aero X200", (short)19, (short)2, (short)0 },
                    { 4, (short)195, "HID Aero X300", (short)5, (short)12, (short)0 },
                    { 5, (short)190, "VertX V100", (short)7, (short)4, (short)2 },
                    { 6, (short)191, "VertX V200", (short)19, (short)2, (short)0 },
                    { 7, (short)192, "VertX V300", (short)5, (short)12, (short)0 }
                });

            migrationBuilder.InsertData(
                table: "CredentialFlagOptions",
                columns: new[] { "Id", "Description", "Name", "Value" },
                values: new object[,]
                {
                    { 1, "Active Credential Record", "Active Credential Record", (short)1 },
                    { 2, "Allow one free anti-passback pass", "Free One Antipassback", (short)2 },
                    { 3, "", "Anti-passback exempt", (short)4 },
                    { 4, "Use timing parameters for the disabled (ADA)", "Timing for disbled (ADA)", (short)8 },
                    { 5, "PIN Exempt for \"Card & PIN\" ACR mode", "Pin Exempt", (short)16 },
                    { 6, "Do not change apb_loc", "No Change APB Location", (short)32 },
                    { 7, "Do not alter either the \"original\" or the \"current\" use count values", "No UpdateAsync Current Use Count", (short)64 },
                    { 8, "Do not alter the \"current\" use count but change the original use limit stored in the cardholder database", "No UpdateAsync Current Use Count but Change Use Limit", (short)128 }
                });

            migrationBuilder.InsertData(
                table: "DoorAccessControlFlags",
                columns: new[] { "Id", "Description", "Name", "Value" },
                values: new object[,]
                {
                    { 1, "ACR_F_DCR	0x0001	\r\n🔹 Purpose: Decrements a user’s “use counter” when they successfully access.\r\n🔹 Effect: Each valid access reduces their remaining allowed uses.\r\n🔹 Use Case: Temporary or limited-access credentials (e.g., one-time use visitor cards or tickets).", "Decrease Counter", 1 },
                    { 2, "ACR_F_CUL	0x0002	\r\n🔹 Purpose: Requires that the use limit is non-zero before granting access.\r\n🔹 Effect: If the use counter reaches zero, access is denied.\r\n🔹 Use Case: Works together with ACR_F_DCR for managing credentials with limited usage.", "Deny Zero", 2 },
                    { 3, "ACR_F_DRSS	0x0004	\r\n🔹 Purpose: Deny duress access requests.\r\n🔹 Effect: Normally, a duress PIN (like a special emergency PIN) grants access but logs a “duress” event. This flag changes that behavior — access is denied instead, but still logged.\r\n🔹 Use Case: High-security environments where duress entries should never open the door (only alert security).", "Denu Duress", 4 },
                    { 4, "ACR_F_ALLUSED	0x0008	\r\n🔹 Purpose: Treat all access grants as “used” immediately — don’t wait for door contact feedback.\r\n🔹 Effect: When access is granted, the system immediately logs it as used, even if the door sensor doesn’t open.\r\n🔹 Use Case: For systems with no door contact sensor, or for virtual readers (logical access).", "No Sensor Door", 8 },
                    { 5, "ACR_F_QEXIT	0x0010	\r\n🔹 Purpose: “Quiet Exit” — disables strike relay activation on REX (Request to Exit).\r\n🔹 Effect: When someone exits, the strike is not pulsed — useful for magnetic locks or silent egress doors.\r\n🔹 Use Case: Hospital wards, offices, or areas where audible clicks must be minimized.", "Quit Exit", 16 },
                    { 6, "ACR_F_FILTER	0x0020	\r\n🔹 Purpose: Filter out detailed door state change transactions (like every open/close event).\r\n🔹 Effect: Reduces event log noise — only key events (grants, denies) are logged.\r\n🔹 Use Case: Typically enabled in production. Disable only if you need fine-grained door state diagnostics.", "Door State Filter", 32 },
                    { 7, "ACR_F_2CARD	0x0040	\r\n🔹 Purpose: Enables two-card control — requires two different valid cards before access is granted.\r\n🔹 Effect: The system waits for a second credential (often within a timeout period).\r\n🔹 Use Case: High-security doors or vaults where two people must be present (dual authentication).", "Two man rules", 64 },
                    { 8, "ACR_F_HOST_CBG	0x0400	\r\n🔹 Purpose: If online, check with the host server before granting access.\r\n🔹 Effect: The controller sends the access request to the host; the host can grant or deny.\r\n🔹 Use Case: Centralized decision-making — e.g., dynamic permissions, host-based rules, or temporary card revocations.\r\n🔹 Note: Often used together with ACR_FE_HOST_BYPASS in the extended flags.", "Host Decision", 1024 },
                    { 9, "ACR_F_HOST_SFT	0x0800	\r\n🔹 Purpose: Defines offline failover behavior.\r\n🔹 Effect: If the host is unreachable or times out, the controller proceeds to grant access using local data instead of denying.\r\n🔹 Use Case: Ensures continuity of access during temporary network outages.\r\n🔹 Note: Use with caution — enables access even when host verification fails.", "Offline Grant", 2048 },
                    { 10, "ACR_F_CIPHER	0x1000	\r\n🔹 Purpose: Enables Cipher Mode (numeric keypad emulates card input).\r\n🔹 Effect: Allows the user to type their card number on a keypad instead of presenting a physical card.\r\n🔹 Use Case: For environments with numeric-only access or backup credential entry.\r\n🔹 Reference: See Command 1117 (Trigger Specification) for keypad mapping.", "Cipher Mode", 4096 },
                    { 11, "ACR_F_LOG_EARLY	0x4000	\r\n🔹 Purpose: Log access transactions immediately upon grant — before door usage is confirmed.\r\n🔹 Effect: Creates an instant “Access Granted” event, then later logs “Used” or “Not Used.”\r\n🔹 Constraint: Automatically disabled if ACR_F_ALLUSED (0x0008) is set.\r\n🔹 Use Case: Real-time systems that require immediate event logging (e.g., monitoring dashboards).", "Log Early", 16384 },
                    { 12, "ACR_F_CNIF_WAIT	0x8000	\r\n🔹 Purpose: Changes “Card Not in File” behavior to show ‘Wait’ pattern instead of “Denied.”\r\n🔹 Effect: The reader shows a temporary wait indication (e.g., blinking LED) — useful when waiting for host validation.\r\n🔹 Use Case: Online readers with host delay — improves user feedback for cards that might soon be recognized after sync.\r\n🔹 Reference: See Command 122 (Reader LED/Buzzer Function Specs).", "Wait for Card in file", 32768 }
                });

            migrationBuilder.InsertData(
                table: "DoorModes",
                columns: new[] { "Id", "Description", "Name", "Value" },
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
                table: "DoorSpareFlagOption",
                columns: new[] { "Id", "Description", "Name", "Value" },
                values: new object[,]
                {
                    { 1, "ACR_FE_NOEXTEND	0x0001	\r\n🔹 Purpose: Prevents the “Extended Door Held Open Timer” from being restarted when a new access is granted.\r\n🔹 Effect: If someone presents a valid credential while the door is already open, the extended hold timer won’t reset — the timer continues to count down.\r\n🔹 Use Case: High-traffic doors where you don’t want repeated badge reads to keep the door open indefinitely.", "No Extend", (short)1 },
                    { 2, "ACR_FE_NOPINCARD	0x0002	\r\n🔹 Purpose: Forces CARD-before-PIN entry sequence in “Card and PIN” mode.\r\n🔹 Effect: PIN entered before a card will be rejected.\r\n🔹 Use Case: Ensures consistent user behavior and security (e.g., requiring card tap first, then PIN entry).", "Card Before Pin", (short)2 },
                    { 3, "ACR_FE_DFO_FLTR	0x0008	\r\n🔹 Purpose: Enables Door Forced Open Filter.\r\n🔹 Effect: If the door opens within 3 seconds after it was last closed, the system will not treat it as a “Door Forced Open” alarm.\r\n🔹 Use Case: Prevents nuisance alarms caused by door bounce, air pressure, or slow latch operation.", "Door Force Filter", (short)8 },
                    { 4, "ACR_FE_NO_ARQ	0x0010	\r\n🔹 Purpose: Blocks all access requests.\r\n🔹 Effect: Every access attempt is automatically reported as “Access Denied – Door Locked.”\r\n🔹 Use Case: Temporarily disables access (e.g., during lockdown, maintenance, or controlled shutdown).", "Blocked All Request", (short)16 },
                    { 5, "ACR_FE_SHNTRLY	0x0020	\r\n🔹 Purpose: Defines a Shunt Relay used for suppressing door alarms during unlock events.\r\n🔹 Effect: When the door is unlocked:\r\n • The shunt relay activates 5 ms before the strike relay.\r\n • It deactivates 1 second after the door closes or the held-open timer expires.\r\n🔹 Note: The dc_held field (door-held timer) must be > 1 for this to function.\r\n🔹 Use Case: Used when connecting to alarm panels or to bypass door contacts during unlocks.", "Shunt Relay", (short)32 },
                    { 6, "ACR_FE_FLOOR_PIN	0x0040	\r\n🔹 Purpose: Enables Floor Selection via PIN for elevators in “Card + PIN” mode.\r\n🔹 Effect: Instead of entering a PIN code, users enter the floor number after presenting a card.\r\n🔹 Use Case: Simplifies elevator access when using a single reader for multiple floors.", "Floor Pin", (short)64 },
                    { 7, "ACR_FE_LINK_MODE	0x0080	\r\n🔹 Purpose: Indicates that the reader is in linking mode (pairing with another device or reader).\r\n🔹 Effect: Set when acr_mode = 29 (start linking) and cleared when:\r\n • The reader is successfully linked, or\r\n • acr_mode = 30 (abort) or timeout occurs.\r\n🔹 Use Case: Used for configuring dual-reader systems (e.g., in/out readers or linked elevator panels).", "Link Mode", (short)128 },
                    { 8, "ACR_FE_DCARD	0x0100	\r\n🔹 Purpose: Enables Double Card Mode.\r\n🔹 Effect: If the same valid card is presented twice within 5 seconds, it generates a double card event.\r\n🔹 Use Case: Used for dual-authentication or special functions (e.g., manager override, arming/disarming security zones).", "Double Card Event", (short)256 },
                    { 9, "ACR_FE_OVERRIDE	0x0200	\r\n🔹 Purpose: Indicates that the reader is operating in a Temporary ACR Mode Override.\r\n🔹 Effect: Typically means that a temporary mode (e.g., unlocked, lockdown) has been forced manually or by schedule.\r\n🔹 Use Case: Allows temporary override of normal access control logic without changing the base configuration.", "Allow Mode Override", (short)512 },
                    { 10, "ACR_FE_CRD_OVR_EN	0x0400	\r\n🔹 Purpose: Enables Override Credentials.\r\n🔹 Effect: Specific credentials (set in FFRM_FLD_ACCESSFLGS) can unlock the door even when it’s locked or access is disabled.\r\n🔹 Use Case: For emergency or master access cards (security, admin, fire personnel).", "Allow Super Card", (short)1024 },
                    { 11, "ACR_FE_ELV_DISABLE	0x0800	\r\n🔹 Purpose: Enables the ability to disable elevator floors using the offline_mode field.\r\n🔹 Effect: Only applies to Elevator Type 1 and 2 ACRs.\r\n🔹 Use Case: Temporarily disables access to certain floors when the elevator or reader is in offline or restricted mode.", "Disable Elevator", (short)2048 },
                    { 12, "ACR_FE_LINK_MODE_ALT	0x1000	\r\n🔹 Purpose: Similar to ACR_FE_LINK_MODE but for Alternate Reader Linking.\r\n🔹 Effect: Set when acr_mode = 32 (start link) and cleared when:\r\n  • Link successful, or\r\n  • acr_mode = 33 (abort) or timeout reached.\r\n🔹 Use Case: Used for alternate or backup reader pairing configurations.", "Alternate Reader Link", (short)4096 },
                    { 13, "🔹 Purpose: Extends the REX (Request-to-Exit) grant time while REX input is active.\r\n🔹 Effect: As long as the REX signal remains active (button pressed or motion detected), the door remains unlocked.\r\n🔹 Use Case: Ideal for long exit paths, large doors, or slow-moving personnel.", "HOLD REX", (short)8192 },
                    { 14, "ACR_FE_HOST_BYPASS	0x4000	\r\n🔹 Purpose: Enables host decision bypass for online authorization.\r\n🔹 Effect: Requires ACR_F_HOST_CBG to also be enabled.\r\n 1. Controller sends credential to host for decision.\r\n 2. If host replies in time → uses host’s decision.\r\n 3. If no reply (timeout): controller checks its local database.\r\n  • If credential valid → grant.\r\n  • If not → deny.\r\n🔹 Use Case: For real-time validation in networked systems but with local fallback during communication loss.\r\n🔹 Supports: Card + PIN readers, online decision making, hybrid access control.", "HOST Decision", (short)16384 }
                });

            migrationBuilder.InsertData(
                table: "Feature",
                columns: new[] { "Id", "ComponentId", "Name" },
                values: new object[,]
                {
                    { 1, (short)1, "Dashboard" },
                    { 2, (short)2, "Events" },
                    { 3, (short)3, "LocationId" },
                    { 4, (short)4, "Alerts" },
                    { 5, (short)5, "Operators" },
                    { 6, (short)6, "Device" },
                    { 7, (short)7, "Doors" },
                    { 8, (short)8, "Card Holder" },
                    { 9, (short)9, "Access Level" },
                    { 10, (short)10, "Access Area" },
                    { 11, (short)11, "Time" },
                    { 12, (short)12, "Trigger & Procedure" },
                    { 13, (short)13, "Report" },
                    { 14, (short)14, "Setting" },
                    { 15, (short)15, "Map" }
                });

            migrationBuilder.InsertData(
                table: "InputModes",
                columns: new[] { "Id", "Description", "Name", "Value" },
                values: new object[,]
                {
                    { 1, "Normally closed, no End-Of-Line (EOL)", "Normally closed", (short)0 },
                    { 2, "Normally open, no EOL", "Normally open", (short)1 },
                    { 3, "Standard (ROM’ed) EOL: 1 kΩ normal, 2 kΩ active", "Standard EOL 1", (short)2 },
                    { 4, "Standard (ROM’ed) EOL: 2 kΩ normal, 1 kΩ active", "Standard EOL 2", (short)3 }
                });

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "Id", "ComponentId", "CreatedDate", "Description", "IsActive", "LocationName", "Uuid" },
                values: new object[] { 1, (short)1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "All Location", true, "Any", "00000000-0000-0000-0000-000000000001" });

            migrationBuilder.InsertData(
                table: "MonitorPointModes",
                columns: new[] { "Id", "Description", "Name", "Value" },
                values: new object[,]
                {
                    { 1, "", "Normal mode (no exit or entry delay)", (short)0 },
                    { 2, "", "Non-latching mode", (short)1 },
                    { 3, "", "Latching mode", (short)2 }
                });

            migrationBuilder.InsertData(
                table: "MultiOccupancyOptions",
                columns: new[] { "Id", "Description", "Name", "Value" },
                values: new object[,]
                {
                    { 1, "Two or more not required in area", "Two or more not required in area", (short)0 },
                    { 2, "Two or more required", "Two or more required", (short)1 }
                });

            migrationBuilder.InsertData(
                table: "OccupancyControlOptions",
                columns: new[] { "Id", "Description", "Name", "Value" },
                values: new object[,]
                {
                    { 1, "Do not change current occupancy count", "Do not change current occupancy count", (short)0 },
                    { 2, "Change current occupancy to occ_set", "Change current occupancy to occ_set", (short)1 }
                });

            migrationBuilder.InsertData(
                table: "OsdpAddresses",
                columns: new[] { "Id", "Description", "Name", "Value" },
                values: new object[,]
                {
                    { 1, "", "Address 0", (short)0 },
                    { 2, "", "Address 1", (short)32 },
                    { 3, "", "Address 2", (short)64 },
                    { 4, "", "Address 3", (short)96 }
                });

            migrationBuilder.InsertData(
                table: "OsdpBaudrates",
                columns: new[] { "Id", "Description", "Name", "Value" },
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
                table: "OutputModes",
                columns: new[] { "Id", "Description", "OfflineMode", "RelayMode", "Value" },
                values: new object[,]
                {
                    { 1, "Normal Mode with Offline: No change", (short)0, (short)0, (short)0 },
                    { 2, "Inverted Mode Offline: No change", (short)0, (short)1, (short)1 },
                    { 3, "Normal Mode Offline: Inactive", (short)1, (short)0, (short)16 },
                    { 4, "Inverted Mode Offline: Inactive", (short)1, (short)1, (short)17 },
                    { 5, "Normal Mode Offline: Active", (short)2, (short)0, (short)32 },
                    { 6, "Inverted Mode Offline: Active", (short)2, (short)1, (short)33 }
                });

            migrationBuilder.InsertData(
                table: "OutputOfflineModes",
                columns: new[] { "Id", "Description", "Name", "Value" },
                values: new object[,]
                {
                    { 1, "No Change", "No Change", (short)0 },
                    { 2, "Relay de-energized", "Inactive", (short)1 },
                    { 3, "Relay energized", "Active", (short)2 }
                });

            migrationBuilder.InsertData(
                table: "ReaderConfigurationModes",
                columns: new[] { "Id", "Description", "Name", "Value" },
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
                table: "ReaderOutConfigurations",
                columns: new[] { "Id", "Description", "Name", "Value" },
                values: new object[,]
                {
                    { 1, "Ignore data from alternate reader", "Ignore", (short)0 },
                    { 2, "Normal Access Reader (two read heads to the same processor)", "Normal", (short)1 }
                });

            migrationBuilder.InsertData(
                table: "RelayModes",
                columns: new[] { "Id", "Description", "Name", "Value" },
                values: new object[,]
                {
                    { 1, "Active is energized", "Normal", (short)0 },
                    { 2, "Active is de-energized", "Inverted", (short)1 }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ComponentId", "CreatedDate", "Name" },
                values: new object[] { 1, (short)1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Administrator" });

            migrationBuilder.InsertData(
                table: "StrikeModes",
                columns: new[] { "Id", "Description", "Name", "Value" },
                values: new object[,]
                {
                    { 1, "Do not use! This would allow the strike to stay active for the entire strike time allowing the door to be opened multiple times.", "Normal", (short)0 },
                    { 2, "Deactivate strike when door opens", "Deactivate On Open", (short)1 },
                    { 3, "Deactivate strike on door close or strike_t_max expires", "Deactivate On Close", (short)2 },
                    { 4, "Used with ACR_S_OPEN or ACR_S_CLOSE, to select tailgate mode: pulse (strk_sio:strk_number+1) relay for each user expected to enter", "Tailgate", (short)16 }
                });

            migrationBuilder.InsertData(
                table: "SystemConfigurations",
                columns: new[] { "Id", "cPort", "cType", "nChannelId", "nPorts", "nScp" },
                values: new object[] { 1, (short)3333, (short)7, (short)1, (short)1, (short)100 });

            migrationBuilder.InsertData(
                table: "SystemSettings",
                columns: new[] { "Id", "GmtOffset", "nAcr", "nAlvl", "nArea", "nCard", "nCp", "nHol", "nMp", "nMpg", "nMsp1Port", "nProc", "nSio", "nTransaction", "nTrgr", "nTz" },
                values: new object[] { 1, (short)-25200, (short)64, (short)32000, (short)127, (short)200, (short)388, (short)255, (short)615, (short)128, (short)3, (short)1024, (short)16, 60000, (short)1024, (short)255 });

            migrationBuilder.InsertData(
                table: "TimeZoneModes",
                columns: new[] { "Id", "Description", "Name", "Value" },
                values: new object[,]
                {
                    { 1, "The time zone is always inactive, regardless of the time zone intervals specified or the holidays in effect.", "Off", (short)0 },
                    { 2, "The time zone is always active, regardless of the time zone intervals specified or the holidays in effect.", "On", (short)1 },
                    { 3, "The Time Zone state is decided using either the Day MaskAsync or the Holiday MaskAsync. If the current day is specified as a Holiday, the state relies only on whether the Holiday MaskAsync Flag for that Holiday is set (if today is Holiday 1, and the Holiday MaskAsync sets flag H1, then the state is active. If today is Holiday 1, and the Holiday MaskAsync does not have flag H1 set, then the state is inactive). Holidays override the standard accessibility rules. If the current day is not specified as a Holiday, the Time Zone is active or inactive depending on whether the current day/time falls within the Day MaskAsync. If Day MaskAsync is M-F, 8-5, the Time Zone is active during those times, and inactive on the weekend and outside working hours.", "Scan", (short)2 },
                    { 4, "Scan time zone interval list and apply only if the date string in expTest matches the current date", "OneTimeEvent", (short)3 },
                    { 5, "This mode is similar to mode Scan Mode, but instead of only checking the Holiday MaskAsync if it is a Holiday, and only checking the Day MaskAsync if not, this mode checks both. If it is not a Holiday, this mode functions normally, only checking the Day MaskAsync. If it is a Holiday, this mode performs a logical OR on the Holiday and Day Masks. If either or both are active, the Time Zone is active, otherwise if neither is active, the Time Zone is inactive.", "Scan, Always Honor Day of Week", (short)4 },
                    { 6, "This mode is similar to mode \"Scan, Always Honor Day of Week\", but it performs a logical AND instead of a logical OR. If it is not a Holiday, this mode functions normally, only checking the Day MaskAsync. If it is a Holiday, this mode is only active if BOTH the Day MaskAsync and Holiday MaskAsync are active. If either one is inactive, the entire Time Zone is inactive.", "Scan, Always Holiday and Day of Week", (short)5 }
                });

            migrationBuilder.InsertData(
                table: "TimeZones",
                columns: new[] { "Id", "ActiveTime", "ComponentId", "DeactiveTime", "IsActive", "Mode", "Name", "Uuid" },
                values: new object[] { 1, "", (short)1, "", true, (short)1, "Always", "00000000-0000-0000-0000-000000000001" });

            migrationBuilder.InsertData(
                table: "TransactionSources",
                columns: new[] { "Id", "Name", "Value" },
                values: new object[,]
                {
                    { 1, "SCP diagnostics", (short)0 },
                    { 2, "SCP to HOST communication driver - not defined", (short)1 },
                    { 3, "SCP local monitor points (tamper & power fault)", (short)2 },
                    { 4, "SIO diagnostics", (short)3 },
                    { 5, "SIO communication driver", (short)4 },
                    { 6, "SIO cabinet tamper", (short)5 },
                    { 7, "SIO power monitor", (short)6 },
                    { 8, "Alarm monitor point", (short)7 },
                    { 9, "Output control point", (short)8 },
                    { 10, "Access Control Reader (ACR)", (short)9 },
                    { 11, "ACR: reader tamper monitor", (short)10 },
                    { 12, "ACR: door position sensor", (short)11 },
                    { 13, "ACR: 1st \"Request to exit\" input", (short)13 },
                    { 14, "ACR: 2nd \"Request to exit\" input", (short)14 },
                    { 15, "Time zone", (short)15 },
                    { 16, "Procedure (action list)", (short)16 },
                    { 17, "Trigger", (short)17 },
                    { 18, "Trigger variable", (short)18 },
                    { 19, "Monitor point group", (short)19 },
                    { 20, "Access area", (short)20 },
                    { 21, "ACR: the alternate reader's tamper monitor source_number", (short)21 },
                    { 22, "LoginDto Service", (short)24 }
                });

            migrationBuilder.InsertData(
                table: "TransactionTypes",
                columns: new[] { "Id", "Name", "Value" },
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
                    { 9, "Door status monitor change-of-state", (short)9 },
                    { 10, "Procedure (command list) log", (short)10 },
                    { 11, "User command request report", (short)11 },
                    { 12, "Change of state: trigger variable, time zone, & triggers", (short)12 },
                    { 13, "ACR mode change", (short)13 },
                    { 14, "Monitor point group status change", (short)14 },
                    { 15, "Access area", (short)15 },
                    { 16, "Extended user command", (short)18 },
                    { 17, "Use limit report", (short)19 },
                    { 18, "Web activity", (short)20 },
                    { 19, "Specify tranTypeCardFull (0x05) instead", (short)21 },
                    { 20, "Specify tranTypeCardID (0x06) instead", (short)22 },
                    { 21, "Operating mode change", (short)24 },
                    { 22, "Elevator Floor Status CoS", (short)26 },
                    { 23, "File Download Status", (short)27 },
                    { 24, "Elevator Floor Access Transaction", (short)29 },
                    { 25, "Specify tranTypeCardFull (0x05) instead", (short)37 },
                    { 26, "Specify tranTypeCardID (0x06) instead", (short)38 },
                    { 27, "Specify tranTypeCardFull (0x05) instead", (short)53 },
                    { 28, "ACR extended feature stateless transition", (short)64 },
                    { 29, "ACR extended feature change-of-state", (short)65 },
                    { 30, "Formatted card and user PIN was captured at an ACR", (short)66 }
                });

            migrationBuilder.InsertData(
                table: "AccessAreas",
                columns: new[] { "Id", "AccessControl", "AreaFlag", "ComponentId", "CreatedDate", "IsActive", "LocationId", "MultiOccupancy", "Name", "OccControl", "OccDown", "OccMax", "OccSet", "OccUp", "Uuid" },
                values: new object[] { 1, (short)0, (short)0, (short)-1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, (short)1, (short)0, "Any Area", (short)0, (short)0, (short)0, (short)0, (short)0, "00000000-0000-0000-0000-000000000001" });

            migrationBuilder.InsertData(
                table: "AccessLevels",
                columns: new[] { "Id", "ComponentId", "IsActive", "LocationId", "Name", "Uuid" },
                values: new object[,]
                {
                    { 1, (short)1, true, (short)1, "No Access", "00000000-0000-0000-0000-000000000001" },
                    { 2, (short)2, true, (short)1, "Full Access", "00000000-0000-0000-0000-000000000001" }
                });

            migrationBuilder.InsertData(
                table: "FeatureRole",
                columns: new[] { "FeatureId", "RoleId", "Id", "IsAllow", "IsWritable" },
                values: new object[,]
                {
                    { (short)1, (short)1, 1, true, true },
                    { (short)2, (short)1, 2, true, true },
                    { (short)3, (short)1, 3, true, true },
                    { (short)4, (short)1, 4, true, true },
                    { (short)5, (short)1, 5, true, true },
                    { (short)6, (short)1, 6, true, true },
                    { (short)7, (short)1, 7, true, true },
                    { (short)8, (short)1, 8, true, true },
                    { (short)9, (short)1, 9, true, true },
                    { (short)10, (short)1, 10, true, true },
                    { (short)11, (short)1, 11, true, true },
                    { (short)12, (short)1, 12, true, true },
                    { (short)13, (short)1, 13, true, true },
                    { (short)14, (short)1, 14, true, true },
                    { (short)15, (short)1, 15, true, true }
                });

            migrationBuilder.InsertData(
                table: "Operators",
                columns: new[] { "Id", "ComponentId", "CreatedDate", "Email", "FirstName", "ImagePath", "IsActive", "LastName", "LocationId", "MiddleName", "Password", "Phone", "RoleId", "Title", "UserId", "Username", "Uuid" },
                values: new object[] { 1, (short)0, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "support@honorsupplying.com", "Administrator", "", true, "", (short)1, "", "2439iBIqejYGcodz6j0vGvyeI25eOrjMX3QtIhgVyo0M4YYmWbS+NmGwo0LLByUY", "", (short)1, "Mr.", "Administrator-001", "admin", "00000000-0000-0000-0000-000000000001" });

            migrationBuilder.InsertData(
                table: "TransactionCodes",
                columns: new[] { "Id", "Description", "Name", "TransactionTypeValue", "Value" },
                values: new object[,]
                {
                    { 1, "SCP power-up diagnostics", "SCP power-up diagnostics", (short)1, (short)1 },
                    { 2, "Host communications offline", "Host communications offline", (short)1, (short)2 },
                    { 3, "Host communications online", "Host communications online", (short)1, (short)3 },
                    { 4, "Transaction count exceeds the preset limit", "Transaction count exceeds the preset limit", (short)1, (short)4 },
                    { 5, "Configuration database save complete", "Configuration database save complete", (short)1, (short)5 },
                    { 6, "Card database save complete", "Card database save complete", (short)1, (short)6 },
                    { 7, "Card database cleared due to SRAM buffer overflow", "Card database cleared due to SRAM buffer overflow", (short)1, (short)7 },
                    { 8, "Communication disabled (result of host command)", "Disabled", (short)2, (short)1 },
                    { 9, "Timeout (no/bad response from unit)", "Offline", (short)2, (short)2 },
                    { 10, "Invalid identification from SIO", "Offline", (short)2, (short)3 },
                    { 11, "Command too long", "Offline", (short)2, (short)4 },
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
                    { 23, "Facility code verified, not used", "Request granted", (short)5, (short)7 },
                    { 24, "Facility code verified, door used", "Request granted", (short)5, (short)8 },
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
                    { 69, "Door use not verified", "Exit cycle", (short)8, (short)1 },
                    { 70, "Door not used", "Exit cycle", (short)8, (short)2 },
                    { 71, "Door used", "Exit cycle", (short)8, (short)3 },
                    { 72, "Door use not verified", "Host initiated request", (short)8, (short)4 },
                    { 73, "Door not used", "Host initiated request", (short)8, (short)5 },
                    { 74, "Door used", "Host initiated request", (short)8, (short)6 },
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
                    { 90, "Command was issued to procedure with no actions - (NOP)", "Command was issued to procedure with no actions - (NOP)", (short)10, (short)10 },
                    { 91, "Command entered by the user", "Command entered by the user", (short)11, (short)10 },
                    { 92, "Became inactive", "Became inactive", (short)12, (short)1 },
                    { 93, "Became active", "Became active", (short)12, (short)2 },
                    { 94, "Disabled", "Disabled", (short)13, (short)1 },
                    { 95, "Unlocked", "Unlocked", (short)13, (short)2 },
                    { 96, "Locked (exit request enabled)", "Locked", (short)13, (short)3 },
                    { 97, "Facility code only", "Facility code only", (short)13, (short)4 },
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
                    { 146, "Firmware download initiated", "Firmware download initiated", (short)20, (short)28 },
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
                table: "TransactionSourceType",
                columns: new[] { "Id", "TransactionSourceValue", "TransactionTypeValue" },
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

            migrationBuilder.CreateIndex(
                name: "IX_AccessAreas_LocationId",
                table: "AccessAreas",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_AccessLevelDoorTimeZones_DoorId",
                table: "AccessLevelDoorTimeZones",
                column: "DoorId");

            migrationBuilder.CreateIndex(
                name: "IX_AccessLevelDoorTimeZones_TimeZoneId",
                table: "AccessLevelDoorTimeZones",
                column: "TimeZoneId");

            migrationBuilder.CreateIndex(
                name: "IX_AccessLevels_LocationId",
                table: "AccessLevels",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_AeroStructureStatuses_LocationId",
                table: "AeroStructureStatuses",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_CardHolderAccessLevel_CardHolderId",
                table: "CardHolderAccessLevel",
                column: "CardHolderId");

            migrationBuilder.CreateIndex(
                name: "IX_CardHolderAdditional_CardHolderId",
                table: "CardHolderAdditional",
                column: "CardHolderId");

            migrationBuilder.CreateIndex(
                name: "IX_CardHolders_LocationId",
                table: "CardHolders",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_ControlPoints_LocationId",
                table: "ControlPoints",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_ControlPoints_ModuleId",
                table: "ControlPoints",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_Credentials_CardHolderId",
                table: "Credentials",
                column: "CardHolderId");

            migrationBuilder.CreateIndex(
                name: "IX_Credentials_LocationId",
                table: "Credentials",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_DaysInWeek_ComponentId",
                table: "DaysInWeek",
                column: "ComponentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Doors_AntiPassBackIn",
                table: "Doors",
                column: "AntiPassBackIn");

            migrationBuilder.CreateIndex(
                name: "IX_Doors_AntiPassBackOut",
                table: "Doors",
                column: "AntiPassBackOut");

            migrationBuilder.CreateIndex(
                name: "IX_Doors_LocationId",
                table: "Doors",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Doors_SensorComponentId",
                table: "Doors",
                column: "SensorComponentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Doors_StrkComponentId",
                table: "Doors",
                column: "StrkComponentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Events_LocationId",
                table: "Events",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_FeatureRole_FeatureId",
                table: "FeatureRole",
                column: "FeatureId");

            migrationBuilder.CreateIndex(
                name: "IX_HardwareAccessLevel_AccessLevelId",
                table: "HardwareAccessLevel",
                column: "AccessLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_HardwareAccessLevel_MacAddress",
                table: "HardwareAccessLevel",
                column: "MacAddress");

            migrationBuilder.CreateIndex(
                name: "IX_HardwareCredential_CredentialId",
                table: "HardwareCredential",
                column: "CredentialId");

            migrationBuilder.CreateIndex(
                name: "IX_HardwareCredential_HardwareCredentialId",
                table: "HardwareCredential",
                column: "HardwareCredentialId");

            migrationBuilder.CreateIndex(
                name: "IX_Hardwares_LocationId",
                table: "Hardwares",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Holidays_LocationId",
                table: "Holidays",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Intervals_ComponentId",
                table: "Intervals",
                column: "ComponentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Modules_LocationId",
                table: "Modules",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Modules_MacAddress",
                table: "Modules",
                column: "MacAddress");

            migrationBuilder.CreateIndex(
                name: "IX_MonitorPointGroup_LocationId",
                table: "MonitorPointGroup",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_MonitorPoints_LocationId",
                table: "MonitorPoints",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_MonitorPoints_ModuleId",
                table: "MonitorPoints",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_Operators_LocationId",
                table: "Operators",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Operators_RoleId",
                table: "Operators",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Readers_ComponentId",
                table: "Readers",
                column: "ComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_Readers_LocationId",
                table: "Readers",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Readers_ModuleId",
                table: "Readers",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestExits_ComponentId",
                table: "RequestExits",
                column: "ComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestExits_LocationId",
                table: "RequestExits",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestExits_ModuleId",
                table: "RequestExits",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_Sensors_LocationId",
                table: "Sensors",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Sensors_ModuleId",
                table: "Sensors",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_Strikes_LocationId",
                table: "Strikes",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Strikes_ModuleId",
                table: "Strikes",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeZoneIntervals_IntervalId",
                table: "TimeZoneIntervals",
                column: "IntervalId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionCodes_TransactionTypeValue",
                table: "TransactionCodes",
                column: "TransactionTypeValue");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionSourceType_TransactionSourceValue",
                table: "TransactionSourceType",
                column: "TransactionSourceValue");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionSourceType_TransactionTypeValue",
                table: "TransactionSourceType",
                column: "TransactionTypeValue");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccessAreaAccessControlOptions");

            migrationBuilder.DropTable(
                name: "AccessAreaCommandOptions");

            migrationBuilder.DropTable(
                name: "AccessLevelDoorTimeZones");

            migrationBuilder.DropTable(
                name: "AeroStructureStatuses");

            migrationBuilder.DropTable(
                name: "AntipassbackModes");

            migrationBuilder.DropTable(
                name: "ArCommandStatuses");

            migrationBuilder.DropTable(
                name: "AreaFlagOptions");

            migrationBuilder.DropTable(
                name: "CardFormats");

            migrationBuilder.DropTable(
                name: "CardHolderAccessLevel");

            migrationBuilder.DropTable(
                name: "CardHolderAdditional");

            migrationBuilder.DropTable(
                name: "Components");

            migrationBuilder.DropTable(
                name: "ControlPoints");

            migrationBuilder.DropTable(
                name: "CredentialFlagOptions");

            migrationBuilder.DropTable(
                name: "DaysInWeek");

            migrationBuilder.DropTable(
                name: "DoorAccessControlFlags");

            migrationBuilder.DropTable(
                name: "DoorModes");

            migrationBuilder.DropTable(
                name: "DoorSpareFlagOption");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "FeatureRole");

            migrationBuilder.DropTable(
                name: "HardwareAccessLevel");

            migrationBuilder.DropTable(
                name: "HardwareCredential");

            migrationBuilder.DropTable(
                name: "Holidays");

            migrationBuilder.DropTable(
                name: "InputModes");

            migrationBuilder.DropTable(
                name: "MonitorPointGroup");

            migrationBuilder.DropTable(
                name: "MonitorPointModes");

            migrationBuilder.DropTable(
                name: "MonitorPoints");

            migrationBuilder.DropTable(
                name: "MultiOccupancyOptions");

            migrationBuilder.DropTable(
                name: "OccupancyControlOptions");

            migrationBuilder.DropTable(
                name: "Operators");

            migrationBuilder.DropTable(
                name: "OsdpAddresses");

            migrationBuilder.DropTable(
                name: "OsdpBaudrates");

            migrationBuilder.DropTable(
                name: "OutputModes");

            migrationBuilder.DropTable(
                name: "OutputOfflineModes");

            migrationBuilder.DropTable(
                name: "ReaderConfigurationModes");

            migrationBuilder.DropTable(
                name: "ReaderOutConfigurations");

            migrationBuilder.DropTable(
                name: "Readers");

            migrationBuilder.DropTable(
                name: "RefreshTokenAudits");

            migrationBuilder.DropTable(
                name: "RelayModes");

            migrationBuilder.DropTable(
                name: "RequestExits");

            migrationBuilder.DropTable(
                name: "StrikeModes");

            migrationBuilder.DropTable(
                name: "SystemConfigurations");

            migrationBuilder.DropTable(
                name: "SystemSettings");

            migrationBuilder.DropTable(
                name: "TimeZoneIntervals");

            migrationBuilder.DropTable(
                name: "TimeZoneModes");

            migrationBuilder.DropTable(
                name: "TransactionCodes");

            migrationBuilder.DropTable(
                name: "TransactionSourceType");

            migrationBuilder.DropTable(
                name: "Feature");

            migrationBuilder.DropTable(
                name: "AccessLevels");

            migrationBuilder.DropTable(
                name: "Credentials");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Doors");

            migrationBuilder.DropTable(
                name: "Intervals");

            migrationBuilder.DropTable(
                name: "TimeZones");

            migrationBuilder.DropTable(
                name: "TransactionSources");

            migrationBuilder.DropTable(
                name: "TransactionTypes");

            migrationBuilder.DropTable(
                name: "CardHolders");

            migrationBuilder.DropTable(
                name: "AccessAreas");

            migrationBuilder.DropTable(
                name: "Sensors");

            migrationBuilder.DropTable(
                name: "Strikes");

            migrationBuilder.DropTable(
                name: "Modules");

            migrationBuilder.DropTable(
                name: "Hardwares");

            migrationBuilder.DropTable(
                name: "Locations");
        }
    }
}
