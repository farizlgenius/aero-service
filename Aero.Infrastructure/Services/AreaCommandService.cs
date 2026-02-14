using System;
using Aero.Application.Interfaces;
using Aero.Domain.Entities;
using Aero.Domain.Interface;
using HID.Aero.ScpdNet.Wrapper;

namespace Aero.Infrastructure.Services;

public class AreaCommandService(ICmndRepository cmnd,IQHwRepository qHw) : BaseAeroCommand, IAreaCommand
{
      public async Task<bool> ConfigureAccessArea(short ScpId, short AreaNo, short MultiOccu, short AccControl, short OccControl, short OccSet, short OccMax, short OccUp, short OccDown, short AreaFlag)
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
        if (flag)
        {
            await cmnd.AddAsync(new CommandAudit
            {
                TagNo = SCPDLL.scpGetTagLastPosted(ScpId),
                ScpId = ScpId,
                Mac = await qHw.GetMacFromComponentAsync(ScpId),
                Command = enCfgCmnd.enCcAreaSrq.ToString(),
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
                Command = enCfgCmnd.enCcAreaSrq.ToString(),
                IsPending = false,
                IsSuccess = false,
                NakReason = "",
                NakDescCode = 0,
                LoationId = await qHw.GetLocationIdFromMacAsync(await qHw.GetMacFromComponentAsync(ScpId))
            });

        }
        return flag;
      }

      public async Task<bool> GetAccessAreaStatus(short ScpId, short ComponentId, short Number)
      {
            CC_AREASRQ cc = new CC_AREASRQ();
            cc.scp_number = ScpId;
            cc.first = ComponentId;
            cc.count = Number;

            bool flag = Send((short)enCfgCmnd.enCcAreaSrq, cc);
        if (flag)
        {
            await cmnd.AddAsync(new CommandAudit
            {
                TagNo = SCPDLL.scpGetTagLastPosted(ScpId),
                ScpId = ScpId,
                Mac = await qHw.GetMacFromComponentAsync(ScpId),
                Command = enCfgCmnd.enCcAreaSrq.ToString(),
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
                Command = enCfgCmnd.enCcAreaSrq.ToString(),
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
