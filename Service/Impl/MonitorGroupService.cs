using HIDAeroService.DTO;
using HIDAeroService.DTO.MonitorGroup;

namespace HIDAeroService.Service.Impl
{
    public sealed class MonitorGroupService : IMonitorGroupService
    {
        public Task<ResponseDto<MonitorGroupDto>> CreateAsync(MonitorGroupDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto<MonitorGroupDto>> DeleteAsync(string mac, short component)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto<IEnumerable<MonitorGroupDto>>> GetAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto<MonitorGroupDto>> UpdateAsync(MonitorGroupDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
