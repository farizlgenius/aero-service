using System;
using Aero.Application.Interfaces;
using HID.Aero.ScpdNet.Wrapper;

namespace Aero.Infrastructure.Services;

public sealed class ScpCommandService : BaseAeroCommand,IScpCommand
{
      /// <summary>
      /// Command used for detach controller
      /// </summary>
      /// <param name="component">SCP Id</param>
      /// <returns>Command status</returns>
      public bool DetachScp(short component)
      {
            CC_ATTACHSCP c = new CC_ATTACHSCP();
            c.nSCPId = component;
            c.nChannelId = 0;
            bool flag = Send((short)enCfgCmnd.enCcDetachScp, c);
            return flag;
      }

      public bool ResetScp(short component)
      {
            CC_RESET cc_reset = new CC_RESET();
            cc_reset.scp_number = component;
            bool flag = Send((short)enCfgCmnd.enCcReset, cc_reset);
            return flag;
      }
}
