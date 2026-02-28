using Aero.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aero.Application.Interfaces
{
    public interface IRunningNumberRepository
    {
        Task<short> GetLowestUnassignedNumberAsync<TEntity>(int max) where TEntity : class, IDriverId;
        Task<short> GetLowestUnassignedNumberByDeviceAsync<TEntity>(int device, int max) where TEntity : class, IDeviceId, IDriverId;
    }
}
