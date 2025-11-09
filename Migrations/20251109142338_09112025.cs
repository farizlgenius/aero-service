using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HIDAeroService.Migrations
{
    /// <inheritdoc />
    public partial class _09112025 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccessAreas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
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
                    ComponentId = table.Column<short>(type: "smallint", nullable: false),
                    MacAddress = table.Column<string>(type: "text", nullable: false),
                    LocationId = table.Column<int>(type: "integer", nullable: false),
                    LocationName = table.Column<string>(type: "text", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessAreas", x => x.Id);
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
                    LocationId = table.Column<int>(type: "integer", nullable: false),
                    LocationName = table.Column<string>(type: "text", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessLevels", x => x.Id);
                    table.UniqueConstraint("AK_AccessLevels_ComponentId", x => x.ComponentId);
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
                    TagNo = table.Column<int>(type: "integer", nullable: false),
                    ScpId = table.Column<int>(type: "integer", nullable: false),
                    ScpMac = table.Column<string>(type: "text", nullable: true),
                    Command = table.Column<string>(type: "text", nullable: true),
                    CommandStatus = table.Column<char>(type: "character(1)", nullable: false),
                    NakReason = table.Column<string>(type: "text", nullable: true),
                    NakDescCode = table.Column<int>(type: "integer", nullable: false),
                    Uuid = table.Column<string>(type: "text", nullable: false),
                    ComponentId = table.Column<short>(type: "smallint", nullable: false),
                    MacAddress = table.Column<string>(type: "text", nullable: false),
                    LocationId = table.Column<int>(type: "integer", nullable: false),
                    LocationName = table.Column<string>(type: "text", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArCommandStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ArEvents",
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
                    LocationId = table.Column<int>(type: "integer", nullable: false),
                    LocationName = table.Column<string>(type: "text", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArEvents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ArScpStructureStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Ip = table.Column<string>(type: "text", nullable: false),
                    Mac = table.Column<string>(type: "text", nullable: false),
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
                    Uuid = table.Column<string>(type: "text", nullable: false),
                    ComponentId = table.Column<short>(type: "smallint", nullable: false),
                    MacAddress = table.Column<string>(type: "text", nullable: false),
                    LocationId = table.Column<int>(type: "integer", nullable: false),
                    LocationName = table.Column<string>(type: "text", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArScpStructureStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CardFormats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
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
                    Uuid = table.Column<string>(type: "text", nullable: false),
                    LocationId = table.Column<int>(type: "integer", nullable: false),
                    LocationName = table.Column<string>(type: "text", nullable: false),
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
                name: "CredentialFlag",
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
                    table.PrimaryKey("PK_CredentialFlag", x => x.Id);
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
                name: "DoorSpareFlags",
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
                    table.PrimaryKey("PK_DoorSpareFlags", x => x.Id);
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
                    LastSync = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Uuid = table.Column<string>(type: "text", nullable: false),
                    ComponentId = table.Column<short>(type: "smallint", nullable: false),
                    MacAddress = table.Column<string>(type: "text", nullable: false),
                    LocationId = table.Column<int>(type: "integer", nullable: false),
                    LocationName = table.Column<string>(type: "text", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hardwares", x => x.Id);
                    table.UniqueConstraint("AK_Hardwares_ComponentId", x => x.ComponentId);
                    table.UniqueConstraint("AK_Hardwares_MacAddress", x => x.MacAddress);
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
                    LocationId = table.Column<int>(type: "integer", nullable: false),
                    LocationName = table.Column<string>(type: "text", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Holidays", x => x.Id);
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
                    ComponentId = table.Column<short>(type: "smallint", nullable: false),
                    DaysDesc = table.Column<string>(type: "text", nullable: false),
                    StartTime = table.Column<string>(type: "text", nullable: false),
                    EndTime = table.Column<string>(type: "text", nullable: false),
                    Uuid = table.Column<string>(type: "text", nullable: false),
                    LocationId = table.Column<int>(type: "integer", nullable: false),
                    LocationName = table.Column<string>(type: "text", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Intervals", x => x.Id);
                    table.UniqueConstraint("AK_Intervals_ComponentId", x => x.ComponentId);
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
                name: "Operators",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    UserName = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    MiddleName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    Role = table.Column<string>(type: "text", nullable: false),
                    Phone = table.Column<string>(type: "text", nullable: false),
                    Image = table.Column<string>(type: "text", nullable: false),
                    Uuid = table.Column<string>(type: "text", nullable: false),
                    ComponentId = table.Column<short>(type: "smallint", nullable: false),
                    MacAddress = table.Column<string>(type: "text", nullable: false),
                    LocationId = table.Column<int>(type: "integer", nullable: false),
                    LocationName = table.Column<string>(type: "text", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operators", x => x.Id);
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
                    ComponentId = table.Column<short>(type: "smallint", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Mode = table.Column<short>(type: "smallint", nullable: false),
                    ActiveTime = table.Column<string>(type: "text", nullable: false),
                    DeactiveTime = table.Column<string>(type: "text", nullable: false),
                    Uuid = table.Column<string>(type: "text", nullable: false),
                    LocationId = table.Column<int>(type: "integer", nullable: false),
                    LocationName = table.Column<string>(type: "text", nullable: false),
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
                    AccessLevelId = table.Column<short>(type: "smallint", nullable: false),
                    Uuid = table.Column<string>(type: "text", nullable: false),
                    LocationId = table.Column<int>(type: "integer", nullable: false),
                    LocationName = table.Column<string>(type: "text", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardHolders", x => x.Id);
                    table.UniqueConstraint("AK_CardHolders_UserId", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_CardHolders_AccessLevels_AccessLevelId",
                        column: x => x.AccessLevelId,
                        principalTable: "AccessLevels",
                        principalColumn: "ComponentId");
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
                    LocationId = table.Column<int>(type: "integer", nullable: false),
                    LocationName = table.Column<string>(type: "text", nullable: false),
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
                });

            migrationBuilder.CreateTable(
                name: "DaysInWeek",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ComponentId = table.Column<short>(type: "smallint", nullable: false),
                    Sunday = table.Column<bool>(type: "boolean", nullable: false),
                    Monday = table.Column<bool>(type: "boolean", nullable: false),
                    Tuesday = table.Column<bool>(type: "boolean", nullable: false),
                    Wednesday = table.Column<bool>(type: "boolean", nullable: false),
                    Thursday = table.Column<bool>(type: "boolean", nullable: false),
                    Friday = table.Column<bool>(type: "boolean", nullable: false),
                    Saturday = table.Column<bool>(type: "boolean", nullable: false),
                    Uuid = table.Column<string>(type: "text", nullable: false),
                    LocationId = table.Column<int>(type: "integer", nullable: false),
                    LocationName = table.Column<string>(type: "text", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
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
                name: "TimeZoneIntervals",
                columns: table => new
                {
                    TimeZoneId = table.Column<short>(type: "smallint", nullable: false),
                    IntervalId = table.Column<short>(type: "smallint", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Uuid = table.Column<string>(type: "text", nullable: false),
                    LocationId = table.Column<int>(type: "integer", nullable: false),
                    LocationName = table.Column<string>(type: "text", nullable: false),
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
                    LocationId = table.Column<int>(type: "integer", nullable: false),
                    LocationName = table.Column<string>(type: "text", nullable: false),
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
                    LocationId = table.Column<int>(type: "integer", nullable: false),
                    LocationName = table.Column<string>(type: "text", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ControlPoints", x => x.Id);
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
                    LocationId = table.Column<int>(type: "integer", nullable: false),
                    LocationName = table.Column<string>(type: "text", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonitorPoints", x => x.Id);
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
                    LocationId = table.Column<int>(type: "integer", nullable: false),
                    LocationName = table.Column<string>(type: "text", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sensors", x => x.Id);
                    table.UniqueConstraint("AK_Sensors_ComponentId", x => x.ComponentId);
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
                    LocationId = table.Column<int>(type: "integer", nullable: false),
                    LocationName = table.Column<string>(type: "text", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Strikes", x => x.Id);
                    table.UniqueConstraint("AK_Strikes_ComponentId", x => x.ComponentId);
                    table.ForeignKey(
                        name: "FK_Strikes_Modules_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Modules",
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
                    LocationId = table.Column<int>(type: "integer", nullable: false),
                    LocationName = table.Column<string>(type: "text", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doors", x => x.Id);
                    table.UniqueConstraint("AK_Doors_ComponentId", x => x.ComponentId);
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
                    LocationId = table.Column<int>(type: "integer", nullable: false),
                    LocationName = table.Column<string>(type: "text", nullable: false),
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
                    LocationId = table.Column<int>(type: "integer", nullable: false),
                    LocationName = table.Column<string>(type: "text", nullable: false),
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
                        name: "FK_RequestExits_Modules_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Modules",
                        principalColumn: "ComponentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AccessLevels",
                columns: new[] { "Id", "ComponentId", "IsActive", "LocationId", "LocationName", "Name", "Uuid" },
                values: new object[,]
                {
                    { 1, (short)1, true, 1, "Main Location", "No Access", "00000000-0000-0000-0000-000000000001" },
                    { 2, (short)2, true, 1, "Main Location", "Full Access", "00000000-0000-0000-0000-000000000001" }
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
                table: "CardFormats",
                columns: new[] { "Id", "Bits", "ChLn", "ChLoc", "ComponentId", "CreatedDate", "Facility", "FcLn", "FcLoc", "Flags", "FunctionId", "IcLn", "IcLoc", "IsActive", "LocationId", "LocationName", "Name", "Offset", "PeLn", "PeLoc", "PoLn", "PoLoc", "UpdatedDate", "Uuid" },
                values: new object[] { 1, (short)26, (short)16, (short)9, (short)0, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (short)-1, (short)0, (short)0, (short)0, (short)1, (short)0, (short)0, true, 1, "Main Location", "26 Bits (No Fac)", (short)0, (short)13, (short)0, (short)13, (short)13, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "00000000-0000-0000-0000-000000000001" });

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
                table: "CredentialFlag",
                columns: new[] { "Id", "Description", "Name", "Value" },
                values: new object[,]
                {
                    { 1, "Active Credential Record", "Active Credential Record", (short)1 },
                    { 2, "Allow one free anti-passback pass", "Free One Antipassback", (short)2 },
                    { 3, "", "Anti-passback exempt", (short)4 },
                    { 4, "Use timing parameters for the disabled (ADA)", "Timing for disbled (ADA)", (short)8 },
                    { 5, "PIN Exempt for \"Card & PIN\" ACR mode", "Pin Exempt", (short)16 }
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
                table: "DoorSpareFlags",
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
                table: "MonitorPointModes",
                columns: new[] { "Id", "Description", "Name", "Value" },
                values: new object[,]
                {
                    { 1, "", "Normal mode (no exit or entry delay)", (short)0 },
                    { 2, "", "Non-latching mode", (short)1 },
                    { 3, "", "Latching mode", (short)2 }
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
                values: new object[] { 1, (short)-25200, (short)64, (short)32000, (short)0, (short)200, (short)388, (short)255, (short)615, (short)128, (short)3, (short)1024, (short)16, 60000, (short)1024, (short)255 });

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
                columns: new[] { "Id", "ActiveTime", "ComponentId", "DeactiveTime", "IsActive", "LocationId", "LocationName", "Mode", "Name", "Uuid" },
                values: new object[] { 1, "", (short)1, "", true, 1, "Main Location", (short)1, "Always", "00000000-0000-0000-0000-000000000001" });

            migrationBuilder.CreateIndex(
                name: "IX_AccessLevelDoorTimeZones_DoorId",
                table: "AccessLevelDoorTimeZones",
                column: "DoorId");

            migrationBuilder.CreateIndex(
                name: "IX_AccessLevelDoorTimeZones_TimeZoneId",
                table: "AccessLevelDoorTimeZones",
                column: "TimeZoneId");

            migrationBuilder.CreateIndex(
                name: "IX_CardHolderAdditional_CardHolderId",
                table: "CardHolderAdditional",
                column: "CardHolderId");

            migrationBuilder.CreateIndex(
                name: "IX_CardHolders_AccessLevelId",
                table: "CardHolders",
                column: "AccessLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_ControlPoints_ModuleId",
                table: "ControlPoints",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_Credentials_CardHolderId",
                table: "Credentials",
                column: "CardHolderId");

            migrationBuilder.CreateIndex(
                name: "IX_DaysInWeek_ComponentId",
                table: "DaysInWeek",
                column: "ComponentId",
                unique: true);

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
                name: "IX_Intervals_ComponentId",
                table: "Intervals",
                column: "ComponentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Modules_MacAddress",
                table: "Modules",
                column: "MacAddress");

            migrationBuilder.CreateIndex(
                name: "IX_MonitorPoints_ModuleId",
                table: "MonitorPoints",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_Readers_ComponentId",
                table: "Readers",
                column: "ComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_Readers_ModuleId",
                table: "Readers",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestExits_ComponentId",
                table: "RequestExits",
                column: "ComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestExits_ModuleId",
                table: "RequestExits",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_Sensors_ModuleId",
                table: "Sensors",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_Strikes_ModuleId",
                table: "Strikes",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeZoneIntervals_IntervalId",
                table: "TimeZoneIntervals",
                column: "IntervalId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccessAreas");

            migrationBuilder.DropTable(
                name: "AccessLevelDoorTimeZones");

            migrationBuilder.DropTable(
                name: "AntipassbackModes");

            migrationBuilder.DropTable(
                name: "ArCommandStatuses");

            migrationBuilder.DropTable(
                name: "ArEvents");

            migrationBuilder.DropTable(
                name: "ArScpStructureStatuses");

            migrationBuilder.DropTable(
                name: "CardFormats");

            migrationBuilder.DropTable(
                name: "CardHolderAdditional");

            migrationBuilder.DropTable(
                name: "Components");

            migrationBuilder.DropTable(
                name: "ControlPoints");

            migrationBuilder.DropTable(
                name: "CredentialFlag");

            migrationBuilder.DropTable(
                name: "DaysInWeek");

            migrationBuilder.DropTable(
                name: "DoorAccessControlFlags");

            migrationBuilder.DropTable(
                name: "DoorModes");

            migrationBuilder.DropTable(
                name: "DoorSpareFlags");

            migrationBuilder.DropTable(
                name: "HardwareAccessLevel");

            migrationBuilder.DropTable(
                name: "HardwareCredential");

            migrationBuilder.DropTable(
                name: "Holidays");

            migrationBuilder.DropTable(
                name: "InputModes");

            migrationBuilder.DropTable(
                name: "MonitorPointModes");

            migrationBuilder.DropTable(
                name: "MonitorPoints");

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
                name: "Credentials");

            migrationBuilder.DropTable(
                name: "Doors");

            migrationBuilder.DropTable(
                name: "Intervals");

            migrationBuilder.DropTable(
                name: "TimeZones");

            migrationBuilder.DropTable(
                name: "CardHolders");

            migrationBuilder.DropTable(
                name: "Sensors");

            migrationBuilder.DropTable(
                name: "Strikes");

            migrationBuilder.DropTable(
                name: "AccessLevels");

            migrationBuilder.DropTable(
                name: "Modules");

            migrationBuilder.DropTable(
                name: "Hardwares");
        }
    }
}
