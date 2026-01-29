using System;
using Aero.Application.DTOs;
using Aero.Domain.Interfaces;

namespace Aero.Application.Interfaces;

public interface IQFeatureRepository : IBaseQueryRespository<FeatureDto>
{
      Task<IEnumerable<FeatureDto>> GetFeatureByRoleAsync(short RoleId);
      Task<FeatureDto> GetFeatureByRoleIdAndFeatureIdAsync(short RoleId, short FeatureId);
}
