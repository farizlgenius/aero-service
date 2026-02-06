

using Aero.Application.DTOs;
using Aero.Domain.Interfaces;

namespace Aero.Application.Interface
{
    public interface ITransactionService
    {
        Task<ResponseDto<PaginationDto>> GetPageTransactionWithCountAsync(PaginationParams param);
        Task<ResponseDto<PaginationDto>> GetPageTransactionWithCountAndDateAndSearchAsync(PaginationParamsWithDate param);

        Task<ResponseDto<bool>> SetTranIndexAsync(string mac);
        Task SaveToDatabaseAsync(IScpReply message);

    }
}
