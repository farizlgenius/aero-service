using System;
using Aero.Application.DTOs;
using Aero.Application.Interfaces;
using Aero.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Aero.Infrastructure.Repositories;

public class QTrigRepository(AppDbContext context) : IQTrigRepository
{
      public async Task<int> CountByMacAndUpdateTimeAsync(string mac, DateTime sync)
      {
            var res = await context.trigger
            .AsNoTracking()
            .Where(x => x.hardware_mac.Equals(mac) && x.updated_date > sync)
            .CountAsync();

            return res;
      }

      public async Task<IEnumerable<TriggerDto>> GetAsync()
      {
            var dtos = await context.trigger
            .AsNoTracking()
            .OrderBy(x => x.component_id)
            .Select(en => new TriggerDto
            {
                  // Base
                  LocationId = en.location_id,
                  Mac = en.hardware_mac,
                  HardwareName = en.hardware.name,
                  IsActive = en.is_active,

                  // Detail
                  Name = en.name,
                  Command = en.command,
                  ProcedureId = en.procedure_id,
                  SourceNumber = en.source_number,
                  SourceType = en.source_type,
                  TranType = en.tran_type,
                  CodeMap = en.code_map.Select(x => new TransactionCodeDto
                  {
                        Name = x.name,
                        Value = x.value,
                        Description = x.description
                  }).ToList(),
                  TimeZone = en.timezone,
            }).ToArrayAsync();

            return dtos;
      }

      public async Task<TriggerDto> GetByComponentIdAsync(short componentId)
      {
            var dtos = await context.trigger
            .AsNoTracking()
            .Where(x => x.component_id == componentId)
            .OrderBy(x => x.component_id)
            .Select(en => new TriggerDto
            {
                  // Base
                  LocationId = en.location_id,
                  Mac = en.hardware_mac,
                  HardwareName = en.hardware.name,
                  IsActive = en.is_active,

                  // Detail
                  Name = en.name,
                  Command = en.command,
                  ProcedureId = en.procedure_id,
                  SourceNumber = en.source_number,
                  SourceType = en.source_type,
                  TranType = en.tran_type,
                  CodeMap = en.code_map.Select(x => new TransactionCodeDto
                  {
                        Name = x.name,
                        Value = x.value,
                        Description = x.description
                  }).ToList(),
                  TimeZone = en.timezone,
            }).FirstOrDefaultAsync();

            return dtos;
      }

      public async Task<IEnumerable<TriggerDto>> GetByLocationIdAsync(short locationId)
      {
            var dtos = await context.trigger
            .AsNoTracking()
            .Where(x => x.location_id == locationId)
            .OrderBy(x => x.component_id)
            .Select(en => new TriggerDto
            {
                  // Base
                  LocationId = en.location_id,
                  Mac = en.hardware_mac,
                  HardwareName = en.hardware.name,
                  IsActive = en.is_active,

                  // Detail
                  Name = en.name,
                  Command = en.command,
                  ProcedureId = en.procedure_id,
                  SourceNumber = en.source_number,
                  SourceType = en.source_type,
                  TranType = en.tran_type,
                  CodeMap = en.code_map.Select(x => new TransactionCodeDto
                  {
                        Name = x.name,
                        Value = x.value,
                        Description = x.description
                  }).ToList(),
                  TimeZone = en.timezone,
            }).ToArrayAsync();

            return dtos;
      }


      public async Task<short> GetLowestUnassignedNumberAsync(int max,string mac)
      {
            if (string.IsNullOrEmpty(mac))
            {
                  if (max <= 0) return -1;

                  var query = context.transaction
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

                  var query = context.trigger
                      .AsNoTracking()
                      .Where(x => x.mac == mac)
                      .Select(x => x.trig_id);

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
