using System;
using Aero.Application.DTOs;
using Aero.Domain.Entities;

namespace Aero.Application.Interfaces;

public interface IRoleService
{
      Task<ResponseDto<bool>> CreateAsync(RoleDto dto);
      Task<ResponseDto<bool>> DeleteByComponentIdAsync(short ComponentId);
      Task<ResponseDto<IEnumerable<ResponseDto<bool>>>> DeleteRangeAsync(List<short> dtos);
      Task<ResponseDto<IEnumerable<RoleDto>>> GetAsync();
    Task<ResponseDto<IEnumerable<RoleDto>>> GetByLocationAsync(short location);
      Task<ResponseDto<RoleDto>> GetByComponentIdAsync(short ComponentId);
      Task<ResponseDto<RoleDto>> UpdateAsync(RoleDto dto);
    Task<ResponseDto<Pagination<RoleDto>>> GetPaginationAsync(PaginationParamsWithFilter param,short location);
}
