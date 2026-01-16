using System;
using System.Net;
using AeroService.Constant;
using AeroService.Data;
using AeroService.Dto;
using AeroService.Dto.License;
using AeroService.DTO;
using AeroService.DTO.License;
using Microsoft.Extensions.Options;

namespace AeroService.Service.Impl;

public sealed class ApiService(HttpClient http,IOptions<AppConfigSettings> options) : IApiService
{
    private readonly AppConfigSettings settings = options.Value;
    public async Task<BaseHttpResponse<ExchangeResponse>> ExchangeAsync(ExchangeRequest body)
    {
        var response = await http.PostAsJsonAsync(settings.LicenseServerUrl + settings.ApiEndpoints.Exchange,body);
        return await response.Content.ReadFromJsonAsync<BaseHttpResponse<ExchangeResponse>>() ?? new BaseHttpResponse<ExchangeResponse>(HttpStatusCode.InternalServerError,null,new Guid(),ResponseMessage.INTERNAL_ERROR,DateTime.UtcNow.ToLocalTime());
    }

      public async Task<BaseHttpResponse<EncryptedLicense>> GenerateDemoLicenseAsync(GenerateDemoRequest body)
      {
            var response = await http.PostAsJsonAsync(settings.LicenseServerUrl + settings.ApiEndpoints.GenerateDemo,body);
            return await response.Content.ReadFromJsonAsync<BaseHttpResponse<EncryptedLicense>>() ?? new BaseHttpResponse<EncryptedLicense>(HttpStatusCode.InternalServerError,null,new Guid(),ResponseMessage.INTERNAL_ERROR,DateTime.UtcNow.ToLocalTime());
      }

      public async Task<BaseHttpResponse<bool>> VerifyAsync(VerifyRequest body)
    {
        var response = await http.PostAsJsonAsync(settings.LicenseServerUrl,body);
        return await response.Content.ReadFromJsonAsync<BaseHttpResponse<bool>>() ?? new BaseHttpResponse<bool>(HttpStatusCode.InternalServerError,false,new Guid(),ResponseMessage.INTERNAL_ERROR,DateTime.UtcNow.ToLocalTime());
    }
}
