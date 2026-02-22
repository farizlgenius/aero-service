using System;

namespace Aero.Domain.Entities;

public sealed class AccessLevel : BaseDomain
{
    public short DriverId { get; private set; }
      public string Name {get; private set;} = string.Empty;
      public List<AccessLevelComponent> Components {get; private set;} = new List<AccessLevelComponent>();

       public AccessLevel() { }

    public AccessLevel(short driverid,string name,List<AccessLevelComponent> components,int location) : base(location)
    {
        SetDriverId(driverid);
        SetName(name);
        SetComponents(components);
    }

    private void SetDriverId(short driver) 
    {
        if (driver <= 0) throw new ArgumentException("Access Level Id invalid.");
        DriverId = driver;
    }

    private void SetName(string name) 
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        this.Name = name;
    }
    private void SetComponents(List<AccessLevelComponent> components) 
    {
        this.Components = components;
    }
}
