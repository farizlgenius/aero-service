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
        public async Task<ResponseDto<bool>> CreateAsync(CreateOperatorDto dto)
        {
            if (await context.Operators.AnyAsync(d => String.Equals(dto.Username,d.Username))) return ResponseHelper.Duplicate<bool>();

            var ComponentId = await helperService.GetLowestUnassignedNumberNoLimitAsync<Operator>(context);

            var en = MapperHelper.DtoToOperator(dto,ComponentId,DateTime.Now);

            await context.Operators.AddAsync(en);
            await context.SaveChangesAsync();

            return ResponseHelper.SuccessBuilder<bool>(true);

        }

        public async Task<ResponseDto<bool>> DeleteByUsernameAsync(string Username)
        {
            var en = await context.Operators
                .Where(o => o.Username == Username)
                .FirstOrDefaultAsync();

            if (en is null) return ResponseHelper.NotFoundBuilder<bool>();

            context.Operators.Remove(en);
            await context.SaveChangesAsync();

            return ResponseHelper.SuccessBuilder<bool>(true);

        }

        public async Task<ResponseDto<IEnumerable<OperatorDto>>> GetAsync()
        {
            var dto = await context.Operators
                .AsNoTracking()
                .Include(x => x.OperatorLocations)
                .ThenInclude(x => x.Location)
                .Select(x => MapperHelper.OperatorToDto(x))
                .ToArrayAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<OperatorDto>>(dto);
        }

        public async Task<ResponseDto<IEnumerable<OperatorDto>>> GetByLocationAsync(short location)
        {
            var dto = await context.Operators
                .AsNoTracking()
                .Include(x => x.OperatorLocations)
                .ThenInclude(x => x.Location)
                .Where(x => x.OperatorLocations.Select(x => x.LocationId).Contains(location))
                .Select(x => MapperHelper.OperatorToDto(x))
                .ToArrayAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<OperatorDto>>(dto);
        }

        public async Task<ResponseDto<OperatorDto>> GetByUsernameAsync(string Username)
        {
            var dto = await context.Operators
                .AsNoTracking()
                .Include(x => x.OperatorLocations)
                .ThenInclude(x => x.Location)
                .Where(o => String.Equals(Username, o.Username))
                .Select(o => MapperHelper.OperatorToDto(o))
                .FirstOrDefaultAsync();

            if(dto is null) return ResponseHelper.NotFoundBuilder<OperatorDto>();
                
            return ResponseHelper.SuccessBuilder<OperatorDto>(dto);
        }

        public async Task<ResponseDto<CreateOperatorDto>> UpdateAsync(CreateOperatorDto dto)
        {
            var en = await context.Operators
                .Include(x => x.OperatorLocations)
                .ThenInclude(x => x.Location)
                .Where(o => String.Equals(dto.Username, o.Username))
                .FirstOrDefaultAsync();

            if(en is null) return ResponseHelper.NotFoundBuilder<CreateOperatorDto>();

            MapperHelper.UpdateOperator(en,dto);

            context.Operators.Update(en);
            await context.SaveChangesAsync();

            return ResponseHelper.SuccessBuilder(dto);
        }
    }
}
