using HID.Aero.ScpdNet.Wrapper;
using AeroService.Constants;
using AeroService.Entity;
using System;

namespace AeroService.Aero.CommandService.Impl
{
    public sealed class HolidayCommandService(ILogger<HolidayCommandService> logger,AeroCommandService command) : IHolidayCommandService
    {
        public async Task<bool> HolidayConfigurationAsync(Holiday dto, short ScpId)
        {
            CC_SCP_HOL cc = new CC_SCP_HOL()
            {
                nScpID = ScpId,
                number = -1,
                year = dto.year,
                month = dto.month,
                day = dto.day,
                //extend = dto.extend,
                type_mask = 1
            };
            var tag = ScpId + "/" + SCPDLL.scpGetTagLastPosted(ScpId) + 1;
            bool flag = command.SendCommand((short)enCfgCmnd.enCcScpHoliday, cc);
            //if (flag)
            //{
            //    return await command.TrackCommandAsync(tag, hardware_id, Constants.command.C1104);
            //}
            return flag;
        }

        public async Task<bool> DeleteHolidayConfigurationAsync(Holiday dto, short ScpId)
        {
            CC_SCP_HOL cc = new CC_SCP_HOL()
            {
                nScpID = ScpId,
                number = -1,
                year = dto.year,
                month = dto.month,
                day = dto.day,
                //extend = dto.extend,
                type_mask = 0
            };
            var tag = ScpId + "/" + SCPDLL.scpGetTagLastPosted(ScpId) + 1;
            bool flag = command.SendCommand((short)enCfgCmnd.enCcScpHoliday, cc);
            //if (flag)
            //{
            //    return await command.TrackCommandAsync(tag, hardware_id, Constants.command.C1104);
            //}
            return flag;
        }


        public async Task<bool> ClearHolidayConfigurationAsync(short ScpId)
        {
            CC_SCP_HOL cc = new CC_SCP_HOL()
            {
                nScpID = ScpId,
                number = -1,
                year = 0,
                month = 1,
                day = 1,
                //extend = dto.extend,
                type_mask = 0
            };
            var tag = ScpId + "/" + SCPDLL.scpGetTagLastPosted(ScpId) + 1;
            bool flag = command.SendCommand((short)enCfgCmnd.enCcScpHoliday, cc);
            //if (flag)
            //{
            //    return await command.TrackCommandAsync(tag, hardware_id, Constants.command.C1104);
            //}
            return flag;

        }
    }
}
