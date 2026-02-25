using Aero.Application.DTOs;
using Aero.Domain.Entities;
using Aero.Domain.Interfaces;
using System;

namespace Aero.Application.Interfaces;

public interface ICredRepository : IBaseRepository<CredentialDto,Credential,Credential>
{
      Task<int> DeleteByCardNoAsync(long Cardno);
    Task ToggleScanCardAsync(ScanCardDto dto);
    Task<short> GetLowestUnassignedIssueCodeByUserIdAsync(int max, string UserId);
    Task<bool> IsAnyWithCardNumberAsync(long cardno);
    Task<IEnumerable<CredentialDto>> GetByUserIdAsync(string UserId);
    Task<IEnumerable<Mode>> GetCredentialFlagAsync();
    Task<List<string>> GetCardHolderFullnameAndUserIdByCardNoAsync(double cardno);
}
