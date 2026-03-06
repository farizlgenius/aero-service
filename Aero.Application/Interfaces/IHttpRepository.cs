using System;
using Aero.Domain.Entities;

namespace Aero.Application.Interface;

public interface IHttpRepository 
{
    #region License Initial Session
    Task<HttpResponse<ExchangeResponse>> ExchangeAsync(ExchangeRequest body);
    Task<HttpResponse<bool>> VerifyAsync(VerifyRequest body);

    #endregion

    #region License Session Demo License
    Task<HttpResponse<EncryptedLicense>> GenerateDemoLicenseAsync(GenerateDemoRequest body);
    #endregion

    #region  License Session License

    Task<HttpResponse<EncryptedLicense>> GenerateLicenseAsync();

    #endregion
}
