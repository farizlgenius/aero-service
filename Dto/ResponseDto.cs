using System.Net;

namespace HIDAeroService.DTO
{

    public sealed class ResponseDto<T>
    {
        public DateTime TimeStamp { get; set; } = DateTime.UtcNow;
        public HttpStatusCode Code { get; set; }
        public new T? Data { get; set; }
        public string Message { get; set; }
        public IEnumerable<string> Details { get; set; }
    }
}
