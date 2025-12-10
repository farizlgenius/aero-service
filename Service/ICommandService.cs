using HID.Aero.ScpdNet.Wrapper;
using HIDAeroService.AeroLibrary;
using static HID.Aero.ScpdNet.Wrapper.SCPReplyMessage;
using static HIDAeroService.Constants.Command;

namespace HIDAeroService.Service
{
    public interface ICommandService
    {
        void Save(int ScpId, string ScpMac, short TagNo, short CommandStatus, string Command, SCPReplyNAK nak);
        void HandleSaveFailCommand(AeroCommand write, SCPReplyMessage message);
    }
}
