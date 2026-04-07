using System;
using Aero.Application.DTOs;
using Aero.Domain.Entities;

namespace Aero.Application.Interfaces;

public interface IPermissionRepository : IBaseRepository<PermissionDto,Permission>
{
      Task<HashSet<PermissionDto>> GetPermissionsFromRoleIdAsync(int roleId);
        Task<PermissionDto> GetFeatureByRoleIdAndFeatureIdAsync(short RoleId, short FeatureId);
        Task<bool> IsPermissionAllowAsync(int roleId, string source, string action);
        
}
