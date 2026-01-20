using System;
using HID.Aero.ScpdNet.Wrapper;

namespace Aero.Infrastructure.Services;

public class BaseAeroCommand
{
      protected bool Send(short command, IConfigCommand cfg)
      {
            SCPConfig scp = new SCPConfig();
            bool success = scp.scpCfgCmndEx(command, cfg);
            return success;
      }
}
