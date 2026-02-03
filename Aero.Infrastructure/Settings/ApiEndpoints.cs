using System;
using Aero.Application.Interfaces;
using Aero.Domain.Interfaces;

namespace Aero.Infrastructure.Settings;

public class ApiEndpoints : IApiEndpoints
{
      public string Exchnage {get; set;} = string.Empty;

      public string GenerateDemo {get; set;} = string.Empty;

      public string GenerateLicense {get; set;} = string.Empty;
}
