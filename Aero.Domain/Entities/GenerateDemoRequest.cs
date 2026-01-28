using System;

namespace Aero.Domain.Entities;

public sealed record GenerateDemoRequest(string company,string customerSite,string machineId,string? sessionId);
