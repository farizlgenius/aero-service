

using Aero.Api.Constants;
using Aero.Application.Constants;
using Aero.Application.DTOs;
using Aero.Application.Helpers;
using Aero.Application.Interface;
using Aero.Application.Interfaces;
using Aero.Domain.Entities;

namespace Aero.Application.Services
{
    public class ModuleService(IQModuleRepository qModule,ISioCommand sio,IQHwRepository qhw) : IModuleService
    {

        public async Task<ResponseDto<IEnumerable<ModuleDto>>> GetAsync()
        {
            var dtos = await qModule.GetAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<ModuleDto>>(dtos);
        }

        public void GetSioStatus(int ScpId, int SioNo)
        {
            sio.GetSioStatus((short)ScpId, (short)SioNo);
        }

        

        public async Task<ResponseDto<bool>> GetStatusAsync(string mac, short Id)
        {

            if (!await qModule.IsAnyByComponentAndMacAsnyc(mac,Id)) return ResponseHelper.NotFoundBuilder<bool>();
            int ScpId = await qhw.GetComponentIdFromMacAsync(mac);
            if (!sio.GetSioStatus((short)ScpId, Id))
            {
                return ResponseHelper.UnsuccessBuilderWithString<bool>(ResponseMessage.COMMAND_UNSUCCESS,MessageBuilder.Unsuccess(mac,Command.MODULE_STATUS));
            }
            return ResponseHelper.SuccessBuilder<bool>(true);

        }

        public async Task<ResponseDto<IEnumerable<ModuleDto>>> GetByMacAsync(string mac)
        {
            var dtos = await qModule.GetByMacAsync(mac);
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


        public async Task<ResponseDto<IEnumerable<Mode>>> GetModeAsync(int param)
        {
            throw new NotImplementedException();
        }


        public async Task<ResponseDto<ModuleDto>> GetByComponentAsync(string mac, short component)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseDto<IEnumerable<ModuleDto>>> GetByLocationAsync(short location)
        {
            var dtos = await qModule.GetByLocationIdAsync(location);
            return ResponseHelper.SuccessBuilder<IEnumerable<ModuleDto>>(dtos);
        }

        public async Task<ResponseDto<IEnumerable<Mode>>> GetBaudrateAsync()
        {
            var dtos = await qModule.GetBaudrateAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<Mode>>(dtos);
        }

        public async Task<ResponseDto<IEnumerable<Mode>>> GetProtocolAsync()
        {
            var dtos = await qModule.GetProtocolAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<Mode>>(dtos);
        }

        public async Task<ResponseDto<Pagination<ModuleDto>>> GetPaginationAsync(PaginationParamsWithFilter param, short location)
        {
            var res = await qModule.GetPaginationAsync(param,location);
            return ResponseHelper.SuccessBuilder(res);
        }
    }
}
