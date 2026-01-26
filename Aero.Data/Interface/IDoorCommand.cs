

using Aero.Domain.Entities;

namespace Aero.Domain.Interfaces;

public interface IDoorCommand
{
      bool ReaderSpecification(short ScpId, short SioNo, short ReaderNo, short DataFormat, short KeyPadMode, short LedDriveMode, short OsdpFlag);
      bool AccessControlReaderConfiguration(short ScpId, short AcrNo, Door dto);
      bool MomentaryUnlock(short ScpId, short AcrNo);
      bool GetAcrStatus(short ScpId, short AcrNo, short Count);
      bool AcrMode(short ScpId, short AcrNo, short Mode);
}
