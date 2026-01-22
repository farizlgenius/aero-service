using System;
using Aero.Application.DTOs;
using Aero.Application.Entities;
using Aero.Application.Interfaces;

namespace Aero.Application.Services;

public class NotificationHandler(IScpNotificationPublisher publisher) : INotificationHandler
{
      public async Task ScpNotifyMemoryAllocate(MemoryAllocateDto allocate)
      {
            await publisher.ScpNotifyMemoryAllocate(allocate);
      }
}
