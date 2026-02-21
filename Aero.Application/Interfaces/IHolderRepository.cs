using System;
using Aero.Application.DTOs;
using Aero.Application.Interfaces;
using Aero.Domain.Entities;
using Aero.Domain.Interfaces;

namespace Aero.Application.Interface;

public interface IHolderRepository : IBaseRepository<CardHolderDto,CardHolder>
{
      Task<int> DeleteByUserIdAsync(string UserId);
      Task<int> DeleteReferenceByUserIdAsync(string UserId);
    Task<int> UpdateImagePathAsync(string path,string userid);
    Task<bool> IsAnyByUserId(string userid);
    Task<CardHolderDto> GetByUserIdAsync(string UserId);
    Task<IEnumerable<string>> GetMacsRelateCredentialByUserIdAsync(string UserId);
}
