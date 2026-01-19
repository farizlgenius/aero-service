

using Aero.Application.DTOs;

namespace Aero.Application.Interface
{
    public interface ICardHolderService
    {
        Task<ResponseDto<IEnumerable<CardHolderDto>>> GetAsync();
        Task<ResponseDto<IEnumerable<CardHolderDto>>> GetByLocationIdAsync(short locaion);
        Task<ResponseDto<CardHolderDto>> GetByUserIdAsync(string UserId);
        Task<ResponseDto<bool>> CreateAsync(CardHolderDto dto);
        Task<ResponseDto<bool>> DeleteAsync(string UserId);
        Task<ResponseDto<CardHolderDto>> UpdateAsync(CardHolderDto dto);
    }
}
