using HIDAeroService.Constant;
using HIDAeroService.Data;
using HIDAeroService.DTO;
using HIDAeroService.DTO.Operator;
using HIDAeroService.Entity;
using HIDAeroService.Helpers;
using HIDAeroService.Mapper;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Security.Cryptography;

namespace HIDAeroService.Service.Impl
{
    public sealed class OperatorService(AppDbContext context,IHelperService<Operator> helperService) : IOperatorService
    {
        public async Task<ResponseDto<bool>> CreateAsync(CreateOperatorDto dto)
        {
            if (string.IsNullOrEmpty(dto.Password)) return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.PASSWORD_UNASSIGN, []);
            if (await context.Operators.AnyAsync(d => String.Equals(dto.Username,d.Username))) return ResponseHelper.Duplicate<bool>();

            var ComponentId = await helperService.GetLowestUnassignedNumberNoLimitAsync<Operator>(context);

            var en = MapperHelper.DtoToOperator(dto,ComponentId,DateTime.Now);

            await context.Operators.AddAsync(en);
            await context.SaveChangesAsync();

            return ResponseHelper.SuccessBuilder<bool>(true);

        }

        public async Task<ResponseDto<bool>> DeleteByIdAsync(short component)
        {
            var en = await context.Operators
                .Where(o => o.ComponentId == component)
                .FirstOrDefaultAsync();

            if (en is null) return ResponseHelper.NotFoundBuilder<bool>();

            // Delete Operator Location reference
            var loc = await context.OperatorLocations
                .Where(x => x.OperatorId == en.ComponentId)
                .ToArrayAsync();

            context.OperatorLocations.RemoveRange(loc);

            context.Operators.Remove(en);
            await context.SaveChangesAsync();

            return ResponseHelper.SuccessBuilder<bool>(true);

        }

        public async Task<ResponseDto<IEnumerable<ResponseDto<bool>>>> DeleteRangeAsync(List<short> dtos)
        {
            bool flag = true;
            List<ResponseDto<bool>> data = new List<ResponseDto<bool>>();
            foreach(var dto in dtos)
            {
                var re = await DeleteByIdAsync(dto);
                if (re.code != HttpStatusCode.OK) flag = false;
                data.Add(re);
            }

            if (!flag) return ResponseHelper.UnsuccessBuilder<IEnumerable<ResponseDto<bool>>>(data);

            var res = ResponseHelper.SuccessBuilder<IEnumerable<ResponseDto<bool>>>(data);

            return res;
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

            // Delete Operator Location reference
            var loc = await context.OperatorLocations
                .Where(x => x.OperatorId == dto.ComponentId)
                .ToArrayAsync();

            context.OperatorLocations.RemoveRange(loc);

            MapperHelper.UpdateOperator(en,dto);

            context.Operators.Update(en);
            await context.SaveChangesAsync();

            return ResponseHelper.SuccessBuilder(dto);
        }

        public async Task<ResponseDto<bool>> UpdatePasswordAsync(PasswordDto dto)
        {
            var en = await context.Operators
                    .Where(x => x.Username == dto.Username)
                    .OrderBy(x => x.Id)
                    .FirstOrDefaultAsync();

            if (en is null) return ResponseHelper.NotFoundBuilder<bool>();

            if (!EncryptHelper.VerifyPassword(dto.Old,en.Password)) return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.UNSUCCESS, [ResponseMessage.OLD_PASSPORT_INCORRECT]);

            en.Password = EncryptHelper.HashPassword(dto.New);

            context.Operators.Update(en);

            await context.SaveChangesAsync();

            return ResponseHelper.SuccessBuilder<bool>(true);


        }
    }
}
