using System;

namespace Aero.Infrastructure.External;

public class AppSetting
{
    public required string Secret { get; set; }
    public short MaxCardFormat { get; set; }
    public required string LicenseServerUrl {get; set;} 
    public Endpoints? Endpoints { get; set; }
}

public class Endpoints
{
    public required string Exchange { get; set; } = string.Empty;
    public required string GenerateDemo { get; set; } = string.Empty;
    public required string GenerateLicense { get; set; } = string.Empty;
}

