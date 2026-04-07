using Aero.Domain.Helpers;
using System;

namespace Aero.Domain.Entities;

public sealed class Role : BaseDomain
{
    public int Id { get; set; } 
    public string Name { get; set; } = string.Empty;
   public List<Permission> Features { get; set; } = new List<Permission>();

    public Role(int Id,string Name,List<Permission> Features)
    {
        if(Id < 0) throw new ArgumentException("Id invalid.",nameof(Id));
        this.Id = Id;
        SetName(Name);
        this.Features = Features;
    }

    private void SetName(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        if (!RegexHelper.IsValidName(name)) throw new ArgumentException("Invalid name.");
        Name = name;
    }



}
