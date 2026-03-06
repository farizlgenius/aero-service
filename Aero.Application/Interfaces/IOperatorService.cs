
using Aero.Application.DTOs;
using Aero.Domain.Entities;

namespace Aero.Application.Interface
{
    public interface IOperatorService
    {
        Task<ResponseDto<IEnumerable<OperatorDto>>> GetAsync();
        Task<ResponseDto<IEnumerable<OperatorDto>>> GetByLocationAsync(int id);
        Task<ResponseDto<OperatorDto>> CreateAsync(CreateOperatorDto dto);
        Task<ResponseDto<OperatorDto>> DeleteByIdAsync(int id);
        Task<ResponseDto<OperatorDto>> UpdateAsync(CreateOperatorDto dto);
        Task<ResponseDto<OperatorDto>> GetByUsernameAsync(string username);
        Task<ResponseDto<bool>> UpdatePasswordAsync(PasswordDto dto);
        Task<ResponseDto<IEnumerable<OperatorDto>>> DeleteRangeAsync(List<int> dtos);
        Task<ResponseDto<Pagination<OperatorDto>>> GetPaginationAsync(PaginationParamsWithFilter param,int location);


    }
}
