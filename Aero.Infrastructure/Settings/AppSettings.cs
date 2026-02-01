using System;
using Aero.Application.Interfaces;

namespace Aero.Infrastructure.Settings;

public sealed class AppSettings : IAppSettings
{
      public string Secret { get; set; } = string.Empty;

      public short MaxCardFormat { get; set; }

      public short AeroPort { get; set; }

      public short ConnectionType { get; set; }

      public short nChannelId { get; set; }

      public short nPort { get; set; }

      public string LicenseServerUrl { get; set; } = string.Empty;

      public IApiEndpoints ApiEndpoints { get; set; }
}
