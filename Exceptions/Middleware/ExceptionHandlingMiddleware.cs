using AeroService.Constant;
using AeroService.Constants;
using AeroService.DTO;
using AeroService.Exceptions.Custom;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Newtonsoft.Json;
using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AeroService.Exceptions.Middleware
{
    public sealed class ExceptionHandlingMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext httpContext, RequestDelegate next)
        {
            
            try
            {
                Console.WriteLine("[] >> " + httpContext.Connection.RemoteIpAddress.ToString());
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
                timestamp = DateTime.UtcNow,
                code = ex is TimeoutException ? HttpStatusCode.RequestTimeout : HttpStatusCode.InternalServerError,
                data = null,
                message = ex is TimeoutException ? ResponseMessage.REQUEST_TIMEOUT : ResponseMessage.INTERNAL_ERROR,
                details = errors
            }));
        }
    }
}
