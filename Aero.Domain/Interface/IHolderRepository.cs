using System;
using Aero.Domain.Entities;
using Aero.Domain.Interfaces;

namespace Aero.Domain.Interface;

public interface IHolderRepository : IBaseRepository<CardHolder>
{
      Task<int> DeleteByUserIdAsync(string UserId);
      Task<int> DeleteReferenceByUserIdAsync(string UserId);
}
