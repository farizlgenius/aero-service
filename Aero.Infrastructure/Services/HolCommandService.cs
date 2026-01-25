using System;
using Aero.Application.DTOs;
using Aero.Application.Interfaces;
using Aero.Domain.Entities;
using Aero.Domain.Interfaces;
using HID.Aero.ScpdNet.Wrapper;

namespace Aero.Infrastructure.Services;

public class HolCommandService : BaseAeroCommand,IHolCommand
{
       public bool HolidayConfiguration(Holiday dto, short ScpId)
        {
            CC_SCP_HOL cc = new CC_SCP_HOL()
            {
                nScpID = ScpId,
                number = -1,
                year = dto.Year,
                month = dto.Month,
                day = dto.Day,
                //extend = dto.extend,
                type_mask = 1
            };
            bool flag = Send((short)enCfgCmnd.enCcScpHoliday, cc);
            return flag;
        }

        public bool DeleteHolidayConfiguration(Holiday dto, short ScpId)
        {
            CC_SCP_HOL cc = new CC_SCP_HOL()
            {
                nScpID = ScpId,
                number = -1,
                year = dto.Year,
                month = dto.Month,
                day = dto.Day,
                //extend = dto.extend,
                type_mask = 0
            };
            bool flag = Send((short)enCfgCmnd.enCcScpHoliday, cc);
            return flag;
        }


        public bool ClearHolidayConfiguration(short ScpId)
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
            bool flag = Send((short)enCfgCmnd.enCcScpHoliday, cc);
            return flag;

        }
}
