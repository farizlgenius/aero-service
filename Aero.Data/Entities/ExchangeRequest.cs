using System;

namespace Aero.Domain.Entities;

public sealed record ExchangeRequest(
    string sessionId,
    string appDhPublic,
    string appSignPublic,
    string signature
);
