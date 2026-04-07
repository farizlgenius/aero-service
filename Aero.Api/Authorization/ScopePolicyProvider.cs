using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Aero.Api.Authorization;

public class ScopePolicyProvider : IAuthorizationPolicyProvider
{
    private readonly DefaultAuthorizationPolicyProvider _fallback;

    public ScopePolicyProvider(IOptions<AuthorizationOptions> options)
    {
        _fallback = new DefaultAuthorizationPolicyProvider(options);
    }

    public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
        => _fallback.GetDefaultPolicyAsync();

    public Task<AuthorizationPolicy?> GetFallbackPolicyAsync()
        => _fallback.GetFallbackPolicyAsync();

    public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        var policy = new AuthorizationPolicyBuilder()
            .AddRequirements(new ScopeRequirement(policyName))
            .Build();

        return Task.FromResult<AuthorizationPolicy?>(policy);
    }
}