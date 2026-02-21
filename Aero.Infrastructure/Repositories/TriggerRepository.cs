using System;
using Aero.Infrastructure.Mapper;
using Aero.Domain.Entities;
using Aero.Domain.Interface;
using Aero.Infrastructure.Data;
using Aero.Application.DTOs;
using HID.Aero.ScpdNet.Wrapper;
using Microsoft.EntityFrameworkCore;
using Aero.Application.Helpers;

namespace Aero.Infrastructure.Repositories;

public class TriggerRepository(AppDbContext context) : ITriggerRepository
{
      public async Task<int> AddAsync(Trigger data)
      {
            var en = Aero.Infrastructure.Mapper.TriggerMapper.ToEf(data);

            await context.trigger.AddAsync(en);
            return await context.SaveChangesAsync();
      }

      public async Task<int> DeleteByComponentIdAsync(short component)
      {
            throw new NotImplementedException();
      }

      public async Task<IEnumerable<Mode>> GetDeviceBySourceAsync(short location, short source)
      {
            switch (source)
            {
                  case (short)tranSrc.tranSrcScpDiag:
                  case (short)tranSrc.tranSrcScpCom:
                  case (short)tranSrc.tranSrcScpLcl:
                  case (short)tranSrc.tranSrcLoginService:
                        var dtos = await context.hardware
                            .AsNoTracking()
                            .Where(x => x.location_id == location)
                            .Select(x => new Mode
                            {
                                  Name = x.name,
                                  Value = x.component_id,
                                  Description = x.mac
                            })
                            .ToArrayAsync();
                        return dtos;
                  case (short)tranSrc.tranSrcSioDiag:
                  case (short)tranSrc.tranSrcSioCom:
                  case (short)tranSrc.tranSrcSioTmpr:
                  case (short)tranSrc.tranSrcSioPwr:
                        dtos = await context.module
                            .AsNoTracking()
                            .Where(x => x.location_id == location)
                            .Select(x => new Mode
                            {
                                  Name = x.model_desc,
                                  Value = x.component_id,
                                  Description = x.mac
                            })
                            .ToArrayAsync();
                        return dtos;
                  case (short)tranSrc.tranSrcMP:
                        dtos = await context.monitor_point
                            .AsNoTracking()
                            .Where(x => x.location_id == location)
                            .Select(x => new Mode
                            {
                                  Name = x.name,
                                  Value = x.component_id,
                                  Description = x.module.mac
                            })
                            .ToArrayAsync();
                        return dtos;
                  case (short)tranSrc.tranSrcCP:
                        dtos = await context.control_point
                            .AsNoTracking()
                            .Where(x => x.location_id == location)
                            .Select(x => new Mode
                            {
                                  Name = x.name,
                                  Value = x.component_id,
                                  Description = x.module.mac
                            })
                            .ToArrayAsync();
                        return dtos;
                  case (short)tranSrc.tranSrcACR:
                  case (short)tranSrc.tranSrcAcrTmpr:
                  case (short)tranSrc.tranSrcAcrDoor:
                  case (short)tranSrc.tranSrcAcrRex0:
                  case (short)tranSrc.tranSrcAcrRex1:
                  case (short)tranSrc.tranSrcAcrTmprAlt:
                        dtos = await context.door
                            .AsNoTracking()
                            .Where(x => x.location_id == location)
                            .Select(x => new Mode
                            {
                                  Name = x.name,
                                  Value = x.component_id,
                                  Description = x.mac
                            })
                            .ToArrayAsync();
                        return dtos;
                  case (short)tranSrc.tranSrcTimeZone:
                        dtos = await context.timezone
                            .AsNoTracking()
                            .Where(x => x.location_id == location)
                            .Select(x => new Mode
                            {
                                  Name = x.name,
                                  Value = x.component_id,
                                  Description = ""
                            })
                            .ToArrayAsync();
                        return dtos;
                  case (short)tranSrc.tranSrcProcedure:
                        dtos = await context.procedure
                            .AsNoTracking()
                            .Where(x => x.location_id == location)
                            .Select(x => new Mode
                            {
                                  Name = x.name,
                                  Value = x.component_id,
                                  Description = x.trigger.hardware_mac
                            })
                            .ToArrayAsync();
                        return dtos;
                  case (short)tranSrc.tranSrcTrigger:
                  case (short)tranSrc.tranSrcTrigVar:
                        dtos = await context.trigger
                            .AsNoTracking()
                            .Where(x => x.location_id == location)
                            .Select(x => new Mode
                            {
                                  Name = x.name,
                                  Value = x.component_id,
                                  Description = x.hardware_mac
                            })
                            .ToArrayAsync();
                        return dtos;
                  case (short)tranSrc.tranSrcMPG:
                        dtos = await context.monitor_group
                            .AsNoTracking()
                            .Where(x => x.location_id == location)
                            .Select(x => new Mode
                            {
                                  Name = x.name,
                                  Value = x.component_id,
                                  Description = x.mac
                            })
                            .ToArrayAsync();
                        return dtos;
                  case (short)tranSrc.tranSrcArea:
                        dtos = await context.area
                            .AsNoTracking()
                            .Where(x => x.location_id == location)
                            .Select(x => new Mode
                            {
                                  Name = x.name,
                                  Value = x.component_id,
                                  Description = ""
                            })
                            .ToArrayAsync();
                        return dtos;
                  default:
                        return new List<Mode>();
            }
      }

      public Task<int> UpdateAsync(Trigger newData)
      {
            throw new NotImplementedException();
      }

      Task<IEnumerable<Mode>> ITriggerRepository.GetDeviceBySourceAsync(short location, short source)
      {
            throw new NotImplementedException();
      }
}
