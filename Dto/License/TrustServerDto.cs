using System;

namespace AeroService.DTO.License;

public sealed record TrustServerDto(string machineId, string peerPublicKey);
