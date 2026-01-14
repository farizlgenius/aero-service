using System;
using System.Net;

namespace AeroService.Dto;

public record BaseHttpResponse<T>(HttpStatusCode code, T payload, Guid uuid, string message, DateTime timeStamp);
