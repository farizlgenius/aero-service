using System;
using System.ComponentModel.DataAnnotations;

namespace Aero.Infrastructure.Data.Entities;

    public class AccessLevelDoorComponent
    {
        [Key]
        public int id {get; set;}
    public short door_id { get; set; }
        public short acr_id {get; set;}
        public Door door { get; set; }
        public short timezone_id {get; set;}
        public TimeZone timezone { get; set; }

        // Reference 

        public int access_level_component_id {get; set;}
        public AccessLevelComponent access_level_component {get; set;}
    }
