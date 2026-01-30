using System;
using Aero.Application.DTOs;

namespace Aero.Application.Interfaces;

public interface IRoleService
{
      Task<ResponseDto<bool>> CreateAsync(RoleDto dto);
      Task<ResponseDto<bool>> DeleteByComponentIdAsync(short ComponentId);
      Task<ResponseDto<IEnumerable<ResponseDto<bool>>>> DeleteRangeAsync(List<short> dtos);
      Task<ResponseDto<IEnumerable<RoleDto>>> GetAsync();
      Task<ResponseDto<RoleDto>> GetByComponentIdAsync(short ComponentId);
      Task<ResponseDto<RoleDto>> UpdateAsync(RoleDto dto);
}
