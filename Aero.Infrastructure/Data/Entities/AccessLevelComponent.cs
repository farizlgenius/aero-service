using System;
using System.ComponentModel.DataAnnotations;

namespace Aero.Infrastructure.Data.Entities;

    public class AccessLevelComponent
    {
        [Key]
        public int id {get; set;}
        public string mac { get; set;} = string.Empty;
        public Hardware hardware { get; set; }
        public ICollection<AccessLevelDoorComponent> door_component { get; set;}

        // Reference 
        public short access_level_id {get; set;}
        public AccessLevel access_level {get; set;}
    }
