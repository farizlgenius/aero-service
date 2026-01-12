using HID.Aero.ScpdNet.Wrapper;
using AeroService.Aero.CommandService;
using AeroService.Aero.CommandService.Impl;
using AeroService.Constant;
using AeroService.Constants;
using AeroService.Data;
using AeroService.DTO;
using AeroService.DTO.AccessArea;
using AeroService.Entity;
using AeroService.Entity.Interface;
using AeroService.Helpers;
using AeroService.Mapper;
using AeroService.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop.Infrastructure;
using System.ComponentModel;

namespace AeroService.Service.Impl
{
    public class AccessAreaService(AppDbContext context, IHelperService<Area> helperService, AeroCommandService command) : IAccessAreaService
    {

        public async Task<ResponseDto<IEnumerable<AccessAreaDto>>> GetAsync()
        {
            var dtos = await context.area
                .AsNoTracking()
                .Select(x => MapperHelper.AccessAreaToDto(x))
                .ToArrayAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<AccessAreaDto>>(dtos);
        }

        public async Task<ResponseDto<AccessAreaDto>> GetByComponentAsync(short component)
        {
            var dto = await context.area
                .AsNoTracking()
                .Where(x => x.component_id == component)
                .Select(x => MapperHelper.AccessAreaToDto(x))
                .FirstOrDefaultAsync();

            return ResponseHelper.SuccessBuilder<AccessAreaDto>(dto);
        }

        public async Task<ResponseDto<bool>> CreateAsync(AccessAreaDto dto)
        {
            var max = await context.system_setting
                .AsNoTracking()
                .Select(x => x.n_area)
                .FirstOrDefaultAsync();

            var ComponentId = await helperService.GetLowestUnassignedNumberAsync<Area>(context, max);
            if (ComponentId == -1) return ResponseHelper.ExceedLimit<bool>();

            var hardwares = await context.hardware
                .Select(x => x.mac)
                .ToArrayAsync();

            //foreach(var mac in hardware)
            //{
            //    var hardware_id = await helperService.GetIdFromMacAsync(mac);
            //    if (!await command.ConfigureAccessArea(hardware_id, component_id, dto.multi_occ, dto.access_control, dto.occ_control, dto.occ_set, dto.occ_max, dto.occ_up, dto.occ_down, dto.area_flag))
            //    {
            //        return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(mac, command.C1121));
            //    }
            //}


            var entity = MapperHelper.DtoToAccessArea(dto,ComponentId,DateTime.UtcNow);
            await context.area.AddAsync(entity);
            await context.SaveChangesAsync();

            return ResponseHelper.SuccessBuilder(true);
        }

        public async Task<ResponseDto<AccessAreaDto>> UpdateAsync(AccessAreaDto dto)
        {

            var entity = await context.area
                .Where(x => x.component_id == dto.component_id)
                .FirstOrDefaultAsync();

            if (entity is null) return ResponseHelper.NotFoundBuilder<AccessAreaDto>();


            var hardwares = await context.hardware
                .Select(x => x.mac)
                .ToArrayAsync();

            foreach (var mac in hardwares)
            {
                var ScpId = await helperService.GetIdFromMacAsync(mac);
                if (!command.ConfigureAccessArea(ScpId, dto.component_id, dto.MultiOccupancy, dto.AccessControl, dto.OccControl, dto.OccSet, dto.OccMax, dto.OccUp, dto.OccDown, dto.AreaFlag))
                {
                    return ResponseHelper.UnsuccessBuilder<AccessAreaDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(mac, Command.C1121));
                }
            }

            MapperHelper.UpdateAccessArea(entity,dto);
            context.area.Update(entity);
            await context.SaveChangesAsync();
            return ResponseHelper.SuccessBuilder(dto);
        }

        public async Task<ResponseDto<bool>> DeleteAsync(short component)
        {
            var entity = await context.area
                .Where(x => x.component_id == component)
                .FirstOrDefaultAsync();

            if (entity is null) return ResponseHelper.NotFoundBuilder<bool>();

            context.area.Remove(entity);
            await context.SaveChangesAsync();
            return ResponseHelper.SuccessBuilder(true);
        }

        public async Task<ResponseDto<IEnumerable<ModeDto>>> GetCommandAsync()
        {
            var dto = await context.access_area_command
                .Select(x => new ModeDto 
                {
                    Name = x.name,
                    Value = x.value,
                    Description = x.description
                })
                .ToArrayAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dto);
        }

        public async Task<ResponseDto<IEnumerable<ModeDto>>> GetAccessControlOptionAsync()
        {
            var dto = await context.area_access_control
                .Select(x => new ModeDto
                {
                    Name = x.name,
                    Value = x.value,
                    Description = x.description
                })
                .ToArrayAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dto);
        }

        public async Task<ResponseDto<IEnumerable<ModeDto>>> GetOccupancyControlOptionAsync()
        {
            var dto = await context.occupancy_control
                .Select(x => new ModeDto
                {
                    Name = x.name,
                    Value = x.value,
                    Description = x.description
                })
                .ToArrayAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dto);
        }

        public async Task<ResponseDto<IEnumerable<ModeDto>>> GetAreaFlagOptionAsync()
        {
            var dto = await context.area_flag
                .Select(x => new ModeDto
                {
                    Name = x.name,
                    Value = x.value,
                    Description = x.description
                })
                .ToArrayAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dto);
        }

        public async Task<ResponseDto<IEnumerable<ModeDto>>> GetMultiOccupancyOptionAsync()
        {
            var dto = await context.multi_occupancy
                .Select(x => new ModeDto
                {
                    Name = x.name,
                    Value = x.value,
                    Description = x.description
                })
                .ToArrayAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dto);
        }
    }
}
