using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Entity
{
    public sealed class TransactionSource
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public short Value { get; set; }
        public ICollection<TransactionSourceType> TransactionSourceTypes { get; set; }
    }
}
