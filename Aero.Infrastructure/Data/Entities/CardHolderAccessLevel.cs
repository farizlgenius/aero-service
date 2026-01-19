using System.ComponentModel.DataAnnotations;

namespace Aero.Infrastructure.Data.Entities
{
    public sealed class CardHolderAccessLevel
    {
        [Key]
        public int id { get; set; }
        public string cardholder_id { get; set; }
        public CardHolder card_holder { get; set; }
        public short access_level_id { get; set; }
        public AccessLevel access_level { get; set; }
    }
}
