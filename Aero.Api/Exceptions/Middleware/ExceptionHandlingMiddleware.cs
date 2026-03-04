using Aero.Api.Constants;
using Aero.Application.DTOs;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Newtonsoft.Json;
using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Aero.Api.Exceptions.Middleware
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

            return context.Response.WriteAsync(JsonConvert.SerializeObject(new ResponseDto<object>(
                ex is TimeoutException ? HttpStatusCode.RequestTimeout : ex is ArgumentException ? HttpStatusCode.BadRequest : HttpStatusCode.InternalServerError,
                DateTime.UtcNow,
                ex is TimeoutException ? ResponseMessage.REQUEST_TIMEOUT : ex is ArgumentException ? ResponseMessage.BAD_REQUEST : ResponseMessage.INTERNAL_ERROR,
                errors,
                null
                )));
        }
    }
}
