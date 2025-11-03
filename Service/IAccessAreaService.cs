using HIDAeroService.DTO;
using HIDAeroService.DTO.AccessArea;

namespace HIDAeroService.Service
{
    public interface IAccessAreaService
    {
        Task<ResponseDto<IEnumerable<AccessAreaDto>>> GetAsync();
        Task<ResponseDto<AccessAreaDto>> GetByComponentAsync(string mac,short component);
        Task<ResponseDto<bool>> CreateAsync(AccessAreaDto dto);
        Task<ResponseDto<bool>> DeleteAsync(string mac,short component);
        Task<ResponseDto<AccessAreaDto>> UpdateAsync(AccessAreaDto dto);
    }
}
