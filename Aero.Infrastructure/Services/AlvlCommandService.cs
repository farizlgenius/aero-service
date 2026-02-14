using Aero.Application.DTOs;
using Aero.Application.Interfaces;
using Aero.Domain.Entities;
using Aero.Domain.Interface;
using Aero.Domain.Interfaces;
using HID.Aero.ScpdNet.Wrapper;
using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Aero.Infrastructure.Services;

public sealed class AlvlCommandService(ICmndRepository cmnd,IQHwRepository qHw) : BaseAeroCommand, IAlvlCommand
{
      public async Task<bool> AccessLevelConfigurationExtended(short ScpId, short component, short tzAcr)
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
        if (flag)
        {
            await cmnd.AddAsync(new CommandAudit
            {
                TagNo = SCPDLL.scpGetTagLastPosted(ScpId),
                ScpId = ScpId,
                Mac = await qHw.GetMacFromComponentAsync(ScpId),
                Command = enCfgCmnd.enCcAlvlEx.ToString(),
                IsPending = true,
                IsSuccess = false,
                NakReason = "",
                NakDescCode = 0,
                LoationId = await qHw.GetLocationIdFromMacAsync(await qHw.GetMacFromComponentAsync(ScpId))
            });
        }
        else
        {
            await cmnd.AddAsync(new CommandAudit
            {
                TagNo = -1,
                ScpId = ScpId,
                Mac = await qHw.GetMacFromComponentAsync(ScpId),
                Command = enCfgCmnd.enCcAlvlEx.ToString(),
                IsPending = false,
                IsSuccess = false,
                NakReason = "",
                NakDescCode = 0,
                LoationId = await qHw.GetLocationIdFromMacAsync(await qHw.GetMacFromComponentAsync(ScpId))
            });

        }
        return flag;
        }

    public async Task<bool> AccessLevelConfigurationExtended(short ScpId, short number, AccessLevel data)
    {
        CC_ALVL_EX cc = new CC_ALVL_EX();
        cc.lastModified = 0;
        cc.scp_number = ScpId;
        cc.alvl_number = number;
        foreach (var d in data.Components)
        {
            cc.tz[d.AcrId] = d.TimezoneId;
        }

        bool flag = Send((short)enCfgCmnd.enCcAlvlEx, cc);
        if (flag)
        {
            await cmnd.AddAsync(new CommandAudit
            {
                TagNo = SCPDLL.scpGetTagLastPosted(ScpId),
                ScpId = ScpId,
                Mac = await qHw.GetMacFromComponentAsync(ScpId),
                Command = enCfgCmnd.enCcAlvlEx.ToString(),
                IsPending = true,
                IsSuccess = false,
                NakReason = "",
                NakDescCode = 0,
                LoationId = await qHw.GetLocationIdFromMacAsync(await qHw.GetMacFromComponentAsync(ScpId))
            });
        }
        else
        {
            await cmnd.AddAsync(new CommandAudit
            {
                TagNo = -1,
                ScpId = ScpId,
                Mac = await qHw.GetMacFromComponentAsync(ScpId),
                Command = enCfgCmnd.enCcAlvlEx.ToString(),
                IsPending = false,
                IsSuccess = false,
                NakReason = "",
                NakDescCode = 0,
                LoationId = await qHw.GetLocationIdFromMacAsync(await qHw.GetMacFromComponentAsync(ScpId))
            });

        }
        return flag;
    }

    public async Task<bool> AccessLevelConfigurationExtendedCreate(short ScpId, short number, List<AccessLevelComponent> component)
      {
            CC_ALVL_EX cc = new CC_ALVL_EX();
            cc.lastModified = 0;
            cc.scp_number = ScpId;
            cc.alvl_number = number;
            foreach (var d in component)
            {
                cc.tz[d.AcrId] = d.TimezoneId;
            }

            bool flag = Send((short)enCfgCmnd.enCcAlvlEx, cc);
        if (flag)
        {
            await cmnd.AddAsync(new CommandAudit
            {
                TagNo = SCPDLL.scpGetTagLastPosted(ScpId),
                ScpId = ScpId,
                Mac = await qHw.GetMacFromComponentAsync(ScpId),
                Command = enCfgCmnd.enCcAlvlEx.ToString(),
                IsPending = true,
                IsSuccess = false,
                NakReason = "",
                NakDescCode = 0,
                LoationId = await qHw.GetLocationIdFromMacAsync(await qHw.GetMacFromComponentAsync(ScpId))
            });
        }
        else
        {
            await cmnd.AddAsync(new CommandAudit
            {
                TagNo = -1,
                ScpId = ScpId,
                Mac = await qHw.GetMacFromComponentAsync(ScpId),
                Command = enCfgCmnd.enCcAlvlEx.ToString(),
                IsPending = false,
                IsSuccess = false,
                NakReason = "",
                NakDescCode = 0,
                LoationId = await qHw.GetLocationIdFromMacAsync(await qHw.GetMacFromComponentAsync(ScpId))
            });

        }
        return flag;
      }

   
}
