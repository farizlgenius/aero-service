using System;
using Aero.Application.Interfaces;


namespace Aero.Infrastructure.Settings;

public sealed class RedisSettings : IRedisSettings
{
      public string Configuration {get; set;} = string.Empty;
}
