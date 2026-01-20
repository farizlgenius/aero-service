using System;
using Aero.Application.Entities;

namespace Aero.Application.Interfaces;

public interface IScpNotificationPublisher
{
      Task ScpNotifyStatus(ScpStatus status);
      Task ScpNotifyMemoryAllocate();
}
