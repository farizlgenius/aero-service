

using Aero.Application.DTOs;

namespace Aero.Application.Interface;

public interface IApiService
{
    #region License Initial Session
    Task<BaseHttpResponse<ExchangeResponse>> ExchangeAsync(ExchangeRequest body);
    Task<BaseHttpResponse<bool>> VerifyAsync(VerifyRequest body);

    #endregion

    #region License Session Demo License
    Task<BaseHttpResponse<EncryptedLicense>> GenerateDemoLicenseAsync(GenerateDemoRequest body);
    #endregion
}
