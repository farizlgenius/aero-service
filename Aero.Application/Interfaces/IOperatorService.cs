
using Aero.Application.DTOs;

namespace Aero.Application.Interface
{
    public interface IOperatorService
    {
        Task<ResponseDto<IEnumerable<OperatorDto>>> GetAsync();
        Task<ResponseDto<IEnumerable<OperatorDto>>> GetByLocationAsync(short location);
        Task<ResponseDto<bool>> CreateAsync(CreateOperatorDto dto);
        Task<ResponseDto<bool>> DeleteByIdAsync(short component);
        Task<ResponseDto<CreateOperatorDto>> UpdateAsync(CreateOperatorDto dto);
        Task<ResponseDto<OperatorDto>> GetByUsernameAsync(string username);
        Task<ResponseDto<bool>> UpdatePasswordAsync(PasswordDto dto);
        Task<ResponseDto<IEnumerable<ResponseDto<bool>>>> DeleteRangeAsync(List<short> dtos);


    }
}
