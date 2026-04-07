using System;
using System.Diagnostics;
using System.Security.Claims;
using System.Text;
using Aero.Application.DTOs;
using Aero.Application.Interfaces;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace Aero.Api.Middlewares;

public class AuditTrailMiddleware(IAuditService service) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (context.Request.Path.StartsWithSegments("/swagger"))
        {
            await next(context);
            return;
        }

        if (context.Request.Path.StartsWithSegments("/aeroHub"))
        {
            await next(context);
            return;
        }

        var stopwatch = Stopwatch.StartNew();

        // Allow request body to be read multiple times
        context.Request.EnableBuffering();

        string requestBody = await ReadRequestBody(context);

        // Get route info (after UseRouting)
        var endpoint = context.GetEndpoint();
        var descriptor = endpoint?.Metadata.GetMetadata<ControllerActionDescriptor>();

        string controller = descriptor?.ControllerName ?? "Unknown";
        string action = descriptor?.ActionName ?? "Unknown";

        string method = context.Request.Method;
        string path = context.Request.Path;
        string user = GetUsername(context.User);
        string ip = context.Connection.RemoteIpAddress?.ToString() ?? "";
        string location = context.Request.RouteValues["location"]?.ToString() ?? "";

        try
        {
            await next(context);
        }
        finally
        {
            stopwatch.Stop();

            var audit = new AuditDto(
                user,
                controller,
                action,
                method,
                path,
                requestBody,
                context.Response.StatusCode,
                stopwatch.ElapsedMilliseconds,
                ip,
                DateTime.UtcNow
            );

            await service.CreateAuditAsync(audit);
        }
    }

    private async Task<string> ReadRequestBody(HttpContext context)
    {
        context.Request.Body.Position = 0;

        using var reader = new StreamReader(
            context.Request.Body,
            Encoding.UTF8,
            leaveOpen: true);

        var body = await reader.ReadToEndAsync();
        context.Request.Body.Position = 0;

        return body;
    }

    public static string GetUsername(ClaimsPrincipal user)
    {
        if (user?.Identity?.IsAuthenticated != true)
            return "anonymous";

        return user.FindFirst(ClaimTypes.Name)?.Value
            ?? user.FindFirst("unique_name")?.Value
            ?? user.FindFirst("preferred_username")?.Value
            ?? user.FindFirst("email")?.Value
            ?? user.FindFirst("sub")?.Value
            ?? "unknown";
    }

}
