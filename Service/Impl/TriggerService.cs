using HID.Aero.ScpdNet.Wrapper;
using AeroService.Aero.CommandService;
using AeroService.Aero.CommandService.Impl;
using AeroService.Constant;
using AeroService.Constants;
using AeroService.Data;
using AeroService.DTO;
using AeroService.DTO.Trigger;
using AeroService.Entity;
using AeroService.Helpers;
using AeroService.Mapper;
using AeroService.Utility;
using Microsoft.EntityFrameworkCore;

namespace AeroService.Service.Impl
{
    public sealed class TriggerService(AppDbContext context,IHelperService<Trigger> helperService,AeroCommandService command) : ITriggerService
    {
        public async Task<ResponseDto<bool>> CreateAsync(TriggerDto dto)
        {
            var ComponentId = await helperService.GetLowestUnassignedNumberAsync<Trigger>(context, 128);
            if (ComponentId == -1) return ResponseHelper.ExceedLimit<bool>();
            var ScpId = await helperService.GetIdFromMacAsync(dto.Mac);
            if (ScpId == 0) return ResponseHelper.NotFoundBuilder<bool>();

            var en = MapperHelper.DtoToTrigger(dto, ComponentId, DateTime.UtcNow);

            if(!command.TriggerSpecification(ScpId,en,ComponentId))
            {
                return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(dto.Mac, Command.C117));
            }

            await context.trigger.AddAsync(en);
            await context.SaveChangesAsync();
            return ResponseHelper.SuccessBuilder<bool>(true);
        }

        public Task<ResponseDto<bool>> DeleteAsync(string Mac, short ComponentId)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseDto<IEnumerable<TriggerDto>>> GetAsync()
        {
            var dto = await context.trigger
                .AsNoTracking()
                .Include(x => x.procedure)
                .ThenInclude(x => x.actions)
                .Include(x => x.code_map)
                .Select(x => MapperHelper.TriggerToDto(x))
                .ToArrayAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<TriggerDto>>(dto);
        }

        public async Task<ResponseDto<IEnumerable<TriggerDto>>> GetByLocationId(short location)
        {
            var dto = await context.trigger
                .AsNoTracking()
                .Include(x => x.procedure)
                .ThenInclude(x => x.actions)
                 .Include(x => x.code_map)
                .Where(x => x.location_id == location)
                .Select(x => MapperHelper.TriggerToDto(x))
                .ToArrayAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<TriggerDto>>(dto);
        }


        public async Task<ResponseDto<IEnumerable<ModeDto>>> GetCommandAsync()
        {
            var dtos = await context.trigger_command
                .AsNoTracking()
                .Select(x => new ModeDto 
                {
                    Name = x.name,
                    Description = x.description,
                    Value = x.value,
                })
                .ToArrayAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dtos);
        }

        public async Task<ResponseDto<IEnumerable<ModeDto>>> GetSourceTypeAsync()
        {
            var dtos = await context.transaction_source
                .AsNoTracking()
                .Select(x => new ModeDto
                {
                    Name = x.name,
                    Description = x.source,
                    Value = x.value,
                })
                .ToArrayAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dtos);
        }

        public async Task<ResponseDto<IEnumerable<ModeDto>>> GetCodeByTranAsync(short tran)
        {
            var dtos = await context.transaction_code
                .AsNoTracking()
                .Where(x => x.transaction_type_value == tran)
                .Select(x => new ModeDto
                {
                    Name = x.name,
                    Description = x.description,
                    Value = x.value,
                })
                .ToArrayAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dtos);
        }

        public async Task<ResponseDto<IEnumerable<ModeDto>>> GetTypeBySourceAsync(short source)
        {
            var dtos = await context.transaction_type
                .AsNoTracking()
                .Where(x => 
                    x.transaction_source_types.All(x => x.transction_source_value == source) &&
                    x.transaction_source_types.Any()
                )
                .Select(x => new ModeDto
                {
                    Name = x.name,
                    Description = "",
                    Value = x.value,
                })
                .ToArrayAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dtos);
        }

        public Task<ResponseDto<TriggerDto>> UpdateAsync(TriggerDto dto)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseDto<IEnumerable<ModeDto>>> GetDeviceBySourceAsync(short location,short source)
        {
            switch (source)
            {
                case (short)tranSrc.tranSrcScpDiag:
                case (short)tranSrc.tranSrcScpCom:
                case (short)tranSrc.tranSrcScpLcl:
                case (short)tranSrc.tranSrcLoginService:
                    var dtos = await context.hardware
                        .AsNoTracking()
                        .Where(x => x.location_id == location)
                        .Select(x => new ModeDto
                        {
                            Name = x.name,
                            Value = x.component_id,
                            Description = x.mac
                        })
                        .ToArrayAsync();
                    return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dtos);
                case (short)tranSrc.tranSrcSioDiag:
                case (short)tranSrc.tranSrcSioCom:
                case (short)tranSrc.tranSrcSioTmpr:
                case (short)tranSrc.tranSrcSioPwr:
                    dtos = await context.module
                        .AsNoTracking()
                        .Where(x => x.location_id == location)
                        .Select(x => new ModeDto
                        {
                            Name = x.model_desc,
                            Value = x.component_id,
                            Description = x.hardware_mac
                        })
                        .ToArrayAsync();
                    return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dtos);
                case (short)tranSrc.tranSrcMP:
                    dtos = await context.monitor_point
                        .AsNoTracking()
                        .Where(x => x.location_id == location)
                        .Select(x => new ModeDto
                        {
                            Name = x.name,
                            Value = x.component_id,
                            Description = x.module.hardware_mac
                        })
                        .ToArrayAsync();
                    return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dtos);
                case (short)tranSrc.tranSrcCP:
                    dtos = await context.control_point
                        .AsNoTracking()
                        .Where(x => x.location_id == location)
                        .Select(x => new ModeDto
                        {
                            Name = x.name,
                            Value = x.component_id,
                            Description = x.module.hardware_mac
                        })
                        .ToArrayAsync();
                    return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dtos);
                case (short)tranSrc.tranSrcACR:
                case (short)tranSrc.tranSrcAcrTmpr:
                case (short)tranSrc.tranSrcAcrDoor:
                case (short)tranSrc.tranSrcAcrRex0:
                case (short)tranSrc.tranSrcAcrRex1:
                case (short)tranSrc.tranSrcAcrTmprAlt:
                    dtos = await context.door
                        .AsNoTracking()
                        .Where(x => x.location_id == location)
                        .Select(x => new ModeDto 
                        {
                            Name = x.name,
                            Value = x.component_id,
                            Description = x.hardware_mac
                        })
                        .ToArrayAsync();
                    return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dtos);
                case (short)tranSrc.tranSrcTimeZone:
                    dtos = await context.timezone
                        .AsNoTracking()
                        .Where(x => x.location_id == location)
                        .Select(x => new ModeDto
                        {
                            Name = x.name,
                            Value = x.component_id,
                            Description = ""
                        })
                        .ToArrayAsync();
                    return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dtos);
                case (short)tranSrc.tranSrcProcedure:
                    dtos = await context.procedure
                        .AsNoTracking()
                        .Where(x => x.location_id == location)
                        .Select(x => new ModeDto
                        {
                            Name = x.name,
                            Value = x.component_id,
                            Description = x.trigger.hardware_mac
                        })
                        .ToArrayAsync();
                    return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dtos);
                case (short)tranSrc.tranSrcTrigger:
                case (short)tranSrc.tranSrcTrigVar:
                    dtos = await context.trigger
                        .AsNoTracking()
                        .Where(x => x.location_id == location)
                        .Select(x => new ModeDto
                        {
                            Name = x.name,
                            Value = x.component_id,
                            Description = x.hardware_mac
                        })
                        .ToArrayAsync();
                    return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dtos);
                case (short)tranSrc.tranSrcMPG:
                    dtos = await context.monitor_group
                        .AsNoTracking()
                        .Where(x => x.location_id == location)
                        .Select(x => new ModeDto
                        {
                            Name = x.name,
                            Value = x.component_id,
                            Description = x.hardware_mac
                        })
                        .ToArrayAsync();
                    return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dtos);
                case (short)tranSrc.tranSrcArea:
                    dtos = await context.area
                        .AsNoTracking()
                        .Where(x => x.location_id == location)
                        .Select(x => new ModeDto
                        {
                            Name = x.name,
                            Value = x.component_id,
                            Description = ""
                        })
                        .ToArrayAsync();
                    return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dtos);
                default:
                    return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>([]);
            }
        }
    }
}
