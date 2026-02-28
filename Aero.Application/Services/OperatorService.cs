
using Aero.Application.Constants;
using Aero.Application.DTOs;
using Aero.Application.Helpers;
using Aero.Application.Interface;
using Aero.Application.Interfaces;
using Aero.Application.Mapper;
using Aero.Domain.Entities;
using Aero.Domain.Helpers;
using Aero.Domain.Interface;
using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Aero.Application.Services
{
    public sealed class OperatorService(IOperatorRepository repo) : IOperatorService
    {
        public async Task<ResponseDto<OperatorDto>> CreateAsync(CreateOperatorDto dto)
        {
            // Check value in license here 
            // ....to be implement


            if (string.IsNullOrEmpty(dto.Password)) return ResponseHelper.UnsuccessBuilder<OperatorDto>(ResponseMessage.PASSWORD_UNASSIGN, []);
            if (await repo.IsAnyByUsernameAsync(dto.Username)) return ResponseHelper.Duplicate<OperatorDto>();

            //var ComponentId = await repo.GetLowestUnassignedNumberAsync(10,"");
            //if (ComponentId == -1) return ResponseHelper.ExceedLimit<OperatorDto>();

            var domain = new Operator(dto.UserId,dto.Username,dto.Password,dto.Email,dto.title,dto.Firstname,dto.Middlename,dto.Lastname,dto.Phone,dto.Image,dto.Role,dto.LocationIds);

            var status = await repo.AddAsync(domain);
            if(status <= 0) return ResponseHelper.UnsuccessBuilder<OperatorDto>(ResponseMessage.SAVE_DATABASE_UNSUCCESS,[]);

            return ResponseHelper.SuccessBuilder<OperatorDto>(await repo.GetByIdAsync(status));

        }

        public async Task<ResponseDto<OperatorDto>> DeleteByIdAsync(int id)
        {
            var en = await repo.GetByIdAsync(id);
            if (en is null) return ResponseHelper.NotFoundBuilder<OperatorDto>();


            var status = await repo.DeleteByIdAsync(id);
            if(status <= 0) return ResponseHelper.UnsuccessBuilder<OperatorDto>(ResponseMessage.DELETE_DATABASE_UNSUCCESS,[]);

            return ResponseHelper.SuccessBuilder<OperatorDto>(en);

        }

        public async Task<ResponseDto<IEnumerable<OperatorDto>>> DeleteRangeAsync(List<int> dtos)
        {
            bool flag = true;
            List<OperatorDto> data = new List<OperatorDto>();
            foreach(var dto in dtos)
            {
                var re = await DeleteByIdAsync(dto);
                if (re.code != HttpStatusCode.OK) flag = false;
                if(re.data is not null) data.Add(re.data);
            }

            if (!flag) return ResponseHelper.UnsuccessBuilder<IEnumerable<OperatorDto>>(data);

            var res = ResponseHelper.SuccessBuilder<IEnumerable<OperatorDto>>(data);

            return res;
        }

        public async Task<ResponseDto<IEnumerable<OperatorDto>>> GetAsync()
        {
           
            var dto = await repo.GetAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<OperatorDto>>(dto);
        }

        public async Task<ResponseDto<IEnumerable<OperatorDto>>> GetByLocationAsync(int location)
        {
            var dto = await repo.GetByLocationIdAsync(location);
            return ResponseHelper.SuccessBuilder<IEnumerable<OperatorDto>>(dto);
        }

        public async Task<ResponseDto<OperatorDto>> GetByUsernameAsync(string Username)
        {
            
           var dto = await repo.GetByUsernameAsync(Username);
                
            return ResponseHelper.SuccessBuilder<OperatorDto>(dto);
        }

        public async Task<ResponseDto<Pagination<OperatorDto>>> GetPaginationAsync(PaginationParamsWithFilter param, int location)
        {
            var dtos = await repo.GetPaginationAsync(param,location);
            return ResponseHelper.SuccessBuilder<Pagination<OperatorDto>>(dtos);
        }

        public async Task<ResponseDto<OperatorDto>> UpdateAsync(CreateOperatorDto dto)
        {
            if(!await repo.IsAnyByUsernameAsync(dto.Username)) return ResponseHelper.NotFoundBuilder<OperatorDto>();

            var domain = new Operator(dto.UserId, dto.Username, dto.Password, dto.Email, dto.title, dto.Firstname, dto.Middlename, dto.Lastname, dto.Phone, dto.Image, dto.Role, dto.LocationIds);

            var status = await repo.UpdateAsync(domain);
            if(status <= 0) return ResponseHelper.UnsuccessBuilder<OperatorDto>(ResponseMessage.UPDATE_RECORD_UNSUCCESS,[]);

            var res = await repo.GetByUsernameAsync(dto.Username);

            return ResponseHelper.SuccessBuilder(res);
        }

        public async Task<ResponseDto<bool>> UpdatePasswordAsync(PasswordDto dto)
        {

            if (!await repo.IsAnyByUsernameAsync(dto.Username)) return ResponseHelper.NotFoundBuilder<bool>();

            var pass = await repo.GetPasswordByUsername(dto.Username);
    
            if (!EncryptHelper.VerifyPassword(dto.Old,pass)) return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.UNSUCCESS, [ResponseMessage.OLD_PASSPORT_INCORRECT]);

            var newPass = EncryptHelper.HashPassword(dto.New);
            
            var status = await repo.UpdatePasswordAsync(dto.Username,newPass);
            if(status <= 0) return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.UPDATE_RECORD_UNSUCCESS,[]);

            return ResponseHelper.SuccessBuilder<bool>(true);


        }
    }
}
