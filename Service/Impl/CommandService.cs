using HID.Aero.ScpdNet.Wrapper;
using HIDAeroService.AeroLibrary;
using HIDAeroService.Data;
using HIDAeroService.DTO.Scp;
using HIDAeroService.Entity;
using HIDAeroService.Hubs;
using Microsoft.AspNetCore.SignalR;
using MiNET.Entities.Passive;
using Org.BouncyCastle.Crypto.Parameters;
using static HID.Aero.ScpdNet.Wrapper.SCPReplyMessage;

namespace HIDAeroService.Service.Impl
{
    public sealed class CommandService(ILogger<CommandService> logger, AppDbContext context, IHelperService<CommandStatus> helperService, AeroCommand write) : ICommandService
    {


        public void Save(int ScpId, string ScpMac, short TagNo, short CommandStatus, string Command, SCPReplyNAK nak)
        {
            CommandStatus s = new CommandStatus();
            s.CreatedDate = DateTime.Now;
            s.UpdatedDate = DateTime.Now;
            s.ScpMac = ScpMac;
            s.TagNo = TagNo;
            s.Status = CommandStatus == 1 ? 'S' : 'F';
            s.Command = Command;
            s.NakReason = Description.GetNakReasonDescription(nak.reason);
            s.NakDescCode = nak.description_code;
            s.ScpId = ScpId;
            context.ArCommandStatuses.Add(s);
            context.SaveChanges();
        }


        public void HandleSaveFailCommand(AeroCommand write, SCPReplyMessage message)
        {
            if (message.cmnd_sts.status == 2)
            {
                // Save to database 
                Save(message.SCPId, helperService.GetMacFromId((short)message.SCPId) == null ? "" : helperService.GetMacFromId((short)message.SCPId), (short)message.cmnd_sts.sequence_number, message.cmnd_sts.status, write.Command, message.cmnd_sts.nak);
            }
        }


    }
}
