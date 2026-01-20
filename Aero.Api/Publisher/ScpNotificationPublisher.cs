using System;
using Aero.Api.Hubs;
using Aero.Application.Entities;
using Aero.Application.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace Aero.Infrastructure.Data.Publisher;

public class ScpNotificationPublisher(IHubContext<AeroHub> hub) : IScpNotificationPublisher
{
      public async Task ScpNotifyStatus(ScpStatus status)
      {
            await hub.Clients.All.SendAsync("SCP.STATUS",status);
      }
}
