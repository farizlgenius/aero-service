using System;
using Aero.Application.DTOs;
using Aero.Domain.Entities;

namespace Aero.Application.Interfaces;

public interface IPositionRepository : IBaseRepository<PositionDto,Position>
{
      Task<bool> IsAnyReferenceByIdAsync(int id);
}
