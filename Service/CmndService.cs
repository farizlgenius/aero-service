using HID.Aero.ScpdNet.Wrapper;
using HIDAeroService.AeroLibrary;
using HIDAeroService.Data;
using HIDAeroService.Dto.Scp;
using HIDAeroService.Entity;
using HIDAeroService.Hubs;
using Microsoft.AspNetCore.SignalR;
using MiNET.Entities.Passive;
using Org.BouncyCastle.Crypto.Parameters;
using static HID.Aero.ScpdNet.Wrapper.SCPReplyMessage;

namespace HIDAeroService.Service
{
    public sealed class CmndService
    {
        private readonly IHubContext<CmndHub> _hub;
        private readonly AeroLibMiddleware _config;
        private readonly ILogger<CmndService> _logger;
        private readonly AppDbContext _context;
        private readonly HelperService _helperService;

        public CmndService(AeroLibMiddleware config,IHubContext<CmndHub> hub,ILogger<CmndService> logger,AppDbContext context,HelperService helperService)
        {
            _helperService = helperService;
            _context = context;
            _logger = logger;
            _hub = hub;
            _config = config;
        }

        public void Save(int ScpId,string ScpMac,short TagNo,short CommandStatus,string Command,SCPReplyNAK nak)
        {
            try
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
                _context.ArCommandStatuses.Add(s);
                _context.SaveChanges();
            }
            catch(Exception e)
            {
                _logger.LogError(e.Message);
                Console.WriteLine(e.Message);
            }
        }

        public void TriggerCommandResponse(short CmndStatus,int TagNumber,string NakReason,int NakDescriptionCode,int ScpId)
        {
            if(_config.write.TagNo == TagNumber)
            {
                var result = _hub.Clients.All.SendAsync("CmndStatus", CmndStatus, TagNumber, NakReason, NakDescriptionCode);
            }

        }

        public void TriggerVerifyScpConfiguration(VerifyScpConfigDto dto)
        {
            _hub.Clients.All.SendAsync("VerifyConfig", dto);
        }

        public void HandleSaveFailCommand(WriteAeroDriver write, SCPReplyMessage message)
        {
            if (message.cmnd_sts.status == 2)
            {
                // Save to database 
                Save(message.SCPId, _helperService.GetMacFromId((short)message.SCPId), (short)message.cmnd_sts.sequence_number, message.cmnd_sts.status, write.Command, message.cmnd_sts.nak);
            }
        }

        public void HandleCommandResponse(SCPReplyMessage message)
        {
            TriggerCommandResponse(message.cmnd_sts.status, message.cmnd_sts.sequence_number, Description.GetNakReasonDescription(message.cmnd_sts.nak.reason), message.cmnd_sts.nak.description_code, message.SCPId);
        }
    }
}
