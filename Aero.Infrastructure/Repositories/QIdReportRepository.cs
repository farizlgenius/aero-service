using System;
using Aero.Application.DTOs;
using Aero.Application.Helpers;
using Aero.Application.Interfaces;
using Aero.Infrastructure.Data;
using Aero.Infrastructure.Mapper;
using Microsoft.EntityFrameworkCore;

namespace Aero.Infrastructure.Repositories;

public class QIdReportRepository(AppDbContext context) : IQIdReportRepository
{
      public async Task<int> AddAsync(IScpReply message)
      {
            var en = IdReportMapper.ToEf(message);
            await context.id_report.AddAsync(en);
            return await context.SaveChangesAsync();
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

      public async Task<IEnumerable<IdReportDto>> DeletePendingRecordAsync(string mac, short scpid)
      {
            var en = await context.id_report
            .Where(x => x.mac.Equals(mac) && x.scp_id == scpid)
            .OrderBy(x => x.scp_id)
            .FirstOrDefaultAsync();

            if(en is null) return Enumerable.Empty<IdReportDto>();

            context.id_report.Remove(en);
            await context.SaveChangesAsync();

            return await GetAsync();
      }

      public async Task<IEnumerable<IdReportDto>> GetAsync()
      {
            var dtos = await context.id_report
               .AsNoTracking()
               .Select(x => new IdReportDto
               {
                     ComponentId = 0,
                     SerialNumber = x.serial_number,
                     MacAddress = x.mac,
                     Ip = x.ip,
                     Port = x.port,
                     Firmware = x.firmware,
                     HardwareType = x.device_id,
                     HardwareTypeDescription = ""
               })
               .ToArrayAsync();

            return dtos;
      }

      public Task<IdReportDto> GetByComponentIdAsync(short componentId)
      {
            throw new NotImplementedException();
      }

      public Task<IEnumerable<IdReportDto>> GetByLocationIdAsync(short locationId)
      {
            throw new NotImplementedException();
      }

      public async Task<int> GetCountAsync()
      {
            return await context.id_report.CountAsync();
      }

      public Task<short> GetLowestUnassignedNumberAsync(int max, string mac)
      {
            throw new NotImplementedException();
      }

      public Task<bool> IsAnyByComponentId(short component)
      {
            throw new NotImplementedException();
      }

      public Task<bool> IsAnyByMacAndScpIdAsync(string mac, int scpid)
      {
            throw new NotImplementedException();
      }

      public async Task<int> UpdateAsync(IScpReply message)
      {
            var en = await context.id_report.Where(x => x.mac.Equals(UtilitiesHelper.ByteToHex(message.id.mac_addr)) && x.scp_id == message.id.scp_id).FirstOrDefaultAsync();

            if(en is null) return 0;
            IdReportMapper.Update(en,message);

            context.id_report.Update(en);
            return await context.SaveChangesAsync();
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
