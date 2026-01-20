using System;
using Aero.Application.DTOs;
using Aero.Application.Helpers;
using Aero.Application.Interfaces;
using HID.Aero.ScpdNet.Wrapper;

namespace Aero.Infrastructure.Services;

public class TzCommandService : BaseAeroCommand, ITzCommand
{
  public bool ExtendedTimeZoneActSpecificationAsync(short ScpId, TimeZoneDto dto, List<IntervalDto> intervals, int activeTime, int deactiveTime)
  {
    CC_SCP_TZEX_ACT cc = new CC_SCP_TZEX_ACT();
    cc.lastModified = 0;
    cc.nScpID = ScpId;
    cc.number = dto.ComponentId;
    cc.mode = dto.Mode;
    cc.actTime = activeTime;
    cc.deactTime = deactiveTime;
    cc.intervals = (short)intervals.Count;
    if (intervals.Count > 0)
    {
      int i = 0;
      foreach (var interval in intervals)
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
}
