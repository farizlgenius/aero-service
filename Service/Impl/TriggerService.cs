using HID.Aero.ScpdNet.Wrapper;
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
                .Include(x => x.CodeMap)
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
                 .Include(x => x.CodeMap)
                .Where(x => x.LocationId == location)
                .Select(x => MapperHelper.TriggerToDto(x))
                .ToArrayAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<TriggerDto>>(dto);
        }


        public async Task<ResponseDto<IEnumerable<ModeDto>>> GetCommandAsync()
        {
            var dtos = await context.TriggerCommands
                .AsNoTracking()
                .Select(x => new ModeDto 
                {
                    Name = x.Name,
                    Description = x.Description,
                    Value = x.Value,
                })
                .ToArrayAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dtos);
        }

        public async Task<ResponseDto<IEnumerable<ModeDto>>> GetSourceTypeAsync()
        {
            var dtos = await context.TransactionSources
                .AsNoTracking()
                .Select(x => new ModeDto
                {
                    Name = x.Name,
                    Description = x.Source,
                    Value = x.Value,
                })
                .ToArrayAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dtos);
        }

        public async Task<ResponseDto<IEnumerable<ModeDto>>> GetCodeByTranAsync(short tran)
        {
            var dtos = await context.TransactionCodes
                .AsNoTracking()
                .Where(x => x.TransactionTypeValue == tran)
                .Select(x => new ModeDto
                {
                    Name = x.Name,
                    Description = x.Description,
                    Value = x.Value,
                })
                .ToArrayAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dtos);
        }

        public async Task<ResponseDto<IEnumerable<ModeDto>>> GetTypeBySourceAsync(short source)
        {
            var dtos = await context.TransactionTypes
                .AsNoTracking()
                .Where(x => 
                    x.TransactionSourceTypes.All(x => x.TransactionSourceValue == source) &&
                    x.TransactionSourceTypes.Any()
                )
                .Select(x => new ModeDto
                {
                    Name = x.Name,
                    Description = "",
                    Value = x.Value,
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
                    var dtos = await context.Hardwares
                        .AsNoTracking()
                        .Where(x => x.LocationId == location)
                        .Select(x => new ModeDto
                        {
                            Name = x.Name,
                            Value = x.ComponentId,
                            Description = x.MacAddress
                        })
                        .ToArrayAsync();
                    return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dtos);
                case (short)tranSrc.tranSrcSioDiag:
                case (short)tranSrc.tranSrcSioCom:
                case (short)tranSrc.tranSrcSioTmpr:
                case (short)tranSrc.tranSrcSioPwr:
                    dtos = await context.Modules
                        .AsNoTracking()
                        .Where(x => x.LocationId == location)
                        .Select(x => new ModeDto
                        {
                            Name = x.Model,
                            Value = x.ComponentId,
                            Description = x.MacAddress
                        })
                        .ToArrayAsync();
                    return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dtos);
                case (short)tranSrc.tranSrcMP:
                    dtos = await context.MonitorPoints
                        .AsNoTracking()
                        .Where(x => x.LocationId == location)
                        .Select(x => new ModeDto
                        {
                            Name = x.Name,
                            Value = x.ComponentId,
                            Description = x.MacAddress
                        })
                        .ToArrayAsync();
                    return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dtos);
                case (short)tranSrc.tranSrcCP:
                    dtos = await context.ControlPoints
                        .AsNoTracking()
                        .Where(x => x.LocationId == location)
                        .Select(x => new ModeDto
                        {
                            Name = x.Name,
                            Value = x.ComponentId,
                            Description = x.MacAddress
                        })
                        .ToArrayAsync();
                    return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dtos);
                case (short)tranSrc.tranSrcACR:
                case (short)tranSrc.tranSrcAcrTmpr:
                case (short)tranSrc.tranSrcAcrDoor:
                case (short)tranSrc.tranSrcAcrRex0:
                case (short)tranSrc.tranSrcAcrRex1:
                case (short)tranSrc.tranSrcAcrTmprAlt:
                    dtos = await context.Doors
                        .AsNoTracking()
                        .Where(x => x.LocationId == location)
                        .Select(x => new ModeDto 
                        {
                            Name = x.Name,
                            Value = x.ComponentId,
                            Description = x.MacAddress
                        })
                        .ToArrayAsync();
                    return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dtos);
                case (short)tranSrc.tranSrcTimeZone:
                    dtos = await context.TimeZones
                        .AsNoTracking()
                        .Where(x => x.LocationId == location)
                        .Select(x => new ModeDto
                        {
                            Name = x.Name,
                            Value = x.ComponentId,
                            Description = ""
                        })
                        .ToArrayAsync();
                    return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dtos);
                case (short)tranSrc.tranSrcProcedure:
                    dtos = await context.Procedures
                        .AsNoTracking()
                        .Where(x => x.LocationId == location)
                        .Select(x => new ModeDto
                        {
                            Name = x.Name,
                            Value = x.ComponentId,
                            Description = x.MacAddress
                        })
                        .ToArrayAsync();
                    return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dtos);
                case (short)tranSrc.tranSrcTrigger:
                case (short)tranSrc.tranSrcTrigVar:
                    dtos = await context.Triggers
                        .AsNoTracking()
                        .Where(x => x.LocationId == location)
                        .Select(x => new ModeDto
                        {
                            Name = x.Name,
                            Value = x.ComponentId,
                            Description = x.MacAddress
                        })
                        .ToArrayAsync();
                    return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dtos);
                case (short)tranSrc.tranSrcMPG:
                    dtos = await context.MonitorGroups
                        .AsNoTracking()
                        .Where(x => x.LocationId == location)
                        .Select(x => new ModeDto
                        {
                            Name = x.Name,
                            Value = x.ComponentId,
                            Description = x.MacAddress
                        })
                        .ToArrayAsync();
                    return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dtos);
                case (short)tranSrc.tranSrcArea:
                    dtos = await context.AccessAreas
                        .AsNoTracking()
                        .Where(x => x.LocationId == location)
                        .Select(x => new ModeDto
                        {
                            Name = x.Name,
                            Value = x.ComponentId,
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
