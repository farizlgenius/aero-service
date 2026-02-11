
using Aero.Api.Constants;
using Aero.Application.Constants;
using Aero.Application.DTOs;
using Aero.Application.Helpers;
using Aero.Application.Interface;
using Aero.Application.Interfaces;
using Aero.Application.Mapper;
using Aero.Domain.Entities;
using Aero.Domain.Interface;

namespace AeroService.Service.Impl
{
    public sealed class AccessAreaService(IQAreaRepository qArea,IQHwRepository qHw,IAreaCommand area,IAreaRepository rArea) : IAccessAreaService
    {

        public async Task<ResponseDto<IEnumerable<AccessAreaDto>>> GetAsync()
        {
            var dtos = await qArea.GetAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<AccessAreaDto>>(dtos);
        }

        public async Task<ResponseDto<AccessAreaDto>> GetByComponentAsync(short component)
        {
            var dto = await qArea.GetByComponentIdAsync(component);
            return ResponseHelper.SuccessBuilder<AccessAreaDto>(dto);
        }

        public async Task<ResponseDto<bool>> CreateAsync(AccessAreaDto dto)
        {

            var ComponentId = await qArea.GetLowestUnassignedNumberAsync(10,"");
            if (ComponentId == -1) return ResponseHelper.ExceedLimit<bool>();

            var domain = AreaMapper.ToDomain(dto);

            var macs = await qHw.GetMacsAsync();

            foreach(var mac in macs)
            {
               var ScpId = await qHw.GetComponentIdFromMacAsync(mac);
               if (!area.ConfigureAccessArea(ScpId, ComponentId, domain.MultiOccupancy, domain.AccessControl, domain.OccControl, domain.OccSet, domain.OccMax, domain.OccUp, domain.OccDown, domain.AreaFlag))
               {
                   return ResponseHelper.UnsuccessBuilderWithString<bool>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(mac, Command.AREA_CONFIG));
               }
            }

            var status = await rArea.AddAsync(domain);

            if(status <= 0) return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.SAVE_DATABASE_UNSUCCESS,[]);

            return ResponseHelper.SuccessBuilder(true);
        }

        public async Task<ResponseDto<AccessAreaDto>> UpdateAsync(AccessAreaDto dto)
        {

            if (await qArea.IsAnyByComponentId(dto.component_id)) return ResponseHelper.NotFoundBuilder<AccessAreaDto>();

            var domain = AreaMapper.ToDomain(dto);


            var macs = await qHw.GetMacsAsync();

            foreach (var mac in macs)
            {
                var ScpId = await qHw.GetComponentIdFromMacAsync(mac);
                if (!area.ConfigureAccessArea(ScpId, dto.component_id, dto.MultiOccupancy, dto.AccessControl, dto.OccControl, dto.OccSet, dto.OccMax, dto.OccUp, dto.OccDown, dto.AreaFlag))
                {
                    return ResponseHelper.UnsuccessBuilderWithString<AccessAreaDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(mac, Command.AREA_CONFIG));
                }
            }

            var status = await rArea.UpdateAsync(domain);
            if(status <= 0) ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.UPDATE_RECORD_UNSUCCESS,[]);
            return ResponseHelper.SuccessBuilder(dto);
        }

        public async Task<ResponseDto<bool>> DeleteAsync(short component)
        {
            if (await qArea.IsAnyByComponentId(component)) return ResponseHelper.NotFoundBuilder<bool>();

            var status = await rArea.DeleteByComponentIdAsync(component);
            if(status <= 0) return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.DELETE_DATABASE_UNSUCCESS,[]);
            return ResponseHelper.SuccessBuilder(true);
        }

        public async Task<ResponseDto<IEnumerable<Mode>>> GetCommandAsync()
        {
            var dto = await qArea.GetCommandAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<Mode>>(dto);
        }

        public async Task<ResponseDto<IEnumerable<Mode>>> GetAccessControlOptionAsync()
        {
            var dto = await qArea.GetAccessControlOptionAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<Mode>>(dto);
        }

        public async Task<ResponseDto<IEnumerable<Mode>>> GetOccupancyControlOptionAsync()
        {
            var dto = await qArea.GetOccupancyControlOptionAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<Mode>>(dto);
        }

        public async Task<ResponseDto<IEnumerable<Mode>>> GetAreaFlagOptionAsync()
        {
            var dto = await qArea.GetAreaFlagOptionAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<Mode>>(dto);
        }

        public async Task<ResponseDto<IEnumerable<Mode>>> GetMultiOccupancyOptionAsync()
        {
            var dto = await qArea.GetMultiOccupancyOptionAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<Mode>>(dto);
        }

        public async Task<ResponseDto<IEnumerable<AccessAreaDto>>> GetByLocationIdAsync(short location)
        {
            var dto = await qArea.GetByLocationIdAsync(location);
            return ResponseHelper.SuccessBuilder<IEnumerable<AccessAreaDto>>(dto);
        }

        public async Task<ResponseDto<Pagination<AccessAreaDto>>> GetPaginationAsync(PaginationParamsWithFilter param, short location)
        {
            var dto = await qArea.GetPaginationAsync(param,location);
            return ResponseHelper.SuccessBuilder<Pagination<AccessAreaDto>>(dto);
        }
    }
}
