using System;
using Aero.Domain.Interface;

namespace Aero.Domain.Entities;

public sealed class Interval : NoMacBaseEntity
{
        public DaysInWeek? Days { get; set; }
        public string DaysDesc { get; set; } = string.Empty;
        public string StartTime { get; set; } = string.Empty;   
        public string EndTime { get; set; } = string.Empty;
}
