using Aero.Application.DTOs;
using Aero.Domain.Entities;
using Aero.Domain.Interface;
using Aero.Infrastructure.Mapper;
using Aero.Infrastructure.Persistences;
using Aero.Infrastructure.Persistences.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Aero.Infrastructure.Repositories;

public class OperatorRepository(AppDbContext context) : IOperatorRepository
{
      public async Task<int> AddAsync(Aero.Domain.Entities.Operator data)
      {
        var en = new Aero.Infrastructure.Persistences.Entities.Operator(data);
        await context.@operator.AddAsync(en);
        var record = await context.SaveChangesAsync();
        if (record <= 0) return -1;
        return en.id;
      }


      public async Task<int> DeleteByIdAsync(int id)
      {
           var en = await context.@operator
           .Where(x => x.id == id)
           .OrderBy(x => x.id)
           .FirstOrDefaultAsync();

           if(en is null) return 0;

           context.@operator.Remove(en);
           return await context.SaveChangesAsync();
      }

      public async Task<int> UpdateAsync(Aero.Domain.Entities.Operator data)
      {
        var en = await context.@operator
        .Include(x => x.operator_locations)
        .Where(x => x.user_name.Equals(data.Username))
        .OrderBy(x => x.id)
        .FirstOrDefaultAsync();

        if (en is null) return 0;

        context.operator_location.RemoveRange(en.operator_locations);

        en.Update(data);

        context.@operator.Update(en);
        return await context.SaveChangesAsync();
    }



      public async Task<int> UpdatePasswordAsync(string username, string password)
      {
            var en = await context.@operator
            .Where(x => x.user_name.Equals(username))
            .FirstOrDefaultAsync();

            if(en is null) return 0;

            en.password = password;

            context.@operator.Update(en);
            return await context.SaveChangesAsync();
      }

    public async Task<IEnumerable<OperatorDto>> GetAsync()
    {
        var data = await context.@operator
           .AsNoTracking()
           .Select(o => 
           new
           {
               o.id,
               o.user_id,
               o.user_name,
               o.email,
               o.title,
               o.first_name,
               o.middle_name,
               o.last_name,
               o.phone,
               o.image,
               o.role_id,
               locationd = o.operator_locations.Select(l => l.location_id),
               o.is_active
           })
           .ToArrayAsync();

        var res = data.Select(o => new OperatorDto(o.id, o.user_id, o.user_name, o.email, o.title, o.first_name, o.middle_name, o.last_name, o.phone, o.image, o.role_id, o.locationd.ToList(), o.is_active)).ToList();

        return res;
    }

    public async Task<OperatorDto> GetByIdAsync(int id)
    {
        var o = await context.@operator
             .AsNoTracking()
             .OrderBy(x => x.id)
             .Select(o =>
               new
               {
                   o.id,
                   o.user_id,
                   o.user_name,
                   o.email,
                   o.title,
                   o.first_name,
                   o.middle_name,
                   o.last_name,
                   o.phone,
                   o.image,
                   o.role_id,
                   locationd = o.operator_locations.Select(l => l.location_id),
                   o.is_active
               })
             .FirstOrDefaultAsync();

        return new OperatorDto(o.id, o.user_id, o.user_name, o.email, o.title, o.first_name, o.middle_name, o.last_name, o.phone, o.image, o.role_id, o.locationd.ToList(), o.is_active);
    }

    public async Task<IEnumerable<OperatorDto>> GetByLocationIdAsync(int locationId)
    {
        var data = await context.@operator
            .AsNoTracking()
            .Where(x => x.operator_locations.Any(x => x.location_id == locationId || x.location_id == 1))
            .Select(o =>
               new
               {
                   o.id,
                   o.user_id,
                   o.user_name,
                   o.email,
                   o.title,
                   o.first_name,
                   o.middle_name,
                   o.last_name,
                   o.phone,
                   o.image,
                   o.role_id,
                   locationd = o.operator_locations.Select(l => l.location_id),
                   o.is_active
               })
            .ToArrayAsync();

        var res = data.Select(o => new OperatorDto(o.id, o.user_id, o.user_name, o.email, o.title, o.first_name, o.middle_name, o.last_name, o.phone, o.image, o.role_id, o.locationd.ToList(), o.is_active)).ToList();

        return res;
    }

    public async Task<OperatorDto> GetByUsernameAsync(string username)
    {
        var o = await context.@operator
             .AsNoTracking()
             .OrderBy(x => x.id)
             .Where(o => o.user_name.Equals(username))
             .Select(o =>
               new
               {
                   o.id,
                   o.user_id,
                   o.user_name,
                   o.email,
                   o.title,
                   o.first_name,
                   o.middle_name,
                   o.last_name,
                   o.phone,
                   o.image,
                   o.role_id,
                   locationd = o.operator_locations.Select(l => l.location_id),
                   o.is_active
               })
             .FirstOrDefaultAsync();

        return new OperatorDto(o.id, o.user_id, o.user_name, o.email, o.title, o.first_name, o.middle_name, o.last_name, o.phone, o.image, o.role_id, o.locationd.ToList(), o.is_active);

    }

    //public async Task<short> GetLowestUnassignedNumberAsync(int max, string mac)
    //{
    //    if (max <= 0) return -1;

    //    var query = context.@operator
    //        .AsNoTracking()
    //        .Select(x => x.component_id);

    //    // Handle empty table case quickly
    //    var hasAny = await query.AnyAsync();
    //    if (!hasAny)
    //        return 1; // start at 1 if table is empty

    //    // Load all numbers into memory (only the column, so it's lightweight)
    //    var numbers = await query.Distinct().OrderBy(x => x).ToListAsync();

    //    short expected = 1;
    //    foreach (var num in numbers)
    //    {
    //        if (num != expected)
    //            return expected; // found the lowest missing number
    //        expected++;
    //    }

    //    // If none missing in sequence, return next number
    //    if (expected > max) return -1;
    //    return expected;
    //}

    public async Task<string> GetPasswordByUsername(string username)
    {
        return await context.@operator.AsNoTracking().Where(x => x.user_name.Equals(username)).Select(x => x.password).FirstOrDefaultAsync() ?? "";
    }

    public async Task<Pagination<OperatorDto>> GetPaginationAsync(PaginationParamsWithFilter param, int location)
    {
        var query = context.@operator.AsNoTracking().AsQueryable();


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
                        EF.Functions.ILike(x.user_name, pattern) ||
                        EF.Functions.ILike(x.email, pattern) ||
                        EF.Functions.ILike(x.title, pattern) ||
                        EF.Functions.ILike(x.first_name, pattern) ||
                        EF.Functions.ILike(x.middle_name, pattern) ||
                        EF.Functions.ILike(x.last_name, pattern) ||
                        EF.Functions.ILike(x.phone, pattern)

                    );
                }
                else // SQL Server
                {
                    query = query.Where(x =>
                        x.user_id.Contains(search) ||
                        x.user_name.Contains(search) ||
                        x.email.Contains(search) ||
                        x.title.Contains(search) ||
                        x.first_name.Contains(search) ||
                        x.middle_name.Contains(search) ||
                        x.last_name.Contains(search) ||
                        x.phone.Contains(search)
                    );
                }
            }
        }

        query = query.Where(x => x.operator_locations.Any(x => x.location_id == location || x.location_id == 1));

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
            .Select(o =>
               new
               {
                   o.id,
                   o.user_id,
                   o.user_name,
                   o.email,
                   o.title,
                   o.first_name,
                   o.middle_name,
                   o.last_name,
                   o.phone,
                   o.image,
                   o.role_id,
                   locationd = o.operator_locations.Select(l => l.location_id),
                   o.is_active
               })
            .ToListAsync();

        var res = data.Select(o => new OperatorDto(o.id, o.user_id, o.user_name, o.email, o.title, o.first_name, o.middle_name, o.last_name, o.phone, o.image, o.role_id, o.locationd.ToList(), o.is_active)).ToList();



        return new Pagination<OperatorDto>
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

    public async Task<bool> IsAnyById(int id)
    {
        return await context.@operator.AnyAsync(x => x.id == id);
    }

    public async Task<bool> IsAnyByUsernameAsync(string name)
    {
        return await context.@operator.AnyAsync(x => x.user_name.Equals(name));
    }

    public async Task<bool> IsAnyByNameAsync(string name)
    {
        return await context.@operator.AnyAsync(x => x.first_name.Equals(name));
    }
}
