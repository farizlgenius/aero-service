using System;

namespace Aero.Domain.Entities;

public sealed record ExchangeResponse(string sessionId, string dhPub, string signPub,string signature);

