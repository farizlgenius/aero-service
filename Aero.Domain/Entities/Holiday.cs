using Aero.Domain.Helpers;
using System;

namespace Aero.Domain.Entities;

public sealed class Holiday : BaseDomain
{
    public int Id { get; set; }
    public short DriverId { get; set; }
    public string Name { get; set; } = string.Empty;
        public short Year { get; set; }
        public short Month { get; set; }
        public short Day { get; set; }
        public short Extend { get; set; }
        public short TypeMask { get; set; }

    public Holiday(short driver,string name,short year, short month, short day, short extend, short typemask,int location,bool status) : base(location,status)
    {
        if (driver <= 0) throw new ArgumentException("Driver id invalid.",nameof(driver));
        SetName(name);
        SetYear(year);
        SetMonth(month);
        SetDay(day);
        this.Extend = extend;
        this.TypeMask = typemask;
    }

    private void SetName(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        if (!RegexHelper.IsValidName(name)) throw new ArgumentException("Invalid name.");
        this.Name = name;
    }

    private void SetYear(short year)
    {
        if (year <= 0 || year < DateTime.UtcNow.Year) throw new ArgumentException("Invalid year.",nameof(year));
        this.Year = year;
    }

    private void SetMonth(short month)
    {
        if(month > 12 || month <= 0) throw new ArgumentException("Invalid month.",nameof(month));
        this.Month = month;
    }

    private void SetDay(short day) 
    {
        if(day <= 0 || day > 31) throw new ArgumentException("Invalid day.",nameof(day));
        this.Day = day;
    }
  
}
