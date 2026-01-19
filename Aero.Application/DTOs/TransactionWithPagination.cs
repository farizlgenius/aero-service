namespace Aero.Application.DTOs
{
    public sealed class TransactionWithPagination
    {
        public int Count { get; set; }
        public List<TransactionDto> Transactions { get; set; } = new List<TransactionDto>();
    }
}
