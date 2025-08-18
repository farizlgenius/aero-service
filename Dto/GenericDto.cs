namespace HIDAeroService.Dto
{
    public class GenericDto
    {
        public DateTime Time { get; set; }
        public short StatusCode { get; set; }
        public string StatusDesc { get; set; }
        public object? Content { get; set; }

    }

    public class GenericDto<T> : GenericDto
    {
        public new T? Content { get; set; }
    }
}
