using Aero.Domain.Helpers;
using System;

namespace Aero.Domain.Entities;

public sealed class SubFeature
{
      public string Name { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;

    public SubFeature(string name,string path)
    {
        SetName(name);
        this.Path = path;
    }

    private void SetName(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        this.Name = name;
    }

}
