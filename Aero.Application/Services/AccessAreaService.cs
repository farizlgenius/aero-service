
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
    public sealed class AccessAreaService(IAreaRepository areaRepo,IHwRepository hw,IAreaCommand area,IAreaRepository rArea) : IAccessAreaService
    {

        public async Task<ResponseDto<IEnumerable<AccessAreaDto>>> GetAsync()
        {
            var dtos = await areaRepo.GetAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<AccessAreaDto>>(dtos);
        }

        public async Task<ResponseDto<AccessAreaDto>> GetByComponentAsync(short id)
        {
            var dto = await areaRepo.GetByIdAsync(id);
            return ResponseHelper.SuccessBuilder<AccessAreaDto>(dto);
        }

        public async Task<ResponseDto<AccessAreaDto>> CreateAsync(AccessAreaDto dto)
        {

            var ComponentId = await areaRepo.GetLowestUnassignedNumberAsync(10,"");
            if (ComponentId == -1) return ResponseHelper.ExceedLimit<AccessAreaDto>();

            var domain = AreaMapper.ToDomain(dto);

            var macs = await hw.GetMacsAsync();

            foreach(var mac in macs)
            {
               var ScpId = await hw.GetComponentIdFromMacAsync(mac);
               if (!await area.ConfigureAccessArea(ScpId, ComponentId, domain.MultiOccupancy, domain.AccessControl, domain.OccControl, domain.OccSet, domain.OccMax, domain.OccUp, domain.OccDown, domain.AreaFlag))
               {
                   return ResponseHelper.UnsuccessBuilderWithString<AccessAreaDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(mac, Command.AREA_CONFIG));
               }
            }

            var status = await rArea.AddAsync(domain);

            if(status <= 0) return ResponseHelper.UnsuccessBuilder<AccessAreaDto>(ResponseMessage.SAVE_DATABASE_UNSUCCESS,[]);

            return ResponseHelper.SuccessBuilder(true);
        }

        public async Task<ResponseDto<AccessAreaDto>> UpdateAsync(AccessAreaDto dto)
        {

            if (await areaRepo.IsAnyById(dto.DriverId)) return ResponseHelper.NotFoundBuilder<AccessAreaDto>();

            var domain = AreaMapper.ToDomain(dto);


            var macs = await hw.GetMacsAsync();

            foreach (var mac in macs)
            {
                var ScpId = await hw.GetComponentIdFromMacAsync(mac);
                if (!await area.ConfigureAccessArea(ScpId, dto.DriverId, dto.MultiOccupancy, dto.AccessControl, dto.OccControl, dto.OccSet, dto.OccMax, dto.OccUp, dto.OccDown, dto.AreaFlag))
                {
                    return ResponseHelper.UnsuccessBuilderWithString<AccessAreaDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(mac, Command.AREA_CONFIG));
                }
            }

            var status = await rArea.UpdateAsync(domain);
            if(status <= 0) ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.UPDATE_RECORD_UNSUCCESS,[]);
            return ResponseHelper.SuccessBuilder(dto);
        }

        public async Task<ResponseDto<AccessAreaDto>> DeleteAsync(short id)
        {
            if (await areaRepo.IsAnyById(id)) return ResponseHelper.NotFoundBuilder<AccessAreaDto>();

            var status = await rArea.DeleteByIdAsync(id);
            if(status <= 0) return ResponseHelper.UnsuccessBuilder<AccessAreaDto>(ResponseMessage.DELETE_DATABASE_UNSUCCESS,[]);
            return ResponseHelper.SuccessBuilder(true);
        }

        public async Task<ResponseDto<IEnumerable<Mode>>> GetCommandAsync()
        {
            var dto = await areaRepo.GetCommandAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<Mode>>(dto);
        }

        public async Task<ResponseDto<IEnumerable<Mode>>> GetAccessControlOptionAsync()
        {
            var dto = await areaRepo.GetAccessControlOptionAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<Mode>>(dto);
        }

        public async Task<ResponseDto<IEnumerable<Mode>>> GetOccupancyControlOptionAsync()
        {
            var dto = await areaRepo.GetOccupancyControlOptionAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<Mode>>(dto);
        }

        public async Task<ResponseDto<IEnumerable<Mode>>> GetAreaFlagOptionAsync()
        {
            var dto = await areaRepo.GetAreaFlagOptionAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<Mode>>(dto);
        }

        public async Task<ResponseDto<IEnumerable<Mode>>> GetMultiOccupancyOptionAsync()
        {
            var dto = await areaRepo.GetMultiOccupancyOptionAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<Mode>>(dto);
        }

        public async Task<ResponseDto<IEnumerable<AccessAreaDto>>> GetByLocationIdAsync(short location)
        {
            var dto = await areaRepo.GetByLocationIdAsync(location);
            return ResponseHelper.SuccessBuilder<IEnumerable<AccessAreaDto>>(dto);
        }

        public async Task<ResponseDto<Pagination<AccessAreaDto>>> GetPaginationAsync(PaginationParamsWithFilter param, short location)
        {
            var dto = await areaRepo.GetPaginationAsync(param,location);
            return ResponseHelper.SuccessBuilder<Pagination<AccessAreaDto>>(dto);
        }
    }
}
