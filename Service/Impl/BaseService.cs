using AutoMapper;
using HIDAeroService.Data;
using HIDAeroService.DTO;
using HIDAeroService.Entity.Interface;
using HIDAeroService.Helpers;
using HIDAeroService.Utility;
using Microsoft.EntityFrameworkCore;

namespace HIDAeroService.Service.Impl
{
    public abstract class BaseService<TDto, TAdd, TEntity>(AppDbContext context,IMapper mapper) : IBaseService<TDto, TAdd, TEntity> where TEntity : class
    {
        public abstract Task<ResponseDto<TDto>> CreateAsync(TAdd dto);
        public abstract Task<ResponseDto<TDto>> DeleteAsync(string mac, short component);
        public abstract Task<ResponseDto<TDto>> UpdateAsync(TAdd dto);

        public abstract Task<ResponseDto<bool>> GetStatusAsync(string mac, short component);

        public abstract Task<ResponseDto<IEnumerable<ModeDto>>> GetModeAsync(int param);

        public abstract Task<ResponseDto<TDto>> GetByComponentAsync(string mac,short component);

        public virtual async Task<ResponseDto<IEnumerable<TDto>>> GetAsync()
        {
            var entities = await context.Set<TEntity>().AsNoTracking().ToArrayAsync();
            if (entities.Count() == 0) return ResponseHelper.NotFoundBuilder<IEnumerable<TDto>>();
            List<TDto> dtos = new List<TDto>();
            foreach (var entity in entities)
            {
                dtos.Add(mapper.Map<TDto>(entity));
            }
            return ResponseHelper.SuccessBuilder<IEnumerable<TDto>>(dtos);
        }


    }
}
