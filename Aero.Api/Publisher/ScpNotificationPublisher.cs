using System;
using Aero.Api.Hubs;
using Aero.Application.DTOs;
using Aero.Application.Entities;
using Aero.Application.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace Aero.Api.Publisher;

public class ScpNotificationPublisher(IHubContext<AeroHub> hub) : IScpNotificationPublisher
{
      public async Task IdReportNotifyAsync(List<IdReportDto> idReports)
      {
            await hub.Clients.All.SendAsync("SCP.ID_REPORT", idReports);
      }

      public async Task ScpNotifyConfigurationAsync(ScpConfiguratiion configuration)
      {
            await hub.Clients.All.SendAsync("SCP.DEVICE_CONFIGURATION", configuration);
      }

      public async Task ScpNotifyMemoryAllocate(MemoryAllocateDto allocate)
      {
            await hub.Clients.All.SendAsync("SCP.MEMORY_ALLOCATE", allocate);
      }

      public async Task ScpNotifyStatus(ScpStatus status)
      {
            await hub.Clients.All.SendAsync("SCP.STATUS", status);
      }

       public async Task ScpNotifyTranStatus(TranStatusDto tran)
        {
            await hub.Clients.All.SendAsync("SCP.TRAN", tran);
        }

}
