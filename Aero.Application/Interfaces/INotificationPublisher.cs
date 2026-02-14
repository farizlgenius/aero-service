using System;
using Aero.Application.DTOs;
using Aero.Application.Entities;
using Aero.Domain.Entities;

namespace Aero.Application.Interfaces;

public interface INotificationPublisher
{
      #region SCP
      Task ScpNotifyStatus(ScpStatus status);
      Task ScpNotifyMemoryAllocate(MemoryAllocateDto allocate);
      Task ScpNotifyConfigurationAsync(ScpConfiguratiion configuration);
      Task IdReportNotifyAsync(List<IdReportDto> idReports);
      Task ScpNotifyTranStatus(TranStatus tran);

      #endregion

      #region SIO
      Task SioNotifyStatus(SioStatus status);

      #endregion

      #region Door


      #endregion

      Task CpNotifyStatus(CpStatus status);

      Task CardScanNotifyStatus(CardScanStatus status);
      Task MpNotifyStatus(MpStatus status);
      Task EventNotifyRecieve();
        Task AcrNotifyStatus(AcrStatus status);
        Task CmndNotifyStatus(CmndStatus status);
}
