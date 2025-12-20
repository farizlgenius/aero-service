using HIDAeroService.DTO;
using HIDAeroService.DTO.Interval;

namespace HIDAeroService.Service
{
    public interface IIntervalService
    {
        Task<ResponseDto<IEnumerable<IntervalDto>>> GetAsync();
        Task<ResponseDto<IEnumerable<IntervalDto>>> GetByLocationAsync(short location);
        Task<ResponseDto<IntervalDto>> GetByIdAsync(short component);
        Task<ResponseDto<bool>> CreateAsync(CreateIntervalDto dto);
        Task<ResponseDto<IntervalDto>> UpdateAsync(IntervalDto dto);
        Task<ResponseDto<bool>> DeleteAsync(short component);
        Task<ResponseDto<IEnumerable<ResponseDto<bool>>>> DeleteRangeAsync(List<short> components);
    }
}
