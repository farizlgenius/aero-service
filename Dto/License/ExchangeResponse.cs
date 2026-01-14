using System;

namespace AeroService.Dto.License;

public sealed record ExchangeResponse(string dhPub, string signPub,string signature);
