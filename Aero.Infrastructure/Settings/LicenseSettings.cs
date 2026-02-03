using System;
using Aero.Domain.Interfaces;

namespace Aero.Infrastructure.Settings;

public sealed class LicenseSettings : ILicenseSettings
{
      public string Secret {get; set;} = string.Empty;

      public string LicenseServerUrl {get; set;} = string.Empty;
}
