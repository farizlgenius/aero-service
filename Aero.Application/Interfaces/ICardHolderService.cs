

using Aero.Application.DTOs;
using Aero.Domain.Entities;

namespace Aero.Application.Interface
{
    public interface ICardHolderService
    {
        Task<ResponseDto<IEnumerable<CardHolderDto>>> GetAsync();
        Task<ResponseDto<IEnumerable<CardHolderDto>>> GetByLocationIdAsync(short locaion);
        Task<ResponseDto<Pagination<CardHolderDto>>> GetPaginationAsync(PaginationParamsWithFilter param,short location);
        Task<ResponseDto<CardHolderDto>> GetByUserIdAsync(string UserId);
        Task<ResponseDto<bool>> UploadImageAsync(string userid,Stream stream);
        Task<ResponseDto<bool>> CreateAsync(CardHolderDto dto);
        Task<ResponseDto<bool>> DeleteAsync(string UserId);
        Task<ResponseDto<CardHolderDto>> UpdateAsync(CardHolderDto dto);
        Task<ResponseDto<bool>> DeleteImageAsync(string userid);
    }
}
