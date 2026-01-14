using System;

namespace AeroService.Dto.License;

public sealed record ExchangeRequest(string dhPub,string signPub);
