using HID.Aero.ScpdNet.Wrapper;
using HIDAeroService.DTO.Interval;
using HIDAeroService.Entity;
using MiNET.Entities.Passive;

namespace HIDAeroService.Aero.CommandService.Impl
{
    public sealed class TimezoneCommandService : BaseCommandService,ITimezoneCommandService
    {
        public async Task<bool> ExtendedTimeZoneActSpecificationAsync(short scpId, Entity.TimeZone dto, List<Interval> intervals, int activeTime, int deactiveTime)
        {
            CC_SCP_TZEX_ACT cc = new CC_SCP_TZEX_ACT();
            cc.lastModified = 0;
            cc.nScpID = scpId;
            cc.number = dto.ComponentId;
            cc.mode = dto.Mode;
            cc.actTime = activeTime;
            cc.deactTime = deactiveTime;
            cc.intervals = (short)intervals.Count;
            if (intervals.Count > 0)
            {
                int i = 0;
                foreach (var interval in intervals)
                {
                    cc.i[i].i_days = (short)ConvertDayToBinary(interval.Days);
                    cc.i[i].i_start = (short)ConvertTimeToEndMinute(interval.StartTime);
                    cc.i[i].i_end = (short)ConvertTimeToEndMinute(interval.StartTime);
                    i++;
                }

            }

            bool flag = SendCommand((short)enCfgCmnd.enCcScpTimezoneExAct, cc);
            if (flag)
            {
                return await SendCommandAsync(SCPDLL.scpGetTagLastPosted(scpId), _commandTimeout);
            }

            return false;
        }

        public async Task<bool> TimeZoneControlAsync(short ScpId,short Component,short Command)
        {
            CC_TZCOMMAND cc = new CC_TZCOMMAND();
            cc.scp_number = ScpId;
            cc.tz_number = Component;
            cc.command = Command;
            bool flag = SendCommand((short)enCfgCmnd.enCcTzCommand, cc);
            if (flag)
            {
                return await SendCommandAsync(SCPDLL.scpGetTagLastPosted(ScpId), _commandTimeout);
            }
            return false;
        }

        private string DaysInWeekToString(DaysInWeekDto days)
        {
            var map = new Dictionary<string, bool>{
                {"Sun",days.Sunday },
                {
                    "Mon",days.Monday
                },
                {
                    "Tue",days.Tuesday
                },
                {
                    "Wed",days.Wednesday
                },
                {
                    "Thu",days.Thursday
                },
                {
                    "Fri",days.Friday
                },
                {
                    "Sat",days.Saturday
                }
            };

            return string.Join(",", map.Where(x => x.Value).Select(x => x.Key));
        }

        private DaysInWeekDto StringToDaysInWeek(string daysString)
        {
            var dto = new DaysInWeekDto();
            if (string.IsNullOrWhiteSpace(daysString)) return dto;

            var parts = daysString.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                  .Select(p => p.Trim());

            foreach (var day in parts)
            {
                switch (day)
                {
                    case "Sun": dto.Sunday = true; break;
                    case "Mon": dto.Monday = true; break;
                    case "Tue": dto.Tuesday = true; break;
                    case "Wed": dto.Wednesday = true; break;
                    case "Thu": dto.Thursday = true; break;
                    case "Fri": dto.Friday = true; break;
                    case "Sat": dto.Saturday = true; break;
                }
            }

            return dto;
        }

        private int ConvertDayToBinary(DaysInWeek days)
        {
            int result = 0;
            result |= (days.Sunday ? 1 : 0) << 0;
            result |= (days.Monday ? 1 : 0) << 1;
            result |= (days.Tuesday ? 1 : 0) << 2;
            result |= (days.Wednesday ? 1 : 0) << 3;
            result |= (days.Thursday ? 1 : 0) << 4;
            result |= (days.Friday ? 1 : 0) << 5;
            result |= (days.Saturday ? 1 : 0) << 6;

            // Holiday
            //result |= 0 << 8;
            //result |= 0 << 9;
            //result |= 0 << 10;
            //result |= 0 << 11;
            //result |= 0 << 12;
            //result |= 0 << 13;
            //result |= 0 << 14;
            //result |= 0 << 15;
            return result;
        }

        private int ConvertTimeToEndMinute(string timeString)
        {
            // Parse "HH:mm"
            var time = TimeSpan.Parse(timeString);

            // Convert hours/minutes to minutes since 12:00 AM
            int startMinutes = time.Hours * 60 + time.Minutes;

            // Return the minute number at the *end* of this minute
            return startMinutes;
        }
    }
}
