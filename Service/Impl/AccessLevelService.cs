using HIDAeroService.Aero.CommandService;
using HIDAeroService.Aero.CommandService.Impl;
using HIDAeroService.Constant;
using HIDAeroService.Constants;
using HIDAeroService.Data;
using HIDAeroService.DTO;
using HIDAeroService.DTO.AccessLevel;
using HIDAeroService.Entity;
using HIDAeroService.Helpers;
using HIDAeroService.Mapper;
using HIDAeroService.Utility;
using Microsoft.EntityFrameworkCore;


namespace HIDAeroService.Service.Impl
{
    public sealed class AccessLevelService(AeroCommandService command, AppDbContext context, IHelperService<AccessLevel> helperService) : IAccessLevelService
    {

        public async Task<ResponseDto<IEnumerable<AccessLevelDto>>> GetAsync()
        {
            var dtos = await context.accesslevel
                .AsNoTracking()
                .Include(x => x.accessleve_door_timezones)
                    .ThenInclude(x => x.timezone)
                .Include(x => x.accessleve_door_timezones)
                    .ThenInclude(x => x.door)
                .Select(x => MapperHelper.AccessLevelToDto(x))
                .ToArrayAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<AccessLevelDto>>(dtos);
        }

        public async Task<ResponseDto<IEnumerable<AccessLevelDto>>> GetByLocationIdAsync(short location)
        {
            var dtos = await context.accesslevel
                .AsNoTracking()
                .Include(x => x.accessleve_door_timezones)
                    .ThenInclude(x => x.timezone)
                .Include(x => x.accessleve_door_timezones)
                    .ThenInclude(x => x.door)
                .Where(x => x.location_id == location)
                .Select(x => MapperHelper.AccessLevelToDto(x))
                .ToArrayAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<AccessLevelDto>>(dtos);
        }



        public async Task<ResponseDto<AccessLevelDto>> GetByComponentIdAsync(short component)
        {
            var dtos = await context.accesslevel
                .AsNoTracking()
                .Include(x => x.accessleve_door_timezones)
                .ThenInclude(x => x.accesslevel)
                .Include(x => x.accessleve_door_timezones)
                .ThenInclude(x => x.timezone)
                .Include(x => x.accessleve_door_timezones)
                .ThenInclude(x => x.door)
                .Where(x => x.component_id == component)
                .Select(x => MapperHelper.AccessLevelToDto(x))
                .FirstOrDefaultAsync();
            return ResponseHelper.SuccessBuilder<AccessLevelDto>(dtos);
        }



        public async Task<ResponseDto<bool>> CreateAsync(CreateUpdateAccessLevelDto dto)
        {
            List<string> errors = new List<string>();
            var max = await context.system_setting.Select(x => x.n_alvl).FirstOrDefaultAsync();
            var ComponentId = await helperService.GetLowestUnassignedNumberAsync<AccessLevel>(context,max);
            if (ComponentId == -1) return ResponseHelper.ExceedLimit<bool>();

            var macs = dto.CreateUpdateAccessLevelDoorTimeZoneDto
                .Select(x => x.DoorMacAddress)
                .Distinct()
                .ToList();


            foreach (var mac in macs)
            {
                var id = await helperService.GetIdFromMacAsync(mac);
                if(id == 0) errors.Add(MessageBuilder.Notfound());
                //if (!await command.AccessLevelConfigurationExtendedCreate(id, component_id, dto.CreateUpdateAccessLevelDoorTimeZoneDto.Where(x => x.DoorMacAddress == mac).ToList()))
                //{
                //    errors.Add(MessageBuilder.Unsuccess(mac, command.C2116));

                //}

            }


            if (errors.Count > 0) return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.COMMAND_UNSUCCESS, errors);
            var entity = MapperHelper.DtoToAccessLevel(dto,ComponentId,DateTime.Now);
            await context.accesslevel.AddAsync(entity);
            await context.SaveChangesAsync();
            return ResponseHelper.SuccessBuilder(true);
        }

        public async Task<ResponseDto<bool>> DeleteAsync(short ComponentId)
        {
            List<string> errors = new List<string>();
            var entity = await context.accesslevel
                .Include(x => x.accessleve_door_timezones)
                .ThenInclude(x => x.door)
                .FirstOrDefaultAsync(x => x.component_id == ComponentId);
            if (entity is null) return ResponseHelper.NotFoundBuilder<bool>();
            foreach (var d in entity.accessleve_door_timezones)
            {
                var ScpId = await helperService.GetIdFromMacAsync(d.door.hardware_mac);
                if (!command.AccessLevelConfigurationExtended(ScpId,ComponentId, 0))
                {
                    errors.Add(MessageBuilder.Unsuccess(d.door.hardware_mac, Command.C2116));
                }
            }
            if (errors.Count > 0) return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.COMMAND_UNSUCCESS,errors);
            context.accesslevel.Remove(entity);
            await context.SaveChangesAsync();
            return ResponseHelper.SuccessBuilder(true);
        }

        public async Task<ResponseDto<AccessLevelDto>> UpdateAsync(CreateUpdateAccessLevelDto dto)
        {

            var entity = await context.accesslevel
                .Include(x => x.accessleve_door_timezones)
                .ThenInclude(x => x.accesslevel)
                .Include(x => x.accessleve_door_timezones)
                .ThenInclude(x => x.timezone)
                .Include(x => x.accessleve_door_timezones)
                .ThenInclude(x => x.door)
                .FirstOrDefaultAsync(x => x.component_id == dto.component_id);

            if (entity is null) return ResponseHelper.NotFoundBuilder<AccessLevelDto>();
            List<string> errors = new List<string>();
            var macs = dto.CreateUpdateAccessLevelDoorTimeZoneDto
                .Select(x => x.DoorMacAddress)
                .Distinct()
                .ToList();


            foreach (var mac in macs)
            {
                var id = await helperService.GetIdFromMacAsync(mac);
                if (id == 0) errors.Add(MessageBuilder.Notfound());
                if (!command.AccessLevelConfigurationExtendedCreate(id, dto.component_id, dto.CreateUpdateAccessLevelDoorTimeZoneDto
                    .Where(x => x.DoorMacAddress == mac).ToList()))
                {
                    errors.Add(MessageBuilder.Unsuccess(mac, Command.C2116));

                }

            }

            if (errors.Count > 0) return ResponseHelper.UnsuccessBuilder<AccessLevelDto>(ResponseMessage.COMMAND_UNSUCCESS, errors);
            MapperHelper.DtoToAccessLevel(dto, dto.component_id, DateTime.Now);
            context.accesslevel.Update(entity);
            await context.SaveChangesAsync();

            var res = await context.accesslevel
                .AsNoTracking()
                .Include(x => x.accessleve_door_timezones)
                .ThenInclude(x => x.timezone)
                .Include(x => x.accessleve_door_timezones)
                .ThenInclude(x => x.door)
                .Where(x => x.component_id == dto.component_id)
                .Select(x => MapperHelper.AccessLevelToDto(x))
                .FirstOrDefaultAsync();

            return ResponseHelper.SuccessBuilder(res);
        }


        public string GetAcrName(string mac, short component)
        {
            return context.door
                .Where(x => x.hardware_mac == mac && x.component_id == component).Select(x => x.name).FirstOrDefault() ?? "";
        }

        public string GetTzName(short component)
        {
            return context.timezone.Where(x => x.component_id == component).Select(x => x.name).FirstOrDefault() ?? "";
        }
    }
}
