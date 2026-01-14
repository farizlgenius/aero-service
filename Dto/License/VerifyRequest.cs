using System;

namespace AeroService.Dto.License;

public sealed record VerifyRequest(string signature);