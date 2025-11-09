using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Entity
{
    public sealed class CardHolderAccessLevel
    {
        [Key]
        public int Id { get; set; }
        public string CardHolderId { get; set; }
        public CardHolder CardHolder { get; set; }
        public short AccessLevelId { get; set; }
        public AccessLevel AccessLevel { get; set; }
    }
}
