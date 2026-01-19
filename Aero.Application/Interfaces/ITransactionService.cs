

using Aero.Application.DTOs;

namespace Aero.Application.Interface
{
    public interface ITransactionService
    {
        Task<ResponseDto<PaginationDto>> GetPageTransactionWithCountAsync(PaginationParams param);
        Task SaveToDatabaseAsync(SCPReplyMessage message);
        Task<ResponseDto<bool>> SetTranIndexAsync(string mac);
        void TriggerEventRecieve();
    }
}
