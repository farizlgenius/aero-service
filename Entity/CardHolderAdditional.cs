using System.ComponentModel.DataAnnotations;

namespace AeroService.Entity
{
    public sealed class CardHolderAdditional
    {
        [Key]
        public int id { get; set; }
        public CardHolder card_holder { get; set; }
        public string holder_id { get; set; } = string.Empty;
        public string additional { get; set; } = string.Empty;
    }
}
