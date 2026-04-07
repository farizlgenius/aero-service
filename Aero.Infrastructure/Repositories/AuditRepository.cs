using System;
using Aero.Application.DTOs;
using Aero.Application.Interfaces;
using Aero.Domain.Entities;
using Aero.Infrastructure.Persistences;
using Aero.Infrastructure.Persistences.Entities;
using Microsoft.EntityFrameworkCore;

namespace Aero.Infrastructure.Repositories;

public class AuditRepository(AppDbContext context) : IAuditRepository
{
      public async Task CreateAuditAsync(AuditDto dto)
      {
            await context.audit_trail.AddAsync(
                new AuditTrail()
                {
                    timestamp = dto.TimeStamp,
                    username = dto.Username,
                    controller = dto.Controller,
                    action = dto.Action,
                    http_method = dto.HttpMethod,
                    path = dto.Path,
                    request_body = dto.RequestBody,
                    status_code = dto.StatusCode,
                    execution_time = dto.ExecutionTimeMs,
                    ip = dto.IpAddress
                }
            );

            await context.SaveChangesAsync();
      }

      public async Task<Pagination<AuditDto>> GetPageTransactionWithCountAndDateAndSearchAsync(PaginationParamsWithFilter param, short location)
      {
            var query = context.audit_trail.AsNoTracking().AsQueryable();


        if (!string.IsNullOrWhiteSpace(param.Search))
        {
            if (!string.IsNullOrWhiteSpace(param.Search))
            {
                var search = param.Search.Trim();

                if (context.Database.IsNpgsql())
                {
                    var pattern = $"%{search}%";

                    query = query.Where(x =>
                        EF.Functions.ILike(x.username, pattern) ||
                        EF.Functions.ILike(x.action, pattern) ||
                        EF.Functions.ILike(x.controller, pattern) ||
                        EF.Functions.ILike(x.action, pattern) ||
                        EF.Functions.ILike(x.path, pattern) ||
                        EF.Functions.ILike(x.status_code.ToString() , pattern)
                    );
                }
                else // SQL Server
                {
                    query = query.Where(x =>
                        x.username.Contains(search) ||
                        x.action.ToString().Contains(search) ||
                        x.controller.Contains(search) ||
                        x.action.Contains(search) ||
                        x.path.Contains(search) || 
                        x.status_code.ToString().Contains(search) 
                    );
                }
            }
        }

        if (location >= 0)
        {
            // query = query.Where(x => x.location_id == location || x.location_id == 1);
        }

        if (param.StartDate != null)
        {
            var startUtc = DateTime.SpecifyKind(param.StartDate.Value, DateTimeKind.Utc);
            query = query.Where(x => x.timestamp >= startUtc);
        }

        if (param.EndDate != null)
        {
            var endUtc = DateTime.SpecifyKind(param.EndDate.Value, DateTimeKind.Utc);
            query = query.Where(x => x.timestamp <= endUtc);
        }

        var count = await query.CountAsync();


        var data = await query
            .AsNoTracking()
            .OrderByDescending(t => t.timestamp)
            .Skip((param.PageNumber - 1) * param.PageSize)
            .Take(param.PageSize)
            .Select(en => new AuditDto(
                  en.username,
                  en.controller,
                  en.action,
                  en.http_method,
                  en.path,
                  en.request_body,
                  en.status_code,
                  en.execution_time,
                  en.ip,
                  en.timestamp
                  )) .ToListAsync();


        return new Pagination<AuditDto>
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
}
