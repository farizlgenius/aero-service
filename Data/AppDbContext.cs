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

        public DbSet<ar_access_lv> ar_access_lvls { get; set; }
        public DbSet<ar_control_point> ar_control_point { get; set; }
        public DbSet<ar_card_holder> ar_card_holders { get; set; }
        public DbSet<ar_credentials> ar_credentials { get; set; }
        public DbSet<ar_acr> ar_acrs { get; set; }
        public DbSet<event_transction> ar_events { get; set; }
        public DbSet<ar_monitor_point> ar_monitor_point { get; set; }
        public DbSet<ar_reader> ar_readers { get; set; }
        public DbSet<ar_n_mp> ar_mp_no { get; set; }

        public DbSet<ar_n_sio> ar_sio_no { get; set; }
        public DbSet<ar_n_cp> ar_cp_no { get; set; }

        public DbSet<ar_scp> ar_scps { get; set; }
        public DbSet<ar_sio> ar_sios { get; set; }
        public DbSet<ar_tz> ar_tzs { get; set; }
        public DbSet<ar_card_format> ar_card_formats { get; set; }
        public DbSet<ar_n_tz> ar_tz_no { get; set; }
        public DbSet<ar_operator> ar_users { get; set; }

        public DbSet<ar_op_mode> ar_op_modes { get; set; }
        public DbSet<ar_n_acr> ar_acr_no { get; set; }
        public DbSet<ar_rdr_cfg_mode> ar_rdr_modes { get; set; }
        public DbSet<ar_strk_mode> ar_strk_modes { get; set; }
        public DbSet<ar_acr_mode> ar_acr_modes { get; set; }
        public DbSet<ar_apb_mode> ar_apb_modes { get; set; }

        public DbSet<ar_ip_mode> ar_ip_modes { get; set; }
        public DbSet<ar_tz_interval> ar_tz_intervals { get; set; }

        public DbSet<ar_nak> ar_naks { get; set; }

        public DbSet<ar_n_alvl> ar_alvl_no { get; set; }
        public DbSet<ar_scp_structure_status> ar_scp_structure_statuses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Apply configuration to all entities that inherit from BaseEntity
            foreach (var entityType in modelBuilder.Model.GetEntityTypes()
                         .Where(e => typeof(ar_base_entity).IsAssignableFrom(e.ClrType)))
            {
                // For CreatedAt
                modelBuilder.Entity(entityType.ClrType)
                    .Property(nameof(ar_base_entity.created_date))
                    .HasColumnType("timestamp without time zone");

                // For UpdatedAt
                modelBuilder.Entity(entityType.ClrType)
                    .Property(nameof(ar_base_entity.updated_date))
                    .HasColumnType("timestamp without time zone");
            }

            // Initial Database 

            modelBuilder.Entity<ar_op_mode>().HasData(
                new ar_op_mode { id=1,value = 0,description = "Normal Mode with Offline: No change" },
                new ar_op_mode { id=2,value = 1,description = "Inverted Mode Offline: No change" },
                new ar_op_mode { id=3,value = 16, description = "Normal Mode Offline: Inactive" },
                 new ar_op_mode { id=4,value = 17, description = "Inverted Mode Offline: Inactive" },
                 new ar_op_mode { id=5,value = 32, description = "Normal Mode Offline: Active" },
                 new ar_op_mode { id=6,value = 33, description = "Inverted Mode Offline: Inactive" }
                );

            modelBuilder.Entity<ar_rdr_cfg_mode>().HasData(
                new ar_rdr_cfg_mode {id=1,name="Single Reader",value=0,description= "Single reader, controlling the door" },
                new ar_rdr_cfg_mode { id = 2, name = "Paired readers, Master", value = 1, description = "Paired readers, Master - this reader controls the door" },
                new ar_rdr_cfg_mode { id=3,name = "Paired readers, Slave",value=2,description= "Paired readers, Slave - this reader does not control door" },
                new ar_rdr_cfg_mode { id=4,name= "Turnstile Reader",value=3,description= "Turnstile Reader. Two modes selected by: n strike_t_min != strike_t_max (original implementation - an access grant pulses the strike output for 1 second) n strike_t_min == strike_t_max (pulses the strike output after a door open/close signal for each additional access grant if several grants are waiting)" },
                new ar_rdr_cfg_mode { id=5,name= "Elevator, no floor",value=4,description= "Elevator, no floor select feedback" },
                new ar_rdr_cfg_mode { id = 6,name = "Elevator with floor",value = 5,description= "Elevator with floor select feedback" }
                );

            modelBuilder.Entity<ar_strk_mode>().HasData(
                new ar_strk_mode { id = 1,name="Normal",value=0,description="Do not use! This would allow the strike to stay active for the entire strike time allowing the door to be opened multiple times."},
                new ar_strk_mode { id= 2,name="Deactivate On Open",value=1,description= "Deactivate strike when door opens" },
                new ar_strk_mode { id =3,name="Deactivate On Close",value=2,description= "Deactivate strike on door close or strike_t_max expires" },
                new ar_strk_mode { id=4,name="Tailgate",value=16,description= "Used with ACR_S_OPEN or ACR_S_CLOSE, to select tailgate mode: pulse (strk_sio:strk_number+1) relay for each user expected to enter" }
                );

            modelBuilder.Entity<ar_acr_mode>().HasData(
                new ar_acr_mode { id = 1, name = "Disable", value = 1, description = "Disable the ACR, no REX" },
                new ar_acr_mode { id = 2, name = "Unlock", value = 2, description = "Unlock (unlimited access)" },
                new ar_acr_mode { id = 3, name = "Locked", value = 3, description = "Locked (no access, REX active)" },
                new ar_acr_mode { id = 4, name = "Facility code only", value = 4, description = "Facility code only" },
                new ar_acr_mode { id = 5, name = "Card only", value = 5, description = "Card only" },
                new ar_acr_mode { id = 6, name = "PIN only", value = 6, description = "PIN only" },
                new ar_acr_mode { id = 7, name = "Card and PIN", value = 7, description = "Card and PIN required" },
                new ar_acr_mode { id = 8, name = "Card or PIN", value = 8, description = "Card or PIN required" }
            );

            modelBuilder.Entity<ar_apb_mode>().HasData(
                new ar_apb_mode { id = 1, name = "None", value = 0, description = "Do not check or alter anti-passback location. No antipassback rules." },
                new ar_apb_mode { id = 2, name = "Soft anti-passback", value = 1, description = "Soft anti-passback: Accept any new location, change the user’s location to current reader, and generate an antipassback violation for an invalid entry." },
                new ar_apb_mode { id = 3, name = "Hard anti-passback", value = 2, description = "Hard anti-passback: Check user location, if a valid entry is made, change user’s location to new location. If an invalid entry is attempted, do not grant access." }

            );

            modelBuilder.Entity<ar_ip_mode>().HasData(
                new ar_ip_mode {id=1,name="Normally closed",value=0,description= "Normally closed, no End-Of-Line (EOL)" },
                new ar_ip_mode { id = 2, name = "Normally open", value = 1, description = "Normally open, no EOL" }
                //new ar_ip_mode { id = 3, name = "Standard EOL 1", value = 2, description = "Standard (ROM’ed) EOL: 1 kΩ normal, 2 kΩ active" },
                //new ar_ip_mode { id = 4, name = "Standard EOL 2", value = 3, description = "Standard (ROM’ed) EOL: 2 kΩ normal, 1 kΩ active" }
                );

            var access1 = new ar_access_lv { id = 1, name = "Full Access", access_lv_number = 0, };
            Utility.SetAllTz(access1, 1);
            var access2 = new ar_access_lv { id = 2, name = "No Access", access_lv_number = 1 };
            Utility.SetAllTz(access2, 0);
            modelBuilder.Entity<ar_access_lv>().HasData(
               access1,
               access2
               
            );

            modelBuilder.Entity<ar_n_alvl>().HasData(
                new ar_n_alvl { id = 1, alvl_number = 0, is_available = false },
                new ar_n_alvl { id = 2, alvl_number = 1, is_available = false }
                );


            modelBuilder.Entity<ar_tz>().HasData(
                new ar_tz { id=1,name="Always",tz_number=1,mode=1,act_time=0,deact_time=0,intervals=0 }
                );

            modelBuilder.Entity<ar_n_tz>().HasData(
                new ar_n_tz { id=1,tz_number=1,is_available=false}
                );

            modelBuilder.Entity<ar_card_format>().HasData(
                new ar_card_format { 
                    id=1,
                    number = 0,
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
                }
             );
        }




    }
}