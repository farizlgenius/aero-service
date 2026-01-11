using AeroService.DTO;
using AeroService.DTO.Credential;
using AeroService.Entity.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AeroService.Service
{
    public interface ICredentialService
    {
        Task<ResponseDto<bool>> CreateAsync(CredentialDto dto);
        Task<ResponseDto<IEnumerable<CredentialDto>>> GetAsync();
        Task<ResponseDto<CredentialDto>> GetByUserId(string UserId);
        Task<ResponseDto<bool>> UpdateAsync(CredentialDto dto);
        Task<ResponseDto<bool>> DeleteAsync(CredentialDto dto);
        Task<bool> ScanCardTrigger(ScanCardDto dto);
        void TriggerCardScan(int ScpId, int FormatNumber, int FacilityCode, double CardHolderId, int IssueCode, short FloorNumber);
        Task<int> GetLowestUnassignedIssueCodeAsync(string userId, int max = 1000);

        Task<ResponseDto<bool>> DeleteCardAsync(DeleteCardDto dto);
        Task<ResponseDto<IEnumerable<ModeDto>>> GetCredentialFlagAsync();
    }
}
