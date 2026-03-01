using System;
using Aero.Application.DTOs;
using Aero.Application.Interfaces;
using Aero.Domain.Entities;
using Aero.Infrastructure.Persistences;
using Aero.Infrastructure.Listenser;
using Aero.Infrastructure.Mapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Aero.Application.Interface;

namespace Aero.Infrastructure.Repositories;

public class CredRepository(AppDbContext context,IHwRepository qHw,IServiceScopeFactory factory) : ICredRepository
{
      public async Task<int> AddAsync(Credential data)
      {
            throw new NotImplementedException();
      }

    public async Task ToggleScanCardAsync(ScanCardDto dto)
    {
        using var scope = factory.CreateScope();
        var m = scope.ServiceProvider.GetRequiredService<AeroWorker>();
        m.isWaitingCardScan = true;
        m.ScanScpId = await qHw.GetComponentIdFromMacAsync(dto.Mac);
        m.ScanAcrNo = dto.DoorId;

    }

      public async Task<int> DeleteByCardNoAsync(long Cardno)
      {
            var en = await context.credential
            .Where(x => x.card_no == Cardno)
            .OrderBy(x => x.card_no)
            .FirstOrDefaultAsync();

            if(en is null) return 0;

            context.credential.Remove(en);
            return await context.SaveChangesAsync();
      }

      public async Task<int> DeleteByIdAsync(int id)
      {
            throw new NotImplementedException();
      }

      public async Task<int> UpdateAsync(Credential newData)
      {
            throw new NotImplementedException();
      }

    public async Task<IEnumerable<CredentialDto>> GetAsync()
    {
        var res = await context.credential
        .AsNoTracking()
        .Select(c => new CredentialDto(c.bits,c.issue_code,c.fac_code,c.card_no,c.pin ?? "",c.active_date,c.deactive_date))
        .ToArrayAsync();

        return res;
    }

    public async Task<CredentialDto> GetByIdAsync(int id)
    {
        var res = await context.credential
        .AsNoTracking()
        .OrderBy(x => x.id)
        .Where(x => x.id == id)
        .Select(c => new CredentialDto(c.bits,c.issue_code,c.fac_code,c.card_no,c.pin ?? "",c.active_date,c.deactive_date)).FirstOrDefaultAsync();

        return res;
    }

    public async Task<IEnumerable<CredentialDto>> GetByLocationIdAsync(int locationId)
    {
        var res = await context.credential
        .AsNoTracking()
        .OrderBy(x => x.id)
        .Where(x => x.location_id == locationId || x.location_id == 1)
       .Select(c => new CredentialDto(c.bits,c.issue_code,c.fac_code,c.card_no,c.pin ?? "",c.active_date,c.deactive_date)).ToArrayAsync();

        return res;
    }

    public async Task<IEnumerable<CredentialDto>> GetByUserIdAsync(string UserId)
    {
        var res = await context.credential
        .AsNoTracking()
        .Where(x => x.user_id.Equals(UserId))
       .Select(c => new CredentialDto(c.bits,c.issue_code,c.fac_code,c.card_no,c.pin ?? "",c.active_date,c.deactive_date)).ToArrayAsync();

        return res;

    }

    public async Task<List<string>> GetCardHolderFullnameAndUserIdByCardNoAsync(double cardno)
    {
        var res = await context.credential.AsNoTracking()
        .OrderBy(x => x.id)
        .Where(x => x.card_no == cardno)
        .Select(x => new List<string> { $"{x.user.title} {x.user.first_name} {x.user.middle_name} {x.user.last_name}", x.user_id })
        .FirstOrDefaultAsync() ?? new List<string> { "", "" };

        return res;

    }

    public async Task<IEnumerable<ModeDto>> GetCredentialFlagAsync()
    {
        var dtos = await context.credential_flag
            .Select(m => new ModeDto(m.name,m.value,m.description))
            .ToArrayAsync();

        return dtos;
    }

    public async Task<short> GetLowestUnassignedIssueCodeByUserIdAsync(int max, string UserId)
    {
        if (max <= 0) return -1;

        var query = context.credential
            .AsNoTracking()
            .Select(x => x.issue_code);

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


    public async Task<short> GetLowestUnassignedNumberAsync(int max, int driverid)
    {
         if (max <= 0) return -1;

            var query = context.monitor_point
                .AsNoTracking()
                .Where(x => x.driver_id == driverid)
                .Select(x => x.id);

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

    public Task<Pagination<CredentialDto>> GetPaginationAsync(PaginationParamsWithFilter param, int location)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> IsAnyById(int id)
    {
        return await context.credential.AnyAsync(x => x.id == id);
    }

    public async Task<bool> IsAnyWithCardNumberAsync(long cardno)
    {
        return await context.credential.AsNoTracking().AnyAsync(x => x.card_no == cardno);
    }

      public Task<bool> IsAnyByNameAsync(string name)
      {
            throw new NotImplementedException();
      }
}
