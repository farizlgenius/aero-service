using System.Data;
using Aero.Api.Constants;
using Aero.Application.Constants;
using Aero.Application.DTOs;
using Aero.Application.Helpers;
using Aero.Application.Interface;
using Aero.Application.Interfaces;
using Aero.Application.Mapper;
using Aero.Domain.Entities;
using Aero.Domain.Interface;

namespace Aero.Application.Services{
    public sealed class ProcedureService(IProcedureRepository rProc,IQProcedureRepository qProc,IQHwRepository qHw,IProcCommand proc) : IProcedureService
    {
        public async Task<ResponseDto<bool>> CreateAsync(ProcedureDto dto)
        {
            var ComponentId = await qProc.GetLowestUnassignedNumberAsync(10,"");
            var ProcId = await qProc.GetLowestUnassignedNumberAsync(10,dto.Mac);

            foreach(var ac in dto.Actions)
            {
                if(ac.ActionType == 9)
                {
                    ac.ScpId = 0;
                }
                else
                {
                    ac.ScpId = await qHw.GetComponentIdFromMacAsync(ac.Mac);
                }
               
            }

            dto.ProcId = ProcId;
            dto.ComponentId = ComponentId;

            var domain = ProcedureMapper.ToDomain(dto);
            

            var ids = await qHw.GetComponentIdsAsync();
            foreach(var ac in domain.Actions)
            {
                if (ac.ActionType == 9)
                {
                    if (!proc.ActionSpecificationAsyncForAllHW(ComponentId, ac, ids.ToList()))
                    {
                        return ResponseHelper.UnsuccessBuilderWithString<bool>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess("", Command.ACTION_SPEC));
                    }
                }
                else if (ac.DelayTime != 0) 
                {
                    if(!proc.ActionSpecificationDelayAsync(ComponentId, ac))
                    {
                        return ResponseHelper.UnsuccessBuilderWithString<bool>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess("", Command.ACTION_SPEC));
                    }
                }
            }

            if (!proc.ActionSpecificationAsync(ComponentId, domain.Actions.ToList()))
            {
                return ResponseHelper.UnsuccessBuilderWithString<bool>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess("", Command.ACTION_SPEC));
            }

            var status = await rProc.AddAsync(domain);
            if(status <= 0) return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.SAVE_DATABASE_UNSUCCESS,[]);

            return ResponseHelper.SuccessBuilder<bool>(true);
            
        }

        public async Task<ResponseDto<bool>> DeleteAsync(short ComponentId)
        {

            if (!await qProc.IsAnyByComponentId(ComponentId)) return ResponseHelper.NotFoundBuilder<bool>();

            var ac = new Aero.Domain.Entities.Action
            {
                ActionType = 0,
            };

            if(!proc.ActionSpecificationAsync(ComponentId, [ac]))
            {
                return ResponseHelper.UnsuccessBuilderWithString<bool>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess("", Command.ACTION_SPEC));
            }

            var status = await rProc.DeleteByComponentIdAsync(ComponentId);
            if(status <= 0) return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.DELETE_DATABASE_UNSUCCESS,[]);

            return ResponseHelper.SuccessBuilder<bool>(true);
        }

        public async Task<ResponseDto<IEnumerable<Mode>>> GetActionType()
        {
            var dtos = await qProc.GetActionTypeAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<Mode>>(dtos);
        }

        public async Task<ResponseDto<IEnumerable<ProcedureDto>>> GetAsync()
        {
           var dtos = await qProc.GetAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<ProcedureDto>>(dtos);
        }

        public async Task<ResponseDto<IEnumerable<ProcedureDto>>> GetByLocationIdAsync(short location)
        {
            var dtos = await qProc.GetByLocationIdAsync(location);

            return ResponseHelper.SuccessBuilder<IEnumerable<ProcedureDto>>(dtos);
        }

        public Task<ResponseDto<ProcedureDto>> UpdateAsync(ProcedureDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
