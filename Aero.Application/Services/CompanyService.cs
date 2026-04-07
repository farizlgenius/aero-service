using System;
using System.Net;
using Aero.Application.Constants;
using Aero.Application.DTOs;
using Aero.Application.Helpers;
using Aero.Application.Interfaces;
using Aero.Domain.Entities;

namespace Aero.Application.Services;

public sealed class CompanyService(ICompanyRepository repo) : ICompanyService
{
      public async Task<ResponseDto<CompanyDto>> CreateAsync(CompanyDto dto)
      {
            // check license 

            if (await repo.IsAnyByNameAsync(dto.Name)) return ResponseHelper.Duplicate<CompanyDto>();

            var domain = new Aero.Domain.Entities.Company(0,dto.Name,dto.Description,dto.LocationId,dto.IsActive);

            var status = await repo.AddAsync(domain);
            if(status <= 0) return ResponseHelper.UnsuccessBuilder<CompanyDto>(ResponseMessage.SAVE_DATABASE_UNSUCCESS,[]);
            return ResponseHelper.SuccessBuilder <CompanyDto> (await repo.GetByIdAsync(status));
      }

      public async Task<ResponseDto<CompanyDto>> DeleteByIdAsync(int id)
      {
            var en = await repo.GetByIdAsync(id);

            if (en is null) return ResponseHelper.NotFoundBuilder<CompanyDto>();


            if (await repo.IsAnyReferenceByIdAsync(id)) return ResponseHelper.FoundReferenceBuilder<CompanyDto>();

            var status = await repo.DeleteByIdAsync(id);
            if(status <= 0) return ResponseHelper.UnsuccessBuilder<CompanyDto>(ResponseMessage.DELETE_DATABASE_UNSUCCESS,[]);

            return ResponseHelper.SuccessBuilder<CompanyDto>(en);
      }

      public async Task<ResponseDto<IEnumerable<CompanyDto>>> DeleteRangeAsync(List<int> ids)
      {
           bool flag = true;
            List<CompanyDto> data = new List<CompanyDto>();
            foreach(var id in ids)
            {
                var re = await DeleteByIdAsync(id);
                if (re.code != HttpStatusCode.OK) flag = false;
                if(re.data is not null) data.Add(re.data);
            }

            if (!flag) return ResponseHelper.UnsuccessBuilder<IEnumerable<CompanyDto>>(data);

            var res = ResponseHelper.SuccessBuilder<IEnumerable<CompanyDto>>(data);

            return res;
      }

      public async Task<ResponseDto<IEnumerable<CompanyDto>>> GetAsync()
      {
            var dto = await repo.GetAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<CompanyDto>>(dto);
      }

      public async Task<ResponseDto<CompanyDto>> GetByIdAsync(int id)
      {
            var dto = await repo.GetByIdAsync(id);

            if(dto is null) return ResponseHelper.NotFoundBuilder<CompanyDto>();

            return ResponseHelper.SuccessBuilder(dto);
      }

      public async Task<ResponseDto<Pagination<CompanyDto>>> GetPaginationAsync(PaginationParamsWithFilter param, int location)
      {
             var dtos = await repo.GetPaginationAsync(param,location);
            return ResponseHelper.SuccessBuilder<Pagination<CompanyDto>>(dtos);
      }

      public async Task<ResponseDto<CompanyDto>> UpdateAsync(CompanyDto dto)
      {
            
            var en = await repo.GetByIdAsync(dto.Id);

            if (en is null) return ResponseHelper.NotFoundBuilder<CompanyDto>();

            var domain = new Aero.Domain.Entities.Company(dto.Id,dto.Name,dto.Description,dto.LocationId,dto.IsActive);

             var status = await repo.UpdateAsync(domain);
            if(status <= 0) return ResponseHelper.UnsuccessBuilder<CompanyDto>(ResponseMessage.UPDATE_RECORD_UNSUCCESS,[]);

            return ResponseHelper.SuccessBuilder<CompanyDto>(dto);
      }
}
