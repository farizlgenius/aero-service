using System;
using System.Net;
using System.Net.Http.Json;
using Aero.Domain.Entities;
using Aero.Domain.Interface;
using Aero.Infrastructure.External;
using Microsoft.Extensions.Options;

namespace Aero.Infrastructure.Repositories;

public class HttpRepository(HttpClient http,IOptions<AppSetting> options) : IHttpRepository
{
    private readonly AppSetting settings = options.Value;
    public async Task<HttpResponse<ExchangeResponse>> ExchangeAsync(ExchangeRequest body)
    {
        var response = await http.PostAsJsonAsync(settings.LicenseServerUrl + settings.Endpoints.Exchange, body);
        return await response.Content.ReadFromJsonAsync<HttpResponse<ExchangeResponse>>() ?? new HttpResponse<ExchangeResponse>(HttpStatusCode.InternalServerError, null, new Guid(), Constant.HttpResponseMessage.INTERNAL_ERROR, DateTime.UtcNow.ToLocalTime());
    }

    public async Task<HttpResponse<EncryptedLicense>> GenerateDemoLicenseAsync(GenerateDemoRequest body)
    {
        var response = await http.PostAsJsonAsync(settings.LicenseServerUrl + settings.Endpoints.GenerateDemo, body);
        return await response.Content.ReadFromJsonAsync<HttpResponse<EncryptedLicense>>() ?? new HttpResponse<EncryptedLicense>(HttpStatusCode.InternalServerError, null, new Guid(), Constant.HttpResponseMessage.INTERNAL_ERROR, DateTime.UtcNow.ToLocalTime());
    }

    public Task<HttpResponse<EncryptedLicense>> GenerateLicenseAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<HttpResponse<bool>> VerifyAsync(VerifyRequest body)
    {
        var response = await http.PostAsJsonAsync(settings.LicenseServerUrl, body);
        return await response.Content.ReadFromJsonAsync<HttpResponse<bool>>() ?? new HttpResponse<bool>(HttpStatusCode.InternalServerError, false, new Guid(), Constant.HttpResponseMessage.INTERNAL_ERROR, DateTime.UtcNow.ToLocalTime());
    }
}
