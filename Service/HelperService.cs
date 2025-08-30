using HIDAeroService.Data;
using HIDAeroService.Entity.Interface;
using LibNoise.Combiner;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace HIDAeroService.Service
{
    public sealed class HelperService
    {
        private readonly AppDbContext _context;
        public HelperService(AppDbContext context) 
        {
            _context = context;
        }

        public async Task<short> GetAvailableComponentNoAsync<TEntity>(short max) where TEntity : class,IActivatable,IComponentNo
        {
            var dbSet = _context.Set<TEntity>();
            var ids = await dbSet
                .Select(e => e.ComponentNo)
                .OrderBy(id => id)
                .ToListAsync();

            if (ids.Count == 0)
                return 1; // First available when table is empty

            int missingId = ids
                .Select((id, index) => new { id, expected = index + 1 })
                .FirstOrDefault(x => x.id != x.expected)?.expected
            ?? ids.Last() + 1;

            if (max > 0 && missingId > max)
                return -1;

            return (short)missingId;
        }

        public string GetMacFromId(short scpid)
        {
            return _context.ArScps.Where(d => d.ScpId == scpid).Select(d => d.Mac).FirstOrDefault() ?? "";
        }

        public short GetScpIdFromMac(string scp_mac)
        {
            return _context.ArScps.Where(d => d.Mac == scp_mac).Select(d => d.ScpId).FirstOrDefault();
        }

        public short GetScpIdFromIp(string ip)
        {
            short scpId = _context.ArScps.Where(d => d.Ip == ip).Select(d => d.ScpId).FirstOrDefault();

            return scpId != 0 ? scpId : (short)1;
        }

        public string GetScpIpFromId(short id)
        {
            return _context.ArScps.Where(d => d.ScpId == id).Select(d => d.Ip).FirstOrDefault();
        }

        public long DateTimeToElapeSecond(string date)
        {
            if(date.Equals("") || date.Equals(null)) return 0;

            DateTimeOffset dto = DateTimeOffset.Parse(date);

            return dto.ToUnixTimeSeconds();
        }


        public string SecondToDateTime(short s)
        {

            // Local epoch start
            DateTime epochLocal = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local);

            // Add elapsed seconds
            DateTime localDateTime = epochLocal.AddSeconds(s);

            return localDateTime.ToString();// prints local date/time
        }

    }
}
