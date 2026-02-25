

using Aero.Application.DTOs;
using Aero.Domain.Entities;

namespace Aero.Application.Interface
{
    public interface IUserService
    {
        Task<ResponseDto<IEnumerable<UserDto>>> GetAsync();
        Task<ResponseDto<IEnumerable<UserDto>>> GetByLocationIdAsync(short locaion);
        Task<ResponseDto<Pagination<UserDto>>> GetPaginationAsync(PaginationParamsWithFilter param,short location);
        Task<ResponseDto<UserDto>> GetByUserIdAsync(string UserId);
        Task<ResponseDto<bool>> UploadImageAsync(string userid,Stream stream);
        Task<ResponseDto<bool>> CreateAsync(UserDto dto);
        Task<ResponseDto<bool>> DeleteAsync(string UserId);
        Task<ResponseDto<UserDto>> UpdateAsync(UserDto dto);
        Task<ResponseDto<bool>> DeleteImageAsync(string userid);
    }
}
