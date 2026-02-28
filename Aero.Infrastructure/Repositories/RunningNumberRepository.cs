using Aero.Application.Interfaces;
using Aero.Domain.Entities;
using Aero.Domain.Interfaces;
using Aero.Infrastructure.Persistences;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aero.Infrastructure.Repositories
{
    public sealed class RunningNumberRepository(AppDbContext context) : IRunningNumberRepository
    {
        public async Task<short> GetLowestUnassignedNumberAsync<TEntity>(int max) where TEntity : class, IDriverId
        {
            var query = context.Set<TEntity>()
                .AsNoTracking()
                .Select(x => x.driver_id);

            // Handle empty table case quickly
            var hasAny = await query.AnyAsync();
            if (!hasAny)
                return 1; // start at 1 if table is empty

            // Load all numbers into memory (only the column, so it's lightweight)
            var numbers = await query.Distinct().OrderBy(x => x).ToListAsync();

            short expected = 1;
            foreach (var num in numbers)
            {
                if (num != expected)
                    return expected; // found the lowest missing number
                expected++;
            }

            // If none missing in sequence, return next number
            return expected;
        }

        public async Task<short> GetLowestUnassignedNumberByDeviceAsync<TEntity>(int device, int max) where TEntity : class,IDeviceId,IDriverId
        {

            if (max <= 0) return -1;

            var query = context.Set<TEntity>()
                .AsNoTracking()
                .Where(x => x.device_id == device)
                .Select(x => x.driver_id);

            // Handle empty table case quickly
            var hasAny = await query.AnyAsync();
            if (!hasAny)
                return 1; // start at 1 if table is empty

            // Load all numbers into memory (only the column, so it's lightweight)
            var numbers = await query.Distinct().OrderBy(x => x).ToListAsync();

            short expected = 1;
            foreach (var num in numbers)
            {
                if (num != expected)
                    return expected; // found the lowest missing number
                expected++;
            }

            // If none missing in sequence, return next number
            if (expected > max) return -1;
            return expected;
        }
    }
}
