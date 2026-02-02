using System;
using Aero.Application.DTOs;
using Aero.Domain.Interfaces;

namespace Aero.Application.Interfaces;

public interface IQIdReportRepository : IBaseQueryRespository<IdReportDto>
{
      Task<int> GetCountAsync();
      Task<bool> IsAnyByMacAndScpIdAsync(string mac,int scpid);
      Task<int> UpdateAsync(IScpReply message);
      Task<int> AddAsync(IScpReply message);
      Task<IEnumerable<IdReportDto>> DeletePendingRecordAsync(string mac,short scpid);
      Task UpdateIpAddressAsync(int ScpId,string ip);
      Task UpdatePortAddressAsync(int ScpId,string port);
      Task<int> DeleteByMacAndScpIdAsync(string mac,int ScpId);
}
