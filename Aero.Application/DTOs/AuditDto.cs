using System;

namespace Aero.Application.DTOs;

public record AuditDto(
      string Username,
      string Controller,
      string Action,
      string HttpMethod,
      string Path,
      string RequestBody,
      int StatusCode,
      long ExecutionTimeMs,
      string IpAddress,
      DateTime TimeStamp
);

