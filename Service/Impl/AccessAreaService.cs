using AutoMapper;
using HID.Aero.ScpdNet.Wrapper;
using HIDAeroService.AeroLibrary;
using HIDAeroService.Constant;
using HIDAeroService.Constants;
using HIDAeroService.Data;
using HIDAeroService.DTO;
using HIDAeroService.DTO.AccessArea;
using HIDAeroService.Entity;
using HIDAeroService.Entity.Interface;
using HIDAeroService.Helpers;
using HIDAeroService.Mapper;
using HIDAeroService.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop.Infrastructure;
using System.ComponentModel;

namespace HIDAeroService.Service.Impl
{
    public class AccessAreaService(AppDbContext context, IHelperService<AccessArea> helperService, AeroCommand command) : IAccessAreaService
    {

        public async Task<ResponseDto<IEnumerable<AccessAreaDto>>> GetAsync()
        {
            var dtos = await context.AccessAreas
                .AsNoTracking()
                .Select(x => MapperHelper.AccessAreaToDto(x))
                .ToArrayAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<AccessAreaDto>>(dtos);
        }

        public async Task<ResponseDto<AccessAreaDto>> GetByComponentAsync(short component)
        {
            var dto = await context.AccessAreas
                .AsNoTracking()
                .Where(x => x.ComponentId == component)
                .Select(x => MapperHelper.AccessAreaToDto(x))
                .FirstOrDefaultAsync();

            return ResponseHelper.SuccessBuilder<AccessAreaDto>(dto);
        }

        public async Task<ResponseDto<bool>> CreateAsync(AccessAreaDto dto)
        {
            var max = await context.SystemSettings
                .AsNoTracking()
                .Select(x => x.nArea)
                .FirstOrDefaultAsync();

            var ComponentId = await helperService.GetLowestUnassignedNumberAsync<AccessArea>(context, max);
            if (ComponentId == -1) return ResponseHelper.ExceedLimit<bool>();

            var hardwares = await context.Hardwares
                .Select(x => x.MacAddress)
                .ToArrayAsync();

            foreach(var mac in hardwares)
            {
                var ScpId = await helperService.GetIdFromMacAsync(mac);
                if (!await command.ConfigureAccessAreaAsync(ScpId, ComponentId, dto.MultiOccupancy, dto.AccessControl, dto.OccControl, dto.OccSet, dto.OccMax, dto.OccUp, dto.OccDown, dto.AreaFlag))
                {
                    return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(mac, Command.C1121));
                }
            }


            var entity = MapperHelper.DtoToAccessArea(dto,ComponentId,DateTime.Now);
            await context.AccessAreas.AddAsync(entity);
            await context.SaveChangesAsync();

            return ResponseHelper.SuccessBuilder(true);
        }

        public async Task<ResponseDto<AccessAreaDto>> UpdateAsync(AccessAreaDto dto)
        {

            var entity = await context.AccessAreas
                .Where(x => x.ComponentId == dto.ComponentId)
                .FirstOrDefaultAsync();

            if (entity is null) return ResponseHelper.NotFoundBuilder<AccessAreaDto>();


            var hardwares = await context.Hardwares
                .Select(x => x.MacAddress)
                .ToArrayAsync();

            foreach (var mac in hardwares)
            {
                var ScpId = await helperService.GetIdFromMacAsync(mac);
                if (!await command.ConfigureAccessAreaAsync(ScpId, dto.ComponentId, dto.MultiOccupancy, dto.AccessControl, dto.OccControl, dto.OccSet, dto.OccMax, dto.OccUp, dto.OccDown, dto.AreaFlag))
                {
                    return ResponseHelper.UnsuccessBuilder<AccessAreaDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(mac, Command.C1121));
                }
            }

            MapperHelper.UpdateAccessArea(entity,dto);
            context.AccessAreas.Update(entity);
            await context.SaveChangesAsync();
            return ResponseHelper.SuccessBuilder(dto);
        }

        public async Task<ResponseDto<bool>> DeleteAsync(short component)
        {
            var entity = await context.AccessAreas
                .Where(x => x.ComponentId == component)
                .FirstOrDefaultAsync();

            if (entity is null) return ResponseHelper.NotFoundBuilder<bool>();

            context.AccessAreas.Remove(entity);
            await context.SaveChangesAsync();
            return ResponseHelper.SuccessBuilder(true);
        }

        public async Task<ResponseDto<IEnumerable<ModeDto>>> GetCommandAsync()
        {
            var dto = await context.AccessAreaCommandOptions
                .Select(x => new ModeDto 
                {
                    Name = x.Name,
                    Value = x.Value,
                    Description = x.Description
                })
                .ToArrayAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dto);
        }

        public async Task<ResponseDto<IEnumerable<ModeDto>>> GetAccessControlOptionAsync()
        {
            var dto = await context.AccessAreaAccessControlOptions
                .Select(x => new ModeDto
                {
                    Name = x.Name,
                    Value = x.Value,
                    Description = x.Description
                })
                .ToArrayAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dto);
        }

        public async Task<ResponseDto<IEnumerable<ModeDto>>> GetOccupancyControlOptionAsync()
        {
            var dto = await context.OccupancyControlOptions
                .Select(x => new ModeDto
                {
                    Name = x.Name,
                    Value = x.Value,
                    Description = x.Description
                })
                .ToArrayAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dto);
        }

        public async Task<ResponseDto<IEnumerable<ModeDto>>> GetAreaFlagOptionAsync()
        {
            var dto = await context.AreaFlagOptions
                .Select(x => new ModeDto
                {
                    Name = x.Name,
                    Value = x.Value,
                    Description = x.Description
                })
                .ToArrayAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dto);
        }

        public async Task<ResponseDto<IEnumerable<ModeDto>>> GetMultiOccupancyOptionAsync()
        {
            var dto = await context.MultiOccupancyOptions
                .Select(x => new ModeDto
                {
                    Name = x.Name,
                    Value = x.Value,
                    Description = x.Description
                })
                .ToArrayAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dto);
        }
    }
}
