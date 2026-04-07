using System;
using Aero.Application.Interfaces;
using Aero.Application.Services;
using Microsoft.AspNetCore.Authorization;

namespace Aero.Api.Authorization;

public sealed class ScopeHandler(IPermissionService service) : AuthorizationHandler<ScopeRequirement>
{
      protected override async Task HandleRequirementAsync(
          AuthorizationHandlerContext context,
          ScopeRequirement requirement)
      {
            var role = context.User.FindFirst("role_id")?.Value;
            if (role == null) return;
            var d = requirement.Scope.Split('.');
            if (d.Length != 2) return;

            var permissions = await service.IsPermissionAllowAsync(int.Parse(role), d[0], d[1]);

      

            if (permissions)
                  context.Succeed(requirement);
      }
}