using HIDAeroService.AeroLibrary;
using HIDAeroService.Constant;
using HIDAeroService.Constants;
using HIDAeroService.Data;
using HIDAeroService.DTO;
using HIDAeroService.DTO.Trigger;
using HIDAeroService.Entity;
using HIDAeroService.Helpers;
using HIDAeroService.Mapper;
using HIDAeroService.Utility;
using Microsoft.EntityFrameworkCore;

namespace HIDAeroService.Service.Impl
{
    public sealed class TriggerService(AppDbContext context,IHelperService<Trigger> helperService,AeroCommand command) : ITriggerService
    {
        public async Task<ResponseDto<bool>> CreateAsync(TriggerDto dto)
        {
            var ComponentId = await helperService.GetLowestUnassignedNumberAsync<Trigger>(context, 128);
            if (ComponentId == -1) return ResponseHelper.ExceedLimit<bool>();
            var ScpId = await helperService.GetIdFromMacAsync(dto.MacAddress);
            if (ScpId == 0) return ResponseHelper.NotFoundBuilder<bool>();

            var en = MapperHelper.DtoToTrigger(dto, ComponentId, DateTime.Now);

            if(!await command.TriggerSpecification(ScpId,en,ComponentId))
            {
                return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(dto.MacAddress, Command.C117));
            }

            await context.Triggers.AddAsync(en);
            await context.SaveChangesAsync();
            return ResponseHelper.SuccessBuilder<bool>(true);
        }

        public Task<ResponseDto<bool>> DeleteAsync(string Mac, short ComponentId)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseDto<IEnumerable<TriggerDto>>> GetAsync()
        {
            var dto = await context.Triggers
                .AsNoTracking()
                .Include(x => x.Procedure)
                .ThenInclude(x => x.Actions)
                .Select(x => MapperHelper.TriggerToDto(x))
                .ToArrayAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<TriggerDto>>(dto);
        }

        public async Task<ResponseDto<IEnumerable<TriggerDto>>> GetByLocationId(short location)
        {
            var dto = await context.Triggers
                .AsNoTracking()
                .Include(x => x.Procedure)
                .ThenInclude(x => x.Actions)
                .Where(x => x.LocationId == location)
                .Select(x => MapperHelper.TriggerToDto(x))
                .ToArrayAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<TriggerDto>>(dto);
        }

        public Task<ResponseDto<TriggerDto>> UpdateAsync(TriggerDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
