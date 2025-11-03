using HIDAeroService.DTO;
using HIDAeroService.DTO.AccessLevel;

namespace HIDAeroService.Service
{
    public interface IAccessLevelService
    {
        Task<ResponseDto<IEnumerable<AccessLevelDto>>> GetAsync();
        Task<ResponseDto<AccessLevelDto>> GetByComponentIdAsync(short component);
        Task<ResponseDto<bool>> CreateAsync(AccessLevelDto dto);
        Task<ResponseDto<bool>> DeleteAsync(short component);
        Task<ResponseDto<AccessLevelDto>> UpdateAsync(AccessLevelDto dto);

        string GetAcrName(string mac, short component);
        string GetTzName(short component);
    }
}
