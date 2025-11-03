using HID.Aero.ScpdNet.Wrapper;
using HIDAeroService.DTO;

namespace HIDAeroService.Service
{
    public interface IEventService
    {
        void SaveToDatabase(SCPReplyMessage message, string tranCodeDesc, string? additional = "");
        Task<ResponseDto<bool>> SetTranIndexAsync(string mac);
    }
}
