

namespace Aero.Application.Interface
{
    public interface ICommandService
    {
        void Save(int ScpId, string ScpMac, short TagNo, short CommandStatus, string Command, SCPReplyNAK nak);
        void HandleSaveFailCommand(AeroCommandService write, SCPReplyMessage message);
    }
}
