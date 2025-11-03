using AutoMapper;
using HIDAeroService.AeroLibrary;
using HIDAeroService.Constant;
using HIDAeroService.Constants;
using HIDAeroService.Data;
using HIDAeroService.DTO;
using HIDAeroService.DTO.AccessArea;
using HIDAeroService.Entity;
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

        public async Task<ResponseDto<AccessAreaDto>> GetByComponentAsync(string mac, short component)
        {
            var dto = await context.AccessAreas
                .AsNoTracking()
                .Where(x => x.MacAddress == mac && x.ComponentId == component)
                .Select(x => MapperHelper.AccessAreaToDto(x))
                .FirstOrDefaultAsync();

            return ResponseHelper.SuccessBuilder<AccessAreaDto>(dto);
        }

        public async Task<ResponseDto<bool>> CreateAsync( AccessAreaDto dto)
        {
            var max = await context.SystemSettings.AsNoTracking().Select(x => x.nArea).FirstOrDefaultAsync();
            var ComponentId = await helperService.GetLowestUnassignedNumberAsync<AccessArea>(context, dto.MacAddress,max);
            if (ComponentId == -1) return ResponseHelper.ExceedLimit<bool>();
            var ScpId = await helperService.GetIdFromMacAsync(dto.MacAddress);
            if (!await command.ConfigureAccessAreaAsync(ScpId, ComponentId, dto.MultiOccupancy, dto.AccessControl, dto.OccControl, dto.OccSet, dto.OccMax, dto.OccUp, dto.OccDown, dto.AreaFlag))
            {
                return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(dto.MacAddress, Command.C1121));
            }

            var entity = MapperHelper.DtoToAccessArea(dto,ComponentId,DateTime.Now);
            await context.AccessAreas.AddAsync(entity);
            await context.SaveChangesAsync();

            return ResponseHelper.SuccessBuilder(true);
        }

        public async Task<ResponseDto<AccessAreaDto>> UpdateAsync(AccessAreaDto dto)
        {

            var entity = await context.AccessAreas
                .Where(x => x.MacAddress == dto.MacAddress && x.ComponentId == dto.ComponentId)
                .FirstOrDefaultAsync();

            if (entity is null) return ResponseHelper.NotFoundBuilder<AccessAreaDto>();

            var ScpId = await helperService.GetIdFromMacAsync(dto.MacAddress);
            if (!await command.ConfigureAccessAreaAsync(ScpId, dto.ComponentId, dto.MultiOccupancy, dto.AccessControl, dto.OccControl, dto.OccSet, dto.OccMax, dto.OccUp, dto.OccDown, dto.AreaFlag))
            {
                return ResponseHelper.UnsuccessBuilder<AccessAreaDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(dto.MacAddress, Command.C1121));
            }

            MapperHelper.UpdateAccessArea(entity,dto);
            context.AccessAreas.Update(entity);
            await context.SaveChangesAsync();
            return ResponseHelper.SuccessBuilder(dto);
        }

        public async Task<ResponseDto<bool>> DeleteAsync(string mac, short component)
        {
            var entity = await context.AccessAreas
                .Where(x => x.ComponentId == component && x.MacAddress == mac)
                .FirstOrDefaultAsync();

            if (entity is null) return ResponseHelper.NotFoundBuilder<bool>();

            context.AccessAreas.Remove(entity);
            await context.SaveChangesAsync();
            return ResponseHelper.SuccessBuilder(true);
        }
    }
}
