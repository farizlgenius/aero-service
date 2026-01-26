using System;
using Aero.Application.DTOs;
using Aero.Application.Interfaces;
using Aero.Domain.Entities;
using Aero.Domain.Interfaces;
using HID.Aero.ScpdNet.Wrapper;

namespace Aero.Infrastructure.Services;

public sealed class AlvlCommandService : BaseAeroCommand,IAlvlCommand
{
      public bool AccessLevelConfigurationExtended(short ScpId, short component, short tzAcr)
        {
            CC_ALVL_EX cc = new CC_ALVL_EX();
            cc.lastModified = 0;
            cc.scp_number = ScpId;
            cc.alvl_number = component;
            for (int i = 0; i < cc.tz.Length; i++)
            {

                cc.tz[i] = tzAcr;

            }

            bool flag = Send((short)enCfgCmnd.enCcAlvlEx, cc);
            return flag;
        }


      public bool AccessLevelConfigurationExtendedCreate(short ScpId, short number, List<CreateUpdateAccessLevelDoorTimeZone> accessLevelDoorTimeZoneDto)
      {
            CC_ALVL_EX cc = new CC_ALVL_EX();
            cc.lastModified = 0;
            cc.scp_number = ScpId;
            cc.alvl_number = number;
            foreach (var d in accessLevelDoorTimeZoneDto)
            {
                cc.tz[d.AcrId] = d.TimezoneId;
            }

            bool flag = Send((short)enCfgCmnd.enCcAlvlEx, cc);
            return flag;
      }
}
