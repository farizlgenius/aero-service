using System;
using Aero.Application.DTOs;
using Aero.Application.Entities;
using Aero.Application.Interfaces;
using Aero.Infrastructure.Data;
using Aero.Infrastructure.Mapper;
using HID.Aero.ScpdNet.Wrapper;
using Microsoft.EntityFrameworkCore;

namespace Aero.Infrastructure.Repositories;

public class IdReportRepository(AppDbContext context) : IQIdReportRepository
{


      public async Task<int> DeleteByComponentIdAsync(short component)
      {
            throw new NotImplementedException();
            
      }

      public async Task<int> DeleteByMacAndComponentIdAsync(string mac, short component)
      {
            var entity = await context.id_report
            .Where(x => x.mac.Equals(mac) && x.scp_id == component)
            .OrderBy(x => x.id)
            .FirstOrDefaultAsync();

            if(entity is null) return 0;

            context.id_report.Remove(entity);

            return await context.SaveChangesAsync();
      }

      public async Task<IdReport> GetByMacAndComponentIdAsync(string mac, short component)
      {
            var res = await context.id_report
            .Where(x => x.mac.Equals(mac) && x.scp_id == component)
             .Select(en => new IdReport()
             {
                  ComponentId = en.scp_id,
                  SerialNumber = en.serial_number,
                  Mac = en.mac,
                  Ip = en.ip,
                  Port = en.port,
                  Firmware = en.firmware,
                  HardwareTypeDescription = "HID",
                  HardwareType = 1,
             })
            .OrderBy(x => x.ComponentId)
            .FirstOrDefaultAsync();

            return res;
      }

      public Task<int> UpdateAsync(IdReport newData)
      {
            throw new NotImplementedException();
      }

      public async Task<IEnumerable<IdReportDto>> RemoveIdReportAsync(SCPReplyMessage message)
      {
            var report = await context.id_report
                .Where(x => x.scp_id == message.SCPId)
                .OrderBy(x => x.id)
                .FirstOrDefaultAsync();

            if (report is null) return await context.id_report
                    .AsNoTracking()
                    .Select(x => IdReportMapper.IdReportToDto(x))
                    .ToArrayAsync();

            context.id_report.Remove(report);
            await context.SaveChangesAsync();

            return await context.id_report
                    .AsNoTracking()
                    .Select(x => MapperHelper.IdReportToDto(x))
                    .ToArrayAsync();
      }

      Task<Domain.Entities.IdReport> IQIdReportRepository.GetByMacAndComponentIdAsync(string mac, short component)
      {
            throw new NotImplementedException();
      }

      public async Task<int> AddAsync(Domain.Entities.IdReport data)
      {
            var en = IdReportMapper.ToEf(data);
            await context.id_report.AddAsync(en);
            return await context.SaveChangesAsync();
      }

      public async Task<int> UpdateAsync(Domain.Entities.IdReport newData)
      {
            var en = IdReportMapper.ToEf(newData);
            context.id_report.Update(en);
            return await context.SaveChangesAsync();

      }
}
