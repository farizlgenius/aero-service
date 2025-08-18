namespace HIDAeroService.Dto
{
    public class PaginationDto<T>
    {
        public new T Data { get; set; }

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
