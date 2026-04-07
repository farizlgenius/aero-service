using Aero.Application.DTOs;
using Aero.Domain.Entities;
using Aero.Domain.Interface;
using Aero.Infrastructure.Persistences;
using Aero.Infrastructure.Mapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aero.Application.Interfaces;

namespace Aero.Infrastructure.Repositories
{
    public sealed class CmndRepository(AppDbContext context) : ICmndRepository
    {
        public async Task<int> AddAsync(CommandAudit data)
        {
            var en = new Aero.Infrastructure.Persistences.Entities.CommandAudit(data);
            await context.AddAsync(en);
            return await context.SaveChangesAsync();
        }

        public Task<int> DeleteByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

            public Task<IEnumerable<CommandAudit>> GetAsync()
            {
                  throw new NotImplementedException();
            }

            public Task<CommandAudit> GetByIdAsync(int id)
            {
                  throw new NotImplementedException();
            }

            public Task<IEnumerable<CommandAudit>> GetByLocationIdAsync(int locationId)
            {
                  throw new NotImplementedException();
            }

            public async Task<Pagination<CommandAudit>> GetCommandStatusAsync(PaginationParamsWithFilter pagination, int location)
        {
            var query = context.commnad_audit.AsNoTracking().AsQueryable();


            if (!string.IsNullOrWhiteSpace(pagination.Search))
            {
                if (!string.IsNullOrWhiteSpace(pagination.Search))
                {
                    var search = pagination.Search.Trim();

                    if (context.Database.IsNpgsql())
                    {
                        var pattern = $"%{search}%";

                        query = query.Where(x =>
                            EF.Functions.ILike(x.mac, pattern) ||
                            EF.Functions.ILike(x.command, pattern) ||
                            EF.Functions.ILike(x.nak_reason, pattern) 
   
                        );
                    }
                    else // SQL Server
                    {
                        query = query.Where(x =>
                            x.mac.Contains(search) ||
                            x.command.Contains(search) ||
                            x.nak_reason.Contains(search) 
                        );
                    }
                }
            }

            if (location >= 0)
            {
                query = query.Where(x => x.location_id == location);
            }

            if (pagination.StartDate != null)
            {
                var startUtc = DateTime.SpecifyKind(pagination.StartDate.Value, DateTimeKind.Utc);
                query = query.Where(x => x.created_date >= startUtc);
            }

            if (pagination.EndDate != null)
            {
                var endUtc = DateTime.SpecifyKind(pagination.EndDate.Value, DateTimeKind.Utc);
                query = query.Where(x => x.created_date <= endUtc);
            }

            var count = await query.CountAsync();


            var data = await query
                .AsNoTracking()
                .OrderByDescending(t => t.created_date)
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .Select(en => new CommandAudit
                {
                    // Base 
                    TagNo = en.tag_no,
                    ScpId = en.device_id,
                    Mac = en.mac,
                    Command = en.command,
                    IsSuccess = en.is_success,
                    IsPending = en.is_success,
                    NakDescCode = en.nake_desc_code,
                    NakReason = en.nak_reason,
                    LoationId = en.location_id

                })
                .ToListAsync();


            return new Pagination<CommandAudit>
            {
                Data = data,
                Page = new PaginationData
                {
                    TotalCount = count,
                    PageNumber = pagination.PageNumber,
                    PageSize = pagination.PageSize,
                    TotalPage = (int)Math.Ceiling(count / (double)pagination.PageSize)
                }
            };
        }


            public Task<Pagination<CommandAudit>> GetPaginationAsync(PaginationParamsWithFilter param, int location)
            {
                  throw new NotImplementedException();
            }

            public Task<bool> IsAnyByIdAsync(int id)
            {
                  throw new NotImplementedException();
            }

            public Task<bool> IsAnyByNameAsync(string name)
            {
                  throw new NotImplementedException();
            }

            public async Task<int> UpdateAsync(CommandAudit data)
        {
            var en = await context.commnad_audit
                .Where(x => x.tag_no == data.TagNo && x.mac == data.Mac && x.device_id == data.ScpId && x.is_pending == true)
                .OrderBy(x => x.id)
                .FirstOrDefaultAsync();

            if (en is null) return 0;

            en.Update(data);

            context.Update(en);
            return await context.SaveChangesAsync();
        }
    }
}
