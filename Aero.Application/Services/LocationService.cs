using System.Net;
using Aero.Application.DTOs;
using Aero.Application.Helpers;
using Aero.Application.Interface;

namespace Aero.Application.Services
{
    public sealed class LocationService() : ILocationService
    {
        public async Task<ResponseDto<bool>> CreateAsync(LocationDto dto)
        {
            if (await context.location.AnyAsync(x => dto.LocationName == x.location_name)) return ResponseHelper.Duplicate<bool>();
            var LocationId = await helperService.GetLowestUnassignedNumberNoLimitAsync<Entity.Location>(context);
            var entity = MapperHelper.DtoToLocation(dto,LocationId,DateTime.UtcNow);
            await context.location.AddAsync(entity);
            await context.SaveChangesAsync();
            return ResponseHelper.SuccessBuilder < bool > (true);

        }

        public async Task<ResponseDto<bool>> DeleteByIdAsync(short Id)
        {
            if (Id == 1) return ResponseHelper.DefaultRecord<bool>();

            var errors = new List<string>();
            var en = await context.location
                .Where(l => l.component_id == Id)
                .OrderBy(l => l.component_id)
                .FirstOrDefaultAsync();

            if (en is null) return ResponseHelper.NotFoundBuilder<bool>();

            // hardware
            if(await context.location
                .AnyAsync(x => x.component_id == Id && x.hardwares.Count() > 0)
                )
            {
                errors.Add("Found relate hardware");
            }

            // modules
            if (await context.location
                .AnyAsync(x => x.component_id == Id && x.modules.Count() > 0)
                )
            {
                errors.Add("Found relate modules");
            }

            // CP
            if (await context.location
                .AnyAsync(x => x.component_id == Id && x.control_points.Count() > 0)
                )
            {
                errors.Add("Found relate control point");
            }

            // MP
            if (await context.location
                .AnyAsync(x => x.component_id == Id && x.monitor_points.Count() > 0)
                )
            {
                errors.Add("Found relate control point");
            }

            // ALVL
            if (await context.location
                .AnyAsync(x => x.component_id == Id && x.accesslevels.Count() > 0)
                )
            {
                errors.Add("Found relate access level");
            }

            // AREA
            if (await context.location
                .AnyAsync(x => x.component_id == Id && x.areas.Count() > 0)
                )
            {
                errors.Add("Found relate access level");
            }

            // Card Holders
            if (await context.location
                .AnyAsync(x => x.component_id == Id && x.cardholders.Count() > 0)
                )
            {
                errors.Add("Found relate card holder");
            }

            // door 
            if (await context.location
                .AnyAsync(x => x.component_id == Id && x.doors.Count() > 0)
                )
            {
                errors.Add("Found relate door");
            }

            // MPG 
            if (await context.location
                .AnyAsync(x => x.component_id == Id && x.monitor_groups.Count() > 0)
                )
            {
                errors.Add("Found relate monitor group");
            }

            // operator 
            if (await context.location
                .AnyAsync(x => x.component_id == Id && x.operator_locations.Count() > 1)
                )
            {
                errors.Add("Found relate operator");
            }

            // Holiday 
            if (await context.location
                .AnyAsync(x => x.component_id == Id && x.holidays.Count() > 0)
                )
            {
                errors.Add("Found relate holiday");
            }

            // Cred 
            if (await context.location
                .AnyAsync(x => x.component_id == Id && x.credentials.Count() > 0)
                )
            {
                errors.Add("Found relate credential");
            }

            // reader 
            if (await context.location
                .AnyAsync(x => x.component_id == Id && x.readers.Count() > 0)
                )
            {
                errors.Add("Found relate reader");
            }

            // rex 
            if (await context.location
                .AnyAsync(x => x.component_id == Id && x.request_exits.Count() > 0)
                )
            {
                errors.Add("Found relate rex");
            }

            // strk 
            if (await context.location
                .AnyAsync(x => x.component_id == Id && x.strikes.Count() > 0)
                )
            {
                errors.Add("Found relate strike");
            }

            // proc 
            if (await context.location
                .AnyAsync(x => x.component_id == Id && x.procedures.Count() > 0)
                )
            {
                errors.Add("Found relate procedure");
            }

            // ac 
            if (await context.location
                .AnyAsync(x => x.component_id == Id && x.actions.Count() > 0)
                )
            {
                errors.Add("Found relate procedure");
            }

            // trigger 
            if (await context.location
                .AnyAsync(x => x.component_id == Id && x.triggers.Count() > 0)
                )
            {
                errors.Add("Found relate trigger");
            }

            // interval 
            if (await context.location
                .AnyAsync(x => x.component_id == Id && x.intervals.Count() > 0)
                )
            {
                errors.Add("Found relate interval");
            }

            // timezone 
            if (await context.location
                .AnyAsync(x => x.component_id == Id && x.timezones.Count() > 0)
                )
            {
                errors.Add("Found relate timezone");
            }

            if (errors.Count > 0) return ResponseHelper.FoundReferenceBuilder<bool>(errors);

            context.location.Remove(en);
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
                data.Add(re);
            }

            if (!flag) return ResponseHelper.UnsuccessBuilder<IEnumerable<ResponseDto<bool>>>(data);

            var res = ResponseHelper.SuccessBuilder<IEnumerable<ResponseDto<bool>>>(data);

            return res;
        }

        public async Task<ResponseDto<IEnumerable<LocationDto>>> GetAsync()
        {
            var dto = await context.location
                .Select(x => new LocationDto
                {
                    Uuid = x.uuid,
                    ComponentId = x.component_id,
                    LocationName = x.location_name,
                    Description = x.description,
                })
                .ToArrayAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<LocationDto>>(dto);
        }

        public async Task<ResponseDto<LocationDto>> GetByIdAsync(short id)
        {
            var dto = await context.location
                .Where(x => x.component_id == id)
                 .Select(x => new LocationDto
                 {
                     Uuid = x.uuid,
                     ComponentId = x.component_id,
                     LocationName = x.location_name,
                     Description = x.description,
                 })
                .FirstOrDefaultAsync();

            if(dto is null) return ResponseHelper.NotFoundBuilder<LocationDto>();

            return ResponseHelper.SuccessBuilder(dto);
        }

        public async Task<ResponseDto<IEnumerable<LocationDto>>> GetRangeLocationById(LocationRangeDto locationIds)
        {
            var dtos = await context.location
                .AsNoTracking()
                .Where(x => locationIds.locationIds.Contains(x.component_id))
                .Select(x => new LocationDto
                {
                    Uuid = x.uuid,
                    ComponentId = x.component_id,
                    LocationName = x.location_name,
                    Description = x.description,
                })
                .ToArrayAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<LocationDto>>(dtos);
        }

        public async Task<ResponseDto<LocationDto>> UpdateAsync(LocationDto dto)
        {
            if (dto.ComponentId == 1) return ResponseHelper.DefaultRecord<LocationDto>();

            var en = await context.location
                .Where(x => x.component_id == dto.ComponentId)
                .FirstOrDefaultAsync();

            if (en is null) return ResponseHelper.NotFoundBuilder<LocationDto>();

            MapperHelper.UpdateLocation(en,dto);

            context.location.Update(en);
            await context.SaveChangesAsync();

            return ResponseHelper.SuccessBuilder<LocationDto>(dto);
        }
    }
}
