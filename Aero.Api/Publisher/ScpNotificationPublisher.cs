using System;
using Aero.Api.Hubs;
using Aero.Application.Entities;
using Aero.Application.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace Aero.Infrastructure.Data.Publisher;

public class ScpNotificationPublisher(IHubContext<AeroHub> hub) : IScpNotificationPublisher
{
      public async Task IdReportNotifyAsync(List<IdReport> idReports)
      {
            await hub.Clients.All.SendAsync("SCP.ID_REPORT", idReports);
      }

      public async Task ScpNotifyConfigurationAsync(ScpConfiguratiion configuration)
      {
            await hub.Clients.All.SendAsync("SCP.DEVICE_CONFIGURATION", configuration);
      }

      public async Task ScpNotifyMemoryAllocate(MemoryAllocate allocate)
      {
            await hub.Clients.All.SendAsync("SCP.MEMORY_ALLOCATE", allocate);
      }

      public async Task ScpNotifyStatus(ScpStatus status)
      {
            await hub.Clients.All.SendAsync("SCP.STATUS", status);
      }
}
