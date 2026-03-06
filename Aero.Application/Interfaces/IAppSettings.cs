using System;
using Aero.Domain.Interfaces;

namespace Aero.Application.Interfaces;

public interface IAppSettings
{
      IAeroDrivers AeroDrivers {get;}
      ILicenseSettings LicenseSettings {get;}
      IApiEndpoints ApiEndpoints {get;}
      IPorts Ports {get;}
}
