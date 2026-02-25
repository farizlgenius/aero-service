

using Aero.Application.DTOs;
using Aero.Application.Interfaces;
using Aero.Domain.Entities;
using Aero.Domain.Interfaces;

namespace Aero.Application.Interface
{
    public interface ITransactionService
    {
        Task<ResponseDto<Pagination<TransactionDto>>> GetPageTransactionWithCountAsync(PaginationParams param);
        Task<ResponseDto<Pagination<TransactionDto>>> GetPageTransactionWithCountAndDateAndSearchAsync(PaginationParamsWithFilter param, short location);
        Task<ResponseDto<IEnumerable<Mode>>> GetSourceAsync();
        Task<ResponseDto<IEnumerable<Mode>>> GetDeviceAsync(int source);

        Task<ResponseDto<bool>> SetTranIndexAsync(string mac);
        Task SaveToDatabaseAsync(IScpReply message);


    }
}
