using HID.Aero.ScpdNet.Wrapper;
using AeroService.Aero.CommandService;
using AeroService.Aero.CommandService.Impl;
using static HID.Aero.ScpdNet.Wrapper.SCPReplyMessage;
using static AeroService.Constants.Command;

namespace AeroService.Service
{
    public interface ICommandService
    {
        void Save(int ScpId, string ScpMac, short TagNo, short CommandStatus, string Command, SCPReplyNAK nak);
        void HandleSaveFailCommand(AeroCommandService write, SCPReplyMessage message);
    }
}
