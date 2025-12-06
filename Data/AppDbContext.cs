using HID.Aero.ScpdNet.Wrapper;
using HIDAeroService.AeroLibrary;
using HIDAeroService.Controllers.V1;
using HIDAeroService.Entity;
using HIDAeroService.Entity.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Xml.Linq;

namespace HIDAeroService.Data
{

    public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {

        // New 
        public DbSet<Hardware> Hardwares { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<Sensor> Sensors { get; set; }
        public DbSet<RequestExit> RequestExits { get; set; }
        public DbSet<MonitorPoint> MonitorPoints { get; set; }
        public DbSet<ControlPoint> ControlPoints { get; set; }
        public DbSet<Strike> Strikes { get; set; }
        public DbSet<Reader> Readers { get; set; }
        public DbSet<SystemSetting> SystemSettings { get; set; }
        public DbSet<Component> Components { get; set; }
        public DbSet<CardFormat> CardFormats { get; set; }
        public DbSet<Entity.TimeZone> TimeZones { get; set; }
        public DbSet<AccessLevel> AccessLevels { get; set; }
        public DbSet<Holiday> Holidays { get; set; }
        public DbSet<Door> Doors { get; set; }
        public DbSet<DoorMode> DoorModes { get; set; }
        public DbSet<OutputMode> OutputModes { get; set; }
        public DbSet<StrikeMode> StrikeModes { get; set; }
        public DbSet<Interval> Intervals { get; set; }
        public DbSet<OutputOfflineMode> OutputOfflineModes { get; set; }
        public DbSet<RelayMode> RelayModes { get; set; }
        public DbSet<TimeZoneMode> TimeZoneModes { get; set; }
        public DbSet<SystemConfiguration> SystemConfigurations { get; set; }
        public DbSet<InputMode> InputModes { get; set; }
        public DbSet<ReaderConfigurationMode> ReaderConfigurationModes { get; set; }
        public DbSet<AntipassbackMode> AntipassbackModes { get; set; }
        public DbSet<AccessLevelDoorTimeZone> AccessLevelDoorTimeZones { get; set; }
        public DbSet<CardHolder> CardHolders { get; set; }
        public DbSet<Credential> Credentials { get; set; }
        public DbSet<AccessArea> AccessAreas { get; set; }
        public DbSet<TimeZoneInterval> TimeZoneIntervals { get; set; }
        public DbSet<ReaderOutConfiguration> ReaderOutConfigurations { get; set; }
        public DbSet<MonitorPointMode> MonitorPointModes { get; set; }
        public DbSet<OsdpBaudrate> OsdpBaudrates { get; set; }
        public DbSet<OsdpAddress> OsdpAddresses { get; set; }
        public DbSet<Operator> Operators { get; set; }
        public DbSet<DoorSpareFlagOption> DoorSpareFlagOption { get; set; }
        public DbSet<DoorAccessControlFlag> DoorAccessControlFlags { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<TransactionSource> TransactionSources { get; set; }
        public DbSet<TransactionType> TransactionTypes { get; set; }
        public DbSet<TransactionCode> TransactionCodes { get; set; }
        public DbSet<CredentialFlagOption> CredentialFlagOptions { get; set; }
        public DbSet<AccessAreaCommandOption> AccessAreaCommandOptions { get; set; }
        public DbSet<AccessAreaAccessControlOption> AccessAreaAccessControlOptions { get; set; }
        public DbSet<OccupancyControlOption> OccupancyControlOptions { get; set; }
        public DbSet<AreaFlagOption> AreaFlagOptions { get; set; }
        public DbSet<MultiOccupancyOption> MultiOccupancyOptions { get; set; }
        public DbSet<RefreshTokenAudit> RefreshTokenAudits { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<SubFeature> SubFeatures { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<FeatureRole> FeatureRoles { get; set; }

        public DbSet<Transaction> Transactions { get; set; }

        public DbSet<FileType> FileTypes { get; set; }
        public DbSet<MonitorGroup> MonitorGroups { get; set; }
        public DbSet<MonitorGroupList> MonitorGroupLists { get; set; }

        public DbSet<MonitorGroupType> MonitorGroupTypes { get; set; }
        public DbSet<MonitorGroupCommand> MonitorGroupCommands { get; set; }
        public DbSet<Procedure> Procedures { get; set; }
        public DbSet<Entity.Action> Actions { get; set; }
        public DbSet<Trigger> Triggers { get; set; }


        // New
        //public DbSet<TransactionFlag> TransactionFlagDetails { get; set; }
        //public DbSet<TypeSys> TypeSys { get; set; }
        //public DbSet<TypeWebActivity> TypeWebActivities { get; set; }
        //public DbSet<TypeFileDownloadStatus> TypeFileDownloadStatuses { get; set; }
        //public DbSet<TypeCos> TypeCoses { get; set; }
        //public DbSet<TypeSioDiag> TypeSioDiags { get; set; }
        //public DbSet<TypeSioComm> TypeSioComms { get; set; }
        //public DbSet<TypeCardBin> TypeCardBins { get; set; }
        //public DbSet<TypeCardBcd> TypeCardBcds { get; set; }
        //public DbSet<TypeCardFull> TypeCardFulls { get; set; }
        //public DbSet<TypeCardID> TypeCardIDs { get; set; }
        //public DbSet<TypeREX> TypeREXes { get; set; }
        //public DbSet<TypeUserCmnd> TypeUserCmnds { get; set; }
        //public DbSet<TypeAcr> TypeAcrs { get; set; }
        //public DbSet<TypeUseLimit> TypeUseLimits { get; set; }
        //public DbSet<TypeCosElevator> TypeCosElevator { get; set; }
        //public DbSet<TypeCosElevatorAccess> TypeCosElevatorAccesses { get; set; }
        //public DbSet<TypeAcrExtFeatureStls> TypeAcrExtFeatureStls { get; set; }
        //public DbSet<TypeAcrExtFeatureCoS> TypeAcrExtFeatureCoSes { get; set; }
        //public DbSet<TypeMPG> TypeMPGs { get; set; }
        //public DbSet<TypeArea> TypeAreas { get; set; }

        // Old
        public DbSet<CommandStatus> ArCommandStatuses { get; set; }

        public DbSet<AeroStructureStatus> AeroStructureStatuses { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // put this inside OnModelCreating(ModelBuilder modelBuilder)
            var datetimeInterface = typeof(IDatetime);

            foreach (var et in modelBuilder.Model.GetEntityTypes()
                         .Where(t => t.ClrType != null && datetimeInterface.IsAssignableFrom(t.ClrType)))
            {
                // get the builder for the concrete CLR type (e.g. Location, ArEvent, ...)
                var builder = modelBuilder.Entity(et.ClrType);

                // configure CreatedDate
                builder.Property<DateTime>(nameof(IDatetime.CreatedDate))
                    .HasColumnType("timestamp without time zone")
                    .HasDefaultValueSql("now()")
                    .ValueGeneratedOnAdd();

                // configure UpdatedDate
                builder.Property<DateTime>(nameof(IDatetime.UpdatedDate))
                    .HasColumnType("timestamp without time zone")
                    .HasDefaultValueSql("now()")
                    // ValueGeneratedOnAddOrUpdate can be used, but in many DBs you'll still want to set UpdatedDate in SaveChanges
                    .ValueGeneratedOnAddOrUpdate();
            }

            //modelBuilder.Entity<IDatetime>()
            //    .Property(nameof(IDatetime.CreatedDate))
            //    .HasColumnType("timestamp without time zone")
            //    .HasDefaultValueSql("now()")
            //    .ValueGeneratedOnAdd();

            //modelBuilder.Entity<IDatetime>()
            //    .Property(nameof(IDatetime.UpdatedDate))
            //    .HasColumnType("timestamp without time zone")
            //    .HasDefaultValueSql("now()")
            //    .ValueGeneratedOnAddOrUpdate();

            modelBuilder.Entity<Hardware>().Property(nameof(Hardware.LastSync)).HasColumnType("timestamp without time zone").HasDefaultValueSql("now()")
                    .ValueGeneratedOnAdd(); 



            #region Hardware 

            modelBuilder.Entity<Hardware>()
                .HasMany(p => p.Module)
                .WithOne(p => p.Hardware)
                .HasForeignKey(p => p.MacAddress)
                .HasPrincipalKey(p => p.MacAddress)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<HardwareCredential>()
                .HasKey(p => new { p.MacAddress, p.HardwareCredentialId });

            modelBuilder.Entity<HardwareCredential>()
                .HasOne(e => e.Hardware)
                .WithMany(e => e.HardwareCredentials)
                .HasForeignKey(e => e.HardwareCredentialId)
                .HasPrincipalKey(e => e.ComponentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<HardwareAccessLevel>()
                .HasOne(e => e.Hardware)
                .WithMany(e => e.HardwareAccessLevels)
                .HasForeignKey(e => e.MacAddress)
                .HasPrincipalKey(e => e.MacAddress)
                .OnDelete(DeleteBehavior.Cascade);

            #endregion

            #region Monitor Point

            modelBuilder.Entity<MonitorPointMode>().HasData(
                new MonitorPointMode { Id = 1, Name = "Normal mode (no exit or entry delay)", Value = 0, Description = "" },
                new MonitorPointMode { Id = 2, Name = "Non-latching mode", Value = 1, Description = "" },
                new MonitorPointMode { Id = 3, Name = "Latching mode", Value = 2, Description = "" }
            );

            modelBuilder.Entity<InputMode>().HasData(
                new InputMode { Id = 1, Name = "Normally closed", Value = 0, Description = "Normally closed, no End-Of-Line (EOL)" },
                new InputMode { Id = 2, Name = "Normally open", Value = 1, Description = "Normally open, no EOL" },
                new InputMode { Id = 3, Name = "Standard EOL 1", Value = 2, Description = "Standard (ROM’ed) EOL: 1 kΩ normal, 2 kΩ active" },
                new InputMode { Id = 4, Name = "Standard EOL 2", Value = 3, Description = "Standard (ROM’ed) EOL: 2 kΩ normal, 1 kΩ active" }
                );

            #endregion

            #region Monitor Group

            modelBuilder.Entity<MonitorGroup>()
                .HasMany(m => m.nMpList)
                .WithOne(l => l.MonitorGroup)
                .HasForeignKey(l => l.MonitorGroupId)
                .HasPrincipalKey(x => x.ComponentId);

            modelBuilder.Entity<MonitorGroupType>()
                .HasData(
                    new MonitorGroupType { Id=1,Name="Monitor Point",Value=1,Description="" },
                    new MonitorGroupType { Id=2,Name="Forced Open",Value=2,Description=""},
                    new MonitorGroupType { Id=3,Name="Held Open",Value=3,Description=""}
                );

            modelBuilder.Entity<MonitorGroupCommand>()
                .HasData(
                    new MonitorGroupCommand { Id=1,Name="Access",Value=1,Description= "If the mask count is zero, mask all monitor points and increment the mask count by one" },
                    new MonitorGroupCommand { Id=2,Name="Override",Value=2,Description= "Set mask count to arg1. If arg1 is zero, then all points get unmasked. If arg1 is not zero, then all points get masked." },
                    new MonitorGroupCommand { Id=3,Name="Force Arm",Value=3,Description= "Force Arm: If the mask count > 1 then decrement the mask count by 1. Otherwise, if the mask count is equal to 1, unmask all non-active monitor points and set the mask count to zero." },
                    new MonitorGroupCommand { Id=4,Name="Arm",Value=4,Description= "If the mask count > 1 then decrement the mask count by one. Otherwise, if the mask count is equal to 1 and no monitor points are active, unmask all monitor points. and set the mask count to zero." },
                    new MonitorGroupCommand { Id=5,Name="Override arm",Value=5,Description= "If the mask count > 1 then decrement the mask count by one, otherwise if the mask count is 1 unmask all monitor points and set the mask count to zero" }
                );


            #endregion

            #region Control Point 

            modelBuilder.Entity<OutputMode>().HasData(
                new OutputMode { Id = 1, RelayMode = 0, OfflineMode = 0, Value = 0, Description = "Normal Mode with Offline: No change" },
                new OutputMode { Id = 2, RelayMode = 1, OfflineMode = 0, Value = 1, Description = "Inverted Mode Offline: No change" },
                new OutputMode { Id = 3, RelayMode = 0, OfflineMode = 1, Value = 16, Description = "Normal Mode Offline: Inactive" },
                 new OutputMode { Id = 4, RelayMode = 1, OfflineMode = 1, Value = 17, Description = "Inverted Mode Offline: Inactive" },
                 new OutputMode { Id = 5, RelayMode = 0, OfflineMode = 2, Value = 32, Description = "Normal Mode Offline: Active" },
                 new OutputMode { Id = 6, RelayMode = 1, OfflineMode = 2, Value = 33, Description = "Inverted Mode Offline: Active" }
                );

            modelBuilder.Entity<OutputOfflineMode>().HasData(
                new OutputOfflineMode { Id = 1, Value = 0, Name = "No Change", Description = "No Change" },
                 new OutputOfflineMode { Id = 2, Value = 1, Name = "Inactive", Description = "Relay de-energized" },
                  new OutputOfflineMode { Id = 3, Value = 2, Name = "Active", Description = "Relay energized" }
                );

            modelBuilder.Entity<RelayMode>().HasData(
                new RelayMode { Id = 1, Value = 0, Name = "Normal", Description = "Active is energized" },
                new RelayMode { Id = 2, Value = 1, Name = "Inverted", Description = "Active is de-energized" }
                );


            #endregion

            #region Module

            modelBuilder.Entity<Module>()
                .HasMany(p => p.Sensors)
                .WithOne(p => p.Module)
                .HasForeignKey(p => p.ModuleId)
                .HasPrincipalKey(p => p.ComponentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Module>()
                .HasMany(p => p.Strikes)
                .WithOne(p => p.Module)
                .HasForeignKey(p => p.ModuleId)
                .HasPrincipalKey(p => p.ComponentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Module>()
                .HasMany(p => p.Readers)
                .WithOne(p => p.Module)
                .HasForeignKey(p => p.ModuleId)
                .HasPrincipalKey(p => p.ComponentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Module>()
                .HasMany(p => p.RequestExits)
                .WithOne(p => p.Module)
                .HasForeignKey(p => p.ModuleId)
                .HasPrincipalKey (p => p.ComponentId)
                .OnDelete (DeleteBehavior.Cascade);


            modelBuilder.Entity<Module>()
                .HasMany(p => p.ControlPoints)
                .WithOne(p => p.Module)
                .HasForeignKey(p => p.ModuleId)
                .HasPrincipalKey(p => p.ComponentId)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Module>()
                .HasMany(p => p.MonitorPoints)
                .WithOne(p => p.Module)
                .HasForeignKey(p => p.ModuleId)
                .HasPrincipalKey(p => p.ComponentId)
                .OnDelete(DeleteBehavior.Cascade);

            #endregion

            #region Access Level

            modelBuilder.Entity<AccessLevelDoorTimeZone>()
                .HasKey(p => new { p.AccessLevelId,p.TimeZoneId,p.DoorId });

            modelBuilder.Entity<AccessLevelDoorTimeZone>()
                .HasOne(s => s.AccessLevel)
                .WithMany(s => s.AccessLevelDoorTimeZones)
                .HasForeignKey(p => p.AccessLevelId)
                .HasPrincipalKey(p => p.ComponentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AccessLevelDoorTimeZone>()
                .HasOne(x => x.TimeZone)
                .WithMany(x => x.AccessLevelDoorTimeZones)
                .HasForeignKey(x => x.TimeZoneId)
                .HasPrincipalKey (x => x.ComponentId)
                .OnDelete (DeleteBehavior.Cascade);

            modelBuilder.Entity<AccessLevelDoorTimeZone>()
                .HasOne(x => x.Door)
                .WithMany(x => x.AccessLevelDoorTimeZones)
                .HasForeignKey(x => x.DoorId)
                .HasPrincipalKey(x => x.ComponentId)
                .OnDelete(DeleteBehavior.Cascade);

            var NoAccess = new AccessLevel { Id = 1, Uuid = SeedDefaults.SystemGuid, Name = "No Access", ComponentId = 1,LocationId=1, IsActive = true };

            var FullAccess = new AccessLevel { Id = 2, Uuid = SeedDefaults.SystemGuid, Name = "Full Access", ComponentId = 2,LocationId=1, IsActive = true };


            modelBuilder.Entity<AccessLevel>().HasData(
               NoAccess,
               FullAccess
            );


            #endregion

            #region Time Zone

            modelBuilder.Entity<TimeZoneInterval>()
                .HasKey(e => new { e.TimeZoneId, e.IntervalId });

            modelBuilder.Entity<TimeZoneInterval>()
                .HasOne(e => e.TimeZone)
                .WithMany(s => s.TimeZoneIntervals)
                .HasForeignKey(e => e.TimeZoneId)
                .HasPrincipalKey(s => s.ComponentId);

            modelBuilder.Entity<TimeZoneInterval>()
                .HasOne(e => e.Interval)
                .WithMany(s => s.TimeZoneIntervals)
                .HasForeignKey(e => e.IntervalId)
                .HasPrincipalKey(e => e.ComponentId);



            modelBuilder.Entity<Entity.TimeZone>().HasData(
                new Entity.TimeZone { Id = 1, Uuid = SeedDefaults.SystemGuid, Name = "Always", ComponentId = 1, Mode = 1, ActiveTime = "", DeactiveTime = "", IsActive = true }
               );

            modelBuilder.Entity<TimeZoneMode>().HasData(
                new TimeZoneMode { Id = 1, Value = 0, Name = "Off", Description = "The time zone is always inactive, regardless of the time zone intervals specified or the holidays in effect." },
                new TimeZoneMode { Id = 2, Value = 1, Name = "On", Description = "The time zone is always active, regardless of the time zone intervals specified or the holidays in effect." },
                new TimeZoneMode { Id = 3, Value = 2, Name = "Scan", Description = "The Time Zone state is decided using either the Day MaskAsync or the Holiday MaskAsync. If the current day is specified as a Holiday, the state relies only on whether the Holiday MaskAsync Flag for that Holiday is set (if today is Holiday 1, and the Holiday MaskAsync sets flag H1, then the state is active. If today is Holiday 1, and the Holiday MaskAsync does not have flag H1 set, then the state is inactive). Holidays override the standard accessibility rules. If the current day is not specified as a Holiday, the Time Zone is active or inactive depending on whether the current day/time falls within the Day MaskAsync. If Day MaskAsync is M-F, 8-5, the Time Zone is active during those times, and inactive on the weekend and outside working hours." },
                new TimeZoneMode { Id = 4, Value = 3, Name = "OneTimeEvent", Description = "Scan time zone interval list and apply only if the date string in expTest matches the current date" },
                new TimeZoneMode { Id = 5, Value = 4, Name = "Scan, Always Honor Day of Week", Description = "This mode is similar to mode Scan Mode, but instead of only checking the Holiday MaskAsync if it is a Holiday, and only checking the Day MaskAsync if not, this mode checks both. If it is not a Holiday, this mode functions normally, only checking the Day MaskAsync. If it is a Holiday, this mode performs a logical OR on the Holiday and Day Masks. If either or both are active, the Time Zone is active, otherwise if neither is active, the Time Zone is inactive." },
                new TimeZoneMode { Id = 6, Value = 5, Name = "Scan, Always Holiday and Day of Week", Description = "This mode is similar to mode \"Scan, Always Honor Day of Week\", but it performs a logical AND instead of a logical OR. If it is not a Holiday, this mode functions normally, only checking the Day MaskAsync. If it is a Holiday, this mode is only active if BOTH the Day MaskAsync and Holiday MaskAsync are active. If either one is inactive, the entire Time Zone is inactive." }
             );

            #endregion

            #region Interval

            modelBuilder.Entity<Interval>()
                .HasIndex(i => i.ComponentId)
                .IsUnique();

            modelBuilder.Entity<DaysInWeek>()
                .HasIndex(d => d.ComponentId)
                .IsUnique();

            modelBuilder.Entity<Interval>().HasOne(u => u.Days).WithOne(u => u.Interval).HasPrincipalKey<Interval>(p => p.ComponentId).HasForeignKey<DaysInWeek>(p => p.ComponentId);

            #endregion

            #region Door

            modelBuilder.Entity<Door>()
                .HasMany(p => p.Readers)
                .WithOne(p => p.Door)
                .HasForeignKey(p => p.ComponentId)
                .HasPrincipalKey(p => p.ComponentId)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Door>()
                .HasOne(p => p.Sensor)
                .WithOne(p => p.SensorDoor)
                .HasForeignKey<Door>(p => p.SensorComponentId)
                .HasPrincipalKey<Sensor>(p => p.ComponentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Door>()
                .HasMany(p => p.RequestExits)
                .WithOne(p => p.Door)
                .HasForeignKey(p => p.ComponentId)
                .HasPrincipalKey(p => p.ComponentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Door>()
                .HasOne(p => p.Strk)
                .WithOne(p => p.StrkDoor)
                .HasForeignKey<Door>(p => p.StrkComponentId)
                .HasPrincipalKey<Strike>(p => p.ComponentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DoorSpareFlagOption>().HasData(
                new DoorSpareFlagOption { Id = 1,Name="No Extend",Value=0x0001,Description= "ACR_FE_NOEXTEND\t0x0001\t\r\n🔹 Purpose: Prevents the “Extended Door Held Open Timer” from being restarted when a new access is granted.\r\n🔹 Effect: If someone presents a valid credential while the door is already open, the extended hold timer won’t reset — the timer continues to count down.\r\n🔹 Use Case: High-traffic doors where you don’t want repeated badge reads to keep the door open indefinitely." },
                new DoorSpareFlagOption { Id = 2, Name="Card Before Pin" ,Value=0x0002,Description= "ACR_FE_NOPINCARD\t0x0002\t\r\n🔹 Purpose: Forces CARD-before-PIN entry sequence in “Card and PIN” mode.\r\n🔹 Effect: PIN entered before a card will be rejected.\r\n🔹 Use Case: Ensures consistent user behavior and security (e.g., requiring card tap first, then PIN entry)." },
                new DoorSpareFlagOption { Id= 3,Name="Door Force Filter",Value=0x0008,Description= "ACR_FE_DFO_FLTR\t0x0008\t\r\n🔹 Purpose: Enables Door Forced Open Filter.\r\n🔹 Effect: If the door opens within 3 seconds after it was last closed, the system will not treat it as a “Door Forced Open” alarm.\r\n🔹 Use Case: Prevents nuisance alarms caused by door bounce, air pressure, or slow latch operation." },
                new DoorSpareFlagOption { Id = 4,Name="Blocked All Request",Value=0x0010,Description= "ACR_FE_NO_ARQ\t0x0010\t\r\n🔹 Purpose: Blocks all access requests.\r\n🔹 Effect: Every access attempt is automatically reported as “Access Denied – Door Locked.”\r\n🔹 Use Case: Temporarily disables access (e.g., during lockdown, maintenance, or controlled shutdown)." },
                new DoorSpareFlagOption {  Id=5,Name="Shunt Relay",Value=0x0020,Description= "ACR_FE_SHNTRLY\t0x0020\t\r\n🔹 Purpose: Defines a Shunt Relay used for suppressing door alarms during unlock events.\r\n🔹 Effect: When the door is unlocked:\r\n • The shunt relay activates 5 ms before the strike relay.\r\n • It deactivates 1 second after the door closes or the held-open timer expires.\r\n🔹 Note: The dc_held field (door-held timer) must be > 1 for this to function.\r\n🔹 Use Case: Used when connecting to alarm panels or to bypass door contacts during unlocks." },
                new DoorSpareFlagOption { Id=6,Name="Floor Pin",Value=0x0040,Description= "ACR_FE_FLOOR_PIN\t0x0040\t\r\n🔹 Purpose: Enables Floor Selection via PIN for elevators in “Card + PIN” mode.\r\n🔹 Effect: Instead of entering a PIN code, users enter the floor number after presenting a card.\r\n🔹 Use Case: Simplifies elevator access when using a single reader for multiple floors." },
                new DoorSpareFlagOption {  Id=7,Name="Link Mode",Value=0x0080,Description= "ACR_FE_LINK_MODE\t0x0080\t\r\n🔹 Purpose: Indicates that the reader is in linking mode (pairing with another device or reader).\r\n🔹 Effect: Set when acr_mode = 29 (start linking) and cleared when:\r\n • The reader is successfully linked, or\r\n • acr_mode = 30 (abort) or timeout occurs.\r\n🔹 Use Case: Used for configuring dual-reader systems (e.g., in/out readers or linked elevator panels)." },
                new DoorSpareFlagOption {  Id=8,Name="Double Card Transaction",Value=0x0100,Description= "ACR_FE_DCARD\t0x0100\t\r\n🔹 Purpose: Enables Double Card Mode.\r\n🔹 Effect: If the same valid card is presented twice within 5 seconds, it generates a double card event.\r\n🔹 Use Case: Used for dual-authentication or special functions (e.g., manager override, arming/disarming security zones)." },
                new DoorSpareFlagOption { Id=9,Name="Allow Mode Override",Value=0x0200,Description= "ACR_FE_OVERRIDE\t0x0200\t\r\n🔹 Purpose: Indicates that the reader is operating in a Temporary ACR Mode Override.\r\n🔹 Effect: Typically means that a temporary mode (e.g., unlocked, lockdown) has been forced manually or by schedule.\r\n🔹 Use Case: Allows temporary override of normal access control logic without changing the base configuration." },
                new DoorSpareFlagOption { Id=10,Name="Allow Super Card",Value=0x0400,Description= "ACR_FE_CRD_OVR_EN\t0x0400\t\r\n🔹 Purpose: Enables Override Credentials.\r\n🔹 Effect: Specific credentials (set in FFRM_FLD_ACCESSFLGS) can unlock the door even when it’s locked or access is disabled.\r\n🔹 Use Case: For emergency or master access cards (security, admin, fire personnel)." },
                new DoorSpareFlagOption {  Id=11,Name="Disable Elevator",Value=0x0800,Description= "ACR_FE_ELV_DISABLE\t0x0800\t\r\n🔹 Purpose: Enables the ability to disable elevator floors using the offline_mode field.\r\n🔹 Effect: Only applies to Elevator TypeDesc 1 and 2 ACRs.\r\n🔹 Use Case: Temporarily disables access to certain floors when the elevator or reader is in offline or restricted mode." },
                new DoorSpareFlagOption { Id=12,Name="Alternate Reader Link",Value=0x1000,Description= "ACR_FE_LINK_MODE_ALT\t0x1000\t\r\n🔹 Purpose: Similar to ACR_FE_LINK_MODE but for Alternate Reader Linking.\r\n🔹 Effect: Set when acr_mode = 32 (start link) and cleared when:\r\n  • Link successful, or\r\n  • acr_mode = 33 (abort) or timeout reached.\r\n🔹 Use Case: Used for alternate or backup reader pairing configurations." },
                new DoorSpareFlagOption {  Id=13,Name="HOLD REX",Value=0x2000,Description= "🔹 Purpose: Extends the REX (Request-to-Exit) grant time while REX input is active.\r\n🔹 Effect: As long as the REX signal remains active (button pressed or motion detected), the door remains unlocked.\r\n🔹 Use Case: Ideal for long exit paths, large doors, or slow-moving personnel." },
                new DoorSpareFlagOption { Id=14,Name="HOST Decision",Value=0x4000,Description= "ACR_FE_HOST_BYPASS\t0x4000\t\r\n🔹 Purpose: Enables host decision bypass for online authorization.\r\n🔹 Effect: Requires ACR_F_HOST_CBG to also be enabled.\r\n 1. Controller sends credential to host for decision.\r\n 2. If host replies in time → uses host’s decision.\r\n 3. If no reply (timeout): controller checks its local database.\r\n  • If credential valid → grant.\r\n  • If not → deny.\r\n🔹 Use Case: For real-time validation in networked systems but with local fallback during communication loss.\r\n🔹 Supports: Card + PIN readers, online decision making, hybrid access control." }
                );


            modelBuilder.Entity<DoorAccessControlFlag>().HasData(
                new DoorAccessControlFlag { Id=1,Name="Decrease Counter",Value=0x0001,Description= "ACR_F_DCR\t0x0001\t\r\n🔹 Purpose: Decrements a user’s “use counter” when they successfully access.\r\n🔹 Effect: Each valid access reduces their remaining allowed uses.\r\n🔹 Use Case: Temporary or limited-access credentials (e.g., one-time use visitor cards or tickets)." },
                new DoorAccessControlFlag { Id=2,Name="Deny Zero",Value=0x0002,Description= "ACR_F_CUL\t0x0002\t\r\n🔹 Purpose: Requires that the use limit is non-zero before granting access.\r\n🔹 Effect: If the use counter reaches zero, access is denied.\r\n🔹 Use Case: Works together with ACR_F_DCR for managing credentials with limited usage." },
                new DoorAccessControlFlag { Id=3,Name="Denu Duress",Value=0x0004,Description= "ACR_F_DRSS\t0x0004\t\r\n🔹 Purpose: Deny duress access requests.\r\n🔹 Effect: Normally, a duress PIN (like a special emergency PIN) grants access but logs a “duress” event. This flag changes that behavior — access is denied instead, but still logged.\r\n🔹 Use Case: High-security environments where duress entries should never open the door (only alert security)." },
                new DoorAccessControlFlag { Id=4,Name="No Sensor Door",Value=0x0008,Description= "ACR_F_ALLUSED\t0x0008\t\r\n🔹 Purpose: Treat all access grants as “used” immediately — don’t wait for door contact feedback.\r\n🔹 Effect: When access is granted, the system immediately logs it as used, even if the door sensor doesn’t open.\r\n🔹 Use Case: For systems with no door contact sensor, or for virtual readers (logical access)." },
                new DoorAccessControlFlag { Id=5,Name="Quit Exit",Value=0x0010,Description= "ACR_F_QEXIT\t0x0010\t\r\n🔹 Purpose: “Quiet Exit” — disables strike relay activation on REX (Request to Exit).\r\n🔹 Effect: When someone exits, the strike is not pulsed — useful for magnetic locks or silent egress doors.\r\n🔹 Use Case: Hospital wards, offices, or areas where audible clicks must be minimized." },
                new DoorAccessControlFlag { Id=6,Name="Door State Filter",Value=0x0020,Description= "ACR_F_FILTER\t0x0020\t\r\n🔹 Purpose: Filter out detailed door state change transactions (like every open/close event).\r\n🔹 Effect: Reduces event log noise — only key events (grants, denies) are logged.\r\n🔹 Use Case: Typically enabled in production. Disable only if you need fine-grained door state diagnostics." },
                new DoorAccessControlFlag { Id=7,Name="Two man rules",Value=0x0040,Description= "ACR_F_2CARD\t0x0040\t\r\n🔹 Purpose: Enables two-card control — requires two different valid cards before access is granted.\r\n🔹 Effect: The system waits for a second credential (often within a timeout period).\r\n🔹 Use Case: High-security doors or vaults where two people must be present (dual authentication)." },
                new DoorAccessControlFlag { Id=8,Name="Host Decision",Value=0x0400,Description= "ACR_F_HOST_CBG\t0x0400\t\r\n🔹 Purpose: If online, check with the host server before granting access.\r\n🔹 Effect: The controller sends the access request to the host; the host can grant or deny.\r\n🔹 Use Case: Centralized decision-making — e.g., dynamic permissions, host-based rules, or temporary card revocations.\r\n🔹 Note: Often used together with ACR_FE_HOST_BYPASS in the extended flags." },
                new DoorAccessControlFlag { Id=9,Name="Offline Grant",Value=0x0800,Description= "ACR_F_HOST_SFT\t0x0800\t\r\n🔹 Purpose: Defines offline failover behavior.\r\n🔹 Effect: If the host is unreachable or times out, the controller proceeds to grant access using local data instead of denying.\r\n🔹 Use Case: Ensures continuity of access during temporary network outages.\r\n🔹 Note: Use with caution — enables access even when host verification fails." },
                new DoorAccessControlFlag { Id=10,Name="Cipher Mode",Value=0x1000,Description= "ACR_F_CIPHER\t0x1000\t\r\n🔹 Purpose: Enables Cipher Mode (numeric keypad emulates card input).\r\n🔹 Effect: Allows the user to type their card number on a keypad instead of presenting a physical card.\r\n🔹 Use Case: For environments with numeric-only access or backup credential entry.\r\n🔹 Reference: See Command 1117 (Trigger Specification) for keypad mapping." },
                new DoorAccessControlFlag { Id=11,Name="Log Early",Value=0x4000,Description= "ACR_F_LOG_EARLY\t0x4000\t\r\n🔹 Purpose: Log access transactions immediately upon grant — before door usage is confirmed.\r\n🔹 Effect: Creates an instant “Access Granted” event, then later logs “Used” or “Not Used.”\r\n🔹 Constraint: Automatically disabled if ACR_F_ALLUSED (0x0008) is set.\r\n🔹 Use Case: Real-time systems that require immediate event logging (e.g., monitoring dashboards)." },
                new DoorAccessControlFlag { Id=12,Name="Wait for Card in file",Value=0x8000, Description= "ACR_F_CNIF_WAIT\t0x8000\t\r\n🔹 Purpose: Changes “Card Not in File” behavior to show ‘Wait’ pattern instead of “Denied.”\r\n🔹 Effect: The reader shows a temporary wait indication (e.g., blinking LED) — useful when waiting for host validation.\r\n🔹 Use Case: Online readers with host delay — improves user feedback for cards that might soon be recognized after sync.\r\n🔹 Reference: See Command 122 (Reader LED/Buzzer Function Specs)." }
                
                );

            modelBuilder.Entity<ReaderConfigurationMode>().HasData(
              new ReaderConfigurationMode { Id = 1, Name = "Single Reader", Value = 0, Description = "Single reader, controlling the door" },
              new ReaderConfigurationMode { Id = 2, Name = "Paired readers, Master", Value = 1, Description = "Paired readers, Master - this reader controls the door" },
              new ReaderConfigurationMode { Id = 3, Name = "Paired readers, Slave", Value = 2, Description = "Paired readers, Slave - this reader does not control door" },
              new ReaderConfigurationMode { Id = 4, Name = "Turnstile Reader", Value = 3, Description = "Turnstile Reader. Two modes selected by: n strike_t_min != strike_t_max (original implementation - an access grant pulses the strike output for 1 second) n strike_t_min == strike_t_max (pulses the strike output after a door open/close signal for each additional access grant if several grants are waiting)" },
              new ReaderConfigurationMode { Id = 5, Name = "Elevator, no floor", Value = 4, Description = "Elevator, no floor select feedback" },
              new ReaderConfigurationMode { Id = 6, Name = "Elevator with floor", Value = 5, Description = "Elevator with floor select feedback" }
              );

            modelBuilder.Entity<StrikeMode>().HasData(
                new StrikeMode { Id = 1, Name = "Normal", Value = 0, Description = "Do not use! This would allow the strike to stay active for the entire strike time allowing the door to be opened multiple times." },
                new StrikeMode { Id = 2, Name = "Deactivate On Open", Value = 1, Description = "Deactivate strike when door opens" },
                new StrikeMode { Id = 3, Name = "Deactivate On Close", Value = 2, Description = "Deactivate strike on door close or strike_t_max expires" },
                new StrikeMode { Id = 4, Name = "Tailgate", Value = 16, Description = "Used with ACR_S_OPEN or ACR_S_CLOSE, to select tailgate mode: pulse (strk_sio:strk_number+1) relay for each user expected to enter" }
                );

            modelBuilder.Entity<DoorMode>().HasData(
                new DoorMode { Id = 1, Name = "Disable", Value = 1, Description = "Disable the ACR, no REX" },
                new DoorMode { Id = 2, Name = "Unlock", Value = 2, Description = "Unlock (unlimited access)" },
                new DoorMode { Id = 3, Name = "Locked", Value = 3, Description = "Locked (no access, REX active)" },
                new DoorMode { Id = 4, Name = "Facility code only", Value = 4, Description = "Facility code only" },
                new DoorMode { Id = 5, Name = "Card only", Value = 5, Description = "Card only" },
                new DoorMode { Id = 6, Name = "PIN only", Value = 6, Description = "PIN only" },
                new DoorMode { Id = 7, Name = "Card and PIN", Value = 7, Description = "Card and PIN required" },
                new DoorMode { Id = 8, Name = "Card or PIN", Value = 8, Description = "Card or PIN required" }
            );

            modelBuilder.Entity<AntipassbackMode>().HasData(
                new AntipassbackMode { Id = 1, Name = "None", Value = 0, Description = "Do not check or alter anti-passback location. No antipassback rules." },
                new AntipassbackMode { Id = 2, Name = "Soft Anti-passback", Value = 1, Description = "Soft anti-passback: Accept any new location, change the user’s location to current reader, and generate an antipassback violation for an invalid entry." },
                new AntipassbackMode { Id = 3, Name = "Hard Anti-passback", Value = 2, Description = "Hard anti-passback: Check user location, if a valid entry is made, change user’s location to new location. If an invalid entry is attempted, do not grant access." },
                new AntipassbackMode { Id = 4, Name = "Time-base Anti-passback - Last (Second)", Value = 3, Description = "Reader-based anti-passback using the ACR’s last valid user. Verify it’s not the same user within the time parameter specified within apb_delay." },
                new AntipassbackMode { Id = 5, Name = "Time-base Anti-passback - History (Second)", Value = 4, Description = "Reader-based anti-passback using the access history from the cardholder database: Check user’s last ACR used, checks for same reader within a specified time (apb_delay). This requires the bSupportTimeApb flag be set in Command 1105: Access Database Specification." },
                new AntipassbackMode { Id = 6, Name = "Area-base Anti-passback (Second)", Value = 5, Description = "Area based anti-passback: Check user’s current location, if it does not match the expected location then check the delay time (apb_delay). Change user’s location on entry. This requires the bSupportTimeApb flag be set in Command 1105: Access Database Specification." },
                new AntipassbackMode { Id = 7, Name = "Time-base Anti-passback - Last (Minute)", Value = 6, Description = "Same as \"Time-base Anti-passback - Last (Second)\" but the apb_delay value is treated as minutes instead of seconds." },
                new AntipassbackMode { Id = 8, Name = "Time-base Anti-passback - History (Minute)", Value = 7, Description = "Same as \"Time-base Anti-passback - History (Second)\" but the apb_delay value is treated as minutes instead of seconds." },
                new AntipassbackMode { Id = 9, Name = "Area-base Anti-passback (Minute)", Value = 8, Description = "Same as \"Area-base Anti-passback (Second)\" but the apb_delay value is treated as minutes instead of seconds." }
            );

            modelBuilder.Entity<OsdpBaudrate>().HasData(
                  new OsdpBaudrate { Id = 1, Value = 0x01, Name = "9600", Description = "" },
                  new OsdpBaudrate { Id = 2, Value = 0x02, Name = "19200", Description = "" },
                  new OsdpBaudrate { Id = 3, Value = 0x03, Name = "38400", Description = "" },
                  new OsdpBaudrate { Id = 4, Value = 0x04, Name = "115200", Description = "" },
                  new OsdpBaudrate { Id = 5, Value = 0x05, Name = "57600", Description = "" },
                  new OsdpBaudrate { Id = 6, Value = 0x06, Name = "230400", Description = "" }
              );

            modelBuilder.Entity<OsdpAddress>().HasData(
                new OsdpAddress { Id = 1, Value = 0x00, Name = "Address 0", Description = "" },
                new OsdpAddress { Id = 2, Value = 0x20, Name = "Address 1", Description = "" },
                new OsdpAddress { Id = 3, Value = 0x40, Name = "Address 2", Description = "" },
                new OsdpAddress { Id = 4, Value = 0x60, Name = "Address 3", Description = "" }
            );


            modelBuilder.Entity<ReaderOutConfiguration>().HasData(
                new ReaderOutConfiguration { Id = 1, Name = "Ignore", Value = 0, Description = "Ignore data from alternate reader" },
                new ReaderOutConfiguration { Id = 2, Name = "Normal", Value = 1, Description = "Normal Access Reader (two read heads to the same processor)" }

                );


            #endregion

            #region User & Credentials

            modelBuilder.Entity<CardHolderAccessLevel>()
                .HasKey(x => new { x.AccessLevelId, x.CardHolderId });

            modelBuilder.Entity<CardHolderAccessLevel>()
                .HasOne(e => e.CardHolder)
                .WithMany(e => e.CardHolderAccessLevels)
                .HasForeignKey(e => e.CardHolderId)
                .HasPrincipalKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CardHolderAccessLevel>()
                .HasOne(e => e.AccessLevel)
                .WithMany(e => e.CardHolderAccessLevels)
                .HasForeignKey(e => e.AccessLevelId)
                .HasPrincipalKey(e => e.ComponentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CardHolder>()
                .HasMany(e => e.Credentials)
                .WithOne(e => e.CardHolder)
                .HasForeignKey(e => e.CardHolderId)
                .HasPrincipalKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CredentialFlagOption>()
                .HasData(
                new CredentialFlagOption {Id=1,Value=0x01,Name="Active Credential Record",Description= "Active Credential Record" },
                new CredentialFlagOption { Id=2,Value=0x02,Name="Free One Antipassback",Description="Allow one free anti-passback pass"},
                new CredentialFlagOption { Id=3,Value=0x04,Name="Anti-passback exempt"},
                new CredentialFlagOption { Id=4,Value=0x08,Name="Timing for disbled (ADA)",Description= "Use timing parameters for the disabled (ADA)" },
                new CredentialFlagOption { Id=5,Value=0x10,Name="Pin Exempt",Description= "PIN Exempt for \"Card & PIN\" ACR mode" },
                new CredentialFlagOption { Id=6,Value=0x20,Name="No Change APB Location",Description= "Do not change apb_loc" },
                new CredentialFlagOption { Id=7,Value=0x40,Name="No UpdateAsync Current Use Count",Description= "Do not alter either the \"original\" or the \"current\" use count values" },
                new CredentialFlagOption { Id=8,Value=0x80,Name="No UpdateAsync Current Use Count but Change Use Limit",Description= "Do not alter the \"current\" use count but change the original use limit stored in the cardholder database" }
                );




            #endregion

            #region CardFormat

            modelBuilder.Entity<CardFormat>().HasData(
                new CardFormat
                {
                    Id=1,
                    Uuid = SeedDefaults.SystemGuid,
                    IsActive=true,
                    Name = "26 Bits (No Fac)",
                    Facility = -1,
                    Offset = 0,
                    FunctionId = 1,
                    Flags = 0,
                    Bits = 26,
                    PeLn = 13,
                    PeLoc = 0,
                    PoLn = 13,
                    PoLoc = 13,
                    FcLn = 0,
                    FcLoc = 0,
                    ChLn = 16,
                    ChLoc = 9,
                    IcLn = 0,
                    IcLoc = 0,
                    CreatedDate=SeedDefaults.SystemDate,
                    UpdatedDate=SeedDefaults.SystemDate,
                }
             );

            #endregion

            #region Location

            modelBuilder.Entity<Location>()
                .HasData(
                new Location { Id=1,ComponentId=1,LocationName="Main",Description="Main Location",CreatedDate=SeedDefaults.SystemDate,UpdatedDate=SeedDefaults.SystemDate,Uuid=SeedDefaults.SystemGuid,IsActive=true }
                );

            modelBuilder.Entity<Location>()
                .HasMany(l => l.Hardwares)
                .WithOne(h => h.Location)
                .HasForeignKey(f => f.LocationId)
                .HasPrincipalKey(p => p.ComponentId);

            modelBuilder.Entity<Location>()
                .HasMany(l => l.Modules)
                .WithOne(h => h.Location)
                .HasForeignKey(f => f.LocationId)
                .HasPrincipalKey(p => p.ComponentId);

            modelBuilder.Entity<Location>()
                .HasMany(l => l.ControlPoints)
                .WithOne(h => h.Location)
                .HasForeignKey(f => f.LocationId)
                .HasPrincipalKey(p => p.ComponentId);

            modelBuilder.Entity<Location>()
               .HasMany(l => l.MonitorPoints)
               .WithOne(h => h.Location)
               .HasForeignKey(f => f.LocationId)
               .HasPrincipalKey(p => p.ComponentId);

            modelBuilder.Entity<Location>()
               .HasMany(l => l.AccessLevels)
               .WithOne(h => h.Location)
               .HasForeignKey(f => f.LocationId)
               .HasPrincipalKey(p => p.ComponentId);

            modelBuilder.Entity<Location>()
               .HasMany(l => l.AccessAreas)
               .WithOne(h => h.Location)
               .HasForeignKey(f => f.LocationId)
               .HasPrincipalKey(p => p.ComponentId);

            modelBuilder.Entity<Location>()
               .HasMany(l => l.CardHolders)
               .WithOne(h => h.Location)
               .HasForeignKey(f => f.LocationId)
               .HasPrincipalKey(p => p.ComponentId);

            modelBuilder.Entity<Location>()
               .HasMany(l => l.Doors)
               .WithOne(h => h.Location)
               .HasForeignKey(f => f.LocationId)
               .HasPrincipalKey(p => p.ComponentId);

            modelBuilder.Entity<Location>()
               .HasMany(l => l.MonitorPointsGroup)
               .WithOne(h => h.Location)
               .HasForeignKey(f => f.LocationId)
               .HasPrincipalKey(p => p.ComponentId);

            modelBuilder.Entity<Location>()
                .HasMany(l => l.Events)
                .WithOne(c => c.Location)
                 .HasForeignKey(f => f.LocationId)
               .HasPrincipalKey(p => p.ComponentId);

            modelBuilder.Entity<Location>()
               .HasMany(l => l.AeroStructureStatuses)
               .WithOne(c => c.Location)
               .HasForeignKey(f => f.LocationId)
               .HasPrincipalKey(p => p.ComponentId);

            modelBuilder.Entity<Location>()
              .HasMany(l => l.Credentials)
              .WithOne(c => c.Location)
              .HasForeignKey(f => f.LocationId)
              .HasPrincipalKey(p => p.ComponentId);

            modelBuilder.Entity<Location>()
              .HasMany(l => l.Credentials)
              .WithOne(c => c.Location)
              .HasForeignKey(f => f.LocationId)
              .HasPrincipalKey(p => p.ComponentId);

            modelBuilder.Entity<Location>()
              .HasMany(l => l.Holidays)
              .WithOne(c => c.Location)
              .HasForeignKey(f => f.LocationId)
              .HasPrincipalKey(p => p.ComponentId);

            modelBuilder.Entity<Location>()
              .HasMany(l => l.Readers)
              .WithOne(c => c.Location)
              .HasForeignKey(f => f.LocationId)
              .HasPrincipalKey(p => p.ComponentId);

            modelBuilder.Entity<Location>()
              .HasMany(l => l.RequestExits)
              .WithOne(c => c.Location)
              .HasForeignKey(f => f.LocationId)
              .HasPrincipalKey(p => p.ComponentId);

            modelBuilder.Entity<Location>()
              .HasMany(l => l.Sensors)
              .WithOne(c => c.Location)
              .HasForeignKey(f => f.LocationId)
              .HasPrincipalKey(p => p.ComponentId);

            modelBuilder.Entity<Location>()
              .HasMany(l => l.Strikes)
              .WithOne(c => c.Location)
              .HasForeignKey(f => f.LocationId)
              .HasPrincipalKey(p => p.ComponentId);

            modelBuilder.Entity<Location>()
                .HasMany(l => l.Triggers)
                .WithOne(c => c.Location)
                .HasForeignKey(f => f.LocationId)
                .HasPrincipalKey(p => p.ComponentId);

            modelBuilder.Entity<Location>()
    .HasMany(l => l.Procedures)
    .WithOne(c => c.Location)
    .HasForeignKey(f => f.LocationId)
    .HasPrincipalKey(p => p.ComponentId);

            modelBuilder.Entity<Location>()
    .HasMany(l => l.Actions)
    .WithOne(c => c.Location)
    .HasForeignKey(f => f.LocationId)
    .HasPrincipalKey(p => p.ComponentId);

            #endregion

            #region Transaction

            //modelBuilder.Entity<TransactionSourceType>()
            //    .HasKey(e => new { e.TransactionSourceValue, e.TransactionTypeValue });

            modelBuilder.Entity<TransactionSourceType>()
                .HasOne(e => e.TransactionSource)
                .WithMany(e => e.TransactionSourceTypes)
                .HasForeignKey(e => e.TransactionSourceValue)
                .HasPrincipalKey(e => e.Value);

            modelBuilder.Entity<TransactionSourceType>()
                .HasOne(e => e.TransactionType)
                .WithMany(e => e.TransactionSourceTypes)
                .HasForeignKey(e => e.TransactionTypeValue)
                .HasPrincipalKey(e => e.Value);

            modelBuilder.Entity<TransactionType>()
                .HasMany(e => e.transactionCodes)
                .WithOne(e => e.TransactionType)
                .HasForeignKey(e => e.TransactionTypeValue)
                .HasPrincipalKey(e => e.Value);

            modelBuilder.Entity<TransactionSource>()
                .HasData(
                    new TransactionSource { Id = 1, Name = "SCP diagnostics", Value = 0x00,Source="Hardware" },
                    new TransactionSource { Id = 2, Name = "SCP to HOST communication driver - not defined", Value = 0x01,Source="Hardware" },
                    new TransactionSource { Id = 3, Name = "SCP local monitor points (tamper & power fault)", Value = 0x02,Source="Hardware" },
                    new TransactionSource { Id = 4, Name = "SIO diagnostics", Value = 0x03,Source="Module" },
                    new TransactionSource { Id = 5, Name = "SIO communication driver", Value = 0x04,Source="Module" },
                    new TransactionSource { Id = 6, Name = "SIO cabinet tamper", Value = 0x05,Source="Module" },
                    new TransactionSource { Id = 7, Name = "SIO power monitor", Value = 0x06,Source="Module" },
                    new TransactionSource { Id = 8, Name = "Alarm monitor point", Value = 0x07,Source="Monitor Point" },
                    new TransactionSource { Id = 9, Name = "Output control point", Value = 0x08,Source="Control Point" },
                    new TransactionSource { Id = 10, Name = "Access Control Reader (ACR)", Value = 0x09,Source="Door" },
                    new TransactionSource { Id = 11, Name = "ACR: reader tamper monitor", Value = 0x0A,Source="Door" },
                    new TransactionSource { Id = 12, Name = "ACR: door position sensor", Value = 0x0B,Source="Door" },
                    new TransactionSource { Id = 13, Name = "ACR: 1st \"Request to exit\" input", Value = 0x0D,Source="Door" },
                    new TransactionSource { Id = 14, Name = "ACR: 2nd \"Request to exit\" input", Value = 0x0E,Source="Door" },
                    new TransactionSource { Id = 15, Name = "Time zone", Value = 0x0F,Source="Time zone" },
                    new TransactionSource { Id = 16, Name = "Procedure (action list)", Value = 0x10,Source="Procedure" },
                    new TransactionSource { Id = 17, Name = "Trigger", Value = 0x11,Source="Trigger" },
                    new TransactionSource { Id = 18, Name = "Trigger variable", Value = 0x12,Source="Trigger" },
                    new TransactionSource { Id = 19, Name = "Monitor point group", Value = 0x13,Source="Monitor point group" },
                    new TransactionSource { Id = 20, Name = "Access area", Value = 0x14 ,Source="Access area"},
                    new TransactionSource { Id = 21, Name = "ACR: the alternate reader's tamper monitor source_number", Value = 0x15,Source="Door" },
                    new TransactionSource { Id = 22, Name = "Login Service", Value = 0x18,Source="" }
                );

            modelBuilder.Entity<TransactionType>()
                .HasData(
                    new TransactionType { Id = 1, Name = "System", Value = 0x01 },
                    new TransactionType { Id = 2, Name = "SIO communication status report",  Value = 0x02 },
                    new TransactionType { Id = 3, Name = "Binary card data",  Value = 0x03 },
                    new TransactionType { Id = 4, Name = "Card data",  Value = 0x04 },
                    new TransactionType { Id = 5, Name = "Formatted card: facility code, card number ID, issue code",  Value = 0x05 },
                    new TransactionType { Id = 6, Name = "Formatted card: card number only", Value = 0x06 },
                    new TransactionType { Id = 7, Name = "Change-of-state", Value = 0x07 },
                    new TransactionType { Id = 8, Name = "Exit request", Value = 0x08 },
                    new TransactionType { Id = 9, Name = "Door status monitor change-of-state", Value = 0x09 },
                    new TransactionType { Id = 10, Name = "Procedure (command list) log", Value = 0x0A },
                    new TransactionType { Id = 11, Name = "User command request report", Value = 0x0B },
                    new TransactionType { Id = 12, Name = "Change of state: trigger variable, time zone, & triggers", Value = 0x0C },
                    new TransactionType { Id = 13, Name = "Door mode change", Value = 0x0D },
                    new TransactionType { Id = 14, Name = "Monitor point group status change", Value = 0x0E },
                    new TransactionType { Id = 15, Name = "Access area", Value = 0x0F },
                    new TransactionType { Id = 16, Name = "Extended user command", Value = 0x12 },
                    new TransactionType { Id = 17, Name = "Use limit report", Value = 0x13 },
                    new TransactionType { Id = 18, Name = "Web activity", Value = 0x14 },
                    new TransactionType { Id = 19, Name = "Specify tranTypeCardFull (0x05) instead", Value = 0x15 },
                    new TransactionType { Id = 20, Name = "Specify tranTypeCardID (0x06) instead", Value = 0x16 },
                    new TransactionType { Id = 21, Name = "Operating mode change", Value = 0x18 },
                    new TransactionType { Id = 22, Name = "Elevator Floor Status CoS", Value = 0x1A },
                    new TransactionType { Id = 23, Name = "File Download Status", Value = 0x1B },
                    new TransactionType { Id = 24, Name = "Elevator Floor Access Transaction", Value = 0x1D },
                    new TransactionType { Id = 25, Name = "Specify tranTypeCardFull (0x05) instead", Value = 0x25 },
                    new TransactionType { Id = 26, Name = "Specify tranTypeCardID (0x06) instead", Value = 0x26 },
                    new TransactionType { Id = 27, Name = "Specify tranTypeCardFull (0x05) instead", Value = 0x35 },
                    new TransactionType { Id = 28, Name = "Door extended feature stateless transition", Value = 0x40 },
                    new TransactionType { Id = 29, Name = "Door extended feature change-of-state", Value = 0x41 },

                    // New
                    new TransactionType { Id=30,Name= "Formatted card and user PIN was captured at an ACR", Value=0x42}
                );

            modelBuilder.Entity<TransactionSourceType>()
                .HasData(
                    // tranSrcScpDiag
                    new TransactionSourceType { Id=1,TransactionSourceValue=0x00,TransactionTypeValue=0x01 },
                    new TransactionSourceType { Id=2,TransactionSourceValue=0x00,TransactionTypeValue=0x14},
                    new TransactionSourceType { Id=3,TransactionSourceValue=0x00,TransactionTypeValue=0x18},
                    new TransactionSourceType { Id=4,TransactionSourceValue=0x00,TransactionTypeValue=0x1B},

                    // tranSrcScpLcl 
                    new TransactionSourceType { Id=5,TransactionSourceValue=0x02,TransactionTypeValue=0x07},

                    // tranSrcSioDiag
                    //new TransactionSourceType { Id=6,TransactionSourceValue=0x03,TransactionTypeValue=0x02},

                    // tranSrcSioCom
                    new TransactionSourceType { Id=7,TransactionSourceValue=0x04,TransactionTypeValue=0x02},

                    // tranSrcSioTmpr
                    new TransactionSourceType { Id=8,TransactionSourceValue=0x05,TransactionTypeValue=0x07},

                    // tranSrcSioPwr
                    new TransactionSourceType { Id = 9, TransactionSourceValue = 0x06, TransactionTypeValue = 0x07 },

                    // tranSrcMP
                    new TransactionSourceType { Id = 10, TransactionSourceValue = 0x07, TransactionTypeValue = 0x07 },

                    // tranSrcCP
                    new TransactionSourceType { Id = 11, TransactionSourceValue = 0x08, TransactionTypeValue = 0x07 },

                    // tranSrcACR
                    new TransactionSourceType { Id = 12, TransactionSourceValue = 0x09, TransactionTypeValue = 0x03 },
                    new TransactionSourceType { Id = 13, TransactionSourceValue = 0x09, TransactionTypeValue = 0x04 },
                    new TransactionSourceType { Id = 14, TransactionSourceValue = 0x09, TransactionTypeValue = 0x05 },
                    new TransactionSourceType { Id = 15, TransactionSourceValue = 0x09, TransactionTypeValue = 0x06 },
                    new TransactionSourceType { Id = 16, TransactionSourceValue = 0x09, TransactionTypeValue = 0x08 },
                    new TransactionSourceType { Id = 17, TransactionSourceValue = 0x09, TransactionTypeValue = 0x0B },
                    new TransactionSourceType { Id = 18, TransactionSourceValue = 0x09, TransactionTypeValue = 0x0D },
                    new TransactionSourceType { Id = 19, TransactionSourceValue = 0x09, TransactionTypeValue = 0x12 },
                    new TransactionSourceType { Id = 20, TransactionSourceValue = 0x09, TransactionTypeValue = 0x13 },
                    new TransactionSourceType { Id = 21, TransactionSourceValue = 0x09, TransactionTypeValue = 0x1A },
                    new TransactionSourceType { Id = 22, TransactionSourceValue = 0x09, TransactionTypeValue = 0x1D },
                    new TransactionSourceType { Id = 23, TransactionSourceValue = 0x09, TransactionTypeValue = 0x40 },
                    new TransactionSourceType { Id = 24, TransactionSourceValue = 0x09, TransactionTypeValue = 0x41 },
                    new TransactionSourceType { Id = 25, TransactionSourceValue = 0x09, TransactionTypeValue = 0x13 },

                    // tranSrcAcrTmpr
                    new TransactionSourceType { Id = 26, TransactionSourceValue = 0x0A, TransactionTypeValue = 0x07 },

                    // tranSrcAcrDoor
                    new TransactionSourceType { Id = 27, TransactionSourceValue = 0x0B, TransactionTypeValue = 0x09 },

                    // tranSrcAcrRex0
                    new TransactionSourceType { Id = 28, TransactionSourceValue = 0x0D, TransactionTypeValue = 0x07 },

                    // tranSrcAcrRex1
                    new TransactionSourceType { Id = 29, TransactionSourceValue = 0x0E, TransactionTypeValue = 0x07 },

                    // tranSrcTimeZone
                    new TransactionSourceType { Id = 30, TransactionSourceValue = 0x0F, TransactionTypeValue = 0x0C },

                    // tranSrcProcedure
                    new TransactionSourceType { Id = 31, TransactionSourceValue = 0x10, TransactionTypeValue = 0x0A },

                    // tranSrcTrigger
                    new TransactionSourceType { Id = 32, TransactionSourceValue = 0x11, TransactionTypeValue = 0x0C },

                    // tranSrcTrigVar
                    new TransactionSourceType { Id = 33, TransactionSourceValue = 0x12, TransactionTypeValue = 0x0C },

                    // tranSrcMPG
                    new TransactionSourceType { Id = 34, TransactionSourceValue = 0x13, TransactionTypeValue = 0x0E },

                    // tranSrcArea
                    new TransactionSourceType { Id = 35, TransactionSourceValue = 0x14, TransactionTypeValue = 0x0F },

                    // tranSrcAcrTmprAlt
                    new TransactionSourceType { Id = 36, TransactionSourceValue = 0x15, TransactionTypeValue = 0x07 },

                    // tranSrcLoginService
                    new TransactionSourceType { Id = 37, TransactionSourceValue = 0x18, TransactionTypeValue = 0x07 }


                );

            modelBuilder.Entity<TransactionCode>()
                .HasData(

                    // TypeSys
                    new TransactionCode { Id=1,Name= "SCP power-up diagnostics",Description= "SCP power-up diagnostics", Value =1,TransactionTypeValue=0x01 },
                    new TransactionCode { Id = 2, Name = "Host communications offline", Description= "Host communications offline", Value = 2, TransactionTypeValue = 0x01 },
                    new TransactionCode { Id = 3, Name = "Host communications online",Description= "Host communications online", Value = 3, TransactionTypeValue = 0x01 },
                    new TransactionCode { Id = 4, Name = "Transaction count exceeds the preset limit",Description= "Transaction count exceeds the preset limit", Value = 4, TransactionTypeValue = 0x01 },
                    new TransactionCode { Id = 5, Name = "Configuration database save complete",Description= "Configuration database save complete", Value = 5, TransactionTypeValue = 0x01 },
                    new TransactionCode { Id = 6, Name = "Card database save complete", Description= "Card database save complete", Value = 6, TransactionTypeValue = 0x01 },
                    new TransactionCode { Id = 7, Name = "Card database cleared due to SRAM buffer overflow",Description= "Card database cleared due to SRAM buffer overflow", Value = 7, TransactionTypeValue = 0x01 },

                    // TypeSioComm
                    new TransactionCode { Id = 8, Name = "Disabled",Description= "Communication disabled (result of host command)", Value = 1, TransactionTypeValue = 0x02 },
                    new TransactionCode { Id = 9, Name = "Offline",Description= "Timeout (no/bad response from unit)", Value = 2, TransactionTypeValue = 0x02 },
                    new TransactionCode { Id = 10, Name = "Offline",Description= "Invalid identification from SIO", Value = 3, TransactionTypeValue = 0x02 },
                    new TransactionCode { Id = 11, Name = "Offline",Description="Command too long", Value = 4, TransactionTypeValue = 0x02 },
                    new TransactionCode { Id = 12, Name = "Online",Description= "Normal connection", Value = 5, TransactionTypeValue = 0x02 },
                    new TransactionCode { Id = 13, Name = "hexLoad report",Description= "ser_num is address loaded (-1 = last record)", Value = 6, TransactionTypeValue = 0x02 },

                    // TypeCardBin
                    new TransactionCode { Id = 14, Name = "Access denied",Description= "Invalid card format", Value = 1, TransactionTypeValue = 0x03 },

                    // TypeCardBcd
                    new TransactionCode { Id = 15, Name = "Access denied",Description= "Invalid card format, forward read", Value = 1, TransactionTypeValue = 0x04 },
                    new TransactionCode { Id = 16, Name = "Access denied",Description= "Invalid card format, reverse read", Value = 2, TransactionTypeValue = 0x04 },

                    // TypeCardFull
                    new TransactionCode { Id = 17, Name = "Request rejected",Description= "Access point \"locked\"", Value = 1, TransactionTypeValue = 0x05 },
                    new TransactionCode { Id = 18, Name = "Request accepted",Description= "Access point \"unlocked\"", Value = 2, TransactionTypeValue = 0x05 },
                    new TransactionCode { Id = 19, Name = "Request rejected",Description= "Invalid facility code", Value = 3, TransactionTypeValue = 0x05 },
                    new TransactionCode { Id = 20, Name = "Request rejected",Description= "Invalid facility code extension", Value = 4, TransactionTypeValue = 0x05 },
                    new TransactionCode { Id = 21, Name = "Request rejected",Description= "Not in card file", Value = 5, TransactionTypeValue = 0x05 },
                    new TransactionCode { Id = 22, Name = "Request rejected",Description= "Invalid issue code", Value = 6, TransactionTypeValue = 0x05 },
                    new TransactionCode { Id = 23, Name = "Request granted",Description= "Facility code verified, not used", Value = 7, TransactionTypeValue = 0x05 },
                    new TransactionCode { Id = 24, Name = "Request granted",Description= "Facility code verified, door used", Value = 8, TransactionTypeValue = 0x05 },
                    new TransactionCode { Id = 25, Name = "Access denied",Description= "Asked for host approval, then timed out", Value = 9, TransactionTypeValue = 0x05 },
                    new TransactionCode { Id = 26, Name = "Reporting that this card is \"about to get access granted\"",Description= "Reporting that this card is \"about to get access granted\"", Value = 10, TransactionTypeValue = 0x05 },
                    new TransactionCode { Id = 27, Name = "Access denied",Description= "Count exceeded", Value = 11, TransactionTypeValue = 0x05 },
                    new TransactionCode { Id = 28, Name = "Access denied",Description= "Asked for host approval, then host denied", Value = 12, TransactionTypeValue = 0x05 },
                    new TransactionCode { Id = 29, Name = "Request rejected",Description= "Airlock is busy", Value = 13, TransactionTypeValue = 0x05 },

                    // TypeCardID
                    new TransactionCode { Id = 30, Name = "Request rejected", Description= "Deactivated card", Value = 1, TransactionTypeValue = 0x06 },
                    new TransactionCode { Id = 31, Name = "Request rejected",Description= "Before activation date", Value = 2, TransactionTypeValue = 0x06 },
                    new TransactionCode { Id = 32, Name = "Request rejected", Description= "After expiration date", Value = 3, TransactionTypeValue = 0x06 },
                    new TransactionCode { Id = 33, Name = "Request rejected",Description= "Invalid time", Value = 4, TransactionTypeValue = 0x06 },
                    new TransactionCode { Id = 34, Name = "Request rejected",Description="Invalid PIN", Value = 5, TransactionTypeValue = 0x06 },
                    new TransactionCode { Id = 35, Name = "Request rejected", Description= "Anti-passback violation", Value = 6, TransactionTypeValue = 0x06 },
                    new TransactionCode { Id = 36, Name = "Request granted",Description= "APB violation, not used", Value = 7, TransactionTypeValue = 0x06 },
                    new TransactionCode { Id = 37, Name = "Request granted", Description= "APB violation, used", Value = 8, TransactionTypeValue = 0x06 },
                    new TransactionCode { Id = 38, Name = "Request rejected", Description= "Duress code detected", Value = 9, TransactionTypeValue = 0x06 },
                    new TransactionCode { Id = 39, Name = "Request granted", Description= "Duress, used", Value = 10, TransactionTypeValue = 0x06 },
                    new TransactionCode { Id = 40, Name = "Request granted", Description= "Duress, not used", Value = 11, TransactionTypeValue = 0x06 },
                    new TransactionCode { Id = 41, Name = "Request granted",Description= "Full test, not used", Value = 12, TransactionTypeValue = 0x06 },
                    new TransactionCode { Id = 42, Name = "Request granted",Description= "Full test, used", Value = 13, TransactionTypeValue = 0x06 },
                    new TransactionCode { Id = 43, Name = "Request denied",Description= "Never allowed at this reader (all Tz's = 0)", Value = 14, TransactionTypeValue = 0x06 },
                    new TransactionCode { Id = 44, Name = "Request denied", Description= "No second card presented", Value = 15, TransactionTypeValue = 0x06 },
                    new TransactionCode { Id = 45, Name = "Request denied", Description= "Occupancy limit reached", Value = 16, TransactionTypeValue = 0x06 },
                    new TransactionCode { Id = 46, Name = "Request denied", Description= "The area is NOT enabled", Value = 17, TransactionTypeValue = 0x06 },
                    new TransactionCode { Id = 47, Name = "Request denied", Description= "Use limit", Value = 18, TransactionTypeValue = 0x06 },
                    new TransactionCode { Id = 48, Name = "Granting access",Description= "Used/not used transaction will follow", Value = 21, TransactionTypeValue = 0x06 },
                    new TransactionCode { Id = 49, Name = "Request rejected",Description= "No escort card presented", Value = 24, TransactionTypeValue = 0x06 },
                    new TransactionCode { Id = 50, Name = "Reserved",Description= "Reserved", Value = 25, TransactionTypeValue = 0x06 },
                    new TransactionCode { Id = 51, Name = "Reserved",Description= "Reserved", Value = 26, TransactionTypeValue = 0x06 },
                    new TransactionCode { Id = 52, Name = "Reserved", Description= "Reserved", Value = 27, TransactionTypeValue = 0x06 },
                    new TransactionCode { Id = 53, Name = "Request rejected",Description= "Airlock is busy", Value = 29, TransactionTypeValue = 0x06 },
                    new TransactionCode { Id = 54, Name = "Request rejected",Description= "Incomplete CARD & PIN sequence", Value = 30, TransactionTypeValue = 0x06 },
                    new TransactionCode { Id = 55, Name = "Request granted",Description= "Double-card event", Value = 31, TransactionTypeValue = 0x06 },
                    new TransactionCode { Id = 56, Name = "Request granted", Description= "Double-card event while in uncontrolled state (locked/unlocked)", Value = 32, TransactionTypeValue = 0x06 },
                    new TransactionCode { Id = 57, Name = "Granting access", Description= "Requires escort, pending escort card", Value = 39, TransactionTypeValue = 0x06 },
                    new TransactionCode { Id = 58, Name = "Request rejected", Description= "Violates minimum occupancy count", Value = 40, TransactionTypeValue = 0x06 },
                    new TransactionCode { Id = 59, Name = "Request rejected",Description= "Card pending at another reader", Value = 41, TransactionTypeValue = 0x06 },

                    // TypeHostCardFullPin 
                    new TransactionCode { Id = 60, Name = "Request rejected", Description = "Card pending at another reader", Value = 41, TransactionTypeValue = 0x42 },

                    // TypeHostCardFullPin 
                    //new TranCode { Id=61,Name= "Reporting that this Card and PIN pair is \"requesting access\"",TranCodeDesc= "Reporting that this Card and PIN pair is \"requesting access\"",Value=1,TransactionTypeValue=0x66 },

                    // TypeCoS
                    new TransactionCode { Id = 62, Name = "Disconnected", Description = "Disconnected (from an input point ID)", Value = 1, TransactionTypeValue = 0x07 },
                    new TransactionCode { Id = 63, Name = "Offline", Description = "Unknown (offline): no report from the ID", Value = 2, TransactionTypeValue = 0x07 },
                    new TransactionCode { Id = 64, Name = "Secure", Description = "Secure (or deactivate relay)", Value = 3, TransactionTypeValue = 0x07 },
                    new TransactionCode { Id = 65, Name = "Alarm", Description = "Alarm (or activated relay: perm or temp)", Value = 4, TransactionTypeValue = 0x07 },
                    new TransactionCode { Id = 66, Name = "Fault", Description = "Fault", Value = 5, TransactionTypeValue = 0x07 },
                    new TransactionCode { Id = 67, Name = "Exit delay in progress", Description = "Exit delay in progress", Value = 6, TransactionTypeValue = 0x07 },
                    new TransactionCode { Id = 68, Name = "Entry delay in progress", Description = "Entry delay in progress", Value = 7, TransactionTypeValue = 0x07 },

                     // TypeREX
                     new TransactionCode { Id = 69, Name = "Exit cycle", Description = "Door use not verified", Value = 1, TransactionTypeValue = 0x08 },
                     new TransactionCode { Id = 70, Name = "Exit cycle", Description = "Door not used", Value = 2, TransactionTypeValue = 0x08 },
                     new TransactionCode { Id = 71, Name = "Exit cycle", Description = "Door used", Value = 3, TransactionTypeValue = 0x08 },
                     new TransactionCode { Id = 72, Name = "Host initiated request", Description = "Door use not verified", Value = 4, TransactionTypeValue = 0x08 },
                     new TransactionCode { Id = 73, Name = "Host initiated request", Description = "Door not used", Value = 5, TransactionTypeValue = 0x08 },
                     new TransactionCode { Id = 74, Name = "Host initiated request", Description = "Door used", Value = 6, TransactionTypeValue = 0x08 },
                     new TransactionCode { Id = 75, Name = "Exit cycle", Description = "Started", Value = 9, TransactionTypeValue = 0x08 },

                     // TypeCoSDoor
                     new TransactionCode { Id = 76, Name = "Disconnected", Description = "Disconnected", Value = 1, TransactionTypeValue = 0x09 },
                     new TransactionCode { Id = 77, Name = "Unknown _RS bits: last known status", Description = "Unknown _RS bits: last known status", Value = 2, TransactionTypeValue = 0x09 },
                     new TransactionCode { Id = 78, Name = "Secure", Description = "Secure", Value = 3, TransactionTypeValue = 0x09 },
                     new TransactionCode { Id = 79, Name = "Alarm", Description = "Alarm (forced, held open or both)", Value = 4, TransactionTypeValue = 0x09 },
                     new TransactionCode { Id = 80, Name = "Fault", Description = "Fault (fault type is encoded in door_status byte)", Value = 5, TransactionTypeValue = 0x09 },

                     // TypeProcedure
                     new TransactionCode { Id = 81, Name = "Cancel procedure (abort delay)", Description = "Cancel procedure (abort delay)", Value = 1, TransactionTypeValue = 0x0A },
                     new TransactionCode { Id = 82, Name = "Execute procedure (start new)", Description = "Execute procedure (start new)", Value = 2, TransactionTypeValue = 0x0A },
                     new TransactionCode { Id = 83, Name = "Resume procedure, if paused", Description = "Resume procedure, if paused", Value = 3, TransactionTypeValue = 0x0A },
                     new TransactionCode { Id = 84, Name = "Execute procedure with prefix 256 actions", Description = "Execute procedure with prefix 256 actions", Value = 4, TransactionTypeValue = 0x0A },
                     new TransactionCode { Id = 85, Name = "Execute procedure with prefix 512 actions", Description = "Execute procedure with prefix 512 actions", Value = 5, TransactionTypeValue = 0x0A },
                     new TransactionCode { Id = 86, Name = "Execute procedure with prefix 1024 actions", Description = "Execute procedure with prefix 1024 actions", Value = 6, TransactionTypeValue = 0x0A },
                     new TransactionCode { Id = 87, Name = "Resume procedure with prefix 256 actions", Description = "Resume procedure with prefix 256 actions", Value = 7, TransactionTypeValue = 0x0A },
                     new TransactionCode { Id = 88, Name = "Resume procedure with prefix 512 actions", Description = "Resume procedure with prefix 512 actions", Value = 8, TransactionTypeValue = 0x0A },
                     new TransactionCode { Id = 89, Name = "Resume procedure with prefix 1024 actions", Description = "Resume procedure with prefix 1024 actions", Value = 9, TransactionTypeValue = 0x0A },
                     new TransactionCode { Id = 90, Name = "Command was issued to procedure with no actions - (NOP)", Description = "Command was issued to procedure with no actions - (NOP)", Value = 10, TransactionTypeValue = 0x0A },


                     // TypeUserCmnd
                     new TransactionCode { Id = 91, Name = "Command entered by the user", Description = "Command entered by the user", Value = 10, TransactionTypeValue = 0x0B },

                     // TypeActivate
                     new TransactionCode { Id = 92, Name = "Became inactive", Description = "Became inactive", Value = 1, TransactionTypeValue = 0x0C },
                     new TransactionCode { Id = 93, Name = "Became active", Description = "Became active", Value = 2, TransactionTypeValue = 0x0C },

                     // TypeAcr
                     new TransactionCode { Id = 94, Name = "Disabled", Description = "Disabled", Value = 1, TransactionTypeValue = 0x0D },
                     new TransactionCode { Id = 95, Name = "Unlocked", Description = "Unlocked", Value = 2, TransactionTypeValue = 0x0D },
                     new TransactionCode { Id = 96, Name = "Locked", Description = "Locked (exit request enabled)", Value = 3, TransactionTypeValue = 0x0D },
                     new TransactionCode { Id = 97, Name = "Facility code only", Description = "Facility code only", Value = 4, TransactionTypeValue = 0x0D },
                     new TransactionCode { Id = 98, Name = "Card only", Description = "Card only", Value = 5, TransactionTypeValue = 0x0D },
                     new TransactionCode { Id = 99, Name = "PIN only", Description = "PIN only", Value = 6, TransactionTypeValue = 0x0D },
                     new TransactionCode { Id = 100, Name = "Card and PIN", Description = "Card and PIN", Value = 7, TransactionTypeValue = 0x0D },
                     new TransactionCode { Id = 101, Name = "PIN or card", Description = "PIN or card", Value = 8, TransactionTypeValue = 0x0D },

                     // TypeMPG
                     new TransactionCode { Id = 102, Name = "First disarm command executed", Description = "First disarm command executed (mask_count was 0, all MPs got masked)", Value = 1, TransactionTypeValue = 0x0E },
                     new TransactionCode { Id = 103, Name = "Subsequent disarm command executed", Description = "Subsequent disarm command executed (mask_count incremented, MPs already masked)", Value = 2, TransactionTypeValue = 0x0E },
                     new TransactionCode { Id = 104, Name = "Override command: armed", Description = "Override command: armed (mask_count cleared, all points unmasked)", Value = 3, TransactionTypeValue = 0x0E },
                     new TransactionCode { Id = 105, Name = "Override command: disarmed", Description = "Override command: disarmed (mask_count set, unmasked all points)", Value = 4, TransactionTypeValue = 0x0E },
                     new TransactionCode { Id = 106, Name = "Force arm command, MPG armed", Description = "Force arm command, MPG armed, (may have active zones, mask_count is now zero)", Value = 5, TransactionTypeValue = 0x0E },
                     new TransactionCode { Id = 107, Name = "Force arm command, MPG not armed", Description = "Force arm command, MPG not armed (mask_count decremented)", Value = 6, TransactionTypeValue = 0x0E },
                     new TransactionCode { Id = 108, Name = "Standard arm command, MPG armed", Description = "Standard arm command, MPG armed (did not have active zones, mask_count is now zero)", Value = 7, TransactionTypeValue = 0x0E },
                     new TransactionCode { Id = 109, Name = "Standard arm command, MPG did not arm", Description = "Standard arm command, MPG did not arm, (had active zones, mask_count unchanged)d", Value = 8, TransactionTypeValue = 0x0E },
                     new TransactionCode { Id = 110, Name = "Standard arm command, MPG still armed", Description = "Standard arm command, MPG still armed, (mask_count decremented)", Value = 9, TransactionTypeValue = 0x0E },
                     new TransactionCode { Id = 111, Name = "Override arm command, MPG armed", Description = "Override arm command, MPG armed (mask_count is now zero)", Value = 10, TransactionTypeValue = 0x0E },
                     new TransactionCode { Id = 112, Name = "Override arm command, MPG did not arm", Description = "Override arm command, MPG did not arm, (mask_count decremented)", Value = 11, TransactionTypeValue = 0x0E },

                     // TypeArea
                     new TransactionCode { Id = 113, Name = "Area disabled", Description = "Area disabled", Value = 1, TransactionTypeValue = 0x0F },
                     new TransactionCode { Id = 114, Name = "Area enabled", Description = "Area enabled", Value = 2, TransactionTypeValue = 0x0F },
                     new TransactionCode { Id = 115, Name = "Occupancy count reached zero", Description = "Occupancy count reached zero", Value = 3, TransactionTypeValue = 0x0F },
                     new TransactionCode { Id = 116, Name = "Occupancy count reached the \"downward-limit\"", Description = "Occupancy count reached the \"downward-limit\"", Value = 4, TransactionTypeValue = 0x0F },
                     new TransactionCode { Id = 117, Name = "Occupancy count reached the \"upward-limit\"", Description = "Occupancy count reached the \"upward-limit\"", Value = 5, TransactionTypeValue = 0x0F },
                     new TransactionCode { Id = 118, Name = "Occupancy count reached the \"max-occupancy-limit\"", Description = "Occupancy count reached the \"max-occupancy-limit\"", Value = 6, TransactionTypeValue = 0x0F },
                     new TransactionCode { Id = 119, Name = "Multi-occupancy mode changed", Description = "Multi-occupancy mode changed", Value = 7, TransactionTypeValue = 0x0F },

                    // TypeWebActivity
                    // Web Activity Transaction Codes (TransactionTypeValue = 0x14)
                    new TransactionCode { Id = 120, Name = "Save home notes", Description = "Save home notes", Value = 1, TransactionTypeValue = 0x14 },
                    new TransactionCode { Id = 121, Name = "Save network settings", Description = "Save network settings", Value = 2, TransactionTypeValue = 0x14 },
                    new TransactionCode { Id = 122, Name = "Save host communication settings", Description = "Save host communication settings", Value = 3, TransactionTypeValue = 0x14 },
                    new TransactionCode { Id = 123, Name = "Add user", Description = "Add user", Value = 4, TransactionTypeValue = 0x14 },
                    new TransactionCode { Id = 124, Name = "DeleteAsync user", Description = "DeleteAsync user", Value = 5, TransactionTypeValue = 0x14 },
                    new TransactionCode { Id = 125, Name = "Modify user", Description = "Modify user", Value = 6, TransactionTypeValue = 0x14 },
                    new TransactionCode { Id = 126, Name = "Save password strength and session timer", Description = "Save password strength and session timer", Value = 7, TransactionTypeValue = 0x14 },
                    new TransactionCode { Id = 127, Name = "Save web server options", Description = "Save web server options", Value = 8, TransactionTypeValue = 0x14 },
                    new TransactionCode { Id = 128, Name = "Save time server settings", Description = "Save time server settings", Value = 9, TransactionTypeValue = 0x14 },
                    new TransactionCode { Id = 129, Name = "Auto save timer settings", Description = "Auto save timer settings", Value = 10, TransactionTypeValue = 0x14 },
                    new TransactionCode { Id = 130, Name = "Load certificate", Description = "Load certificate", Value = 11, TransactionTypeValue = 0x14 },
                    new TransactionCode { Id = 131, Name = "Logged out by link", Description = "Logged out by link", Value = 12, TransactionTypeValue = 0x14 },
                    new TransactionCode { Id = 132, Name = "Logged out by timeout", Description = "Logged out by timeout", Value = 13, TransactionTypeValue = 0x14 },
                    new TransactionCode { Id = 133, Name = "Logged out by user", Description = "Logged out by user", Value = 14, TransactionTypeValue = 0x14 },
                    new TransactionCode { Id = 134, Name = "Logged out by apply", Description = "Logged out by apply", Value = 15, TransactionTypeValue = 0x14 },
                    new TransactionCode { Id = 135, Name = "Invalid login", Description = "Invalid login", Value = 16, TransactionTypeValue = 0x14 },
                    new TransactionCode { Id = 136, Name = "Successful login", Description = "Successful login", Value = 17, TransactionTypeValue = 0x14 },
                    new TransactionCode { Id = 137, Name = "Network diagnostic saved", Description = "Network diagnostic saved", Value = 18, TransactionTypeValue = 0x14 },
                    new TransactionCode { Id = 138, Name = "Card DB size saved", Description = "Card DB size saved", Value = 19, TransactionTypeValue = 0x14 },
                    new TransactionCode { Id = 139, Name = "Diagnostic page saved", Description = "Diagnostic page saved", Value = 21, TransactionTypeValue = 0x14 },
                    new TransactionCode { Id = 140, Name = "Security options page saved", Description = "Security options page saved", Value = 22, TransactionTypeValue = 0x14 },
                    new TransactionCode { Id = 141, Name = "Add-on package page saved", Description = "Add-on package page saved", Value = 23, TransactionTypeValue = 0x14 },
                    //new TranCode { Id = 142, Name = "Not used", TranCodeDesc = "Not used", Value = 24, TransactionTypeValue = 0x14 },
                    //new TranCode { Id = 143, Name = "Not used", TranCodeDesc = "Not used", Value = 25, TransactionTypeValue = 0x14 },
                    //new TranCode { Id = 144, Name = "Not used", TranCodeDesc = "Not used", Value = 26, TransactionTypeValue = 0x14 },
                    new TransactionCode { Id = 145, Name = "Invalid login limit reached", Description = "Invalid login limit reached", Value = 27, TransactionTypeValue = 0x14 },
                    new TransactionCode { Id = 146, Name = "Firmware download initiated", Description = "Firmware download initiated", Value = 28, TransactionTypeValue = 0x14 },
                    new TransactionCode { Id = 147, Name = "Advanced networking routes saved", Description = "Advanced networking routes saved", Value = 29, TransactionTypeValue = 0x14 },
                    new TransactionCode { Id = 148, Name = "Advanced networking reversion timer started", Description = "Advanced networking reversion timer started", Value = 30, TransactionTypeValue = 0x14 },
                    new TransactionCode { Id = 149, Name = "Advanced networking reversion timer elapsed", Description = "Advanced networking reversion timer elapsed", Value = 31, TransactionTypeValue = 0x14 },
                    new TransactionCode { Id = 150, Name = "Advanced networking route changes reverted", Description = "Advanced networking route changes reverted", Value = 32, TransactionTypeValue = 0x14 },
                    new TransactionCode { Id = 151, Name = "Advanced networking route changes cleared", Description = "Advanced networking route changes cleared", Value = 33, TransactionTypeValue = 0x14 },
                    new TransactionCode { Id = 152, Name = "Certificate generation started", Description = "Certificate generation started", Value = 34, TransactionTypeValue = 0x14 },


                    // TypeOperatingMode
                    new TransactionCode { Id = 153, Name = "Operating mode changed to mode 0", Description = "Operating mode changed to mode 0", Value = 1, TransactionTypeValue = 0x18 },
                    new TransactionCode { Id = 154, Name = "Operating mode changed to mode 1", Description = "Operating mode changed to mode 1", Value = 2, TransactionTypeValue = 0x18 },
                    new TransactionCode { Id = 155, Name = "Operating mode changed to mode 2", Description = "Operating mode changed to mode 2", Value = 3, TransactionTypeValue = 0x18 },
                    new TransactionCode { Id = 156, Name = "Operating mode changed to mode 3", Description = "Operating mode changed to mode 3", Value = 4, TransactionTypeValue = 0x18 },
                    new TransactionCode { Id = 157, Name = "Operating mode changed to mode 4", Description = "Operating mode changed to mode 4", Value = 5, TransactionTypeValue = 0x18 },
                    new TransactionCode { Id = 158, Name = "Operating mode changed to mode 5", Description = "Operating mode changed to mode 5", Value = 6, TransactionTypeValue = 0x18 },
                    new TransactionCode { Id = 159, Name = "Operating mode changed to mode 6", Description = "Operating mode changed to mode 6", Value = 7, TransactionTypeValue = 0x18 },
                    new TransactionCode { Id = 160, Name = "Operating mode changed to mode 7", Description = "Operating mode changed to mode 7", Value = 8, TransactionTypeValue = 0x18 },

                    // TypeCoSElevator
                    new TransactionCode { Id = 161, Name = "Secure", Description = "Floor status is secure", Value = 1, TransactionTypeValue = 0x1A },
                    new TransactionCode { Id = 162, Name = "Public", Description = "Floor status is public", Value = 2, TransactionTypeValue = 0x1A },
                    new TransactionCode { Id = 163, Name = "Disabled (override)", Description = "Floor status is disabled (override)", Value = 3, TransactionTypeValue = 0x1A },

                    // TypeFileDownloadStatus
                    new TransactionCode { Id = 164, Name = "File transfer success", Description = "File transfer success", Value = 1, TransactionTypeValue = 0x1B },
                    new TransactionCode { Id = 165, Name = "File transfer error", Description = "File transfer error", Value = 2, TransactionTypeValue = 0x1B },
                    new TransactionCode { Id = 166, Name = "File delete successful", Description = "File delete successful", Value = 3, TransactionTypeValue = 0x1B },
                    new TransactionCode { Id = 167, Name = "File delete unsuccessful", Description = "File delete unsuccessful", Value = 4, TransactionTypeValue = 0x1B },
                    new TransactionCode { Id = 168, Name = "OSDP file transfer complete (primary ACR)", Description = "OSDP file transfer complete (primary ACR) - look at source number for ACR number", Value = 5, TransactionTypeValue = 0x1B },
                    new TransactionCode { Id = 169, Name = "OSDP file transfer error (primary ACR)", Description = "OSDP file transfer error (primary ACR) - look at source number for ACR number", Value = 6, TransactionTypeValue = 0x1B },
                    new TransactionCode { Id = 170, Name = "OSDP file transfer complete (alternate ACR)", Description = "OSDP file transfer complete (alternate ACR) - look at source number for ACR number", Value = 7, TransactionTypeValue = 0x1B },
                    new TransactionCode { Id = 171, Name = "OSDP file transfer error (alternate ACR)", Description = "OSDP file transfer error (alternate ACR) - look at source number for ACR number", Value = 8, TransactionTypeValue = 0x1B },

                    // TypeCoSElevatorAccess
                    new TransactionCode { Id = 172, Name = "Elevator access", Description = "Elevator access", Value = 1, TransactionTypeValue = 0x1D },

                    // TypeAcrExtFeatureStls
                    new TransactionCode { Id = 173, Name = "Extended status updated", Description = "Extended status updated", Value = 1, TransactionTypeValue = 0x40 },

                    // TypeAcrExtFeatureCoS
                    new TransactionCode { Id = 174, Name = "Secure / Inactive", Description = "Secure / Inactive", Value = 3, TransactionTypeValue = 0x41 },
                    new TransactionCode { Id = 175, Name = "Alarm / Active", Description = "Alarm / Active", Value = 4, TransactionTypeValue = 0x41 }

                );

            modelBuilder.Entity<Transaction>()
                .HasMany(x => x.TransactionFlags)
                .WithOne(x => x.Transaction);

            //// TypeSys
            
            //modelBuilder.Entity<Transaction>()
            //    .HasOne(t => t.TypeSys)
            //    .WithOne(t => t.Transaction); 

            //modelBuilder.Entity<TypeSys>()
            //    .HasMany(x => x.TransactionFlags)
            //    .WithOne(x => x.TypeSys);

            //// Type Web Activity

            //modelBuilder.Entity<Transaction>()
            //    .HasOne(t => t.TypeWebActivity)
            //    .WithOne(t => t.Transaction);

            //modelBuilder.Entity<TypeWebActivity>()
            //    .HasMany(x => x.TransactionFlags)
            //    .WithOne(x => x.TypeWebActivity);

            //// Type File Download

            //modelBuilder.Entity<Transaction>()
            //    .HasOne(t => t.TypeFileDownloadStatus)
            //    .WithOne(t => t.Transaction);

            //modelBuilder.Entity<TypeFileDownloadStatus>()
            //    .HasMany(x => x.TransactionFlags)
            //    .WithOne(x => x.TypeFileDownloadStatus);

            //// Type Cos

            //modelBuilder.Entity<Transaction>()
            //    .HasOne(t => t.TypeCos)
            //    .WithOne(t => t.Transaction);

            //modelBuilder.Entity<TypeCos>()
            //    .HasMany(x => x.TransactionFlags)
            //    .WithOne(x => x.TypeCos);

            //// TypeSioDiag

            //modelBuilder.Entity<Transaction>()
            //    .HasOne(t => t.TypeSioDiag)
            //    .WithOne(t => t.Transaction);

            //modelBuilder.Entity<TypeSioDiag>()
            //    .HasMany(x => x.TransactionFlags)
            //    .WithOne(x => x.TypeSioDiag);

            //// TypeSioComm

            //modelBuilder.Entity<Transaction>()
            //    .HasOne(t => t.TypeSioComm)
            //    .WithOne(t => t.Transaction);

            //modelBuilder.Entity<HIDAeroService.Entity.TypeSioComm>()
            //    .HasMany(x => x.TransactionFlags)
            //    .WithOne(x => x.TypeSioComm);

            //// TypeCardBin

            //modelBuilder.Entity<Transaction>()
            //    .HasOne(t => t.TypeCardBin)
            //    .WithOne(t => t.Transaction);

            //modelBuilder.Entity<HIDAeroService.Entity.TypeCardBin>()
            //    .HasMany(x => x.TransactionFlags)
            //    .WithOne(x => x.TypeCardBin);

            //// TypeCardBcd

            //modelBuilder.Entity<Transaction>()
            //    .HasOne(t => t.TypeCardBcd)
            //    .WithOne(t => t.Transaction);

            //modelBuilder.Entity<HIDAeroService.Entity.TypeCardBcd>()
            //    .HasMany(x => x.TransactionFlags)
            //    .WithOne(x => x.TypeCardBcd);

            //// TypeCardFull
            //modelBuilder.Entity<Transaction>()
            //    .HasOne(t => t.TypeCardFull)
            //    .WithOne(t => t.Transaction);

            //modelBuilder.Entity<HIDAeroService.Entity.TypeCardFull>()
            //    .HasMany(x => x.TransactionFlags)
            //    .WithOne(x => x.TypeCardFull);


            //// TypeCardID
            //modelBuilder.Entity<Transaction>()
            //    .HasOne(t => t.TypeCardID)
            //    .WithOne(t => t.Transaction);

            //modelBuilder.Entity<HIDAeroService.Entity.TypeCardID>()
            //    .HasMany(x => x.TransactionFlags)
            //    .WithOne(x => x.TypeCardID);

            //// TypeREX
            //modelBuilder.Entity<Transaction>()
            //    .HasOne(t => t.TypeREX)
            //    .WithOne(t => t.Transaction);

            //modelBuilder.Entity<HIDAeroService.Entity.TypeREX>()
            //    .HasMany(x => x.TransactionFlags)
            //    .WithOne(x => x.TypeREX);

            //// TypeUserCmnd
            //modelBuilder.Entity<Transaction>()
            //    .HasOne(t => t.TypeUserCmnd)
            //    .WithOne(t => t.Transaction);

            //modelBuilder.Entity<HIDAeroService.Entity.TypeUserCmnd>()
            //    .HasMany(x => x.TransactionFlags)
            //    .WithOne(x => x.TypeUserCmnd);

            ////TypeAcr
            //modelBuilder.Entity<Transaction>()
            //   .HasOne(t => t.TypeAcr)
            //   .WithOne(t => t.Transaction);

            //modelBuilder.Entity<HIDAeroService.Entity.TypeAcr>()
            //    .HasMany(x => x.TransactionFlags)
            //    .WithOne(x => x.TypeAcr);

            //// TypeUseLimit

            //modelBuilder.Entity<Transaction>()
            //   .HasOne(t => t.TypeUseLimit)
            //   .WithOne(t => t.Transaction);

            //modelBuilder.Entity<HIDAeroService.Entity.TypeUseLimit>()
            //    .HasMany(x => x.TransactionFlags)
            //    .WithOne(x => x.TypeUseLimit);

            //// TypeCosElevator

            //modelBuilder.Entity<Transaction>()
            //   .HasOne(t => t.TypeCosElevator)
            //   .WithOne(t => t.Transaction);

            //modelBuilder.Entity<HIDAeroService.Entity.TypeCosElevator>()
            //    .HasMany(x => x.TransactionFlags)
            //    .WithOne(x => x.TypeCosElevator);

            //// TypeCosElevatorAccess

            //modelBuilder.Entity<Transaction>()
            //   .HasOne(t => t.TypeCosElevatorAccess)
            //   .WithOne(t => t.Transaction);

            //modelBuilder.Entity<HIDAeroService.Entity.TypeCosElevatorAccess>()
            //    .HasMany(x => x.TransactionFlags)
            //    .WithOne(x => x.TypeCosElevatorAccess);

            //// TypeAcrExtFeatureStls

            //modelBuilder.Entity<Transaction>()
            //   .HasOne(t => t.TypeAcrExtFeatureStls)
            //   .WithOne(t => t.Transaction);

            //modelBuilder.Entity<HIDAeroService.Entity.TypeAcrExtFeatureStls>()
            //    .HasMany(x => x.TransactionFlags)
            //    .WithOne(x => x.TypeAcrExtFeatureStls);

            //// TypeAcrExtFeatureCoS

            //modelBuilder.Entity<Transaction>()
            //   .HasOne(t => t.TypeAcrExtFeatureCoS)
            //   .WithOne(t => t.Transaction);

            //modelBuilder.Entity<HIDAeroService.Entity.TypeAcrExtFeatureCoS>()
            //    .HasMany(x => x.TransactionFlags)
            //    .WithOne(x => x.TypeAcrExtFeatureCoS);

            //// TypeCosDoor

            //modelBuilder.Entity<Transaction>()
            //   .HasOne(t => t.TypeCoSDoor)
            //   .WithOne(t => t.Transaction);

            //modelBuilder.Entity<HIDAeroService.Entity.TypeCoSDoor>()
            //    .HasMany(x => x.TransactionFlags)
            //    .WithOne(x => x.TypeCoSDoor);

            //// TypeMPG

            //modelBuilder.Entity<Transaction>()
            //   .HasOne(t => t.TypeMPG)
            //   .WithOne(t => t.Transaction);

            //modelBuilder.Entity<HIDAeroService.Entity.TypeMPG>()
            //    .HasMany(x => x.TransactionFlags)
            //    .WithOne(x => x.TypeMPG);

            //// TypeArea

            //modelBuilder.Entity<Transaction>()
            //   .HasOne(t => t.TypeArea)
            //   .WithOne(t => t.Transaction);

            //modelBuilder.Entity<HIDAeroService.Entity.TypeArea>()
            //    .HasMany(x => x.TransactionFlags)
            //    .WithOne(x => x.TypeArea);


            #endregion

            #region Access Area

            modelBuilder.Entity<AccessArea>()
                .HasMany(a => a.DoorsIn)
                .WithOne(d => d.AreaIn)
                .HasForeignKey(f => f.AntiPassBackIn)
                .HasPrincipalKey(p => p.ComponentId);

            modelBuilder.Entity<AccessArea>()
                .HasMany(a => a.DoorsOut)
                .WithOne(d => d.AreaOut)
                .HasForeignKey(f => f.AntiPassBackOut)
                .HasPrincipalKey(p => p.ComponentId);

            modelBuilder.Entity<AccessArea>()
                .HasData(
                    new AccessArea { Id=1,ComponentId=-1,MultiOccupancy=0,AccessControl=0,OccControl=0,OccSet=0,OccMax=0,OccUp=0,OccDown=0,AreaFlag=0,Uuid=SeedDefaults.SystemGuid,LocationId=1,IsActive=true,CreatedDate=SeedDefaults.SystemDate,UpdatedDate=SeedDefaults.SystemDate,Name="Any Area", }
                );

            modelBuilder.Entity<AccessAreaCommandOption>()
                .HasData(
                    new AccessAreaCommandOption { Id=1,Value=1,Name="Disable Area",Description= "Disable Area" },
                    new AccessAreaCommandOption { Id = 2, Value = 2, Name = "Enable area", Description = "Enable area" },
                    new AccessAreaCommandOption { Id = 3, Value = 3, Name = "Set current occupancy to occ_set value", Description = "Set current occupancy to occ_set value" },
                    new AccessAreaCommandOption { Id = 4, Value = 5, Name = "Clear occupancy counts of the “standard” and “special” users", Description = "Clear occupancy counts of the “standard” and “special” users" },
                    new AccessAreaCommandOption { Id = 5, Value = 6, Name = "Disable multi-occupancy rules", Description = "Disable multi-occupancy rules" },
                    new AccessAreaCommandOption { Id = 6, Value = 7, Name = "Enable standard multi-occupancy processing", Description = "Enable standard multi-occupancy processing" }
                );

            modelBuilder.Entity<AccessAreaAccessControlOption>()
                .HasData(
                    new AccessAreaAccessControlOption { Id=1,Name= "NOP",Value=0,Description="No Operation" },
                    new AccessAreaAccessControlOption { Id=2,Name= "Disable area",Value=1,Description="No One Can Access" },
                    new AccessAreaAccessControlOption { Id=3,Name="Enable area",Value=2,Description="Enable Area"}
                );

            modelBuilder.Entity<OccupancyControlOption>()
                .HasData(
                    new OccupancyControlOption {Id=1,Name= "Do not change current occupancy count",Value=0,Description= "Do not change current occupancy count" },
                    new OccupancyControlOption { Id=2,Name= "Change current occupancy to occ_set",Value=1,Description= "Change current occupancy to occ_set" }
                );

            modelBuilder.Entity<AreaFlagOption>()
                .HasData(
                    new AreaFlagOption { Id=1,Name="Interlock",Value=0x01,Description= "Area can have open thresholds to only one other area" },
                    new AreaFlagOption { Id=2,Name="AirLock One Door Only",Value=0x02,Description= "Just (O)ne (D)oor (O)nly is allowed to be open into this area (AREA_F_AIRLOCK must also be set)" }
                );

            modelBuilder.Entity<MultiOccupancyOption>()
                .HasData(
                    new MultiOccupancyOption { Id = 1, Name = "Two or more not required in area", Value = 0, Description = "Two or more not required in area" },
                    new MultiOccupancyOption { Id = 2, Name = "Two or more required", Value = 1, Description = "Two or more required" }
                    );

            #endregion

            #region Feature

            modelBuilder.Entity<Feature>()
                .HasMany(s => s.SubFeatures)
                .WithOne(s => s.Features)
                .HasForeignKey(k => k.FeatureId)
                .HasPrincipalKey(l => l.ComponentId);


            modelBuilder.Entity<Feature>()
                .HasData(
                    new Feature { Id = 1, ComponentId = 1, Name = "Dashboard", Path = "/" },
                    new Feature { Id = 2, ComponentId = 2, Name = "Transactions", Path = "/event" },
                    new Feature { Id = 3, ComponentId = 3, Name = "Locations", Path = "/location" },
                    new Feature { Id = 4, ComponentId = 4, Name = "Alerts", Path = "/alert" },
                    new Feature { Id = 5, ComponentId = 5, Name = "Operators" },
                    new Feature { Id = 6, ComponentId = 6, Name = "Devices" },
                    new Feature { Id = 7, ComponentId = 7, Name = "Doors", Path = "/door" },
                    new Feature { Id = 8, ComponentId = 8, Name = "Card Holder", Path = "/cardholder" },
                    new Feature { Id = 9, ComponentId = 9, Name = "Access Level", Path = "/level" },
                    new Feature { Id = 10, ComponentId = 10, Name = "Access Area", Path = "/area" },
                    new Feature { Id = 11, ComponentId = 11, Name = "Time" },
                    new Feature { Id = 12, ComponentId = 12, Name = "Trigger & Procedure" },
                    new Feature { Id = 13, ComponentId = 13, Name = "Reports" },
                    new Feature { Id = 14, ComponentId = 14, Name = "Settings", Path = "/setting" },
                    new Feature { Id = 15, ComponentId = 15, Name = "Maps", Path = "/map" }

                );

            modelBuilder.Entity<SubFeature>()
                .HasData(
                new SubFeature { Id = 1, ComponentId = 1, Name = "Operator", Path = "/operator", FeatureId = 5 },
                new SubFeature { Id = 2, ComponentId = 2, Name = "Role", Path = "/role", FeatureId = 5 },
                new SubFeature { Id = 3, ComponentId = 3, Name = "Hardwares", Path = "/hardware", FeatureId = 6 },
                new SubFeature { Id = 4, ComponentId = 4, Name = "Modules", Path = "/module", FeatureId = 6 },
                new SubFeature { Id = 5, ComponentId = 5, Name = "Control Points", Path = "/control", FeatureId = 6 },
                new SubFeature { Id = 6, ComponentId = 6, Name = "Monitor Points", Path = "/monitor", FeatureId = 6 },
                new SubFeature { Id = 7, ComponentId = 7, Name = "Monitor Points Groups", Path = "/monitorgroup", FeatureId = 6 },
                new SubFeature { Id = 8, ComponentId = 8, Name = "Timezone", Path = "/timezone", FeatureId = 11 },
                new SubFeature { Id = 9, ComponentId = 9, Name = "Holidays", Path = "/holiday", FeatureId = 11 },
                new SubFeature { Id = 10, ComponentId = 10, Name = "Intervals", Path = "/interval", FeatureId = 11 },
                new SubFeature { Id = 11, ComponentId = 11, Name = "Trigger", Path = "/trigger", FeatureId = 12 },
                new SubFeature { Id = 12, ComponentId = 12, Name = "Procedure", Path = "/action", FeatureId = 12 },
                new SubFeature { Id = 13, ComponentId = 13, Name = "Transaction", Path = "/transaction", FeatureId = 13 },
                new SubFeature { Id = 14, ComponentId = 14, Name = "Audit Trail", Path = "/audit", FeatureId = 13 }


                );

            #endregion

            #region FeatureRole 

            modelBuilder.Entity<FeatureRole>()
                .HasData(
                    new FeatureRole { FeatureId=1,RoleId=1,IsAllow=true,IsCreate=true,IsModify=true,IsDelete=true,IsAction=true },
                     new FeatureRole { FeatureId = 2, RoleId = 1, IsAllow = true, IsCreate=true,IsModify=true,IsDelete=true,IsAction=true },
                      new FeatureRole { FeatureId = 3, RoleId = 1, IsAllow = true, IsCreate=true,IsModify=true,IsDelete=true,IsAction=true },
                       new FeatureRole { FeatureId = 4, RoleId = 1, IsAllow = true, IsCreate=true,IsModify=true,IsDelete=true,IsAction=true },
                        new FeatureRole {  FeatureId = 5, RoleId = 1, IsAllow = true, IsCreate=true,IsModify=true,IsDelete=true,IsAction=true },
                         new FeatureRole { FeatureId = 6, RoleId = 1, IsAllow = true, IsCreate=true,IsModify=true,IsDelete=true,IsAction=true },
                          new FeatureRole { FeatureId = 7, RoleId = 1, IsAllow = true, IsCreate=true,IsModify=true,IsDelete=true,IsAction=true },
                           new FeatureRole {  FeatureId = 8, RoleId = 1, IsAllow = true, IsCreate=true,IsModify=true,IsDelete=true,IsAction=true },
                            new FeatureRole {  FeatureId = 9, RoleId = 1, IsAllow = true, IsCreate=true,IsModify=true,IsDelete=true,IsAction=true },
                             new FeatureRole {  FeatureId = 10, RoleId = 1, IsAllow = true, IsCreate=true,IsModify=true,IsDelete=true,IsAction=true },
                              new FeatureRole {  FeatureId = 11, RoleId = 1, IsAllow = true, IsCreate=true,IsModify=true,IsDelete=true,IsAction=true },
                               new FeatureRole {  FeatureId = 12, RoleId = 1, IsAllow = true, IsCreate=true,IsModify=true,IsDelete=true,IsAction=true },
                                new FeatureRole {  FeatureId = 13, RoleId = 1, IsAllow = true, IsCreate=true,IsModify=true,IsDelete=true,IsAction=true },
                                 new FeatureRole {  FeatureId = 14, RoleId = 1, IsAllow = true, IsCreate=true,IsModify=true,IsDelete=true,IsAction=true },
                                  new FeatureRole {  FeatureId = 15, RoleId = 1, IsAllow = true, IsCreate=true,IsModify=true,IsDelete=true,IsAction=true }
                );

            #endregion

            #region Role

            modelBuilder.Entity<Role>()
                .HasData(
                    new Role { Id=1,ComponentId=1,Name="Administrator",UpdatedDate=SeedDefaults.SystemDate,CreatedDate=SeedDefaults.SystemDate}
                );

            #endregion

            #region Operator

            //modelBuilder.Entity<Role>()
            //    .HasMany(o => o.Operators)
            //    .WithOne(r => r.Role)
            //    .HasForeignKey(o => o.RoleId)
            //    .HasPrincipalKey(r => r.ComponentId)
            //    .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OperatorLocation>()
                .HasKey(x => new { x.LocationId, x.OperatorId });

            modelBuilder.Entity<OperatorLocation>()
                .HasOne(x => x.Location)
                .WithMany(x => x.OperatorLocations)
                .HasForeignKey(x => x.LocationId)
                .HasPrincipalKey(x => x.ComponentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OperatorLocation>()
                .HasOne(x => x.Operator)
                .WithMany(x => x.OperatorLocations)
                .HasForeignKey(x => x.OperatorId)
                .HasPrincipalKey(x => x.ComponentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OperatorLocation>()
                .HasData(
                    new OperatorLocation { Id=1,LocationId=1,OperatorId=1 }
                );

            modelBuilder.Entity<Operator>()
                .HasOne(o => o.Role)
                .WithMany(r => r.Operators)
                .HasForeignKey(o => o.RoleId)
                .HasPrincipalKey(r => r.ComponentId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<FeatureRole>()
                .HasKey(e => new { e.RoleId, e.FeatureId });

            modelBuilder.Entity<FeatureRole>()
                .HasOne(e => e.Role)
                .WithMany(e => e.FeatureRoles)
                .HasForeignKey(e => e.RoleId)
                .HasPrincipalKey(e => e.ComponentId);

            modelBuilder.Entity<FeatureRole>()
                .HasOne(e => e.Feature)
                .WithMany(e => e.FeatureRoles)
                .HasForeignKey(e => e.FeatureId)
                .HasPrincipalKey(e => e.ComponentId);

            modelBuilder.Entity<Operator>()
                .HasData(
                    new Operator { Id = 1,ComponentId=1,UserId="Administrator",Username = "admin", Password = "2439iBIqejYGcodz6j0vGvyeI25eOrjMX3QtIhgVyo0M4YYmWbS+NmGwo0LLByUY", Email = "support@honorsupplying.com", Title = "Mr.", FirstName = "Administrator", MiddleName = "", LastName = "", Phone = "", ImagePath = "", RoleId = 1,Uuid=SeedDefaults.SystemGuid,CreatedDate=SeedDefaults.SystemDate,UpdatedDate=SeedDefaults.SystemDate,IsActive=true }
                );

            #endregion

            #region Procedure

            modelBuilder.Entity<Procedure>()
                .HasMany(x => x.Actions)
                .WithOne(x => x.Procedure)
                .HasForeignKey(x => x.ProcedureId)
                .HasPrincipalKey(x => x.ComponentId);

            #endregion

            #region Trigger

            modelBuilder.Entity<Trigger>()
                .HasOne(x => x.Procedure)
                .WithOne(x => x.Trigger)
                .HasForeignKey<Trigger>(x => x.ProcedureId)
                .HasPrincipalKey<Procedure>(x => x.ComponentId);

            #endregion



            #region File Type

            modelBuilder.Entity<FileType>()
               .HasData(
                   new FileType
                   {
                       Id = 1,
                       Value = 0,
                       Name = "Host Comm certificate file. The file should be in the same format currently used by the default certificate (PEM)."
                   },
                   new FileType
                   {
                       Id = 2,
                       Value = 1,
                       Name = "User defined file. This file can contain any type of data, up to one block in size. This file can have a name on the SCP up to 259 bytes."
                   },
                   new FileType
                   {
                       Id = 3,
                       Value = 2,
                       Name = "License file. This file will be generated by HID and needed on only those products that require a license."
                   },
                   new FileType
                   {
                       Id = 4,
                       Value = 3,
                       Name = "Peer certificate"
                   },
                   new FileType
                   {
                       Id = 5,
                       Value = 4,
                       Name = "OSDP file transfer files"
                   },
                   new FileType
                   {
                       Id = 6,
                       Value = 7,
                       Name = "Linq certificate"
                   },
                   new FileType
                   {
                       Id = 7,
                       Value = 8,
                       Name = "Over-Watch certificate"
                   },
                   new FileType
                   {
                       Id = 8,
                       Value = 9,
                       Name = "Web server certificate"
                   },
                   new FileType
                   {
                       Id = 9,
                       Value = 10,
                       Name = "HID Origo™ certificate"
                   },
                   new FileType
                   {
                       Id = 10,
                       Value = 11,
                       Name = "Aperio certificate"
                   },
                   new FileType
                   {
                       Id = 11,
                       Value = 12,
                       Name = "Host translator service for OEM cloud certificate"
                   },
                   new FileType
                   {
                       Id = 12,
                       Value = 13,
                       Name = "Driver trust store"
                   },
                   new FileType
                   {
                       Id = 13,
                       Value = 16,
                       Name = "802.1x TLS authentication"
                   },
                   new FileType
                   {
                       Id = 14,
                       Value = 18,
                       Name = "HTS OEM cloud authentication"
                   }
               );


            #endregion

         



            /*
             *
            * Below should be inside License Feature
            * */
            modelBuilder.Entity<SystemConfiguration>().HasData(
                new SystemConfiguration { Id = 1, nPorts = 1, nScp = 100, nChannelId = 1, cType = 7, cPort = 3333 }
                );

            modelBuilder.Entity<SystemSetting>().HasData(
                new SystemSetting { Id = 1, nMsp1Port = 3, nTransaction = 60000, nSio = 16, nMp = 615, nCp = 388, nAcr = 64, nAlvl = 32000, nTrgr = 1024, nProc = 1024, GmtOffset = -25200, nTz = 255, nHol = 255, nMpg = 128, nCard = 200,nArea= 127}
                );

            #region Component

            modelBuilder.Entity<Component>().HasData(
                new Component { Id = 1, ModelNo = 196, Name = "HID Aero X1100", nInput = 7, nOutput = 4, nReader = 4 },
                new Component { Id = 2, ModelNo = 193, Name = "HID Aero X100", nInput = 7, nOutput = 4, nReader = 4 },
                new Component { Id = 3, ModelNo = 194, Name = "HID Aero X200", nInput = 19, nOutput = 2, nReader = 0 },
                new Component { Id = 4, ModelNo = 195, Name = "HID Aero X300", nInput = 5, nOutput = 12, nReader = 0 },
                new Component { Id = 5, ModelNo = 190, Name = "VertX V100", nInput = 7, nOutput = 4, nReader = 2 },
                new Component { Id = 6, ModelNo = 191, Name = "VertX V200", nInput = 19, nOutput = 2, nReader = 0 },
                new Component { Id = 7, ModelNo = 192, Name = "VertX V300", nInput = 5, nOutput = 12, nReader = 0 }
             );

            #endregion



  




        }




    }
}