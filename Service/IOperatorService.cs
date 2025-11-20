using HIDAeroService.DTO;
using HIDAeroService.DTO.Operator;

namespace HIDAeroService.Service
{
    public interface IOperatorService
    {
        Task<ResponseDto<IEnumerable<OperatorDto>>> GetAsync();
        Task<ResponseDto<bool>> CreateAsync(OperatorDto dto);
        Task<ResponseDto<bool>> DeleteByUsernameAsync(string Username);
        Task<ResponseDto<OperatorDto>> UpdateAsync(OperatorDto dto);
        Task<ResponseDto<OperatorDto>> GetByUsernameAsync(string Username);
    }
}
