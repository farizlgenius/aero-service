using HIDAeroService.Constants;
using HIDAeroService.Dto;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace HIDAeroService.Helpers
{
    public class Helper
    {
        public static BaseResponse<T> ResponseBuilder<T>(HttpStatusCode statusCode,string statusDesc,T data)
        {
            return new BaseResponse<T>
            {
                Code = statusCode,
                Detail = statusDesc,
                Time = DateTime.Now,
                Content = data
            };
        }

        public static BaseResponse ResponseBuilder(HttpStatusCode statusCode, string statusDesc)
        {
            return new BaseResponse
            {
                Code = statusCode,
                Detail = statusDesc,
                Time = DateTime.Now,
            };
        }

        public static Response<T> ResponseBuilder<T>(HttpStatusCode code,T data,string messsage,List<string> errors)
        {
            return new Response<T>
            {
                Code = code,
                TimeStamp = DateTime.Now,
                Data=data,
                Message= messsage,
                Errors=errors
            };
        }

        public static string ResponseCommandUnsuccessMessageBuilder(short Id)
        {
            return $"[{Id}] :" + ConstantsHelper.COMMAND_UNSUCCESS;
        }
    }
}
