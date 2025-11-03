using HIDAeroService.DTO;
using HIDAeroService.Entity;

namespace HIDAeroService.Service
{
    public interface IBaseService<TDto, TAdd, TEntity>
    {
        Task<ResponseDto<IEnumerable<TDto>>> GetAsync();
        Task<ResponseDto<TDto>> CreateAsync(TAdd dto);
        Task<ResponseDto<TDto>> DeleteAsync(string mac, short component);
        Task<ResponseDto<TDto>> UpdateAsync(TAdd dto);
        Task<ResponseDto<TDto>> GetByComponentAsync(string mac, short component);
        Task<ResponseDto<bool>> GetStatusAsync(string mac, short component);
        Task<ResponseDto<IEnumerable<ModeDto>>> GetModeAsync(int param);
    }
}
