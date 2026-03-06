using System;
using Aero.Application.DTOs;
using Aero.Application.Helpers;
using Aero.Application.Interfaces;
using Aero.Domain.Entities;
using Aero.Domain.Interfaces;
using HID.Aero.ScpdNet.Wrapper;

namespace Aero.Infrastructure.Services;

public sealed class TzCommandService : BaseAeroCommand, ITzCommand
{
  public bool ExtendedTimeZoneActSpecification(short ScpId, Domain.Entities.TimeZone domain)
  {
        long active = UtilitiesHelper.DateTimeToElapeSecond(domain.ActiveTime);
        long deactive = UtilitiesHelper.DateTimeToElapeSecond(domain.DeactiveTime);
        CC_SCP_TZEX_ACT cc = new CC_SCP_TZEX_ACT();
    cc.lastModified = 0;
    cc.nScpID = ScpId;
    cc.number = domain.DriverId;
    cc.mode = domain.Mode;
    cc.actTime = (int)active;
    cc.deactTime = (int)deactive;
    cc.intervals = (short)domain.Intervals.Count;
    if (domain.Intervals.Count > 0)
    {
      int i = 0;
      foreach (var interval in domain.Intervals)
      {
        cc.i[i].i_days = (short)UtilitiesHelper.ConvertDayToBinary(interval.Days);
        cc.i[i].i_start = (short)UtilitiesHelper.ConvertTimeToEndMinute(interval.StartTime);
        cc.i[i].i_end = (short)UtilitiesHelper.ConvertTimeToEndMinute(interval.EndTime);
        i++;
      }

    }
    bool flag = Send((short)enCfgCmnd.enCcScpTimezoneExAct, cc);
    return flag;
  }

  public bool TimeZoneControl(short ScpId, short TzNo, short Command)
  {
    CC_TZCOMMAND cc = new CC_TZCOMMAND();
    cc.scp_number = ScpId;
    cc.tz_number = TzNo;
    cc.command = Command;
    bool flag = Send((short)enCfgCmnd.enCcTzCommand, cc);
    return flag;

  }
}
