using AutoMapper;
using HIDAeroService.Data;
using HIDAeroService.Entity;
using Microsoft.EntityFrameworkCore;
using HIDAeroService.Constants;
using System.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Net;
using System.Runtime.CompilerServices;
using HIDAeroService.Logging;
using HIDAeroService.AeroLibrary;
using HIDAeroService.Utility;
using MiNET.Worlds;
using HIDAeroService.Helpers;
using HIDAeroService.Constant;
using MiNET.Entities;
using System.ComponentModel;
using HIDAeroService.DTO;
using HIDAeroService.DTO.TimeZone;
using HIDAeroService.DTO.Interval;
using HIDAeroService.Mapper;

namespace HIDAeroService.Service.Impl
{
    public class TimeZoneService(AppDbContext context, IHelperService<Entity.TimeZone> helperService, AeroCommand command, IMapper mapper, ILogger<TimeZoneService> logger) : ITimeZoneService
    {
        public async Task<ResponseDto<IEnumerable<TimeZoneDto>>> GetAsync()
        {
            var dtos = await context.TimeZones
                .AsNoTracking()
                .Include(c => c.TimeZoneIntervals)
                .ThenInclude(x => x.Interval)
                .ThenInclude(x => x.Days)
                .Select(x => MapperHelper.TimeZoneToDto(x))
                .ToArrayAsync();

            if (dtos.Count() == 0) return ResponseHelper.NotFoundBuilder<IEnumerable<TimeZoneDto>>();
            return ResponseHelper.SuccessBuilder<IEnumerable<TimeZoneDto>>(dtos);
        }

        public async Task<ResponseDto<TimeZoneDto>> GetByComponentIdAsync(short component)
        {
            var dto = await context.TimeZones
                .AsNoTracking()
                .Include(s => s.TimeZoneIntervals)
                .ThenInclude(x => x.Interval)
                .ThenInclude(x => x.Days)
                .Where(a => a.ComponentId == component)
                .Select(x => MapperHelper.TimeZoneToDto(x))
                .FirstOrDefaultAsync();

            if (dto is null) return ResponseHelper.NotFoundBuilder<TimeZoneDto>();
            return ResponseHelper.SuccessBuilder<TimeZoneDto>(dto);
        }

        public async Task<ResponseDto<bool>> CreateAsync(CreateTimeZoneDto dto)
        {
            List<string> errors = new List<string>();
            var max = await context.SystemSettings.AsNoTracking().Select(x => x.nTz).FirstOrDefaultAsync();
            var ComponentId = await helperService.GetLowestUnassignedNumberAsync<Entity.TimeZone>(context,max);
            if (ComponentId == -1) return ResponseHelper.ExceedLimit<bool>();

            var timezone = MapperHelper.CreateTimeZoneDtoToTimeZone(dto,ComponentId);
            List<Interval> intervals = new List<Interval>();
            foreach(var interval in dto.Intervals)
            {
                intervals.Add(MapperHelper.IntervalDtoToInterval(interval));
            }
            

            List<string> macs = await context.Hardwares.AsNoTracking().Select(x => x.MacAddress).ToListAsync();

            foreach (var mac in macs)
            {
                short id = await helperService.GetIdFromMacAsync(mac);
                long active = helperService.DateTimeToElapeSecond(dto.ActiveTime);
                long deactive = helperService.DateTimeToElapeSecond(dto.DeactiveTime);
                if (!await command.ExtendedTimeZoneActSpecificationAsync(id, timezone,intervals, (int)active, (int)deactive))
                {
                    errors.Add(MessageBuilder.Unsuccess(mac, Command.C3103));
                }
            }
            if (errors.Count > 0) return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.COMMAND_UNSUCCESS,errors);

            await context.TimeZones.AddAsync(timezone);
            await context.SaveChangesAsync();

            List<TimeZoneInterval> links = new List<TimeZoneInterval>();
            foreach (var interval in dto.Intervals)
            {
                links.Add(
                    new TimeZoneInterval
                    {
                        Uuid = dto.Uuid,
                        LocationId = dto.LocationId,
                        LocationName = dto.LocationName,
                        IsActive = dto.IsActive,

                        IntervalId = interval.ComponentId,
                        TimeZoneId = ComponentId
                    }
                );
            }

            await context.TimeZoneIntervals.AddRangeAsync(links);
            await context.SaveChangesAsync();

            return ResponseHelper.SuccessBuilder(true);
        }

        public async Task<ResponseDto<bool>> DeleteAsync(short component)
        {
            var entity = await context.TimeZones.Include(s => s.TimeZoneIntervals).FirstOrDefaultAsync(x => x.ComponentId == component);
            if (entity is null) return ResponseHelper.NotFoundBuilder<bool>();

            context.TimeZones.Remove(entity);
            await context.SaveChangesAsync();

            return ResponseHelper.SuccessBuilder<bool>(true);
        }

        public async Task<ResponseDto<TimeZoneDto>> UpdateAsync(TimeZoneDto dto)
        {
            List<string> errors = new List<string>();
            var entity = await context.TimeZones
                .Include(x => x.TimeZoneIntervals)
                .ThenInclude(x => x.Interval)
                .FirstOrDefaultAsync(p => p.ComponentId == dto.ComponentId);
            if (entity is null) return ResponseHelper.NotFoundBuilder<TimeZoneDto>();

            entity = MapperHelper.TimeZoneDtoMapTimeZone(dto,entity);
            var intervals = dto.Intervals.Select(s => MapperHelper.IntervalDtoToInterval(s)).ToList();

            List<string> macs = await context.Hardwares.AsNoTracking().Select(x => x.MacAddress).ToListAsync();
            foreach (var mac in macs)
            {
                short id = await helperService.GetIdFromMacAsync(mac);
                long active = helperService.DateTimeToElapeSecond(dto.ActiveTime);
                long deactive = helperService.DateTimeToElapeSecond(dto.DeactiveTime);
                if (!await command.ExtendedTimeZoneActSpecificationAsync(id, entity, intervals, (int)active, (int)deactive))
                {
                    errors.Add(MessageBuilder.Unsuccess(mac, Command.C3103));
                }
            }
            if (errors.Count > 0) return ResponseHelper.UnsuccessBuilder<TimeZoneDto>(ResponseMessage.COMMAND_UNSUCCESS,errors);

            context.TimeZones.Update(entity);
            await context.SaveChangesAsync();

            var linked = await context.TimeZoneIntervals.Where(x => x.TimeZoneId == dto.ComponentId).ToListAsync();

            context.TimeZoneIntervals.RemoveRange(linked);

            var newLinked = intervals.Select(s => new TimeZoneInterval 
            {
                TimeZoneId = dto.ComponentId,
                IntervalId = s.ComponentId,
                
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,

            }).ToList();
            context.TimeZoneIntervals.UpdateRange(newLinked);
            await context.SaveChangesAsync();

            return ResponseHelper.SuccessBuilder(dto);
        }


        public async Task<ResponseDto<IEnumerable<ModeDto>>> GetModeAsync(int param)
        {
            var dtos = await context.TimeZoneModes.AsNoTracking().Select(s => new ModeDto
            {
                Name = s.Name,
                Value = s.Value,
                Description = s.Description,

            }).ToArrayAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dtos);
        }


    }
}
