

using Aero.Application.DTOs;
using Aero.Domain.Entities;

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

        Task<ResponseDto<IEnumerable<Mode>>> GetCommandAsync();
        Task<ResponseDto<IEnumerable<Mode>>> GetCodeByTranAsync(short tran);
        Task<ResponseDto<IEnumerable<Mode>>> GetTypeBySourceAsync(short source);
        Task<ResponseDto<IEnumerable<Mode>>> GetSourceTypeAsync();
        Task<ResponseDto<IEnumerable<Mode>>> GetDeviceBySourceAsync(short location, short source);
    }
}
