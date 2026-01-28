using System;
using Aero.Domain.Interface;
using HID.Aero.ScpdNet.Wrapper;

namespace Aero.Infrastructure.Services;

public class AreaCommandService : BaseAeroCommand, IAreaCommand
{
      public bool ConfigureAccessArea(short ScpId, short AreaNo, short MultiOccu, short AccControl, short OccControl, short OccSet, short OccMax, short OccUp, short OccDown, short AreaFlag)
      {
            CC_AREA_SPC cc = new CC_AREA_SPC();
            cc.scp_number = ScpId;
            cc.area_number = AreaNo;
            cc.multi_occupancy = MultiOccu;
            cc.access_control = AccControl;
            cc.occ_control = OccControl;
            cc.occ_set = OccSet;
            cc.occ_max = OccMax;
            cc.occ_up = OccUp;
            cc.occ_down = OccDown;
            cc.area_flags = AreaFlag;

            bool flag = Send((short)enCfgCmnd.enCcAreaSpc, cc);
            return flag;
      }

      public bool GetAccessAreaStatus(short ScpId, short ComponentId, short Number)
      {
            CC_AREASRQ cc = new CC_AREASRQ();
            cc.scp_number = ScpId;
            cc.first = ComponentId;
            cc.count = Number;

            bool flag = Send((short)enCfgCmnd.enCcAreaSrq, cc);
            return flag;
      }
}
