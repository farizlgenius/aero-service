using System;
using Aero.Application.DTOs;
using Aero.Application.Interfaces;
using HID.Aero.ScpdNet.Wrapper;

namespace Aero.Infrastructure.Services;

public sealed class MpgCommandService : BaseAeroCommand,IMpgCommand
{
      public bool ConfigureMonitorPointGroup(short ScpId, short ComponentId, short nMonitor, List<MonitorGroupListDto> list)
        {
            CC_MPG c = new CC_MPG();
            c.lastModified = 0;
            c.scp_number = ScpId;
            c.mpg_number = ComponentId;
            c.nMpCount = nMonitor;
            int i = 0;
            foreach (var l in list)
            {
                c.nMpList[i] = l.PointType;
                i += 1;
                c.nMpList[i] = l.PointNumber;
            }

            bool flag = Send((short)enCfgCmnd.enCcMpg, c);
            return flag;
        }

        public bool MonitorPointGroupArmDisarm(short ScpId, short ComponentId, short Command, short Arg1)
        {
            CC_MPGSET c = new CC_MPGSET();
            c.scp_number = ScpId;
            c.mpg_number = ComponentId;
            c.command = Command;
            c.arg1 = Arg1;

            bool flag = Send((short)enCfgCmnd.enCcMpgSet, c);
            //if (flag)
            //{
            //    return await TrackCommandAsync(tag, hardware_id, Constants.command.C321);
            //}
            return flag;
        }


}
