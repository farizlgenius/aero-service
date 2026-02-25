using System;
using Aero.Application.DTOs;
using Aero.Application.Interfaces;
using Aero.Domain.Entities;
using Aero.Domain.Interfaces;

namespace Aero.Application.Interface;

public interface IUserRepository : IBaseRepository<UserDto,User,User>
{
      Task<int> DeleteByUserIdAsync(string UserId);
      Task<int> DeleteReferenceByUserIdAsync(string UserId);
    Task<int> UpdateImagePathAsync(string path,string userid);
    Task<bool> IsAnyByUserId(string userid);
    Task<UserDto> GetByUserIdAsync(string UserId);
    Task<IEnumerable<string>> GetMacsRelateCredentialByUserIdAsync(string UserId);
}
