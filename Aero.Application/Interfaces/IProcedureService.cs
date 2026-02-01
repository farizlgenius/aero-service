

using Aero.Application.DTOs;
using Aero.Domain.Entities;

namespace Aero.Application.Interface
{
    public interface IProcedureService
    {
        Task<ResponseDto<IEnumerable<ProcedureDto>>> GetAsync();
        Task<ResponseDto<IEnumerable<ProcedureDto>>> GetByLocationIdAsync(short location);
        Task<ResponseDto<bool>> CreateAsync(ProcedureDto dto);
        Task<ResponseDto<bool>> DeleteAsync(short ComponentId);
        Task<ResponseDto<ProcedureDto>> UpdateAsync(ProcedureDto dto);
        Task<ResponseDto<IEnumerable<Mode>>> GetActionType();
    }
}
