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
    public sealed class CmndService(IHubContext<CmndHub> hub, ILogger<CmndService> logger, AppDbContext context, IHelperService<ArCommandStatus> helperService, AeroCommand write)
    {


        public void Save(int ScpId, string ScpMac, short TagNo, short CommandStatus, string Command, SCPReplyNAK nak)
        {
            ArCommandStatus s = new ArCommandStatus();
            s.CreatedDate = DateTime.Now;
            s.UpdatedDate = DateTime.Now;
            s.ScpMac = ScpMac;
            s.TagNo = TagNo;
            s.CommandStatus = CommandStatus == 1 ? 'S' : 'F';
            s.Command = Command;
            s.NakReason = Description.GetNakReasonDescription(nak.reason);
            s.NakDescCode = nak.description_code;
            s.ScpId = ScpId;
            context.ArCommandStatuses.Add(s);
            context.SaveChanges();
        }


        public void TriggerVerifyScpConfiguration(VerifyScpConfigDto dto)
        {
            hub.Clients.All.SendAsync("VerifyConfig", dto);
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
