using Aero.Domain.Helpers;
using System;

namespace Aero.Domain.Entities;

public sealed class Role : BaseDomain
{
    public int Id { get; set; } 
    public short DriverId { get; set; }
    public string Name { get; set; } = string.Empty;
   public List<Feature> Features { get; set; } = new List<Feature>();

    public Role(short DriverId,string Name,List<Feature> Features)
    {
        SetDriverId(DriverId);
        SetName(Name);
        this.Features = Features;
    }

    private void SetDriverId(short driver)
    {
        if (driver <= 0) throw new ArgumentException("Invalid driver id.",nameof(driver));
        this.DriverId = driver;
    }

    private void SetName(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        if (!RegexHelper.IsValidName(name)) throw new ArgumentException("Invalid name.");
        Name = name;
    }



}
