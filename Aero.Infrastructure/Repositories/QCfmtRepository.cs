using System;
using Aero.Application.DTOs;
using Aero.Application.Interfaces;
using Aero.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Aero.Infrastructure.Repositories;

public class QCfmtRepository(AppDbContext context) : IQCfmtRepository
{
      public async Task<int> CountByUpdateTimeAsync(DateTime sync)
      {
           var res = await context.card_format
           .AsNoTracking()
           .Where(x => x.updated_date > sync)
           .CountAsync();

           return res;
      }

      public async Task<IEnumerable<CardFormatDto>> GetAsync()
      {
            var res = await context.card_format
            .AsNoTracking()
            .OrderBy(x => x.component_id)
            .Select(x => new CardFormatDto
            {
                // Baes 
                IsActive = x.is_active,

                // extend_desc
                Name = x.name,
                ComponentId = x.component_id,
                Facility = x.facility,
                Bits = x.bits,
                PeLn = x.pe_ln,
                PeLoc = x.pe_loc,
                PoLn = x.po_ln,
                PoLoc = x.po_loc,
                FcLn = x.fc_ln,
                FcLoc = x.fc_loc,
                ChLn = x.ch_ln,
                ChLoc = x.ch_loc,
                IcLn = x.ic_ln,
                IcLoc = x.ic_loc,

            })
            .ToArrayAsync();

            return res;
      }

      public async Task<CardFormatDto> GetByComponentIdAsync(short componentId)
      {
            var res = await context.card_format
            .AsNoTracking()
            .Where(x => x.component_id == componentId)
            .OrderBy(x => x.component_id)
            .Select(x => new CardFormatDto
            {
                // Baes 
                IsActive = x.is_active,

                // extend_desc
                Name = x.name,
                ComponentId = x.component_id,
                Facility = x.facility,
                Bits = x.bits,
                PeLn = x.pe_ln,
                PeLoc = x.pe_loc,
                PoLn = x.po_ln,
                PoLoc = x.po_loc,
                FcLn = x.fc_ln,
                FcLoc = x.fc_loc,
                ChLn = x.ch_ln,
                ChLoc = x.ch_loc,
                IcLn = x.ic_ln,
                IcLoc = x.ic_loc,

            })
            .FirstOrDefaultAsync();

            return res;
      }

      public async Task<IEnumerable<CardFormatDto>> GetByLocationIdAsync(short locationId)
      {
             var res = await context.card_format
            .AsNoTracking()
            .Where(x => x.location_id == locationId)
            .OrderBy(x => x.component_id)
            .Select(x => new CardFormatDto
            {
                // Baes 
                IsActive = x.is_active,

                // extend_desc
                Name = x.name,
                ComponentId = x.component_id,
                Facility = x.facility,
                Bits = x.bits,
                PeLn = x.pe_ln,
                PeLoc = x.pe_loc,
                PoLn = x.po_ln,
                PoLoc = x.po_loc,
                FcLn = x.fc_ln,
                FcLoc = x.fc_loc,
                ChLn = x.ch_ln,
                ChLoc = x.ch_loc,
                IcLn = x.ic_ln,
                IcLoc = x.ic_loc,

            })
            .ToArrayAsync();

            return res;
      }

      public async Task<short> GetLowestUnassignedNumberAsync(int max,string mac)
      {
            if (max <= 0) return -1;

            var query = context.card_format
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

      public async Task<bool> IsAnyByComponentId(short component)
      {
            return await context.card_format.AnyAsync(x => x.component_id == component);
      }
}
