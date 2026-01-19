using System;

namespace Aero.Application.DTOs;

public sealed record ExchangeRequest(string sessionId,string appDhPublic,string appSignPublic,string signature);
