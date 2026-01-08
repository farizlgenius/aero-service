using HIDAeroService.Constant;
using HIDAeroService.Data;
using HIDAeroService.DTO;
using HIDAeroService.DTO.Operator;
using HIDAeroService.Entity;
using HIDAeroService.Helpers;
using HIDAeroService.Mapper;
using Microsoft.EntityFrameworkCore;
using MiNET.Entities;
using System.Net;
using System.Security.Cryptography;

namespace HIDAeroService.Service.Impl
{
    public sealed class OperatorService(AppDbContext context,IHelperService<Operator> helperService) : IOperatorService
    {
        public async Task<ResponseDto<bool>> CreateAsync(CreateOperatorDto dto)
        {
            if (string.IsNullOrEmpty(dto.Password)) return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.PASSWORD_UNASSIGN, []);
            if (await context.@operator.AnyAsync(d => String.Equals(dto.Username,d.user_name))) return ResponseHelper.Duplicate<bool>();

            var ComponentId = await helperService.GetLowestUnassignedNumberNoLimitAsync<Operator>(context);

            var en = MapperHelper.DtoToOperator(dto,ComponentId,DateTime.Now);

            await context.@operator.AddAsync(en);
            await context.SaveChangesAsync();

            return ResponseHelper.SuccessBuilder<bool>(true);

        }

        public async Task<ResponseDto<bool>> DeleteByIdAsync(short component)
        {
            var en = await context.@operator
                .Where(o => o.component_id == component)
                .FirstOrDefaultAsync();

            if (en is null) return ResponseHelper.NotFoundBuilder<bool>();

            // Delete operator location reference
            var loc = await context.operator_location
                .Where(x => x.operator_id == en.component_id)
                .ToArrayAsync();

            context.operator_location.RemoveRange(loc);

            context.@operator.Remove(en);
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
            var dto = await context.@operator
                .AsNoTracking()
                .Select(x => new OperatorDto
                {
                    Uuid = x.uuid,
                    LocationIds = x.operator_locations.Select(x => x.location.component_id).ToList(),
                    IsActive = x.is_active,

                    // extend_desc 
                    ComponentId = x.component_id,
                    Username = x.user_name,
                    Email = x.email,
                    Title = x.title,
                    FirstName = x.first_name,
                    MiddleName = x.middle_name,
                    LastName = x.last_name,
                    Phone = x.phone,
                    Image = x.image_path,
                    RoleId = x.role_id,
                })
                .ToArrayAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<OperatorDto>>(dto);
        }

        public async Task<ResponseDto<IEnumerable<OperatorDto>>> GetByLocationAsync(short location)
        {
            var dto = await context.@operator
                .AsNoTracking()
                .Where(x => x.operator_locations.Select(x => x.location_id).Contains(location))
                .Select(x => new OperatorDto
                {
                    Uuid = x.uuid,
                    LocationIds = x.operator_locations.Select(x => x.location.component_id).ToList(),
                    IsActive = x.is_active,

                    // extend_desc 
                    ComponentId = x.component_id,
                    Username = x.user_name,
                    Email = x.email,
                    Title = x.title,
                    FirstName = x.first_name,
                    MiddleName = x.middle_name,
                    LastName = x.last_name,
                    Phone = x.phone,
                    Image = x.image_path,
                    RoleId = x.role_id,
                })
                .ToArrayAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<OperatorDto>>(dto);
        }

        public async Task<ResponseDto<OperatorDto>> GetByUsernameAsync(string Username)
        {
            var dto = await context.@operator
                .AsNoTracking()
                .Where(o => String.Equals(Username, o.user_name))
                .Select(x => new OperatorDto
                {
                    Uuid = x.uuid,
                    LocationIds = x.operator_locations.Select(x => x.location.component_id).ToList(),
                    IsActive = x.is_active,

                    // extend_desc 
                    ComponentId = x.component_id,
                    Username = x.user_name,
                    Email = x.email,
                    Title = x.title,
                    FirstName = x.first_name,
                    MiddleName = x.middle_name,
                    LastName = x.last_name,
                    Phone = x.phone,
                    Image = x.image_path,
                    RoleId = x.role_id,
                })
                .FirstOrDefaultAsync();

            if(dto is null) return ResponseHelper.NotFoundBuilder<OperatorDto>();
                
            return ResponseHelper.SuccessBuilder<OperatorDto>(dto);
        }

        

        public async Task<ResponseDto<CreateOperatorDto>> UpdateAsync(CreateOperatorDto dto)
        {
            var en = await context.@operator
                .Include(x => x.operator_locations)
                .ThenInclude(x => x.location)
                .Where(o => String.Equals(dto.Username, o.user_name))
                .FirstOrDefaultAsync();

            if(en is null) return ResponseHelper.NotFoundBuilder<CreateOperatorDto>();

            // Delete operator location reference
            var loc = await context.operator_location
                .Where(x => x.operator_id == dto.ComponentId)
                .ToArrayAsync();

            context.operator_location.RemoveRange(loc);

            MapperHelper.UpdateOperator(en,dto);

            context.@operator.Update(en);
            await context.SaveChangesAsync();

            return ResponseHelper.SuccessBuilder(dto);
        }

        public async Task<ResponseDto<bool>> UpdatePasswordAsync(PasswordDto dto)
        {
            var en = await context.@operator
                    .Where(x => x.user_name == dto.Username)
                    .OrderBy(x => x.id)
                    .FirstOrDefaultAsync();

            if (en is null) return ResponseHelper.NotFoundBuilder<bool>();

            if (!EncryptHelper.VerifyPassword(dto.Old,en.password)) return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.UNSUCCESS, [ResponseMessage.OLD_PASSPORT_INCORRECT]);

            en.password = EncryptHelper.HashPassword(dto.New);

            context.@operator.Update(en);

            await context.SaveChangesAsync();

            return ResponseHelper.SuccessBuilder<bool>(true);


        }
    }
}
