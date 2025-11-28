using HIDAeroService.Data;
using HIDAeroService.DTO;
using HIDAeroService.DTO.Location;
using HIDAeroService.Entity;
using HIDAeroService.Helpers;
using HIDAeroService.Mapper;
using Microsoft.EntityFrameworkCore;

namespace HIDAeroService.Service.Impl
{
    public sealed class LocationService(AppDbContext context,IHelperService<Location> helperService) : ILocationService
    {
        public async Task<ResponseDto<bool>> CreateAsync(LocationDto dto)
        {
            if (await context.Locations.AnyAsync(x => dto.LocationName == x.LocationName)) return ResponseHelper.Duplicate<bool>();
            var LocationId = await helperService.GetLowestUnassignedNumberNoLimitAsync<Location>(context);
            var entity = MapperHelper.DtoToLocation(dto,LocationId,DateTime.Now);
            await context.Locations.AddAsync(entity);
            await context.SaveChangesAsync();
            return ResponseHelper.SuccessBuilder < bool > (true);

        }

        public async Task<ResponseDto<bool>> DeleteByIdAsync(short Id)
        {
            var en = await context.Locations
                .Where(l => l.ComponentId == Id)
                .OrderBy(l => l.ComponentId)
                .FirstOrDefaultAsync();

            if (en is null) return ResponseHelper.NotFoundBuilder<bool>();

            context.Locations.Remove(en);
            await context.SaveChangesAsync();

            return ResponseHelper.SuccessBuilder<bool>(true);
        }

        public async Task<ResponseDto<IEnumerable<LocationDto>>> GetAsync()
        {
            var dto = await context.Locations
                .Select(x => MapperHelper.LocationToDto(x))
                .ToArrayAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<LocationDto>>(dto);
        }

        public async Task<ResponseDto<LocationDto>> GetByIdAsync(short id)
        {
            var dto = await context.Locations
                .Where(x => x.ComponentId == id)
                .Select(x => MapperHelper.LocationToDto(x))
                .FirstOrDefaultAsync();

            return ResponseHelper.SuccessBuilder(dto);
        }

        public async Task<ResponseDto<IEnumerable<LocationDto>>> GetRangeLocationById(LocationRangeDto locationIds)
        {
            var dtos = await context.Locations
                .AsNoTracking()
                .Where(x => locationIds.locationIds.Contains(x.ComponentId))
                .Select(x => MapperHelper.LocationToDto(x))
                .ToArrayAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<LocationDto>>(dtos);
        }

        public async Task<ResponseDto<LocationDto>> UpdateAsync(LocationDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
