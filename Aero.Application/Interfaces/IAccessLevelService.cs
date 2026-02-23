using Aero.Application.DTOs;
using Aero.Domain.Entities;

namespace Aero.Application.Interface
{
    public interface IAccessLevelService
    {
        Task<ResponseDto<IEnumerable<AccessLevelDto>>> GetAsync();
        Task<ResponseDto<IEnumerable<AccessLevelDto>>> GetByLocationIdAsync(int location);
        Task<ResponseDto<Pagination<AccessLevelDto>>> GetPaginationAsync(PaginationParamsWithFilter param, int location);
        Task<ResponseDto<AccessLevelDto>> GetByIdAsync(int id);
        Task<ResponseDto<AccessLevelDto>> CreateAsync(CreateAccessLevelDto dto);
        Task<ResponseDto<AccessLevelDto>> DeleteAsync(int id);
        Task<ResponseDto<AccessLevelDto>> UpdateAsync(AccessLevelDto dto);
        Task<string> GetAcrName(string mac, int id);
        Task<string> GetTzName(int id);

    }
}
