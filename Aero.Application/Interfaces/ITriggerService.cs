

using Aero.Application.DTOs;

namespace Aero.Application.Interface
{
    public interface ITriggerService
    {
        Task<ResponseDto<IEnumerable<TriggerDto>>> GetAsync();
        Task<ResponseDto<IEnumerable<TriggerDto>>> GetByLocationId(short location);
        Task<ResponseDto<bool>> CreateAsync(TriggerDto dto);
        Task<ResponseDto<bool>> DeleteAsync(string Mac,short ComponentId);
        Task<ResponseDto<TriggerDto>> UpdateAsync(TriggerDto dto);

        // 

        Task<ResponseDto<IEnumerable<ModeDto>>> GetCommandAsync();
        Task<ResponseDto<IEnumerable<ModeDto>>> GetCodeByTranAsync(short tran);
        Task<ResponseDto<IEnumerable<ModeDto>>> GetTypeBySourceAsync(short source);
        Task<ResponseDto<IEnumerable<ModeDto>>> GetSourceTypeAsync();
        Task<ResponseDto<IEnumerable<ModeDto>>> GetDeviceBySourceAsync(short location, short source);
    }
}
