using System;
using Aero.Application.DTOs;
using Aero.Application.Interfaces;
using Aero.Domain.Entities;
using Aero.Infrastructure.Persistences;
using Microsoft.EntityFrameworkCore;

namespace Aero.Infrastructure.Repositories;

public class CompanyRepository(AppDbContext context) : ICompanyRepository
{
      public async Task<int> AddAsync(Company data)
      {
            var en = new Aero.Infrastructure.Persistences.Entities.Company(data);
            await context.companies.AddAsync(en);
            var rec = await context.SaveChangesAsync();
            if (rec <= 0) return -1;
            return en.id;
      }

      public async Task<int> DeleteByIdAsync(int id)
      {
            var en = await context.companies
            .AsNoTracking()
            .Where(x => x.id == id)
            .OrderBy(x => x.id)
            .FirstOrDefaultAsync();

            if (en is null) return -1;

            context.companies.Remove(en);
            return await context.SaveChangesAsync();
      }

      public async Task<IEnumerable<CompanyDto>> GetAsync()
      {
            var res = await context.companies
            .AsNoTracking()
            .Select(x => new CompanyDto(x.id, x.name, x.description, x.location_id, x.is_active))
            .ToArrayAsync();

            return res;
      }

      public async Task<CompanyDto> GetByIdAsync(int id)
      {
            var en = await context.companies
           .AsNoTracking()
           .Where(x => x.id == id)
           .OrderBy(x => x.id)
           .Select(x => new CompanyDto(x.id, x.name, x.description, x.location_id, x.is_active))
           .FirstOrDefaultAsync();

            return en;
      }

      public async Task<IEnumerable<CompanyDto>> GetByLocationIdAsync(int locationId)
      {
            var res = await context.companies
            .AsNoTracking()
            .Where(x => x.location_id == locationId)
            .Select(x => new CompanyDto(x.id, x.name, x.description, x.location_id, x.is_active))
            .ToArrayAsync();

            return res;
      }

      public async Task<Pagination<CompanyDto>> GetPaginationAsync(PaginationParamsWithFilter param, int location)
      {
            var query = context.companies.AsNoTracking().AsQueryable();


            if (!string.IsNullOrWhiteSpace(param.Search))
            {
                  if (!string.IsNullOrWhiteSpace(param.Search))
                  {
                        var search = param.Search.Trim();

                        if (context.Database.IsNpgsql())
                        {
                              var pattern = $"%{search}%";

                              query = query.Where(x =>
                                  EF.Functions.ILike(x.name, pattern) ||
                                  EF.Functions.ILike(x.description, pattern)
                              );
                        }
                        else // SQL Server
                        {
                              query = query.Where(x =>
                                  x.name.Contains(search) ||
                                  x.description.Contains(search)
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
                 .Select(x => new CompanyDto(x.id, x.name, x.description, x.location_id, x.is_active))
                .ToListAsync();


            return new Pagination<CompanyDto>
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

      public async Task<bool> IsAnyByIdAsync(int id)
      {
            return await context.companies.AsNoTracking()
            .AnyAsync(x => x.id == id);
      }

      public async Task<bool> IsAnyByNameAsync(string name)
      {
            return await context.companies.AsNoTracking()
            .AnyAsync(x => x.name.Equals(name));
      }

      public async Task<bool> IsAnyReferenceByIdAsync(int id)
      {
            return await context.companies.AsNoTracking().AnyAsync(x => x.id == id && x.users.Count > 0);
      }

      public async Task<int> UpdateAsync(Company data)
      {
            var en = await context.companies
            .AsNoTracking()
            .Where(x => x.id == data.Id)
            .OrderBy(x => x.id)
            .FirstOrDefaultAsync();

            if (en is null) return -1;

            en.Update(data);

            context.companies.Update(en);
            return await context.SaveChangesAsync();
      }
}
