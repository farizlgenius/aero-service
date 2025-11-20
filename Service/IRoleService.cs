using HIDAeroService.DTO;
using HIDAeroService.DTO.Role;
using Microsoft.AspNetCore.Mvc;

namespace HIDAeroService.Service
{
    public interface IRoleService
    {
        Task<ResponseDto<IEnumerable<RoleDto>>> GetAsync();
        Task<ResponseDto<bool>> CreateAsync(RoleDto dto);
        Task<ResponseDto<bool>> DeleteByComponentIdAsync(short ComponentId);
        Task<ResponseDto<RoleDto>> UpdateAsync(RoleDto dto);
        Task<ResponseDto<RoleDto>> GetByComponentIdAsync(short ComponentId);
    }
}
