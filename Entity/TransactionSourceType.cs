using System.ComponentModel.DataAnnotations;

namespace AeroService.Entity
{
    public sealed class TransactionSourceType
    {
        [Key]
        public int id { get; set; }
        public short transction_source_value { get; set; }
        public TransactionSource transaction_source { get; set; }
        public short transction_type_value { get; set; }
        public TransactionType transaction_type { get; set; }
    }
}
