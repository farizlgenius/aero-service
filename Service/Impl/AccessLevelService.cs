using AutoMapper;
using HIDAeroService.AeroLibrary;
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
    public sealed class AccessLevelService(AeroCommand command, AppDbContext context, IHelperService<AccessLevel> helperService, IMapper mapper) : IAccessLevelService
    {

        public async Task<ResponseDto<IEnumerable<AccessLevelDto>>> GetAsync()
        {
            var dtos = await context.AccessLevels
                .AsNoTracking()
                .Include(x => x.AccessLevelDoorTimeZones)
                    .ThenInclude(x => x.TimeZone)
                .Include(x => x.AccessLevelDoorTimeZones)
                    .ThenInclude(x => x.Door)
                .Select(x => MapperHelper.AccessLevelToDto(x))
                .ToArrayAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<AccessLevelDto>>(dtos);
        }



        public async Task<ResponseDto<AccessLevelDto>> GetByComponentIdAsync(short component)
        {
            var dtos = await context.AccessLevels
                .AsNoTracking()
                .Include(x => x.AccessLevelDoorTimeZones)
                .ThenInclude(x => x.AccessLevel)
                .Include(x => x.AccessLevelDoorTimeZones)
                .ThenInclude(x => x.TimeZone)
                .Include(x => x.AccessLevelDoorTimeZones)
                .ThenInclude(x => x.Door)
                .Where(x => x.ComponentId == component)
                .Select(x => MapperHelper.AccessLevelToDto(x))
                .FirstOrDefaultAsync();
            return ResponseHelper.SuccessBuilder<AccessLevelDto>(dtos);
        }



        public async Task<ResponseDto<bool>> CreateAsync(CreateUpdateAccessLevelDto dto)
        {
            List<string> errors = new List<string>();
            var max = await context.SystemSettings.Select(x => x.nAlvl).FirstOrDefaultAsync();
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
                //if (!await command.AccessLevelConfigurationExtendedCreateAsync(id, ComponentId, dto.CreateUpdateAccessLevelDoorTimeZoneDto.Where(x => x.DoorMacAddress == mac).ToList()))
                //{
                //    errors.Add(MessageBuilder.Unsuccess(mac, Command.C2116));

                //}

            }


            if (errors.Count > 0) return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.COMMAND_UNSUCCESS, errors);
            var entity = MapperHelper.DtoToAccessLevel(dto,ComponentId,DateTime.Now);
            await context.AccessLevels.AddAsync(entity);
            await context.SaveChangesAsync();
            return ResponseHelper.SuccessBuilder(true);
        }

        public async Task<ResponseDto<bool>> DeleteAsync(short ComponentId)
        {
            List<string> errors = new List<string>();
            var entity = await context.AccessLevels
                .Include(x => x.AccessLevelDoorTimeZones)
                .ThenInclude(x => x.Door)
                .FirstOrDefaultAsync(x => x.ComponentId == ComponentId);
            if (entity is null) return ResponseHelper.NotFoundBuilder<bool>();
            foreach (var d in entity.AccessLevelDoorTimeZones)
            {
                var ScpId = await helperService.GetIdFromMacAsync(d.Door.MacAddress);
                if (!await command.AccessLevelConfigurationExtendedAsync(ScpId,ComponentId, 0))
                {
                    errors.Add(MessageBuilder.Unsuccess(d.Door.MacAddress,Command.C2116));
                }
            }
            if (errors.Count > 0) return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.COMMAND_UNSUCCESS,errors);
            context.AccessLevels.Remove(entity);
            await context.SaveChangesAsync();
            return ResponseHelper.SuccessBuilder(true);
        }

        public async Task<ResponseDto<AccessLevelDto>> UpdateAsync(CreateUpdateAccessLevelDto dto)
        {

            var entity = await context.AccessLevels
                .Include(x => x.AccessLevelDoorTimeZones)
                .ThenInclude(x => x.AccessLevel)
                .Include(x => x.AccessLevelDoorTimeZones)
                .ThenInclude(x => x.TimeZone)
                .Include(x => x.AccessLevelDoorTimeZones)
                .ThenInclude(x => x.Door)
                .FirstOrDefaultAsync(x => x.ComponentId == dto.ComponentId);

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
                if (!await command.AccessLevelConfigurationExtendedCreateAsync(id, dto.ComponentId, dto.CreateUpdateAccessLevelDoorTimeZoneDto
                    .Where(x => x.DoorMacAddress == mac).ToList()))
                {
                    errors.Add(MessageBuilder.Unsuccess(mac, Command.C2116));

                }

            }

            if (errors.Count > 0) return ResponseHelper.UnsuccessBuilder<AccessLevelDto>(ResponseMessage.COMMAND_UNSUCCESS, errors);
            MapperHelper.DtoToAccessLevel(dto, dto.ComponentId, DateTime.Now);
            context.AccessLevels.Update(entity);
            await context.SaveChangesAsync();

            var res = await context.AccessLevels
                .AsNoTracking()
                .Include(x => x.AccessLevelDoorTimeZones)
                .ThenInclude(x => x.TimeZone)
                .Include(x => x.AccessLevelDoorTimeZones)
                .ThenInclude(x => x.Door)
                .Where(x => x.ComponentId == dto.ComponentId)
                .Select(x => MapperHelper.AccessLevelToDto(x))
                .FirstOrDefaultAsync();

            return ResponseHelper.SuccessBuilder(res);
        }


        public string GetAcrName(string mac, short component)
        {
            return context.Doors
                .Where(x => x.MacAddress == mac && x.ComponentId == component).Select(x => x.Name).FirstOrDefault() ?? "";
        }

        public string GetTzName(short component)
        {
            return context.TimeZones.Where(x => x.ComponentId == component).Select(x => x.Name).FirstOrDefault() ?? "";
        }
    }
}
