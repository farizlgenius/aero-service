using Aero.Application.DTOs;
using Aero.Application.Helpers;
using Aero.Domain.Entities;
using Aero.Domain.Interface;
using Aero.Infrastructure.Persistences;
using Aero.Infrastructure.Mapper;
using Microsoft.EntityFrameworkCore;
using System;
using Aero.Application.Interface;
using System.ComponentModel;

namespace Aero.Infrastructure.Repositories;

public class HolderRepository(AppDbContext context) : IUserRepository
{
      public async Task<int> AddAsync(User data)
      {
            var en = new Aero.Infrastructure.Persistences.Entities.User(data);
            await context.user.AddAsync(en);
            return await context.SaveChangesAsync();
      }

      public async Task<int> DeleteByIdAsync(int id)
      {
            var en = await context.user
            .Where(x => x.id == id)
            .OrderBy(x => x.id)
            .FirstOrDefaultAsync();

            if(en is null) return 0;

            context.user.Remove(en);
            return await context.SaveChangesAsync();
      }

      public async Task<int> DeleteByUserIdAsync(string UserId)
      {
            var en = await context.user
            .Where(x => x.user_id.Equals(UserId))
            .OrderBy(x => x.id)
            .FirstOrDefaultAsync();

            if(en is null) return 0;

            context.user.Remove(en);
            return await context.SaveChangesAsync();
      }

      public async Task<int> DeleteReferenceByUserIdAsync(string UserId)
      {
           // Additionals
           var additional = await context.user_additional
           .Where(x => x.user_id.Equals(UserId))
           .ToArrayAsync();

            context.user_additional.RemoveRange(additional);

            // Access Level
            var access = await context.user_access_level
            .Where(x => x.user_id.Equals(UserId))
            .ToArrayAsync();

            context.user_access_level.RemoveRange(access);

            // Credentials
            var credential = await context.credential
            .Where(x => x.user_id.Equals(UserId))
            .ToArrayAsync();

            context.credential.RemoveRange(credential);

            return await context.SaveChangesAsync();
      }

      public async Task<int> UpdateAsync(User newData)
      {
            var en = await context.user
            .Where(x => x.user_id.Equals(newData.UserId))
            .OrderBy(x => x.id)
            .FirstOrDefaultAsync();

            if(en is null) return 0;

            // Delete Credentials 

            // Delete link from access level 

            en.Update(newData);

            context.user.Update(en);
            return await context.SaveChangesAsync();
      }

    public async Task<int> UpdateImagePathAsync(string path,string userid)
    {
        var en = await context.user
            .Where(x => x.user_id.Equals(userid))
            .OrderBy(x => x.id)
            .FirstOrDefaultAsync();

        if(en is null) return 0;

        en.SetImage(path);
        context.user.Update(en);
        return await context.SaveChangesAsync();
    }

    public async Task<IEnumerable<UserDto>> GetAsync()
    {

        var data = await context.user
        .AsNoTracking()
        .OrderBy(x => x.created_date)
        .Select(c => new {
            c.user_id,
            c.title,
            c.first_name,
            c.middle_name,
            c.last_name,
            gender = c.gender,
            c.email,
            c.phone,
            c.company_id,
            company = c.company.name,
            c.position_id,
            position = c.position.name,
            c.image,
            c.department_id,
            department = c.department.name,
            c.flag,
            additionals = c.additionals.Select(a => new { a.additional}),
            creds = c.credentials.Select(c => new CredentialDto(
                c.bits,
                c.issue_code,
                c.fac_code,
                c.card_no,
                c.pin,
                c.active_date,
                c.deactive_date
            )),
            accesslevel = c.user_access_levels.Select(a => new
            {
                a.accesslevel_id,
                a.accessLevel.name,
                components = a.accessLevel.components.Select(c => new
                {
                    c.driver_id,
                    c.device_id,
                    c.door_id,
                    c.acr_id,
                    c.timezone_id
                }),
                a.accessLevel.location_id,
                a.accessLevel.is_active

            }),
            c.location_id,
            c.is_active
            })
        .ToArrayAsync();

        var res = data.Select(d => new UserDto(
            d.user_id,
            d.title,
            d.first_name,
            d.middle_name,
            d.last_name,
            (int)d.gender,
            d.email,
            d.phone,
            d.company_id,
            d.company,
            d.position_id,
            d.position,
            d.image,
            d.department_id,
            d.department,
            d.flag,
            d.additionals.Select(x => x.additional).ToList(),
            d.creds.ToList(),
            d.accesslevel.Select(x => new AccessLevelDto(
                x.accesslevel_id,
                x.name,
                x.components.Select(c => new AccessLevelComponentDto(
                    c.driver_id,
                    c.device_id,
                    c.door_id,
                    c.acr_id,
                    c.timezone_id
                )).ToList(),
                x.location_id,
                x.is_active
            )).ToList(),
            d.location_id,
            d.is_active
        )).ToList();

        return res;
    }

    public async Task<UserDto> GetByIdAsync(int Id)
    {
        var d = await context.user
       .AsNoTracking()
       .Where(x => x.id == Id)
       .OrderBy(x => x.created_date)
       .Select(c => new {
            c.user_id,
            c.title,
            c.first_name,
            c.middle_name,
            c.last_name,
            gender = c.gender,
            c.email,
            c.phone,
            c.company_id,
            company = c.company.name,
            c.position_id,
            position = c.position.name,
            c.image,
            c.department_id,
            department = c.department.name,
            c.flag,
            additionals = c.additionals.Select(a => new { a.additional}),
            creds = c.credentials.Select(c => new CredentialDto(
                c.bits,
                c.issue_code,
                c.fac_code,
                c.card_no,
                c.pin,
                c.active_date,
                c.deactive_date
            )),
            accesslevel = c.user_access_levels.Select(a => new
            {
                a.accesslevel_id,
                a.accessLevel.name,
                components = a.accessLevel.components.Select(c => new
                {
                    c.driver_id,
                    c.device_id,
                    c.door_id,
                    c.acr_id,
                    c.timezone_id
                }),
                a.accessLevel.location_id,
                a.accessLevel.is_active

            }),
            c.location_id,
            c.is_active
            })
       .FirstOrDefaultAsync();

        var res = new UserDto(
            d.user_id,
            d.title,
            d.first_name,
            d.middle_name,
            d.last_name,
            (int)d.gender,
            d.email,
            d.phone,
            d.company_id,
            d.company,
            d.position_id,
            d.position,
            d.image,
            d.department_id,
            d.department,
            d.flag,
            d.additionals.Select(x => x.additional).ToList(),
            d.creds.ToList(),
            d.accesslevel.Select(x => new AccessLevelDto(
                x.accesslevel_id,
                x.name,
                x.components.Select(c => new AccessLevelComponentDto(
                    c.driver_id,
                    c.device_id,
                    c.door_id,
                    c.acr_id,
                    c.timezone_id
                )).ToList(),
                x.location_id,
                x.is_active
            )).ToList(),
            d.location_id,
            d.is_active
        );

        return res;
    }

    public async Task<IEnumerable<UserDto>> GetByLocationIdAsync(int locationId)
    {
        var data = await context.user
       .AsNoTracking()
       .Where(x => x.location_id == locationId || x.location_id == 1)
       .OrderBy(x => x.created_date)
       .Select(c => new {
            c.user_id,
            c.title,
            c.first_name,
            c.middle_name,
            c.last_name,
            gender = c.gender,
            c.email,
            c.phone,
            c.company_id,
            company = c.company.name,
            c.position_id,
            position = c.position.name,
            c.image,
            c.department_id,
            department = c.department.name,
            c.flag,
            additionals = c.additionals.Select(a => new { a.additional}),
            creds = c.credentials.Select(c => new CredentialDto(
                c.bits,
                c.issue_code,
                c.fac_code,
                c.card_no,
                c.pin,
                c.active_date,
                c.deactive_date
            )),
            accesslevel = c.user_access_levels.Select(a => new
            {
                a.accesslevel_id,
                a.accessLevel.name,
                components = a.accessLevel.components.Select(c => new
                {
                    c.driver_id,
                    c.device_id,
                    c.door_id,
                    c.acr_id,
                    c.timezone_id
                }),
                a.accessLevel.location_id,
                a.accessLevel.is_active

            }),
            c.location_id,
            c.is_active
            })
       .ToArrayAsync();

       var res = data.Select(d => new UserDto(
            d.user_id,
            d.title,
            d.first_name,
            d.middle_name,
            d.last_name,
            (int)d.gender,
            d.email,
            d.phone,
            d.company_id,
            d.company,
            d.position_id,
            d.position,
            d.image,
            d.department_id,
            d.department,
            d.flag,
            d.additionals.Select(x => x.additional).ToList(),
            d.creds.ToList(),
            d.accesslevel.Select(x => new AccessLevelDto(
                x.accesslevel_id,
                x.name,
                x.components.Select(c => new AccessLevelComponentDto(
                    c.driver_id,
                    c.device_id,
                    c.door_id,
                    c.acr_id,
                    c.timezone_id
                )).ToList(),
                x.location_id,
                x.is_active
            )).ToList(),
            d.location_id,
            d.is_active
        )).ToList();

        return res;
    }

    public async Task<UserDto> GetByUserIdAsync(string UserId)
    {
        var d = await context.user
       .AsNoTracking()
       .Where(x => x.user_id.Equals(UserId))
       .OrderBy(x => x.created_date)
      .Select(c => new {
            c.user_id,
            c.title,
            c.first_name,
            c.middle_name,
            c.last_name,
            gender = c.gender,
            c.email,
            c.phone,
            c.company_id,
            company = c.company.name,
            c.position_id,
            position = c.position.name,
            c.image,
            c.department_id,
            department = c.department.name,
            c.flag,
            additionals = c.additionals.Select(a => new { a.additional}),
            creds = c.credentials.Select(c => new CredentialDto(
                c.bits,
                c.issue_code,
                c.fac_code,
                c.card_no,
                c.pin,
                c.active_date,
                c.deactive_date
            )),
            accesslevel = c.user_access_levels.Select(a => new
            {
                a.accesslevel_id,
                a.accessLevel.name,
                components = a.accessLevel.components.Select(c => new
                {
                    c.driver_id,
                    c.device_id,
                    c.door_id,
                    c.acr_id,
                    c.timezone_id
                }),
                a.accessLevel.location_id,
                a.accessLevel.is_active

            }),
            c.location_id,
            c.is_active
            })
       .FirstOrDefaultAsync();

        var res = new UserDto(
            d.user_id,
            d.title,
            d.first_name,
            d.middle_name,
            d.last_name,
            (int)d.gender,
            d.email,
            d.phone,
            d.company_id,
            d.company,
            d.position_id,
            d.position,
            d.image,
            d.department_id,
            d.department,
            d.flag,
            d.additionals.Select(x => x.additional).ToList(),
            d.creds.ToList(),
            d.accesslevel.Select(x => new AccessLevelDto(
                x.accesslevel_id,
                x.name,
                x.components.Select(c => new AccessLevelComponentDto(
                    c.driver_id,
                    c.device_id,
                    c.door_id,
                    c.acr_id,
                    c.timezone_id
                )).ToList(),
                x.location_id,
                x.is_active
            )).ToList(),
            d.location_id,
            d.is_active
        );

        return res;
    }



    public async Task<IEnumerable<string>> GetMacsRelateCredentialByUserIdAsync(string UserId)
    {
        // var m = await context.credential
        // .Where(x => x.cardholder_id.Equals(UserId)).Select(x => x.)
        throw new NotImplementedException();
    }

    public async Task<Pagination<UserDto>> GetPaginationAsync(PaginationParamsWithFilter param, int location)
    {

        var query = context.user.AsNoTracking().AsQueryable();


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
                        EF.Functions.ILike(x.gender.ToString(), pattern) ||
                        EF.Functions.ILike(x.email, pattern) ||
                        EF.Functions.ILike(x.phone, pattern) ||
                        EF.Functions.ILike(x.company.name, pattern) ||
                        EF.Functions.ILike(x.department.name, pattern) ||
                        EF.Functions.ILike(x.position.name, pattern)

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
                        x.gender.ToString().Contains(search) ||
                        x.email.Contains(search) ||
                        x.phone.Contains(search) ||
                        x.company.name.Contains(search) ||
                        x.department.name.Contains(search) ||
                        x.position.name.Contains(search)
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
            .Select(c => new {
            c.user_id,
            c.title,
            c.first_name,
            c.middle_name,
            c.last_name,
            gender = c.gender,
            c.email,
            c.phone,
            c.company_id,
            company = c.company.name,
            c.position_id,
            position = c.position.name,
            c.image,
            c.department_id,
            department = c.department.name,
            c.flag,
            additionals = c.additionals.Select(a => new { a.additional}),
            creds = c.credentials.Select(c => new CredentialDto(
                c.bits,
                c.issue_code,
                c.fac_code,
                c.card_no,
                c.pin,
                c.active_date,
                c.deactive_date
            )),
            accesslevel = c.user_access_levels.Select(a => new
            {
                a.accesslevel_id,
                a.accessLevel.name,
                components = a.accessLevel.components.Select(c => new
                {
                    c.driver_id,
                    c.device_id,
                    c.door_id,
                    c.acr_id,
                    c.timezone_id
                }),
                a.accessLevel.location_id,
                a.accessLevel.is_active

            }),
            c.location_id,
            c.is_active
            })
            .ToListAsync();

             var res = data.Select(d => new UserDto(
            d.user_id,
            d.title,
            d.first_name,
            d.middle_name,
            d.last_name,
            (int)d.gender,
            d.email,
            d.phone,
            d.company_id,
            d.company,
            d.position_id,
            d.position,
            d.image,
            d.department_id,
            d.department,
            d.flag,
            d.additionals.Select(x => x.additional).ToList(),
            d.creds.ToList(),
            d.accesslevel.Select(x => new AccessLevelDto(
                x.accesslevel_id,
                x.name,
                x.components.Select(c => new AccessLevelComponentDto(
                    c.driver_id,
                    c.device_id,
                    c.door_id,
                    c.acr_id,
                    c.timezone_id
                )).ToList(),
                x.location_id,
                x.is_active
            )).ToList(),
            d.location_id,
            d.is_active
        )).ToList();


        return new Pagination<UserDto>
        {
            Data = res,
            Page = new PaginationData
            {
                TotalCount = count,
                PageNumber = param.PageNumber,
                PageSize = param.PageSize,
                TotalPage = (int)Math.Ceiling(count / (double)param.PageSize)
            }
        };
    }

    public Task<bool> IsAnyByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> IsAnyByUserId(string userid)
    {
        return await context.user.AnyAsync(x => x.user_id.Equals(userid));
    }

      

      public async Task<bool> IsAnyByNameAsync(string name)
      {
            return await context.user.AsNoTracking().AnyAsync(x => x.first_name.Equals(name) || x.last_name.Equals(name));
      }
}
