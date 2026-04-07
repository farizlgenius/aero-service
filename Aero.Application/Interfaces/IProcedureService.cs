

using Aero.Application.DTOs;
using Aero.Domain.Entities;

namespace Aero.Application.Interface
{
    public interface IProcedureService
    {
        Task<ResponseDto<IEnumerable<ProcedureDto>>> GetAsync();
        Task<ResponseDto<IEnumerable<ProcedureDto>>> GetByLocationIdAsync(int location);
        Task<ResponseDto<Pagination<ProcedureDto>>> GetPaginationAsync(PaginationParamsWithFilter param,int location);
        Task<ResponseDto<ProcedureDto>> CreateAsync(ProcedureDto dto);
        Task<ResponseDto<ProcedureDto>> DeleteAsync(int id);
        Task<ResponseDto<ProcedureDto>> UpdateAsync(ProcedureDto dto);
        Task<ResponseDto<IEnumerable<ModeDto>>> GetActionType();
    }
}
