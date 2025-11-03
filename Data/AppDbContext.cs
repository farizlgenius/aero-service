using HID.Aero.ScpdNet.Wrapper;
using HIDAeroService.AeroLibrary;
using HIDAeroService.Controllers.V1;
using HIDAeroService.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
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
        public DbSet<AccessLevelDoorTimeZone> DoorTimeZones { get; set; }
        public DbSet<CardHolder> CardHolders { get; set; }
        public DbSet<Credential> Credentials { get; set; }
        public DbSet<AccessArea> AccessAreas { get; set; }
        public DbSet<AccessLevelCredential> AccessLevelCredentials { get; set; }
        public DbSet<TimeZoneInterval> TimeZoneIntervals { get; set; }
        public DbSet<ReaderOutConfiguration> ReaderOutConfigurations { get; set; }
        public DbSet<MonitorPointMode> MonitorPointModes { get; set; }
        public DbSet<OsdpBaudrate> OsdpBaudrates { get; set; }
        public DbSet<OsdpAddress> OsdpAddresses { get; set; }
        public DbSet<Operator> Operators { get; set; }
        public DbSet<DoorSpareFlag> DoorSpareFlags { get; set; }
        public DbSet<DoorAccessControlFlag> DoorAccessControlFlags { get; set; }

        // Old 

        public DbSet<ArEvent> ArEvents { get; set; }

        public DbSet<ArCommandStatus> ArCommandStatuses { get; set; }

        public DbSet<ArScpStructureStatus> ArScpStructureStatuses { get; set; }





        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            // Apply configuration to all entities that inherit from BaseEntity
            foreach (var entityType in modelBuilder.Model.GetEntityTypes()
                         .Where(e => typeof(BaseEntity).IsAssignableFrom(e.ClrType)))
            {
                // For CreatedAt
                modelBuilder.Entity(entityType.ClrType)
                    .Property(nameof(BaseEntity.CreatedDate))
                    .HasColumnType("timestamp without time zone")
                    .HasDefaultValueSql("now()")
                    .ValueGeneratedOnAdd();

                // For UpdatedAt
                modelBuilder.Entity(entityType.ClrType)
                    .Property(nameof(BaseEntity.UpdatedDate))
                    .HasColumnType("timestamp without time zone")
                    .HasDefaultValueSql("now()")
                    .ValueGeneratedOnAdd();
            }

            // Apply configuration to all entities that inherit from BaseEntity
            foreach (var entityType in modelBuilder.Model.GetEntityTypes()
                         .Where(e => typeof(NoMacBaseEntity).IsAssignableFrom(e.ClrType)))
            {
                // For CreatedAt
                modelBuilder.Entity(entityType.ClrType)
                    .Property(nameof(NoMacBaseEntity.CreatedDate))
                    .HasColumnType("timestamp without time zone")
                    .HasDefaultValueSql("now()")
                     .ValueGeneratedOnAdd();

                // For UpdatedAt
                modelBuilder.Entity(entityType.ClrType)
                    .Property(nameof(NoMacBaseEntity.UpdatedDate))
                    .HasColumnType("timestamp without time zone")
                    .HasDefaultValueSql("now()")
                    .ValueGeneratedOnAdd();
            }

            modelBuilder.Entity<Hardware>().Property(nameof(Hardware.LastSync)).HasColumnType("timestamp without time zone");

            // Initial Database 

            modelBuilder.Entity<OutputMode>().HasData(
                new OutputMode { Id = 1, RelayMode = 0, OfflineMode = 0, Value = 0, Description = "Normal Mode with Offline: No change"},
                new OutputMode { Id = 2, RelayMode = 1, OfflineMode = 0, Value = 1, Description = "Inverted Mode Offline: No change" },
                new OutputMode { Id = 3, RelayMode = 0, OfflineMode = 1, Value = 16, Description = "Normal Mode Offline: Inactive" },
                 new OutputMode { Id = 4, RelayMode = 1, OfflineMode = 1, Value = 17, Description = "Inverted Mode Offline: Inactive" },
                 new OutputMode { Id = 5, RelayMode = 0, OfflineMode = 2, Value = 32, Description = "Normal Mode Offline: Active" },
                 new OutputMode { Id = 6, RelayMode = 1, OfflineMode = 2, Value = 33, Description = "Inverted Mode Offline: Active" }
                );

            modelBuilder.Entity<OutputOfflineMode>().HasData(
                new OutputOfflineMode { Id = 1, Value = 0, Name = "No Change",Description="No Change"  },
                 new OutputOfflineMode { Id = 2, Value = 1, Name = "Inactive",Description= "Relay de-energized" },
                  new OutputOfflineMode { Id = 3, Value = 2, Name = "Active",Description = "Relay energized" }
                );

            modelBuilder.Entity<RelayMode>().HasData(
                new RelayMode { Id = 1, Value = 0, Name = "Normal",Description="Active is energized" },
                new RelayMode { Id = 2, Value = 1, Name = "Inverted",Description="Active is de-energized" }
                );

            modelBuilder.Entity<ReaderConfigurationMode>().HasData(
                new ReaderConfigurationMode { Id = 1, Name = "Single Reader", Value = 0, Description = "Single reader, controlling the door" },
                new ReaderConfigurationMode { Id = 2, Name = "Paired readers, Master", Value = 1, Description = "Paired readers, Master - this reader controls the door" },
                new ReaderConfigurationMode { Id = 3, Name = "Paired readers, Slave", Value = 2, Description = "Paired readers, Slave - this reader does not control door" },
                new ReaderConfigurationMode { Id = 4, Name = "Turnstile Reader", Value = 3, Description = "Turnstile Reader. Two modes selected by: n strike_t_min != strike_t_max (original implementation - an access grant pulses the strike output for 1 second) n strike_t_min == strike_t_max (pulses the strike output after a door open/close signal for each additional access grant if several grants are waiting)" },
                new ReaderConfigurationMode { Id = 5, Name = "Elevator, no floor", Value = 4, Description = "Elevator, no floor select feedback" },
                new ReaderConfigurationMode { Id = 6, Name = "Elevator with floor", Value = 5, Description = "Elevator with floor select feedback"}
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
                new DoorMode { Id = 7, Name = "Card and PIN", Value = 7, Description = "Card and PIN required"},
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

            modelBuilder.Entity<InputMode>().HasData(
                new InputMode { Id = 1, Name = "Normally closed", Value = 0, Description = "Normally closed, no End-Of-Line (EOL)" },
                new InputMode { Id = 2, Name = "Normally open", Value = 1, Description = "Normally open, no EOL" },
                new InputMode { Id = 3, Name = "Standard EOL 1", Value = 2, Description = "Standard (ROM’ed) EOL: 1 kΩ normal, 2 kΩ active" },
                new InputMode { Id = 4, Name = "Standard EOL 2", Value = 3, Description = "Standard (ROM’ed) EOL: 2 kΩ normal, 1 kΩ active" }
                );

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

            modelBuilder.Entity<AccessLevel>()
                .HasMany(s => s.AccessLevelDoorTimeZones)
                .WithOne(s => s.AccessLevel)
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

            var NoAccess = new AccessLevel { Id = 1, Uuid = SeedDefaults.SystemGuid, Name = "No Access", ComponentId = 1, IsActive = true };

            var FullAccess = new AccessLevel { Id = 2, Uuid = SeedDefaults.SystemGuid, Name = "Full Access", ComponentId = 2, IsActive = true };


            modelBuilder.Entity<AccessLevel>().HasData(
               NoAccess,
               FullAccess
            );

            modelBuilder.Entity<AccessLevelCredential>()
                .HasKey(e => new { e.CredentialId, e.AccessLevelId });

            modelBuilder.Entity<AccessLevelCredential>()
                .HasOne(e => e.AccessLevel)
                .WithMany(s => s.AccessLevelCredentials)
                .HasForeignKey(e => e.AccessLevelId)
                .HasPrincipalKey(e => e.ComponentId);

            modelBuilder.Entity<AccessLevelCredential>()
                .HasOne(e => e.Credential)
                .WithMany(s => s.AccessLevelCredentials)
                .HasForeignKey(e => e.CredentialId)
                .HasPrincipalKey(e => e.ComponentId);


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

            modelBuilder.Entity<DoorSpareFlag>().HasData(
                new DoorSpareFlag { Id = 1,Name="No Extend",Value=0x0001,Description= "ACR_FE_NOEXTEND\t0x0001\t\r\n🔹 Purpose: Prevents the “Extended Door Held Open Timer” from being restarted when a new access is granted.\r\n🔹 Effect: If someone presents a valid credential while the door is already open, the extended hold timer won’t reset — the timer continues to count down.\r\n🔹 Use Case: High-traffic doors where you don’t want repeated badge reads to keep the door open indefinitely." },
                new DoorSpareFlag { Id = 2, Name="Card Before Pin" ,Value=0x0002,Description= "ACR_FE_NOPINCARD\t0x0002\t\r\n🔹 Purpose: Forces CARD-before-PIN entry sequence in “Card and PIN” mode.\r\n🔹 Effect: PIN entered before a card will be rejected.\r\n🔹 Use Case: Ensures consistent user behavior and security (e.g., requiring card tap first, then PIN entry)." },
                new DoorSpareFlag { Id= 3,Name="Door Force Filter",Value=0x0008,Description= "ACR_FE_DFO_FLTR\t0x0008\t\r\n🔹 Purpose: Enables Door Forced Open Filter.\r\n🔹 Effect: If the door opens within 3 seconds after it was last closed, the system will not treat it as a “Door Forced Open” alarm.\r\n🔹 Use Case: Prevents nuisance alarms caused by door bounce, air pressure, or slow latch operation." },
                new DoorSpareFlag { Id = 4,Name="Blocked All Request",Value=0x0010,Description= "ACR_FE_NO_ARQ\t0x0010\t\r\n🔹 Purpose: Blocks all access requests.\r\n🔹 Effect: Every access attempt is automatically reported as “Access Denied – Door Locked.”\r\n🔹 Use Case: Temporarily disables access (e.g., during lockdown, maintenance, or controlled shutdown)." },
                new DoorSpareFlag {  Id=5,Name="Shunt Relay",Value=0x0020,Description= "ACR_FE_SHNTRLY\t0x0020\t\r\n🔹 Purpose: Defines a Shunt Relay used for suppressing door alarms during unlock events.\r\n🔹 Effect: When the door is unlocked:\r\n • The shunt relay activates 5 ms before the strike relay.\r\n • It deactivates 1 second after the door closes or the held-open timer expires.\r\n🔹 Note: The dc_held field (door-held timer) must be > 1 for this to function.\r\n🔹 Use Case: Used when connecting to alarm panels or to bypass door contacts during unlocks." },
                new DoorSpareFlag { Id=6,Name="Floor Pin",Value=0x0040,Description= "ACR_FE_FLOOR_PIN\t0x0040\t\r\n🔹 Purpose: Enables Floor Selection via PIN for elevators in “Card + PIN” mode.\r\n🔹 Effect: Instead of entering a PIN code, users enter the floor number after presenting a card.\r\n🔹 Use Case: Simplifies elevator access when using a single reader for multiple floors." },
                new DoorSpareFlag {  Id=7,Name="Link Mode",Value=0x0080,Description= "ACR_FE_LINK_MODE\t0x0080\t\r\n🔹 Purpose: Indicates that the reader is in linking mode (pairing with another device or reader).\r\n🔹 Effect: Set when acr_mode = 29 (start linking) and cleared when:\r\n • The reader is successfully linked, or\r\n • acr_mode = 30 (abort) or timeout occurs.\r\n🔹 Use Case: Used for configuring dual-reader systems (e.g., in/out readers or linked elevator panels)." },
                new DoorSpareFlag {  Id=8,Name="Double Card Event",Value=0x0100,Description= "ACR_FE_DCARD\t0x0100\t\r\n🔹 Purpose: Enables Double Card Mode.\r\n🔹 Effect: If the same valid card is presented twice within 5 seconds, it generates a double card event.\r\n🔹 Use Case: Used for dual-authentication or special functions (e.g., manager override, arming/disarming security zones)." },
                new DoorSpareFlag { Id=9,Name="Allow Mode Override",Value=0x0200,Description= "ACR_FE_OVERRIDE\t0x0200\t\r\n🔹 Purpose: Indicates that the reader is operating in a Temporary ACR Mode Override.\r\n🔹 Effect: Typically means that a temporary mode (e.g., unlocked, lockdown) has been forced manually or by schedule.\r\n🔹 Use Case: Allows temporary override of normal access control logic without changing the base configuration." },
                new DoorSpareFlag { Id=10,Name="Allow Super Card",Value=0x0400,Description= "ACR_FE_CRD_OVR_EN\t0x0400\t\r\n🔹 Purpose: Enables Override Credentials.\r\n🔹 Effect: Specific credentials (set in FFRM_FLD_ACCESSFLGS) can unlock the door even when it’s locked or access is disabled.\r\n🔹 Use Case: For emergency or master access cards (security, admin, fire personnel)." },
                new DoorSpareFlag {  Id=11,Name="Disable Elevator",Value=0x0800,Description= "ACR_FE_ELV_DISABLE\t0x0800\t\r\n🔹 Purpose: Enables the ability to disable elevator floors using the offline_mode field.\r\n🔹 Effect: Only applies to Elevator Type 1 and 2 ACRs.\r\n🔹 Use Case: Temporarily disables access to certain floors when the elevator or reader is in offline or restricted mode." },
                new DoorSpareFlag { Id=12,Name="Alternate Reader Link",Value=0x1000,Description= "ACR_FE_LINK_MODE_ALT\t0x1000\t\r\n🔹 Purpose: Similar to ACR_FE_LINK_MODE but for Alternate Reader Linking.\r\n🔹 Effect: Set when acr_mode = 32 (start link) and cleared when:\r\n  • Link successful, or\r\n  • acr_mode = 33 (abort) or timeout reached.\r\n🔹 Use Case: Used for alternate or backup reader pairing configurations." },
                new DoorSpareFlag {  Id=13,Name="HOLD REX",Value=0x2000,Description= "🔹 Purpose: Extends the REX (Request-to-Exit) grant time while REX input is active.\r\n🔹 Effect: As long as the REX signal remains active (button pressed or motion detected), the door remains unlocked.\r\n🔹 Use Case: Ideal for long exit paths, large doors, or slow-moving personnel." },
                new DoorSpareFlag { Id=14,Name="HOST Decision",Value=0x4000,Description= "ACR_FE_HOST_BYPASS\t0x4000\t\r\n🔹 Purpose: Enables host decision bypass for online authorization.\r\n🔹 Effect: Requires ACR_F_HOST_CBG to also be enabled.\r\n 1. Controller sends credential to host for decision.\r\n 2. If host replies in time → uses host’s decision.\r\n 3. If no reply (timeout): controller checks its local database.\r\n  • If credential valid → grant.\r\n  • If not → deny.\r\n🔹 Use Case: For real-time validation in networked systems but with local fallback during communication loss.\r\n🔹 Supports: Card + PIN readers, online decision making, hybrid access control." }
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


            #endregion

            #region User & Credentials

            modelBuilder.Entity<CardHolder>()
                .HasMany(e => e.Credentials)
                .WithOne(e => e.CardHolder)
                .HasForeignKey(e => e.CardHolderId)
                .HasPrincipalKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CredentialFlag>()
                .HasData(
                new CredentialFlag {Id=1,Value=0x01,Name="Active Credential Record",Description= "Active Credential Record" },
                new CredentialFlag { Id=2,Value=0x02,Name="Free One Antipassback",Description="Allow one free anti-passback pass"},
                new CredentialFlag { Id=3,Value=0x04,Name="Anti-passback exempt"},
                new CredentialFlag { Id=4,Value=0x08,Name="Timing for disbled (ADA)",Description= "Use timing parameters for the disabled (ADA)" },
                new CredentialFlag { Id=5,Value=0x10,Name="Pin Exempt",Description= "PIN Exempt for \"Card & PIN\" ACR mode" }
                );


            #endregion

            #region CardFormat

            modelBuilder.Entity<CardFormat>().HasData(
                new CardFormat
                {
                    Id=1,
                    Uuid = SeedDefaults.SystemGuid,
                    LocationId=1,
                    LocationName="Main Location",
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






            modelBuilder.Entity<SystemConfiguration>().HasData(
                new SystemConfiguration { Id = 1, nPorts = 1, nScp = 100, nChannelId = 1, cType = 7, cPort = 3333 }
                );

            modelBuilder.Entity<SystemSetting>().HasData(
                new SystemSetting { Id = 1, nMsp1Port = 3, nTransaction = 60000, nSio = 16, nMp = 615, nCp = 388, nAcr = 64, nAlvl = 32000, nTrgr = 1024, nProc = 1024, GmtOffset = -25200, nTz = 255, nHol = 255, nMpg = 128, nCard = 200 }
                );

            modelBuilder.Entity<Component>().HasData(
                new Component { Id = 1, ModelNo = 196, Name = "HID Aero X1100", nInput = 7, nOutput = 4, nReader = 4 },
                new Component { Id = 2, ModelNo = 193, Name = "HID Aero X100", nInput = 7, nOutput = 4, nReader = 4 },
                new Component { Id = 3, ModelNo = 194, Name = "HID Aero X200", nInput = 19, nOutput = 2, nReader = 0 },
                new Component { Id = 4, ModelNo = 195, Name = "HID Aero X300", nInput = 5, nOutput = 12, nReader = 0 },
                new Component { Id = 5, ModelNo = 190, Name = "VertX V100", nInput = 7, nOutput = 4, nReader = 2 },
                new Component { Id = 6, ModelNo = 191, Name = "VertX V200", nInput = 19, nOutput = 2, nReader = 0 },
                new Component { Id = 7, ModelNo = 192, Name = "VertX V300", nInput = 5, nOutput = 12, nReader = 0}
             );


            modelBuilder.Entity<TimeZoneMode>().HasData(
                new TimeZoneMode { Id = 1, Value = 0, Name = "Off", Description = "The time zone is always inactive, regardless of the time zone intervals specified or the holidays in effect." },
                new TimeZoneMode { Id = 2,  Value = 1, Name = "On", Description = "The time zone is always active, regardless of the time zone intervals specified or the holidays in effect." },
                new TimeZoneMode { Id = 3, Value = 2, Name = "Scan", Description = "The Time Zone state is decided using either the Day MaskAsync or the Holiday MaskAsync. If the current day is specified as a Holiday, the state relies only on whether the Holiday MaskAsync Flag for that Holiday is set (if today is Holiday 1, and the Holiday MaskAsync sets flag H1, then the state is active. If today is Holiday 1, and the Holiday MaskAsync does not have flag H1 set, then the state is inactive). Holidays override the standard accessibility rules. If the current day is not specified as a Holiday, the Time Zone is active or inactive depending on whether the current day/time falls within the Day MaskAsync. If Day MaskAsync is M-F, 8-5, the Time Zone is active during those times, and inactive on the weekend and outside working hours." },
                new TimeZoneMode { Id = 4,  Value = 3, Name = "OneTimeEvent", Description = "Scan time zone interval list and apply only if the date string in expTest matches the current date" },
                new TimeZoneMode { Id = 5,  Value = 4, Name = "Scan, Always Honor Day of Week", Description = "This mode is similar to mode Scan Mode, but instead of only checking the Holiday MaskAsync if it is a Holiday, and only checking the Day MaskAsync if not, this mode checks both. If it is not a Holiday, this mode functions normally, only checking the Day MaskAsync. If it is a Holiday, this mode performs a logical OR on the Holiday and Day Masks. If either or both are active, the Time Zone is active, otherwise if neither is active, the Time Zone is inactive." },
                new TimeZoneMode { Id = 6, Value = 5, Name = "Scan, Always Holiday and Day of Week", Description = "This mode is similar to mode \"Scan, Always Honor Day of Week\", but it performs a logical AND instead of a logical OR. If it is not a Holiday, this mode functions normally, only checking the Day MaskAsync. If it is a Holiday, this mode is only active if BOTH the Day MaskAsync and Holiday MaskAsync are active. If either one is inactive, the entire Time Zone is inactive." }
             );


            modelBuilder.Entity<OsdpBaudrate>().HasData(
                new OsdpBaudrate { Id=1,Value=0x01,Name="9600",Description="" },
                new OsdpBaudrate { Id=2,Value=0x02, Name = "19200",Description=""},
                new OsdpBaudrate { Id=3,Value=0x03, Name = "38400",Description=""},
                new OsdpBaudrate {  Id =4,Value=0x04, Name = "115200",Description=""},
                new OsdpBaudrate {  Id =5,Value=0x05, Name = "57600",Description=""},
                new OsdpBaudrate { Id=6,Value=0x06, Name = "230400",Description=""}
                );

            modelBuilder.Entity<OsdpAddress>().HasData(
                new OsdpAddress { Id=1,Value=0x00,Name="Address 0",Description="" },
                new OsdpAddress { Id=2,Value=0x20,Name="Address 1",Description=""},
                new OsdpAddress { Id=3,Value=0x40,Name="Address 2",Description=""},
                new OsdpAddress { Id=4,Value=0x60,Name="Address 3",Description=""}
                );


            modelBuilder.Entity<ReaderOutConfiguration>().HasData(
                new ReaderOutConfiguration { Id=1,Name= "Ignore",Value=0,Description= "Ignore data from alternate reader" },
                new ReaderOutConfiguration { Id=2,Name= "Normal",Value=1,Description= "Normal Access Reader (two read heads to the same processor)" }

                );

            modelBuilder.Entity<MonitorPointMode>().HasData(
                new MonitorPointMode { Id = 1, Name = "Normal mode (no exit or entry delay)", Value = 0, Description = "" },
                new MonitorPointMode { Id = 2, Name = "Non-latching mode", Value = 1, Description = "" },
                new MonitorPointMode { Id = 3, Name = "Latching mode", Value = 2, Description = "" }
            );


        }




    }
}