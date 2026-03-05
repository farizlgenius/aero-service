

using Aero.Api.Constants;
using Aero.Application.Constants;
using Aero.Application.DTOs;
using Aero.Application.Helpers;
using Aero.Application.Interface;
using Aero.Application.Interfaces;
using Aero.Domain.Entities;

namespace Aero.Application.Services
{
    public class ModuleService(IModuleRepository moduleRepo,ISioCommand sio,IDeviceRepository hw) : IModuleService
    {

        public async Task<ResponseDto<IEnumerable<ModuleDto>>> GetAsync()
        {
            var dtos = await moduleRepo.GetAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<ModuleDto>>(dtos);
        }

        public void GetSioStatus(int ScpId, int SioNo)
        {
            sio.GetSioStatus((short)ScpId, (short)SioNo);
        }

        

        public async Task<ResponseDto<bool>> GetStatusAsync(int device, short driver)
        {

            if (!await moduleRepo.IsAnyByDriverAndDeviceIdAsnyc(device,driver)) return ResponseHelper.NotFoundBuilder<bool>();
            if (!sio.GetSioStatus((short)device, driver))
            {
                return ResponseHelper.UnsuccessBuilderWithString<bool>(ResponseMessage.COMMAND_UNSUCCESS,MessageBuilder.Unsuccess(await hw.GetMacFromComponentAsync((short)device),Command.MODULE_STATUS));
            }
            return ResponseHelper.SuccessBuilder<bool>(true);

        }

        public async Task<ResponseDto<IEnumerable<ModuleDto>>> GetByDeviceIdAsync(int device)
        {
            var dtos = await moduleRepo.GetByDeviceIdAsync(device);
            return ResponseHelper.SuccessBuilder<IEnumerable<ModuleDto>>(dtos);
        }

        public async Task<ResponseDto<ModuleDto>> CreateAsync(ModuleDto dto)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseDto<ModuleDto>> DeleteAsync(string mac, short component)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseDto<ModuleDto>> UpdateAsync(ModuleDto dto)
        {
            throw new NotImplementedException();
        }


        public async Task<ResponseDto<IEnumerable<ModeDto>>> GetModeAsync(int param)
        {
            throw new NotImplementedException();
        }


        public async Task<ResponseDto<ModuleDto>> GetByComponentAsync(string mac, short component)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseDto<IEnumerable<ModuleDto>>> GetByLocationAsync(int location)
        {
            var dtos = await moduleRepo.GetByLocationIdAsync(location);
            return ResponseHelper.SuccessBuilder<IEnumerable<ModuleDto>>(dtos);
        }

        public async Task<ResponseDto<IEnumerable<ModeDto>>> GetBaudrateAsync()
        {
            var dtos = await moduleRepo.GetBaudrateAsync();
            return ResponseHelper.SuccessBuilder(dtos);
        }

        public async Task<ResponseDto<IEnumerable<ModeDto>>> GetProtocolAsync()
        {
            var dtos = await moduleRepo.GetProtocolAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dtos);
        }

        public async Task<ResponseDto<Pagination<ModuleDto>>> GetPaginationAsync(PaginationParamsWithFilter param, int location)
        {
            var res = await moduleRepo.GetPaginationAsync(param,location);
            return ResponseHelper.SuccessBuilder(res);
        }
    }
}
