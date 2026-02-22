using System;
using Aero.Application.DTOs;
using Aero.Application.Interfaces;
using Aero.Domain.Entities;
using Aero.Infrastructure.Data;
using Aero.Infrastructure.Listenser;
using Aero.Infrastructure.Mapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Aero.Infrastructure.Repositories;

public class CredRepository(AppDbContext context,IQHwRepository qHw,IServiceScopeFactory factory) : ICredRepository
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

      public async Task<int> DeleteByComponentIdAsync(short component)
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
        .Select(entity => new CredentialDto
        {
            // Base
            LocationId = entity.location_id,
            IsActive = entity.is_active,

            // extend_desc
            ComponentId = entity.component_id,
            Bits = entity.bits,
            IssueCode = entity.issue_code,
            FacilityCode = entity.fac_code,
            CardNo = entity.card_no,
            Pin = entity.pin,
            ActiveDate = entity.active_date,
            DeactiveDate = entity.deactive_date,
            //card_holder = entity.card_holder is not null ? CardHolderToDto(entity.card_holder) : null,

        }).ToArrayAsync();

        return res;
    }

    public async Task<CredentialDto> GetByComponentIdAsync(short componentId)
    {
        var res = await context.credential
        .AsNoTracking()
        .OrderBy(x => x.component_id)
        .Where(x => x.component_id == componentId)
        .Select(entity => new CredentialDto
        {
            // Base
            LocationId = entity.location_id,
            IsActive = entity.is_active,

            // extend_desc
            ComponentId = entity.component_id,
            Bits = entity.bits,
            IssueCode = entity.issue_code,
            FacilityCode = entity.fac_code,
            CardNo = entity.card_no,
            Pin = entity.pin,
            ActiveDate = entity.active_date,
            DeactiveDate = entity.deactive_date,
            //card_holder = entity.card_holder is not null ? CardHolderToDto(entity.card_holder) : null,

        }).FirstOrDefaultAsync();

        return res;
    }

    public async Task<IEnumerable<CredentialDto>> GetByLocationIdAsync(short locationId)
    {
        var res = await context.credential
        .AsNoTracking()
        .OrderBy(x => x.component_id)
        .Where(x => x.location_id == locationId || x.location_id == 1)
        .Select(entity => new CredentialDto
        {
            // Base
            LocationId = entity.location_id,
            IsActive = entity.is_active,

            // extend_desc
            ComponentId = entity.component_id,
            Bits = entity.bits,
            IssueCode = entity.issue_code,
            FacilityCode = entity.fac_code,
            CardNo = entity.card_no,
            Pin = entity.pin,
            ActiveDate = entity.active_date,
            DeactiveDate = entity.deactive_date,
            //card_holder = entity.card_holder is not null ? CardHolderToDto(entity.card_holder) : null,

        }).ToArrayAsync();

        return res;
    }

    public async Task<IEnumerable<CredentialDto>> GetByUserIdAsync(string UserId)
    {
        var res = await context.credential
        .AsNoTracking()
        .Where(x => x.cardholder_id.Equals(UserId))
        .Select(entity => new CredentialDto
        {
            // Base
            LocationId = entity.location_id,
            IsActive = entity.is_active,

            // extend_desc
            ComponentId = entity.component_id,
            Bits = entity.bits,
            IssueCode = entity.issue_code,
            FacilityCode = entity.fac_code,
            CardNo = entity.card_no,
            Pin = entity.pin,
            ActiveDate = entity.active_date,
            DeactiveDate = entity.deactive_date,
            //card_holder = entity.card_holder is not null ? CardHolderToDto(entity.card_holder) : null,

        }).ToArrayAsync();

        return res;

    }

    public async Task<List<string>> GetCardHolderFullnameAndUserIdByCardNoAsync(double cardno)
    {
        var res = await context.credential.AsNoTracking()
        .OrderBy(x => x.component_id)
        .Where(x => x.card_no == cardno)
        .Select(x => new List<string> { $"{x.cardholder.title} {x.cardholder.first_name} {x.cardholder.middle_name} {x.cardholder.last_name}", x.cardholder_id })
        .FirstOrDefaultAsync() ?? new List<string> { "", "" };

        return res;

    }

    public async Task<IEnumerable<Mode>> GetCredentialFlagAsync()
    {
        var dtos = await context.credential_flag
            .Select(flag => new Mode
            {
                Name = flag.name,
                Description = flag.description,
                Value = flag.value,
            })
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


    public async Task<short> GetLowestUnassignedNumberAsync(int max, string mac)
    {
        if (string.IsNullOrEmpty(mac))
        {
            if (max <= 0) return -1;

            var query = context.monitor_point
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

            var query = context.monitor_point
                .AsNoTracking()
                .Where(x => x.mac == mac)
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
    }

    public Task<Pagination<CredentialDto>> GetPaginationAsync(PaginationParamsWithFilter param, short location)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> IsAnyByComponentId(short component)
    {
        return await context.credential.AnyAsync(x => x.component_id == component);
    }

    public async Task<bool> IsAnyWithCardNumberAsync(long cardno)
    {
        return await context.credential.AsNoTracking().AnyAsync(x => x.card_no == cardno);
    }
}
