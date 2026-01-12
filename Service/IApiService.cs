using System;
using AeroService.DTO.License;

namespace AeroService.Service;

public interface IApiService
{
    Task PostTrustedLicenseServer(string url,TrustServerDto body);
}
