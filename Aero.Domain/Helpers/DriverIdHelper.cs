using Aero.Domain.Entities;
using Aero.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Aero.Domain.Helpers
{
    public class DriverIdHelper
    {
        public static short GetLowestUnassignedNumberAsync<TEntity>(
            IQueryable<TEntity> source,
            string mac,
            int max) where TEntity : class , IDriverId,IDeviceId
        {
            if (max <= 0)
                return -1;

            var numbers = source
                .Where(x => x.device_id.Equals(mac))
                .Select(x => x.driver_id)
                .Where(n => n > 0 && n <= max)
                .Distinct()
                .ToList();

            if (numbers.Count == 0)
                return 1;

            var used = new HashSet<short>(numbers);

            for (short i = 1; i <= max; i++)
            {
                if (!used.Contains(i))
                    return i;
            }

            return -1;
        }
    }
}
