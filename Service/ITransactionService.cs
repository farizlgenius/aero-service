using HID.Aero.ScpdNet.Wrapper;
using AeroService.DTO;
using AeroService.DTO.Transactions;

namespace AeroService.Service
{
    public interface ITransactionService
    {
        Task<ResponseDto<PaginationDto>> GetPageTransactionWithCountAsync(PaginationParams param);
        Task SaveToDatabaseAsync(SCPReplyMessage message);
        Task<ResponseDto<bool>> SetTranIndexAsync(string mac);
        void TriggerEventRecieve();
    }
}
