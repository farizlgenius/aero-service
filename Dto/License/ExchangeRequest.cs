using System;

namespace AeroService.Dto.License;

public sealed record ExchangeRequest(string sessionId,string appDhPublic,string appSignPublic,string signature);
