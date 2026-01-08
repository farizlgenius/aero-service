using HIDAeroService.Entity.Interface;
using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Entity
{
    public class AccessLevelDoorTimeZone 
    {
        [Key]
        public int id { get; set; }
        public short accesslevel_id { get; set; }
        public AccessLevel accesslevel { get; set; }
        public short timezone_id { get; set; }
        public TimeZone timezone { get; set; }
        public short door_id { get; set; }
        public Door door { get; set; }


      
    }
}
