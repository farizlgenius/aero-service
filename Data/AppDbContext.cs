using HID.Aero.ScpdNet.Wrapper;
using HIDAeroService.AeroLibrary;
using HIDAeroService.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Xml.Linq;

namespace HIDAeroService.Data
{

    public sealed class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<ArAccessLevel> ArAcccessLevels { get; set; }
        public DbSet<ArControlPoint> ArControlPoints { get; set; }
        public DbSet<ArCardHolder> ArCardHolders { get; set; }
        public DbSet<ArCredential> ArCredentials { get; set; }
        public DbSet<ArAcr> ArAcrs { get; set; }
        public DbSet<ArEvent> ArEvents { get; set; }
        public DbSet<ArMonitorPoint> ArMonitorPoints { get; set; }
        public DbSet<ArReader> ArReaders { get; set; }
        public DbSet<ArMpNo> ArMpNo { get; set; }

        public DbSet<ArSioNo> ArSioNo { get; set; }
        public DbSet<ArCpNo> ArCpNo { get; set; }

        public DbSet<ArScp> ArScps { get; set; }
        public DbSet<ArSio> ArSios { get; set; }
        public DbSet<ArTimeZone> ArTimeZones { get; set; }
        public DbSet<ArCardFormat> ArCardFormats { get; set; }
        public DbSet<ArOperator> ArOperators { get; set; }

        public DbSet<ArOpMode> ArOpModes { get; set; }
        public DbSet<ArAcrNo> ArAcrNo { get; set; }
        public DbSet<ArReaderConfigMode> ArReaderModes { get; set; }
        public DbSet<ArStrkMode> ArStrikeModes { get; set; }
        public DbSet<ArAcrMode> ArAcrModes { get; set; }
        public DbSet<ArApbMode> ArApbModes { get; set; }

        public DbSet<ArIpMode> ArIpModes { get; set; }
        public DbSet<ArInterval> ArIntervals { get; set; }

        public DbSet<ArCommandStatus> ArCommandStatuses { get; set; }

        public DbSet<ArScpStructureStatus> ArScpStructureStatuses { get; set; }

        public DbSet<ArSystemConfig> ArSystemConfigs { get; set; }

        public DbSet<ArScpSetting> ArScpSettings { get; set; }

        public DbSet<ArScpComponent> ArScpComponents { get; set; }

        public DbSet<ArHoliday> ArHolidays { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Apply configuration to all entities that inherit from BaseEntity
            foreach (var entityType in modelBuilder.Model.GetEntityTypes()
                         .Where(e => typeof(ArBaseEntity).IsAssignableFrom(e.ClrType)))
            {
                // For CreatedAt
                modelBuilder.Entity(entityType.ClrType)
                    .Property(nameof(ArBaseEntity.CreatedDate))
                    .HasColumnType("timestamp without time zone");

                // For UpdatedAt
                modelBuilder.Entity(entityType.ClrType)
                    .Property(nameof(ArBaseEntity.UpdatedDate))
                    .HasColumnType("timestamp without time zone");
            }

            modelBuilder.Entity<ArScp>().Property(nameof(ArScp.LastSync)).HasColumnType("timestamp without time zone");

            // Initial Database 

            modelBuilder.Entity<ArOpMode>().HasData(
                new ArOpMode { Id=1,Value = 0,Description = "Normal Mode with Offline: No change",CreatedDate=SeedDefaults.SeedDate,UpdatedDate=SeedDefaults.SeedDate },
                new ArOpMode { Id=2, Value = 1, Description = "Inverted Mode Offline: No change", CreatedDate = SeedDefaults.SeedDate, UpdatedDate = SeedDefaults.SeedDate },
                new ArOpMode { Id=3, Value = 16, Description = "Normal Mode Offline: Inactive", CreatedDate = SeedDefaults.SeedDate, UpdatedDate = SeedDefaults.SeedDate },
                 new ArOpMode { Id=4, Value = 17, Description = "Inverted Mode Offline: Inactive", CreatedDate = SeedDefaults.SeedDate, UpdatedDate = SeedDefaults.SeedDate },
                 new ArOpMode { Id=5, Value = 32, Description = "Normal Mode Offline: Active", CreatedDate = SeedDefaults.SeedDate, UpdatedDate = SeedDefaults.SeedDate },
                 new ArOpMode { Id=6, Value = 33, Description = "Inverted Mode Offline: Inactive", CreatedDate = SeedDefaults.SeedDate, UpdatedDate = SeedDefaults.SeedDate }
                );

            modelBuilder.Entity<ArReaderConfigMode>().HasData(
                new ArReaderConfigMode {Id=1,Name="Single Reader", Value = 0,Description= "Single reader, controlling the door", CreatedDate = SeedDefaults.SeedDate, UpdatedDate = SeedDefaults.SeedDate },
                new ArReaderConfigMode { Id = 2, Name = "Paired readers, Master", Value = 1, Description = "Paired readers, Master - this reader controls the door", CreatedDate = SeedDefaults.SeedDate, UpdatedDate = SeedDefaults.SeedDate },
                new ArReaderConfigMode { Id=3,Name = "Paired readers, Slave", Value = 2,Description= "Paired readers, Slave - this reader does not control door", CreatedDate = SeedDefaults.SeedDate, UpdatedDate = SeedDefaults.SeedDate },
                new ArReaderConfigMode { Id=4,Name= "Turnstile Reader", Value = 3,Description= "Turnstile Reader. Two modes selected by: n strike_t_min != strike_t_max (original implementation - an access grant pulses the strike output for 1 second) n strike_t_min == strike_t_max (pulses the strike output after a door open/close signal for each additional access grant if several grants are waiting)", CreatedDate = SeedDefaults.SeedDate, UpdatedDate = SeedDefaults.SeedDate },
                new ArReaderConfigMode { Id=5,Name= "Elevator, no floor", Value = 4,Description= "Elevator, no floor select feedback", CreatedDate = SeedDefaults.SeedDate, UpdatedDate = SeedDefaults.SeedDate },
                new ArReaderConfigMode { Id = 6,Name = "Elevator with floor", Value = 5,Description= "Elevator with floor select feedback", CreatedDate = SeedDefaults.SeedDate, UpdatedDate = SeedDefaults.SeedDate }
                );

            modelBuilder.Entity<ArStrkMode>().HasData(
                new ArStrkMode { Id = 1,Name="Normal",Value=0,Description="Do not use! This would allow the strike to stay active for the entire strike time allowing the door to be opened multiple times.", CreatedDate = SeedDefaults.SeedDate, UpdatedDate = SeedDefaults.SeedDate },
                new ArStrkMode { Id= 2,Name="Deactivate On Open",Value=1,Description= "Deactivate strike when door opens", CreatedDate = SeedDefaults.SeedDate, UpdatedDate = SeedDefaults.SeedDate },
                new ArStrkMode { Id =3,Name="Deactivate On Close",Value=2,Description= "Deactivate strike on door close or strike_t_max expires", CreatedDate = SeedDefaults.SeedDate, UpdatedDate = SeedDefaults.SeedDate },
                new ArStrkMode { Id=4,Name="Tailgate",Value=16,Description= "Used with ACR_S_OPEN or ACR_S_CLOSE, to select tailgate mode: pulse (strk_sio:strk_number+1) relay for each user expected to enter", CreatedDate = SeedDefaults.SeedDate, UpdatedDate = SeedDefaults.SeedDate }
                );

            modelBuilder.Entity<ArAcrMode>().HasData(
                new ArAcrMode { Id = 1, Name = "Disable", Value = 1, Description = "Disable the ACR, no REX", CreatedDate = SeedDefaults.SeedDate, UpdatedDate = SeedDefaults.SeedDate },
                new ArAcrMode { Id = 2, Name = "Unlock", Value = 2, Description = "Unlock (unlimited access)", CreatedDate = SeedDefaults.SeedDate, UpdatedDate = SeedDefaults.SeedDate },
                new ArAcrMode { Id = 3, Name = "Locked", Value = 3, Description = "Locked (no access, REX active)", CreatedDate = SeedDefaults.SeedDate, UpdatedDate = SeedDefaults.SeedDate },
                new ArAcrMode { Id = 4, Name = "Facility code only", Value = 4, Description = "Facility code only", CreatedDate = SeedDefaults.SeedDate, UpdatedDate = SeedDefaults.SeedDate },
                new ArAcrMode { Id = 5, Name = "Card only", Value = 5, Description = "Card only", CreatedDate = SeedDefaults.SeedDate, UpdatedDate = SeedDefaults.SeedDate },
                new ArAcrMode { Id = 6, Name = "PIN only", Value = 6, Description = "PIN only", CreatedDate = SeedDefaults.SeedDate, UpdatedDate = SeedDefaults.SeedDate },
                new ArAcrMode { Id = 7, Name = "Card and PIN", Value = 7, Description = "Card and PIN required", CreatedDate = SeedDefaults.SeedDate, UpdatedDate = SeedDefaults.SeedDate },
                new ArAcrMode { Id = 8, Name = "Card or PIN", Value = 8, Description = "Card or PIN required", CreatedDate = SeedDefaults.SeedDate, UpdatedDate = SeedDefaults.SeedDate }
            );

            modelBuilder.Entity<ArApbMode>().HasData(
                new ArApbMode { Id = 1, Name = "None", Value = 0, Description = "Do not check or alter anti-passback location. No antipassback rules.", CreatedDate = SeedDefaults.SeedDate, UpdatedDate = SeedDefaults.SeedDate },
                new ArApbMode { Id = 2, Name = "Soft anti-passback", Value = 1, Description = "Soft anti-passback: Accept any new location, change the user’s location to current reader, and generate an antipassback violation for an invalId entry.", CreatedDate = SeedDefaults.SeedDate, UpdatedDate = SeedDefaults.SeedDate },
                new ArApbMode { Id = 3, Name = "Hard anti-passback", Value = 2, Description = "Hard anti-passback: Check user location, if a valid entry is made, change user’s location to new location. If an invalid entry is attempted, do not grant access.", CreatedDate = SeedDefaults.SeedDate, UpdatedDate = SeedDefaults.SeedDate }

            );

            modelBuilder.Entity<ArIpMode>().HasData(
                new ArIpMode {Id=1,Name="Normally closed",Value=0,Description= "Normally closed, no End-Of-Line (EOL)", CreatedDate = SeedDefaults.SeedDate, UpdatedDate = SeedDefaults.SeedDate },
                new ArIpMode { Id = 2, Name = "Normally open", Value = 1, Description = "Normally open, no EOL", CreatedDate = SeedDefaults.SeedDate, UpdatedDate = SeedDefaults.SeedDate }
                //new ar_ip_mode { id = 3, name = "Standard EOL 1", Value = 2, Description = "Standard (ROM’ed) EOL: 1 kΩ normal, 2 kΩ active" },
                //new ar_ip_mode { id = 4, name = "Standard EOL 2", Value = 3, Description = "Standard (ROM’ed) EOL: 2 kΩ normal, 1 kΩ active" }
                );

            var access1 = new ArAccessLevel { Id = 2, Name = "No Access", ComponentNo = 0, CreatedDate = SeedDefaults.SeedDate, UpdatedDate = SeedDefaults.SeedDate, IsActive = true };
            Utility.SetAllTz(access1, 0);
            var access2 = new ArAccessLevel { Id = 1, Name = "Full Access", ComponentNo = 1, CreatedDate = SeedDefaults.SeedDate, UpdatedDate = SeedDefaults.SeedDate, IsActive = true };
            Utility.SetAllTz(access2, 1);

            modelBuilder.Entity<ArAccessLevel>().HasData(
               access1,
               access2
               
            );



            modelBuilder.Entity<ArTimeZone>().HasData(
                new ArTimeZone { Id = 1,Name="Always",ComponentNo=1,Mode=1,ActiveTime="",DeactiveTime="",Intervals=0,IntervalsNoList="",IsActive=true,CreatedDate=SeedDefaults.SeedDate,UpdatedDate=SeedDefaults.SeedDate }
                );


            modelBuilder.Entity<ArCardFormat>().HasData(
                new ArCardFormat {
                    Id = 1,
                    CommandId = 0,
                    CardFormatName = "26 Bits No Fac",
                    ComponentNo = 0,
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
                    IsActive = true,
                    CreatedDate = SeedDefaults.SeedDate,
                    UpdatedDate = SeedDefaults.SeedDate,
                }
             );

            modelBuilder.Entity<ArSystemConfig>().HasData(
                new ArSystemConfig { Id = 1,NPorts=1,NScp=100,NChannelId=1,CType=7,CPort=3333,CreatedDate=SeedDefaults.SeedDate,UpdatedDate=SeedDefaults.SeedDate}
                );

            modelBuilder.Entity<ArScpSetting>().HasData(
                new ArScpSetting { Id=1,NMsp1Port=3,NTransaction=60000,NSio=16,NMp=615,NCp=388,NAcr=64,NAlvl=32000,NTrgr=1024,NProc=1024,GmtOffset=-25200,NTz=255,NHol=255,NMpg=128,NCard=200,CreatedDate=SeedDefaults.SeedDate,UpdatedDate=SeedDefaults.SeedDate}
                );

            modelBuilder.Entity<ArScpComponent>().HasData(
                new ArScpComponent { Id=1,ModelNo=196,Name="HID Aero X1100",NInput=7,NOutput=4,NReader=4,CreatedDate=SeedDefaults.SeedDate,UpdatedDate=SeedDefaults.SeedDate},
    new ArScpComponent { Id = 2, ModelNo = 193, Name = "HID Aero X100", NInput = 7, NOutput = 4, NReader = 4 , CreatedDate = SeedDefaults.SeedDate, UpdatedDate = SeedDefaults.SeedDate },
    new ArScpComponent { Id = 3, ModelNo = 194, Name = "HID Aero X200", NInput = 19, NOutput = 2, NReader = 0 , CreatedDate = SeedDefaults.SeedDate, UpdatedDate = SeedDefaults.SeedDate },
    new ArScpComponent { Id = 4, ModelNo = 195, Name = "HID Aero X300", NInput = 5, NOutput = 12, NReader = 0 , CreatedDate = SeedDefaults.SeedDate, UpdatedDate = SeedDefaults.SeedDate },
    new ArScpComponent { Id = 5, ModelNo = 190, Name = "VertX V100", NInput = 7, NOutput = 4, NReader = 2 , CreatedDate = SeedDefaults.SeedDate, UpdatedDate = SeedDefaults.SeedDate },
    new ArScpComponent { Id = 6, ModelNo = 191, Name = "VertX V200", NInput = 19, NOutput = 2, NReader = 0 , CreatedDate = SeedDefaults.SeedDate, UpdatedDate = SeedDefaults.SeedDate },
    new ArScpComponent { Id = 7, ModelNo = 192, Name = "VertX V300", NInput = 5, NOutput = 12, NReader = 0, CreatedDate = SeedDefaults.SeedDate, UpdatedDate = SeedDefaults.SeedDate }
                );
        }




    }
}