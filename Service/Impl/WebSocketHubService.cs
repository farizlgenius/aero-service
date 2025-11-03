using HIDAeroService.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace HIDAeroService.Service.Impl
{
    public sealed class WebSocketHubService
    {
        private readonly IHubContext<AeroHub> _hub;
        public WebSocketHubService(IHubContext<AeroHub> hub)
        {
            _hub = hub;
        }
    }
}
