using HIDAeroService.AeroLibrary;
using HIDAeroService.Constant;
using HIDAeroService.Constants;
using HIDAeroService.Data;
using HIDAeroService.DTO;
using HIDAeroService.DTO.MonitorGroup;
using HIDAeroService.Entity;
using HIDAeroService.Helpers;
using HIDAeroService.Mapper;
using HIDAeroService.Utility;
using Microsoft.EntityFrameworkCore;

namespace HIDAeroService.Service.Impl
{
    public sealed class MonitorGroupService(AppDbContext context,AeroCommand command,IHelperService<MonitorGroup> helperService) : IMonitorGroupService
    {
        public async Task<ResponseDto<bool>> CreateAsync(MonitorGroupDto dto)
        {

            if (!await context.Hardwares.AnyAsync(x => x.MacAddress.Equals(dto.MacAddress))) return ResponseHelper.NotFoundBuilder<bool>();

            var ScpId = await helperService.GetIdFromMacAsync(dto.MacAddress);
            if(ScpId == 0) return ResponseHelper.NotFoundBuilder<bool>();

            var ComponentId = await helperService.GetLowestUnassignedNumberAsync<MonitorGroup>(context,128);
            if (ComponentId == -1) return ResponseHelper.ExceedLimit<bool>();

            var entity = MapperHelper.DtoToMonitorGroup(dto,ComponentId,DateTime.Now);

            if (!await command.ConfigureMonitorPointGroup(ScpId,ComponentId,dto.nMpCount,entity.nMpList.ToList()))
            {
                return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(dto.MacAddress, Command.C120));
            }

            await context.AddAsync(entity);
            await context.SaveChangesAsync();

            return ResponseHelper.SuccessBuilder<bool>(true);
        }

        public async Task<ResponseDto<bool>> DeleteAsync(string mac, short Component)
        {
            var en = await context.MonitorGroups
                .Where(x => x.MacAddress == mac && x.ComponentId == Component)
                .FirstOrDefaultAsync();

            if (en is null) return ResponseHelper.NotFoundBuilder<bool>();

            var ScpId = await helperService.GetIdFromMacAsync(mac);

            if(!await command.ConfigureMonitorPointGroup(ScpId, Component, 0, []))
            {
                return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(mac, Command.C120));
            }

            context.Remove(en);
            await context.SaveChangesAsync();

            return ResponseHelper.SuccessBuilder(true);
        }

        public async Task<ResponseDto<IEnumerable<MonitorGroupDto>>> GetAsync()
        {
            var dto = await context.MonitorGroups
                .AsNoTracking()
                .Include(x => x.nMpList)
                .Select(x => MapperHelper.MonitorGroupToDto(x))
                .ToArrayAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<MonitorGroupDto>>(dto);
        }

        public async Task<ResponseDto<IEnumerable<MonitorGroupDto>>> GetByLocationAsync(short location)
        {
            var dto = await context.MonitorGroups
                .AsNoTracking()
                .Include(x => x.nMpList)
                .Where(x => x.LocationId == location)
                .Select(x => MapperHelper.MonitorGroupToDto(x))
                .ToArrayAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<MonitorGroupDto>>(dto);
        }

        public async Task<ResponseDto<IEnumerable<ModeDto>>> GetCommandAsync()
        {
            var dtos = await context.MonitorGroupCommands
                 .AsNoTracking()
                 .Select(x => new ModeDto 
                 {
                     Name = x.Name,
                     Value = x.Value,
                     Description = x.Description,
                 })
                 .ToArrayAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dtos);
        
        }


        public async Task<ResponseDto<IEnumerable<ModeDto>>> GetTypeAsync()
        {
            var dtos = await context.MonitorGroupTypes
                 .AsNoTracking()
                 .Select(x => new ModeDto 
                 {
                     Name = x.Name,
                     Value = x.Value,
                     Description = x.Description,
                 })
                 .ToArrayAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dtos);
        }

        public async Task<ResponseDto<bool>> MonitorGroupCommandAsync(MonitorGroupCommandDto dto)
        {
            if(!await context.MonitorGroups.AnyAsync(x => x.MacAddress == dto.MacAddress && x.ComponentId == dto.ComponentId)) return ResponseHelper.NotFoundBuilder<bool>();

            var ScpId = await helperService.GetIdFromMacAsync(dto.MacAddress);

            if(!await command.MonitorPointGroupArmDisarmAsync(ScpId,dto.ComponentId,dto.Command,dto.Arg))
            {
                return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(dto.MacAddress, Command.C321));
            }

            return ResponseHelper.SuccessBuilder<bool>(true);
        }

        public async Task<ResponseDto<MonitorGroupDto>> UpdateAsync(MonitorGroupDto dto)
        {
            var en = await context.MonitorGroups
                .Include(x => x.nMpList)
                .Where(x => x.MacAddress == dto.MacAddress && x.ComponentId == dto.ComponentId)
                .FirstOrDefaultAsync();

            if (en is null) return ResponseHelper.NotFoundBuilder<MonitorGroupDto>();

            var ScpId = await helperService.GetIdFromMacAsync(dto.MacAddress);


            // Delete relate table first 
            context.MonitorGroupLists.RemoveRange(en.nMpList);
            MapperHelper.UpdateMonitorGroup(en,dto);

            if (!await command.ConfigureMonitorPointGroup(ScpId, dto.ComponentId, dto.nMpCount, en.nMpList.ToList()))
            {
                return ResponseHelper.UnsuccessBuilder<MonitorGroupDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(dto.MacAddress, Command.C120));
            }


            context.MonitorGroups.Update(en);
            await context.SaveChangesAsync();

            return ResponseHelper.SuccessBuilder<MonitorGroupDto>(dto);
                
        }
    }
}
