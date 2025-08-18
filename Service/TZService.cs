using HIDAeroService.Data;
using HIDAeroService.Dto;
using HIDAeroService.Dto.Time;
using HIDAeroService.Entity;
using HIDAeroService.Mapper;
using HIDAeroService.Models;

namespace HIDAeroService.Service
{
    public class TZService
    {
        private readonly AppDbContext _context;
        private readonly HelperService _helperService;
        private readonly AppConfigData _config;
        public TZService(AppDbContext context,HelperService helperService,AppConfigData config) 
        {
            _helperService = helperService;
            _context = context;
            _config = config;
        }

        public List<TZDto> GetTimeZoneDtoList()
        {
            var tzs = _context.ar_tzs.ToList();
            List<TZDto> res = new List<TZDto>();
            int i = 1;
            foreach(var tz in tzs)
            {
                res.Add(MapperHelper.TzToTzDto(tz,i,tz.act_time == 0 ? "-" : _helperService.SecondToDateTime(tz.act_time),tz.deact_time == 0 ? "-" : _helperService.SecondToDateTime(tz.deact_time)));
                i += 1;
            }
            return res;
        }

        public List<ar_tz> GetTimeZoneList()
        {
            return _context.ar_tzs.ToList();
        }

        public short GetUniqueTzNo()
        {
            short highestTzNumber;
            if (!_context.ar_tz_no.Any())
            {
                SavenTzDB(1, false);
                return 1;
            }


            if (_context.ar_tz_no.Any(p => p.is_available == true))
            {
                highestTzNumber = _context.ar_tz_no.Where(p => p.is_available == true).Select(p => p.tz_number).First();
                return highestTzNumber;
            }
            else
            {
                highestTzNumber = _context.ar_tz_no.Where(p => p.is_available == false).Max(p => p.tz_number);
                highestTzNumber++;
                SavenTzDB(highestTzNumber, false);
                return highestTzNumber;
            }

        }


        public bool SavenTzDB(short TzNo, bool isAvailable = false)
        {
            try
            {
                ar_n_tz n = new ar_n_tz();
                n.tz_number = TzNo;
                n.is_available = isAvailable;
                _context.ar_tz_no.Add(n);
                _context.SaveChanges();
                return true;

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public bool CreateTimeZone(CreateTimeZoneDto dto)
        {
            List<short> ScpIdList = _context.ar_scps.Select(p => p.scp_id).ToList();
            short TzNumber = GetUniqueTzNo();
            foreach(short i in ScpIdList)
            {
                short active = 0;
                short deactive = 0;
                List<TimeZoneInterval> intervals = new List<TimeZoneInterval>();
                if(dto.ActiveTime != "")
                {
                    active = _helperService.DateTimeToElapeSecond(dto.ActiveTime);
                }
                if(dto.DeactiveTime != "")
                {
                    deactive = _helperService.DateTimeToElapeSecond(dto.DeactiveTime);
                }
                // Calculate IDays
                foreach(var d in dto.IntervalsDetail)
                {
                    TimeZoneInterval interval = new TimeZoneInterval();
                    interval.IDays = (short)ConvertToBinary(d.Days);
                    interval.IStart = (short)ConvertTimeToEndMinute(d.IStart);
                    interval.IEnd = (short)ConvertTimeToEndMinute(d.IEnd); ;
                    intervals.Add(interval);
                }
                
                if (!_config.write.ExtendedTimeZoneActSpecification(i,TzNumber,2,active,deactive,dto.Intervals,intervals))
                {
                    return false;
                }

                if (!SaveTimeZoneToDatabase(dto, TzNumber,active,deactive))
                {
                    Console.WriteLine("Fail Save Database");
                    return false;
                }
            }

            return true;
        }

        public bool SaveTimeZoneToDatabase(CreateTimeZoneDto dto,short TzNumber,short active,short deactive) 
        {
            try
            {
                ar_tz data = new ar_tz();
                data.name = dto.Name;
                data.tz_number = TzNumber;
                data.mode = 2;
                data.act_time = active;
                data.deact_time = deactive;
                data.intervals = dto.Intervals;
                _context.ar_tzs.Add(data);
                foreach(var d in dto.IntervalsDetail)
                {
                    ar_tz_interval interval = new ar_tz_interval();
                    interval.tz_number = TzNumber;
                    interval.intervals_number = d.IntervalNumner;
                    interval.i_days = (short)ConvertToBinary(d.Days);
                    interval.i_start = d.IStart;
                    interval.i_end = d.IEnd;
                    _context.ar_tz_intervals.Add(interval);
                }
                _context.SaveChanges();
                return true;
            }catch (Exception e) { Console.WriteLine(e.Message); return false; }
        }

        private int ConvertToBinary(DaysInWeekDto days)
        {
            int result = 0;
            result |= (days.Sunday ? 1 : 0) << 0;
            result |= (days.Monday ? 1 : 0) << 1;
            result |= (days.Tuesday ? 1 : 0) << 2;
            result |= (days.Wednesday ? 1 : 0) << 3;
            result |= (days.Thursday ? 1 : 0) << 4;
            result |= (days.Friday ? 1 : 0) << 5;
            result |= (days.Saturday ? 1 : 0) << 6;
            return result;
        }

        private int ConvertTimeToEndMinute(string timeString)
        {
            // Parse "HH:mm"
            var time = TimeSpan.Parse(timeString);

            // Convert hours/minutes to minutes since 12:00 AM
            int startMinutes = time.Hours * 60 + time.Minutes;

            // Return the minute number at the *end* of this minute
            return startMinutes + 59;
        }

        public int GetTzRecAlloc()
        {
            return _context.ar_tzs.Count();
        }
    }
}
