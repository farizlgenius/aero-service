using System.ComponentModel.DataAnnotations;

namespace HIDAeroService.Entity
{
    public sealed class TransactionFlag
    {
        [Key]
        public int id { get; set; }
        public string topic { get; set; } = string.Empty;
        public string name { get; set; } = string.Empty;
        public string description {  get; set; } = string.Empty;
        public Transaction transaction { get; set; }

    }
}
