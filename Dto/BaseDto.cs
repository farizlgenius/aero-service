using System.Net;

namespace HIDAeroService.Dto
{
    public class BaseDto
    {
        public DateTime Time { get; set; }
        public short StatusCode { get; set; }
        public string StatusDesc { get; set; }

    }

    public class BaseDto<T> : BaseDto
    {
        public new T? Content { get; set; }
    }

    public class BaseResponse
    {
        public DateTime Time { get; set; }
        public HttpStatusCode Code { get; set; }
        public string Detail { get; set; }
    }

    public class BaseResponse<T> : BaseResponse
    {
        public new T? Content { get; set; }
    }

    public class ApiResponse
    {
        public DateTime Time { get; set; }
        public HttpStatusCode Code { get; set; }
        public string Detail { get; set; }
    }

    public class ApiResponse<T> : BaseResponse
    {
        public new T? Content { get; set; }
    }

    public class Response<T>
    {
        public DateTime TimeStamp { get; set; }
        public HttpStatusCode Code { get; set; }
        public new T? Data { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; }
    }
}
