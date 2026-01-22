using System;
using Aero.Application.DTOs;
using Aero.Application.Entities;

namespace Aero.Application.Interfaces;

public interface IScpNotificationPublisher
{
      Task ScpNotifyStatus(ScpStatus status);
      Task ScpNotifyMemoryAllocate(MemoryAllocateDto allocate);
      Task ScpNotifyConfigurationAsync(ScpConfiguratiion configuration);
      Task IdReportNotifyAsync(List<IdReportDto> idReports);
      Task ScpNotifyTranStatus(TranStatusDto tran);
}
