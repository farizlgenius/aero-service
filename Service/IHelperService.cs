using HIDAeroService.Entity.Interface;
using Microsoft.EntityFrameworkCore;

namespace HIDAeroService.Service
{
    public interface IHelperService<TEntity> : IHelperService
    {
        Task<short> GetLowestUnassignedNumberAsync<TEntity>(DbContext db,string hardwareMac,int max,CancellationToken ct = default) where TEntity : class, IComponentId,IMac;

        Task<short> GetLowestUnassignedNumberAsync<Entity>(DbContext db,int max,CancellationToken ct = default) where Entity : class, IComponentId;
        Task<short> GetLowestUnassignedNumberNoLimitAsync<Entity>(DbContext db, CancellationToken ct=default) where Entity : class, IComponentId;
    }

    public interface IHelperService
    {
        short GetIdFromMac(string mac);
        string GetMacFromId(short id);
        Task<string> GetMacFromIdAsync(short id);
        Task<short> GetIdFromMacAsync(string mac);
        long DateTimeToElapeSecond(string date);
        
    }
}
