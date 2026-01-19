
using Aero.Application.DTOs;

namespace Aero.Application.Interface
{
    public interface IRoleService
    {
        Task<ResponseDto<IEnumerable<RoleDto>>> GetAsync();
        Task<ResponseDto<bool>> CreateAsync(RoleDto dto);
        Task<ResponseDto<bool>> DeleteByComponentIdAsync(short ComponentId);
        Task<ResponseDto<IEnumerable<ResponseDto<bool>>>> DeleteRangeAsync(List<short> dtos);
        Task<ResponseDto<RoleDto>> UpdateAsync(RoleDto dto);
        Task<ResponseDto<RoleDto>> GetByComponentIdAsync(short ComponentId);
    }
}
