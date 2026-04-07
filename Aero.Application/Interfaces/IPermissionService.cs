using System;
using Aero.Application.DTOs;

namespace Aero.Application.Interfaces;

public interface IPermissionService
{
      Task<bool> IsPermissionAllowAsync(int roleId, string source, string action);
      Task<ResponseDto<IEnumerable<PermissionDto>>> GetFeatureListAsync();
        Task<ResponseDto<IEnumerable<PermissionDto>>> GetFeatureByRoleAsync(short RoleId);
        Task<ResponseDto<PermissionDto>> GetFeatureByRoleIdAndFeatureIdAsync(short RoleId,short FeatureId);
}
