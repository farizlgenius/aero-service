using AeroService.DTO;
using AeroService.DTO.Role;
using Microsoft.AspNetCore.Mvc;

namespace AeroService.Service
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
