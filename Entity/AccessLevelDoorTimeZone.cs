using HIDAeroService.Entity.Interface;
using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Entity
{
    public class AccessLevelDoorTimeZone 
    {
        [Key]
        public int Id { get; set; }
        public short AccessLevelId { get; set; }
        public AccessLevel AccessLevel { get; set; }
        public short TimeZoneId { get; set; }
        public TimeZone TimeZone { get; set; }
        public short DoorId { get; set; }
        public Door Door { get; set; }


        //public AccessLevel AccessLevel { get; set; }
        //public short TzAcr1 { get; set; }
        //public short TzAcr2 { get; set; }
        //public short TzAcr3 { get; set; }
        //public short TzAcr4 { get; set; }
        //public short TzAcr5 { get; set; }
        //public short TzAcr6 { get; set; }
        //public short TzAcr7 { get; set; }
        //public short TzAcr8 { get; set; }
        //public short TzAcr9 { get; set; }
        //public short TzAcr10 { get; set; }
        //public short TzAcr11 { get; set; }
        //public short TzAcr12 { get; set; }
        //public short TzAcr13 { get; set; }
        //public short TzAcr14 { get; set; }
        //public short TzAcr15 { get; set; }
        //public short TzAcr16 { get; set; }
        //public short TzAcr17 { get; set; }
        //public short TzAcr18 { get; set; }
        //public short TzAcr19 { get; set; }
        //public short TzAcr20 { get; set; }
        //public short TzAcr21 { get; set; }
        //public short TzAcr22 { get; set; }
        //public short TzAcr23 { get; set; }
        //public short TzAcr24 { get; set; }
        //public short TzAcr25 { get; set; }
        //public short TzAcr26 { get; set; }
        //public short TzAcr27 { get; set; }
        //public short TzAcr28 { get; set; }
        //public short TzAcr29 { get; set; }
        //public short TzAcr30 { get; set; }
        //public short TzAcr31 { get; set; }
        //public short TzAcr32 { get; set; }
        //public short TzAcr33 { get; set; }
        //public short TzAcr34 { get; set; }
        //public short TzAcr35 { get; set; }
        //public short TzAcr36 { get; set; }
        //public short TzAcr37 { get; set; }
        //public short TzAcr38 { get; set; }
        //public short TzAcr39 { get; set; }
        //public short TzAcr40 { get; set; }
        //public short TzAcr41 { get; set; }
        //public short TzAcr42 { get; set; }
        //public short TzAcr43 { get; set; }
        //public short TzAcr44 { get; set; }
        //public short TzAcr45 { get; set; }
        //public short TzAcr46 { get; set; }
        //public short TzAcr47 { get; set; }
        //public short TzAcr48 { get; set; }
        //public short TzAcr49 { get; set; }
        //public short TzAcr50 { get; set; }
        //public short TzAcr51 { get; set; }
        //public short TzAcr52 { get; set; }
        //public short TzAcr53 { get; set; }
        //public short TzAcr54 { get; set; }
        //public short TzAcr55 { get; set; }
        //public short TzAcr56 { get; set; }
        //public short TzAcr57 { get; set; }
        //public short TzAcr58 { get; set; }
        //public short TzAcr59 { get; set; }
        //public short TzAcr60 { get; set; }
        //public short TzAcr61 { get; set; }
        //public short TzAcr62 { get; set; }
        //public short TzAcr63 { get; set; }
        //public short TzAcr64 { get; set; }
    }
}
