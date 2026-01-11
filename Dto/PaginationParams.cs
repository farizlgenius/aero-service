namespace AeroService.DTO
{
    public sealed class PaginationParams
    {
        public int PageNumber { get; set; } = 1; // Default to page 1
        public int PageSize { get; set; } = 10; // Default page size
    }
}
