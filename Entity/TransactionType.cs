using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Entity
{
    public sealed class TransactionType
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public short Value { get; set; }
        public ICollection<TransactionSourceType> TransactionSourceTypes { get; set; }
        public ICollection<TransactionCode> transactionCodes { get; set; }
    }
}
