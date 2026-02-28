using System;
using Aero.Application.DTOs;
using Aero.Application.Interfaces;
using Aero.Domain.Entities;
using Aero.Domain.Interfaces;

namespace Aero.Application.Interface;

public interface IRoleRepository : IBaseRepository<RoleDto,Aero.Domain.Entities.Role>
{
    Task<bool> IsAnyReferenceByIdAsync(int id);
    Task<short> GetLowestUnassignedNumberAsync(int max);
    Task<short> GetLowestUnassignedNumberAsync();
}
