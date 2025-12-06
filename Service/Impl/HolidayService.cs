using HIDAeroService.AeroLibrary;
using HIDAeroService.Constant;
using HIDAeroService.Constants;
using HIDAeroService.Data;
using HIDAeroService.DTO;
using HIDAeroService.DTO.Holiday;
using HIDAeroService.Entity;
using HIDAeroService.Helpers;
using HIDAeroService.Utility;
using Microsoft.EntityFrameworkCore;
using MiNET.Blocks;
using MiNET.Entities;
using MiNET.Worlds;
using System.Net;

namespace HIDAeroService.Service.Impl
{
    public class HolidayService(AeroCommand command, AppDbContext context, IHelperService<Holiday> helperService) : IHolidayService
    {

        public async Task<ResponseDto<IEnumerable<HolidayDto>>> GetAsync()
        {
            var dtos = await context.Holidays.AsNoTracking()
                .Select(p => new HolidayDto
                {
                    // Base
                    Uuid = p.Uuid,
                    LocationId = p.LocationId,
                    IsActive = p.IsActive,

                    // ExtendDesc
                    ComponentId = p.ComponentId,
                    Day = p.Day,
                    Month = p.Month,
                    Year = p.Year,
                    Extend = p.Extend,
                    TypeMask = p.TypeMask

                }).ToArrayAsync();
            if (dtos.Count() == 0) return ResponseHelper.NotFoundBuilder<IEnumerable<HolidayDto>>();
            return ResponseHelper.SuccessBuilder<IEnumerable<HolidayDto>>(dtos);
        }

        public async Task<ResponseDto<bool>> ClearAsync()
        {
            List<string> errors = new List<string>();
            var macs = await context.Hardwares.Select(x => x.MacAddress).ToArrayAsync();
            foreach (var mac in macs)
            {
                short ScpId = await helperService.GetIdFromMacAsync(mac);
                if (!await command.ClearHolidayConfigurationAsync(ScpId))
                {
                    errors.Add(MessageBuilder.Unsuccess(mac, Command.C1104));
                }
            }
            if (errors.Count > 0) return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.COMMAND_UNSUCCESS,errors);
            var holidays = await context.Holidays.ToArrayAsync();
            context.Holidays.RemoveRange(holidays);
            await context.SaveChangesAsync();
            return ResponseHelper.SuccessBuilder(true);
        }

        public async Task<ResponseDto<HolidayDto>> GetByComponentIdAsync(short component)
        {
            var dto = await context.Holidays.AsNoTracking().Where(p => p.ComponentId == component).Select(p => new HolidayDto 
            {
                // Base
                Uuid = p.Uuid,
                LocationId = p.LocationId,
                IsActive = p.IsActive,

                // ExtendDesc
                Day = p.Day,
                Month = p.Month,
                Year = p.Year,
                Extend = p.Extend,
                TypeMask = p.TypeMask

            }).FirstOrDefaultAsync();
            if (dto == null) return ResponseHelper.NotFoundBuilder<HolidayDto>();
            return ResponseHelper.SuccessBuilder(dto);
        }



        public async Task<ResponseDto<bool>> CreateAsync(CreateHolidayDto dto)
        {
            List<string> errors = new List<string>();

            if (await context.Holidays.AnyAsync(u => u.Day == dto.Day && u.Month == dto.Month && u.Year == dto.Year)) return ResponseHelper.Duplicate<bool>();

            var ComponentId = await helperService.GetLowestUnassignedNumberNoLimitAsync<Holiday>(context);
            if (ComponentId == -1) return ResponseHelper.ExceedLimit<bool>();

            // Send command 
            List<short> ids = await context.Hardwares.AsNoTracking().Select(p => p.ComponentId).ToListAsync();

            var holiday = new Holiday
            {
                // Base
                LocationId = dto.LocationId,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                IsActive = true,


                // ExtendDesc
                ComponentId = ComponentId,
                Day = dto.Day,
                Month = dto.Month,
                Year = dto.Year,
                Extend = 0,
                TypeMask = dto.TypeMask,

            };

            foreach (var id in ids)
            {
                string mac = await helperService.GetMacFromIdAsync(id);
                if (!await command.HolidayConfigurationAsync(holiday, id))
                {
                    errors.Add(MessageBuilder.Unsuccess(mac, Command.C1104));

                }
            }

            if (errors.Count > 0) return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.COMMAND_UNSUCCESS, errors);


            await context.Holidays.AddAsync(holiday);
            await context.SaveChangesAsync();
            return ResponseHelper.SuccessBuilder(true);
        }

        public async Task<ResponseDto<bool>> DeleteAsync(short component)
        {
            List<string> errors = new List<string>();
            var entity = await context.Holidays.FirstOrDefaultAsync(x => x.ComponentId == component);
            if (entity is null) return ResponseHelper.NotFoundBuilder<bool>();
            // Send command 
            List<short> ids = await context.Hardwares.Select(p => p.ComponentId).ToListAsync();


            foreach (var id in ids)
            {
                string mac = await helperService.GetMacFromIdAsync(id);
                if (!await command.DeleteHolidayConfigurationAsync(entity, id))
                {
                    errors.Add(MessageBuilder.Unsuccess(mac, Command.C1104));
                }
            }

            if (errors.Count > 0) return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.COMMAND_UNSUCCESS,errors);
            context.Holidays.Remove(entity);
            await context.SaveChangesAsync();
            return ResponseHelper.SuccessBuilder(true);
        }

        public async Task<ResponseDto<HolidayDto>> UpdateAsync(HolidayDto dto)
        {
            List<string> errors = new List<string>();
            var entity = await context.Holidays.FirstOrDefaultAsync(p => p.ComponentId == dto.ComponentId);
            if (entity is null) return ResponseHelper.NotFoundBuilder<HolidayDto>();

            if (await context.Holidays.AnyAsync(u => u.Day == dto.Day && u.Month == dto.Month && u.Year == dto.Year)) return ResponseHelper.Duplicate<HolidayDto>();

            // Send command 
            List<short> ids = await context.Hardwares.Select(p => p.ComponentId).ToListAsync();

            entity.Day = dto.Day;
            entity.Month = dto.Month;
            entity.Year = dto.Year;
            entity.TypeMask = dto.TypeMask;
            entity.Extend = 0;
            entity.UpdatedDate = DateTime.Now;

            foreach (var id in ids)
            {
                string mac = await helperService.GetMacFromIdAsync(id);
                if (!await command.HolidayConfigurationAsync(entity, id))
                {
                    errors.Add(MessageBuilder.Unsuccess(mac, Command.C1104));
                }
            }

            if (errors.Count > 0) return ResponseHelper.UnsuccessBuilder<HolidayDto>(ResponseMessage.COMMAND_UNSUCCESS,errors);

            context.Holidays.Update(entity);
            await context.SaveChangesAsync();
            return ResponseHelper.SuccessBuilder(dto);
        }

    }
}
