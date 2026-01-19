using System;

namespace Aero.Infrastructure.External;

public class AppSetting
{
    public string Secret { get; set; }
    public short MaxCardFormat { get; set; }
    public string LicenseServerUrl {get; set;} 
    public Endpoints Endpoints { get; set; }
}

public class Endpoints
{
    public string Exchange { get; set; } = string.Empty;
    public string GenerateDemo { get; set; } = string.Empty;
    public string GenerateLicense { get; set; } = string.Empty;
}

