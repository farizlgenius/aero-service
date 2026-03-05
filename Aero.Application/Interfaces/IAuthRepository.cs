using System;
using Aero.Domain.Entities;

namespace Aero.Application.Interface;

public interface IAuthRepository
{
      Task<int> RevokeTokenAsync(string hashed);
      Task<RefreshToken> GetRefreshTokenByHashed(string hashed);
      Task<int> AddRefreshTokenAsync(string hased,string username,string info,TimeSpan ttl);
      Task<int> RotateRefreshTokenAsync(string hashed,string username,string info,TimeSpan ttl);
}
