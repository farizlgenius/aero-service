using System.Net;

namespace Aero.Application.DTOs
{

    public sealed class ResponseDto<T>
    {
        public DateTime timestamp { get; set; } = DateTime.UtcNow;
        public HttpStatusCode code { get; set; }
        public new T? data { get; set; } 
        public string message { get; set; } = string.Empty;
        public IEnumerable<string>? details { get; set; }
    }
}
