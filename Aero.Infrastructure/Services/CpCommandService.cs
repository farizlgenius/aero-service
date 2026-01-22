using System;
using Aero.Application.Interfaces;
using HID.Aero.ScpdNet.Wrapper;

namespace Aero.Infrastructure.Services;

public sealed class CpCommandService : BaseAeroCommand, ICpCommand
{
      public bool OutputPointSpecification(short ScpId, short SioNo, short OutputNo, short OutputMode)
      {
            CC_OP cc_op = new CC_OP();
            cc_op.lastModified = 0;
            cc_op.scp_number = ScpId;
            cc_op.sio_number = SioNo;
            cc_op.output = OutputNo;
            cc_op.mode = OutputMode;

            bool flag = Send((short)enCfgCmnd.enCcOutput, cc_op);
            return flag;
      }


      public bool ControlPointConfiguration(short ScpId, short SioNo, short CpNo, short OutputNo, short DefaultPulseTime)
      {
            CC_CP cc_cp = new CC_CP();
            cc_cp.lastModified = 0;
            cc_cp.scp_number = ScpId;
            cc_cp.sio_number = SioNo;
            cc_cp.cp_number = CpNo;
            cc_cp.op_number = OutputNo;
            cc_cp.dflt_pulse = DefaultPulseTime;

            bool flag = Send((short)enCfgCmnd.enCcCP, cc_cp);
            return flag;

      }



      public bool ControlPointCommand(short ScpId, short cpNo, short command)
      {
            CC_CPCTL cc_cpctl = new CC_CPCTL();
            cc_cpctl.scp_number = ScpId;
            cc_cpctl.cp_number = cpNo;
            cc_cpctl.command = command;
            cc_cpctl.on_time = 0;
            cc_cpctl.off_time = 0;
            cc_cpctl.repeat = 0;

            bool flag = Send((short)enCfgCmnd.enCcCpCtl, cc_cpctl);
            return flag;

      }

      public bool GetCpStatus(short ScpId, short CpNo, short Count)
      {
            CC_CPSRQ cc = new CC_CPSRQ();
            cc.scp_number = ScpId;
            cc.first = CpNo;
            cc.count = Count;

            bool flag = Send((short)enCfgCmnd.enCcCpSrq, cc);
            return flag;
      }
}
