
using System.Net;
using Aero.Application.Constants;
using Aero.Application.DTOs;
using Aero.Application.Helpers;
using Aero.Application.Interface;
using Aero.Application.Interfaces;
using Aero.Application.Mapper;
using Aero.Domain.Helpers;
using Aero.Domain.Interface;

namespace Aero.Application.Services
{
    public sealed class OperatorService(IQOperatorRepository qOper,IOperatorRepository rOper) : IOperatorService
    {
        public async Task<ResponseDto<bool>> CreateAsync(CreateOperatorDto dto)
        {
            if (string.IsNullOrEmpty(dto.Password)) return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.PASSWORD_UNASSIGN, []);
            if (await qOper.IsAnyByUsernameAsync(dto.Username)) return ResponseHelper.Duplicate<bool>();

            var ComponentId = await qOper.GetLowestUnassignedNumberAsync(10,"");

            var domain = OperatorMapper.ToDomain(dto);
            domain.ComponentId = ComponentId;

            var status = await rOper.AddAsync(domain);
            if(status <= 0) return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.SAVE_DATABASE_UNSUCCESS,[]);

            return ResponseHelper.SuccessBuilder<bool>(true);

        }

        public async Task<ResponseDto<bool>> DeleteByIdAsync(short component)
        {

            if (!await qOper.IsAnyByComponentId(component)) return ResponseHelper.NotFoundBuilder<bool>();

            var status = await rOper.DeleteByComponentIdAsync(component);
            if(status <= 0) return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.DELETE_DATABASE_UNSUCCESS,[]);

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
           

            return ResponseHelper.SuccessBuilder<IEnumerable<OperatorDto>>(dto);
        }

        public async Task<ResponseDto<IEnumerable<OperatorDto>>> GetByLocationAsync(short location)
        {
            var dto = await qOper.GetAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<OperatorDto>>(dto);
        }

        public async Task<ResponseDto<OperatorDto>> GetByUsernameAsync(string Username)
        {
            
           var dto = await qOper.GetByUsernameAsync(Username);
                
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

            if (!await qOper.IsAnyByUsernameAsync(dto.Username)) return ResponseHelper.NotFoundBuilder<bool>();

    

            if (!EncryptHelper.VerifyPassword(dto.Old,domain.Old)) return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.UNSUCCESS, [ResponseMessage.OLD_PASSPORT_INCORRECT]);

            domain.New = EncryptHelper.HashPassword(dto.New);

            var status = await rOper.UpdateAsync(domain);
            if(status <= 0) return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.UPDATE_RECORD_UNSUCCESS,[]);

            return ResponseHelper.SuccessBuilder<bool>(true);


        }
    }
}
