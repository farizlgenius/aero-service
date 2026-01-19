using System;

namespace Aero.Application.DTOs;

public sealed record HandshakeResult(string sessionId, byte[] sharedKey);
