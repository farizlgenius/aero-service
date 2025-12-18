using HIDAeroService.Data;
using HIDAeroService.DTO;
using HIDAeroService.DTO.Location;
using HIDAeroService.Entity;
using HIDAeroService.Helpers;
using HIDAeroService.Mapper;
using Microsoft.EntityFrameworkCore;
using System.Net;

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
            if (Id == 1) return ResponseHelper.DefaultRecord<bool>();

            var errors = new List<string>();
            var en = await context.Locations
                .Where(l => l.ComponentId == Id)
                .OrderBy(l => l.ComponentId)
                .FirstOrDefaultAsync();

            if (en is null) return ResponseHelper.NotFoundBuilder<bool>();

            // Hardware
            if(await context.Locations
                .AnyAsync(x => x.ComponentId == Id && x.Hardwares.Count() > 0)
                )
            {
                errors.Add("Found relate hardware");
            }

            // Module
            if (await context.Locations
                .AnyAsync(x => x.ComponentId == Id && x.Modules.Count() > 0)
                )
            {
                errors.Add("Found relate module");
            }

            // CP
            if (await context.Locations
                .AnyAsync(x => x.ComponentId == Id && x.ControlPoints.Count() > 0)
                )
            {
                errors.Add("Found relate control point");
            }

            // MP
            if (await context.Locations
                .AnyAsync(x => x.ComponentId == Id && x.MonitorPoints.Count() > 0)
                )
            {
                errors.Add("Found relate control point");
            }

            // ALVL
            if (await context.Locations
                .AnyAsync(x => x.ComponentId == Id && x.AccessLevels.Count() > 0)
                )
            {
                errors.Add("Found relate access level");
            }

            // AREA
            if (await context.Locations
                .AnyAsync(x => x.ComponentId == Id && x.AccessAreas.Count() > 0)
                )
            {
                errors.Add("Found relate access level");
            }

            // Card Holders
            if (await context.Locations
                .AnyAsync(x => x.ComponentId == Id && x.CardHolders.Count() > 0)
                )
            {
                errors.Add("Found relate card holder");
            }

            // Doors 
            if (await context.Locations
                .AnyAsync(x => x.ComponentId == Id && x.Doors.Count() > 0)
                )
            {
                errors.Add("Found relate door");
            }

            // MPG 
            if (await context.Locations
                .AnyAsync(x => x.ComponentId == Id && x.MonitorPointsGroup.Count() > 0)
                )
            {
                errors.Add("Found relate monitor group");
            }

            // Operator 
            if (await context.Locations
                .AnyAsync(x => x.ComponentId == Id && x.OperatorLocations.Count() > 1)
                )
            {
                errors.Add("Found relate operator");
            }

            // Holiday 
            if (await context.Locations
                .AnyAsync(x => x.ComponentId == Id && x.Holidays.Count() > 0)
                )
            {
                errors.Add("Found relate holiday");
            }

            // Cred 
            if (await context.Locations
                .AnyAsync(x => x.ComponentId == Id && x.Credentials.Count() > 0)
                )
            {
                errors.Add("Found relate credential");
            }

            // reader 
            if (await context.Locations
                .AnyAsync(x => x.ComponentId == Id && x.Readers.Count() > 0)
                )
            {
                errors.Add("Found relate reader");
            }

            // rex 
            if (await context.Locations
                .AnyAsync(x => x.ComponentId == Id && x.RequestExits.Count() > 0)
                )
            {
                errors.Add("Found relate rex");
            }

            // strk 
            if (await context.Locations
                .AnyAsync(x => x.ComponentId == Id && x.Strikes.Count() > 0)
                )
            {
                errors.Add("Found relate strike");
            }

            // proc 
            if (await context.Locations
                .AnyAsync(x => x.ComponentId == Id && x.Procedures.Count() > 0)
                )
            {
                errors.Add("Found relate procedure");
            }

            // ac 
            if (await context.Locations
                .AnyAsync(x => x.ComponentId == Id && x.Actions.Count() > 0)
                )
            {
                errors.Add("Found relate procedure");
            }

            // trigger 
            if (await context.Locations
                .AnyAsync(x => x.ComponentId == Id && x.Triggers.Count() > 0)
                )
            {
                errors.Add("Found relate trigger");
            }

            // interval 
            if (await context.Locations
                .AnyAsync(x => x.ComponentId == Id && x.Intervals.Count() > 0)
                )
            {
                errors.Add("Found relate interval");
            }

            // timezone 
            if (await context.Locations
                .AnyAsync(x => x.ComponentId == Id && x.TimeZones.Count() > 0)
                )
            {
                errors.Add("Found relate timezone");
            }

            if (errors.Count > 0) return ResponseHelper.FoundReferenceBuilder<bool>(errors);

            context.Locations.Remove(en);
            await context.SaveChangesAsync();

            return ResponseHelper.SuccessBuilder<bool>(true);
        }

        public async Task<ResponseDto<IEnumerable<ResponseDto<bool>>>> DeleteRangeAsync(List<short> components)
        {
            bool flag = true;
            List<ResponseDto<bool>> data = new List<ResponseDto<bool>>();
            foreach(var id in components)
            {
                var re = await DeleteByIdAsync(id);
                if (re.code != HttpStatusCode.OK) flag = false;
                data.Add(await DeleteByIdAsync(id));
            }

            if (!flag) return ResponseHelper.UnsuccessBuilder<IEnumerable<ResponseDto<bool>>>(data);

            var res = ResponseHelper.SuccessBuilder<IEnumerable<ResponseDto<bool>>>(data);

            return res;
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
            if (dto.ComponentId == 1) return ResponseHelper.DefaultRecord<LocationDto>();

            var en = await context.Locations
                .Where(x => x.ComponentId == dto.ComponentId)
                .FirstOrDefaultAsync();

            if (en is null) return ResponseHelper.NotFoundBuilder<LocationDto>();

            MapperHelper.UpdateLocation(en,dto);

            context.Locations.Update(en);
            await context.SaveChangesAsync();

            return ResponseHelper.SuccessBuilder<LocationDto>(dto);
        }
    }
}
