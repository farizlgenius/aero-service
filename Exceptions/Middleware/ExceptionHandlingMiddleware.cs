using HIDAeroService.Constant;
using HIDAeroService.Constants;
using HIDAeroService.DTO;
using HIDAeroService.Exceptions.Custom;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Newtonsoft.Json;
using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HIDAeroService.Exceptions.Middleware
{
    public sealed class ExceptionHandlingMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext httpContext, RequestDelegate next)
        {
            
            try
            {
                await next(httpContext);

            }
            catch (Exception ex)
            {
                await HandleException(httpContext, ex);
            }
        }


        private Task HandleException(HttpContext context, Exception ex)
        {
           
            List<string> errors = new List<string>();
            errors.Add(ex.Message);
            context.Response.StatusCode = ex is TimeoutException ? (int)HttpStatusCode.RequestTimeout : (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            return context.Response.WriteAsync(JsonConvert.SerializeObject(new ResponseDto<object>()
            {
                TimeStamp = DateTime.UtcNow,
                Code = ex is TimeoutException ? HttpStatusCode.RequestTimeout : HttpStatusCode.InternalServerError,
                Data = null,
                Message = ex is TimeoutException ? ResponseMessage.REQUEST_TIMEOUT : ResponseMessage.INTERNAL_ERROR,
                Details = errors
            }));
        }
    }
}
