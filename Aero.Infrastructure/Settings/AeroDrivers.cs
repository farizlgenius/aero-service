using System;
using Aero.Domain.Interfaces;

namespace Aero.Infrastructure.Settings;

public sealed class AeroDrivers : IAeroDrivers
{
      public short nPort {get; set;}

      public short nScps {get; set;}

      public short nChannelId {get; set;}

      public short cType {get; set;}
}
