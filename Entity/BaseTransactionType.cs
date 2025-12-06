namespace HIDAeroService.Entity
{
    public class BaseTransactionType
    {
        public int Id { get; set; }
        public ICollection<TransactionFlag> TransactionFlags { get; set; }
        public Transaction Transaction { get; set; }
    }
}
