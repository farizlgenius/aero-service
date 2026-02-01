

using Aero.Application.DTOs;
using Aero.Domain.Entities;

namespace Aero.Application.Interface
{
    public interface ICredentialService
    {
        Task<ResponseDto<IEnumerable<CredentialDto>>> GetAsync();
        Task<ResponseDto<IEnumerable<CredentialDto>>> GetByUserId(string UserId);
        Task<bool> ScanCardTrigger(ScanCardDto dto);

        Task<ResponseDto<bool>> DeleteCardAsync(DeleteCardDto dto);
        Task<ResponseDto<IEnumerable<Mode>>> GetCredentialFlagAsync();
    }
}
