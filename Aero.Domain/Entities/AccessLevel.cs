using Aero.Domain.Helpers;
using System;

namespace Aero.Domain.Entities;

public sealed class AccessLevel : BaseDomain
{
    public int Id { get; private set; }
      public string Name {get; private set;} = string.Empty;
      public List<AccessLevelComponent> Components {get; private set;} = new List<AccessLevelComponent>();

       public AccessLevel() { }

    public AccessLevel(int id, string name, List<AccessLevelComponent> components, int location) : base(location)
    {
        SetId(id);
        SetName(name);
        SetComponents(components);
    }

    private void SetId(int id) 
    {
        if (id <= 0) throw new ArgumentException("Access Level Id invalid.");
        Id = id;
    }




    private void SetName(string name) 
    {
        
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        if (!RegexHelper.IsValidOnlyCharAndDigit(name)) throw new ArgumentException("Name invalid.");
        this.Name = name;
    }
    private void SetComponents(List<AccessLevelComponent> components) 
    {
        this.Components = components;
    }
}
