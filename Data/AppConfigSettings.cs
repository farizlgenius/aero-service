namespace AeroService.Data
{
    public sealed class AppConfigSettings
    {
        public short MaxCardFormat { get; set; }
        public string LicenseServerUrl {get; set;} = string.Empty;

    }
}
