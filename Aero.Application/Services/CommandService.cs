using HID.Aero.ScpdNet.Wrapper;
using AeroService.Aero.CommandService;
using AeroService.Aero.CommandService.Impl;
using AeroService.AeroLibrary;
using AeroService.Data;
using AeroService.DTO.Scp;
using AeroService.Entity;
using AeroService.Hubs;
using Microsoft.AspNetCore.SignalR;
using MiNET.Entities.Passive;
using Org.BouncyCastle.Crypto.Parameters;
using static HID.Aero.ScpdNet.Wrapper.SCPReplyMessage;

namespace AeroService.Service.Impl
{
    public sealed class CommandService(ILogger<CommandService> logger, AppDbContext context, IHelperService<CommandLog> helperService, AeroCommandService write) : ICommandService
    {


        public void Save(int ScpId, string ScpMac, short TagNo, short CommandStatus, string Command, SCPReplyNAK nak)
        {
            CommandLog s = new CommandLog();
            s.created_date = DateTime.UtcNow;
            s.updated_date = DateTime.UtcNow;
            s.hardware_mac = ScpMac;
            s.tag_no = TagNo;
            s.status = CommandStatus == 1 ? 'S' : 'F';
            s.command = Command;
            s.nak_reason = Description.GetNakReasonDescription(nak.reason);
            s.nake_desc_code = nak.description_code;
            s.hardware_id = ScpId;
            context.commnad_log.Add(s);
            context.SaveChanges();
        }


        public void HandleSaveFailCommand(AeroCommandService write, SCPReplyMessage message)
        {
            if (message.cmnd_sts.status == 2)
            {
                // Save to database 
                Save(message.SCPId, helperService.GetMacFromId((short)message.SCPId) == null ? "" : helperService.GetMacFromId((short)message.SCPId), (short)message.cmnd_sts.sequence_number, message.cmnd_sts.status, "", message.cmnd_sts.nak);
            }
        }


    }
}
