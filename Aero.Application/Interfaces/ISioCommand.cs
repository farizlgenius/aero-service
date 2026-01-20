using System;

namespace Aero.Application.Interfaces;

public interface ISioCommand
{
  bool SioDriverConfiguration(short ScpId, short SioDriverNo, short IoModulePort, int BaudRate, short ProtocolType);
  bool SioPanelConfiguration(short ScpId, short SioNo, short Model, short nInput, short nOutput, short nReader, short ModuleAddress, short SIODriverPort, bool isEnable);
}
