using AeroService.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace AeroService.Service.Impl
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
