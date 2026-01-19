using System;

namespace Aero.Application.DTOs;

public sealed record GenerateDemoRequest(string company,string customerSite,string machineId,string? sessionId);