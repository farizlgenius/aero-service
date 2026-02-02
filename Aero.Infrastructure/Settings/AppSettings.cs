using System;
using Aero.Application.Interfaces;
using Aero.Domain.Interfaces;

namespace Aero.Infrastructure.Settings;

public sealed class AppSettings : IAppSettings
{

      public IApiEndpoints ApiEndpoints { get; set; }
      public IPorts Ports {get; set;}

      public IAeroDrivers AeroDrivers {get; set;}

      public ILicenseSettings LicenseSettings {get; set;}
}
