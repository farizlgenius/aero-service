using HIDAeroService.DTO;
using HIDAeroService.DTO.CardHolder;

namespace HIDAeroService.Service
{
    public interface ICardHolderService
    {
        Task<ResponseDto<IEnumerable<CardHolderDto>>> GetAsync();
        Task<ResponseDto<CardHolderDto>> GetByUserIdAsync(string UserId);
        Task<ResponseDto<bool>> CreateAsync(CardHolderDto dto);
        Task<ResponseDto<bool>> DeleteAsync(string UserId);
        Task<ResponseDto<CardHolderDto>> UpdateAsync(CardHolderDto dto);
    }
}
