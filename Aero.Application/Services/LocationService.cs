using System.Net;
using Aero.Application.Constants;
using Aero.Application.DTOs;
using Aero.Application.Helpers;
using Aero.Application.Interface;
using Aero.Application.Interfaces;
using Aero.Domain.Entities;
using Aero.Domain.Interface;

namespace Aero.Application.Services
{
    public sealed class LocationService(ILocationRepository repo) : ILocationService
    {
        public async Task<ResponseDto<LocationDto>> CreateAsync(LocationDto dto)
        {
            // check license 

            if (await repo.IsAnyByLocationNameAsync(dto.Name)) return ResponseHelper.Duplicate<LocationDto>();

            var domain = new Aero.Domain.Entities.Location(dto.Name,dto.Description);

            var status = await repo.AddAsync(domain);
            if(status <= 0) return ResponseHelper.UnsuccessBuilder<LocationDto>(ResponseMessage.SAVE_DATABASE_UNSUCCESS,[]);
            return ResponseHelper.SuccessBuilder <LocationDto> (await repo.GetByIdAsync(status));

        }

        public async Task<ResponseDto<LocationDto>> DeleteByIdAsync(int id)
        {
            if (id == 1) return ResponseHelper.DefaultRecord<LocationDto>();

            var en = await repo.GetByIdAsync(id);

            if (en is null) return ResponseHelper.NotFoundBuilder<LocationDto>();

            List<string> errors = await repo.CheckRelateReferenceByIdAsync(id);

            if (errors.Count > 0) return ResponseHelper.FoundReferenceBuilder<LocationDto>(errors);

            var status = await repo.DeleteByIdAsync(id);
            if(status <= 0) return ResponseHelper.UnsuccessBuilder<LocationDto>(ResponseMessage.DELETE_DATABASE_UNSUCCESS,[]);

            return ResponseHelper.SuccessBuilder<LocationDto>(en);
        }

        public async Task<ResponseDto<IEnumerable<LocationDto>>> DeleteRangeAsync(List<int> ids)
        {
            bool flag = true;
            List<LocationDto> data = new List<LocationDto>();
            foreach(var id in ids)
            {
                var re = await DeleteByIdAsync(id);
                if (re.code != HttpStatusCode.OK) flag = false;
                if(re.data is not null) data.Add(re.data);
            }

            if (!flag) return ResponseHelper.UnsuccessBuilder<IEnumerable<LocationDto>>(data);

            var res = ResponseHelper.SuccessBuilder<IEnumerable<LocationDto>>(data);

            return res;
        }

        public async Task<ResponseDto<IEnumerable<LocationDto>>> GetAsync()
        {
            var dto = await repo.GetAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<LocationDto>>(dto);
        }

        public async Task<ResponseDto<LocationDto>> GetByComponentIdAsync(int id)
        {
            var dto = await repo.GetByIdAsync(id);

            if(dto is null) return ResponseHelper.NotFoundBuilder<LocationDto>();

            return ResponseHelper.SuccessBuilder(dto);
        }

        public async Task<ResponseDto<IEnumerable<LocationDto>>> GetRangeLocationById(LocationRangeDto locationIds)
        {
            
            var dtos = await repo.GetLocationsByListIdAsync(locationIds);
            return ResponseHelper.SuccessBuilder<IEnumerable<LocationDto>>(dtos);
        }


        public async Task<ResponseDto<Pagination<LocationDto>>> GetPaginationAsync(PaginationParamsWithFilter param,int location)
        {
            var dtos = await repo.GetPaginationAsync(param,location);
            return ResponseHelper.SuccessBuilder<Pagination<LocationDto>>(dtos);
        }

        public async Task<ResponseDto<LocationDto>> UpdateAsync(LocationDto dto)
        {
            if (dto.Id == 1) return ResponseHelper.DefaultRecord<LocationDto>();
            
            var en = await repo.GetByIdAsync(dto.Id);

            if (en is null) return ResponseHelper.NotFoundBuilder<LocationDto>();

            var domain = new Aero.Domain.Entities.Location(dto.Name,dto.Description);

             var status = await repo.UpdateAsync(domain);
            if(status <= 0) return ResponseHelper.UnsuccessBuilder<LocationDto>(ResponseMessage.UPDATE_RECORD_UNSUCCESS,[]);

            return ResponseHelper.SuccessBuilder<LocationDto>(dto);
        }


    }
}
