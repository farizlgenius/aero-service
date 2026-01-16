using System;

namespace AeroService.DTO.License;

public sealed record HandshakeResult(string sessionId, byte[] sharedKey);
