using System;
using Aero.Application.DTOs;
using Aero.Domain.Entities;
using Aero.Domain.Interfaces;

namespace Aero.Application.Interfaces;

public interface IQCredRepository : IBaseQueryRespository<CredentialDto>
{
      Task<short> GetLowestUnassignedIssueCodeByUserIdAsync(int max,string UserId);
      Task<bool> IsAnyWithCardNumberAsync(long cardno);
      Task<IEnumerable<CredentialDto>> GetByUserIdAsync(string UserId);
      Task<IEnumerable<Mode>> GetCredentialFlagAsync();
      Task<string> GetCardHolderFullnameByCardNoAsync(long cardno);
}
