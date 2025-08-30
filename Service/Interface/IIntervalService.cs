using HIDAeroService.Dto;
using HIDAeroService.Dto.Interval;
using HIDAeroService.Dto.TimeZone;

namespace HIDAeroService.Service.Interface
{
    public interface IIntervalService
    {
        Task<Response<IEnumerable<IntervalDto>>> GetAsync();
        Task<Response<IntervalDto>> CreateAsync(IntervalDto dto);
        Task<Response<IntervalDto>> RemoveAsync(short componentNo);
        Task<Response<IntervalDto>> UpdateAsync(IntervalDto dto);
    }
}
