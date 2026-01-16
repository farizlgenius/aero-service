using System;
using AeroService.Dto;
using AeroService.Dto.License;
using AeroService.DTO;
using AeroService.DTO.License;

namespace AeroService.Service;

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
