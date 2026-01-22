using Aero.Application.Interfaces;

using System.Globalization;

namespace AeroService.Service.Impl
{
    public sealed class HelperService : IHelper
    {



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

    public class HelperService(AppDbContext context) : IHelper
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
