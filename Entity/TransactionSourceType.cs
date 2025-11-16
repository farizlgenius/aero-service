using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Entity
{
    public sealed class TransactionSourceType
    {
        [Key]
        public int Id { get; set; }
        public short TransactionSourceValue { get; set; }
        public TransactionSource TransactionSource { get; set; }
        public short TransactionTypeValue { get; set; }
        public TransactionType TransactionType { get; set; }
    }
}
