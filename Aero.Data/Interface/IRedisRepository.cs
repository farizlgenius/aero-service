using System;
using Aero.Domain.Entities;

namespace Aero.Domain.Interface;

public interface IRedisRepository
{
    Task<string?> GetAsync(string key);
    Task<bool> SetAsync(string key, string value, TimeSpan? expiry = null);
    Task<bool> KeyExistsAsync(string key);
    Task<bool> DeleteAsync(string key);
}
