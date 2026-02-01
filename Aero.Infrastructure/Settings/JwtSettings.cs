using System;
using Aero.Application.Interfaces;

namespace Aero.Infrastructure.Settings;

public sealed class JwtSettings : IJwtSettings
{
      public string Secret { get; set; } = string.Empty;

      public string Issuer { get; set; } = string.Empty;

      public string Audience { get; set; } = string.Empty;

      public short AccessTokenMinute { get; set; }
}
