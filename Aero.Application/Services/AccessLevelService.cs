using Aero.Api.Constants;
using Aero.Application.Constants;
using Aero.Application.DTOs;
using Aero.Application.Helpers;
using Aero.Application.Interface;
using Aero.Application.Interfaces;
using Aero.Application.Mapper;
using Aero.Domain.Interface;
using Aero.Domain.Interfaces;

namespace Aero.Application.Services
{
    public sealed class AccessLevelService(IQAlvlRepository qAlvl,IQHwRepository qHw,IAlvlCommand alvl,IAlvlRepository rAlvl) : IAccessLevelService
    {
        public async Task<ResponseDto<IEnumerable<AccessLevelDto>>> GetByLocationIdAsync(short location)
        {
            var dtos = await qAlvl.GetByLocationIdAsync(location);       
            return ResponseHelper.SuccessBuilder<IEnumerable<AccessLevelDto>>(dtos);
        }



        public async Task<ResponseDto<AccessLevelDto>> GetByComponentIdAsync(short component)
        {
            var dtos = await qAlvl.GetByComponentIdAsync(component);      
            return ResponseHelper.SuccessBuilder<AccessLevelDto>(dtos);
        }



        public async Task<ResponseDto<bool>> CreateAsync(CreateUpdateAccessLevelDto dto)
        {
            List<string> errors = new List<string>();
            var ComponentId = await qAlvl.GetLowestUnassignedNumberAsync(10,"");
            if (ComponentId == -1) return ResponseHelper.ExceedLimit<bool>();

            
            var domain = AccessLevelMapper.ToCreateDomain(dto);

            foreach(var component in domain.Components)
            {
                if (!alvl.AccessLevelConfigurationExtendedCreate(await qHw.GetComponentIdFromMacAsync(component.Mac), ComponentId,component.DoorComponents))
                {
                   errors.Add(MessageBuilder.Unsuccess(component.Mac, Command.ALVL_CONFIG));

                }
            }


            if (errors.Count > 0) return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.COMMAND_UNSUCCESS, errors);

            var status = await rAlvl.AddCreateAsync(domain);
            if(status <= 0) return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.SAVE_DATABASE_UNSUCCESS,errors);
            return ResponseHelper.SuccessBuilder(true);
        }

        public async Task<ResponseDto<bool>> DeleteAsync(short ComponentId)
        {
            List<string> errors = new List<string>();
            if (!await qAlvl.IsAnyByComponentId(ComponentId)) return ResponseHelper.NotFoundBuilder<bool>();

            var domain = await qAlvl.GetByComponentIdAsync(ComponentId);

            foreach (var component in domain.Components)
            {
                var ScpId = await qHw.GetComponentIdFromMacAsync(component.Mac);
                if (!alvl.AccessLevelConfigurationExtended(ScpId,ComponentId, 0))
                {
                    errors.Add(MessageBuilder.Unsuccess(component.Mac, Command.ALVL_CONFIG));
                }
            }
            if (errors.Count > 0) return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.COMMAND_UNSUCCESS,errors);
            var status = await rAlvl.DeleteByComponentIdAsync(ComponentId);
            if(status <= 0) return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.DELETE_DATABASE_UNSUCCESS,errors);
            return ResponseHelper.SuccessBuilder(true);
        }

        public async Task<ResponseDto<AccessLevelDto>> UpdateAsync(CreateUpdateAccessLevelDto dto)
        {


            if (!await qAlvl.IsAnyByComponentId(dto.ComponentId)) return ResponseHelper.NotFoundBuilder<AccessLevelDto>();

            var domain = AccessLevelMapper.ToCreateDomain(dto);

            List<string> errors = new List<string>();

            foreach (var component in domain.Components)
            {
                var ScpId = await qHw.GetComponentIdFromMacAsync(component.Mac);
                if (ScpId == 0)
                {
                    errors.Add(MessageBuilder.Notfound());
                    continue;
                }
                if (!alvl.AccessLevelConfigurationExtendedCreate(ScpId, dto.ComponentId,component.DoorComponents))
                {
                    errors.Add(MessageBuilder.Unsuccess(component.Mac, Command.ALVL_CONFIG));

                }

            }

            if (errors.Count > 0) return ResponseHelper.UnsuccessBuilder<AccessLevelDto>(ResponseMessage.COMMAND_UNSUCCESS, errors);
            
            var status = await rAlvl.UpdateCreateAsync(domain);

            if(status <= 0) return ResponseHelper.UnsuccessBuilder<AccessLevelDto>(ResponseMessage.UPDATE_RECORD_UNSUCCESS,errors);

            return ResponseHelper.SuccessBuilder(await qAlvl.GetByComponentIdAsync(dto.ComponentId));
        }


        public async Task<string> GetAcrName(string mac, short component)
        {
            return await qAlvl.GetACRNameByComponentIdAndMacAsync(component,mac) ?? "";
        }

        public async Task<string> GetTzName(short component)
        {
            return await qAlvl.GetTimezoneNameByComponentIdAsync(component);
        }

            public async Task<ResponseDto<IEnumerable<AccessLevelDto>>> GetAsync()
            {
                  var res = await qAlvl.GetAsync();
                  return ResponseHelper.SuccessBuilder(res);
            }
      }
}
