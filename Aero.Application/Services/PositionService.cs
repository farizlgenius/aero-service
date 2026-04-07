using System;
using System.Net;
using Aero.Application.Constants;
using Aero.Application.DTOs;
using Aero.Application.Helpers;
using Aero.Application.Interfaces;
using Aero.Domain.Entities;

namespace Aero.Application.Services;

public class PositionService(IPositionRepository repo) : IPositionService
{
      public async Task<ResponseDto<PositionDto>> CreateAsync(PositionDto dto)
      {
            // check license 

            if (await repo.IsAnyByNameAsync(dto.Name)) return ResponseHelper.Duplicate<PositionDto>();

            var domain = new Aero.Domain.Entities.Position(0,dto.Name,dto.Description,dto.LocationId,dto.IsActive);

            var status = await repo.AddAsync(domain);
            if(status <= 0) return ResponseHelper.UnsuccessBuilder<PositionDto>(ResponseMessage.SAVE_DATABASE_UNSUCCESS,[]);
            return ResponseHelper.SuccessBuilder <PositionDto> (await repo.GetByIdAsync(status));
      }

      public async Task<ResponseDto<PositionDto>> DeleteByIdAsync(int id)
      {
            var en = await repo.GetByIdAsync(id);

            if (en is null) return ResponseHelper.NotFoundBuilder<PositionDto>();


            if (await repo.IsAnyReferenceByIdAsync(id)) return ResponseHelper.FoundReferenceBuilder<PositionDto>();

            var status = await repo.DeleteByIdAsync(id);
            if(status <= 0) return ResponseHelper.UnsuccessBuilder<PositionDto>(ResponseMessage.DELETE_DATABASE_UNSUCCESS,[]);

            return ResponseHelper.SuccessBuilder<PositionDto>(en);
      }

      public async Task<ResponseDto<IEnumerable<PositionDto>>> DeleteRangeAsync(List<int> ids)
      {
           bool flag = true;
            List<PositionDto> data = new List<PositionDto>();
            foreach(var id in ids)
            {
                var re = await DeleteByIdAsync(id);
                if (re.code != HttpStatusCode.OK) flag = false;
                if(re.data is not null) data.Add(re.data);
            }

            if (!flag) return ResponseHelper.UnsuccessBuilder<IEnumerable<PositionDto>>(data);

            var res = ResponseHelper.SuccessBuilder<IEnumerable<PositionDto>>(data);

            return res;
      }

      public async Task<ResponseDto<IEnumerable<PositionDto>>> GetAsync()
      {
            var dto = await repo.GetAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<PositionDto>>(dto);
      }

      public async Task<ResponseDto<PositionDto>> GetByIdAsync(int id)
      {
            var dto = await repo.GetByIdAsync(id);

            if(dto is null) return ResponseHelper.NotFoundBuilder<PositionDto>();

            return ResponseHelper.SuccessBuilder(dto);
      }

      public async Task<ResponseDto<Pagination<PositionDto>>> GetPaginationAsync(PaginationParamsWithFilter param, int location)
      {
             var dtos = await repo.GetPaginationAsync(param,location);
            return ResponseHelper.SuccessBuilder<Pagination<PositionDto>>(dtos);
      }

      public async Task<ResponseDto<PositionDto>> UpdateAsync(PositionDto dto)
      {
            
            var en = await repo.GetByIdAsync(dto.Id);

            if (en is null) return ResponseHelper.NotFoundBuilder<PositionDto>();

            var domain = new Aero.Domain.Entities.Position(dto.Id,dto.Name,dto.Description,dto.LocationId,dto.IsActive);

             var status = await repo.UpdateAsync(domain);
            if(status <= 0) return ResponseHelper.UnsuccessBuilder<PositionDto>(ResponseMessage.UPDATE_RECORD_UNSUCCESS,[]);

            return ResponseHelper.SuccessBuilder<PositionDto>(dto);
      }
}
