using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Entity
{
    public sealed class ar_nak : ar_base_entity
    {
        public int tag_no { get; set; }
        public string? nak_reason { get; set; }
        public int nak_desc_code { get; set; }
        public string? nak_desc { get; set; }
    }
}
