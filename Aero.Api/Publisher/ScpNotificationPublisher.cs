using System;
using Aero.Api.Hubs;
using Aero.Application.DTOs;
using Aero.Application.Entities;
using Aero.Application.Interfaces;
using Aero.Domain.Entities;
using Microsoft.AspNetCore.SignalR;

namespace Aero.Api.Publisher;

public class ScpNotificationPublisher(IHubContext<AeroHub> hub) : INotificationPublisher
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

      public async Task ScpNotifyTranStatus(TranStatus tran)
      {
            await hub.Clients.All.SendAsync("SCP.TRAN", tran);
      }

      public async Task SioNotifyStatus(SioStatus status)
      {
            await hub.Clients.All.SendAsync("SIO.STATUS", status);
      }


      public async Task CpNotifyStatus(CpStatus status)
      {
            //GetOnlineStatus()
            await hub.Clients.All.SendAsync("CP.STATUS", status);
      }

      public async Task CardScanNotifyStatus(CardScanStatus status)
      {
            await hub.Clients.All.SendAsync("CRED.STATUS", status);
            // read.isWaitingCardScan = false;
      }

      public async Task MpNotifyStatus(MpStatus status)
      {
            //GetOnlineStatus()
            await hub.Clients.All.SendAsync("MP.STATUS", status);
      }

      public async Task EventNotifyRecieve()
      {
            await hub.Clients.All.SendAsync("EVENT.TRIGGER");
      }
      public async Task AcrNotifyStatus(AcrStatus status)
      {
            await hub.Clients.All.SendAsync("ACR.STATUS",status);
      }



}
