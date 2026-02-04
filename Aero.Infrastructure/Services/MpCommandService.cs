using System;
using Aero.Application.Interfaces;
using HID.Aero.ScpdNet.Wrapper;

namespace Aero.Infrastructure.Services;

public sealed class MpCommandService : BaseAeroCommand,IMpCommand
{
       public bool MonitorPointConfiguration(short ScpId, short SioNo, short InputNo, short LfCode, short Mode, short DelayEntry, short DelayExit, short nMp)
        {
            CC_MP cc_mp = new CC_MP();
            cc_mp.lastModified = 0;
            cc_mp.scp_number = ScpId;
            cc_mp.sio_number = SioNo;
            cc_mp.mp_number = nMp;
            cc_mp.ip_number = InputNo;
            cc_mp.lf_code = LfCode;
            cc_mp.mode = Mode;
            cc_mp.delay_entry = DelayEntry;
            cc_mp.delay_exit = DelayExit;

            bool flag = Send((short)enCfgCmnd.enCcMP, cc_mp);
            return flag;
        }

        public bool MonitorPointMask(short ScpId, short MpNo, int SetClear)
        {
            CC_MPMASK cc = new CC_MPMASK();
            cc.scp_number = ScpId;
            cc.mp_number = MpNo;
            cc.set_clear = (short)SetClear;

            bool flag = Send((short)enCfgCmnd.enCcMpMask, cc);
            return flag;
        }

        public bool GetMpStatus(short ScpId, short MpNo, short Count)
        {
            CC_MPSRQ cc = new CC_MPSRQ();
            cc.scp_number = ScpId;
            cc.first = MpNo;
            cc.count = Count;

            bool flag = Send((short)enCfgCmnd.enCcMpSrq, cc);
            return flag;
        }

      public bool InputPointSpecification(short ScpId, short SioNo, short InputNo, short InputMode, short Debounce, short HoldTime)
      {
        CC_IP cc = new CC_IP();
        cc.scp_number = ScpId;
        cc.lastModified = 0;
        cc.sio_number = SioNo;
        cc.input = InputNo;
        cc.icvt_num = InputMode;
        cc.debounce = Debounce;
        cc.hold_time = HoldTime;
        bool flag = Send((short)enCfgCmnd.enCcInput, cc);
        return flag;
      }
}
