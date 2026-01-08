using HIDAeroService.Aero.CommandService;
using HIDAeroService.AeroLibrary;
using HIDAeroService.Constant;
using HIDAeroService.Constants;
using HIDAeroService.Data;
using HIDAeroService.DTO;
using HIDAeroService.DTO.Holiday;
using HIDAeroService.Entity;
using HIDAeroService.Helpers;
using HIDAeroService.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiNET.Blocks;
using MiNET.Entities;
using MiNET.Worlds;
using System.Net;

namespace HIDAeroService.Service.Impl
{
    public class HolidayService(IHolidayCommandService command, AppDbContext context, IHelperService<Holiday> helperService) : IHolidayService
    {

        public async Task<ResponseDto<IEnumerable<HolidayDto>>> GetAsync()
        {
            var dtos = await context.holiday.AsNoTracking()
                .Select(p => new HolidayDto
                {
                    // Base
                    Uuid = p.uuid,
                    LocationId = p.location_id,
                    IsActive = p.is_active,

                    // extend_desc
                    component_id = p.component_id,
                    Day = p.day,
                    Month = p.month,
                    Year = p.year,
                    Extend = p.extend,
                    TypeMask = p.type_mask

                }).ToArrayAsync();
            if (dtos.Count() == 0) return ResponseHelper.NotFoundBuilder<IEnumerable<HolidayDto>>();
            return ResponseHelper.SuccessBuilder<IEnumerable<HolidayDto>>(dtos);
        }

        public async Task<ResponseDto<bool>> ClearAsync()
        {
            List<string> errors = new List<string>();
            var macs = await context.hardware.Select(x => x.mac).ToArrayAsync();
            foreach (var mac in macs)
            {
                short ScpId = await helperService.GetIdFromMacAsync(mac);
                //if (!await command.ClearHolidayConfigurationAsync(hardware_id))
                //{
                //    errors.Add(MessageBuilder.Unsuccess(mac, command.C1104));
                //}
            }
            if (errors.Count > 0) return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.COMMAND_UNSUCCESS,errors);
            var holidays = await context.holiday.ToArrayAsync();
            context.holiday.RemoveRange(holidays);
            await context.SaveChangesAsync();
            return ResponseHelper.SuccessBuilder(true);
        }

        public async Task<ResponseDto<HolidayDto>> GetByComponentIdAsync(short component)
        {
            var dto = await context.holiday.AsNoTracking().Where(p => p.component_id == component).Select(p => new HolidayDto 
            {
                // Base
                Uuid = p.uuid,
                LocationId = p.location_id,
                IsActive = p.is_active,

                // extend_desc
                Day = p.day,
                Month = p.month,
                Year = p.year,
                Extend = p.extend,
                TypeMask = p.type_mask

            }).FirstOrDefaultAsync();
            if (dto == null) return ResponseHelper.NotFoundBuilder<HolidayDto>();
            return ResponseHelper.SuccessBuilder(dto);
        }



        public async Task<ResponseDto<bool>> CreateAsync(CreateHolidayDto dto)
        {
            List<string> errors = new List<string>();

            if (await context.holiday.AnyAsync(u => u.day == dto.Day && u.month == dto.Month && u.year == dto.Year)) return ResponseHelper.Duplicate<bool>();

            var ComponentId = await helperService.GetLowestUnassignedNumberNoLimitAsync<Holiday>(context);
            if (ComponentId == -1) return ResponseHelper.ExceedLimit<bool>();

            // Send command 
            List<short> ids = await context.hardware.AsNoTracking().Select(p => p.component_id).ToListAsync();

            var holiday = new Holiday
            {
                // Base
                location_id = dto.LocationId,
                created_date = DateTime.Now,
                updated_date = DateTime.Now,
                is_active = true,


                // extend_desc
                component_id = ComponentId,
                day = dto.Day,
                month = dto.Month,
                year = dto.Year,
                extend = 0,
                type_mask = dto.TypeMask,

            };

            foreach (var id in ids)
            {
                string mac = await helperService.GetMacFromIdAsync(id);
                //if (!await command.HolidayConfigurationAsync(holiday, id))
                //{
                //    errors.Add(MessageBuilder.Unsuccess(mac, command.C1104));

                //}
            }

            if (errors.Count > 0) return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.COMMAND_UNSUCCESS, errors);


            await context.holiday.AddAsync(holiday);
            await context.SaveChangesAsync();
            return ResponseHelper.SuccessBuilder(true);
        }

        public async Task<ResponseDto<bool>> DeleteAsync(short component)
        {
            List<string> errors = new List<string>();
            var entity = await context.holiday.FirstOrDefaultAsync(x => x.component_id == component);
            if (entity is null) return ResponseHelper.NotFoundBuilder<bool>();
            // Send command 
            List<short> ids = await context.hardware.Select(p => p.component_id).ToListAsync();


            foreach (var id in ids)
            {
                string mac = await helperService.GetMacFromIdAsync(id);
                //if (!await command.DeleteHolidayConfigurationAsync(entity, id))
                //{
                //    errors.Add(MessageBuilder.Unsuccess(mac, command.C1104));
                //}
            }

            if (errors.Count > 0) return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.COMMAND_UNSUCCESS,errors);
            context.holiday.Remove(entity);
            await context.SaveChangesAsync();
            return ResponseHelper.SuccessBuilder(true);
        }

        public async Task<ResponseDto<HolidayDto>> UpdateAsync(HolidayDto dto)
        {
            List<string> errors = new List<string>();
            var entity = await context.holiday.FirstOrDefaultAsync(p => p.component_id == dto.component_id);
            if (entity is null) return ResponseHelper.NotFoundBuilder<HolidayDto>();

            if (await context.holiday.AnyAsync(u => u.day == dto.Day && u.month == dto.Month && u.year == dto.Year)) return ResponseHelper.Duplicate<HolidayDto>();

            // Send command 
            List<short> ids = await context.hardware.Select(p => p.component_id).ToListAsync();

            entity.day = dto.Day;
            entity.month = dto.Month;
            entity.year = dto.Year;
            entity.type_mask = dto.TypeMask;
            entity.extend = 0;
            entity.updated_date = DateTime.Now;

            foreach (var id in ids)
            {
                string mac = await helperService.GetMacFromIdAsync(id);
                //if (!await command.HolidayConfigurationAsync(entity, id))
                //{
                //    errors.Add(MessageBuilder.Unsuccess(mac, command.C1104));
                //}
            }

            if (errors.Count > 0) return ResponseHelper.UnsuccessBuilder<HolidayDto>(ResponseMessage.COMMAND_UNSUCCESS,errors);

            context.holiday.Update(entity);
            await context.SaveChangesAsync();
            return ResponseHelper.SuccessBuilder(dto);
        }

        public async Task<ResponseDto<IEnumerable<ResponseDto<bool>>>> DeleteRangeAsync(List<short> components)
        {
            bool flag = true;
            List<ResponseDto<bool>> data = new List<ResponseDto<bool>>();
            foreach (var dto in components)
            {
                var re = await DeleteAsync(dto);
                if (re.code != HttpStatusCode.OK) flag = false;
                data.Add(re);
            }

            if (!flag) return ResponseHelper.UnsuccessBuilder<IEnumerable<ResponseDto<bool>>>(data);

            var res = ResponseHelper.SuccessBuilder<IEnumerable<ResponseDto<bool>>>(data);

            return res;
        }

        public async Task<ResponseDto<IEnumerable<HolidayDto>>> GetByLocationAsync(short location)
        {
            var dtos = await context.holiday
                .AsNoTracking()
                .Where(x => x.location_id == location)
                    .Select(p => new HolidayDto
                    {
                        // Base
                        Uuid = p.uuid,
                        LocationId = p.location_id,
                        IsActive = p.is_active,

                        // extend_desc
                        component_id = p.component_id,
                        Day = p.day,
                        Month = p.month,
                        Year = p.year,
                        Extend = p.extend,
                        TypeMask = p.type_mask

                    }).ToArrayAsync();
            if (dtos.Count() == 0) return ResponseHelper.NotFoundBuilder<IEnumerable<HolidayDto>>();
            return ResponseHelper.SuccessBuilder<IEnumerable<HolidayDto>>(dtos);
        }
    }
}
