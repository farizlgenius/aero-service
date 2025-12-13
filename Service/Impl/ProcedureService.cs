using HIDAeroService.AeroLibrary;
using HIDAeroService.Constant;
using HIDAeroService.Constants;
using HIDAeroService.Data;
using HIDAeroService.DTO;
using HIDAeroService.DTO.Action;
using HIDAeroService.DTO.Procedure;
using HIDAeroService.Entity;
using HIDAeroService.Helpers;
using HIDAeroService.Mapper;
using HIDAeroService.Utility;
using Microsoft.EntityFrameworkCore;

namespace HIDAeroService.Service.Impl
{
    public sealed class ProcedureService(AppDbContext context,AeroCommand command,IHelperService<Procedure> helperService) : IProcedureService
    {
        public async Task<ResponseDto<bool>> CreateAsync(ProcedureDto dto)
        {
            var ComponentId = await helperService.GetLowestUnassignedNumberAsync<Procedure>(context,128);

            if(!await command.ActionSpecification(ComponentId,dto.Actions))
            {
                return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess("", Command.C118));
            }

            var en = MapperHelper.DtoToProcedure(dto,ComponentId,DateTime.Now);

            await context.Procedures.AddAsync(en);
            await context.SaveChangesAsync();

            return ResponseHelper.SuccessBuilder<bool>(true);
            
        }

        public async Task<ResponseDto<bool>> DeleteAsync(string Mac,short ComponentId)
        {
            var en = await context
                .Procedures
                .AsNoTracking()
                .Where(x => x.MacAddress == Mac && x.ComponentId == ComponentId)
                .FirstOrDefaultAsync();

            if (en is null) return ResponseHelper.NotFoundBuilder<bool>();

            var ac = new ActionDto
            {
                ActionType = 0,
                
            };

            if(!await command.ActionSpecification(ComponentId, [ac]))
            {
                return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess("", Command.C118));
            }

            context.Procedures.Remove(en);
            await context.SaveChangesAsync();

            return ResponseHelper.SuccessBuilder<bool>(true);
        }

        public async Task<ResponseDto<IEnumerable<ModeDto>>> GetActionType()
        {
            var dtos = await context.ActionTypes
                .AsNoTracking()
                .Select(x => new ModeDto 
                {
                    Name = x.Name,
                    Value = x.Value,
                    Description = x.Description,
                }).ToArrayAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dtos);
        }

        public async Task<ResponseDto<IEnumerable<ProcedureDto>>> GetAsync()
        {
            var dtos = await context.Procedures
                .AsNoTracking()
                .Include(x => x.Actions)
                .Select(x => MapperHelper.ProcedureToDto(x))
                .ToArrayAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<ProcedureDto>>(dtos);
        }

        public async Task<ResponseDto<IEnumerable<ProcedureDto>>> GetByLocationIdAsync(short location)
        {
            var dtos = await context.Procedures
                .AsNoTracking()
                .Include(x => x.Actions)
                .Where(x => x.LocationId == location)
                .Select(x => MapperHelper.ProcedureToDto(x))
                .ToArrayAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<ProcedureDto>>(dtos);
        }

        public Task<ResponseDto<ProcedureDto>> UpdateAsync(ProcedureDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
