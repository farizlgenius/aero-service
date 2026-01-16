using System;

namespace AeroService.DTO.License;

public sealed record GenerateDemoRequest(string company,string customerSite,string machineId,string? sessionId);