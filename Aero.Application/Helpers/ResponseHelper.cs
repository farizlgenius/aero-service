

using System.Net;
using Aero.Application.Constants;
using Aero.Application.DTOs;

namespace Aero.Application.Helpers
{
    public sealed class ResponseHelper
    {
        public static ResponseDto<T> SuccessBuilder<T>(T data)
        {
            return new ResponseDto<T>()
            {
                code = HttpStatusCode.OK,
                timestamp = DateTime.UtcNow,
                message = ResponseMessage.SUCCESS,
                details = Enumerable.Empty<string>(),
                data = data
            };
        }

        public static ResponseDto<T> UnsuccessBuilderWithString<T>(string message, string detail)
        {
            return new ResponseDto<T>()
            {
                code = HttpStatusCode.InternalServerError,
                timestamp = DateTime.UtcNow,
                message = message,
                details = new List<string> { detail },
                data = default
            };
        }

        public static ResponseDto<T> UnsuccessBuilder<T>(string message, IEnumerable<string> details)
        {
            return new ResponseDto<T>()
            {
                code = HttpStatusCode.InternalServerError,
                timestamp = DateTime.UtcNow,
                message = message,
                details = details.ToList(),
                data = default
            };
        }

        public static ResponseDto<T> UnsuccessBuilder<T>(T data)
        {
            return new ResponseDto<T>()
            {
                code = HttpStatusCode.InternalServerError,
                timestamp = DateTime.UtcNow,
                message = ResponseMessage.UNSUCCESS,
                details = Enumerable.Empty<string>(),
                data = data
            };
        }

        public static ResponseDto<T> NotFoundBuilder<T>()
        {
            return new ResponseDto<T>()
            {
                code = HttpStatusCode.NotFound,
                timestamp = DateTime.UtcNow,
                message = ResponseMessage.NOT_FOUND,
                details = Enumerable.Empty<string>(),
                data = default
            };
        }

        public static ResponseDto<T> NotFoundBuilder<T>(IEnumerable<string> message)
        {
            return new ResponseDto<T>()
            {
                code = HttpStatusCode.NotFound,
                timestamp = DateTime.UtcNow,
                message = ResponseMessage.NOT_FOUND,
                details=message,
                data = default
            };
        }

        public static ResponseDto<T> FoundReferenceBuilder<T>()
        {
            return new ResponseDto<T>()
            {
                code = HttpStatusCode.InternalServerError,
                timestamp = DateTime.UtcNow,
                message = ResponseMessage.FOUND_REFERENCE,
                details = Enumerable.Empty<string>(),
                data = default
            };
        }

        public static ResponseDto<T> FoundReferenceBuilder<T>(IEnumerable<string> message)
        {
            return new ResponseDto<T>()
            {
                code = HttpStatusCode.InternalServerError,
                timestamp = DateTime.UtcNow,
                message = ResponseMessage.FOUND_REFERENCE,
                details = message,
                data = default
            };
        }


        public static ResponseDto<T> ExceedLimit<T>()
        {
            return new ResponseDto<T>()
            {
                code = HttpStatusCode.InternalServerError,
                timestamp = DateTime.UtcNow,
                message = ResponseMessage.COMPONENT_EXCEED_LIMIT,
                details = Enumerable.Empty<string>(),
                data = default
            };
        }

        public static ResponseDto<T> Duplicate<T>()
        {
            return new ResponseDto<T>()
            {
                code = HttpStatusCode.BadRequest,
                timestamp = DateTime.UtcNow,
                message = ResponseMessage.DUPLICATE_USER,
                details = Enumerable.Empty<string>(),
                data = default
            };
        }

        public static ResponseDto<T> Unauthorize<T>()
        {
            return new ResponseDto<T>()
            {
                code = HttpStatusCode.Unauthorized,
                timestamp = DateTime.UtcNow,
                message = ResponseMessage.UNAUTHORIZED,
                details = Enumerable.Empty<string>(),
                data = default
            };
        }

        public static ResponseDto<T> Unauthorize<T>(IEnumerable<string> message)
        {
            return new ResponseDto<T>()
            {
                code = HttpStatusCode.Unauthorized,
                timestamp = DateTime.UtcNow,
                message = ResponseMessage.UNAUTHORIZED,
                details = message,
                data = default
            };
        }

        public static ResponseDto<T> DefaultRecord<T>()
        {
            return new ResponseDto<T>()
            {
                code = HttpStatusCode.NotAcceptable,
                timestamp = DateTime.UtcNow,
                message = ResponseMessage.DELETE_DEFAULT,
                details = Enumerable.Empty<string>(),
                data = default
            };
        }


    }
}
