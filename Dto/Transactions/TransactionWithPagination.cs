namespace HIDAeroService.DTO.Transactions
{
    public sealed class TransactionWithPagination
    {
        public int Count { get; set; }
        public ICollection<TransactionDto> Transactions { get; set; }
    }
}
