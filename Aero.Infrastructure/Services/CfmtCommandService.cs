using System;
using Aero.Application.DTOs;
using Aero.Application.Interfaces;
using Aero.Domain.Entities;
using Aero.Domain.Interface;
using Aero.Domain.Interfaces;
using HID.Aero.ScpdNet.Wrapper;

namespace Aero.Infrastructure.Services;

public sealed class CfmtCommandService(ICmndRepository cmnd,IQHwRepository qHw) : BaseAeroCommand,ICfmtCommand
{
      public async Task<bool> CardFormatterConfiguration(short ScpId, short FormatNo, short facility, short offset, short function_id, short flags, short bits, short pe_ln, short pe_loc, short po_ln, short po_loc, short fc_ln, short fc_loc, short ch_ln, short ch_loc, short ic_ln, short ic_loc)
      {
            CC_SCP_CFMT cc = new CC_SCP_CFMT();
            cc.lastModified = 0;
            cc.nScpID = ScpId;
            cc.number = FormatNo;
            cc.facility = facility;
            cc.offset = offset;
            cc.function_id = function_id;
            cc.arg.sensor.flags = flags;
            cc.arg.sensor.bits = bits;
            cc.arg.sensor.pe_ln = pe_ln;
            cc.arg.sensor.pe_loc = pe_loc;
            cc.arg.sensor.po_ln = po_ln;
            cc.arg.sensor.po_loc = po_loc;
            cc.arg.sensor.ch_ln = ch_ln;
            cc.arg.sensor.ch_loc = ch_loc;
            cc.arg.sensor.ic_ln = ic_ln;
            cc.arg.sensor.ic_loc = ic_loc;

            bool flag = Send((short)enCfgCmnd.enCcScpCfmt, cc);
        if (flag)
        {
            await cmnd.AddAsync(new CommandAudit
            {
                TagNo = SCPDLL.scpGetTagLastPosted(ScpId),
                ScpId = ScpId,
                Mac = await qHw.GetMacFromComponentAsync(ScpId),
                Command = enCfgCmnd.enCcScpCfmt.ToString(),
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
                Command = enCfgCmnd.enCcScpCfmt.ToString(),
                IsPending = false,
                IsSuccess = false,
                NakReason = "",
                NakDescCode = 0,
                LoationId = await qHw.GetLocationIdFromMacAsync(await qHw.GetMacFromComponentAsync(ScpId))
            });

        }
        return flag;
      }



      public async Task<bool> CardFormatterConfiguration(short ScpId, CardFormat dto, short funtionId)
      {
            CC_SCP_CFMT cc = new CC_SCP_CFMT();
            cc.lastModified = 0;
            cc.nScpID = ScpId;
            cc.number = dto.ComponentId;
            cc.facility = dto.Facility;
            cc.offset = 0;
            cc.function_id = funtionId;
            //cc.arg.sensor.flags = dto.f;
            cc.arg.sensor.bits = dto.Bits;
            cc.arg.sensor.pe_ln = dto.PeLn;
            cc.arg.sensor.pe_loc = dto.PeLoc;
            cc.arg.sensor.po_ln = dto.PoLn;
            cc.arg.sensor.po_loc = dto.PoLoc;
            cc.arg.sensor.ch_ln = dto.ChLn;
            cc.arg.sensor.ch_loc = dto.ChLoc;
            cc.arg.sensor.ic_ln = dto.IcLn;
            cc.arg.sensor.ic_loc = dto.IcLoc;

            bool flag = Send((short)enCfgCmnd.enCcScpCfmt, cc);
        if (flag)
        {
            await cmnd.AddAsync(new CommandAudit
            {
                TagNo = SCPDLL.scpGetTagLastPosted(ScpId),
                ScpId = ScpId,
                Mac = await qHw.GetMacFromComponentAsync(ScpId),
                Command = enCfgCmnd.enCcScpCfmt.ToString(),
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
                Command = enCfgCmnd.enCcScpCfmt.ToString(),
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
