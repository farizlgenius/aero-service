

using System.Net;
using Aero.Application.Constants;
using Aero.Application.DTOs;

namespace Aero.Application.Helpers
{
    public sealed class ResponseHelper
    {
        public static ResponseDto<T> SuccessBuilder<T>(T data)
        {
            return new ResponseDto<T>
            (
                 HttpStatusCode.OK,
                 DateTime.UtcNow,
                 ResponseMessage.SUCCESS,
                 Enumerable.Empty<string>(),
                 data
            );
        }

        public static ResponseDto<T> UnsuccessBuilderWithString<T>(string message, string detail)
        {
            return new ResponseDto<T>
            (
                HttpStatusCode.InternalServerError,
                DateTime.UtcNow,
                 message,
                 new List<string> { detail },
                 default
            );
        }

        public static ResponseDto<T> UnsuccessBuilder<T>(string message, IEnumerable<string> details)
        {
            return new ResponseDto<T>
            (
                 HttpStatusCode.InternalServerError,
                 DateTime.UtcNow,
                 message,
                 details.ToList(),
                 default
            );
        }

        public static ResponseDto<T> UnsuccessBuilder<T>(T data)
        {
            return new ResponseDto<T>
            (
                 HttpStatusCode.InternalServerError,
                 DateTime.UtcNow,
                 ResponseMessage.UNSUCCESS,
                 Enumerable.Empty<string>(),
                 data
            );
        }

        public static ResponseDto<T> NotFoundBuilder<T>()
        {
            return new ResponseDto<T>
            (
                 HttpStatusCode.NotFound,
                 DateTime.UtcNow,
                 ResponseMessage.NOT_FOUND,
                 Enumerable.Empty<string>(),
                 default
            );
        }

        public static ResponseDto<T> NotFoundBuilder<T>(IEnumerable<string> message)
        {
            return new ResponseDto<T>
            (
                 HttpStatusCode.NotFound,
                 DateTime.UtcNow,
                 ResponseMessage.NOT_FOUND,
                message,
                 default
            );
        }

        public static ResponseDto<T> NotFoundBuilder<T>(string message)
        {
            return new ResponseDto<T>(HttpStatusCode.NotFound,
                 DateTime.UtcNow,
                 message,
                 [],
                 default);

        }

        public static ResponseDto<T> FoundReferenceBuilder<T>()
        {
            return new ResponseDto<T>(HttpStatusCode.InternalServerError,
                 DateTime.UtcNow,
                 ResponseMessage.FOUND_REFERENCE,
                 Enumerable.Empty<string>(),
                 default);
         
        }

        public static ResponseDto<T> FoundReferenceBuilder<T>(IEnumerable<string> message)
        {
            return new ResponseDto<T>(HttpStatusCode.InternalServerError,
                 DateTime.UtcNow,
                 ResponseMessage.FOUND_REFERENCE,
                 message,
                 default);
       
        }


        public static ResponseDto<T> ExceedLimit<T>()
        {
            return new ResponseDto<T>(HttpStatusCode.InternalServerError,
                 DateTime.UtcNow,
                 ResponseMessage.COMPONENT_EXCEED_LIMIT,
                 Enumerable.Empty<string>(),
                 default);
          
        }

        public static ResponseDto<T> Duplicate<T>()
        {
            return new ResponseDto<T>(
                HttpStatusCode.BadRequest,
                 DateTime.UtcNow,
                 ResponseMessage.DUPLICATE_RECORD,
                 Enumerable.Empty<string>(),
                 default
            );
            
        }

        public static ResponseDto<T> BadRequest<T>()
        {
            return new ResponseDto<T>(
                HttpStatusCode.BadRequest,
                 DateTime.UtcNow,
                 ResponseMessage.DUPLICATE_RECORD,
                 Enumerable.Empty<string>(),
                 default
            );
      
        }

        public static ResponseDto<T> BadRequestName<T>()
        {
            return new ResponseDto<T>(
                 HttpStatusCode.BadRequest,
                 DateTime.UtcNow,
                 ResponseMessage.DUPLICATE_NAME,
                 Enumerable.Empty<string>(),
                 default
            );
          
        }

        public static ResponseDto<T> Unauthorize<T>()
        {
            return new ResponseDto<T>(
                 HttpStatusCode.Unauthorized,
                 DateTime.UtcNow,
                 ResponseMessage.UNAUTHORIZED,
                 Enumerable.Empty<string>(),
                 default
            );
           
        }

        public static ResponseDto<T> Unauthorize<T>(IEnumerable<string> message)
        {
            return new ResponseDto<T>(
 HttpStatusCode.Unauthorized,
                 DateTime.UtcNow,
                 ResponseMessage.UNAUTHORIZED,
                 message,
                 default
            );
        
        }

        public static ResponseDto<T> DefaultRecord<T>()
        {
            return new ResponseDto<T>(HttpStatusCode.NotAcceptable,
                 DateTime.UtcNow,
                 ResponseMessage.DELETE_DEFAULT,
                 Enumerable.Empty<string>(),
                 default);
          
        }


    }
}
