using System.Net;

namespace Aero.Application.DTOs;

public sealed record ResponseDto<T>(HttpStatusCode code,DateTime timestamp,string message,IEnumerable<string>? details,T? data );
