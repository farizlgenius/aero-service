using System;
using Aero.Application.DTOs;
using Aero.Application.Interfaces;
using Aero.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Aero.Infrastructure.Repositories;

public class QHolderRepository(AppDbContext context) : IQHolderRepository
{
      public async Task<IEnumerable<CardHolderDto>> GetAsync()
      {
            var res = await context.cardholder
            .AsNoTracking()
            .OrderBy(x => x.created_date)
            .Select(c => new CardHolderDto
            {
                  // Base
                  Uuid = c.uuid,
                  LocationId = c.location_id,
                  IsActive = c.is_active,

                  // extend_desc
                  Flag = c.flag,
                  UserId = c.user_id,
                  Title = c.title,
                  FirstName = c.first_name,
                  MiddleName = c.middle_name,
                  LastName = c.last_name,
                  Sex = c.sex,
                  Email = c.email,
                  Phone = c.phone,
                  Company = c.company,
                  Position = c.position,
                  Department = c.department,
                  ImagePath = c.image_path,
                  Additionals = c.additional
                .Where(x => x.holder_id == c.user_id)
                .Select(x => x.additional).ToList(),
                  Credentials = c.credentials
                .Select(c => new CredentialDto
                {
                      // Base
                      Uuid = c.uuid,
                      LocationId = c.location_id,
                      IsActive = c.is_active,

                      // extend_desc
                      component_id = c.component_id,
                      Bits = c.bits,
                      IssueCode = c.issue_code,
                      FacilityCode = c.fac_code,
                      CardNo = c.card_no,
                      Pin = c.pin,
                      ActiveDate = c.active_date,
                      DeactiveDate = c.deactive_date,
                      //card_holder = entity.card_holder is not null ? CardHolderToDto(entity.card_holder) : null,

                }).ToList(),

            })
            .ToArrayAsync();

            return res;
      }

      public Task<CardHolderDto> GetByComponentIdAsync(short componentId)
      {
            throw new NotImplementedException();
      }

      public Task<IEnumerable<CardHolderDto>> GetByLocationIdAsync(short locationId)
      {
            throw new NotImplementedException();
      }


      public async Task<short> GetLowestUnassignedNumberAsync(int max)
      {
            if (max <= 0) return -1;

            var query = context.cardholder
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

      public Task<bool> IsAnyByComponentId(short component)
      {
            throw new NotImplementedException();
      }
}
