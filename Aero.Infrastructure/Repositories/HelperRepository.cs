using System;
using Aero.Application.Interfaces;
using Aero.Domain.Entities;
using Aero.Domain.Interface;
using Aero.Infrastructure.Data;

using Microsoft.EntityFrameworkCore;

namespace Aero.Infrastructure.Repositories;

public sealed class HelperRepository(AppDbContext context) : IHelperRepository
{
       public async Task<short> GetLowestUnassignedNumberAsync<TEntity>(string hardwareMac, int max, CancellationToken ct) where TEntity : class,IComponentId, IMac
        {
            if (max <= 0) return -1;

            var query = context.Set<TEntity>()
                .AsNoTracking()
                .Where(x => x.mac == hardwareMac)
                .Select(x => x.component_id);

            // Handle empty table case quickly
            var hasAny = await query.AnyAsync(ct);
            if (!hasAny)
                return 1; // start at 1 if table is empty

            // Load all numbers into memory (only the column, so it's lightweight)
            var numbers = await query.Distinct().OrderBy(x => x).ToListAsync(ct);

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


        public async Task<short> GetLowestUnassignedNumberAsync<TEntity>(int max, CancellationToken ct) where TEntity : class, IComponentId
        {
            if (max <= 0) return -1;

            var query = context.Set<TEntity>()
                .AsNoTracking()
                .Select(x => x.component_id);

            // Handle empty table case quickly
            var hasAny = await query.AnyAsync(ct);
            if (!hasAny)
                return 1; // start at 1 if table is empty

            // Load all numbers into memory (only the column, so it's lightweight)
            var numbers = await query.Distinct().OrderBy(x => x).ToListAsync(ct);

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

        public async Task<short> GetLowestUnassignedNumberNoLimitAsync<Entity>(CancellationToken ct) where Entity : class, IComponentId
        {
            var query = context.Set<Entity>()
                .AsNoTracking()
                .Select(x => x.component_id);

            // Handle empty table case quickly
            var hasAny = await query.AnyAsync(ct);
            if (!hasAny)
                return 1; // start at 1 if table is empty

            // Load all numbers into memory (only the column, so it's lightweight)
            var numbers = await query.Distinct().OrderBy(x => x).ToListAsync(ct);

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
}
