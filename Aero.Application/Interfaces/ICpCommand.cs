using System;

namespace Aero.Application.Interfaces;

public interface ICpCommand 
{
      bool OutputPointSpecification(short ScpId, short SioNo, short OutputNo, short OutputMode);
      bool ControlPointConfiguration(short ScpId, short SioNo, short CpNo, short OutputNo, short DefaultPulseTime);
      bool ControlPointCommand(short ScpId, short cpNo, short command);
      bool GetCpStatus(short ScpId, int DriverId, short Count);
}
