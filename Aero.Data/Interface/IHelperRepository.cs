using System;

namespace Aero.Domain.Interface;

public interface IHelperRepository
{
      Task<short> GetLowestUnassignedNumberAsync<TEntity>(string hardwareMac, int max, CancellationToken ct) where TEntity : class,IComponentId, IMac;
      Task<short> GetLowestUnassignedNumberAsync<TEntity>(int max, CancellationToken ct) where TEntity : class, IComponentId;
      Task<short> GetLowestUnassignedNumberNoLimitAsync<Entity>(CancellationToken ct) where Entity : class, IComponentId;
}
