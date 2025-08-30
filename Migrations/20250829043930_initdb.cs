using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HIDAeroService.Migrations
{
    /// <inheritdoc />
    public partial class initdb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ArAcccessLevels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ComponentNo = table.Column<short>(type: "smallint", nullable: false),
                    TzAcr1 = table.Column<short>(type: "smallint", nullable: false),
                    TzAcr2 = table.Column<short>(type: "smallint", nullable: false),
                    TzAcr3 = table.Column<short>(type: "smallint", nullable: false),
                    TzAcr4 = table.Column<short>(type: "smallint", nullable: false),
                    TzAcr5 = table.Column<short>(type: "smallint", nullable: false),
                    TzAcr6 = table.Column<short>(type: "smallint", nullable: false),
                    TzAcr7 = table.Column<short>(type: "smallint", nullable: false),
                    TzAcr8 = table.Column<short>(type: "smallint", nullable: false),
                    TzAcr9 = table.Column<short>(type: "smallint", nullable: false),
                    TzAcr10 = table.Column<short>(type: "smallint", nullable: false),
                    TzAcr11 = table.Column<short>(type: "smallint", nullable: false),
                    TzAcr12 = table.Column<short>(type: "smallint", nullable: false),
                    TzAcr13 = table.Column<short>(type: "smallint", nullable: false),
                    TzAcr14 = table.Column<short>(type: "smallint", nullable: false),
                    TzAcr15 = table.Column<short>(type: "smallint", nullable: false),
                    TzAcr16 = table.Column<short>(type: "smallint", nullable: false),
                    TzAcr17 = table.Column<short>(type: "smallint", nullable: false),
                    TzAcr18 = table.Column<short>(type: "smallint", nullable: false),
                    TzAcr19 = table.Column<short>(type: "smallint", nullable: false),
                    TzAcr20 = table.Column<short>(type: "smallint", nullable: false),
                    TzAcr21 = table.Column<short>(type: "smallint", nullable: false),
                    TzAcr22 = table.Column<short>(type: "smallint", nullable: false),
                    TzAcr23 = table.Column<short>(type: "smallint", nullable: false),
                    TzAcr24 = table.Column<short>(type: "smallint", nullable: false),
                    TzAcr25 = table.Column<short>(type: "smallint", nullable: false),
                    TzAcr26 = table.Column<short>(type: "smallint", nullable: false),
                    TzAcr27 = table.Column<short>(type: "smallint", nullable: false),
                    TzAcr28 = table.Column<short>(type: "smallint", nullable: false),
                    TzAcr29 = table.Column<short>(type: "smallint", nullable: false),
                    TzAcr30 = table.Column<short>(type: "smallint", nullable: false),
                    TzAcr31 = table.Column<short>(type: "smallint", nullable: false),
                    TzAcr32 = table.Column<short>(type: "smallint", nullable: false),
                    TzAcr33 = table.Column<short>(type: "smallint", nullable: false),
                    TzAcr34 = table.Column<short>(type: "smallint", nullable: false),
                    TzAcr35 = table.Column<short>(type: "smallint", nullable: false),
                    TzAcr36 = table.Column<short>(type: "smallint", nullable: false),
                    TzAcr37 = table.Column<short>(type: "smallint", nullable: false),
                    TzAcr38 = table.Column<short>(type: "smallint", nullable: false),
                    TzAcr39 = table.Column<short>(type: "smallint", nullable: false),
                    TzAcr40 = table.Column<short>(type: "smallint", nullable: false),
                    TzAcr41 = table.Column<short>(type: "smallint", nullable: false),
                    TzAcr42 = table.Column<short>(type: "smallint", nullable: false),
                    TzAcr43 = table.Column<short>(type: "smallint", nullable: false),
                    TzAcr44 = table.Column<short>(type: "smallint", nullable: false),
                    TzAcr45 = table.Column<short>(type: "smallint", nullable: false),
                    TzAcr46 = table.Column<short>(type: "smallint", nullable: false),
                    TzAcr47 = table.Column<short>(type: "smallint", nullable: false),
                    TzAcr48 = table.Column<short>(type: "smallint", nullable: false),
                    TzAcr49 = table.Column<short>(type: "smallint", nullable: false),
                    TzAcr50 = table.Column<short>(type: "smallint", nullable: false),
                    TzAcr51 = table.Column<short>(type: "smallint", nullable: false),
                    TzAcr52 = table.Column<short>(type: "smallint", nullable: false),
                    TzAcr53 = table.Column<short>(type: "smallint", nullable: false),
                    TzAcr54 = table.Column<short>(type: "smallint", nullable: false),
                    TzAcr55 = table.Column<short>(type: "smallint", nullable: false),
                    TzAcr56 = table.Column<short>(type: "smallint", nullable: false),
                    TzAcr57 = table.Column<short>(type: "smallint", nullable: false),
                    TzAcr58 = table.Column<short>(type: "smallint", nullable: false),
                    TzAcr59 = table.Column<short>(type: "smallint", nullable: false),
                    TzAcr60 = table.Column<short>(type: "smallint", nullable: false),
                    TzAcr61 = table.Column<short>(type: "smallint", nullable: false),
                    TzAcr62 = table.Column<short>(type: "smallint", nullable: false),
                    TzAcr63 = table.Column<short>(type: "smallint", nullable: false),
                    TzAcr64 = table.Column<short>(type: "smallint", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArAcccessLevels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ArAcrModes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<short>(type: "smallint", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArAcrModes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ArAcrNo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ScpMac = table.Column<string>(type: "text", nullable: false),
                    SioNo = table.Column<short>(type: "smallint", nullable: false),
                    AcrNo = table.Column<short>(type: "smallint", nullable: false),
                    IsAvailable = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArAcrNo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ArAcrs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ScpMac = table.Column<string>(type: "text", nullable: false),
                    AcrNo = table.Column<short>(type: "smallint", nullable: false),
                    AccessCfg = table.Column<short>(type: "smallint", nullable: false),
                    RdrSio = table.Column<short>(type: "smallint", nullable: false),
                    ReaderNo = table.Column<short>(type: "smallint", nullable: false),
                    StrkSio = table.Column<short>(type: "smallint", nullable: false),
                    StrkNo = table.Column<short>(type: "smallint", nullable: false),
                    StrkMin = table.Column<short>(type: "smallint", nullable: false),
                    StrkMax = table.Column<short>(type: "smallint", nullable: false),
                    StrkMode = table.Column<short>(type: "smallint", nullable: false),
                    SensorSio = table.Column<short>(type: "smallint", nullable: false),
                    SensorNo = table.Column<short>(type: "smallint", nullable: false),
                    DcHeld = table.Column<short>(type: "smallint", nullable: false),
                    Rex1Sio = table.Column<short>(type: "smallint", nullable: false),
                    Rex1No = table.Column<short>(type: "smallint", nullable: false),
                    Rex2Sio = table.Column<short>(type: "smallint", nullable: false),
                    Rex2No = table.Column<short>(type: "smallint", nullable: false),
                    Rex1TzMask = table.Column<short>(type: "smallint", nullable: false),
                    Rex2TzMask = table.Column<short>(type: "smallint", nullable: false),
                    AlternateReaderSio = table.Column<short>(type: "smallint", nullable: false),
                    AlternateReaderNo = table.Column<short>(type: "smallint", nullable: false),
                    AlternateReaderSpec = table.Column<short>(type: "smallint", nullable: false),
                    CdFormat = table.Column<short>(type: "smallint", nullable: false),
                    ApbMode = table.Column<short>(type: "smallint", nullable: false),
                    OfflineMode = table.Column<short>(type: "smallint", nullable: false),
                    DefaultMode = table.Column<short>(type: "smallint", nullable: false),
                    DoorMode = table.Column<short>(type: "smallint", nullable: false),
                    DefaultLEDMode = table.Column<short>(type: "smallint", nullable: false),
                    PreAlarm = table.Column<short>(type: "smallint", nullable: false),
                    ApbDelay = table.Column<short>(type: "smallint", nullable: false),
                    StrkT2 = table.Column<short>(type: "smallint", nullable: false),
                    DcHeld2 = table.Column<short>(type: "smallint", nullable: false),
                    StrkFollowPulse = table.Column<short>(type: "smallint", nullable: false),
                    StrkFollowDelay = table.Column<short>(type: "smallint", nullable: false),
                    NExtFeatureType = table.Column<short>(type: "smallint", nullable: false),
                    IlPBSio = table.Column<short>(type: "smallint", nullable: false),
                    IlPBNumber = table.Column<short>(type: "smallint", nullable: false),
                    IlPBLongPress = table.Column<short>(type: "smallint", nullable: false),
                    IlPBOutSio = table.Column<short>(type: "smallint", nullable: false),
                    IlPBOutNum = table.Column<short>(type: "smallint", nullable: false),
                    DfOfFilterTime = table.Column<short>(type: "smallint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArAcrs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ArApbModes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<short>(type: "smallint", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArApbModes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ArCardFormats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CardFormatName = table.Column<string>(type: "text", nullable: false),
                    ComponentNo = table.Column<short>(type: "smallint", nullable: false),
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
                    CommandId = table.Column<short>(type: "smallint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArCardFormats", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ArCardHolders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CardHolderId = table.Column<string>(type: "text", nullable: false),
                    CardHolderRefNo = table.Column<string>(type: "text", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    MiddleName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    Sex = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: true),
                    Phone = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    HolderStatus = table.Column<string>(type: "text", nullable: true),
                    IssueCodeRunningNo = table.Column<int>(type: "integer", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArCardHolders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ArCommandStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TagNo = table.Column<int>(type: "integer", nullable: false),
                    ScpId = table.Column<int>(type: "integer", nullable: false),
                    ScpMac = table.Column<string>(type: "text", nullable: false),
                    Command = table.Column<string>(type: "text", nullable: false),
                    CommandStatus = table.Column<char>(type: "character(1)", nullable: false),
                    NakReason = table.Column<string>(type: "text", nullable: true),
                    NakDescCode = table.Column<int>(type: "integer", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArCommandStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ArControlPoints",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ScpMac = table.Column<string>(type: "text", nullable: false),
                    SioNo = table.Column<short>(type: "smallint", nullable: false),
                    CpNo = table.Column<short>(type: "smallint", nullable: false),
                    OpNo = table.Column<short>(type: "smallint", nullable: false),
                    Mode = table.Column<short>(type: "smallint", nullable: false),
                    DefaultPulseTime = table.Column<short>(type: "smallint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArControlPoints", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ArCpNo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ScpMac = table.Column<string>(type: "text", nullable: false),
                    SioNo = table.Column<short>(type: "smallint", nullable: false),
                    CpNo = table.Column<short>(type: "smallint", nullable: false),
                    IsAvailable = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArCpNo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ArCredentials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CardHolderRefNo = table.Column<string>(type: "text", nullable: false),
                    Bits = table.Column<int>(type: "integer", nullable: false),
                    IssueCode = table.Column<int>(type: "integer", nullable: false),
                    FacilityCode = table.Column<int>(type: "integer", nullable: false),
                    CardNo = table.Column<long>(type: "bigint", nullable: false),
                    Pin = table.Column<string>(type: "text", nullable: true),
                    ActTime = table.Column<string>(type: "text", nullable: false),
                    DeactTime = table.Column<string>(type: "text", nullable: false),
                    AccessLevel = table.Column<short>(type: "smallint", nullable: false),
                    Image = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArCredentials", x => x.Id);
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
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArEvents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ArIntervals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ComponentNo = table.Column<short>(type: "smallint", nullable: false),
                    IDays = table.Column<short>(type: "smallint", nullable: false),
                    IDaysDays = table.Column<string>(type: "text", nullable: false),
                    IStart = table.Column<short>(type: "smallint", nullable: false),
                    IStartTime = table.Column<string>(type: "text", nullable: false),
                    IEnd = table.Column<short>(type: "smallint", nullable: false),
                    IEndTime = table.Column<string>(type: "text", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArIntervals", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ArIpModes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<short>(type: "smallint", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArIpModes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ArMonitorPoints",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ScpIp = table.Column<string>(type: "text", nullable: false),
                    ScpMac = table.Column<string>(type: "text", nullable: false),
                    SioNo = table.Column<short>(type: "smallint", nullable: false),
                    MpNo = table.Column<short>(type: "smallint", nullable: false),
                    IpNo = table.Column<short>(type: "smallint", nullable: false),
                    IcvtNo = table.Column<short>(type: "smallint", nullable: false),
                    LfCode = table.Column<short>(type: "smallint", nullable: false),
                    DelayEntry = table.Column<short>(type: "smallint", nullable: false),
                    DelayExit = table.Column<short>(type: "smallint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArMonitorPoints", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ArMpNo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ScpMac = table.Column<string>(type: "text", nullable: false),
                    SioNo = table.Column<short>(type: "smallint", nullable: false),
                    MpNo = table.Column<short>(type: "smallint", nullable: false),
                    IsAvailable = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArMpNo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ArOperators",
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
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArOperators", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ArOpModes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Value = table.Column<short>(type: "smallint", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArOpModes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ArReaderModes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<short>(type: "smallint", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArReaderModes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ArReaders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ScpMac = table.Column<string>(type: "text", nullable: false),
                    SioNo = table.Column<short>(type: "smallint", nullable: false),
                    ReaderNo = table.Column<short>(type: "smallint", nullable: false),
                    LedDriveMode = table.Column<short>(type: "smallint", nullable: false),
                    OsdpFlag = table.Column<short>(type: "smallint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArReaders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ArScpComponents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ModelNo = table.Column<short>(type: "smallint", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    NInput = table.Column<short>(type: "smallint", nullable: false),
                    NOutput = table.Column<short>(type: "smallint", nullable: false),
                    NReader = table.Column<short>(type: "smallint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArScpComponents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ArScps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ScpId = table.Column<short>(type: "smallint", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Model = table.Column<string>(type: "text", nullable: false),
                    Mac = table.Column<string>(type: "text", nullable: false),
                    Ip = table.Column<string>(type: "text", nullable: false),
                    Port = table.Column<int>(type: "integer", nullable: false),
                    SerialNumber = table.Column<string>(type: "text", nullable: false),
                    NSio = table.Column<short>(type: "smallint", nullable: false),
                    NMp = table.Column<short>(type: "smallint", nullable: false),
                    NCp = table.Column<short>(type: "smallint", nullable: false),
                    NAcr = table.Column<short>(type: "smallint", nullable: false),
                    NAlvl = table.Column<short>(type: "smallint", nullable: false),
                    Ntrgr = table.Column<short>(type: "smallint", nullable: false),
                    Nproc = table.Column<short>(type: "smallint", nullable: false),
                    NTz = table.Column<short>(type: "smallint", nullable: false),
                    NHol = table.Column<short>(type: "smallint", nullable: false),
                    NMpg = table.Column<short>(type: "smallint", nullable: false),
                    IsUpload = table.Column<bool>(type: "boolean", nullable: false),
                    IsReset = table.Column<bool>(type: "boolean", nullable: false),
                    LastSync = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArScps", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ArScpSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NMsp1Port = table.Column<short>(type: "smallint", nullable: false),
                    NTransaction = table.Column<int>(type: "integer", nullable: false),
                    NSio = table.Column<short>(type: "smallint", nullable: false),
                    NMp = table.Column<short>(type: "smallint", nullable: false),
                    NCp = table.Column<short>(type: "smallint", nullable: false),
                    NAcr = table.Column<short>(type: "smallint", nullable: false),
                    NAlvl = table.Column<short>(type: "smallint", nullable: false),
                    NTrgr = table.Column<short>(type: "smallint", nullable: false),
                    NProc = table.Column<short>(type: "smallint", nullable: false),
                    GmtOffset = table.Column<short>(type: "smallint", nullable: false),
                    NTz = table.Column<short>(type: "smallint", nullable: false),
                    NHol = table.Column<short>(type: "smallint", nullable: false),
                    NMpg = table.Column<short>(type: "smallint", nullable: false),
                    NCard = table.Column<short>(type: "smallint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArScpSettings", x => x.Id);
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
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArScpStructureStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ArSioNo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ScpIp = table.Column<string>(type: "text", nullable: false),
                    ScpMac = table.Column<string>(type: "text", nullable: false),
                    SioNo = table.Column<short>(type: "smallint", nullable: false),
                    IsAvailable = table.Column<bool>(type: "boolean", nullable: false),
                    Port = table.Column<short>(type: "smallint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArSioNo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ArSios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ScpName = table.Column<string>(type: "text", nullable: false),
                    ScpIp = table.Column<string>(type: "text", nullable: false),
                    ScpMac = table.Column<string>(type: "text", nullable: false),
                    SioNumber = table.Column<short>(type: "smallint", nullable: false),
                    NInput = table.Column<short>(type: "smallint", nullable: false),
                    NOutput = table.Column<short>(type: "smallint", nullable: false),
                    NReader = table.Column<short>(type: "smallint", nullable: false),
                    Model = table.Column<short>(type: "smallint", nullable: false),
                    ModeDescription = table.Column<string>(type: "text", nullable: false),
                    Address = table.Column<short>(type: "smallint", nullable: false),
                    Msp1No = table.Column<short>(type: "smallint", nullable: false),
                    PortNo = table.Column<short>(type: "smallint", nullable: false),
                    BaudRate = table.Column<short>(type: "smallint", nullable: false),
                    NProtocol = table.Column<short>(type: "smallint", nullable: false),
                    NDialect = table.Column<short>(type: "smallint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArSios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ArStrikeModes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<short>(type: "smallint", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArStrikeModes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ArSystemConfigs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NPorts = table.Column<short>(type: "smallint", nullable: false),
                    NScp = table.Column<short>(type: "smallint", nullable: false),
                    NChannelId = table.Column<short>(type: "smallint", nullable: false),
                    CType = table.Column<short>(type: "smallint", nullable: false),
                    CPort = table.Column<short>(type: "smallint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArSystemConfigs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ArTzs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ComponentNo = table.Column<short>(type: "smallint", nullable: false),
                    Mode = table.Column<short>(type: "smallint", nullable: false),
                    ActiveTime = table.Column<string>(type: "text", nullable: false),
                    DeactiveTime = table.Column<string>(type: "text", nullable: false),
                    Intervals = table.Column<short>(type: "smallint", nullable: false),
                    IntervalsNoList = table.Column<string>(type: "text", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArTzs", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "ArAcccessLevels",
                columns: new[] { "Id", "ComponentNo", "CreatedDate", "IsActive", "Name", "TzAcr1", "TzAcr10", "TzAcr11", "TzAcr12", "TzAcr13", "TzAcr14", "TzAcr15", "TzAcr16", "TzAcr17", "TzAcr18", "TzAcr19", "TzAcr2", "TzAcr20", "TzAcr21", "TzAcr22", "TzAcr23", "TzAcr24", "TzAcr25", "TzAcr26", "TzAcr27", "TzAcr28", "TzAcr29", "TzAcr3", "TzAcr30", "TzAcr31", "TzAcr32", "TzAcr33", "TzAcr34", "TzAcr35", "TzAcr36", "TzAcr37", "TzAcr38", "TzAcr39", "TzAcr4", "TzAcr40", "TzAcr41", "TzAcr42", "TzAcr43", "TzAcr44", "TzAcr45", "TzAcr46", "TzAcr47", "TzAcr48", "TzAcr49", "TzAcr5", "TzAcr50", "TzAcr51", "TzAcr52", "TzAcr53", "TzAcr54", "TzAcr55", "TzAcr56", "TzAcr57", "TzAcr58", "TzAcr59", "TzAcr6", "TzAcr60", "TzAcr61", "TzAcr62", "TzAcr63", "TzAcr64", "TzAcr7", "TzAcr8", "TzAcr9", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, (short)1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "Full Access", (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, (short)1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, (short)0, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "No Access", (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, (short)0, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "ArAcrModes",
                columns: new[] { "Id", "CreatedDate", "Description", "Name", "UpdatedDate", "Value" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Disable the ACR, no REX", "Disable", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (short)1 },
                    { 2, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unlock (unlimited access)", "Unlock", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (short)2 },
                    { 3, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Locked (no access, REX active)", "Locked", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (short)3 },
                    { 4, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Facility code only", "Facility code only", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (short)4 },
                    { 5, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Card only", "Card only", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (short)5 },
                    { 6, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "PIN only", "PIN only", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (short)6 },
                    { 7, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Card and PIN required", "Card and PIN", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (short)7 },
                    { 8, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Card or PIN required", "Card or PIN", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (short)8 }
                });

            migrationBuilder.InsertData(
                table: "ArApbModes",
                columns: new[] { "Id", "CreatedDate", "Description", "Name", "UpdatedDate", "Value" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Do not check or alter anti-passback location. No antipassback rules.", "None", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (short)0 },
                    { 2, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Soft anti-passback: Accept any new location, change the user’s location to current reader, and generate an antipassback violation for an invalId entry.", "Soft anti-passback", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (short)1 },
                    { 3, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Hard anti-passback: Check user location, if a valid entry is made, change user’s location to new location. If an invalid entry is attempted, do not grant access.", "Hard anti-passback", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (short)2 }
                });

            migrationBuilder.InsertData(
                table: "ArCardFormats",
                columns: new[] { "Id", "Bits", "CardFormatName", "ChLn", "ChLoc", "CommandId", "ComponentNo", "CreatedDate", "Facility", "FcLn", "FcLoc", "Flags", "FunctionId", "IcLn", "IcLoc", "IsActive", "Offset", "PeLn", "PeLoc", "PoLn", "PoLoc", "UpdatedDate" },
                values: new object[] { 1, (short)26, "26 Bits No Fac", (short)16, (short)9, (short)0, (short)0, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (short)-1, (short)0, (short)0, (short)0, (short)1, (short)0, (short)0, true, (short)0, (short)13, (short)0, (short)13, (short)13, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "ArIpModes",
                columns: new[] { "Id", "CreatedDate", "Description", "Name", "UpdatedDate", "Value" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Normally closed, no End-Of-Line (EOL)", "Normally closed", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (short)0 },
                    { 2, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Normally open, no EOL", "Normally open", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (short)1 }
                });

            migrationBuilder.InsertData(
                table: "ArOpModes",
                columns: new[] { "Id", "CreatedDate", "Description", "UpdatedDate", "Value" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Normal Mode with Offline: No change", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (short)0 },
                    { 2, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Inverted Mode Offline: No change", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (short)1 },
                    { 3, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Normal Mode Offline: Inactive", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (short)16 },
                    { 4, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Inverted Mode Offline: Inactive", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (short)17 },
                    { 5, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Normal Mode Offline: Active", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (short)32 },
                    { 6, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Inverted Mode Offline: Inactive", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (short)33 }
                });

            migrationBuilder.InsertData(
                table: "ArReaderModes",
                columns: new[] { "Id", "CreatedDate", "Description", "Name", "UpdatedDate", "Value" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Single reader, controlling the door", "Single Reader", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (short)0 },
                    { 2, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Paired readers, Master - this reader controls the door", "Paired readers, Master", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (short)1 },
                    { 3, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Paired readers, Slave - this reader does not control door", "Paired readers, Slave", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (short)2 },
                    { 4, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Turnstile Reader. Two modes selected by: n strike_t_min != strike_t_max (original implementation - an access grant pulses the strike output for 1 second) n strike_t_min == strike_t_max (pulses the strike output after a door open/close signal for each additional access grant if several grants are waiting)", "Turnstile Reader", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (short)3 },
                    { 5, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Elevator, no floor select feedback", "Elevator, no floor", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (short)4 },
                    { 6, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Elevator with floor select feedback", "Elevator with floor", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (short)5 }
                });

            migrationBuilder.InsertData(
                table: "ArScpComponents",
                columns: new[] { "Id", "CreatedDate", "ModelNo", "NInput", "NOutput", "NReader", "Name", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (short)196, (short)7, (short)4, (short)4, "HID Aero X1100", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (short)193, (short)7, (short)4, (short)4, "HID Aero X100", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (short)194, (short)19, (short)2, (short)0, "HID Aero X200", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (short)195, (short)5, (short)12, (short)0, "HID Aero X300", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (short)190, (short)7, (short)4, (short)2, "VertX V100", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 6, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (short)191, (short)19, (short)2, (short)0, "VertX V200", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 7, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (short)192, (short)5, (short)12, (short)0, "VertX V300", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "ArScpSettings",
                columns: new[] { "Id", "CreatedDate", "GmtOffset", "NAcr", "NAlvl", "NCard", "NCp", "NHol", "NMp", "NMpg", "NMsp1Port", "NProc", "NSio", "NTransaction", "NTrgr", "NTz", "UpdatedDate" },
                values: new object[] { 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (short)-25200, (short)64, (short)32000, (short)200, (short)388, (short)255, (short)615, (short)128, (short)3, (short)1024, (short)16, 60000, (short)1024, (short)255, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "ArStrikeModes",
                columns: new[] { "Id", "CreatedDate", "Description", "Name", "UpdatedDate", "Value" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Do not use! This would allow the strike to stay active for the entire strike time allowing the door to be opened multiple times.", "Normal", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (short)0 },
                    { 2, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Deactivate strike when door opens", "Deactivate On Open", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (short)1 },
                    { 3, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Deactivate strike on door close or strike_t_max expires", "Deactivate On Close", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (short)2 },
                    { 4, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Used with ACR_S_OPEN or ACR_S_CLOSE, to select tailgate mode: pulse (strk_sio:strk_number+1) relay for each user expected to enter", "Tailgate", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (short)16 }
                });

            migrationBuilder.InsertData(
                table: "ArSystemConfigs",
                columns: new[] { "Id", "CPort", "CType", "CreatedDate", "NChannelId", "NPorts", "NScp", "UpdatedDate" },
                values: new object[] { 1, (short)3333, (short)7, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (short)1, (short)1, (short)100, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "ArTzs",
                columns: new[] { "Id", "ActiveTime", "ComponentNo", "CreatedDate", "DeactiveTime", "Intervals", "IntervalsNoList", "IsActive", "Mode", "Name", "UpdatedDate" },
                values: new object[] { 1, "", (short)1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", (short)0, "", false, (short)1, "Always", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArAcccessLevels");

            migrationBuilder.DropTable(
                name: "ArAcrModes");

            migrationBuilder.DropTable(
                name: "ArAcrNo");

            migrationBuilder.DropTable(
                name: "ArAcrs");

            migrationBuilder.DropTable(
                name: "ArApbModes");

            migrationBuilder.DropTable(
                name: "ArCardFormats");

            migrationBuilder.DropTable(
                name: "ArCardHolders");

            migrationBuilder.DropTable(
                name: "ArCommandStatuses");

            migrationBuilder.DropTable(
                name: "ArControlPoints");

            migrationBuilder.DropTable(
                name: "ArCpNo");

            migrationBuilder.DropTable(
                name: "ArCredentials");

            migrationBuilder.DropTable(
                name: "ArEvents");

            migrationBuilder.DropTable(
                name: "ArIntervals");

            migrationBuilder.DropTable(
                name: "ArIpModes");

            migrationBuilder.DropTable(
                name: "ArMonitorPoints");

            migrationBuilder.DropTable(
                name: "ArMpNo");

            migrationBuilder.DropTable(
                name: "ArOperators");

            migrationBuilder.DropTable(
                name: "ArOpModes");

            migrationBuilder.DropTable(
                name: "ArReaderModes");

            migrationBuilder.DropTable(
                name: "ArReaders");

            migrationBuilder.DropTable(
                name: "ArScpComponents");

            migrationBuilder.DropTable(
                name: "ArScps");

            migrationBuilder.DropTable(
                name: "ArScpSettings");

            migrationBuilder.DropTable(
                name: "ArScpStructureStatuses");

            migrationBuilder.DropTable(
                name: "ArSioNo");

            migrationBuilder.DropTable(
                name: "ArSios");

            migrationBuilder.DropTable(
                name: "ArStrikeModes");

            migrationBuilder.DropTable(
                name: "ArSystemConfigs");

            migrationBuilder.DropTable(
                name: "ArTzs");
        }
    }
}
