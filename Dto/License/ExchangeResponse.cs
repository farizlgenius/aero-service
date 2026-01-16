using System;

namespace AeroService.Dto.License;

public sealed record ExchangeResponse(string sessionId, string dhPub, string signPub,string signature);
