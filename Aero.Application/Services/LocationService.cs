using System.Net;
using Aero.Application.Constants;
using Aero.Application.DTOs;
using Aero.Application.Helpers;
using Aero.Application.Interface;
using Aero.Application.Interfaces;
using Aero.Application.Mapper;
using Aero.Domain.Entities;
using Aero.Domain.Interface;

namespace Aero.Application.Services
{
    public sealed class LocationService(IQLocationRespository qLoc,ILocationRepository rLoc) : ILocationService
    {
        public async Task<ResponseDto<bool>> CreateAsync(LocationDto dto)
        {
            if (await qLoc.IsAnyByLocationNameAsync(dto.LocationName)) return ResponseHelper.Duplicate<bool>();

            var domain = LocationMapper.ToDomain(dto);

            var LocationId = await qLoc.GetLowestUnassignedNumberAsync(10,"");
            if (LocationId == -1) return ResponseHelper.ExceedLimit<bool>();

            domain.ComponentId = LocationId;

            var status = await rLoc.AddAsync(domain);
            if(status <= 0) return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.SAVE_DATABASE_UNSUCCESS,[]);
            return ResponseHelper.SuccessBuilder < bool > (true);

        }

        public async Task<ResponseDto<bool>> DeleteByComponentIdAsync(short component)
        {
            if (component == 1) return ResponseHelper.DefaultRecord<bool>();

            var en = await qLoc.GetByComponentIdAsync(component);

            if (en is null) return ResponseHelper.NotFoundBuilder<bool>();

            List<string> errors = await qLoc.CheckRelateReferenceByComponentIdAsync(component);

            if (errors.Count > 0) return ResponseHelper.FoundReferenceBuilder<bool>(errors);

            var status = await rLoc.DeleteByComponentIdAsync(component);
            if(status <= 0) return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.DELETE_DATABASE_UNSUCCESS,[]);

            return ResponseHelper.SuccessBuilder<bool>(true);
        }

        public async Task<ResponseDto<IEnumerable<ResponseDto<bool>>>> DeleteRangeAsync(List<short> components)
        {
            bool flag = true;
            List<ResponseDto<bool>> data = new List<ResponseDto<bool>>();
            foreach(var id in components)
            {
                var re = await DeleteByComponentIdAsync(id);
                if (re.code != HttpStatusCode.OK) flag = false;
                data.Add(re);
            }

            if (!flag) return ResponseHelper.UnsuccessBuilder<IEnumerable<ResponseDto<bool>>>(data);

            var res = ResponseHelper.SuccessBuilder<IEnumerable<ResponseDto<bool>>>(data);

            return res;
        }

        public async Task<ResponseDto<IEnumerable<LocationDto>>> GetAsync()
        {
            var dto = await qLoc.GetAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<LocationDto>>(dto);
        }

        public async Task<ResponseDto<LocationDto>> GetByComponentIdAsync(short component)
        {
            var dto = await qLoc.GetByComponentIdAsync(component);

            if(dto is null) return ResponseHelper.NotFoundBuilder<LocationDto>();

            return ResponseHelper.SuccessBuilder(dto);
        }

        public async Task<ResponseDto<IEnumerable<LocationDto>>> GetRangeLocationById(LocationRangeDto locationIds)
        {
            
            var dtos = await qLoc.GetLocationsByListIdAsync(locationIds);
            return ResponseHelper.SuccessBuilder<IEnumerable<LocationDto>>(dtos);
        }


        public async Task<ResponseDto<Pagination<LocationDto>>> GetPaginationAsync(PaginationParamsWithFilter param,short location)
        {
            var dtos = await qLoc.GetPaginationAsync(param,location);
            return ResponseHelper.SuccessBuilder<Pagination<LocationDto>>(dtos);
        }

        public async Task<ResponseDto<LocationDto>> UpdateAsync(LocationDto dto)
        {
            if (dto.ComponentId == 1) return ResponseHelper.DefaultRecord<LocationDto>();
            
            var en = await qLoc.GetByComponentIdAsync(dto.ComponentId);

            if (en is null) return ResponseHelper.NotFoundBuilder<LocationDto>();

            var domain = LocationMapper.ToDomain(dto);

             var status = await rLoc.UpdateAsync(domain);
            if(status <= 0) return ResponseHelper.UnsuccessBuilder<LocationDto>(ResponseMessage.UPDATE_RECORD_UNSUCCESS,[]);

            return ResponseHelper.SuccessBuilder<LocationDto>(dto);
        }


    }
}
