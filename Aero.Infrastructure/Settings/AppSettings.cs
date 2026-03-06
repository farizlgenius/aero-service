using Aero.Application.Interfaces;
using Aero.Domain.Interfaces;

namespace Aero.Infrastructure.Settings;

public sealed class AppSettings : IAppSettings
{

      public ApiEndpoints ApiEndpoints { get; set; }
      public Ports Ports {get; set;}

      public AeroDrivers AeroDrivers {get; set;}

      public LicenseSettings LicenseSettings {get; set;}

      IAeroDrivers IAppSettings.AeroDrivers => AeroDrivers;

      ILicenseSettings IAppSettings.LicenseSettings => LicenseSettings;

      IApiEndpoints IAppSettings.ApiEndpoints => ApiEndpoints;

      IPorts IAppSettings.Ports => Ports;
}
