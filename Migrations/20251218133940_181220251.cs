using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HIDAeroService.Migrations
{
    /// <inheritdoc />
    public partial class _181220251 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccessAreas_Locations_LocationId",
                table: "AccessAreas");

            migrationBuilder.DropForeignKey(
                name: "FK_AccessLevels_Locations_LocationId",
                table: "AccessLevels");

            migrationBuilder.DropForeignKey(
                name: "FK_Actions_Locations_LocationId",
                table: "Actions");

            migrationBuilder.DropForeignKey(
                name: "FK_CardHolders_Locations_LocationId",
                table: "CardHolders");

            migrationBuilder.DropForeignKey(
                name: "FK_ControlPoints_Locations_LocationId",
                table: "ControlPoints");

            migrationBuilder.DropForeignKey(
                name: "FK_Credentials_Locations_LocationId",
                table: "Credentials");

            migrationBuilder.DropForeignKey(
                name: "FK_Doors_Locations_LocationId",
                table: "Doors");

            migrationBuilder.DropForeignKey(
                name: "FK_HardwareAccessLevel_Hardwares_MacAddress",
                table: "HardwareAccessLevel");

            migrationBuilder.DropForeignKey(
                name: "FK_HardwareCredential_Hardwares_HardwareCredentialId",
                table: "HardwareCredential");

            migrationBuilder.DropForeignKey(
                name: "FK_Hardwares_Locations_LocationId",
                table: "Hardwares");

            migrationBuilder.DropForeignKey(
                name: "FK_Holidays_Locations_LocationId",
                table: "Holidays");

            migrationBuilder.DropForeignKey(
                name: "FK_Intervals_Locations_LocationId",
                table: "Intervals");

            migrationBuilder.DropForeignKey(
                name: "FK_Modules_Hardwares_MacAddress",
                table: "Modules");

            migrationBuilder.DropForeignKey(
                name: "FK_Modules_Locations_LocationId",
                table: "Modules");

            migrationBuilder.DropForeignKey(
                name: "FK_MonitorGroups_Locations_LocationId",
                table: "MonitorGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_MonitorPoints_Locations_LocationId",
                table: "MonitorPoints");

            migrationBuilder.DropForeignKey(
                name: "FK_Procedures_Locations_LocationId",
                table: "Procedures");

            migrationBuilder.DropForeignKey(
                name: "FK_Readers_Locations_LocationId",
                table: "Readers");

            migrationBuilder.DropForeignKey(
                name: "FK_RequestExits_Locations_LocationId",
                table: "RequestExits");

            migrationBuilder.DropForeignKey(
                name: "FK_Sensors_Locations_LocationId",
                table: "Sensors");

            migrationBuilder.DropForeignKey(
                name: "FK_Strikes_Locations_LocationId",
                table: "Strikes");

            migrationBuilder.DropForeignKey(
                name: "FK_TimeZones_Locations_LocationId",
                table: "TimeZones");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Locations_LocationId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Triggers_Locations_LocationId",
                table: "Triggers");

            migrationBuilder.DropTable(
                name: "AeroStructureStatuses");

            migrationBuilder.DeleteData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.UpdateData(
                table: "AccessLevels",
                keyColumn: "Id",
                keyValue: 1,
                column: "LocationId",
                value: (short)1);

            migrationBuilder.UpdateData(
                table: "AccessLevels",
                keyColumn: "Id",
                keyValue: 2,
                column: "LocationId",
                value: (short)1);

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "Id", "ComponentId", "CreatedDate", "Description", "IsActive", "LocationName", "Uuid" },
                values: new object[] { 1, (short)1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Main Location", true, "Main", "00000000-0000-0000-0000-000000000001" });

            migrationBuilder.UpdateData(
                table: "TimeZones",
                keyColumn: "Id",
                keyValue: 1,
                column: "LocationId",
                value: (short)1);

            migrationBuilder.AddForeignKey(
                name: "FK_AccessAreas_Locations_LocationId",
                table: "AccessAreas",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "ComponentId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AccessLevels_Locations_LocationId",
                table: "AccessLevels",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "ComponentId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Actions_Locations_LocationId",
                table: "Actions",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "ComponentId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CardHolders_Locations_LocationId",
                table: "CardHolders",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "ComponentId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ControlPoints_Locations_LocationId",
                table: "ControlPoints",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "ComponentId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Credentials_Locations_LocationId",
                table: "Credentials",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "ComponentId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Doors_Locations_LocationId",
                table: "Doors",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "ComponentId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HardwareAccessLevel_Hardwares_MacAddress",
                table: "HardwareAccessLevel",
                column: "MacAddress",
                principalTable: "Hardwares",
                principalColumn: "MacAddress",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HardwareCredential_Hardwares_HardwareCredentialId",
                table: "HardwareCredential",
                column: "HardwareCredentialId",
                principalTable: "Hardwares",
                principalColumn: "ComponentId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Hardwares_Locations_LocationId",
                table: "Hardwares",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "ComponentId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Holidays_Locations_LocationId",
                table: "Holidays",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "ComponentId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Intervals_Locations_LocationId",
                table: "Intervals",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "ComponentId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Modules_Hardwares_MacAddress",
                table: "Modules",
                column: "MacAddress",
                principalTable: "Hardwares",
                principalColumn: "MacAddress",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Modules_Locations_LocationId",
                table: "Modules",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "ComponentId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MonitorGroups_Locations_LocationId",
                table: "MonitorGroups",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "ComponentId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MonitorPoints_Locations_LocationId",
                table: "MonitorPoints",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "ComponentId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Procedures_Locations_LocationId",
                table: "Procedures",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "ComponentId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Readers_Locations_LocationId",
                table: "Readers",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "ComponentId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RequestExits_Locations_LocationId",
                table: "RequestExits",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "ComponentId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Sensors_Locations_LocationId",
                table: "Sensors",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "ComponentId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Strikes_Locations_LocationId",
                table: "Strikes",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "ComponentId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TimeZones_Locations_LocationId",
                table: "TimeZones",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "ComponentId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Locations_LocationId",
                table: "Transactions",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "ComponentId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Triggers_Locations_LocationId",
                table: "Triggers",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "ComponentId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccessAreas_Locations_LocationId",
                table: "AccessAreas");

            migrationBuilder.DropForeignKey(
                name: "FK_AccessLevels_Locations_LocationId",
                table: "AccessLevels");

            migrationBuilder.DropForeignKey(
                name: "FK_Actions_Locations_LocationId",
                table: "Actions");

            migrationBuilder.DropForeignKey(
                name: "FK_CardHolders_Locations_LocationId",
                table: "CardHolders");

            migrationBuilder.DropForeignKey(
                name: "FK_ControlPoints_Locations_LocationId",
                table: "ControlPoints");

            migrationBuilder.DropForeignKey(
                name: "FK_Credentials_Locations_LocationId",
                table: "Credentials");

            migrationBuilder.DropForeignKey(
                name: "FK_Doors_Locations_LocationId",
                table: "Doors");

            migrationBuilder.DropForeignKey(
                name: "FK_HardwareAccessLevel_Hardwares_MacAddress",
                table: "HardwareAccessLevel");

            migrationBuilder.DropForeignKey(
                name: "FK_HardwareCredential_Hardwares_HardwareCredentialId",
                table: "HardwareCredential");

            migrationBuilder.DropForeignKey(
                name: "FK_Hardwares_Locations_LocationId",
                table: "Hardwares");

            migrationBuilder.DropForeignKey(
                name: "FK_Holidays_Locations_LocationId",
                table: "Holidays");

            migrationBuilder.DropForeignKey(
                name: "FK_Intervals_Locations_LocationId",
                table: "Intervals");

            migrationBuilder.DropForeignKey(
                name: "FK_Modules_Hardwares_MacAddress",
                table: "Modules");

            migrationBuilder.DropForeignKey(
                name: "FK_Modules_Locations_LocationId",
                table: "Modules");

            migrationBuilder.DropForeignKey(
                name: "FK_MonitorGroups_Locations_LocationId",
                table: "MonitorGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_MonitorPoints_Locations_LocationId",
                table: "MonitorPoints");

            migrationBuilder.DropForeignKey(
                name: "FK_Procedures_Locations_LocationId",
                table: "Procedures");

            migrationBuilder.DropForeignKey(
                name: "FK_Readers_Locations_LocationId",
                table: "Readers");

            migrationBuilder.DropForeignKey(
                name: "FK_RequestExits_Locations_LocationId",
                table: "RequestExits");

            migrationBuilder.DropForeignKey(
                name: "FK_Sensors_Locations_LocationId",
                table: "Sensors");

            migrationBuilder.DropForeignKey(
                name: "FK_Strikes_Locations_LocationId",
                table: "Strikes");

            migrationBuilder.DropForeignKey(
                name: "FK_TimeZones_Locations_LocationId",
                table: "TimeZones");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Locations_LocationId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Triggers_Locations_LocationId",
                table: "Triggers");

            migrationBuilder.DeleteData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.CreateTable(
                name: "AeroStructureStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LocationId = table.Column<short>(type: "smallint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    IpAddress = table.Column<string>(type: "text", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    MacAddress = table.Column<string>(type: "text", nullable: false),
                    RecAllocAcr = table.Column<int>(type: "integer", nullable: false),
                    RecAllocAlvl = table.Column<int>(type: "integer", nullable: false),
                    RecAllocArea = table.Column<int>(type: "integer", nullable: false),
                    RecAllocCardActive = table.Column<int>(type: "integer", nullable: false),
                    RecAllocCp = table.Column<int>(type: "integer", nullable: false),
                    RecAllocCrdb = table.Column<int>(type: "integer", nullable: false),
                    RecAllocEal = table.Column<int>(type: "integer", nullable: false),
                    RecAllocHoliday = table.Column<int>(type: "integer", nullable: false),
                    RecAllocMp = table.Column<int>(type: "integer", nullable: false),
                    RecAllocMpg = table.Column<int>(type: "integer", nullable: false),
                    RecAllocProc = table.Column<int>(type: "integer", nullable: false),
                    RecAllocSioPort = table.Column<int>(type: "integer", nullable: false),
                    RecAllocTimezone = table.Column<int>(type: "integer", nullable: false),
                    RecAllocTransaction = table.Column<int>(type: "integer", nullable: false),
                    RecAllocTrig = table.Column<int>(type: "integer", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    Uuid = table.Column<string>(type: "text", nullable: false)
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

            migrationBuilder.UpdateData(
                table: "AccessLevels",
                keyColumn: "Id",
                keyValue: 1,
                column: "LocationId",
                value: (short)0);

            migrationBuilder.UpdateData(
                table: "AccessLevels",
                keyColumn: "Id",
                keyValue: 2,
                column: "LocationId",
                value: (short)0);

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "Id", "ComponentId", "CreatedDate", "Description", "IsActive", "LocationName", "Uuid" },
                values: new object[,]
                {
                    { 1, (short)0, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Central Location", true, "Central", "00000000-0000-0000-0000-000000000001" },
                    { 2, (short)1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Main Location", true, "Main", "00000000-0000-0000-0000-000000000001" }
                });

            migrationBuilder.UpdateData(
                table: "TimeZones",
                keyColumn: "Id",
                keyValue: 1,
                column: "LocationId",
                value: (short)0);

            migrationBuilder.CreateIndex(
                name: "IX_AeroStructureStatuses_LocationId",
                table: "AeroStructureStatuses",
                column: "LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccessAreas_Locations_LocationId",
                table: "AccessAreas",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "ComponentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AccessLevels_Locations_LocationId",
                table: "AccessLevels",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "ComponentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Actions_Locations_LocationId",
                table: "Actions",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "ComponentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CardHolders_Locations_LocationId",
                table: "CardHolders",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "ComponentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ControlPoints_Locations_LocationId",
                table: "ControlPoints",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "ComponentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Credentials_Locations_LocationId",
                table: "Credentials",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "ComponentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Doors_Locations_LocationId",
                table: "Doors",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "ComponentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HardwareAccessLevel_Hardwares_MacAddress",
                table: "HardwareAccessLevel",
                column: "MacAddress",
                principalTable: "Hardwares",
                principalColumn: "MacAddress",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HardwareCredential_Hardwares_HardwareCredentialId",
                table: "HardwareCredential",
                column: "HardwareCredentialId",
                principalTable: "Hardwares",
                principalColumn: "ComponentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Hardwares_Locations_LocationId",
                table: "Hardwares",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "ComponentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Holidays_Locations_LocationId",
                table: "Holidays",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "ComponentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Intervals_Locations_LocationId",
                table: "Intervals",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "ComponentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Modules_Hardwares_MacAddress",
                table: "Modules",
                column: "MacAddress",
                principalTable: "Hardwares",
                principalColumn: "MacAddress",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Modules_Locations_LocationId",
                table: "Modules",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "ComponentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MonitorGroups_Locations_LocationId",
                table: "MonitorGroups",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "ComponentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MonitorPoints_Locations_LocationId",
                table: "MonitorPoints",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "ComponentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Procedures_Locations_LocationId",
                table: "Procedures",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "ComponentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Readers_Locations_LocationId",
                table: "Readers",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "ComponentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RequestExits_Locations_LocationId",
                table: "RequestExits",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "ComponentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sensors_Locations_LocationId",
                table: "Sensors",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "ComponentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Strikes_Locations_LocationId",
                table: "Strikes",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "ComponentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TimeZones_Locations_LocationId",
                table: "TimeZones",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "ComponentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Locations_LocationId",
                table: "Transactions",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "ComponentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Triggers_Locations_LocationId",
                table: "Triggers",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "ComponentId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
