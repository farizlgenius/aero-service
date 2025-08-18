using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Entity
{
    public sealed class ar_credentials : ar_base_entity
    {
        public Guid card_holder_refenrence_number { get; set; }
        public int bits{ get; set; }
        public int issue_code{ get; set; }
        public int facility_code{ get; set; }
        public long card_number{ get; set; }
        public string? pin{  get; set; }
        public int act_time{ get; set; }
        public int deact_time{ get; set; }
        public short access_level { get; set; }
        public string? image { get; set; }

        public bool is_active { get; set; }

    }
}
