using System;
using Aero.Application.DTOs;
using Aero.Domain.Entities;

namespace Aero.Application.Interfaces;

public interface IDepartmentRepository : IBaseRepository<DepartmentDto,Department>
{
      Task<bool> IsAnyReferenceByIdAsync(int id);
}
