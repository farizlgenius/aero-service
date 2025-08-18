using HIDAeroService.Data;
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

        public string GetMacFromId(short scpid)
        {
            return _context.ar_scps.Where(d => d.scp_id == scpid).Select(d => d.mac).First();
        }

        public short GetScpIdFromMac(string scp_mac)
        {
            return _context.ar_scps.Where(d => d.mac == scp_mac).Select(d => d.scp_id).First();
        }

        public short GetScpIdFromIp(string ip)
        {
            return _context.ar_scps.Where(d => d.ip_address == ip).Select(d => d.scp_id).First();
        }

        public string GetScpIpFromId(short id)
        {
            return _context.ar_scps.Where(d => d.scp_id == id).Select(d => d.ip_address).First();
        }

        public short DateTimeToElapeSecond(string date)
        {
            string isoDate = "2025-08-05T12:00:00+07:00";

            // Parse as DateTimeOffset to get exact date and time
            DateTimeOffset dto = DateTimeOffset.Parse(isoDate, null, DateTimeStyles.RoundtripKind);

            // Convert to DateTime in local time, ignoring offset
            DateTime localDateTime = dto.DateTime;

            // Get Unix epoch start in local time (midnight Jan 1 1970 local)
            DateTime epochLocal = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local);

            // Calculate elapsed seconds (difference)
            TimeSpan elapsed = localDateTime - epochLocal;

            short secondsElapsed = (short)elapsed.TotalSeconds;

            return secondsElapsed;
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
