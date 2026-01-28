using Aero.Application.DTOs;

namespace Aero.Application.Interface
{
    public interface IAccessLevelService
    {
        Task<ResponseDto<IEnumerable<AccessLevelDto>>> GetAsync();
        Task<ResponseDto<IEnumerable<AccessLevelDto>>> GetByLocationIdAsync(short location);
        Task<ResponseDto<AccessLevelDto>> GetByComponentIdAsync(short component);
        Task<ResponseDto<bool>> CreateAsync(CreateUpdateAccessLevelDto dto);
        Task<ResponseDto<bool>> DeleteAsync(short component);
        Task<ResponseDto<AccessLevelDto>> UpdateAsync(CreateUpdateAccessLevelDto dto);
        Task<string> GetAcrName(string mac, short component);
        Task<string> GetTzName(short component);

    }
}
