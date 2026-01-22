using System;
using Aero.Application.DTOs;
using Aero.Application.Entities;

namespace Aero.Application.Interfaces;

public interface INotificationHandler
{
      Task ScpNotifyMemoryAllocate(MemoryAllocateDto allocate);
}
