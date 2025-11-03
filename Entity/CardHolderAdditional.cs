using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Entity
{
    public sealed class CardHolderAdditional
    {
        [Key]
        public int Id { get; set; }
        public CardHolder CardHolder { get; set; }
        public string HolderId { get; set; } = string.Empty;
        public string Additional { get; set; } = string.Empty;
    }
}
