
using Aero.Application.DTOs;
using Aero.Application.Helpers;
using Aero.Application.Interface;
using Aero.Application.Interfaces;

namespace Aero.Application.Services
{
    public class TimeZoneService(IQTzRepository qTz) : ITimeZoneService
    {
        public async Task<ResponseDto<IEnumerable<TimeZoneDto>>> GetAsync()
        {
            var dtos = await qTz.GetAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<TimeZoneDto>>(dtos);
        }

        public async Task<ResponseDto<TimeZoneDto>> GetByComponentIdAsync(short component)
        {
            var dto = await qTz.GetByComponentIdAsync(component);

            if (dto is null) return ResponseHelper.NotFoundBuilder<TimeZoneDto>();
            return ResponseHelper.SuccessBuilder<TimeZoneDto>(dto);
        }

        public async Task<ResponseDto<bool>> CreateAsync(CreateTimeZoneDto dto)
        {
            List<string> errors = new List<string>();
            var max = await context.system_setting.AsNoTracking().Select(x => x.n_tz).FirstOrDefaultAsync();
            var ComponentId = await helperService.GetLowestUnassignedNumberAsync<Entity.TimeZone>(context,max);
            if (ComponentId == -1) return ResponseHelper.ExceedLimit<bool>();

            var timezone = MapperHelper.CreateTimeZoneDtoToTimeZone(dto,ComponentId);
            List<Interval> intervals = new List<Interval>();
            foreach(var interval in dto.Intervals)
            {
                intervals.Add(MapperHelper.DtoToInterval(interval));
            }
            

            List<string> macs = await context.hardware.AsNoTracking().Select(x => x.mac).ToListAsync();

            foreach (var mac in macs)
            {
                short id = await helperService.GetIdFromMacAsync(mac);
                long active = helperService.DateTimeToElapeSecond(dto.ActiveTime);
                long deactive = helperService.DateTimeToElapeSecond(dto.DeactiveTime);
                //if (!await command.ExtendedTimeZoneActSpecificationAsync(id, timezone,interval, (int)active, (int)deactive))
                //{
                //    errors.Add(MessageBuilder.Unsuccess(mac, command.C3103));
                //}
            }
            if (errors.Count > 0) return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.COMMAND_UNSUCCESS,errors);

            await context.timezone.AddAsync(timezone);
            await context.SaveChangesAsync();

            List<TimeZoneInterval> links = new List<TimeZoneInterval>();
            foreach (var interval in dto.Intervals)
            {
                links.Add(
                    new TimeZoneInterval
                    {
                        uuid = dto.Uuid,
                        is_active = dto.IsActive,

                        interval_id = interval.ComponentId,
                        timezone_id = ComponentId
                    }
                );
            }

            await context.timezone_interval.AddRangeAsync(links);
            await context.SaveChangesAsync();

            return ResponseHelper.SuccessBuilder(true);
        }

        public async Task<ResponseDto<bool>> DeleteAsync(short component)
        {
            List<string> errors = new List<string>();
            var entity = await context.timezone
                .Include(s => s.timezone_intervals)
                .FirstOrDefaultAsync(x => x.component_id == component);

            if (entity is null) return ResponseHelper.NotFoundBuilder<bool>();

            var hw = await context.hardware
                .AsNoTracking()
                .Where(x => x.location_id == entity.location_id)
                .Select(x => x.component_id)
                .ToArrayAsync();

            foreach(var id in hw)
            {
                //if (!await command.TimeZoneControlAsync(id,component,3))
                //{
                //    errors.Add(MessageBuilder.Unsuccess(await helperService.GetMacFromIdAsync(id),command.C314));
                //}
            }

            if (errors.Count > 0) return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.COMMAND_UNSUCCESS,errors);

            context.timezone.Remove(entity);
            await context.SaveChangesAsync();

            return ResponseHelper.SuccessBuilder<bool>(true);
        }

        public async Task<ResponseDto<TimeZoneDto>> UpdateAsync(TimeZoneDto dto)
        {
            List<string> errors = new List<string>();
            var entity = await context.timezone
                .Include(x => x.timezone_intervals)
                .ThenInclude(x => x.interval)
                .FirstOrDefaultAsync(p => p.component_id == dto.ComponentId);
            if (entity is null) return ResponseHelper.NotFoundBuilder<TimeZoneDto>();

            entity = MapperHelper.TimeZoneDtoMapTimeZone(dto,entity);
            var intervals = dto.Intervals.Select(s => MapperHelper.DtoToInterval(s)).ToList();

            List<string> macs = await context.hardware.AsNoTracking().Select(x => x.mac).ToListAsync();
            foreach (var mac in macs)
            {
                short id = await helperService.GetIdFromMacAsync(mac);
                long active = helperService.DateTimeToElapeSecond(dto.ActiveTime);
                long deactive = helperService.DateTimeToElapeSecond(dto.DeactiveTime);
                //if (!await command.ExtendedTimeZoneActSpecificationAsync(id, entity, interval, (int)active, (int)deactive))
                //{
                //    errors.Add(MessageBuilder.Unsuccess(mac, command.C3103));
                //}
            }
            if (errors.Count > 0) return ResponseHelper.UnsuccessBuilder<TimeZoneDto>(ResponseMessage.COMMAND_UNSUCCESS,errors);

            context.timezone.Update(entity);
            await context.SaveChangesAsync();

            var linked = await context.timezone_interval.Where(x => x.timezone_id == dto.ComponentId).ToListAsync();

            context.timezone_interval.RemoveRange(linked);

            var newLinked = intervals.Select(s => new TimeZoneInterval 
            {
                timezone_id = dto.ComponentId,
                interval_id = s.component_id,
                
                created_date = DateTime.UtcNow,
                updated_date = DateTime.UtcNow,

            }).ToList();
            context.timezone_interval.UpdateRange(newLinked);
            await context.SaveChangesAsync();

            return ResponseHelper.SuccessBuilder(dto);
        }


        public async Task<ResponseDto<IEnumerable<ModeDto>>> GetModeAsync(int param)
        {
            var dtos = await context.timezone_mode.AsNoTracking().Select(s => new ModeDto
            {
                Name = s.name,
                Value = s.value,
                Description = s.description,

            }).ToArrayAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dtos);
        }

        public async Task<ResponseDto<IEnumerable<ModeDto>>> GetCommandAsync()
        {
            var dtos = await context.timezone_command.AsNoTracking().Select(s => new ModeDto
            {
                Name = s.name,
                Value = s.value,
                Description = s.description,

            }).ToArrayAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dtos);
        }

        public async Task<ResponseDto<IEnumerable<TimeZoneDto>>> GetByLocationAsync(short location)
        {
            var dtos = await context.timezone
                .AsNoTracking()
                .Include(c => c.timezone_intervals)
                .ThenInclude(x => x.interval)
                .ThenInclude(x => x.days)
                .Where(x => x.location_id == location)
                .Select(x => MapperHelper.TimeZoneToDto(x))
                .ToArrayAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<TimeZoneDto>>(dtos);
        }
    }
}
