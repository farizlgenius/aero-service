using HIDAeroService.Data;
using HIDAeroService.DTO;
using HIDAeroService.DTO.Operator;
using HIDAeroService.Entity;
using HIDAeroService.Helpers;
using HIDAeroService.Mapper;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace HIDAeroService.Service.Impl
{
    public sealed class OperatorService(AppDbContext context,IHelperService<Operator> helperService) : IOperatorService
    {
        public async Task<ResponseDto<bool>> CreateAsync(OperatorDto dto)
        {
            if (await context.Operators.AnyAsync(d => String.Equals(dto.UserName,d.UserName))) return ResponseHelper.Duplicate<bool>();

            var ComponentId = await helperService.GetLowestUnassignedNumberNoLimitAsync<Operator>(context);

            var en = MapperHelper.DtoToOperator(dto,ComponentId,DateTime.Now);

            await context.Operators.AddAsync(en);
            await context.SaveChangesAsync();

            return ResponseHelper.SuccessBuilder<bool>(true);

        }

        public async Task<ResponseDto<bool>> DeleteByUsernameAsync(string Username)
        {
            var en = await context.Operators
                .Where(o => o.UserName == Username)
                .FirstOrDefaultAsync();

            if (en is null) return ResponseHelper.NotFoundBuilder<bool>();

            context.Operators.Remove(en);
            await context.SaveChangesAsync();

            return ResponseHelper.SuccessBuilder<bool>(true);

        }

        public async Task<ResponseDto<IEnumerable<OperatorDto>>> GetAsync()
        {
            return ResponseHelper.SuccessBuilder<IEnumerable<OperatorDto>>(
                await context.Operators
                .AsNoTracking()
                .Select(x => MapperHelper.OperatorToDto(x))
                .ToArrayAsync()
             );
        }

        public async Task<ResponseDto<OperatorDto>> GetByUsernameAsync(string Username)
        {
            var dto = await context.Operators
                .AsNoTracking()
                .Where(o => String.Equals(Username, o.UserName))
                .Select(o => MapperHelper.OperatorToDto(o))
                .FirstOrDefaultAsync();

            if(dto is null) return ResponseHelper.NotFoundBuilder<OperatorDto>();
                
            return ResponseHelper.SuccessBuilder<OperatorDto>(dto);
        }

        public async Task<ResponseDto<OperatorDto>> UpdateAsync(OperatorDto dto)
        {
            var en = await context.Operators
                .Where(o => String.Equals(dto.UserName, o.UserName))
                .FirstOrDefaultAsync();

            if(en is null) return ResponseHelper.NotFoundBuilder<OperatorDto>();

            MapperHelper.UpdateOperator(en,dto);

            context.Operators.Update(en);
            await context.SaveChangesAsync();

            return ResponseHelper.SuccessBuilder(dto);
        }
    }
}
