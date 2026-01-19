using System;
using System.Net;

namespace Aero.Application.DTOs;

public sealed record BaseHttpResponse<T>(HttpStatusCode code, T payload, Guid uuid, string message, DateTime timeStamp);
