using Aero.Domain.Helpers;
using System;

namespace Aero.Domain.Entities;

public sealed class Location : BaseDomain
{        
        public int Id { get; set; } 
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

    public Location(string name,string description)
    {
        SetName(name);
        this.Description = description;
    }

    private void SetName(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        if (!RegexHelper.IsValidName(name)) throw new ArgumentException("Name invalid.");
        Name = name;
    }
}
