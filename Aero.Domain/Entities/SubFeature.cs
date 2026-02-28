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
        SetPath(path);
    }

    private void SetName(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        if (!RegexHelper.IsValidName(name)) throw new ArgumentException("Name invalid.", nameof(name));
        this.Name = name;
    }

    private void SetPath(string path)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);
        this.Path = path;
    }
}
