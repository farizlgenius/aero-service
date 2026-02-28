using Aero.Application.Constants;
using Aero.Application.DTOs;
using Aero.Application.Helpers;
using Aero.Application.Interface;
using Aero.Application.Interfaces;
using Aero.Application.Mapper;
using Aero.Domain.Entities;
using Aero.Domain.Interface;
using System.ComponentModel;
using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Aero.Application.Services
{
    public sealed class RoleService(IRoleRepository repo) : IRoleService
    {
        public async Task<ResponseDto<RoleDto>> CreateAsync(RoleDto dto)
        {
            // Check value in license here 
            // ....to be implement

            if (await repo.IsAnyByNameAsync(dto.Name.Trim())) return ResponseHelper.BadRequestName<RoleDto>();


            var DriverId = await repo.GetLowestUnassignedNumberAsync();

            var domain = new Domain.Entities.Role(DriverId,dto.Name,dto.Features.Select(x => new Feature(0,x.Name,x.Path,x.SubItem.Select(s => new SubFeature(s.Name,x.Path)).ToList(),x.IsAllow,x.IsCreate,x.IsModify,x.IsDelete,x.IsAction)).ToList());

            var status = await repo.AddAsync(domain);
            if(status <= 0) return ResponseHelper.UnsuccessBuilder<RoleDto>(ResponseMessage.SAVE_DATABASE_UNSUCCESS,[]);
            return ResponseHelper.SuccessBuilder<RoleDto>(await repo.GetByIdAsync(status));
        }

        public async Task<ResponseDto<RoleDto>> DeleteByIdAsync(int id)
        {
            var en = await repo.GetByIdAsync(id);

            if (en is null) return ResponseHelper.NotFoundBuilder<RoleDto>();

            if (await repo.IsAnyReferenceByIdAsync(id) ) return ResponseHelper.FoundReferenceBuilder<RoleDto>([ResponseMessage.FOUND_REFERENCE]);


            var status = await repo.DeleteByIdAsync(id);
            if(status <= 0) return ResponseHelper.UnsuccessBuilder<RoleDto>(ResponseMessage.DELETE_DATABASE_UNSUCCESS,[]);

            return ResponseHelper.SuccessBuilder<RoleDto>(en);
        }

        public async Task<ResponseDto<IEnumerable<RoleDto>>> DeleteRangeAsync(List<int> dtos)
        {
            bool flag = true;
            List<RoleDto> data = new List<RoleDto>();
            foreach (var dto in dtos)
            {
                var re = await DeleteByIdAsync(dto);
                if (re.code != HttpStatusCode.OK) flag = false;
                if(re.data is not null) data.Add(re.data);
            }

            if (!flag) return ResponseHelper.UnsuccessBuilder<IEnumerable<RoleDto>>(data);

            var res = ResponseHelper.SuccessBuilder<IEnumerable<RoleDto>>(data);

            return res;
        }

        public async Task<ResponseDto<IEnumerable<RoleDto>>> GetAsync()
        {
            var dtos = await repo.GetAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<RoleDto>>(dtos);

        }

        public async Task<ResponseDto<RoleDto>> GetByIdAsync(int id)
        {
            var dto = await repo.GetByIdAsync(id);

            return ResponseHelper.SuccessBuilder(dto);
                
        }

        public async Task<ResponseDto<IEnumerable<RoleDto>>> GetByLocationAsync(int location)
        {
            var dto = await repo.GetByLocationIdAsync(location);

            return ResponseHelper.SuccessBuilder(dto);
        }

        public async Task<ResponseDto<Pagination<RoleDto>>> GetPaginationAsync(PaginationParamsWithFilter param, int location)
        {
            var res = await repo.GetPaginationAsync(param,location);
            return ResponseHelper.SuccessBuilder(res);
        }

        public async Task<ResponseDto<RoleDto>> UpdateAsync(RoleDto dto)
        {
            if(!await repo.IsAnyById(dto.Id)) return ResponseHelper.NotFoundBuilder<RoleDto>();

            var domain = RoleMapper.ToDomain(dto);

            var status = await repo.UpdateAsync(domain);
            if(status <= 0) return ResponseHelper.UnsuccessBuilder<RoleDto>(ResponseMessage.UPDATE_RECORD_UNSUCCESS,[]);

            return ResponseHelper.SuccessBuilder(dto);
        }
    }
}
