using HIDAeroService.DTO;
using HIDAeroService.DTO.AccessLevel;

namespace HIDAeroService.Service
{
    public interface IAccessLevelService
    {
        Task<ResponseDto<IEnumerable<AccessLevelDto>>> GetAsync();
        Task<ResponseDto<IEnumerable<AccessLevelDto>>> GetByLocationIdAsync(short location);
        Task<ResponseDto<AccessLevelDto>> GetByComponentIdAsync(short component);
        Task<ResponseDto<bool>> CreateAsync(CreateUpdateAccessLevelDto dto);
        Task<ResponseDto<bool>> DeleteAsync(short component);
        Task<ResponseDto<AccessLevelDto>> UpdateAsync(CreateUpdateAccessLevelDto dto);

        string GetAcrName(string mac, short component);
        string GetTzName(short component);
    }
}
