using Aero.Domain.Helpers;
using System;

namespace Aero.Domain.Entities;

public sealed class AccessArea : BaseDomain
{
    public int Id { get; private set; }
    public short DriverId { get; private set; }
      public string Name { get; private set; } = string.Empty;
      public short MultiOccupancy { get; private set; }
      public short AccessControl { get; private set; }
      public short OccControl { get; private set; }
      public short OccSet { get; private set; }
      public short OccMax { get; private set; }
      public short OccUp { get; private set; }
      public short OccDown { get; private set; }
      public short AreaFlag { get; private set; }

    public AccessArea() { }

    public AccessArea(short driver,string name,short multiocc,short acs,short occcontrol,short occset,short occmax,short occup,short occdown,short areaflag,int location,bool status) : base(location,status)
    {
        SetDriverId(driver);
        SetName(name);
        this.MultiOccupancy = multiocc;
        this.AccessControl = acs;
        this.OccControl = occcontrol;
        this.OccSet = occset;
        this.OccMax = occmax;
        this.OccUp = occup;
        this.OccDown = occdown;
        this.AreaFlag = areaflag;
    }

    private void SetDriverId(short driver)
    {
        if (driver <= 0) throw new ArgumentException("Driver id invalid.");
        this.DriverId = driver;
    }
    private void SetName(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        if (!RegexHelper.IsValidName(name.Trim())) throw new ArgumentException("Name invalid.");

        this.Name = name;
    }



}

