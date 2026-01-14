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
    public async Task<ExchangeResponse> ExchangeAsync(ExchangeRequest body)
    {
        var response = await http.PostAsJsonAsync(settings.LicenseServerUrl,body);
        if (response.IsSuccessStatusCode)
        {
            var res = await response.Content.ReadFromJsonAsync<BaseHttpResponse<ExchangeResponse>>() ?? new BaseHttpResponse<ExchangeResponse>(HttpStatusCode.InternalServerError,new ExchangeResponse("","",""),new Guid(),ResponseMessage.INTERNAL_ERROR,DateTime.UtcNow.ToLocalTime());
            return res.payload;
        }
        return new ExchangeResponse("","","");
    }

    public async Task<bool> VerifyAsync(VerifyRequest body)
    {
        var response = await http.PostAsJsonAsync(settings.LicenseServerUrl,body);
        if (response.IsSuccessStatusCode)
        {
            var res = await response.Content.ReadFromJsonAsync<BaseHttpResponse<bool>>() ?? new BaseHttpResponse<bool>(HttpStatusCode.InternalServerError,false,new Guid(),ResponseMessage.INTERNAL_ERROR,DateTime.UtcNow.ToLocalTime());
            return res.payload;
        }
        return false;
    }
}
