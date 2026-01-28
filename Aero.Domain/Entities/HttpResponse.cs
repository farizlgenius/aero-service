using System;
using System.Net;

namespace Aero.Domain.Entities;

public sealed record HttpResponse<T>(HttpStatusCode code, T payload, Guid uuid, string message, DateTime timeStamp);
