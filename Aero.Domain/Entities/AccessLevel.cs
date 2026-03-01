using Aero.Domain.Helpers;
using System;

namespace Aero.Domain.Entities;

public sealed class AccessLevel : BaseDomain
{
      public string Name {get; private set;} = string.Empty;
      public List<AccessLevelComponent> Components {get; private set;} = new List<AccessLevelComponent>();

       public AccessLevel() { }

    public AccessLevel(string name, List<AccessLevelComponent> components, int location,bool status) : base(location,status)
    {
        SetName(name);
        SetComponents(components);
    }


    private void SetName(string name) 
    {
        
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        if (!RegexHelper.IsValidName(name.Trim())) throw new ArgumentException("Name invalid.");
        this.Name = name;
    }
    private void SetComponents(List<AccessLevelComponent> components) 
    {
        this.Components = components;
    }
}
