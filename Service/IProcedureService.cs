using AeroService.DTO;
using AeroService.DTO.Procedure;
using Microsoft.AspNetCore.Mvc;

namespace AeroService.Service
{
    public interface IProcedureService
    {
        Task<ResponseDto<IEnumerable<ProcedureDto>>> GetAsync();
        Task<ResponseDto<IEnumerable<ProcedureDto>>> GetByLocationIdAsync(short location);
        Task<ResponseDto<bool>> CreateAsync(ProcedureDto dto);
        Task<ResponseDto<bool>> DeleteAsync(string Mac,short ComponentId);
        Task<ResponseDto<ProcedureDto>> UpdateAsync(ProcedureDto dto);
        Task<ResponseDto<IEnumerable<ModeDto>>> GetActionType();
    }
}
