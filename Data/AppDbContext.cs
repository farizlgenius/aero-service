using AeroService.Entity;
using HID.Aero.ScpdNet.Wrapper;
using AeroService.AeroLibrary;
using AeroService.Controllers.V1;
using AeroService.Entity;
using AeroService.Entity.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Xml.Linq;

namespace AeroService.Data
{

    public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {

        // New 
        public DbSet<Hardware> hardware { get; set; }
        public DbSet<Module> module { get; set; }
        public DbSet<Sensor> sensor { get; set; }
        public DbSet<RequestExit> request_exit { get; set; }
        public DbSet<MonitorPoint> monitor_point { get; set; }
        public DbSet<ControlPoint> control_point { get; set; }
        public DbSet<Strike> strike { get; set; }
        public DbSet<Reader> reader { get; set; }
        public DbSet<SystemSetting> system_setting { get; set; }
        public DbSet<HardwareComponent> hardware_component { get; set; }
        public DbSet<CardFormat> card_format { get; set; }
        public DbSet<Entity.TimeZone> timezone { get; set; }
        public DbSet<AccessLevel> accesslevel { get; set; }
        public DbSet<Holiday> holiday { get; set; }
        public DbSet<Door> door { get; set; }
        public DbSet<DoorMode> door_mode { get; set; }
        public DbSet<OutputMode> output_mode { get; set; }
        public DbSet<StrikeMode> strike_mode { get; set; }
        public DbSet<Interval> interval { get; set; }
        public DbSet<RelayOfflineMode> relay_offline_mode { get; set; }
        public DbSet<RelayMode> relay_mode { get; set; }
        public DbSet<TimeZoneMode> timezone_mode { get; set; }
        public DbSet<SystemConfiguration> system_configuration { get; set; }
        public DbSet<InputMode> input_mode { get; set; }
        public DbSet<ReaderConfigurationMode> reader_configuration_mode { get; set; }
        public DbSet<AntipassbackMode> antipassback_mode { get; set; }
        public DbSet<AccessLevelDoorTimeZone> accesslevel_door_timezone { get; set; }
        public DbSet<CardHolder> cardholder { get; set; }
        public DbSet<Credential> credential { get; set; }
        public DbSet<Area> area { get; set; }
        public DbSet<TimeZoneInterval> timezone_interval { get; set; }
        public DbSet<ReaderOutConfiguration> reader_out_configuration { get; set; }
        public DbSet<MonitorPointMode> monitor_point_mode { get; set; }
        public DbSet<OsdpBaudrate> osdp_baudrate { get; set; }
        public DbSet<OsdpAddress> osdp_address { get; set; }
        public DbSet<Operator> @operator { get; set; }
        public DbSet<DoorSpareFlag> door_spare_flag { get; set; }
        public DbSet<DoorAccessControlFlag> door_access_control_flag { get; set; }
        public DbSet<Location> location { get; set; }
        public DbSet<TransactionSource> transaction_source { get; set; }
        public DbSet<TransactionType> transaction_type { get; set; }
        public DbSet<TransactionCode> transaction_code { get; set; }
        public DbSet<CredentialFlag> credential_flag { get; set; }
        public DbSet<AccessAreaCommand> access_area_command { get; set; }
        public DbSet<AreaAccessControl> area_access_control { get; set; }
        public DbSet<OccupancyControl> occupancy_control { get; set; }
        public DbSet<AreaFlag> area_flag { get; set; }
        public DbSet<MultiOccupancy> multi_occupancy { get; set; }
        public DbSet<RefreshToken> refresh_token { get; set; }
        public DbSet<Feature> feature { get; set; }
        public DbSet<SubFeature> sub_feature { get; set; }
        public DbSet<Role> role { get; set; }
        public DbSet<FeatureRole> feature_role { get; set; }
        public DbSet<Transaction> transaction { get; set; }
        public DbSet<FileType> file_type { get; set; }
        public DbSet<MonitorGroup> monitor_group { get; set; }
        public DbSet<MonitorGroupList> monitor_group_list { get; set; }
        public DbSet<MonitorGroupType> monitor_group_type { get; set; }
        public DbSet<MonitorGroupCommand> monitor_group_command { get; set; }
        public DbSet<Procedure> procedure { get; set; }
        public DbSet<Entity.Action> action { get; set; }
        public DbSet<Trigger> trigger { get; set; }
        public DbSet<CardHolderAdditional> cardholder_additional { get; set; }
        public DbSet<CardHolderAccessLevel> cardholder_accesslevel { get; set; }
        public DbSet<ActionType> action_type { get; set; }
        public DbSet<TimeZoneCommand> timezone_command { get; set; }
        public DbSet<TriggerCommand> trigger_command { get; set; }
        public DbSet<TriggerTranCode> trigger_tran_code { get; set; }
        public DbSet<OperatorLocation> operator_location { get; set; }
        public DbSet<WeakPassword> weak_password { get; set; }
        public DbSet<PasswordRule> password_rule { get; set; }
        public DbSet<HardwareType> hardware_type { get; set; }
        public DbSet<IdReport> id_report { get; set; }
        public DbSet<ModuleBaudrate> module_baudrate { get; set; }
        public DbSet<ModuleProtocol> module_protocol { get; set; }
        public DbSet<MonitorPointLogFunction> monitor_point_log_function { get; set; }
        public DbSet<CommandLog> commnad_log { get; set; }
        public DbSet<DaysInWeek> days_in_week { get; set; }
        public DbSet<HardwareAccessLevel> hardware_accesslevel { get; set; }
        public DbSet<HardwareCredential> hardware_credential { get; set; }
        public DbSet<TransactionFlag> transaction_flag { get; set; }
        public DbSet<TransactionSourceType> transaction_source_type { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            #region Hardware 

            modelBuilder.Entity<HardwareType>()
                .HasData(
                new HardwareType { id=1,component_id=1,name="HID Aero",description="HID Intelligent Controller" },
                new HardwareType { id=2,component_id=2,name="HID Amico",description="HID Face Terminal"}
                );

            modelBuilder.Entity<Hardware>()
                .HasMany(p => p.modules)
                .WithOne(p => p.hardware)
                .HasForeignKey(p => p.hardware_mac)
                .HasPrincipalKey(p => p.mac)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Hardware>()
                .HasMany(p => p.monitor_groups)
                .WithOne(p => p.hardware)
                .HasForeignKey(p => p.hardware_mac)
                .HasPrincipalKey(p => p.mac)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Hardware>()
                .HasMany(p => p.doors)
                .WithOne(t => t.hardware)
                .HasForeignKey(p => p.hardware_mac)
                .HasPrincipalKey(t => t.mac)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<HardwareCredential>()
                .HasKey(p => new { p.hardware_mac, p.hardware_credential_id });

            modelBuilder.Entity<HardwareCredential>()
                .HasOne(e => e.hardware)
                .WithMany(e => e.hardware_credentials)
                .HasForeignKey(e => e.hardware_credential_id)
                .HasPrincipalKey(e => e.component_id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<HardwareAccessLevel>()
                .HasOne(e => e.hardware)
                .WithMany(e => e.hardware_accesslevels)
                .HasForeignKey(e => e.hardware_mac)
                .HasPrincipalKey(e => e.mac)
                .OnDelete(DeleteBehavior.Restrict);

            #endregion

            #region Monitor Point

            modelBuilder.Entity<MonitorPointMode>().HasData(
                new MonitorPointMode { id = 1, name = "Normal mode (no exit or entry delay)", value = 0, description = "" },
                new MonitorPointMode { id = 2, name = "Non-latching mode", value = 1, description = "" },
                new MonitorPointMode { id = 3, name = "Latching mode", value = 2, description = "" }
            );

            modelBuilder.Entity<InputMode>().HasData(
                new InputMode { id = 1, name = "Normally closed", value = 0, description = "Normally closed, no End-Of-Line (EOL)" },
                new InputMode { id = 2, name = "Normally open", value = 1, description = "Normally open, no EOL" },
                new InputMode { id = 3, name = "Standard EOL 1", value = 2, description = "Standard (ROM’ed) EOL: 1 kΩ normal, 2 kΩ active" },
                new InputMode { id = 4, name = "Standard EOL 2", value = 3, description = "Standard (ROM’ed) EOL: 2 kΩ normal, 1 kΩ active" }
                );

            modelBuilder.Entity<MonitorPointLogFunction>()
                .HasData(
                    new MonitorPointLogFunction { id=1,name= "Logs all", value=0,description= "Logs all changes" },
                    new MonitorPointLogFunction { id=2,name="No Masked",value=1,description= "Do not log contact change-of-state if masked" },
                    new MonitorPointLogFunction { id=3,name="No Mask & Fault to Fault",value=2,description= "Do not log contact change-of-state if masked and no fault-to-fault changes" }
                );

            #endregion

            #region Monitor Group

            modelBuilder.Entity<MonitorGroup>()
                .HasMany(m => m.n_mp_list)
                .WithOne(l => l.monitor_group)
                .HasForeignKey(l => l.monitor_group_id)
                .HasPrincipalKey(x => x.component_id);

            modelBuilder.Entity<MonitorGroupType>()
                .HasData(
                    new MonitorGroupType { id=1,name="Monitor Point",value=1,description="" },
                    new MonitorGroupType { id=2,name="Forced Open",value=2,description=""},
                    new MonitorGroupType { id=3,name="Held Open",value=3,description=""}
                );

            modelBuilder.Entity<MonitorGroupCommand>()
                .HasData(
                    new MonitorGroupCommand { id=1,name="Access",value=1,description= "If the mask count is zero, mask all monitor points and increment the mask count by one" },
                    new MonitorGroupCommand { id=2,name="Override",value=2,description= "Set mask count to arg1. If arg1 is zero, then all points get unmasked. If arg1 is not zero, then all points get masked." },
                    new MonitorGroupCommand { id=3,name="Force Arm",value=3,description= "Force Arm: If the mask count > 1 then decrement the mask count by 1. Otherwise, if the mask count is equal to 1, unmask all non-active monitor points and set the mask count to zero." },
                    new MonitorGroupCommand { id=4,name="Arm",value=4,description= "If the mask count > 1 then decrement the mask count by one. Otherwise, if the mask count is equal to 1 and no monitor points are active, unmask all monitor points. and set the mask count to zero." },
                    new MonitorGroupCommand { id=5,name="Override arm",value=5,description= "If the mask count > 1 then decrement the mask count by one, otherwise if the mask count is 1 unmask all monitor points and set the mask count to zero" }
                );


            #endregion

            #region Control Point 

            modelBuilder.Entity<OutputMode>().HasData(
                new OutputMode { id = 1, relay_mode = 0, offline_mode = 0, value = 0, description = "Normal mode with Offline: No change" },
                new OutputMode { id = 2, relay_mode = 1, offline_mode = 0, value = 1, description = "Inverted mode Offline: No change" },
                new OutputMode { id = 3, relay_mode = 0, offline_mode = 1, value = 16, description = "Normal mode Offline: Inactive" },
                 new OutputMode { id = 4, relay_mode = 1, offline_mode = 1, value = 17, description = "Inverted mode Offline: Inactive" },
                 new OutputMode { id = 5, relay_mode = 0, offline_mode = 2, value = 32, description = "Normal mode Offline: Active" },
                 new OutputMode { id = 6, relay_mode = 1, offline_mode = 2, value = 33, description = "Inverted mode Offline: Active" }
                );

            modelBuilder.Entity<RelayOfflineMode>().HasData(
                new RelayOfflineMode { id = 1, value = 0, name = "No Change", description = "No Change" },
                 new RelayOfflineMode { id = 2, value = 1, name = "Inactive", description = "Relay de-energized" },
                  new RelayOfflineMode { id = 3, value = 2, name = "Active", description = "Relay energized" }
                );

            modelBuilder.Entity<RelayMode>().HasData(
                new RelayMode { id = 1, value = 0, name = "Normal", description = "Active is energized" },
                new RelayMode { id = 2, value = 1, name = "Inverted", description = "Active is de-energized" }
                );


            #endregion

            #region Module

            modelBuilder.Entity<Module>()
                .HasMany(p => p.sensors)
                .WithOne(p => p.module)
                .HasForeignKey(p => p.module_id)
                .HasPrincipalKey(p => p.component_id)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Module>()
                .HasMany(p => p.strikes)
                .WithOne(p => p.module)
                .HasForeignKey(p => p.module_id)
                .HasPrincipalKey(p => p.component_id)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Module>()
                .HasMany(p => p.readers)
                .WithOne(p => p.module)
                .HasForeignKey(p => p.module_id)
                .HasPrincipalKey(p => p.component_id)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Module>()
                .HasMany(p => p.request_exits)
                .WithOne(p => p.module)
                .HasForeignKey(p => p.module_id)
                .HasPrincipalKey (p => p.component_id)
                .OnDelete (DeleteBehavior.Cascade);


            modelBuilder.Entity<Module>()
                .HasMany(p => p.control_points)
                .WithOne(p => p.module)
                .HasForeignKey(p => p.module_id)
                .HasPrincipalKey(p => p.component_id)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Module>()
                .HasMany(p => p.monitor_points)
                .WithOne(p => p.module)
                .HasForeignKey(p => p.module_id)
                .HasPrincipalKey(p => p.component_id)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ModuleBaudrate>()
                .HasData(
                new ModuleBaudrate { id=1,name="9600",value=9600,description="9600" },
                new ModuleBaudrate { id = 2, name = "19200", value = 19200, description = "19200" },
                new ModuleBaudrate { id = 3, name = "38400", value = 38400, description = "38400" },
                new ModuleBaudrate { id = 4, name = "115200", value = 115200, description = "115200" }
                );

            modelBuilder.Entity<ModuleProtocol>()
                .HasData(
                new ModuleProtocol { id = 1, name = "Aero", value = 0, description = "HID Aero X100, X200 and X300 protocol" },
                new ModuleProtocol { id = 2, name = "VertX", value = 15, description = "VertX™ V100, V200 and V300 protocol" },
                new ModuleProtocol { id = 3, name = "Aperio", value = 16, description = "Aperio" }
                );

            #endregion

            #region Access Level

            modelBuilder.Entity<AccessLevelDoorTimeZone>()
                .HasKey(p => new { p.accesslevel_id,p.timezone_id,p.door_id });

            modelBuilder.Entity<AccessLevelDoorTimeZone>()
                .HasOne(s => s.accesslevel)
                .WithMany(s => s.accessleve_door_timezones)
                .HasForeignKey(p => p.accesslevel_id)
                .HasPrincipalKey(p => p.component_id)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AccessLevelDoorTimeZone>()
                .HasOne(x => x.timezone)
                .WithMany(x => x.accesslevel_door_timezones)
                .HasForeignKey(x => x.timezone_id)
                .HasPrincipalKey (x => x.component_id)
                .OnDelete (DeleteBehavior.Cascade);

            modelBuilder.Entity<AccessLevelDoorTimeZone>()
                .HasOne(x => x.door)
                .WithMany(x => x.accesslevel_door_timezones)
                .HasForeignKey(x => x.door_id)
                .HasPrincipalKey(x => x.component_id)
                .OnDelete(DeleteBehavior.Cascade);

            var NoAccess = new AccessLevel { id = 1, uuid = SeedDefaults.SystemGuid, name = "No Access", component_id = 1,location_id=1, is_active = true };

            var FullAccess = new AccessLevel { id = 2, uuid = SeedDefaults.SystemGuid, name = "Full Access", component_id = 2,location_id=1, is_active = true };


            modelBuilder.Entity<AccessLevel>().HasData(
               NoAccess,
               FullAccess
            );


            #endregion

            #region Time Zone

            modelBuilder.Entity<TimeZoneInterval>()
                .HasKey(e => new { e.timezone_id, e.interval_id });

            //modelBuilder.Entity<TimeZoneInterval>()
            //    .HasOne(e => e.timezone)
            //    .WithMany(s => s.timezone_interval)
            //    .HasForeignKey(e => e.timezone_id)
            //    .HasPrincipalKey(s => s.component_id);


            //modelBuilder.Entity<TimeZoneInterval>()
            //    .HasOne(e => e.interval)
            //    .WithMany(s => s.timezone_interval)
            //    .HasForeignKey(e => e.interval_id)
            //    .HasPrincipalKey(e => e.component_id);

            modelBuilder.Entity<Entity.TimeZone>()
                .HasMany(e => e.timezone_intervals)
                .WithOne(s => s.timezone)
                .HasForeignKey(e => e.timezone_id)
                .HasPrincipalKey(s => s.component_id)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Interval>()
                .HasMany(e => e.timezone_intervals)
                .WithOne(s => s.interval)
                .HasForeignKey(e => e.interval_id)
                .HasPrincipalKey(e => e.component_id)
                .OnDelete(DeleteBehavior.Cascade);



            modelBuilder.Entity<Entity.TimeZone>().HasData(
                new Entity.TimeZone { id = 1, uuid = SeedDefaults.SystemGuid, name = "Always", component_id = 1, mode = 1, active_time = "", deactive_time = "", is_active = true,location_id=1 }
               );

            modelBuilder.Entity<TimeZoneMode>().HasData(
                new TimeZoneMode { id = 1, value = 0, name = "Off", description = "The time zone is always inactive, regardless of the time zone interval specified or the holiday in effect." },
                new TimeZoneMode { id = 2, value = 1, name = "On", description = "The time zone is always active, regardless of the time zone interval specified or the holiday in effect." },
                new TimeZoneMode { id = 3, value = 2, name = "Scan", description = "The time Zone state is decided using either the day MaskAsync or the Holiday MaskAsync. If the current day is specified as a Holiday, the state relies only on whether the Holiday MaskAsync flag for that Holiday is set (if today is Holiday 1, and the Holiday MaskAsync sets flag H1, then the state is active. If today is Holiday 1, and the Holiday MaskAsync does not have flag H1 set, then the state is inactive). holiday override the standard accessibility rules. If the current day is not specified as a Holiday, the time Zone is active or inactive depending on whether the current day/time falls within the day MaskAsync. If day MaskAsync is M-F, 8-5, the time Zone is active during those times, and inactive on the weekend and outside working hours." },
                new TimeZoneMode { id = 4, value = 3, name = "OneTimeEvent", description = "Scan time zone interval list and apply only if the date string in expTest matches the current date" },
                new TimeZoneMode { id = 5, value = 4, name = "Scan, Always Honor day of Week", description = "This mode is similar to mode Scan mode, but instead of only checking the Holiday MaskAsync if it is a Holiday, and only checking the day MaskAsync if not, this mode checks both. If it is not a Holiday, this mode functions normally, only checking the day MaskAsync. If it is a Holiday, this mode performs a logical OR on the Holiday and day Masks. If either or both are active, the time Zone is active, otherwise if neither is active, the time Zone is inactive." },
                new TimeZoneMode { id = 6, value = 5, name = "Scan, Always Holiday and day of Week", description = "This mode is similar to mode \"Scan, Always Honor day of Week\", but it performs a logical AND instead of a logical OR. If it is not a Holiday, this mode functions normally, only checking the day MaskAsync. If it is a Holiday, this mode is only active if BOTH the day MaskAsync and Holiday MaskAsync are active. If either one is inactive, the entire time Zone is inactive." }
             );

            modelBuilder.Entity<TimeZoneCommand>()
                .HasData(
                new TimeZoneCommand { id=1,name="Temporary Clear",value=1,description= "Temporary Clear - Deactivate time Zone until it would normally change. Next interval change will clear the override." },
                new TimeZoneCommand { id=2,name="Temporary Set",value=2,description= "Temporary Set - Activate time Zone until it would normally change. Next interval change will clear the override." },
                new TimeZoneCommand { id=3,name="Override Clear",value=3,description= "Override Clear - Deactivate time Zone until next command 314" },
                new TimeZoneCommand { id=4,name="Override Set",value=4,description= "Override Set - Activate time Zone until next command 314" },
                new TimeZoneCommand { id=5,name="Release",value=5,description= "Release time Zone (Return to Normal). Will take the time zone out of the temporary or override mode and put it in the proper state." },
                new TimeZoneCommand { id=6,name="Refresh",value=6,description= "Refresh - causes time zone state to be logged to the transaction log. Commonly used for triggers." }
            );

            #endregion

            #region Interval

            modelBuilder.Entity<Interval>()
                .HasIndex(i => i.component_id)
                .IsUnique();

            modelBuilder.Entity<DaysInWeek>()
                .HasIndex(d => d.component_id)
                .IsUnique();

            modelBuilder.Entity<Interval>().HasOne(u => u.days).WithOne(u => u.interval).HasPrincipalKey<Interval>(p => p.component_id).HasForeignKey<DaysInWeek>(p => p.component_id);

            #endregion

            #region Door

            modelBuilder.Entity<Door>()
                .HasMany(p => p.readers)
                .WithOne(p => p.door)
                .HasForeignKey(p => p.component_id)
                .HasPrincipalKey(p => p.component_id)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Door>()
                .HasOne(p => p.sensor)
                .WithOne(p => p.sensor_door)
                .HasForeignKey<Door>(p => p.sensor_id)
                .HasPrincipalKey<Sensor>(p => p.component_id)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Door>()
                .HasMany(p => p.request_exits)
                .WithOne(p => p.door)
                .HasForeignKey(p => p.component_id)
                .HasPrincipalKey(p => p.component_id)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Door>()
                .HasOne(p => p.strike)
                .WithOne(p => p.strike_door)
                .HasForeignKey<Door>(p => p.strike_id)
                .HasPrincipalKey<Strike>(p => p.component_id)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DoorSpareFlag>().HasData(
                new DoorSpareFlag { id = 1,name="No extend",value=0x0001,description= "ACR_FE_NOEXTEND\t0x0001\t\r\n🔹 Purpose: Prevents the “Extended door Held Open Timer” from being restarted when a new access is granted.\r\n🔹 Effect: If someone presents a valid credential while the door is already open, the extended hold timer won’t reset — the timer continues to count down.\r\n🔹 Use Case: High-traffic door where you don’t want repeated badge reads to keep the door open indefinitely." },
                new DoorSpareFlag { id = 2, name="Card Before pin" ,value=0x0002,description= "ACR_FE_NOPINCARD\t0x0002\t\r\n🔹 Purpose: Forces CARD-before-PIN entry sequence in “Card and PIN” mode.\r\n🔹 Effect: PIN entered before a card will be rejected.\r\n🔹 Use Case: Ensures consistent user behavior and security (e.g., requiring card tap first, then PIN entry)." },
                new DoorSpareFlag { id= 3,name="door Force Filter",value=0x0008,description= "ACR_FE_DFO_FLTR\t0x0008\t\r\n🔹 Purpose: Enables door Forced Open Filter.\r\n🔹 Effect: If the door opens within 3 seconds after it was last closed, the system will not treat it as a “door Forced Open” alarm.\r\n🔹 Use Case: Prevents nuisance alarms caused by door bounce, air pressure, or slow latch operation." },
                new DoorSpareFlag { id = 4,name="Blocked All Request",value=0x0010,description= "ACR_FE_NO_ARQ\t0x0010\t\r\n🔹 Purpose: Blocks all access requests.\r\n🔹 Effect: Every access attempt is automatically reported as “Access Denied – door Locked.”\r\n🔹 Use Case: Temporarily disables access (e.g., during lockdown, maintenance, or controlled shutdown)." },
                new DoorSpareFlag {  id=5,name="Shunt Relay",value=0x0020,description= "ACR_FE_SHNTRLY\t0x0020\t\r\n🔹 Purpose: Defines a Shunt Relay used for suppressing door alarms during unlock events.\r\n🔹 Effect: When the door is unlocked:\r\n • The shunt relay activates 5 ms before the strike relay.\r\n • It deactivates 1 second after the door closes or the held-open timer expires.\r\n🔹 Note: The dc_held field (door-held timer) must be > 1 for this to function.\r\n🔹 Use Case: Used when connecting to alarm panels or to bypass door contacts during unlocks." },
                new DoorSpareFlag { id=6,name="Floor pin",value=0x0040,description= "ACR_FE_FLOOR_PIN\t0x0040\t\r\n🔹 Purpose: Enables Floor Selection via PIN for elevators in “Card + PIN” mode.\r\n🔹 Effect: Instead of entering a PIN code, users enter the floor number after presenting a card.\r\n🔹 Use Case: Simplifies elevator access when using a single reader for multiple floors." },
                new DoorSpareFlag {  id=7,name="Link mode",value=0x0080,description= "ACR_FE_LINK_MODE\t0x0080\t\r\n🔹 Purpose: Indicates that the reader is in linking mode (pairing with another device or reader).\r\n🔹 Effect: Set when acr_mode = 29 (start linking) and cleared when:\r\n • The reader is successfully linked, or\r\n • acr_mode = 30 (abort) or timeout occurs.\r\n🔹 Use Case: Used for configuring dual-reader systems (e.g., in/out reader or linked elevator panels)." },
                new DoorSpareFlag {  id=8,name="Double Card transaction",value=0x0100,description= "ACR_FE_DCARD\t0x0100\t\r\n🔹 Purpose: Enables Double Card mode.\r\n🔹 Effect: If the same valid card is presented twice within 5 seconds, it generates a double card event.\r\n🔹 Use Case: Used for dual-authentication or special functions (e.g., manager override, arming/disarming security zones)." },
                new DoorSpareFlag { id=9,name="Allow mode Override",value=0x0200,description= "ACR_FE_OVERRIDE\t0x0200\t\r\n🔹 Purpose: Indicates that the reader is operating in a Temporary ACR mode Override.\r\n🔹 Effect: Typically means that a temporary mode (e.g., unlocked, lockdown) has been forced manually or by schedule.\r\n🔹 Use Case: Allows temporary override of normal access control logic without changing the base configuration." },
                new DoorSpareFlag { id=10,name="Allow Super Card",value=0x0400,description= "ACR_FE_CRD_OVR_EN\t0x0400\t\r\n🔹 Purpose: Enables Override credential.\r\n🔹 Effect: Specific credential (set in FFRM_FLD_ACCESSFLGS) can unlock the door even when it’s locked or access is disabled.\r\n🔹 Use Case: For emergency or master access cards (security, admin, fire personnel)." },
                new DoorSpareFlag {  id=11,name="Disable Elevator",value=0x0800,description= "ACR_FE_ELV_DISABLE\t0x0800\t\r\n🔹 Purpose: Enables the ability to disable elevator floors using the offline_mode field.\r\n🔹 Effect: Only applies to Elevator type_desc 1 and 2 ACRs.\r\n🔹 Use Case: Temporarily disables access to certain floors when the elevator or reader is in offline or restricted mode." },
                new DoorSpareFlag { id=12,name="Alternate Reader Link",value=0x1000,description= "ACR_FE_LINK_MODE_ALT\t0x1000\t\r\n🔹 Purpose: Similar to ACR_FE_LINK_MODE but for Alternate Reader Linking.\r\n🔹 Effect: Set when acr_mode = 32 (start link) and cleared when:\r\n  • Link successful, or\r\n  • acr_mode = 33 (abort) or timeout reached.\r\n🔹 Use Case: Used for alternate or backup reader pairing configurations." },
                new DoorSpareFlag {  id=13,name="HOLD REX",value=0x2000,description= "🔹 Purpose: Extends the REX (Request-to-Exit) grant time while REX input is active.\r\n🔹 Effect: As long as the REX signal remains active (button pressed or motion detected), the door remains unlocked.\r\n🔹 Use Case: Ideal for long exit paths, large door, or slow-moving personnel." },
                new DoorSpareFlag { id=14,name="HOST Decision",value=0x4000,description= "ACR_FE_HOST_BYPASS\t0x4000\t\r\n🔹 Purpose: Enables host decision bypass for online authorization.\r\n🔹 Effect: Requires ACR_F_HOST_CBG to also be enabled.\r\n 1. Controller sends credential to host for decision.\r\n 2. If host replies in time → uses host’s decision.\r\n 3. If no reply (timeout): controller checks its local database.\r\n  • If credential valid → grant.\r\n  • If not → deny.\r\n🔹 Use Case: For real-time validation in networked systems but with local fallback during communication loss.\r\n🔹 Supports: Card + PIN reader, online decision making, hybrid access control." }
                );


            modelBuilder.Entity<DoorAccessControlFlag>().HasData(
                new DoorAccessControlFlag { id=1,name="Decrease Counter",value=0x0001,description= "ACR_F_DCR\t0x0001\t\r\n🔹 Purpose: Decrements a user’s “use counter” when they successfully access.\r\n🔹 Effect: Each valid access reduces their remaining allowed uses.\r\n🔹 Use Case: Temporary or limited-access credential (e.g., one-time use visitor cards or tickets)." },
                new DoorAccessControlFlag { id=2,name="Deny Zero",value=0x0002,description= "ACR_F_CUL\t0x0002\t\r\n🔹 Purpose: Requires that the use limit is non-zero before granting access.\r\n🔹 Effect: If the use counter reaches zero, access is denied.\r\n🔹 Use Case: Works together with ACR_F_DCR for managing credential with limited usage." },
                new DoorAccessControlFlag { id=3,name="Denu Duress",value=0x0004,description= "ACR_F_DRSS\t0x0004\t\r\n🔹 Purpose: Deny duress access requests.\r\n🔹 Effect: Normally, a duress PIN (like a special emergency PIN) grants access but logs a “duress” event. This flag changes that behavior — access is denied instead, but still logged.\r\n🔹 Use Case: High-security environments where duress entries should never open the door (only alert security)." },
                new DoorAccessControlFlag { id=4,name="No sensor door",value=0x0008,description= "ACR_F_ALLUSED\t0x0008\t\r\n🔹 Purpose: Treat all access grants as “used” immediately — don’t wait for door contact feedback.\r\n🔹 Effect: When access is granted, the system immediately logs it as used, even if the door sensor doesn’t open.\r\n🔹 Use Case: For systems with no door contact sensor, or for virtual reader (logical access)." },
                new DoorAccessControlFlag { id=5,name="Quit Exit",value=0x0010,description= "ACR_F_QEXIT\t0x0010\t\r\n🔹 Purpose: “Quiet Exit” — disables strike relay activation on REX (Request to Exit).\r\n🔹 Effect: When someone exits, the strike is not pulsed — useful for magnetic locks or silent egress door.\r\n🔹 Use Case: Hospital wards, offices, or area where audible clicks must be minimized." },
                new DoorAccessControlFlag { id=6,name="door State Filter",value=0x0020,description= "ACR_F_FILTER\t0x0020\t\r\n🔹 Purpose: Filter out detailed door state change transactions (like every open/close event).\r\n🔹 Effect: Reduces event log noise — only key events (grants, denies) are logged.\r\n🔹 Use Case: Typically enabled in production. Disable only if you need fine-grained door state diagnostics." },
                new DoorAccessControlFlag { id=7,name="Two man rules",value=0x0040,description= "ACR_F_2CARD\t0x0040\t\r\n🔹 Purpose: Enables two-card control — requires two different valid cards before access is granted.\r\n🔹 Effect: The system waits for a second credential (often within a timeout period).\r\n🔹 Use Case: High-security door or vaults where two people must be present (dual authentication)." },
                new DoorAccessControlFlag { id=8,name="Host Decision",value=0x0400,description= "ACR_F_HOST_CBG\t0x0400\t\r\n🔹 Purpose: If online, check with the host server before granting access.\r\n🔹 Effect: The controller sends the access request to the host; the host can grant or deny.\r\n🔹 Use Case: Centralized decision-making — e.g., dynamic permissions, host-based rules, or temporary card revocations.\r\n🔹 Note: Often used together with ACR_FE_HOST_BYPASS in the extended flags." },
                new DoorAccessControlFlag { id=9,name="Offline Grant",value=0x0800,description= "ACR_F_HOST_SFT\t0x0800\t\r\n🔹 Purpose: Defines offline failover behavior.\r\n🔹 Effect: If the host is unreachable or times out, the controller proceeds to grant access using local data instead of denying.\r\n🔹 Use Case: Ensures continuity of access during temporary network outages.\r\n🔹 Note: Use with caution — enables access even when host verification fails." },
                new DoorAccessControlFlag { id=10,name="Cipher mode",value=0x1000,description= "ACR_F_CIPHER\t0x1000\t\r\n🔹 Purpose: Enables Cipher mode (numeric keypad emulates card input).\r\n🔹 Effect: Allows the user to type their card number on a keypad instead of presenting a physical card.\r\n🔹 Use Case: For environments with numeric-only access or backup credential entry.\r\n🔹 Reference: See command 1117 (trigger Specification) for keypad mapping." },
                new DoorAccessControlFlag { id=11,name="Log Early",value=0x4000,description= "ACR_F_LOG_EARLY\t0x4000\t\r\n🔹 Purpose: Log access transactions immediately upon grant — before door usage is confirmed.\r\n🔹 Effect: Creates an instant “Access Granted” event, then later logs “Used” or “Not Used.”\r\n🔹 Constraint: Automatically disabled if ACR_F_ALLUSED (0x0008) is set.\r\n🔹 Use Case: Real-time systems that require immediate event logging (e.g., monitoring dashboards)." },
                new DoorAccessControlFlag { id=12,name="Wait for Card in file",value=0x8000, description= "ACR_F_CNIF_WAIT\t0x8000\t\r\n🔹 Purpose: Changes “Card Not in File” behavior to show ‘Wait’ pattern instead of “Denied.”\r\n🔹 Effect: The reader shows a temporary wait indication (e.g., blinking LED) — useful when waiting for host validation.\r\n🔹 Use Case: Online reader with host delay — improves user feedback for cards that might soon be recognized after sync.\r\n🔹 Reference: See command 122 (Reader LED/Buzzer Function Specs)." }
                
                );

            modelBuilder.Entity<ReaderConfigurationMode>().HasData(
              new ReaderConfigurationMode { id = 1, name = "Single Reader", value = 0, description = "Single reader, controlling the door" },
              new ReaderConfigurationMode { id = 2, name = "Paired reader, Master", value = 1, description = "Paired reader, Master - this reader controls the door" },
              new ReaderConfigurationMode { id = 3, name = "Paired reader, Slave", value = 2, description = "Paired reader, Slave - this reader does not control door" },
              new ReaderConfigurationMode { id = 4, name = "Turnstile Reader", value = 3, description = "Turnstile Reader. Two modes selected by: n strike_t_min != strike_t_max (original implementation - an access grant pulses the strike output for 1 second) n strike_t_min == strike_t_max (pulses the strike output after a door open/close signal for each additional access grant if several grants are waiting)" },
              new ReaderConfigurationMode { id = 5, name = "Elevator, no floor", value = 4, description = "Elevator, no floor select feedback" },
              new ReaderConfigurationMode { id = 6, name = "Elevator with floor", value = 5, description = "Elevator with floor select feedback" }
              );

            modelBuilder.Entity<StrikeMode>().HasData(
                new StrikeMode { id = 1, name = "Normal", value = 0, description = "Do not use! This would allow the strike to stay active for the entire strike time allowing the door to be opened multiple times." },
                new StrikeMode { id = 2, name = "Deactivate On Open", value = 1, description = "Deactivate strike when door opens" },
                new StrikeMode { id = 3, name = "Deactivate On Close", value = 2, description = "Deactivate strike on door close or strike_t_max expires" },
                new StrikeMode { id = 4, name = "Tailgate", value = 16, description = "Used with ACR_S_OPEN or ACR_S_CLOSE, to select tailgate mode: pulse (strk_sio:strk_number+1) relay for each user expected to enter" }
                );

            modelBuilder.Entity<DoorMode>().HasData(
                new DoorMode { id = 1, name = "Disable", value = 1, description = "Disable the ACR, no REX" },
                new DoorMode { id = 2, name = "Unlock", value = 2, description = "Unlock (unlimited access)" },
                new DoorMode { id = 3, name = "Locked", value = 3, description = "Locked (no access, REX active)" },
                new DoorMode { id = 4, name = "facility code only", value = 4, description = "facility code only" },
                new DoorMode { id = 5, name = "Card only", value = 5, description = "Card only" },
                new DoorMode { id = 6, name = "PIN only", value = 6, description = "PIN only" },
                new DoorMode { id = 7, name = "Card and PIN", value = 7, description = "Card and PIN required" },
                new DoorMode { id = 8, name = "Card or PIN", value = 8, description = "Card or PIN required" }
            );

            modelBuilder.Entity<AntipassbackMode>().HasData(
                new AntipassbackMode { id = 1, name = "None", value = 0, description = "Do not check or alter anti-passback location. No antipassback rules." },
                new AntipassbackMode { id = 2, name = "Soft Anti-passback", value = 1, description = "Soft anti-passback: Accept any new location, change the user’s location to current reader, and generate an antipassback violation for an invalid entry." },
                new AntipassbackMode { id = 3, name = "Hard Anti-passback", value = 2, description = "Hard anti-passback: Check user location, if a valid entry is made, change user’s location to new location. If an invalid entry is attempted, do not grant access." },
                new AntipassbackMode { id = 4, name = "time-base Anti-passback - Last (Second)", value = 3, description = "Reader-based anti-passback using the ACR’s last valid user. Verify it’s not the same user within the time parameter specified within apb_delay." },
                new AntipassbackMode { id = 5, name = "time-base Anti-passback - History (Second)", value = 4, description = "Reader-based anti-passback using the access history from the cardholder database: Check user’s last ACR used, checks for same reader within a specified time (apb_delay). This requires the bSupportTimeApb flag be set in command 1105: Access Database Specification." },
                new AntipassbackMode { id = 6, name = "Area-base Anti-passback (Second)", value = 5, description = "Area based anti-passback: Check user’s current location, if it does not match the expected location then check the delay time (apb_delay). Change user’s location on entry. This requires the bSupportTimeApb flag be set in command 1105: Access Database Specification." },
                new AntipassbackMode { id = 7, name = "time-base Anti-passback - Last (Minute)", value = 6, description = "Same as \"time-base Anti-passback - Last (Second)\" but the apb_delay value is treated as minutes instead of seconds." },
                new AntipassbackMode { id = 8, name = "time-base Anti-passback - History (Minute)", value = 7, description = "Same as \"time-base Anti-passback - History (Second)\" but the apb_delay value is treated as minutes instead of seconds." },
                new AntipassbackMode { id = 9, name = "Area-base Anti-passback (Minute)", value = 8, description = "Same as \"Area-base Anti-passback (Second)\" but the apb_delay value is treated as minutes instead of seconds." }
            );

            modelBuilder.Entity<OsdpBaudrate>().HasData(
                  new OsdpBaudrate { id = 1, value = 0x01, name = "9600", description = "" },
                  new OsdpBaudrate { id = 2, value = 0x02, name = "19200", description = "" },
                  new OsdpBaudrate { id = 3, value = 0x03, name = "38400", description = "" },
                  new OsdpBaudrate { id = 4, value = 0x04, name = "115200", description = "" },
                  new OsdpBaudrate { id = 5, value = 0x05, name = "57600", description = "" },
                  new OsdpBaudrate { id = 6, value = 0x06, name = "230400", description = "" }
              );

            modelBuilder.Entity<OsdpAddress>().HasData(
                new OsdpAddress { id = 1, value = 0x00, name = "address 0", description = "" },
                new OsdpAddress { id = 2, value = 0x20, name = "address 1", description = "" },
                new OsdpAddress { id = 3, value = 0x40, name = "address 2", description = "" },
                new OsdpAddress { id = 4, value = 0x60, name = "address 3", description = "" }
            );


            modelBuilder.Entity<ReaderOutConfiguration>().HasData(
                new ReaderOutConfiguration { id = 1, name = "Ignore", value = 0, description = "Ignore data from alternate reader" },
                new ReaderOutConfiguration { id = 2, name = "Normal", value = 1, description = "Normal Access Reader (two read heads to the same processor)" }

                );


            #endregion

            #region User & Credentials

            modelBuilder.Entity<CardHolderAccessLevel>()
                .HasKey(x => new { x.access_level_id, x.cardholder_id });

            modelBuilder.Entity<CardHolderAccessLevel>()
                .HasOne(e => e.card_holder)
                .WithMany(e => e.access_levels)
                .HasForeignKey(e => e.cardholder_id)
                .HasPrincipalKey(e => e.user_id)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CardHolderAccessLevel>()
                .HasOne(e => e.access_level)
                .WithMany(e => e.cardholder_accesslevel)
                .HasForeignKey(e => e.access_level_id)
                .HasPrincipalKey(e => e.component_id)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CardHolder>()
                .HasMany(e => e.credentials)
                .WithOne(e => e.cardholder)
                .HasForeignKey(e => e.cardholder_id)
                .HasPrincipalKey(e => e.user_id)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CredentialFlag>()
                .HasData(
                new CredentialFlag {id=1,value=0x01,name="Active credential Record",description= "Active credential Record" },
                new CredentialFlag { id=2,value=0x02,name="Free One Antipassback",description="Allow one free anti-passback pass"},
                new CredentialFlag { id=3,value=0x04,name="Anti-passback exempt"},
                new CredentialFlag { id=4,value=0x08,name="Timing for disbled (ADA)",description= "Use timing parameters for the disabled (ADA)" },
                new CredentialFlag { id=5,value=0x10,name="pin Exempt",description= "PIN Exempt for \"Card & PIN\" ACR mode" },
                new CredentialFlag { id=6,value=0x20,name="No Change APB location",description= "Do not change apb_loc" },
                new CredentialFlag { id=7,value=0x40,name="No UpdateAsync Current Use Count",description= "Do not alter either the \"original\" or the \"current\" use count values" },
                new CredentialFlag { id=8,value=0x80,name="No UpdateAsync Current Use Count but Change Use Limit",description= "Do not alter the \"current\" use count but change the original use limit stored in the cardholder database" }
                );




            #endregion

            #region CardFormat

            modelBuilder.Entity<CardFormat>().HasData(
                new CardFormat
                {
                    id=1,
                    uuid = SeedDefaults.SystemGuid,
                    is_active=true,
                    name = "26 bits (No Fac)",
                    facility = -1,
                    offset = 0,
                    function_id = 1,
                    flags = 0,
                    bits = 26,
                    pe_ln = 13,
                    pe_loc = 0,
                    po_ln = 13,
                    po_loc = 13,
                    fc_ln = 0,
                    fc_loc = 0,
                    ch_ln = 16,
                    ch_loc = 9,
                    ic_ln = 0,
                    ic_loc = 0,
                    created_date=SeedDefaults.SystemDate,
                    updated_date=SeedDefaults.SystemDate,
                }
             );

            #endregion

            #region Location

            modelBuilder.Entity<Location>()
                .HasData(
                new Location { id = 1, component_id = 1, location_name = "Main", description = "Main location", created_date = SeedDefaults.SystemDate, updated_date = SeedDefaults.SystemDate, uuid = SeedDefaults.SystemGuid, is_active = true }
                );

            modelBuilder.Entity<Location>()
                .HasMany(l => l.hardwares)
                .WithOne(h => h.location)
                .HasForeignKey(f => f.location_id)
                .HasPrincipalKey(p => p.component_id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Location>()
                .HasMany(l => l.modules)
                .WithOne(h => h.location)
                .HasForeignKey(f => f.location_id)
                .HasPrincipalKey(p => p.component_id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Location>()
                .HasMany(l => l.control_points)
                .WithOne(h => h.location)
                .HasForeignKey(f => f.location_id)
                .HasPrincipalKey(p => p.component_id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Location>()
               .HasMany(l => l.monitor_points)
               .WithOne(h => h.location)
               .HasForeignKey(f => f.location_id)
               .HasPrincipalKey(p => p.component_id)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Location>()
               .HasMany(l => l.accesslevels)
               .WithOne(h => h.location)
               .HasForeignKey(f => f.location_id)
               .HasPrincipalKey(p => p.component_id)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Location>()
               .HasMany(l => l.areas)
               .WithOne(h => h.location)
               .HasForeignKey(f => f.location_id)
               .HasPrincipalKey(p => p.component_id)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Location>()
               .HasMany(l => l.cardholders)
               .WithOne(h => h.location)
               .HasForeignKey(f => f.location_id)
               .HasPrincipalKey(p => p.component_id)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Location>()
               .HasMany(l => l.doors)
               .WithOne(h => h.location)
               .HasForeignKey(f => f.location_id)
               .HasPrincipalKey(p => p.component_id)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Location>()
               .HasMany(l => l.monitor_groups)
               .WithOne(h => h.location)
               .HasForeignKey(f => f.location_id)
               .HasPrincipalKey(p => p.component_id)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Location>()
                .HasMany(l => l.transactions)
                .WithOne(c => c.location)
                 .HasForeignKey(f => f.location_id)
               .HasPrincipalKey(p => p.component_id)
               .OnDelete(DeleteBehavior.SetNull);


            modelBuilder.Entity<Location>()
              .HasMany(l => l.credentials)
              .WithOne(c => c.location)
              .HasForeignKey(f => f.location_id)
              .HasPrincipalKey(p => p.component_id)
              .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Location>()
              .HasMany(l => l.credentials)
              .WithOne(c => c.location)
              .HasForeignKey(f => f.location_id)
              .HasPrincipalKey(p => p.component_id)
              .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Location>()
              .HasMany(l => l.holidays)
              .WithOne(c => c.location)
              .HasForeignKey(f => f.location_id)
              .HasPrincipalKey(p => p.component_id)
              .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Location>()
              .HasMany(l => l.readers)
              .WithOne(c => c.location)
              .HasForeignKey(f => f.location_id)
              .HasPrincipalKey(p => p.component_id)
              .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Location>()
              .HasMany(l => l.request_exits)
              .WithOne(c => c.location)
              .HasForeignKey(f => f.location_id)
              .HasPrincipalKey(p => p.component_id)
              .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Location>()
              .HasMany(l => l.sensors)
              .WithOne(c => c.location)
              .HasForeignKey(f => f.location_id)
              .HasPrincipalKey(p => p.component_id)
              .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Location>()
              .HasMany(l => l.strikes)
              .WithOne(c => c.location)
              .HasForeignKey(f => f.location_id)
              .HasPrincipalKey(p => p.component_id)
              .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Location>()
                .HasMany(l => l.triggers)
                .WithOne(c => c.location)
                .HasForeignKey(f => f.location_id)
                .HasPrincipalKey(p => p.component_id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Location>()
                .HasMany(l => l.procedures)
                .WithOne(c => c.location)
                .HasForeignKey(f => f.location_id)
                .HasPrincipalKey(p => p.component_id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Location>()
                .HasMany(l => l.actions)
                .WithOne(c => c.location)
                .HasForeignKey(f => f.location_id)
                .HasPrincipalKey(p => p.component_id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Location>()
                .HasMany(l => l.intervals)
                .WithOne(c => c.location)
                .HasForeignKey(f => f.location_id)
                .HasPrincipalKey(p => p.component_id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Location>()
                .HasMany(l => l.timezones)
                .WithOne(c => c.location)
                .HasForeignKey(f => f.location_id)
                .HasPrincipalKey(p => p.component_id)
                .OnDelete(DeleteBehavior.Restrict);


            #endregion

            #region Transaction

            //modelBuilder.Entity<TransactionSourceType>()
            //    .HasKey(e => new { e.transction_source_value, e.transaction_type_value });

            modelBuilder.Entity<TransactionSourceType>()
                .HasOne(e => e.transaction_source)
                .WithMany(e => e.transaction_source_type)
                .HasForeignKey(e => e.transction_source_value)
                .HasPrincipalKey(e => e.value);

            modelBuilder.Entity<TransactionSourceType>()
                .HasOne(e => e.transaction_type)
                .WithMany(e => e.transaction_source_types)
                .HasForeignKey(e => e.transction_type_value)
                .HasPrincipalKey(e => e.value);

            modelBuilder.Entity<TransactionType>()
                .HasMany(e => e.transaction_codes)
                .WithOne(e => e.transaction_type)
                .HasForeignKey(e => e.transaction_type_value)
                .HasPrincipalKey(e => e.value);

            modelBuilder.Entity<TransactionSource>()
                .HasData(
                    new TransactionSource { id = 1, name = "SCP diagnostics", value = 0x00,source="hardware" },
                    new TransactionSource { id = 2, name = "SCP to HOST communication driver - not defined", value = 0x01,source="hardware" },
                    new TransactionSource { id = 3, name = "SCP local monitor points (tamper & power fault)", value = 0x02,source="hardware" },
                    new TransactionSource { id = 4, name = "SIO diagnostics", value = 0x03,source="modules" },
                    new TransactionSource { id = 5, name = "SIO communication driver", value = 0x04,source="modules" },
                    new TransactionSource { id = 6, name = "SIO cabinet tamper", value = 0x05,source="modules" },
                    new TransactionSource { id = 7, name = "SIO power monitor", value = 0x06,source="modules" },
                    new TransactionSource { id = 8, name = "Alarm monitor point", value = 0x07,source="Monitor Point" },
                    new TransactionSource { id = 9, name = "Output control point", value = 0x08,source="Control Point" },
                    new TransactionSource { id = 10, name = "Access Control Reader (ACR)", value = 0x09,source="door" },
                    new TransactionSource { id = 11, name = "ACR: reader tamper monitor", value = 0x0A,source="door" },
                    new TransactionSource { id = 12, name = "ACR: door position sensor", value = 0x0B,source="door" },
                    new TransactionSource { id = 13, name = "ACR: 1st \"Request to exit\" input", value = 0x0D,source="door" },
                    new TransactionSource { id = 14, name = "ACR: 2nd \"Request to exit\" input", value = 0x0E,source="door" },
                    new TransactionSource { id = 15, name = "time zone", value = 0x0F,source="time zone" },
                    new TransactionSource { id = 16, name = "procedure (action list)", value = 0x10,source="procedure" },
                    new TransactionSource { id = 17, name = "trigger", value = 0x11,source="trigger" },
                    new TransactionSource { id = 18, name = "trigger variable", value = 0x12,source="trigger" },
                    new TransactionSource { id = 19, name = "Monitor point group", value = 0x13,source="Monitor point group" },
                    new TransactionSource { id = 20, name = "Access area", value = 0x14 ,source="Access area"},
                    new TransactionSource { id = 21, name = "ACR: the alternate reader's tamper monitor source_number", value = 0x15,source="door" },
                    new TransactionSource { id = 22, name = "Login Service", value = 0x18,source="hardware" }
                );

            modelBuilder.Entity<TransactionType>()
                .HasData(
                    new TransactionType { id = 1, name = "System", value = 0x01 },
                    new TransactionType { id = 2, name = "SIO communication status report",  value = 0x02 },
                    new TransactionType { id = 3, name = "Binary card data",  value = 0x03 },
                    new TransactionType { id = 4, name = "Card data",  value = 0x04 },
                    new TransactionType { id = 5, name = "Formatted card: facility code, card number ID, issue code",  value = 0x05 },
                    new TransactionType { id = 6, name = "Formatted card: card number only", value = 0x06 },
                    new TransactionType { id = 7, name = "Change-of-state", value = 0x07 },
                    new TransactionType { id = 8, name = "Exit request", value = 0x08 },
                    new TransactionType { id = 9, name = "door status monitor change-of-state", value = 0x09 },
                    new TransactionType { id = 10, name = "procedure (command list) log", value = 0x0A },
                    new TransactionType { id = 11, name = "User command request report", value = 0x0B },
                    new TransactionType { id = 12, name = "Change of state: trigger variable, time zone, & triggers", value = 0x0C },
                    new TransactionType { id = 13, name = "door mode change", value = 0x0D },
                    new TransactionType { id = 14, name = "Monitor point group status change", value = 0x0E },
                    new TransactionType { id = 15, name = "Access area", value = 0x0F },
                    new TransactionType { id = 16, name = "Extended user command", value = 0x12 },
                    new TransactionType { id = 17, name = "Use limit report", value = 0x13 },
                    new TransactionType { id = 18, name = "Web activity", value = 0x14 },
                    new TransactionType { id = 19, name = "Specify tranTypeCardFull (0x05) instead", value = 0x15 },
                    new TransactionType { id = 20, name = "Specify tranTypeCardID (0x06) instead", value = 0x16 },
                    new TransactionType { id = 21, name = "Operating mode change", value = 0x18 },
                    new TransactionType { id = 22, name = "Elevator Floor status CoS", value = 0x1A },
                    new TransactionType { id = 23, name = "File Download status", value = 0x1B },
                    new TransactionType { id = 24, name = "Elevator Floor Access transaction", value = 0x1D },
                    new TransactionType { id = 25, name = "Specify tranTypeCardFull (0x05) instead", value = 0x25 },
                    new TransactionType { id = 26, name = "Specify tranTypeCardID (0x06) instead", value = 0x26 },
                    new TransactionType { id = 27, name = "Specify tranTypeCardFull (0x05) instead", value = 0x35 },
                    new TransactionType { id = 28, name = "door extended feature stateless transition", value = 0x40 },
                    new TransactionType { id = 29, name = "door extended feature change-of-state", value = 0x41 },

                    // New
                    new TransactionType { id=30,name= "Formatted card and user PIN was captured at an ACR", value=0x42}
                );

            modelBuilder.Entity<TransactionSourceType>()
                .HasData(
                    // tranSrcScpDiag
                    new TransactionSourceType { id=1,transction_source_value=0x00,transction_type_value=0x01 },
                    new TransactionSourceType { id=2,transction_source_value=0x00,transction_type_value=0x14},
                    new TransactionSourceType { id=3,transction_source_value=0x00,transction_type_value=0x18},
                    new TransactionSourceType { id=4,transction_source_value=0x00,transction_type_value=0x1B},

                    // tranSrcScpLcl 
                    new TransactionSourceType { id=5,transction_source_value=0x02,transction_type_value=0x07},

                    // tranSrcSioDiag
                    //new TransactionSourceType { id=6,transction_source_value=0x03,transaction_type_value=0x02},

                    // tranSrcSioCom
                    new TransactionSourceType { id=7,transction_source_value=0x04,transction_type_value=0x02},

                    // tranSrcSioTmpr
                    new TransactionSourceType { id=8,transction_source_value=0x05,transction_type_value=0x07},

                    // tranSrcSioPwr
                    new TransactionSourceType { id = 9, transction_source_value = 0x06, transction_type_value = 0x07 },

                    // tranSrcMP
                    new TransactionSourceType { id = 10, transction_source_value = 0x07, transction_type_value = 0x07 },

                    // tranSrcCP
                    new TransactionSourceType { id = 11, transction_source_value = 0x08, transction_type_value = 0x07 },

                    // tranSrcACR
                    new TransactionSourceType { id = 12, transction_source_value = 0x09, transction_type_value = 0x03 },
                    new TransactionSourceType { id = 13, transction_source_value = 0x09, transction_type_value = 0x04 },
                    new TransactionSourceType { id = 14, transction_source_value = 0x09, transction_type_value = 0x05 },
                    new TransactionSourceType { id = 15, transction_source_value = 0x09, transction_type_value = 0x06 },
                    new TransactionSourceType { id = 16, transction_source_value = 0x09, transction_type_value = 0x08 },
                    new TransactionSourceType { id = 17, transction_source_value = 0x09, transction_type_value = 0x0B },
                    new TransactionSourceType { id = 18, transction_source_value = 0x09, transction_type_value = 0x0D },
                    new TransactionSourceType { id = 19, transction_source_value = 0x09, transction_type_value = 0x12 },
                    new TransactionSourceType { id = 20, transction_source_value = 0x09, transction_type_value = 0x13 },
                    new TransactionSourceType { id = 21, transction_source_value = 0x09, transction_type_value = 0x1A },
                    new TransactionSourceType { id = 22, transction_source_value = 0x09, transction_type_value = 0x1D },
                    new TransactionSourceType { id = 23, transction_source_value = 0x09, transction_type_value = 0x40 },
                    new TransactionSourceType { id = 24, transction_source_value = 0x09, transction_type_value = 0x41 },
                    new TransactionSourceType { id = 25, transction_source_value = 0x09, transction_type_value = 0x13 },

                    // tranSrcAcrTmpr
                    new TransactionSourceType { id = 26, transction_source_value = 0x0A, transction_type_value = 0x07 },

                    // tranSrcAcrDoor
                    new TransactionSourceType { id = 27, transction_source_value = 0x0B, transction_type_value = 0x09 },

                    // tranSrcAcrRex0
                    new TransactionSourceType { id = 28, transction_source_value = 0x0D, transction_type_value = 0x07 },

                    // tranSrcAcrRex1
                    new TransactionSourceType { id = 29, transction_source_value = 0x0E, transction_type_value = 0x07 },

                    // tranSrcTimeZone
                    new TransactionSourceType { id = 30, transction_source_value = 0x0F, transction_type_value = 0x0C },

                    // tranSrcProcedure
                    new TransactionSourceType { id = 31, transction_source_value = 0x10, transction_type_value = 0x0A },

                    // tranSrcTrigger
                    new TransactionSourceType { id = 32, transction_source_value = 0x11, transction_type_value = 0x0C },

                    // tranSrcTrigVar
                    new TransactionSourceType { id = 33, transction_source_value = 0x12, transction_type_value = 0x0C },

                    // tranSrcMPG
                    new TransactionSourceType { id = 34, transction_source_value = 0x13, transction_type_value = 0x0E },

                    // tranSrcArea
                    new TransactionSourceType { id = 35, transction_source_value = 0x14, transction_type_value = 0x0F },

                    // tranSrcAcrTmprAlt
                    new TransactionSourceType { id = 36, transction_source_value = 0x15, transction_type_value = 0x07 },

                    // tranSrcLoginService
                    new TransactionSourceType { id = 37, transction_source_value = 0x18, transction_type_value = 0x07 }


                );

            modelBuilder.Entity<TransactionCode>()
                .HasData(

                    // TypeSys
                    new TransactionCode { id=1,name= "SCP power-up diagnostics",description= "SCP power-up diagnostics", value =1,transaction_type_value=0x01 },
                    new TransactionCode { id = 2, name = "Host communications offline", description= "Host communications offline", value = 2, transaction_type_value = 0x01 },
                    new TransactionCode { id = 3, name = "Host communications online",description= "Host communications online", value = 3, transaction_type_value = 0x01 },
                    new TransactionCode { id = 4, name = "transaction count exceeds the preset limit",description= "transaction count exceeds the preset limit", value = 4, transaction_type_value = 0x01 },
                    new TransactionCode { id = 5, name = "Configuration database save complete",description= "Configuration database save complete", value = 5, transaction_type_value = 0x01 },
                    new TransactionCode { id = 6, name = "Card database save complete", description= "Card database save complete", value = 6, transaction_type_value = 0x01 },
                    new TransactionCode { id = 7, name = "Card database cleared due to SRAM buffer overflow",description= "Card database cleared due to SRAM buffer overflow", value = 7, transaction_type_value = 0x01 },

                    // TypeSioComm
                    new TransactionCode { id = 8, name = "Disabled",description= "Communication disabled (result of host command)", value = 1, transaction_type_value = 0x02 },
                    new TransactionCode { id = 9, name = "Offline",description= "Timeout (no/bad response from unit)", value = 2, transaction_type_value = 0x02 },
                    new TransactionCode { id = 10, name = "Offline",description= "Invalid identification from SIO", value = 3, transaction_type_value = 0x02 },
                    new TransactionCode { id = 11, name = "Offline",description="command too long", value = 4, transaction_type_value = 0x02 },
                    new TransactionCode { id = 12, name = "Online",description= "Normal connection", value = 5, transaction_type_value = 0x02 },
                    new TransactionCode { id = 13, name = "hexLoad report",description= "ser_num is address loaded (-1 = last record)", value = 6, transaction_type_value = 0x02 },

                    // TypeCardBin
                    new TransactionCode { id = 14, name = "Access denied",description= "Invalid card format", value = 1, transaction_type_value = 0x03 },

                    // TypeCardBcd
                    new TransactionCode { id = 15, name = "Access denied",description= "Invalid card format, forward read", value = 1, transaction_type_value = 0x04 },
                    new TransactionCode { id = 16, name = "Access denied",description= "Invalid card format, reverse read", value = 2, transaction_type_value = 0x04 },

                    // TypeCardFull
                    new TransactionCode { id = 17, name = "Request rejected",description= "Access point \"locked\"", value = 1, transaction_type_value = 0x05 },
                    new TransactionCode { id = 18, name = "Request accepted",description= "Access point \"unlocked\"", value = 2, transaction_type_value = 0x05 },
                    new TransactionCode { id = 19, name = "Request rejected",description= "Invalid facility code", value = 3, transaction_type_value = 0x05 },
                    new TransactionCode { id = 20, name = "Request rejected",description= "Invalid facility code extension", value = 4, transaction_type_value = 0x05 },
                    new TransactionCode { id = 21, name = "Request rejected",description= "Not in card file", value = 5, transaction_type_value = 0x05 },
                    new TransactionCode { id = 22, name = "Request rejected",description= "Invalid issue code", value = 6, transaction_type_value = 0x05 },
                    new TransactionCode { id = 23, name = "Request granted",description= "facility code verified, not used", value = 7, transaction_type_value = 0x05 },
                    new TransactionCode { id = 24, name = "Request granted",description= "facility code verified, door used", value = 8, transaction_type_value = 0x05 },
                    new TransactionCode { id = 25, name = "Access denied",description= "Asked for host approval, then timed out", value = 9, transaction_type_value = 0x05 },
                    new TransactionCode { id = 26, name = "Reporting that this card is \"about to get access granted\"",description= "Reporting that this card is \"about to get access granted\"", value = 10, transaction_type_value = 0x05 },
                    new TransactionCode { id = 27, name = "Access denied",description= "Count exceeded", value = 11, transaction_type_value = 0x05 },
                    new TransactionCode { id = 28, name = "Access denied",description= "Asked for host approval, then host denied", value = 12, transaction_type_value = 0x05 },
                    new TransactionCode { id = 29, name = "Request rejected",description= "Airlock is busy", value = 13, transaction_type_value = 0x05 },

                    // TypeCardID
                    new TransactionCode { id = 30, name = "Request rejected", description= "Deactivated card", value = 1, transaction_type_value = 0x06 },
                    new TransactionCode { id = 31, name = "Request rejected",description= "Before activation date", value = 2, transaction_type_value = 0x06 },
                    new TransactionCode { id = 32, name = "Request rejected", description= "After expiration date", value = 3, transaction_type_value = 0x06 },
                    new TransactionCode { id = 33, name = "Request rejected",description= "Invalid time", value = 4, transaction_type_value = 0x06 },
                    new TransactionCode { id = 34, name = "Request rejected",description="Invalid PIN", value = 5, transaction_type_value = 0x06 },
                    new TransactionCode { id = 35, name = "Request rejected", description= "Anti-passback violation", value = 6, transaction_type_value = 0x06 },
                    new TransactionCode { id = 36, name = "Request granted",description= "APB violation, not used", value = 7, transaction_type_value = 0x06 },
                    new TransactionCode { id = 37, name = "Request granted", description= "APB violation, used", value = 8, transaction_type_value = 0x06 },
                    new TransactionCode { id = 38, name = "Request rejected", description= "Duress code detected", value = 9, transaction_type_value = 0x06 },
                    new TransactionCode { id = 39, name = "Request granted", description= "Duress, used", value = 10, transaction_type_value = 0x06 },
                    new TransactionCode { id = 40, name = "Request granted", description= "Duress, not used", value = 11, transaction_type_value = 0x06 },
                    new TransactionCode { id = 41, name = "Request granted",description= "Full test, not used", value = 12, transaction_type_value = 0x06 },
                    new TransactionCode { id = 42, name = "Request granted",description= "Full test, used", value = 13, transaction_type_value = 0x06 },
                    new TransactionCode { id = 43, name = "Request denied",description= "Never allowed at this reader (all Tz's = 0)", value = 14, transaction_type_value = 0x06 },
                    new TransactionCode { id = 44, name = "Request denied", description= "No second card presented", value = 15, transaction_type_value = 0x06 },
                    new TransactionCode { id = 45, name = "Request denied", description= "Occupancy limit reached", value = 16, transaction_type_value = 0x06 },
                    new TransactionCode { id = 46, name = "Request denied", description= "The area is NOT enabled", value = 17, transaction_type_value = 0x06 },
                    new TransactionCode { id = 47, name = "Request denied", description= "Use limit", value = 18, transaction_type_value = 0x06 },
                    new TransactionCode { id = 48, name = "Granting access",description= "Used/not used transaction will follow", value = 21, transaction_type_value = 0x06 },
                    new TransactionCode { id = 49, name = "Request rejected",description= "No escort card presented", value = 24, transaction_type_value = 0x06 },
                    new TransactionCode { id = 50, name = "Reserved",description= "Reserved", value = 25, transaction_type_value = 0x06 },
                    new TransactionCode { id = 51, name = "Reserved",description= "Reserved", value = 26, transaction_type_value = 0x06 },
                    new TransactionCode { id = 52, name = "Reserved", description= "Reserved", value = 27, transaction_type_value = 0x06 },
                    new TransactionCode { id = 53, name = "Request rejected",description= "Airlock is busy", value = 29, transaction_type_value = 0x06 },
                    new TransactionCode { id = 54, name = "Request rejected",description= "Incomplete CARD & PIN sequence", value = 30, transaction_type_value = 0x06 },
                    new TransactionCode { id = 55, name = "Request granted",description= "Double-card event", value = 31, transaction_type_value = 0x06 },
                    new TransactionCode { id = 56, name = "Request granted", description= "Double-card event while in uncontrolled state (locked/unlocked)", value = 32, transaction_type_value = 0x06 },
                    new TransactionCode { id = 57, name = "Granting access", description= "Requires escort, pending escort card", value = 39, transaction_type_value = 0x06 },
                    new TransactionCode { id = 58, name = "Request rejected", description= "Violates minimum occupancy count", value = 40, transaction_type_value = 0x06 },
                    new TransactionCode { id = 59, name = "Request rejected",description= "Card pending at another reader", value = 41, transaction_type_value = 0x06 },

                    // TypeHostCardFullPin 
                    new TransactionCode { id = 60, name = "Request rejected", description = "Card pending at another reader", value = 41, transaction_type_value = 0x42 },

                    // TypeHostCardFullPin 
                    //new tran_code { id=61,name= "Reporting that this Card and PIN pair is \"requesting access\"",tran_code_desc= "Reporting that this Card and PIN pair is \"requesting access\"",value=1,transaction_type_value=0x66 },

                    // TypeCoS
                    new TransactionCode { id = 62, name = "Disconnected", description = "Disconnected (from an input point ID)", value = 1, transaction_type_value = 0x07 },
                    new TransactionCode { id = 63, name = "Offline", description = "Unknown (offline): no report from the ID", value = 2, transaction_type_value = 0x07 },
                    new TransactionCode { id = 64, name = "Secure", description = "Secure (or deactivate relay)", value = 3, transaction_type_value = 0x07 },
                    new TransactionCode { id = 65, name = "Alarm", description = "Alarm (or activated relay: perm or temp)", value = 4, transaction_type_value = 0x07 },
                    new TransactionCode { id = 66, name = "Fault", description = "Fault", value = 5, transaction_type_value = 0x07 },
                    new TransactionCode { id = 67, name = "Exit delay in progress", description = "Exit delay in progress", value = 6, transaction_type_value = 0x07 },
                    new TransactionCode { id = 68, name = "Entry delay in progress", description = "Entry delay in progress", value = 7, transaction_type_value = 0x07 },

                     // TypeREX
                     new TransactionCode { id = 69, name = "Exit cycle", description = "door use not verified", value = 1, transaction_type_value = 0x08 },
                     new TransactionCode { id = 70, name = "Exit cycle", description = "door not used", value = 2, transaction_type_value = 0x08 },
                     new TransactionCode { id = 71, name = "Exit cycle", description = "door used", value = 3, transaction_type_value = 0x08 },
                     new TransactionCode { id = 72, name = "Host initiated request", description = "door use not verified", value = 4, transaction_type_value = 0x08 },
                     new TransactionCode { id = 73, name = "Host initiated request", description = "door not used", value = 5, transaction_type_value = 0x08 },
                     new TransactionCode { id = 74, name = "Host initiated request", description = "door used", value = 6, transaction_type_value = 0x08 },
                     new TransactionCode { id = 75, name = "Exit cycle", description = "Started", value = 9, transaction_type_value = 0x08 },

                     // TypeCoSDoor
                     new TransactionCode { id = 76, name = "Disconnected", description = "Disconnected", value = 1, transaction_type_value = 0x09 },
                     new TransactionCode { id = 77, name = "Unknown _RS bits: last known status", description = "Unknown _RS bits: last known status", value = 2, transaction_type_value = 0x09 },
                     new TransactionCode { id = 78, name = "Secure", description = "Secure", value = 3, transaction_type_value = 0x09 },
                     new TransactionCode { id = 79, name = "Alarm", description = "Alarm (forced, held open or both)", value = 4, transaction_type_value = 0x09 },
                     new TransactionCode { id = 80, name = "Fault", description = "Fault (fault type is encoded in door_status byte)", value = 5, transaction_type_value = 0x09 },

                     // TypeProcedure
                     new TransactionCode { id = 81, name = "Cancel procedure (abort delay)", description = "Cancel procedure (abort delay)", value = 1, transaction_type_value = 0x0A },
                     new TransactionCode { id = 82, name = "Execute procedure (start new)", description = "Execute procedure (start new)", value = 2, transaction_type_value = 0x0A },
                     new TransactionCode { id = 83, name = "Resume procedure, if paused", description = "Resume procedure, if paused", value = 3, transaction_type_value = 0x0A },
                     new TransactionCode { id = 84, name = "Execute procedure with prefix 256 actions", description = "Execute procedure with prefix 256 actions", value = 4, transaction_type_value = 0x0A },
                     new TransactionCode { id = 85, name = "Execute procedure with prefix 512 actions", description = "Execute procedure with prefix 512 actions", value = 5, transaction_type_value = 0x0A },
                     new TransactionCode { id = 86, name = "Execute procedure with prefix 1024 actions", description = "Execute procedure with prefix 1024 actions", value = 6, transaction_type_value = 0x0A },
                     new TransactionCode { id = 87, name = "Resume procedure with prefix 256 actions", description = "Resume procedure with prefix 256 actions", value = 7, transaction_type_value = 0x0A },
                     new TransactionCode { id = 88, name = "Resume procedure with prefix 512 actions", description = "Resume procedure with prefix 512 actions", value = 8, transaction_type_value = 0x0A },
                     new TransactionCode { id = 89, name = "Resume procedure with prefix 1024 actions", description = "Resume procedure with prefix 1024 actions", value = 9, transaction_type_value = 0x0A },
                     new TransactionCode { id = 90, name = "command was issued to procedure with no actions - (NOP)", description = "command was issued to procedure with no actions - (NOP)", value = 10, transaction_type_value = 0x0A },


                     // TypeUserCmnd
                     new TransactionCode { id = 91, name = "command entered by the user", description = "command entered by the user", value = 10, transaction_type_value = 0x0B },

                     // TypeActivate
                     new TransactionCode { id = 92, name = "Became inactive", description = "Became inactive", value = 1, transaction_type_value = 0x0C },
                     new TransactionCode { id = 93, name = "Became active", description = "Became active", value = 2, transaction_type_value = 0x0C },

                     // TypeAcr
                     new TransactionCode { id = 94, name = "Disabled", description = "Disabled", value = 1, transaction_type_value = 0x0D },
                     new TransactionCode { id = 95, name = "Unlocked", description = "Unlocked", value = 2, transaction_type_value = 0x0D },
                     new TransactionCode { id = 96, name = "Locked", description = "Locked (exit request enabled)", value = 3, transaction_type_value = 0x0D },
                     new TransactionCode { id = 97, name = "facility code only", description = "facility code only", value = 4, transaction_type_value = 0x0D },
                     new TransactionCode { id = 98, name = "Card only", description = "Card only", value = 5, transaction_type_value = 0x0D },
                     new TransactionCode { id = 99, name = "PIN only", description = "PIN only", value = 6, transaction_type_value = 0x0D },
                     new TransactionCode { id = 100, name = "Card and PIN", description = "Card and PIN", value = 7, transaction_type_value = 0x0D },
                     new TransactionCode { id = 101, name = "PIN or card", description = "PIN or card", value = 8, transaction_type_value = 0x0D },

                     // TypeMPG
                     new TransactionCode { id = 102, name = "First disarm command executed", description = "First disarm command executed (mask_count was 0, all MPs got masked)", value = 1, transaction_type_value = 0x0E },
                     new TransactionCode { id = 103, name = "Subsequent disarm command executed", description = "Subsequent disarm command executed (mask_count incremented, MPs already masked)", value = 2, transaction_type_value = 0x0E },
                     new TransactionCode { id = 104, name = "Override command: armed", description = "Override command: armed (mask_count cleared, all points unmasked)", value = 3, transaction_type_value = 0x0E },
                     new TransactionCode { id = 105, name = "Override command: disarmed", description = "Override command: disarmed (mask_count set, unmasked all points)", value = 4, transaction_type_value = 0x0E },
                     new TransactionCode { id = 106, name = "Force arm command, MPG armed", description = "Force arm command, MPG armed, (may have active zones, mask_count is now zero)", value = 5, transaction_type_value = 0x0E },
                     new TransactionCode { id = 107, name = "Force arm command, MPG not armed", description = "Force arm command, MPG not armed (mask_count decremented)", value = 6, transaction_type_value = 0x0E },
                     new TransactionCode { id = 108, name = "Standard arm command, MPG armed", description = "Standard arm command, MPG armed (did not have active zones, mask_count is now zero)", value = 7, transaction_type_value = 0x0E },
                     new TransactionCode { id = 109, name = "Standard arm command, MPG did not arm", description = "Standard arm command, MPG did not arm, (had active zones, mask_count unchanged)d", value = 8, transaction_type_value = 0x0E },
                     new TransactionCode { id = 110, name = "Standard arm command, MPG still armed", description = "Standard arm command, MPG still armed, (mask_count decremented)", value = 9, transaction_type_value = 0x0E },
                     new TransactionCode { id = 111, name = "Override arm command, MPG armed", description = "Override arm command, MPG armed (mask_count is now zero)", value = 10, transaction_type_value = 0x0E },
                     new TransactionCode { id = 112, name = "Override arm command, MPG did not arm", description = "Override arm command, MPG did not arm, (mask_count decremented)", value = 11, transaction_type_value = 0x0E },

                     // TypeArea
                     new TransactionCode { id = 113, name = "Area disabled", description = "Area disabled", value = 1, transaction_type_value = 0x0F },
                     new TransactionCode { id = 114, name = "Area enabled", description = "Area enabled", value = 2, transaction_type_value = 0x0F },
                     new TransactionCode { id = 115, name = "Occupancy count reached zero", description = "Occupancy count reached zero", value = 3, transaction_type_value = 0x0F },
                     new TransactionCode { id = 116, name = "Occupancy count reached the \"downward-limit\"", description = "Occupancy count reached the \"downward-limit\"", value = 4, transaction_type_value = 0x0F },
                     new TransactionCode { id = 117, name = "Occupancy count reached the \"upward-limit\"", description = "Occupancy count reached the \"upward-limit\"", value = 5, transaction_type_value = 0x0F },
                     new TransactionCode { id = 118, name = "Occupancy count reached the \"max-occupancy-limit\"", description = "Occupancy count reached the \"max-occupancy-limit\"", value = 6, transaction_type_value = 0x0F },
                     new TransactionCode { id = 119, name = "Multi-occupancy mode changed", description = "Multi-occupancy mode changed", value = 7, transaction_type_value = 0x0F },

                    // TypeWebActivity
                    // Web Activity transaction Codes (transaction_type_value = 0x14)
                    new TransactionCode { id = 120, name = "Save home notes", description = "Save home notes", value = 1, transaction_type_value = 0x14 },
                    new TransactionCode { id = 121, name = "Save network settings", description = "Save network settings", value = 2, transaction_type_value = 0x14 },
                    new TransactionCode { id = 122, name = "Save host communication settings", description = "Save host communication settings", value = 3, transaction_type_value = 0x14 },
                    new TransactionCode { id = 123, name = "Add user", description = "Add user", value = 4, transaction_type_value = 0x14 },
                    new TransactionCode { id = 124, name = "DeleteAsync user", description = "DeleteAsync user", value = 5, transaction_type_value = 0x14 },
                    new TransactionCode { id = 125, name = "Modify user", description = "Modify user", value = 6, transaction_type_value = 0x14 },
                    new TransactionCode { id = 126, name = "Save password strength and session timer", description = "Save password strength and session timer", value = 7, transaction_type_value = 0x14 },
                    new TransactionCode { id = 127, name = "Save web server options", description = "Save web server options", value = 8, transaction_type_value = 0x14 },
                    new TransactionCode { id = 128, name = "Save time server settings", description = "Save time server settings", value = 9, transaction_type_value = 0x14 },
                    new TransactionCode { id = 129, name = "Auto save timer settings", description = "Auto save timer settings", value = 10, transaction_type_value = 0x14 },
                    new TransactionCode { id = 130, name = "Load certificate", description = "Load certificate", value = 11, transaction_type_value = 0x14 },
                    new TransactionCode { id = 131, name = "Logged out by link", description = "Logged out by link", value = 12, transaction_type_value = 0x14 },
                    new TransactionCode { id = 132, name = "Logged out by timeout", description = "Logged out by timeout", value = 13, transaction_type_value = 0x14 },
                    new TransactionCode { id = 133, name = "Logged out by user", description = "Logged out by user", value = 14, transaction_type_value = 0x14 },
                    new TransactionCode { id = 134, name = "Logged out by apply", description = "Logged out by apply", value = 15, transaction_type_value = 0x14 },
                    new TransactionCode { id = 135, name = "Invalid login", description = "Invalid login", value = 16, transaction_type_value = 0x14 },
                    new TransactionCode { id = 136, name = "Successful login", description = "Successful login", value = 17, transaction_type_value = 0x14 },
                    new TransactionCode { id = 137, name = "Network diagnostic saved", description = "Network diagnostic saved", value = 18, transaction_type_value = 0x14 },
                    new TransactionCode { id = 138, name = "Card DB size saved", description = "Card DB size saved", value = 19, transaction_type_value = 0x14 },
                    new TransactionCode { id = 139, name = "Diagnostic page saved", description = "Diagnostic page saved", value = 21, transaction_type_value = 0x14 },
                    new TransactionCode { id = 140, name = "Security options page saved", description = "Security options page saved", value = 22, transaction_type_value = 0x14 },
                    new TransactionCode { id = 141, name = "Add-on package page saved", description = "Add-on package page saved", value = 23, transaction_type_value = 0x14 },
                    //new tran_code { id = 142, name = "Not used", tran_code_desc = "Not used", value = 24, transaction_type_value = 0x14 },
                    //new tran_code { id = 143, name = "Not used", tran_code_desc = "Not used", value = 25, transaction_type_value = 0x14 },
                    //new tran_code { id = 144, name = "Not used", tran_code_desc = "Not used", value = 26, transaction_type_value = 0x14 },
                    new TransactionCode { id = 145, name = "Invalid login limit reached", description = "Invalid login limit reached", value = 27, transaction_type_value = 0x14 },
                    new TransactionCode { id = 146, name = "firmware download initiated", description = "firmware download initiated", value = 28, transaction_type_value = 0x14 },
                    new TransactionCode { id = 147, name = "Advanced networking routes saved", description = "Advanced networking routes saved", value = 29, transaction_type_value = 0x14 },
                    new TransactionCode { id = 148, name = "Advanced networking reversion timer started", description = "Advanced networking reversion timer started", value = 30, transaction_type_value = 0x14 },
                    new TransactionCode { id = 149, name = "Advanced networking reversion timer elapsed", description = "Advanced networking reversion timer elapsed", value = 31, transaction_type_value = 0x14 },
                    new TransactionCode { id = 150, name = "Advanced networking route changes reverted", description = "Advanced networking route changes reverted", value = 32, transaction_type_value = 0x14 },
                    new TransactionCode { id = 151, name = "Advanced networking route changes cleared", description = "Advanced networking route changes cleared", value = 33, transaction_type_value = 0x14 },
                    new TransactionCode { id = 152, name = "Certificate generation started", description = "Certificate generation started", value = 34, transaction_type_value = 0x14 },


                    // TypeOperatingMode
                    new TransactionCode { id = 153, name = "Operating mode changed to mode 0", description = "Operating mode changed to mode 0", value = 1, transaction_type_value = 0x18 },
                    new TransactionCode { id = 154, name = "Operating mode changed to mode 1", description = "Operating mode changed to mode 1", value = 2, transaction_type_value = 0x18 },
                    new TransactionCode { id = 155, name = "Operating mode changed to mode 2", description = "Operating mode changed to mode 2", value = 3, transaction_type_value = 0x18 },
                    new TransactionCode { id = 156, name = "Operating mode changed to mode 3", description = "Operating mode changed to mode 3", value = 4, transaction_type_value = 0x18 },
                    new TransactionCode { id = 157, name = "Operating mode changed to mode 4", description = "Operating mode changed to mode 4", value = 5, transaction_type_value = 0x18 },
                    new TransactionCode { id = 158, name = "Operating mode changed to mode 5", description = "Operating mode changed to mode 5", value = 6, transaction_type_value = 0x18 },
                    new TransactionCode { id = 159, name = "Operating mode changed to mode 6", description = "Operating mode changed to mode 6", value = 7, transaction_type_value = 0x18 },
                    new TransactionCode { id = 160, name = "Operating mode changed to mode 7", description = "Operating mode changed to mode 7", value = 8, transaction_type_value = 0x18 },

                    // TypeCoSElevator
                    new TransactionCode { id = 161, name = "Secure", description = "Floor status is secure", value = 1, transaction_type_value = 0x1A },
                    new TransactionCode { id = 162, name = "Public", description = "Floor status is public", value = 2, transaction_type_value = 0x1A },
                    new TransactionCode { id = 163, name = "Disabled (override)", description = "Floor status is disabled (override)", value = 3, transaction_type_value = 0x1A },

                    // TypeFileDownloadStatus
                    new TransactionCode { id = 164, name = "File transfer success", description = "File transfer success", value = 1, transaction_type_value = 0x1B },
                    new TransactionCode { id = 165, name = "File transfer error", description = "File transfer error", value = 2, transaction_type_value = 0x1B },
                    new TransactionCode { id = 166, name = "File delete successful", description = "File delete successful", value = 3, transaction_type_value = 0x1B },
                    new TransactionCode { id = 167, name = "File delete unsuccessful", description = "File delete unsuccessful", value = 4, transaction_type_value = 0x1B },
                    new TransactionCode { id = 168, name = "OSDP file transfer complete (primary ACR)", description = "OSDP file transfer complete (primary ACR) - look at source number for ACR number", value = 5, transaction_type_value = 0x1B },
                    new TransactionCode { id = 169, name = "OSDP file transfer error (primary ACR)", description = "OSDP file transfer error (primary ACR) - look at source number for ACR number", value = 6, transaction_type_value = 0x1B },
                    new TransactionCode { id = 170, name = "OSDP file transfer complete (alternate ACR)", description = "OSDP file transfer complete (alternate ACR) - look at source number for ACR number", value = 7, transaction_type_value = 0x1B },
                    new TransactionCode { id = 171, name = "OSDP file transfer error (alternate ACR)", description = "OSDP file transfer error (alternate ACR) - look at source number for ACR number", value = 8, transaction_type_value = 0x1B },

                    // TypeCoSElevatorAccess
                    new TransactionCode { id = 172, name = "Elevator access", description = "Elevator access", value = 1, transaction_type_value = 0x1D },

                    // TypeAcrExtFeatureStls
                    new TransactionCode { id = 173, name = "Extended status updated", description = "Extended status updated", value = 1, transaction_type_value = 0x40 },

                    // TypeAcrExtFeatureCoS
                    new TransactionCode { id = 174, name = "Secure / Inactive", description = "Secure / Inactive", value = 3, transaction_type_value = 0x41 },
                    new TransactionCode { id = 175, name = "Alarm / Active", description = "Alarm / Active", value = 4, transaction_type_value = 0x41 }

                );

            modelBuilder.Entity<Transaction>()
                .HasMany(x => x.transaction_flag)
                .WithOne(x => x.transaction);

            //// TypeSys
            
            //modelBuilder.Entity<transaction>()
            //    .HasOne(t => t.TypeSys)
            //    .WithOne(t => t.transaction); 

            //modelBuilder.Entity<TypeSys>()
            //    .HasMany(x => x.transaction_flag)
            //    .WithOne(x => x.TypeSys);

            //// hardware_type Web Activity

            //modelBuilder.Entity<transaction>()
            //    .HasOne(t => t.TypeWebActivity)
            //    .WithOne(t => t.transaction);

            //modelBuilder.Entity<TypeWebActivity>()
            //    .HasMany(x => x.transaction_flag)
            //    .WithOne(x => x.TypeWebActivity);

            //// hardware_type File Download

            //modelBuilder.Entity<transaction>()
            //    .HasOne(t => t.TypeFileDownloadStatus)
            //    .WithOne(t => t.transaction);

            //modelBuilder.Entity<TypeFileDownloadStatus>()
            //    .HasMany(x => x.transaction_flag)
            //    .WithOne(x => x.TypeFileDownloadStatus);

            //// hardware_type Cos

            //modelBuilder.Entity<transaction>()
            //    .HasOne(t => t.TypeCos)
            //    .WithOne(t => t.transaction);

            //modelBuilder.Entity<TypeCos>()
            //    .HasMany(x => x.transaction_flag)
            //    .WithOne(x => x.TypeCos);

            //// TypeSioDiag

            //modelBuilder.Entity<transaction>()
            //    .HasOne(t => t.TypeSioDiag)
            //    .WithOne(t => t.transaction);

            //modelBuilder.Entity<TypeSioDiag>()
            //    .HasMany(x => x.transaction_flag)
            //    .WithOne(x => x.TypeSioDiag);

            //// TypeSioComm

            //modelBuilder.Entity<transaction>()
            //    .HasOne(t => t.TypeSioComm)
            //    .WithOne(t => t.transaction);

            //modelBuilder.Entity<HIDAeroService.Entity.TypeSioComm>()
            //    .HasMany(x => x.transaction_flag)
            //    .WithOne(x => x.TypeSioComm);

            //// TypeCardBin

            //modelBuilder.Entity<transaction>()
            //    .HasOne(t => t.TypeCardBin)
            //    .WithOne(t => t.transaction);

            //modelBuilder.Entity<HIDAeroService.Entity.TypeCardBin>()
            //    .HasMany(x => x.transaction_flag)
            //    .WithOne(x => x.TypeCardBin);

            //// TypeCardBcd

            //modelBuilder.Entity<transaction>()
            //    .HasOne(t => t.TypeCardBcd)
            //    .WithOne(t => t.transaction);

            //modelBuilder.Entity<HIDAeroService.Entity.TypeCardBcd>()
            //    .HasMany(x => x.transaction_flag)
            //    .WithOne(x => x.TypeCardBcd);

            //// TypeCardFull
            //modelBuilder.Entity<transaction>()
            //    .HasOne(t => t.TypeCardFull)
            //    .WithOne(t => t.transaction);

            //modelBuilder.Entity<HIDAeroService.Entity.TypeCardFull>()
            //    .HasMany(x => x.transaction_flag)
            //    .WithOne(x => x.TypeCardFull);


            //// TypeCardID
            //modelBuilder.Entity<transaction>()
            //    .HasOne(t => t.TypeCardID)
            //    .WithOne(t => t.transaction);

            //modelBuilder.Entity<HIDAeroService.Entity.TypeCardID>()
            //    .HasMany(x => x.transaction_flag)
            //    .WithOne(x => x.TypeCardID);

            //// TypeREX
            //modelBuilder.Entity<transaction>()
            //    .HasOne(t => t.TypeREX)
            //    .WithOne(t => t.transaction);

            //modelBuilder.Entity<HIDAeroService.Entity.TypeREX>()
            //    .HasMany(x => x.transaction_flag)
            //    .WithOne(x => x.TypeREX);

            //// TypeUserCmnd
            //modelBuilder.Entity<transaction>()
            //    .HasOne(t => t.TypeUserCmnd)
            //    .WithOne(t => t.transaction);

            //modelBuilder.Entity<HIDAeroService.Entity.TypeUserCmnd>()
            //    .HasMany(x => x.transaction_flag)
            //    .WithOne(x => x.TypeUserCmnd);

            ////TypeAcr
            //modelBuilder.Entity<transaction>()
            //   .HasOne(t => t.TypeAcr)
            //   .WithOne(t => t.transaction);

            //modelBuilder.Entity<HIDAeroService.Entity.TypeAcr>()
            //    .HasMany(x => x.transaction_flag)
            //    .WithOne(x => x.TypeAcr);

            //// TypeUseLimit

            //modelBuilder.Entity<transaction>()
            //   .HasOne(t => t.TypeUseLimit)
            //   .WithOne(t => t.transaction);

            //modelBuilder.Entity<HIDAeroService.Entity.TypeUseLimit>()
            //    .HasMany(x => x.transaction_flag)
            //    .WithOne(x => x.TypeUseLimit);

            //// TypeCosElevator

            //modelBuilder.Entity<transaction>()
            //   .HasOne(t => t.TypeCosElevator)
            //   .WithOne(t => t.transaction);

            //modelBuilder.Entity<HIDAeroService.Entity.TypeCosElevator>()
            //    .HasMany(x => x.transaction_flag)
            //    .WithOne(x => x.TypeCosElevator);

            //// TypeCosElevatorAccess

            //modelBuilder.Entity<transaction>()
            //   .HasOne(t => t.TypeCosElevatorAccess)
            //   .WithOne(t => t.transaction);

            //modelBuilder.Entity<HIDAeroService.Entity.TypeCosElevatorAccess>()
            //    .HasMany(x => x.transaction_flag)
            //    .WithOne(x => x.TypeCosElevatorAccess);

            //// TypeAcrExtFeatureStls

            //modelBuilder.Entity<transaction>()
            //   .HasOne(t => t.TypeAcrExtFeatureStls)
            //   .WithOne(t => t.transaction);

            //modelBuilder.Entity<HIDAeroService.Entity.TypeAcrExtFeatureStls>()
            //    .HasMany(x => x.transaction_flag)
            //    .WithOne(x => x.TypeAcrExtFeatureStls);

            //// TypeAcrExtFeatureCoS

            //modelBuilder.Entity<transaction>()
            //   .HasOne(t => t.TypeAcrExtFeatureCoS)
            //   .WithOne(t => t.transaction);

            //modelBuilder.Entity<HIDAeroService.Entity.TypeAcrExtFeatureCoS>()
            //    .HasMany(x => x.transaction_flag)
            //    .WithOne(x => x.TypeAcrExtFeatureCoS);

            //// TypeCosDoor

            //modelBuilder.Entity<transaction>()
            //   .HasOne(t => t.TypeCoSDoor)
            //   .WithOne(t => t.transaction);

            //modelBuilder.Entity<HIDAeroService.Entity.TypeCoSDoor>()
            //    .HasMany(x => x.transaction_flag)
            //    .WithOne(x => x.TypeCoSDoor);

            //// TypeMPG

            //modelBuilder.Entity<transaction>()
            //   .HasOne(t => t.TypeMPG)
            //   .WithOne(t => t.transaction);

            //modelBuilder.Entity<HIDAeroService.Entity.TypeMPG>()
            //    .HasMany(x => x.transaction_flag)
            //    .WithOne(x => x.TypeMPG);

            //// TypeArea

            //modelBuilder.Entity<transaction>()
            //   .HasOne(t => t.TypeArea)
            //   .WithOne(t => t.transaction);

            //modelBuilder.Entity<HIDAeroService.Entity.TypeArea>()
            //    .HasMany(x => x.transaction_flag)
            //    .WithOne(x => x.TypeArea);


            #endregion

            #region Access Area

            modelBuilder.Entity<Area>()
                .HasMany(a => a.door_in)
                .WithOne(d => d.area_in)
                .HasForeignKey(f => f.antipassback_in)
                .HasPrincipalKey(p => p.component_id);

            modelBuilder.Entity<Area>()
                .HasMany(a => a.door_out)
                .WithOne(d => d.area_out)
                .HasForeignKey(f => f.antipassback_out)
                .HasPrincipalKey(p => p.component_id);

            modelBuilder.Entity<Area>()
                .HasData(
                    new Area { id=1,component_id=-1,multi_occ=0,access_control=0,occ_control=0,occ_set=0,occ_max=0,occ_up=0,occ_down=0,area_flag=0,uuid=SeedDefaults.SystemGuid,location_id=1,is_active=true,created_date=SeedDefaults.SystemDate,updated_date=SeedDefaults.SystemDate,name="Any Area", }
                );

            modelBuilder.Entity<AccessAreaCommand>()
                .HasData(
                    new AccessAreaCommand { id=1,value=1,name="Disable Area",description= "Disable Area" },
                    new AccessAreaCommand { id = 2, value = 2, name = "Enable area", description = "Enable area" },
                    new AccessAreaCommand { id = 3, value = 3, name = "Set current occupancy to occ_set value", description = "Set current occupancy to occ_set value" },
                    new AccessAreaCommand { id = 4, value = 5, name = "Clear occupancy counts of the “standard” and “special” users", description = "Clear occupancy counts of the “standard” and “special” users" },
                    new AccessAreaCommand { id = 5, value = 6, name = "Disable multi-occupancy rules", description = "Disable multi-occupancy rules" },
                    new AccessAreaCommand { id = 6, value = 7, name = "Enable standard multi-occupancy processing", description = "Enable standard multi-occupancy processing" }
                );

            modelBuilder.Entity<AreaAccessControl>()
                .HasData(
                    new AreaAccessControl { id=1,name= "NOP",value=0,description="No Operation" },
                    new AreaAccessControl { id=2,name= "Disable area",value=1,description="No One Can Access" },
                    new AreaAccessControl { id=3,name="Enable area",value=2,description="Enable Area"}
                );

            modelBuilder.Entity<OccupancyControl>()
                .HasData(
                    new OccupancyControl {id=1,name= "Do not change current occupancy count",value=0,description= "Do not change current occupancy count" },
                    new OccupancyControl { id=2,name= "Change current occupancy to occ_set",value=1,description= "Change current occupancy to occ_set" }
                );

            modelBuilder.Entity<AreaFlag>()
                .HasData(
                    new AreaFlag { id=1,name="Interlock",value=0x01,description= "Area can have open thresholds to only one other area" },
                    new AreaFlag { id=2,name="AirLock One door Only",value=0x02,description= "Just (O)ne (D)oor (O)nly is allowed to be open into this area (AREA_F_AIRLOCK must also be set)" }
                );

            modelBuilder.Entity<MultiOccupancy>()
                .HasData(
                    new MultiOccupancy { id = 1, name = "Two or more not required in area", value = 0, description = "Two or more not required in area" },
                    new MultiOccupancy { id = 2, name = "Two or more required", value = 1, description = "Two or more required" }
                    );

            #endregion

            #region Feature

            modelBuilder.Entity<Feature>()
                .HasMany(s => s.sub_feature)
                .WithOne(s => s.feature)
                .HasForeignKey(k => k.feature_id)
                .HasPrincipalKey(l => l.component_id);


            modelBuilder.Entity<Feature>()
                .HasData(
                    new Feature { id = 1, component_id = 1, name = "Dashboard", path = "/" },
                    new Feature { id = 2, component_id = 2, name = "transaction", path = "/event" },
                    new Feature { id = 3, component_id = 3, name = "location", path = "/location" },
                    new Feature { id = 4, component_id = 4, name = "Alerts", path = "/alert" },
                    new Feature { id = 5, component_id = 5, name = "operator" },
                    new Feature { id = 6, component_id = 6, name = "Devices" },
                    new Feature { id = 7, component_id = 7, name = "door", path = "/door" },
                    new Feature { id = 8, component_id = 8, name = "Card Holder", path = "/cardholder" },
                    new Feature { id = 9, component_id = 9, name = "Access Level", path = "/level" },
                    new Feature { id = 10, component_id = 10, name = "Access Area", path = "/area" },
                    new Feature { id = 11, component_id = 11, name = "time" },
                    new Feature { id = 12, component_id = 12, name = "trigger & procedure" },
                    new Feature { id = 13, component_id = 13, name = "Reports" },
                    new Feature { id = 14, component_id = 14, name = "Settings", path = "/setting" },
                    new Feature { id = 15, component_id = 15, name = "Maps", path = "/map" },
                    new Feature { id=16,component_id=16,name="ControlPoint",path="/control"},
                    new Feature { id=17,component_id=17,name="MonitorPoint",path="/monitor"},
                    new Feature { id=18,component_id=18,name="monitor_group",path="/monitorgroup"}

                );

            modelBuilder.Entity<SubFeature>()
                .HasData(
                new SubFeature { id = 1, component_id = 1, name = "operator", path = "/operator", feature_id = 5 },
                new SubFeature { id = 2, component_id = 2, name = "role", path = "/role", feature_id = 5 },
                new SubFeature { id = 3, component_id = 3, name = "hardware", path = "/hardware", feature_id = 6 },
                new SubFeature { id = 4, component_id = 4, name = "modules", path = "/modules", feature_id = 6 },
                new SubFeature { id = 5, component_id = 5, name = "Timezone", path = "/timezone", feature_id = 11 },
                new SubFeature { id = 6, component_id = 6, name = "holiday", path = "/holiday", feature_id = 11 },
                new SubFeature { id = 7, component_id = 7, name = "interval", path = "/interval", feature_id = 11 },
                new SubFeature { id = 8, component_id = 8, name = "trigger", path = "/trigger", feature_id = 12 },
                new SubFeature { id = 9, component_id = 9, name = "procedure", path = "/action", feature_id = 12 },
                new SubFeature { id = 10, component_id = 10, name = "transaction", path = "/transaction", feature_id = 13 },
                new SubFeature { id = 11, component_id = 11, name = "Audit Trail", path = "/audit", feature_id = 13 }


                );

            #endregion

            #region FeatureRole 

            modelBuilder.Entity<FeatureRole>()
                .HasData(
                    new FeatureRole { feature_id = 1, role_id = 1, is_allow = true, is_create = true, is_modify = true, is_delete = true, is_action = true },
                    new FeatureRole { feature_id = 2, role_id = 1, is_allow = true, is_create = true, is_modify = true, is_delete = true, is_action = true },
                    new FeatureRole { feature_id = 3, role_id = 1, is_allow = true, is_create = true, is_modify = true, is_delete = true, is_action = true },
                    new FeatureRole { feature_id = 4, role_id = 1, is_allow = true, is_create = true, is_modify = true, is_delete = true, is_action = true },
                    new FeatureRole { feature_id = 5, role_id = 1, is_allow = true, is_create = true, is_modify = true, is_delete = true, is_action = true },
                    new FeatureRole { feature_id = 6, role_id = 1, is_allow = true, is_create = true, is_modify = true, is_delete = true, is_action = true },
                    new FeatureRole { feature_id = 7, role_id = 1, is_allow = true, is_create = true, is_modify = true, is_delete = true, is_action = true },
                    new FeatureRole { feature_id = 8, role_id = 1, is_allow = true, is_create = true, is_modify = true, is_delete = true, is_action = true },
                    new FeatureRole { feature_id = 9, role_id = 1, is_allow = true, is_create = true, is_modify = true, is_delete = true, is_action = true },
                    new FeatureRole { feature_id = 10, role_id = 1, is_allow = true, is_create = true, is_modify = true, is_delete = true, is_action = true },
                    new FeatureRole { feature_id = 11, role_id = 1, is_allow = true, is_create = true, is_modify = true, is_delete = true, is_action = true },
                    new FeatureRole { feature_id = 12, role_id = 1, is_allow = true, is_create = true, is_modify = true, is_delete = true, is_action = true },
                    new FeatureRole { feature_id = 13, role_id = 1, is_allow = true, is_create = true, is_modify = true, is_delete = true, is_action = true },
                    new FeatureRole { feature_id = 14, role_id = 1, is_allow = true, is_create = true, is_modify = true, is_delete = true, is_action = true },
                    new FeatureRole { feature_id = 15, role_id = 1, is_allow = true, is_create = true, is_modify = true, is_delete = true, is_action = true },
                    new FeatureRole { feature_id = 16, role_id = 1, is_allow = true, is_create = true, is_modify = true, is_delete = true, is_action = true },
                    new FeatureRole { feature_id = 17, role_id = 1, is_allow = true, is_create = true, is_modify = true, is_delete = true, is_action = true },
                    new FeatureRole { feature_id = 18, role_id = 1, is_allow = true, is_create = true, is_modify = true, is_delete = true, is_action = true }
                );

            #endregion

            #region Role

            modelBuilder.Entity<Role>()
                .HasData(
                    new Role { id=1,component_id=1,name="Administrator",updated_date=SeedDefaults.SystemDate,created_date=SeedDefaults.SystemDate}
                );

            #endregion

            #region Operator

            //modelBuilder.Entity<role>()
            //    .HasMany(o => o.operator)
            //    .WithOne(r => r.role)
            //    .HasForeignKey(o => o.role_id)
            //    .HasPrincipalKey(r => r.component_id)
            //    .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PasswordRule>()
                .HasMany(x => x.weaks)
                .WithOne(x => x.password_rule)
                .HasForeignKey(x => x.password_rule_id)
                .HasPrincipalKey(x => x.id)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OperatorLocation>()
                .HasKey(x => new { x.location_id, x.operator_id });

            modelBuilder.Entity<OperatorLocation>()
                .HasOne(x => x.location)
                .WithMany(x => x.operator_locations)
                .HasForeignKey(x => x.location_id)
                .HasPrincipalKey(x => x.component_id)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OperatorLocation>()
                .HasOne(x => x.@operator)
                .WithMany(x => x.operator_locations)
                .HasForeignKey(x => x.operator_id)
                .HasPrincipalKey(x => x.component_id)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Operator>()
                .HasOne(o => o.role)
                .WithMany(r => r.operators)
                .HasForeignKey(o => o.role_id)
                .HasPrincipalKey(r => r.component_id)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<FeatureRole>()
                .HasKey(e => new { e.role_id, e.feature_id });

            modelBuilder.Entity<FeatureRole>()
                .HasOne(e => e.role)
                .WithMany(e => e.feature_roles)
                .HasForeignKey(e => e.role_id)
                .HasPrincipalKey(e => e.component_id);

            modelBuilder.Entity<FeatureRole>()
                .HasOne(e => e.feature)
                .WithMany(e => e.feature_role)
                .HasForeignKey(e => e.feature_id)
                .HasPrincipalKey(e => e.component_id);

            modelBuilder.Entity<PasswordRule>()
                .HasData(
                new PasswordRule { id=1,len=4,is_digit=false,is_lower=false,is_symbol=false,is_upper=false }
                );

            modelBuilder.Entity<WeakPassword>()
                .HasData(
                new WeakPassword { id = 1,pattern="P@ssw0rd",password_rule_id=1},
                new WeakPassword { id=2,pattern="password",password_rule_id=1},
                new WeakPassword { id=3,pattern="admin",password_rule_id=1},
                new WeakPassword { id=4,pattern="123456",password_rule_id=1}
                );

            modelBuilder.Entity<Operator>()
                .HasData(
                    new Operator { id = 1,component_id=1,user_id="Administrator",user_name = "admin", password = "2439iBIqejYGcodz6j0vGvyeI25eOrjMX3QtIhgVyo0M4YYmWbS+NmGwo0LLByUY", email = "support@honorsupplying.com", title = "Mr.", first_name = "Administrator", middle_name = "", last_name = "", phone = "", image_path = "", role_id = 1,uuid=SeedDefaults.SystemGuid,created_date=SeedDefaults.SystemDate,updated_date=SeedDefaults.SystemDate,is_active=true }
                );

            modelBuilder.Entity<OperatorLocation>()
            .HasData(
                new OperatorLocation { id = 1, location_id = 1, operator_id = 1 }
            );

            #endregion

            #region Procedure

            modelBuilder.Entity<Procedure>()
                .HasMany(x => x.actions)
                .WithOne(x => x.procedure)
                .HasForeignKey(x => x.procedure_id)
                .HasPrincipalKey(x => x.component_id);

            modelBuilder.Entity<ActionType>()
                .HasData(
                    //new action_type { id = 1, name = "Delete all actions", value = 0, description = "Deletes all configured actions" },

                    new ActionType { id = 2, name = "Monitor Point Mask", value = 1, description = "command 306: Monitor Point Mask" },
                    new ActionType { id = 3, name = "Control Point command", value = 2, description = "command 307: Control Point command" },
                    new ActionType { id = 4, name = "door mode", value = 3, description = "command 308: ACR mode" },
                    //new action_type { id = 5, name = "Forced Open Mask", value = 4, description = "command 309: Forced Open Mask" },
                    //new action_type { id = 6, name = "Held Open Mask", value = 5, description = "command 310: Held Open Mask Control" },
                    new ActionType { id = 7, name = "Momentary Unlock", value = 6, description = "command 311: Momentary Unlock" },
                    //new action_type { id = 8, name = "procedure Control command", value = 7, description = "command 312: procedure Control command" },
                    //new action_type { id = 9, name = "trigger Variable Control", value = 8, description = "command 313: trigger Variable Control command" },
                    new ActionType { id = 10, name = "time Zone Control", value = 9, description = "command 314: time Zone Control" },
                    //new action_type { id = 11, name = "Reader LED mode Control", value = 10, description = "command 315: Reader LED mode Control" },

                    //new action_type { id = 12, name = "Anti-passback Control", value = 11, description = "command 3319: Anti-passback Control (free pass only)" },

                    new ActionType { id = 13, name = "Monitor Point Group Arm/Disarm", value = 14, description = "command 321: Monitor Point Group Arm/Disarm" },

                    //new action_type { id = 14, name = "Set action hardware_type by Mask Count", value = 15, description = "Set action_type prefix based on mask_count" },
                    //new action_type { id = 15, name = "Set action hardware_type by Active Points", value = 16, description = "Set action_type prefix based on active points" },

                    //new action_type { id = 16, name = "Modify Access Area Configuration", value = 17, description = "command 322: Modify Access Area Configuration" },

                    //new action_type { id = 17, name = "Abort Wait For door Open", value = 18, description = "Abort pending access request (turnstile mode)" },

                    //new action_type { id = 18, name = "Temporary Reader LED Control", value = 19, description = "command 325: Temporary Reader LED Control" },
                    //new action_type { id = 19, name = "LCD Text Output", value = 20, description = "command 326: Text Output to LCD Terminal" },

                    new ActionType { id = 20, name = "Temporary door mode", value = 24, description = "command 334: Temporary ACR mode" },
                    new ActionType { id = 21, name = "Host Simulated Card Read", value = 25, description = "command 331: Host Simulated Card Read" },
                    //new action_type { id = 22, name = "Set Cardholder Use Limit", value = 26, description = "command 3323: Set Cardholder Use Limit (all only)" },
                    //new action_type { id = 23, name = "Set Operating mode", value = 27, description = "command 335: Set Operating mode" },
                    //new action_type { id = 24, name = "Host Simulated Key Press", value = 28, description = "command 339: Host Simulated Key Press" },

                    //new action_type { id = 25, name = "Filter trigger transaction", value = 29, description = "Filter transaction in calling trigger" },
                    //new action_type { id = 26, name = "trigger Activation Summary", value = 30, description = "command 1820: trigger Activation Summary" },

                    //new action_type { id = 27, name = "Delay (0.1 Second)", value = 126, description = "Delay in 0.1 second increments" },
                    new ActionType { id = 28, name = "Delay (Seconds)", value = 127, description = "Delay in seconds" }
                );


            #endregion

            #region Trigger

            modelBuilder.Entity<Trigger>()
                .HasOne(x => x.procedure)
                .WithOne(x => x.trigger)
                .HasForeignKey<Trigger>(x => x.procedure_id)
                .HasPrincipalKey<Procedure>(x => x.component_id);

            modelBuilder.Entity<Trigger>()
                .HasMany(x => x.code_map)
                .WithOne(x => x.trigger)
                .HasForeignKey(x => x.value)
                .HasPrincipalKey(x => x.component_id)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TriggerCommand>()
                .HasData(
                    new TriggerCommand { id=1,name= "Abort a delayed procedure", description= "Abort a delayed procedure", value=1},
                    new TriggerCommand { id = 2, name = "Execute actions with prefix 0", description = "Execute actions with prefix 0", value = 2 },
                    new TriggerCommand { id = 3, name = "Resume a delayed procedure and execute actions with prefix 0", description = "Resume a delayed procedure and execute actions with prefix 0", value = 3 },
                    new TriggerCommand { id = 4, name = "Execute actions with prefix 256", description = "Execute actions with prefix 256", value = 4 },
                    new TriggerCommand { id = 5, name = "Execute actions with prefix 512", description = "Execute actions with prefix 512", value = 5 },
                    new TriggerCommand { id = 6, name = "Execute actions with prefix 1024", description = "Execute actions with prefix 1024", value = 6 },
                    new TriggerCommand { id = 7, name = "Resume a delayed procedure and execute actions with prefix 256", description = "Resume a delayed procedure and execute actions with prefix 256", value = 7 },
                    new TriggerCommand { id = 8, name = "Resume a delayed procedure and execute actions with prefix 512", description = "Resume a delayed procedure and execute actions with prefix 512", value = 8 },
                    new TriggerCommand { id = 9, name = "Resume a delayed procedure and execute actions with prefix 1024", description = "Resume a delayed procedure and execute actions with prefix 1024", value = 9 }
                );

            #endregion

            #region File Type

            modelBuilder.Entity<FileType>()
               .HasData(
                   new FileType
                   {
                       id = 1,
                       value = 0,
                       name = "Host Comm certificate file. The file should be in the same format currently used by the default certificate (PEM)."
                   },
                   new FileType
                   {
                       id = 2,
                       value = 1,
                       name = "User defined file. This file can contain any type of data, up to one block in size. This file can have a name on the SCP up to 259 bytes."
                   },
                   new FileType
                   {
                       id = 3,
                       value = 2,
                       name = "License file. This file will be generated by HID and needed on only those products that require a license."
                   },
                   new FileType
                   {
                       id = 4,
                       value = 3,
                       name = "Peer certificate"
                   },
                   new FileType
                   {
                       id = 5,
                       value = 4,
                       name = "OSDP file transfer files"
                   },
                   new FileType
                   {
                       id = 6,
                       value = 7,
                       name = "Linq certificate"
                   },
                   new FileType
                   {
                       id = 7,
                       value = 8,
                       name = "Over-Watch certificate"
                   },
                   new FileType
                   {
                       id = 8,
                       value = 9,
                       name = "Web server certificate"
                   },
                   new FileType
                   {
                       id = 9,
                       value = 10,
                       name = "HID Origo™ certificate"
                   },
                   new FileType
                   {
                       id = 10,
                       value = 11,
                       name = "Aperio certificate"
                   },
                   new FileType
                   {
                       id = 11,
                       value = 12,
                       name = "Host translator service for OEM cloud certificate"
                   },
                   new FileType
                   {
                       id = 12,
                       value = 13,
                       name = "Driver trust store"
                   },
                   new FileType
                   {
                       id = 13,
                       value = 16,
                       name = "802.1x TLS authentication"
                   },
                   new FileType
                   {
                       id = 14,
                       value = 18,
                       name = "HTS OEM cloud authentication"
                   }
               );


            #endregion

            #region Component

            modelBuilder.Entity<HardwareComponent>().HasData(
                new HardwareComponent { id = 1, model_no = 196, name = "HID Aero X1100", n_input = 7, n_output = 4, n_reader = 4 },
                new HardwareComponent { id = 2, model_no = 193, name = "HID Aero X100", n_input = 7, n_output = 4, n_reader = 4 },
                new HardwareComponent { id = 3, model_no = 194, name = "HID Aero X200", n_input = 19, n_output = 2, n_reader = 0 },
                new HardwareComponent { id = 4, model_no = 195, name = "HID Aero X300", n_input = 5, n_output = 12, n_reader = 0 },
                new HardwareComponent { id = 5, model_no = 190, name = "VertX V100", n_input = 7, n_output = 4, n_reader = 2 },
                new HardwareComponent { id = 6, model_no = 191, name = "VertX V200", n_input = 19, n_output = 2, n_reader = 0 },
                new HardwareComponent { id = 7, model_no = 192, name = "VertX V300", n_input = 5, n_output = 12, n_reader = 0 }
             );

            #endregion
         



            /*
             *
            * Below should be inside License feature
            * */
            modelBuilder.Entity<SystemConfiguration>().HasData(
                new SystemConfiguration { id = 1, n_ports = 1, n_scp = 100, n_channel_id = 1, c_type = 7, c_port = 3333 }
                );

            modelBuilder.Entity<SystemSetting>().HasData(
                new SystemSetting { id = 1, m_msp1_port = 3, n_transaction = 60000, n_sio = 33, n_mp = 615, n_cp = 388, n_acr = 64, n_alvl = 32000, n_trgr = 1024, n_proc = 1024, gmt_offset = -25200, n_tz = 255, n_hol = 255, n_mpg = 128, n_card = 200,n_area= 127}
                );

            



        }




    }
}