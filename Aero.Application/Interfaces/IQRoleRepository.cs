using System;
using Aero.Application.DTOs;
using Aero.Domain.Interfaces;

namespace Aero.Application.Interfaces;

public interface IQRoleRepository : IBaseQueryRespository<RoleDto>
{
      Task<bool> IsAnyByNameAsync(string name);
      Task<bool> IsAnyReferenceByComponentIdAsync(short component);
}
