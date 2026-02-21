namespace Aero.Infrastructure.Data.Entities
{
    public class BaseTransactionType
    {
        public int id { get; set; }
        public ICollection<TransactionFlag> transaction_flags { get; set; }
        public Transaction transaction { get; set; }
    }
}
