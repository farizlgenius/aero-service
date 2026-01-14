using System;
using AeroService.Dto.License;
using AeroService.DTO.License;

namespace AeroService.Service;

public interface IApiService
{
    #region License Initial Session
    Task<ExchangeResponse> ExchangeAsync(ExchangeRequest body);
    Task<bool> VerifyAsync(VerifyRequest body);

    #endregion
}
