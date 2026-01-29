using System;
using Aero.Application.DTOs;
using Aero.Application.Interfaces;
using Aero.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Aero.Infrastructure.Repositories;

public class QIdReportRepository(AppDbContext context) : IQIdReportRepository
{
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

      public Task<short> GetLowestUnassignedNumberAsync(int max)
      {
            throw new NotImplementedException();
      }

      public Task<bool> IsAnyByComponentId(short component)
      {
            throw new NotImplementedException();
      }
}
