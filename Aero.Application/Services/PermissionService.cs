using System;
using Aero.Application.DTOs;
using Aero.Application.Helpers;
using Aero.Application.Interfaces;

namespace Aero.Application.Services;

public sealed class PermissionService(IPermissionRepository repo) : IPermissionService
{
      public async Task<bool> IsPermissionAllowAsync(int roleId, string source, string action)
      {
           return await repo.IsPermissionAllowAsync(roleId, source, action);
      }

       public async Task<ResponseDto<IEnumerable<PermissionDto>>> GetFeatureByRoleAsync(short RoleId)
        {
            var dtos = await repo.GetPermissionsFromRoleIdAsync(RoleId);

            return ResponseHelper.SuccessBuilder<IEnumerable<PermissionDto>>(dtos);
        }

        public async Task<ResponseDto<IEnumerable<PermissionDto>>> GetFeatureListAsync()
        {
            var dtos = await repo.GetAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<PermissionDto>>(dtos);
        }

        public async Task<ResponseDto<PermissionDto>> GetFeatureByRoleIdAndFeatureIdAsync(short RoleId, short FeatureId)
        {
            var dto = await repo.GetFeatureByRoleIdAndFeatureIdAsync(RoleId,FeatureId);
            return ResponseHelper.SuccessBuilder<PermissionDto>(dto);
        }
}
