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
                Uuid = x.uuid,
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

      public Task<CardFormatDto> GetByComponentIdAsync(short componentId)
      {
            throw new NotImplementedException();
      }

      public Task<IEnumerable<CardFormatDto>> GetByLocationIdAsync(short locationId)
      {
            throw new NotImplementedException();
      }

      public Task<short> GetLowestUnassignedNumberAsync(int max)
      {
            throw new NotImplementedException();
      }

      public Task<bool> IsAnyByComponentId(short component)
      {
            throw new NotImplementedException();
      }
}
