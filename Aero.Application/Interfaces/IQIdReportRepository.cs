using System;
using Aero.Application.DTOs;
using Aero.Domain.Interfaces;

namespace Aero.Application.Interfaces;

public interface IQIdReportRepository : IBaseQueryRespository<IdReportDto>
{
      Task<IdReportDto> GetByMacAndScpIdAsync(string mac,short scpid);
      Task<int> GetCountAsync(short location);
    Task<IEnumerable<IdReportDto>> GetAsync(short location);
      Task<bool> IsAnyByMacAndScpIdAsync(string mac,int scpid);
      Task<int> UpdateAsync(IScpReply message);
      Task<int> AddAsync(IScpReply message);
      Task<IEnumerable<IdReportDto>> DeletePendingRecordAsync(short scpid);
      Task UpdateIpAddressAsync(int ScpId,string ip);
      Task UpdatePortAddressAsync(int ScpId,string port);
      Task<int> DeleteByMacAndScpIdAsync(string mac,int ScpId);
}
