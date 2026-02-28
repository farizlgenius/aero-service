using System;
using Aero.Application.DTOs;
using Aero.Domain.Entities;

namespace Aero.Application.Interfaces;

public interface IRoleService
{
      Task<ResponseDto<RoleDto>> CreateAsync(RoleDto dto);
      Task<ResponseDto<RoleDto>> DeleteByIdAsync(int id);
      Task<ResponseDto<IEnumerable<RoleDto>>> DeleteRangeAsync(List<int> ids);
      Task<ResponseDto<IEnumerable<RoleDto>>> GetAsync();
    Task<ResponseDto<IEnumerable<RoleDto>>> GetByLocationAsync(int location);
      Task<ResponseDto<RoleDto>> GetByIdAsync(int id);
      Task<ResponseDto<RoleDto>> UpdateAsync(RoleDto dto);
    Task<ResponseDto<Pagination<RoleDto>>> GetPaginationAsync(PaginationParamsWithFilter param,int location);
}
