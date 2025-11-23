using HIDAeroService.Constants;
using HIDAeroService.Logging;
using HIDAeroService.Model;
using HIDAeroService.Utility;
using System.Net;
using HIDAeroService.Constant;
using HIDAeroService.DTO;

namespace HIDAeroService.Helpers
{
    public sealed class ResponseHelper
    {
        public static ResponseDto<T> SuccessBuilder<T>(T data)
        {
            return new ResponseDto<T>()
            {
                Code = HttpStatusCode.OK,
                TimeStamp = DateTime.UtcNow,
                Message = ResponseMessage.SUCCESS,
                Details = Enumerable.Empty<string>(),
                Data = data
            };
        }

        public static ResponseDto<T> UnsuccessBuilder<T>(string message, string detail)
        {
            return UnsuccessBuilder<T>(message, new List<string> { detail });
        }

        public static ResponseDto<T> UnsuccessBuilder<T>(string message, IEnumerable<string> details)
        {
            return new ResponseDto<T>()
            {
                Code = HttpStatusCode.InternalServerError,
                TimeStamp = DateTime.UtcNow,
                Message = message,
                Details = details.ToList(),
                Data = default
            };
        }

        public static ResponseDto<T> NotFoundBuilder<T>()
        {
            return new ResponseDto<T>()
            {
                Code = HttpStatusCode.NotFound,
                TimeStamp = DateTime.UtcNow,
                Message = ResponseMessage.NOT_FOUND_RECORD,
                Details = Enumerable.Empty<string>(),
                Data = default
            };
        }

        public static ResponseDto<T> NotFoundBuilder<T>(IEnumerable<string> message)
        {
            return new ResponseDto<T>()
            {
                Code = HttpStatusCode.NotFound,
                TimeStamp = DateTime.UtcNow,
                Message = ResponseMessage.NOT_FOUND_RECORD,
                Details=message,
                Data = default
            };
        }

        public static ResponseDto<T> FoundReferenceBuilder<T>()
        {
            return new ResponseDto<T>()
            {
                Code = HttpStatusCode.InternalServerError,
                TimeStamp = DateTime.UtcNow,
                Message = ResponseMessage.FOUND_RELATE_REFERENCE,
                Details = Enumerable.Empty<string>(),
                Data = default
            };
        }


        public static ResponseDto<T> ExceedLimit<T>()
        {
            return new ResponseDto<T>()
            {
                Code = HttpStatusCode.InternalServerError,
                TimeStamp = DateTime.UtcNow,
                Message = ResponseMessage.COMPONENT_EXCEED_LIMIT,
                Details = Enumerable.Empty<string>(),
                Data = default
            };
        }

        public static ResponseDto<T> Duplicate<T>()
        {
            return new ResponseDto<T>()
            {
                Code = HttpStatusCode.BadRequest,
                TimeStamp = DateTime.UtcNow,
                Message = ResponseMessage.DUPLICATE_USER,
                Details = Enumerable.Empty<string>(),
                Data = default
            };
        }

        public static ResponseDto<T> Unauthorize<T>()
        {
            return new ResponseDto<T>()
            {
                Code = HttpStatusCode.Unauthorized,
                TimeStamp = DateTime.UtcNow,
                Message = ResponseMessage.UNAUTHORIZED,
                Details = Enumerable.Empty<string>(),
                Data = default
            };
        }

        public static ResponseDto<T> Unauthorize<T>(IEnumerable<string> message)
        {
            return new ResponseDto<T>()
            {
                Code = HttpStatusCode.Unauthorized,
                TimeStamp = DateTime.UtcNow,
                Message = ResponseMessage.UNAUTHORIZED,
                Details = message,
                Data = default
            };
        }


    }
}
