using HIDAeroService.DTO.Transactions;

namespace HIDAeroService.DTO
{
    public class PaginationDto
    {
        public IEnumerable<TransactionDto> Data { get; set; }

        public PaginationData Page { get; set; }
    }

    public class PaginationData
    {
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPage { get; set; }

    }
}
