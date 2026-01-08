using HID.Aero.ScpdNet.Wrapper;
using HIDAeroService.DTO;
using HIDAeroService.DTO.Transactions;

namespace HIDAeroService.Service
{
    public interface ITransactionService
    {
        Task<ResponseDto<PaginationDto>> GetPageTransactionWithCountAsync(PaginationParams param);
        Task SaveToDatabaseAsync(SCPReplyMessage message);
        Task<ResponseDto<bool>> SetTranIndexAsync(string mac);
        void TriggerEventRecieve();
    }
}
