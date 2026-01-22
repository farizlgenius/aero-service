using System;

namespace Aero.Application.Interfaces;

public interface IMpCommand
{
  bool InputPointSpecification(short ScpId, short SioNo, short InputNo, short InputMode, short Debounce, short HoldTime);
  bool MonitorPointConfiguration(short ScpId, short SioNo, short InputNo, short LfCode, short Mode, short DelayEntry, short DelayExit, short nMp);
  bool MonitorPointMask(short ScpId, short MpNo, int SetClear);
  bool GetMpStatus(short ScpId, short MpNo, short Count);
}
