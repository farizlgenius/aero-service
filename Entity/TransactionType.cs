using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Entity
{
    public sealed class TransactionType
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; } = string.Empty;
        public short value { get; set; }
        public ICollection<TransactionSourceType> transaction_source_types { get; set; }
        public ICollection<TransactionCode> transaction_codes { get; set; }
    }
}
