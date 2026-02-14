using System;
using System.ComponentModel.DataAnnotations;

namespace Aero.Infrastructure.Data.Entities;

    public class AccessLevelComponent
    {
        [Key]
        public int id {get; set;}
        public short alvl_id { get; set; }
        public string mac { get; set;} = string.Empty;
        public Hardware hardware { get; set; }
        public short door_id { get; set; }
        public short acr_id { get; set; }
        public Door door { get; set; }
        public short timezone_id { get; set; }
        public TimeZone timezone { get; set; }

        // Reference 
        public short access_level_id {get; set;}
        public AccessLevel access_level {get; set;}
    }
