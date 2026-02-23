using Aero.Application.DTOs;
using Aero.Domain.Entities;
using Aero.Domain.Interface;
using Aero.Infrastructure.Persistences;
using Aero.Infrastructure.Mapper;
using Microsoft.EntityFrameworkCore;
using System;
using Aero.Application.Interface;

namespace Aero.Infrastructure.Repositories;

public class CfmtRepository(AppDbContext context) : ICfmtRepository
{
      public async Task<int> AddAsync(CardFormat data)
      {
           var en = CardFormatMapper.ToEf(data);
           await context.card_format.AddAsync(en);
           return await context.SaveChangesAsync();
      }

      public async Task<int> DeleteByIdAsync(int id)
      {
            var en = await context.card_format
            .Where(x => x.id == id)
            .OrderBy(x => x.id)
            .FirstOrDefaultAsync();

            if(en is null) return 0;

            context.card_format.Remove(en);
            return await context.SaveChangesAsync();
      }

      public async Task<int> UpdateAsync(CardFormat data)
      {
            var en = await context.card_format
            .Where(x => x.id == data.Id)
            .OrderBy(x => x.id)
            .FirstOrDefaultAsync();

            if(en is null) return 0;

        en.Update(data);

            context.card_format.Update(en);
            return await context.SaveChangesAsync();
      }

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
        .OrderBy(x => x.id)
        .Select(c => new CardFormatDto(c.id,c.name,c.facility,c.offset,c.function_id,c.flags,c.bits,c.pe_ln,c.pe_loc,c.po_ln,c.po_loc,c.fc_ln,c.fc_loc,c.ch_ln,c.ch_loc,c.ic_ln,c.ic_loc,c.location_id,c.is_active))
        .ToArrayAsync();

        return res;
    }

    public async Task<CardFormatDto> GetByIdAsync(int id)
    {
        var res = await context.card_format
        .AsNoTracking()
        .Where(x => x.id == id)
        .OrderBy(x => x.id)
        .Select(c => new CardFormatDto(c.id, c.name, c.facility, c.offset, c.function_id, c.flags, c.bits, c.pe_ln, c.pe_loc, c.po_ln, c.po_loc, c.fc_ln, c.fc_loc, c.ch_ln, c.ch_loc, c.ic_ln, c.ic_loc, c.location_id, c.is_active))
        .FirstOrDefaultAsync();

        return res;
    }

    public async Task<IEnumerable<CardFormatDto>> GetByLocationIdAsync(int locationId)
    {
        var res = await context.card_format
       .AsNoTracking()
       .Where(x => x.location_id == locationId || x.location_id == 1)
       .OrderBy(x => x.id)
      .Select(c => new CardFormatDto(c.id, c.name, c.facility, c.offset, c.function_id, c.flags, c.bits, c.pe_ln, c.pe_loc, c.po_ln, c.po_loc, c.fc_ln, c.fc_loc, c.ch_ln, c.ch_loc, c.ic_ln, c.ic_loc, c.location_id, c.is_active))
       .ToArrayAsync();

        return res;
    }

    public async Task<short> GetLowestUnassignedNumberAsync(int max)
    {
        if (max <= 0) return -1;

        var query = context.card_format
            .AsNoTracking()
            .Select(x => x.driver_id);

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

    public async Task<Pagination<CardFormatDto>> GetPaginationAsync(PaginationParamsWithFilter param, int location)
    {

        var query = context.card_format.AsNoTracking().AsQueryable();


        if (!string.IsNullOrWhiteSpace(param.Search))
        {
            if (!string.IsNullOrWhiteSpace(param.Search))
            {
                var search = param.Search.Trim();

                if (context.Database.IsNpgsql())
                {
                    var pattern = $"%{search}%";

                    query = query.Where(x =>
                        EF.Functions.ILike(x.name.ToString(), pattern) ||
                        EF.Functions.ILike(x.facility.ToString(), pattern) ||
                        EF.Functions.ILike(x.bits.ToString(), pattern)

                    );
                }
                else // SQL Server
                {
                    query = query.Where(x =>
                        x.name.ToString().Contains(search) ||
                        x.facility.ToString().Contains(search) ||
                        x.bits.ToString().Contains(search)
                    );
                }
            }
        }

        query = query.Where(x => x.location_id == location || x.location_id == 1);

        if (param.StartDate != null)
        {
            var startUtc = DateTime.SpecifyKind(param.StartDate.Value, DateTimeKind.Utc);
            query = query.Where(x => x.created_date >= startUtc);
        }

        if (param.EndDate != null)
        {
            var endUtc = DateTime.SpecifyKind(param.EndDate.Value, DateTimeKind.Utc);
            query = query.Where(x => x.created_date <= endUtc);
        }

        var count = await query.CountAsync();


        var data = await query
            .AsNoTracking()
            .OrderByDescending(t => t.created_date)
            .Skip((param.PageNumber - 1) * param.PageSize)
            .Take(param.PageSize)
            .Select(c => new CardFormatDto(c.id, c.name, c.facility, c.offset, c.function_id, c.flags, c.bits, c.pe_ln, c.pe_loc, c.po_ln, c.po_loc, c.fc_ln, c.fc_loc, c.ch_ln, c.ch_loc, c.ic_ln, c.ic_loc, c.location_id, c.is_active))
            .ToListAsync();


        return new Pagination<CardFormatDto>
        {
            Data = data,
            Page = new PaginationData
            {
                TotalCount = count,
                PageNumber = param.PageNumber,
                PageSize = param.PageSize,
                TotalPage = (int)Math.Ceiling(count / (double)param.PageSize)
            }
        };
    }

    public async Task<bool> IsAnyById(int id)
    {
        return await context.card_format.AnyAsync(x => x.id == id);
    }

    public Task<short> GetLowestUnassignedNumberByMacAsync(string mac, int max)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> IsAnyByNameAsync(string name)
    {
        return await context.card_format.AnyAsync(x => x.name.Equals(name.Trim()));
    }
}
