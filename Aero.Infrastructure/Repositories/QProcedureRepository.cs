using System;
using Aero.Application.DTOs;
using Aero.Application.Interfaces;
using Aero.Domain.Entities;
using Aero.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Aero.Infrastructure.Repositories;

public class QProcedureRepository(AppDbContext context) : IQProcedureRepository
{
      public async Task<IEnumerable<Mode>> GetActionTypeAsync()
      {
            var dtos = await context.action_type
                .AsNoTracking()
                .Select(x => new Mode 
                {
                    Name = x.name,
                    Value = x.value,
                    Description = x.description,
                }).ToArrayAsync();

            return dtos;
      }

      public async Task<IEnumerable<ProcedureDto>> GetAsync()
      {
             var dtos = await context.procedure
                .AsNoTracking()
                .Select(x => new ProcedureDto
                {
                    // Base
                    ComponentId = x.component_id,
                    Mac = x.trigger.hardware_mac,
                    HardwareName = x.trigger.hardware.name,
                    LocationId = x.location_id,
                    IsActive = x.is_active,

                    // Detail
                    Name = x.name,
                    Actions = x.actions
                .Select(en => new ActionDto
                {
                    // Base
                    ComponentId = en.component_id,
                    Mac = x.trigger.hardware_mac,
                    LocationId = en.location_id,
                    IsActive = en.is_active,

                    // Detail
                    ScpId = en.scp_id,
                    ActionType = en.action_type,
                    ActionTypeDesc = en.action_type_desc,
                    Arg1 = en.arg1,
                    Arg2 = en.arg2,
                    Arg3 = en.arg3,
                    Arg4 = en.arg4,
                    Arg5 = en.arg5,
                    Arg6 = en.arg6,
                    Arg7 = en.arg7,
                    StrArg = en.str_arg,
                    DelayTime = en.delay_time,
                })
                .ToList()


                })
                .ToArrayAsync();

            return dtos;
      }

      public Task<ProcedureDto> GetByComponentIdAsync(short componentId)
      {
            throw new NotImplementedException();
      }

      public async Task<IEnumerable<ProcedureDto>> GetByLocationIdAsync(short locationId)
      {
            var dtos = await context.procedure
                .AsNoTracking()
                .Where(x => x.location_id == locationId)
                .Select(x => new ProcedureDto
                {
                    // Base
                    ComponentId = x.component_id,
                    Mac = x.trigger.hardware_mac,
                    HardwareName = x.trigger.hardware.name,
                    LocationId = x.location_id,
                    IsActive = x.is_active,

                    // Detail
                    Name = x.name,
                    Actions = x.actions
                .Select(en => new ActionDto
                {
                    // Base
                    ComponentId = en.component_id,
                    Mac = x.trigger.hardware_mac,
                    LocationId = en.location_id,
                    IsActive = en.is_active,

                    // Detail
                    ScpId = en.scp_id,
                    ActionType = en.action_type,
                    ActionTypeDesc = en.action_type_desc,
                    Arg1 = en.arg1,
                    Arg2 = en.arg2,
                    Arg3 = en.arg3,
                    Arg4 = en.arg4,
                    Arg5 = en.arg5,
                    Arg6 = en.arg6,
                    Arg7 = en.arg7,
                    StrArg = en.str_arg,
                    DelayTime = en.delay_time,
                })
                .ToList()


                })
                .ToArrayAsync();

            return dtos;
      }

      public async Task<short> GetLowestUnassignedNumberAsync(int max,string mac)
      {
            if (string.IsNullOrEmpty(mac))
            {
                  if (max <= 0) return -1;

                  var query = context.procedure
                      .AsNoTracking()
                      .Select(x => x.component_id);

                  // Handle empty table case quickly
                  var hasAny = await query.AnyAsync();
                  if (!hasAny)
                        return 1; // start at 1 if table is empty

                  // Load all numbers into memory (only the column, so it's lightweight)
                  var numbers = await query.Distinct().OrderBy(x => x).ToListAsync();

                  short expected = 1;
                  foreach (var num in numbers)
                  {
                        if (num != expected)
                              return expected; // found the lowest missing number
                        expected++;
                  }

                  // If none missing in sequence, return next number
                  if (expected > max) return -1;
                  return expected;
            }
            else
            {
                   if (max <= 0) return -1;

                  var query = context.procedure
                      .AsNoTracking()
                      .Where(x => x.mac == mac)
                      .Select(x => x.proc_id);

                  // Handle empty table case quickly
                  var hasAny = await query.AnyAsync();
                  if (!hasAny)
                        return 1; // start at 1 if table is empty

                  // Load all numbers into memory (only the column, so it's lightweight)
                  var numbers = await query.Distinct().OrderBy(x => x).ToListAsync();

                  short expected = 1;
                  foreach (var num in numbers)
                  {
                        if (num != expected)
                              return expected; // found the lowest missing number
                        expected++;
                  }

                  // If none missing in sequence, return next number
                  if (expected > max) return -1;
                  return expected;
            }
      }

      public Task<bool> IsAnyByComponentId(short component)
      {
            throw new NotImplementedException();
      }
}
