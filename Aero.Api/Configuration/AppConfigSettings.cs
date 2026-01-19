using Aero.Domain.Interface;

namespace AeroService.Data
{
    public sealed class AppSettings 
    {
        public string Secret { get; set; } = string.Empty;
        public short MaxCardFormat { get; set; }
        public string LicenseServerUrl {get; set;} = string.Empty;
        public ApiEndpoints ApiEndpoints { get; set; } = new ApiEndpoints();

    }

    public sealed class ApiEndpoints 
    {
        public string Exchange { get; set; } = string.Empty;
        public string GenerateDemo { get; set; } = string.Empty;
        public string GenerateLicense { get; set; } = string.Empty;
    }
}
