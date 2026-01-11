using System.ComponentModel.DataAnnotations;

namespace AeroService.Entity
{
    public sealed class TransactionSource
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; } = string.Empty;
        public short value { get; set; }
        public string source { get; set; } = string.Empty;
        public ICollection<TransactionSourceType> transaction_source_type { get; set; }
    }
}
