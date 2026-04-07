using System;
using System.Net;
using Aero.Application.Constants;
using Aero.Application.DTOs;
using Aero.Application.Helpers;
using Aero.Application.Interfaces;
using Aero.Domain.Entities;

namespace Aero.Application.Services;

public sealed class DepartmentService(IDepartmentRepository repo) : IDepartmentService
{
      public async Task<ResponseDto<DepartmentDto>> CreateAsync(DepartmentDto dto)
      {
            // check license 

            if (await repo.IsAnyByNameAsync(dto.Name)) return ResponseHelper.Duplicate<DepartmentDto>();

            var domain = new Aero.Domain.Entities.Department(0,dto.Name,dto.Description,dto.LocationId,dto.IsActive);

            var status = await repo.AddAsync(domain);
            if(status <= 0) return ResponseHelper.UnsuccessBuilder<DepartmentDto>(ResponseMessage.SAVE_DATABASE_UNSUCCESS,[]);
            return ResponseHelper.SuccessBuilder <DepartmentDto> (await repo.GetByIdAsync(status));
      }

      public async Task<ResponseDto<DepartmentDto>> DeleteByIdAsync(int id)
      {
            var en = await repo.GetByIdAsync(id);

            if (en is null) return ResponseHelper.NotFoundBuilder<DepartmentDto>();


            if (await repo.IsAnyReferenceByIdAsync(id)) return ResponseHelper.FoundReferenceBuilder<DepartmentDto>();

            var status = await repo.DeleteByIdAsync(id);
            if(status <= 0) return ResponseHelper.UnsuccessBuilder<DepartmentDto>(ResponseMessage.DELETE_DATABASE_UNSUCCESS,[]);

            return ResponseHelper.SuccessBuilder<DepartmentDto>(en);
      }

      public async Task<ResponseDto<IEnumerable<DepartmentDto>>> DeleteRangeAsync(List<int> ids)
      {
           bool flag = true;
            List<DepartmentDto> data = new List<DepartmentDto>();
            foreach(var id in ids)
            {
                var re = await DeleteByIdAsync(id);
                if (re.code != HttpStatusCode.OK) flag = false;
                if(re.data is not null) data.Add(re.data);
            }

            if (!flag) return ResponseHelper.UnsuccessBuilder<IEnumerable<DepartmentDto>>(data);

            var res = ResponseHelper.SuccessBuilder<IEnumerable<DepartmentDto>>(data);

            return res;
      }

      public async Task<ResponseDto<IEnumerable<DepartmentDto>>> GetAsync()
      {
            var dto = await repo.GetAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<DepartmentDto>>(dto);
      }

      public async Task<ResponseDto<DepartmentDto>> GetByIdAsync(int id)
      {
            var dto = await repo.GetByIdAsync(id);

            if(dto is null) return ResponseHelper.NotFoundBuilder<DepartmentDto>();

            return ResponseHelper.SuccessBuilder(dto);
      }

      public async Task<ResponseDto<Pagination<DepartmentDto>>> GetPaginationAsync(PaginationParamsWithFilter param, int location)
      {
             var dtos = await repo.GetPaginationAsync(param,location);
            return ResponseHelper.SuccessBuilder<Pagination<DepartmentDto>>(dtos);
      }

      public async Task<ResponseDto<DepartmentDto>> UpdateAsync(DepartmentDto dto)
      {
            
            var en = await repo.GetByIdAsync(dto.Id);

            if (en is null) return ResponseHelper.NotFoundBuilder<DepartmentDto>();

            var domain = new Aero.Domain.Entities.Department(dto.Id,dto.Name,dto.Description,dto.LocationId,dto.IsActive);

             var status = await repo.UpdateAsync(domain);
            if(status <= 0) return ResponseHelper.UnsuccessBuilder<DepartmentDto>(ResponseMessage.UPDATE_RECORD_UNSUCCESS,[]);

            return ResponseHelper.SuccessBuilder<DepartmentDto>(dto);
      }
}
