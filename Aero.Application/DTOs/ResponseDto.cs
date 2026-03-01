using System.Net;

namespace Aero.Application.DTOs;

public sealed record ResponseDto<T>(DateTime timestamp, HttpStatusCode code, T? data, string message, IEnumerable<string>? details);
