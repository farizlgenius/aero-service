using System;
using Aero.Application.DTOs;
using Aero.Application.Interfaces;
using Aero.Domain.Entities;
using Aero.Domain.Interfaces;

namespace Aero.Application.Interface;

public interface IRoleRepository : IBaseRepository<RoleDto,Aero.Domain.Entities.Role>
{
    Task<bool> IsAnyByNameAsync(string name);
    Task<bool> IsAnyReferenceByComponentIdAsync(short component);
}
