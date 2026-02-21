using Aero.Application.DTOs;
using Aero.Application.Entities;
using Aero.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aero.Application.Interfaces
{
    public interface IIdReportRepository : IBaseRepository<IdReportDto,IdReport>
    {
        Task<IdReportDto> GetByMacAndScpIdAsync(string mac, short scpid);
        Task<int> GetCountAsync(short location);
        Task<IEnumerable<IdReportDto>> GetAsync(short location);
        Task<bool> IsAnyByMacAndScpIdAsync(string mac, int scpid);
        Task<int> UpdateAsync(IScpReply message);
        Task<int> AddAsync(IScpReply message);
        Task<IEnumerable<IdReportDto>> DeletePendingRecordAsync(short scpid);
        Task UpdateIpAddressAsync(int ScpId, string ip);
        Task UpdatePortAddressAsync(int ScpId, string port);
        Task<int> DeleteByMacAndScpIdAsync(string mac, int ScpId);
    }
}
