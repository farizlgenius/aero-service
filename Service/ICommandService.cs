using HID.Aero.ScpdNet.Wrapper;
using HIDAeroService.Aero.CommandService;
using HIDAeroService.Aero.CommandService.Impl;
using static HID.Aero.ScpdNet.Wrapper.SCPReplyMessage;
using static HIDAeroService.Constants.Command;

namespace HIDAeroService.Service
{
    public interface ICommandService
    {
        void Save(int ScpId, string ScpMac, short TagNo, short CommandStatus, string Command, SCPReplyNAK nak);
        void HandleSaveFailCommand(AeroCommandService write, SCPReplyMessage message);
    }
}
