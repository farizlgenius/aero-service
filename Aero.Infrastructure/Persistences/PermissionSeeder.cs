using System;
using Aero.Domain.Enums;

namespace Aero.Infrastructure.Persistences;

public static class PermissionSeeder
{
    // public static readonly string[] Resources =
    // {
    //     "device","door","user","role","location","company",
    //     "department","position","holiday","module","timezone"
    //     // add all controllers here
    // };

    public static readonly List<string> Resources = Enum.GetValues(typeof(Permission))
                 .Cast<Permission>()
                 .Select(s => s.ToString().ToLower())   // convert to string
                 .ToList();

    public static readonly List<string> Actions = Enum.GetValues(typeof(Aero.Domain.Enums.Action))
             .Cast<Aero.Domain.Enums.Action>()
             .Select(s => s.ToString().ToLower())   // convert to string
             .ToList();

    public static IEnumerable<string> GenerateScopes()
        => Resources.SelectMany(r => Actions.Select(a => $"{r}:{a}"));
}