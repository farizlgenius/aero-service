using HIDAeroService.DTO;
using HIDAeroService.DTO.Trigger;

namespace HIDAeroService.Service
{
    public interface ITriggerService
    {
        Task<ResponseDto<IEnumerable<TriggerDto>>> GetAsync();
        Task<ResponseDto<IEnumerable<TriggerDto>>> GetByLocationId(short location);
        Task<ResponseDto<bool>> CreateAsync(TriggerDto dto);
        Task<ResponseDto<bool>> DeleteAsync(string Mac,short ComponentId);
        Task<ResponseDto<TriggerDto>> UpdateAsync(TriggerDto dto);
    }
}
