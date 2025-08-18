using HIDAeroService.Data;
using HIDAeroService.Dto.Scp;
using HIDAeroService.Hubs;
using Microsoft.AspNetCore.SignalR;
using MiNET.Entities.Passive;

namespace HIDAeroService.Service
{
    public sealed class CmndService
    {
        private readonly IHubContext<CmndHub> _hub;
        private readonly AppConfigData _config;

        public CmndService(AppConfigData config,IHubContext<CmndHub> hub)
        {
            _hub = hub;
            _config = config;
        }

        public void TriggerCommandResponse(short CmndStatus,int TagNumber,string NakReason,int NakDescriptionCode)
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
    }
}
