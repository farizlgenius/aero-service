using System.ComponentModel.DataAnnotations;

namespace Aero.Infrastructure.Persistences.Entities
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
