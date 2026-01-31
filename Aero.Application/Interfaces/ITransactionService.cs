

using Aero.Application.DTOs;

namespace Aero.Application.Interface
{
    public interface ITransactionService
    {
        Task<ResponseDto<PaginationDto>> GetPageTransactionWithCountAsync(PaginationParams param);

        Task<ResponseDto<bool>> SetTranIndexAsync(string mac);

    }
}
