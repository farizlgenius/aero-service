using HID.Aero.ScpdNet.Wrapper;
using AeroService.DTO.Interval;
using AeroService.Entity;
using MiNET.Entities.Passive;
using System;

namespace AeroService.Aero.CommandService.Impl
{
    public sealed class TimeZoneCommandService(AeroCommandService command) : ITimeZoneCommandService
    {

        public async Task<bool> TimeZoneControlAsync(short ScpId, short Component, short Command)
        {
            CC_TZCOMMAND cc = new CC_TZCOMMAND();
            cc.scp_number = ScpId;
            cc.tz_number = Component;
            cc.command = Command;
            var tag = ScpId + "/" + SCPDLL.scpGetTagLastPosted(ScpId) + 1;
            bool flag = command.SendCommand((short)enCfgCmnd.enCcTzCommand, cc);
            //if (flag)
            //{
            //    return await command.TrackCommandAsync(tag, hardware_id, Constants.command.C314);
            //}
            return flag;
        }


    }
}
