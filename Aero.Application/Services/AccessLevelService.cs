using Aero.Api.Constants;
using Aero.Application.Constants;
using Aero.Application.DTOs;
using Aero.Application.Helpers;
using Aero.Application.Interface;
using Aero.Application.Interfaces;
using Aero.Application.Mapper;
using Aero.Domain.Entities;
using Aero.Domain.Interface;
using Aero.Domain.Interfaces;
using System.Security.Cryptography;

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



        public async Task<ResponseDto<bool>> CreateAsync(AccessLevelDto dto)
        {
            List<string> errors = new List<string>();
            var ComponentId = await qAlvl.GetLowestUnassignedNumberAsync(10,"");
            
            if (ComponentId == -1) return ResponseHelper.ExceedLimit<bool>();

            
            var domain = AccessLevelMapper.ToDomain(dto);
            domain.ComponentId = ComponentId;

            var macs = domain.Components.Select(x => x.Mac).Distinct();

            for(int i = 0;i < macs.Count(); i++)
            {
                var AlvlId = await qAlvl.GetLowestUnassignedNumberAsync(10, macs.ElementAt(i));
                domain.Components.ElementAt(i).AlvlId = AlvlId; 
                if (!await alvl.AccessLevelConfigurationExtended(await qHw.GetComponentIdFromMacAsync(macs.ElementAt(i)),AlvlId, domain))
                {
                    errors.Add(MessageBuilder.Unsuccess(macs.ElementAt(i), Command.ALVL_CONFIG));

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
                if (!await alvl.AccessLevelConfigurationExtended(ScpId,ComponentId, 0))
                {
                    errors.Add(MessageBuilder.Unsuccess(component.Mac, Command.ALVL_CONFIG));
                }
            }
            if (errors.Count > 0) return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.COMMAND_UNSUCCESS,errors);
            var status = await rAlvl.DeleteByComponentIdAsync(ComponentId);
            if(status <= 0) return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.DELETE_DATABASE_UNSUCCESS,errors);
            return ResponseHelper.SuccessBuilder(true);
        }

        public async Task<ResponseDto<AccessLevelDto>> UpdateAsync(AccessLevelDto dto)
        {

            List<string> errors = new List<string>();
            if (!await qAlvl.IsAnyByComponentId(dto.ComponentId)) return ResponseHelper.NotFoundBuilder<AccessLevelDto>();

            var domain = AccessLevelMapper.ToDomain(dto);
            var macs = domain.Components.Select(x => x.Mac).Distinct();

            for (int i = 0; i < macs.Count(); i++)
            {
                //var AlvlId = await qAlvl.GetLowestUnassignedNumberAsync(10, macs.ElementAt(i));
                //domain.Components.ElementAt(i).AlvlId = AlvlId;
                if (!await alvl.AccessLevelConfigurationExtended(await qHw.GetComponentIdFromMacAsync(macs.ElementAt(i)),domain.Components.Where(x => x.Mac.Equals(macs.ElementAt(i))).Select(x => x.AlvlId).FirstOrDefault(),domain))
                {
                    errors.Add(MessageBuilder.Unsuccess(macs.ElementAt(i), Command.ALVL_CONFIG));

                }
            }



            //foreach (var component in domain.Components)
            //{
            //    var ScpId = await qHw.GetComponentIdFromMacAsync(component.Mac);
            //    if (ScpId == 0)
            //    {
            //        errors.Add(MessageBuilder.Notfound());
            //        continue;
            //    }
            //    if (!alvl.AccessLevelConfigurationExtendedCreate(ScpId, dto.ComponentId,component.DoorComponents))
            //    {
            //        errors.Add(MessageBuilder.Unsuccess(component.Mac, Command.ALVL_CONFIG));

            //    }

            //}

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

        public async Task<ResponseDto<Pagination<AccessLevelDto>>> GetPaginationAsync(PaginationParamsWithFilter param, short location)
        {
            var res = await qAlvl.GetPaginationAsync(param,location);
            return ResponseHelper.SuccessBuilder(res);
        }
    }
}
