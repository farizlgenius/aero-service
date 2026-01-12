using AeroService.Data;
using AeroService.Entity.Interface;
using AeroService.Model;
using LibNoise.Combiner;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace AeroService.Service.Impl
{
    public sealed class HelperService<TEntity>(AppDbContext context) : IHelperService<TEntity>
    {


        public async Task<string> GetMacFromIdAsync(short id)
        {
            return await context.hardware.Where(d => d.component_id == id).Select(d => d.mac).FirstOrDefaultAsync() ?? "";
        }


        public async Task<short> GetIdFromMacAsync(string mac)
        {
            return await context.hardware.Where(d => d.mac == mac).Select(d => (short)d.component_id).FirstOrDefaultAsync();
        }

        public long DateTimeToElapeSecond(string date)
        {
            if (date.Equals("") || date.Equals(null)) return 0;

            DateTimeOffset dto = DateTimeOffset.Parse(date);

            return dto.ToUnixTimeSeconds();
        }

        public short GetIdFromMac(string mac)
        {
            return context.hardware.Where(x => x.mac == mac).Select(x => (short)x.id).FirstOrDefault();
        }

        public string GetMacFromId(short id)
        {
            return context.hardware.Where(x => x.component_id == id).Select(x => x.mac).FirstOrDefault() ?? "";
        }

        public async Task<short> GetLowestUnassignedNumberAsync<TEntity>(DbContext db,string hardwareMac,int max, CancellationToken ct) where TEntity : class,IComponentId,IMac
        {
            if (max <= 0) return -1;

            var query = db.Set<TEntity>()
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


        public async Task<short> GetLowestUnassignedNumberAsync<TEntity>(DbContext db, int max, CancellationToken ct) where TEntity : class, IComponentId
        {
            if (max <= 0) return -1;

            var query = db.Set<TEntity>()
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

        public async Task<short> GetLowestUnassignedNumberNoLimitAsync<Entity>(DbContext db,CancellationToken ct) where Entity : class,IComponentId
        {

            var query = db.Set<Entity>()
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

        public async Task<string> GetHardwareNameById(short id)
        {
            return await context.hardware.AsNoTracking().OrderBy(x => x.component_id).Where(x => x.component_id == id).Select(x => x.name).FirstOrDefaultAsync() ?? "";
        }
    }

    public class HelperService(AppDbContext context) : IHelperService
    {


        public async Task<int> GetLowestHardwareNumberAsync(CancellationToken ct)
        {

            var query = context.hardware
                .AsNoTracking()
                .Select(x => x.id);

            // Handle empty table case quickly
            var hasAny = await query.AnyAsync(ct);
            if (!hasAny)
                return 1; // start at 1 if table is empty

            // Load all numbers into memory (only the column, so it's lightweight)
            var numbers = await query.Distinct().OrderBy(x => x).ToListAsync(ct);

            int expected = 1;
            foreach (var num in numbers)
            {
                if (num != expected)
                    return expected; // found the lowest missing number
                expected++;
            }

            // If none missing in sequence, return next number
            return expected;
        }


        public async Task<string> GetMacFromIdAsync(short id)
        {
            return await context.hardware.Where(d => d.component_id == id).Select(d => d.mac).FirstOrDefaultAsync() ?? "";
        }


        public async Task<short> GetIdFromMacAsync(string mac)
        {
            return await context.hardware.Where(d => d.mac == mac).Select(d => (short)d.component_id).FirstOrDefaultAsync();
        }

        public long DateTimeToElapeSecond(string date)
        {
            if (date.Equals("") || date.Equals(null)) return 0;

            DateTimeOffset dto = DateTimeOffset.Parse(date);

            return dto.ToUnixTimeSeconds();
        }

        public short GetIdFromMac(string mac)
        {
            return context.hardware.Where(x => x.mac == mac).Select(x => (short)x.id).FirstOrDefault();
        }

        public string GetMacFromId(short id)
        {
            return context.hardware.Where(x => x.component_id == id).Select(x => x.mac).FirstOrDefault() ?? "";
        }

        public async Task<string> GetHardwareNameById(short id)
        {
            return await context.hardware.AsNoTracking().OrderBy(x => x.component_id).Where(x => x.component_id == id).Select(x => x.name).FirstOrDefaultAsync() ?? "";
        }
    }
}
