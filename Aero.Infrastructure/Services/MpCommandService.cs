using System;
using Aero.Application.Interfaces;
using HID.Aero.ScpdNet.Wrapper;

namespace Aero.Infrastructure.Services;

public sealed class MpCommandService : BaseAeroCommand, IMpCommand
{
  public bool InputPointSpecification(short ScpId, short SioNo, short InputNo, short InputMode, short Debounce, short HoldTime)
  {
    CC_IP cc_ip = new CC_IP();
    cc_ip.lastModified = 0;
    cc_ip.scp_number = ScpId;
    cc_ip.sio_number = SioNo;
    cc_ip.input = InputNo;
    cc_ip.icvt_num = InputMode;
    cc_ip.debounce = Debounce;
    cc_ip.hold_time = HoldTime;

    bool flag = Send((short)enCfgCmnd.enCcInput, cc_ip);
    return flag;
  }
}
