using System;
using Aero.Application.DTOs;
using Aero.Application.Interfaces;
using Aero.Domain.Entities;
using Aero.Domain.Interface;
using Aero.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Aero.Infrastructure.Repositories;

public sealed class QCredRepository(AppDbContext context) : IQCredRepository
{
      public async Task<IEnumerable<CredentialDto>> GetAsync()
      {
            var res = await context.credential
            .AsNoTracking()
            .Select(entity => new CredentialDto
            {
                // Base
                Uuid = entity.uuid,
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
                Uuid = entity.uuid,
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
            .Where(x => x.location_id == locationId)
            .Select(entity => new CredentialDto
            {
                // Base
                Uuid = entity.uuid,
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

      public async Task<short> GetLowestUnassignedIssueCodeByUserIdAsync(int max,string UserId)
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

      public async Task<short> GetLowestUnassignedNumberAsync(int max)
      {
             if (max <= 0) return -1;

            var query = context.credential
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
            return await context.credential.AnyAsync(x => x.component_id == component);
      }

      public async Task<bool> IsAnyWithCardNumberAsync(long cardno)
      {
            return await context.credential.AsNoTracking().AnyAsync(x => x.card_no == cardno);
      }
}
