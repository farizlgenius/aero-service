using AeroService.Aero.CommandService;
using AeroService.Aero.CommandService.Impl;
using AeroService.Constant;
using AeroService.Constants;
using AeroService.Data;
using AeroService.DTO;
using AeroService.DTO.Action;
using AeroService.DTO.Procedure;
using AeroService.Entity;
using AeroService.Helpers;
using AeroService.Mapper;
using AeroService.Utility;
using Microsoft.EntityFrameworkCore;

namespace AeroService.Service.Impl
{
    public sealed class ProcedureService(AppDbContext context,AeroCommandService command,IHelperService<Procedure> helperService) : IProcedureService
    {
        public async Task<ResponseDto<bool>> CreateAsync(ProcedureDto dto)
        {
            var ComponentId = await helperService.GetLowestUnassignedNumberAsync<Procedure>(context,128);

            foreach(var ac in dto.Actions)
            {
                if(ac.ActionType == 9)
                {
                    ac.ScpId = 0;
                }
                else
                {
                    ac.ScpId = await helperService.GetIdFromMacAsync(ac.Mac);
                }
               
            }

            var en = MapperHelper.DtoToProcedure(dto, ComponentId, DateTime.UtcNow);


            var ids = await context.hardware.AsNoTracking().Select(x => x.component_id).ToListAsync();
            foreach(var ac in en.actions)
            {
                if (ac.action_type == 9)
                {
                    if (!command.ActionSpecificationAsyncForAllHW(ComponentId, ac, ids))
                    {
                        return ResponseHelper.UnsuccessBuilderWithString<bool>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess("", Command.C118));
                    }
                }
                else if (ac.delay_time != 0) 
                {
                    if(!command.ActionSpecificationDelayAsync(ComponentId, ac))
                    {
                        return ResponseHelper.UnsuccessBuilderWithString<bool>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess("", Command.C118));
                    }
                }
            }

            if (!command.ActionSpecificationAsync(ComponentId, en.actions.ToList()))
            {
                return ResponseHelper.UnsuccessBuilderWithString<bool>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess("", Command.C118));
            }


            await context.procedure.AddAsync(en);
            await context.SaveChangesAsync();

            return ResponseHelper.SuccessBuilder<bool>(true);
            
        }

        public async Task<ResponseDto<bool>> DeleteAsync(string Mac,short ComponentId)
        {
            var en = await context.procedure
                .AsNoTracking()
                .Where(x => x.trigger.hardware_mac == Mac && x.component_id == ComponentId)
                .FirstOrDefaultAsync();

            if (en is null) return ResponseHelper.NotFoundBuilder<bool>();

            var ac = new Entity.Action
            {
                action_type = 0,
            };

            if(!command.ActionSpecificationAsync(ComponentId, [ac]))
            {
                return ResponseHelper.UnsuccessBuilderWithString<bool>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess("", Command.C118));
            }

            context.procedure.Remove(en);
            await context.SaveChangesAsync();

            return ResponseHelper.SuccessBuilder<bool>(true);
        }

        public async Task<ResponseDto<IEnumerable<ModeDto>>> GetActionType()
        {
            var dtos = await context.action_type
                .AsNoTracking()
                .Select(x => new ModeDto 
                {
                    Name = x.name,
                    Value = x.value,
                    Description = x.description,
                }).ToArrayAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dtos);
        }

        public async Task<ResponseDto<IEnumerable<ProcedureDto>>> GetAsync()
        {
            var dtos = await context.procedure
                .AsNoTracking()
                .Include(x => x.actions)
                .Select(x => new ProcedureDto
                {
                    // Base
                    Uuid = x.uuid,
                    ComponentId = x.component_id,
                    Mac = x.trigger.hardware_mac,
                    HardwareName = x.trigger.hardware.name,
                    LocationId = x.location_id,
                    IsActive = x.is_active,

                    // Detail
                    Name = x.name,
                    Actions = x.actions
                .Select(en => new ActionDto
                {
                    // Base
                    Uuid = en.uuid,
                    ComponentId = en.component_id,
                    Mac = x.trigger.hardware_mac,
                    LocationId = en.location_id,
                    IsActive = en.is_active,

                    // Detail
                    ScpId = en.hardware_id,
                    ActionType = en.action_type,
                    ActionTypeDesc = en.action_type_desc,
                    Arg1 = en.arg1,
                    Arg2 = en.arg2,
                    Arg3 = en.arg3,
                    Arg4 = en.arg4,
                    Arg5 = en.arg5,
                    Arg6 = en.arg6,
                    Arg7 = en.arg7,
                    StrArg = en.str_arg,
                    DelayTime = en.delay_time,
                })
                .ToList()


                })
                .ToArrayAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<ProcedureDto>>(dtos);
        }

        public async Task<ResponseDto<IEnumerable<ProcedureDto>>> GetByLocationIdAsync(short location)
        {
            var dtos = await context.procedure
                .AsNoTracking()
                .Include(x => x.actions)
                .Where(x => x.location_id == location)
                .Select(x => new ProcedureDto
                {
                    // Base
                    Uuid = x.uuid,
                    ComponentId = x.component_id,
                    Mac = x.trigger.hardware_mac,
                    HardwareName = x.trigger.hardware.name,
                    LocationId = x.location_id,
                    IsActive = x.is_active,

                    // Detail
                    Name = x.name,
                    Actions = x.actions
                .Select(en => new ActionDto
                {
                    // Base
                    Uuid = en.uuid,
                    ComponentId = en.component_id,
                    Mac = x.trigger.hardware_mac,
                    LocationId = en.location_id,
                    IsActive = en.is_active,

                    // Detail
                    ScpId = en.hardware_id,
                    ActionType = en.action_type,
                    ActionTypeDesc = en.action_type_desc,
                    Arg1 = en.arg1,
                    Arg2 = en.arg2,
                    Arg3 = en.arg3,
                    Arg4 = en.arg4,
                    Arg5 = en.arg5,
                    Arg6 = en.arg6,
                    Arg7 = en.arg7,
                    StrArg = en.str_arg,
                    DelayTime = en.delay_time,
                })
                .ToList()


                })
                .ToArrayAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<ProcedureDto>>(dtos);
        }

        public Task<ResponseDto<ProcedureDto>> UpdateAsync(ProcedureDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
