using System;
using System.Net;

namespace Aero.Api.Exceptions;

public class GlobalExceptionMiddleware : IMiddleware
{
      public async Task InvokeAsync(HttpContext context, RequestDelegate next)
      {
            try
            {
                  await next(context);
                  
            }catch(Exception ex)
            {
                  await HandleExceptionAsync(context, ex);
            }
      }

      private Task HandleExceptionAsync(HttpContext context, Exception ex)
      {
            context.Response.ContentType = "application/json";
            switch (ex)
            {
                  // Bad Request Exception
                  case BadRequestException:
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        return context.Response.WriteAsJsonAsync(ResponseBuilder(HttpStatusCode.BadRequest, ex));
                  // Forbifden Exception
                  case ForbiddenException:
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        return context.Response.WriteAsJsonAsync(ResponseBuilder(HttpStatusCode.Forbidden, ex)); 
                  // Not Found Exception
                  case NotFoundException:
                        context.Response.StatusCode = StatusCodes.Status404NotFound;
                        return context.Response.WriteAsJsonAsync(ResponseBuilder(HttpStatusCode.NotFound, ex));
                  // Unauthorized Exception
                  case UnauthorizedException:
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        return context.Response.WriteAsJsonAsync(ResponseBuilder(HttpStatusCode.Unauthorized, ex));     
                  // Default Exception
                  default:
                        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                        return context.Response.WriteAsJsonAsync(ResponseBuilder(HttpStatusCode.InternalServerError, ex));
            }
      }

      private object ResponseBuilder(HttpStatusCode code,Exception ex)
      {
            if(ex.InnerException is null)
            {
                  return new
                  {
                        Timestamp = DateTime.UtcNow,
                        Code = code,
                        Message = ex.Message,
                  };
            }
            else
            {
                  return new
                  {
                        Timestamp = DateTime.UtcNow,
                        Code = code,
                        Details = new
                        {
                              Exception = ex.Message ,
                              InnerException = ex.InnerException.Message
                        }
                  };
            }
            
      }
}
