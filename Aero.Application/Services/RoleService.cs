using Aero.Application.Constants;
using Aero.Application.DTOs;
using Aero.Application.Helpers;
using Aero.Application.Interfaces;
using Aero.Application.Mapper;
using Aero.Domain.Entities;
using Aero.Domain.Interface;
using System.ComponentModel;
using System.Net;

namespace Aero.Application.Services
{
    public sealed class RoleService(IQRoleRepository qRole,IRoleRepository rRole) : IRoleService
    {
        public async Task<ResponseDto<bool>> CreateAsync(RoleDto dto)
        {
            if (await qRole.IsAnyByNameAsync(dto.Name)) return ResponseHelper.Duplicate<bool>();

            var ComponentId = await qRole.GetLowestUnassignedNumberAsync(10,"");

            var domain = RoleMapper.ToDomain(dto);
            domain.ComponentId = ComponentId;

            var status = await rRole.AddAsync(domain);
            if(status <= 0) return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.SAVE_DATABASE_UNSUCCESS,[]);
            return ResponseHelper.SuccessBuilder<bool>(true);
        }

        public async Task<ResponseDto<bool>> DeleteByComponentIdAsync(short ComponentId)
        {

            if (!await qRole.IsAnyByComponentId(ComponentId)) return ResponseHelper.NotFoundBuilder<bool>();

            if (await qRole.IsAnyReferenceByComponentIdAsync(ComponentId) ) return ResponseHelper.FoundReferenceBuilder<bool>([ResponseMessage.FOUND_REFERENCE]);


            var status = await rRole.DeleteByComponentIdAsync(ComponentId);
            if(status <= 0) return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.DELETE_DATABASE_UNSUCCESS,[]);

            return ResponseHelper.SuccessBuilder<bool>(true);
        }

        public async Task<ResponseDto<IEnumerable<ResponseDto<bool>>>> DeleteRangeAsync(List<short> dtos)
        {
            bool flag = true;
            List<ResponseDto<bool>> data = new List<ResponseDto<bool>>();
            foreach (var dto in dtos)
            {
                var re = await DeleteByComponentIdAsync(dto);
                if (re.code != HttpStatusCode.OK) flag = false;
                data.Add(re);
            }

            if (!flag) return ResponseHelper.UnsuccessBuilder<IEnumerable<ResponseDto<bool>>>(data);

            var res = ResponseHelper.SuccessBuilder<IEnumerable<ResponseDto<bool>>>(data);

            return res;
        }

        public async Task<ResponseDto<IEnumerable<RoleDto>>> GetAsync()
        {
            var dtos = await qRole.GetAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<RoleDto>>(dtos);

        }

        public async Task<ResponseDto<RoleDto>> GetByComponentIdAsync(short ComponentId)
        {
            var dto = await qRole.GetByComponentIdAsync(ComponentId);

            return ResponseHelper.SuccessBuilder(dto);
                
        }

        public async Task<ResponseDto<IEnumerable<RoleDto>>> GetByLocationAsync(short location)
        {
            var dto = await qRole.GetByLocationIdAsync(location);

            return ResponseHelper.SuccessBuilder(dto);
        }

        public async Task<ResponseDto<Pagination<RoleDto>>> GetPaginationAsync(PaginationParamsWithFilter param, short location)
        {
            var res = await qRole.GetPaginationAsync(param,location);
            return ResponseHelper.SuccessBuilder(res);
        }

        public async Task<ResponseDto<RoleDto>> UpdateAsync(RoleDto dto)
        {
            if(!await qRole.IsAnyByComponentId(dto.ComponentId)) return ResponseHelper.NotFoundBuilder<RoleDto>();

            var domain = RoleMapper.ToDomain(dto);

            var status = await rRole.UpdateAsync(domain);
            if(status <= 0) return ResponseHelper.UnsuccessBuilder<RoleDto>(ResponseMessage.UPDATE_RECORD_UNSUCCESS,[]);

            return ResponseHelper.SuccessBuilder(dto);
        }
    }
}
