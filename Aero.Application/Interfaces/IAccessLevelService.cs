using Aero.Application.DTOs;
using Aero.Domain.Entities;

namespace Aero.Application.Interface
{
    public interface IAccessLevelService
    {
        Task<ResponseDto<IEnumerable<AccessLevelDto>>> GetAsync();
        Task<ResponseDto<IEnumerable<AccessLevelDto>>> GetByLocationIdAsync(short location);
        Task<ResponseDto<Pagination<AccessLevelDto>>> GetPaginationAsync(PaginationParamsWithFilter param,short location);
        Task<ResponseDto<AccessLevelDto>> GetByComponentIdAsync(short component);
        Task<ResponseDto<bool>> CreateAsync(CreateUpdateAccessLevelDto dto);
        Task<ResponseDto<bool>> DeleteAsync(short component);
        Task<ResponseDto<AccessLevelDto>> UpdateAsync(CreateUpdateAccessLevelDto dto);
        Task<string> GetAcrName(string mac, short component);
        Task<string> GetTzName(short component);

    }
}
