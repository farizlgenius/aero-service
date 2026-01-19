using System;

namespace Aero.Application.DTOs;

public sealed record ExchangeResponse(string sessionId, string dhPub, string signPub,string signature);
