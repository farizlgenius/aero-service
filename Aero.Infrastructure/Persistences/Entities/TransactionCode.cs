using System.ComponentModel.DataAnnotations;

namespace Aero.Infrastructure.Persistences.Entities
{
    public sealed class TransactionCode
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public short value { get; set; }
        public short transaction_type_value { get; set; }
        public TransactionType transaction_type { get; set; }

    }

}
