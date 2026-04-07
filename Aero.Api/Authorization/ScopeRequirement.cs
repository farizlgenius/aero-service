using System;
using Microsoft.AspNetCore.Authorization;

namespace Aero.Api.Authorization;

public class ScopeRequirement : IAuthorizationRequirement
{
    public string Scope { get; }
    public ScopeRequirement(string scope) => Scope = scope;
}