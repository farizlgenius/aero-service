using System;
using Aero.Application.DTOs;
using Aero.Application.Interfaces;
using Aero.Domain.Entities;
using Aero.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Aero.Infrastructure.Repositories;

public class QHolderRepository(AppDbContext context,IFileStorage file) : IQHolderRepository
{
      public async Task<IEnumerable<CardHolderDto>> GetAsync()
      {
        
            var res = await context.cardholder
            .AsNoTracking()
            .OrderBy(x => x.created_date)
            .Select(c => new CardHolderDto
            {
                  // Base
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
                  Additionals = c.additionals
                .Where(x => x.holder_id == c.user_id)
                .Select(x => x.additional).ToList(),
                  Credentials = c.credentials
                .Select(c => new CredentialDto
                {
                      // Base
                      LocationId = c.location_id,
                      IsActive = c.is_active,

                      // extend_desc
                      ComponentId = c.component_id,
                      Bits = c.bits,
                      IssueCode = c.issue_code,
                      FacilityCode = c.fac_code,
                      CardNo = c.card_no,
                      Pin = c.pin,
                      ActiveDate = c.active_date,
                      DeactiveDate = c.deactive_date,
                      //card_holder = entity.card_holder is not null ? CardHolderToDto(entity.card_holder) : null,

                }).ToList(),
                AccessLevels = c.cardholder_access_levels.Select(x => new AccessLevelDto
                {
                    ComponentId = x.accessLevel.component_id,
                    LocationId = x.accessLevel.location_id,
                    IsActive = x.accessLevel.is_active,
                    Name = x.accessLevel.name,
                    Components = x.accessLevel.components.Select(x => new AccessLevelComponentDto
                    {
                        Mac = x.mac,
                        DoorId = x.door_id,
                        AcrId = x.acr_id,
                        TimezoneId = x.timezone_id,
                        AlvlId = x.alvl_id
                    }).ToList()

                }).ToList()

            })
            .ToArrayAsync();

            return res;
      }

      public async Task<CardHolderDto> GetByComponentIdAsync(short componentId)
      {
             var res = await context.cardholder
            .AsNoTracking()
            .Where(x => x.component_id == componentId)
            .OrderBy(x => x.created_date)
            .Select(c => new CardHolderDto
            {
                  // Base
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
                  Additionals = c.additionals
                .Where(x => x.holder_id == c.user_id)
                .Select(x => x.additional).ToList(),
                  Credentials = c.credentials
                .Select(c => new CredentialDto
                {
                      // Base
                      LocationId = c.location_id,
                      IsActive = c.is_active,

                      // extend_desc
                      ComponentId = c.component_id,
                      Bits = c.bits,
                      IssueCode = c.issue_code,
                      FacilityCode = c.fac_code,
                      CardNo = c.card_no,
                      Pin = c.pin,
                      ActiveDate = c.active_date,
                      DeactiveDate = c.deactive_date,
                      //card_holder = entity.card_holder is not null ? CardHolderToDto(entity.card_holder) : null,

                }).ToList(),
                AccessLevels = c.cardholder_access_levels.Select(x => new AccessLevelDto
                {
                    ComponentId = x.accessLevel.component_id,
                    LocationId = x.accessLevel.location_id,
                    IsActive = x.accessLevel.is_active,
                    Name = x.accessLevel.name,
                    Components = x.accessLevel.components.Select(x => new AccessLevelComponentDto
                    {

                        Mac = x.mac,
                        DoorId = x.door_id,
                        AcrId = x.acr_id,
                        TimezoneId = x.timezone_id,
                        AlvlId = x.alvl_id
                    }).ToList()

                }).ToList()

            })
            .FirstOrDefaultAsync();

            return res;
      }

      public async Task<IEnumerable<CardHolderDto>> GetByLocationIdAsync(short locationId)
      {
             var res = await context.cardholder
            .AsNoTracking()
            .Where(x => x.location_id == locationId || x.location_id == 1)
            .OrderBy(x => x.created_date)
            .Select(c => new CardHolderDto
            {
                  // Base
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
                  Additionals = c.additionals
                .Where(x => x.holder_id == c.user_id)
                .Select(x => x.additional).ToList(),
                  Credentials = c.credentials
                .Select(c => new CredentialDto
                {
                      // Base
                      LocationId = c.location_id,
                      IsActive = c.is_active,

                      // extend_desc
                      ComponentId = c.component_id,
                      Bits = c.bits,
                      IssueCode = c.issue_code,
                      FacilityCode = c.fac_code,
                      CardNo = c.card_no,
                      Pin = c.pin,
                      ActiveDate = c.active_date,
                      DeactiveDate = c.deactive_date,
                      //card_holder = entity.card_holder is not null ? CardHolderToDto(entity.card_holder) : null,

                }).ToList(),
                AccessLevels = c.cardholder_access_levels.Select(x => new AccessLevelDto
                {
                    ComponentId = x.accessLevel.component_id,
                    LocationId = x.accessLevel.location_id,
                    IsActive = x.accessLevel.is_active,
                    Name = x.accessLevel.name,
                    Components = x.accessLevel.components.Select(x => new AccessLevelComponentDto
                    {
                        Mac = x.mac,

                        DoorId = x.door_id,
                        AcrId = x.acr_id,
                        TimezoneId = x.timezone_id,
                        AlvlId = x.alvl_id
                    }).ToList()

                }).ToList()

            })
            .ToArrayAsync();

            return res;
      }

      public async Task<CardHolderDto> GetByUserIdAsync(string UserId)
      {
             var res = await context.cardholder
            .AsNoTracking()
            .Where(x => x.user_id.Equals(UserId))
            .OrderBy(x => x.created_date)
            .Select(c => new CardHolderDto
            {
                  // Base
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
                  Additionals = c.additionals
                .Where(x => x.holder_id == c.user_id)
                .Select(x => x.additional).ToList(),
                  Credentials = c.credentials
                .Select(c => new CredentialDto
                {
                      // Base
                      LocationId = c.location_id,
                      IsActive = c.is_active,

                      // extend_desc
                      ComponentId = c.component_id,
                      Bits = c.bits,
                      IssueCode = c.issue_code,
                      FacilityCode = c.fac_code,
                      CardNo = c.card_no,
                      Pin = c.pin,
                      ActiveDate = c.active_date,
                      DeactiveDate = c.deactive_date,
                      //card_holder = entity.card_holder is not null ? CardHolderToDto(entity.card_holder) : null,

                }).ToList(),
                AccessLevels = c.cardholder_access_levels.Select(x => new AccessLevelDto
                {
                    ComponentId = x.accessLevel.component_id,
                    LocationId = x.accessLevel.location_id,
                    IsActive = x.accessLevel.is_active,
                    Name = x.accessLevel.name,
                    Components = x.accessLevel.components.Select(x => new AccessLevelComponentDto
                    {
                        Mac = x.mac,
                        DoorId = x.door_id,
                        AcrId = x.acr_id,
                        TimezoneId = x.timezone_id,
                        AlvlId = x.alvl_id
                    }).ToList()

                }).ToList()

            })
            .FirstOrDefaultAsync();

            return res;
      }

      public async Task<short> GetLowestUnassignedNumberAsync(int max,string mac)
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

      public async Task<IEnumerable<string>> GetMacsRelateCredentialByUserIdAsync(string UserId)
      {
            // var m = await context.credential
            // .Where(x => x.cardholder_id.Equals(UserId)).Select(x => x.)
            throw new NotImplementedException();
      }

    public async Task<Pagination<CardHolderDto>> GetPaginationAsync(PaginationParamsWithFilter param, short location)
    {

        var query = context.cardholder.AsNoTracking().AsQueryable();


        if (!string.IsNullOrWhiteSpace(param.Search))
        {
            if (!string.IsNullOrWhiteSpace(param.Search))
            {
                var search = param.Search.Trim();

                if (context.Database.IsNpgsql())
                {
                    var pattern = $"%{search}%";

                    query = query.Where(x =>
                        EF.Functions.ILike(x.user_id, pattern) ||
                        EF.Functions.ILike(x.title, pattern) ||
                        EF.Functions.ILike(x.first_name, pattern) ||
                        EF.Functions.ILike(x.middle_name, pattern) ||
                        EF.Functions.ILike(x.last_name, pattern) ||
                        EF.Functions.ILike(x.sex, pattern) ||
                        EF.Functions.ILike(x.email, pattern) ||
                        EF.Functions.ILike(x.phone, pattern) ||
                        EF.Functions.ILike(x.company, pattern) ||
                        EF.Functions.ILike(x.department, pattern) ||
                        EF.Functions.ILike(x.position, pattern) 

                    );
                }
                else // SQL Server
                {
                    query = query.Where(x =>
                        x.user_id.Contains(search) ||
                        x.title.Contains(search) ||
                        x.first_name.Contains(search) ||
                        x.middle_name.Contains(search) ||
                        x.last_name.Contains(search) ||
                        x.sex.Contains(search) ||
                        x.email.Contains(search) ||
                        x.phone.Contains(search) ||
                        x.company.Contains(search) ||
                        x.department.Contains(search) ||
                        x.position.Contains(search) 
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
            .Select(c => new CardHolderDto
            {
                // Base
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
                Additionals = c.additionals
                .Where(x => x.holder_id == c.user_id)
                .Select(x => x.additional).ToList(),
                Credentials = c.credentials
                .Select(c => new CredentialDto
                {
                    // Base
                    LocationId = c.location_id,
                    IsActive = c.is_active,

                    // extend_desc
                    ComponentId = c.component_id,
                    Bits = c.bits,
                    IssueCode = c.issue_code,
                    FacilityCode = c.fac_code,
                    CardNo = c.card_no,
                    Pin = c.pin,
                    ActiveDate = c.active_date,
                    DeactiveDate = c.deactive_date,
                    //card_holder = entity.card_holder is not null ? CardHolderToDto(entity.card_holder) : null,

                }).ToList(),
                AccessLevels = c.cardholder_access_levels.Select(x => new AccessLevelDto 
                {
                    ComponentId = x.accessLevel.component_id,
                    LocationId = x.accessLevel.location_id,
                    IsActive = x.accessLevel.is_active,
                    Name = x.accessLevel.name,
                    Components = x.accessLevel.components.Select(x => new AccessLevelComponentDto 
                    {
                        Mac = x.mac,
                        DoorId = x.door_id,
                        AcrId = x.acr_id,
                        TimezoneId = x.timezone_id,
                        AlvlId = x.alvl_id
                    }).ToList()
                    
                }).ToList()

            })
            .ToListAsync();


        return new Pagination<CardHolderDto>
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

    public Task<bool> IsAnyByComponentId(short component)
      {
            throw new NotImplementedException();
      }

      public async Task<bool> IsAnyByUserId(string userid)
      {
            return await context.cardholder.AnyAsync(x => x.user_id.Equals(userid));
      }
}
