
using Aero.Api.Constants;
using Aero.Application.Constants;
using Aero.Application.DTOs;
using Aero.Application.Helpers;
using Aero.Application.Interface;
using Aero.Application.Interfaces;
using Aero.Domain.Entities;
using Aero.Domain.Interface;

namespace AeroService.Service.Impl
{
    public sealed class AccessAreaService(IAreaRepository repo,IHwRepository hw,IAreaCommand area,ISettingRepository setting) : IAccessAreaService
    {

        public async Task<ResponseDto<IEnumerable<AccessAreaDto>>> GetAsync()
        {
            var dtos = await repo.GetAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<AccessAreaDto>>(dtos);
        }

        public async Task<ResponseDto<AccessAreaDto>> GetByComponentAsync(int id)
        {
            var dto = await repo.GetByIdAsync(id);
            return ResponseHelper.SuccessBuilder<AccessAreaDto>(dto);
        }

        public async Task<ResponseDto<AccessAreaDto>> CreateAsync(CreateAccessAreaDto dto)
        {
            // Check value in license here 
            // ....to be implement

            if (await repo.IsAnyByNameAsync(dto.Name.Trim())) return ResponseHelper.BadRequestName<AccessAreaDto>();

            var ScpSetting = await setting.GetScpSettingAsync();

            var DriverId = await repo.GetLowestUnassignedNumberAsync(ScpSetting.nArea);
            if (DriverId == -1) return ResponseHelper.ExceedLimit<AccessAreaDto>();

            var domain = new AccessArea(DriverId,dto.Name,dto.MultiOccupancy,dto.AccessControl,dto.OccControl,dto.OccSet,dto.OccMax,dto.OccUp,dto.OccDown,dto.AreaFlag,dto.LocationId,dto.Status);

            var macs = await hw.GetMacsAsync();

            foreach(var mac in macs)
            {
               var ScpId = await hw.GetComponentIdFromMacAsync(mac);
               if (!await area.ConfigureAccessArea(ScpId, DriverId, domain.MultiOccupancy, domain.AccessControl, domain.OccControl, domain.OccSet, domain.OccMax, domain.OccUp, domain.OccDown, domain.AreaFlag))
               {
                   return ResponseHelper.UnsuccessBuilderWithString<AccessAreaDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(mac, Command.AREA_CONFIG));
               }
            }

            var status = await repo.AddAsync(domain);

            if(status <= 0) return ResponseHelper.UnsuccessBuilder<AccessAreaDto>(ResponseMessage.SAVE_DATABASE_UNSUCCESS,[]);

            return ResponseHelper.SuccessBuilder(await repo.GetByIdAsync(status));
        }

        public async Task<ResponseDto<AccessAreaDto>> UpdateAsync(AccessAreaDto dto)
        {

            if (await repo.IsAnyById(dto.Id)) return ResponseHelper.NotFoundBuilder<AccessAreaDto>();

            var domain = new AccessArea(dto.DriverId, dto.Name, dto.MultiOccupancy,dto.AccessControl,dto.OccControl,dto.OccSet,dto.OccMax,dto.OccUp,dto.OccDown,dto.AreaFlag,dto.LocationId,dto.IsActive);


            var macs = await hw.GetMacsAsync();

            foreach (var mac in macs)
            {
                var ScpId = await hw.GetComponentIdFromMacAsync(mac);
                if (!await area.ConfigureAccessArea(ScpId, dto.DriverId, dto.MultiOccupancy, dto.AccessControl, dto.OccControl, dto.OccSet, dto.OccMax, dto.OccUp, dto.OccDown, dto.AreaFlag))
                {
                    return ResponseHelper.UnsuccessBuilderWithString<AccessAreaDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(mac, Command.AREA_CONFIG));
                }
            }

            var status = await repo.UpdateAsync(domain);
            if(status <= 0) ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.UPDATE_RECORD_UNSUCCESS,[]);
            return ResponseHelper.SuccessBuilder(dto);
        }

        public async Task<ResponseDto<AccessAreaDto>> DeleteAsync(int id)
        {
            var en = await repo.GetByIdAsync(id);
            if (en is null) return ResponseHelper.NotFoundBuilder<AccessAreaDto>();

            var status = await repo.DeleteByIdAsync(id);
            if(status <= 0) return ResponseHelper.UnsuccessBuilder<AccessAreaDto>(ResponseMessage.DELETE_DATABASE_UNSUCCESS,[]);
            return ResponseHelper.SuccessBuilder(en);
        }

        public async Task<ResponseDto<IEnumerable<ModeDto>>> GetCommandAsync()
        {
            var dto = await repo.GetCommandAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dto);
        }

        public async Task<ResponseDto<IEnumerable<ModeDto>>> GetAccessControlOptionAsync()
        {
            var dto = await repo.GetAccessControlOptionAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dto);
        }

        public async Task<ResponseDto<IEnumerable<ModeDto>>> GetOccupancyControlOptionAsync()
        {
            var dto = await repo.GetOccupancyControlOptionAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dto);
        }

        public async Task<ResponseDto<IEnumerable<ModeDto>>> GetAreaFlagOptionAsync()
        {
            var dto = await repo.GetAreaFlagOptionAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dto);
        }

        public async Task<ResponseDto<IEnumerable<ModeDto>>> GetMultiOccupancyOptionAsync()
        {
            var dto = await repo.GetMultiOccupancyOptionAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dto);
        }

        public async Task<ResponseDto<IEnumerable<AccessAreaDto>>> GetByLocationIdAsync(int location)
        {
            var dto = await repo.GetByLocationIdAsync(location);
            return ResponseHelper.SuccessBuilder<IEnumerable<AccessAreaDto>>(dto);
        }

        public async Task<ResponseDto<Pagination<AccessAreaDto>>> GetPaginationAsync(PaginationParamsWithFilter param, int location)
        {
            var dto = await repo.GetPaginationAsync(param,location);
            return ResponseHelper.SuccessBuilder<Pagination<AccessAreaDto>>(dto);
        }
    }
}
