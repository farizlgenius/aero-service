using System;
using Microsoft.AspNetCore.Authorization;

namespace Aero.Api.Authorization;

public sealed class ScopeAttribute : AuthorizeAttribute
{
    public ScopeAttribute(string scope)
    {
        Policy = scope;
    }
}
