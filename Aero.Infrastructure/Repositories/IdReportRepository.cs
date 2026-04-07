using Aero.Application.DTOs;
using Aero.Application.Helpers;
using Aero.Application.Interfaces;
using Aero.Domain.Entities;
using Aero.Domain.Interfaces;
using Aero.Infrastructure.Persistences;
using Aero.Infrastructure.Persistences.Entities;
using Aero.Infrastructure.Mapper;
using Microsoft.EntityFrameworkCore;
using System;

namespace Aero.Infrastructure.Repositories;

public class IdReportRepository(AppDbContext context) : IIdReportRepository
{
      public async Task<int> AddAsync(IScpReply message)
      {
            var en = new Aero.Infrastructure.Persistences.Entities.IdReport(message);
            await context.id_report.AddAsync(en);
            return await context.SaveChangesAsync();
      }

      public Task<int> AddAsync(Application.Entities.IdReport data)
      {
            throw new NotImplementedException();
      }

      public Task<int> DeleteByIdAsync(int id)
      {
            throw new NotImplementedException();
      }

      public async Task<int> DeleteByMacAndScpIdAsync(string mac, int ScpId)
      {
            var en = await context.id_report
            .Where(x => x.mac.Equals(mac) && x.scp_id == ScpId)
            .FirstOrDefaultAsync();

            if(en is null) return 0;

            context.id_report.Remove(en);
            return await context.SaveChangesAsync();
      }

      public async Task<IEnumerable<IdReportDto>> DeletePendingRecordAsync(short scpid)
      {
            var en = await context.id_report
            .Where(x => x.scp_id == scpid)
            .OrderBy(x => x.scp_id)
            .FirstOrDefaultAsync();

            if(en is null) return Enumerable.Empty<IdReportDto>();

            context.id_report.Remove(en);
            await context.SaveChangesAsync();

            return await GetAsync(en.location_id);
      }

      public async Task<IEnumerable<IdReportDto>> GetAsync(int location)
      {
            var dtos = await context.id_report
               .AsNoTracking()
               .Where(x => x.location_id == location || x.location_id == 1)
               .Select(x => new IdReportDto
               (
                    x.scp_id,
                    x.serial_number,
                     x.mac,
                     x.ip,
                  x.port,
                      x.firmware,
                     x.device_id,
                    "AERO"
               ))
               .ToArrayAsync();

            return dtos;
      }

    public async Task<IEnumerable<IdReportDto>> GetAsync()
    {

        var dtos = await context.id_report
           .AsNoTracking()
           .Select(x => new IdReportDto(
             x.scp_id,
             x.serial_number,
             x.mac,
             x.ip,
              x.port,
              x.firmware,
                x.device_id,
               "AERO"
           ))
           .ToArrayAsync();

        return dtos;
    }

      public async Task<IEnumerable<IdReportDto>> GetAsync(short location)
      {
            var dtos = await context.id_report
           .AsNoTracking()
           .Where(x => x.location_id == location || x.location_id == 1)
           .Select(x => new IdReportDto(
             x.scp_id,
             x.serial_number,
             x.mac,
             x.ip,
              x.port,
              x.firmware,
                x.device_id,
               "AERO"
           ))
           .ToArrayAsync();

        return dtos;
      }

      public Task<IdReportDto> GetByIdAsync(int id)
      {
            throw new NotImplementedException();
      }

      public Task<IEnumerable<IdReportDto>> GetByLocationIdAsync(int locationId)
      {
            throw new NotImplementedException();
      }

      public async Task<IdReportDto> GetByMacAndScpIdAsync(string mac, short scpid)
      {
             var dtos = await context.id_report
               .AsNoTracking()
               .Where(x => x.mac.Equals(mac) && x.scp_id == scpid)
                          .OrderBy(x => x.id)
               .Select(x => new IdReportDto(
                 x.scp_id,
                  x.serial_number,
                    x.mac,
                      x.ip,
                    x.port,
                     x.firmware,
                     x.device_id,
                    ""
               ))
    
               .FirstOrDefaultAsync();

            return dtos;
      }

      public async Task<int> GetCountAsync(int location)
      {
            return await context.id_report.CountAsync();
      }

      public Task<int> GetCountAsync(short location)
      {
            throw new NotImplementedException();
      }

      public Task<short> GetLowestUnassignedNumberAsync(int max, string mac)
      {
            throw new NotImplementedException();
      }

    public Task<Pagination<IdReportDto>> GetPaginationAsync(PaginationParamsWithFilter param, int location)
    {
        throw new NotImplementedException();
    }

    public Task<bool> IsAnyByIdAsync(int id)
      {
            throw new NotImplementedException();
      }

      public async Task<bool> IsAnyByMacAndScpIdAsync(string mac, int scpid)
      {
        return await context.id_report.AnyAsync(x => x.mac.Equals(mac) && x.scp_id == scpid);
      }

      public Task<bool> IsAnyByNameAsync(string name)
      {
            throw new NotImplementedException();
      }

      public async Task<int> UpdateAsync(IScpReply message)
      {
            var en = await context.id_report.Where(x => x.mac.Equals(UtilitiesHelper.ByteToHex(message.id.mac_addr)) && x.scp_id == message.id.scp_id).FirstOrDefaultAsync();

            if(en is null) return 0;
            
            en.Update(message);

            context.id_report.Update(en);
            return await context.SaveChangesAsync();
      }

      public Task<int> UpdateAsync(Application.Entities.IdReport data)
      {
            throw new NotImplementedException();
      }

      public async Task UpdateIpAddressAsync(int ScpId, string ip)
      {
            var report = await context.id_report
                    .Where(x => x.scp_id == ScpId)
                    .OrderBy(x => x.id)
                    .FirstOrDefaultAsync();

                if (report is null) return;

                report.ip = ip;

                context.id_report.Update(report);
                await context.SaveChangesAsync();
      }

      public async Task UpdatePortAddressAsync(int ScpId, string port)
      {
            var report = await context.id_report
                    .Where(x => x.scp_id == ScpId)
                    .OrderBy(x => x.id)
                    .FirstOrDefaultAsync();

                if (report is null) return;

                report.port = port;

                context.id_report.Update(report);
                await context.SaveChangesAsync();
      }
}
