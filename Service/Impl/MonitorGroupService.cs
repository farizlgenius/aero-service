using AeroService.Aero.CommandService;
using AeroService.Aero.CommandService.Impl;
using AeroService.Constant;
using AeroService.Constants;
using AeroService.Data;
using AeroService.DTO;
using AeroService.DTO.MonitorGroup;
using AeroService.Entity;
using AeroService.Helpers;
using AeroService.Mapper;
using AeroService.Model;
using AeroService.Utility;
using Microsoft.EntityFrameworkCore;

namespace AeroService.Service.Impl
{
    public sealed class MonitorGroupService(AppDbContext context,AeroCommandService command,IHelperService<MonitorGroup> helperService) : IMonitorGroupService
    {
        public async Task<ResponseDto<bool>> CreateAsync(MonitorGroupDto dto)
        {

            if (!await context.hardware.AnyAsync(x => x.mac.Equals(dto.Mac))) return ResponseHelper.NotFoundBuilder<bool>();

            var ScpId = await helperService.GetIdFromMacAsync(dto.Mac);
            if(ScpId == 0) return ResponseHelper.NotFoundBuilder<bool>();

            var ComponentId = await helperService.GetLowestUnassignedNumberAsync<MonitorGroup>(context,128);
            if (ComponentId == -1) return ResponseHelper.ExceedLimit<bool>();

            var entity = MapperHelper.DtoToMonitorGroup(dto,ComponentId,DateTime.Now);

            if (!command.ConfigureMonitorPointGroup(ScpId,ComponentId,dto.nMpCount,entity.n_mp_list.ToList()))
            {
                return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(dto.Mac, Command.C120));
            }

            await context.AddAsync(entity);
            await context.SaveChangesAsync();

            return ResponseHelper.SuccessBuilder<bool>(true);
        }

        public async Task<ResponseDto<bool>> DeleteAsync(string mac, short Component)
        {
            var en = await context.monitor_group
                .Where(x => x.hardware_mac == mac && x.component_id == Component)
                .FirstOrDefaultAsync();

            if (en is null) return ResponseHelper.NotFoundBuilder<bool>();

            var ScpId = await helperService.GetIdFromMacAsync(mac);

            if(!command.ConfigureMonitorPointGroup(ScpId, Component, 0, []))
            {
                return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(mac, Command.C120));
            }

            context.Remove(en);
            await context.SaveChangesAsync();

            return ResponseHelper.SuccessBuilder(true);
        }

        public async Task<ResponseDto<IEnumerable<MonitorGroupDto>>> GetAsync()
        {
            var dto = await context.monitor_group
                .AsNoTracking()
                .Include(x => x.n_mp_list)
                .Select(en => new MonitorGroupDto
                {
                    // Base 
                    Uuid = en.uuid,
                    ComponentId = en.component_id,
                    Mac = en.hardware_mac,
                    LocationId = en.location_id,
                    IsActive = en.is_active,

                    // Detail
                    Name = en.name,
                    nMpCount = en.n_mp_count,
                    nMpList = en.n_mp_list.Select(x => new MonitorGroupListDto
                    {
                        PointType = x.point_type,
                        PointNumber = x.point_number,
                        PointTypeDesc = x.point_type_desc,
                    }).ToList(),
                })
                .ToArrayAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<MonitorGroupDto>>(dto);
        }

        public async Task<ResponseDto<IEnumerable<MonitorGroupDto>>> GetByLocationAsync(short location)
        {
            var dto = await context.monitor_group
                .AsNoTracking()
                .Include(x => x.n_mp_list)
                .Where(x => x.location_id == location)
                .Select(en => new MonitorGroupDto
                {
                    // Base 
                    Uuid = en.uuid,
                    ComponentId = en.component_id,
                    Mac = en.hardware_mac,
                    LocationId = en.location_id,
                    IsActive = en.is_active,

                    // Detail
                    Name = en.name,
                    nMpCount = en.n_mp_count,
                    nMpList = en.n_mp_list.Select(x => new MonitorGroupListDto
                    {
                        PointType = x.point_type,
                        PointNumber = x.point_number,
                        PointTypeDesc = x.point_type_desc,
                    }).ToList(),
                })
                .ToArrayAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<MonitorGroupDto>>(dto);
        }

        public async Task<ResponseDto<IEnumerable<ModeDto>>> GetCommandAsync()
        {
            var dtos = await context.monitor_group_command
                 .AsNoTracking()
                 .Select(x => new ModeDto 
                 {
                     Name = x.name,
                     Value = x.value,
                     Description = x.description,
                 })
                 .ToArrayAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dtos);
        
        }


        public async Task<ResponseDto<IEnumerable<ModeDto>>> GetTypeAsync()
        {
            var dtos = await context.monitor_group_type
                 .AsNoTracking()
                 .Select(x => new ModeDto 
                 {
                     Name = x.name,
                     Value = x.value,
                     Description = x.description,
                 })
                 .ToArrayAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dtos);
        }

        public async Task<ResponseDto<bool>> MonitorGroupCommandAsync(MonitorGroupCommandDto dto)
        {
            if(!await context.monitor_group.AnyAsync(x => x.hardware_mac == dto.MacAddress && x.component_id == dto.ComponentId)) return ResponseHelper.NotFoundBuilder<bool>();

            var ScpId = await helperService.GetIdFromMacAsync(dto.MacAddress);

            if(!command.MonitorPointGroupArmDisarm(ScpId,dto.ComponentId,dto.Command,dto.Arg))
            {
                return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(dto.MacAddress, Command.C321));
            }

            return ResponseHelper.SuccessBuilder<bool>(true);
        }

        public async Task<ResponseDto<MonitorGroupDto>> UpdateAsync(MonitorGroupDto dto)
        {
            var en = await context.monitor_group
                .Include(x => x.n_mp_list)
                .Where(x => x.hardware_mac == dto.Mac && x.component_id == dto.ComponentId)
                .FirstOrDefaultAsync();

            if (en is null) return ResponseHelper.NotFoundBuilder<MonitorGroupDto>();

            var ScpId = await helperService.GetIdFromMacAsync(dto.Mac);


            // Delete relate table first 
            context.monitor_group_list.RemoveRange(en.n_mp_list);
            MapperHelper.UpdateMonitorGroup(en,dto);

            if (!command.ConfigureMonitorPointGroup(ScpId, dto.ComponentId, dto.nMpCount, en.n_mp_list.ToList()))
            {
                return ResponseHelper.UnsuccessBuilder<MonitorGroupDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(dto.Mac, Command.C120));
            }


            context.monitor_group.Update(en);
            await context.SaveChangesAsync();

            return ResponseHelper.SuccessBuilder<MonitorGroupDto>(dto);
                
        }
    }
}
