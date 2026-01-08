using HID.Aero.ScpdNet.Wrapper;
using HIDAeroService.DTO.Interval;
using HIDAeroService.Entity;
using MiNET.Entities.Passive;
using System;

namespace HIDAeroService.Aero.CommandService.Impl
{
    public sealed class TimeZoneCommandService(AeroCommandService command) : ITimeZoneCommandService
    {
        public async Task<bool> ExtendedTimeZoneActSpecificationAsync(short ScpId, Entity.TimeZone dto, List<Interval> intervals, int activeTime, int deactiveTime)
        {
            CC_SCP_TZEX_ACT cc = new CC_SCP_TZEX_ACT();
            cc.lastModified = 0;
            cc.nScpID = ScpId;
            cc.number = dto.component_id;
            cc.mode = dto.mode;
            cc.actTime = activeTime;
            cc.deactTime = deactiveTime;
            cc.intervals = (short)intervals.Count;
            if (intervals.Count > 0)
            {
                int i = 0;
                foreach (var interval in intervals)
                {
                    cc.i[i].i_days = (short)ConvertDayToBinary(interval.days);
                    cc.i[i].i_start = (short)ConvertTimeToEndMinute(interval.start_time);
                    cc.i[i].i_end = (short)ConvertTimeToEndMinute(interval.start_time);
                    i++;
                }

            }
            var tag = ScpId + "/" + SCPDLL.scpGetTagLastPosted(ScpId) + 1;
            bool flag = command.SendCommand((short)enCfgCmnd.enCcScpTimezoneExAct, cc);
            //if (flag)
            //{
            //    return await command.TrackCommandAsync(tag, hardware_id, Constants.command.C3103);
            //}
            return flag;
        }

        public async Task<bool> TimeZoneControlAsync(short ScpId,short Component,short Command)
        {
            CC_TZCOMMAND cc = new CC_TZCOMMAND();
            cc.scp_number = ScpId;
            cc.tz_number = Component;
            cc.command = Command;
            var tag = ScpId + "/" + SCPDLL.scpGetTagLastPosted(ScpId) + 1;
            bool flag = command.SendCommand((short)enCfgCmnd.enCcTzCommand, cc);
            //if (flag)
            //{
            //    return await command.TrackCommandAsync(tag, hardware_id, Constants.command.C314);
            //}
            return flag;
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
            result |= (days.sunday ? 1 : 0) << 0;
            result |= (days.monday ? 1 : 0) << 1;
            result |= (days.tuesday ? 1 : 0) << 2;
            result |= (days.wednesday ? 1 : 0) << 3;
            result |= (days.thursday ? 1 : 0) << 4;
            result |= (days.friday ? 1 : 0) << 5;
            result |= (days.saturday ? 1 : 0) << 6;

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
