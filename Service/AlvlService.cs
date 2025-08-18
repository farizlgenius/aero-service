using HIDAeroService.Data;
using HIDAeroService.Dto.AccessLevel;
using HIDAeroService.Entity;
using HIDAeroService.Mapper;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace HIDAeroService.Service
{
    public class AlvlService
    {
        private ILogger<AlvlService> _logger;
        private readonly AppDbContext _context;
        private readonly AppConfigData _config;
        private readonly HelperService _helperService;
        public AlvlService(AppDbContext context,ILogger<AlvlService> logger,AppConfigData config,HelperService helperService) 
        {
            _helperService = helperService;
            _config = config;
            _logger = logger;
            _context = context;
        }

        public List<ar_access_lv> GetAccessLevelList()
        {
            _logger.LogInformation("Get Access Level List");
            return _context.ar_access_lvls.ToList();
        }

        public List<AccessLevelDto> GetAccessLevelDtoList()
        {
            _logger.LogInformation("Get Access Level Dto List");
            List<ar_access_lv> acs = _context.ar_access_lvls.ToList();
            List<AccessLevelDto> b = new List<AccessLevelDto>();
            foreach(var a in acs)
            {
                b.Add(MapperHelper.AccessLevelToAccessLevelDto(a));
            }
            return b;
        }

        public AccessLevelTimeZoneDto GetAccessLevelTimeZone(short AlvlNo)
        {
            try
            {
                _logger.LogInformation("Get Access Level TimeZone");
                AccessLevelTimeZoneDto a = new AccessLevelTimeZoneDto();
                ar_access_lv d = _context.ar_access_lvls.AsNoTracking().Where(p => p.access_lv_number == AlvlNo).First();
                PropertyInfo[] targetprops = typeof(AccessLevelTimeZoneDto).GetProperties();
                PropertyInfo[] sourceprops = typeof(ar_access_lv).GetProperties();
                foreach (var sp in sourceprops)
                {
                    // Try to find a property in the target with same name and type
                    var tp = targetprops.FirstOrDefault(tp =>
                        tp.Name == sp.Name && tp.PropertyType == sp.PropertyType);

                    if (tp != null && tp.CanWrite)
                    {
                        var value = sp.GetValue(d);      // ✅ Get value from source object
                        tp.SetValue(a, value);           // ✅ Set value to target DTO
                    }
                }
                return a;

            }
            catch(Exception e)
            {
                _logger.LogInformation("Not Found Access Level");
                return null;
            }

        }

        public string CreateAccessLevel(CreateAccessLevelDto dto)
        {
            try
            {
                short AccessLevelNumber = GetUniqueAccessLevelNo(dto.AccessLevelNumner);
                List<string> ScpIpList = _context.ar_scps.Select(p => p.ip_address).ToList();
                //foreach (var sp in ScpIpList)
                //{
                //    short ScpId = _helperService.GetScpIdFromIp(sp);
                //    if (!_config.write.CreateAccessLevel(ScpId, AccessLevelNumber, dto.Doors))
                //    {
                //        throw new Exception("Fail to send create access level command");
                //    }
                //}
                SaveAccessLevelToDatabase(dto,AccessLevelNumber);
                return "Created";
            }
            catch (Exception e) 
            {
                Console.WriteLine(e.Message);
                _logger.LogError(e.Message);
                return e.Message;
            }

            

        }

        public short GetUniqueAccessLevelNo(short AccessLevelNumber)
        {

            short highestCpNumber;
            if (!_context.ar_alvl_no.Any(p => p.alvl_number == AccessLevelNumber))
                return 0;

            if (_context.ar_cp_no.Any(p => p.is_available == true))
            {
                highestCpNumber = _context.ar_alvl_no.Where(p => p.is_available == true).Select(p => p.alvl_number).First();
                return highestCpNumber;
            }
            else
            {
                highestCpNumber = _context.ar_alvl_no.Where(p => p.is_available == false).Max(p => p.alvl_number);
                highestCpNumber += 1;
                return highestCpNumber;
            }

        }

        public bool SaveAccessLevelToDatabase(CreateAccessLevelDto dto,short alvlNumber)
        {
            ar_n_alvl ndata = new ar_n_alvl();
            _context.ar_access_lvls.Add(MapperHelper.AccessLevelDtoToAccessLevel(dto,alvlNumber));
            ndata.alvl_number = alvlNumber;
            ndata.is_available = false;
            _context.ar_alvl_no.Add(ndata);
            _context.SaveChanges();
            return true;
        }

        public string RemoveAccessLevel(AccessLevelDto dto)
        {
            try
            {
                var data = _context.ar_access_lvls.Where(p => p.access_lv_number == dto.AccessLevelNumber).FirstOrDefault();
                if (data != null) 
                {
                    _context.ar_access_lvls.Remove(data);
                }
                var ndata = _context.ar_alvl_no.Where(p => p.alvl_number == dto.AccessLevelNumber).FirstOrDefault();
                if (ndata != null) 
                {
                    ndata.is_available = true;
                }
                _context.SaveChanges();
                return "Removed";
            }catch(Exception e)
            {
                Console.WriteLine(e.Message);
                _logger.LogError(e.Message);
                return e.Message;
            }
        }


        public int GetAlvlRecAlloc()
        {
            return _context.ar_access_lvls.Count();
        }
    }
}
