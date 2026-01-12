using System;
using AeroService.Data;
using AeroService.DTO;
using AeroService.DTO.License;
using Microsoft.Extensions.Options;

namespace AeroService.Service.Impl;

public sealed class ApiService(HttpClient http,IOptions<AppConfigSettings> options) : IApiService
{
    private readonly AppConfigSettings settings = options.Value;
    public async Task PostTrustedLicenseServer(string url, TrustServerDto body)
    {
        var response = await http.PostAsJsonAsync(settings.LicenseUrl,body);
        if (response.IsSuccessStatusCode)
        {
            
        }
    }
}
