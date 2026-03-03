using System.Data;
using Aero.Api.Constants;
using Aero.Application.Constants;
using Aero.Application.DTOs;
using Aero.Application.Helpers;
using Aero.Application.Interface;
using Aero.Application.Interfaces;

using Aero.Domain.Entities;
using Aero.Domain.Interface;

namespace Aero.Application.Services{
    public sealed class ProcedureService(IProcedureRepository repo,IDeviceRepository hw,IProcCommand proc,ISettingRepository setting) : IProcedureService
    {
        public async Task<ResponseDto<ProcedureDto>> CreateAsync(ProcedureDto dto)
        {
            // Check value in license here 
            // ....to be implement

            if(await repo.IsAnyByNameAsync(dto.Name.Trim())) return ResponseHelper.BadRequestName<ProcedureDto>();

            var ScpSetting = await setting.GetScpSettingAsync();

            var DriverId = await repo.GetLowestUnassignedNumberAsync(ScpSetting.nProc,dto.DeviceId);



            var domain = new Procedure(
                dto.Id,
                dto.DeviceId,
                dto.DriverId,
                dto.TriggerId,
                dto.Name,
                dto.Actions.Select(x => new Aero.Domain.Entities.Action(
                    x.ActionType == 0 ? (short)0 :(short)x.DeviceId,
                    x.ActionType,
                    x.ActionDetail,
                    x.Arg1,
                    x.Arg2,
                    x.Arg3,
                    x.Arg4,
                    x.Arg5,
                    x.Arg6,
                    x.Arg7,
                    x.StrArg,
                    x.DelayTime,
                    x.ProcedureId,
                    x.LocationId,
                    x.IsActive
                )).ToList()
                );
            

            var ids = await hw.GetDriverIdsAsync();
            foreach(var ac in domain.Actions)
            {
                if (ac.ActionType == 9)
                {
                    if (!proc.ActionSpecificationAsyncForAllHW(DriverId, ac, ids.ToList()))
                    {
                        return ResponseHelper.UnsuccessBuilderWithString<ProcedureDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess("", Command.ACTION_SPEC));
                    }
                }
                else if (ac.DelayTime != 0) 
                {
                    if(!proc.ActionSpecificationDelayAsync(DriverId, ac))
                    {
                        return ResponseHelper.UnsuccessBuilderWithString<ProcedureDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess("", Command.ACTION_SPEC));
                    }
                }
            }

            if (!proc.ActionSpecificationAsync(DriverId, domain.Actions.ToList()))
            {
                return ResponseHelper.UnsuccessBuilderWithString<ProcedureDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess("", Command.ACTION_SPEC));
            }

            var status = await repo.AddAsync(domain);
            if(status <= 0) return ResponseHelper.UnsuccessBuilder<ProcedureDto>(ResponseMessage.SAVE_DATABASE_UNSUCCESS,[]);

            return ResponseHelper.SuccessBuilder<ProcedureDto>(await repo.GetByIdAsync(status));
            
        }

        public async Task<ResponseDto<ProcedureDto>> DeleteAsync(int id)
        {
            var en = await repo.GetByIdAsync(id);

            if (en is null) return ResponseHelper.NotFoundBuilder<ProcedureDto>();

            var ac = new Aero.Domain.Entities.Action
            {
                ActionType = 0,
            };

            if(!proc.ActionSpecificationAsync(en.DriverId, [ac]))
            {
                return ResponseHelper.UnsuccessBuilderWithString<ProcedureDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess("", Command.ACTION_SPEC));
            }

            var status = await repo.DeleteByIdAsync(id);
            if(status <= 0) return ResponseHelper.UnsuccessBuilder<ProcedureDto>(ResponseMessage.DELETE_DATABASE_UNSUCCESS,[]);

            return ResponseHelper.SuccessBuilder<ProcedureDto>(en);
        }

        public async Task<ResponseDto<IEnumerable<ModeDto>>> GetActionType()
        {
            var dtos = await repo.GetActionTypeAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dtos);
        }

        public async Task<ResponseDto<IEnumerable<ProcedureDto>>> GetAsync()
        {
           var dtos = await repo.GetAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<ProcedureDto>>(dtos);
        }

        public async Task<ResponseDto<IEnumerable<ProcedureDto>>> GetByLocationIdAsync(int location)
        {
            var dtos = await repo.GetByLocationIdAsync(location);

            return ResponseHelper.SuccessBuilder<IEnumerable<ProcedureDto>>(dtos);
        }

        public async Task<ResponseDto<Pagination<ProcedureDto>>> GetPaginationAsync(PaginationParamsWithFilter param, int location)
        {
            var res = await repo.GetPaginationAsync(param,location);
            return ResponseHelper.SuccessBuilder(res);
        }

        public Task<ResponseDto<ProcedureDto>> UpdateAsync(ProcedureDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
