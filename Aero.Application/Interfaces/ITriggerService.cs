

using Aero.Application.DTOs;
using Aero.Domain.Entities;

namespace Aero.Application.Interface
{
    public interface ITriggerService
    {
        Task<ResponseDto<IEnumerable<TriggerDto>>> GetAsync();
        Task<ResponseDto<IEnumerable<TriggerDto>>> GetByLocationId(int location);
        Task<ResponseDto<TriggerDto>> CreateAsync(TriggerDto dto);
        Task<ResponseDto<TriggerDto>> DeleteAsync(int id);
        Task<ResponseDto<TriggerDto>> UpdateAsync(TriggerDto dto);

        Task<ResponseDto<Pagination<TriggerDto>>> GetPaginationAsync(PaginationParamsWithFilter param,int location);

        Task<ResponseDto<IEnumerable<ModeDto>>> GetCommandAsync();
        Task<ResponseDto<IEnumerable<ModeDto>>> GetCodeByTranAsync(short tran);
        Task<ResponseDto<IEnumerable<ModeDto>>> GetTypeBySourceAsync(short source);
        Task<ResponseDto<IEnumerable<ModeDto>>> GetSourceTypeAsync();
        Task<ResponseDto<IEnumerable<ModeDto>>> GetDeviceBySourceAsync(short location, short source);
    }
}
