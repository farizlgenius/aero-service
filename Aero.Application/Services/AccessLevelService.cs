using Aero.Api.Constants;
using Aero.Application.Constants;
using Aero.Application.DTOs;
using Aero.Application.Helpers;
using Aero.Application.Interface;
using Aero.Application.Interfaces;
using Aero.Application.Mapper;
using Aero.Domain.Entities;
using Aero.Domain.Helpers;
using Aero.Domain.Interface;
using Aero.Domain.Interfaces;
using System.Security.Cryptography;

namespace Aero.Application.Services
{
    public sealed class AccessLevelService(IAlvlRepository repo,IHwRepository hw,IAlvlCommand alvl) : IAccessLevelService
    {
        public async Task<ResponseDto<IEnumerable<AccessLevelDto>>> GetByLocationIdAsync(short location)
        {
            var dtos = await repo.GetByLocationIdAsync(location);       
            return ResponseHelper.SuccessBuilder<IEnumerable<AccessLevelDto>>(dtos);
        }



        public async Task<ResponseDto<AccessLevelDto>> GetByIdAsync(int id)
        {
            var dtos = await repo.GetByIdAsync(id);      
            return ResponseHelper.SuccessBuilder<AccessLevelDto>(dtos);
        }



        public async Task<ResponseDto<AccessLevelDto>> CreateAsync(CreateAccessLevelDto dto)
        {
            if(await repo.IsAnyByNameAsync(dto.Name.Trim())) return ResponseHelper.BadRequestName<AccessLevelDto>();
            List<string> errors = new List<string>();

            var domain = new CreateAccessLevel(
                dto.Name,
                dto.Components.Select(x => new AccessLevelComponent(x.DriverId,x.Mac,x.DoorId,x.AcrId,x.TimeZoneId)).ToList(),
                dto.LocationId);

            var macs = domain.Components.Select(x => x.Mac).Distinct();

            for(int i = 0;i < macs.Count(); i++)
            {
                var DriverId = await repo.GetLowestUnassignedNumberAsync(10, macs.ElementAt(i));
                domain.Components.ElementAt(i).SetDriverId(DriverId); 
                if (!await alvl.AccessLevelConfigurationExtended(await hw.GetComponentIdFromMacAsync(macs.ElementAt(i)), DriverId, domain))
                {
                    errors.Add(MessageBuilder.Unsuccess(macs.ElementAt(i), Command.ALVL_CONFIG));

                }
            }


            if (errors.Count > 0) return ResponseHelper.UnsuccessBuilder<AccessLevelDto>(ResponseMessage.COMMAND_UNSUCCESS, errors);

            var status = await repo.AddAsync(domain);
            if(status <= 0) return ResponseHelper.UnsuccessBuilder<AccessLevelDto>(ResponseMessage.SAVE_DATABASE_UNSUCCESS,errors);
            return ResponseHelper.SuccessBuilder(await repo.GetByIdAsync());
        }

        public async Task<ResponseDto<AccessLevelDto>> DeleteAsync(short ComponentId)
        {
            List<string> errors = new List<string>();
            if (!await repo.IsAnyById(ComponentId)) return ResponseHelper.NotFoundBuilder<bool>();

            var domain = await repo.GetByIdAsync(ComponentId);

            foreach (var component in domain.Components)
            {
                var ScpId = await hw.GetComponentIdFromMacAsync(component.Mac);
                if (!await alvl.AccessLevelConfigurationExtended(ScpId,ComponentId, 0))
                {
                    errors.Add(MessageBuilder.Unsuccess(component.Mac, Command.ALVL_CONFIG));
                }
            }
            if (errors.Count > 0) return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.COMMAND_UNSUCCESS,errors);
            var status = await repo.DeleteByIdAsync(ComponentId);
            if(status <= 0) return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.DELETE_DATABASE_UNSUCCESS,errors);
            return ResponseHelper.SuccessBuilder(true);
        }

        public async Task<ResponseDto<AccessLevelDto>> UpdateAsync(AccessLevelDto dto)
        {

            List<string> errors = new List<string>();
            if (!await repo.IsAnyById(dto.ComponentId)) return ResponseHelper.NotFoundBuilder<AccessLevelDto>();

            var domain = AccessLevelMapper.ToDomain(dto);
            var macs = domain.Components.Select(x => x.Mac).Distinct();

            for (int i = 0; i < macs.Count(); i++)
            {
                //var AlvlId = await qAlvl.GetLowestUnassignedNumberAsync(10, macs.ElementAt(i));
                //domain.Components.ElementAt(i).AlvlId = AlvlId;
                if (!await alvl.AccessLevelConfigurationExtended(await hw.GetComponentIdFromMacAsync(macs.ElementAt(i)),domain.Components.Where(x => x.Mac.Equals(macs.ElementAt(i))).Select(x => x.AlvlId).FirstOrDefault(),domain))
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
            
            var status = await repo.UpdateCreateAsync(domain);

            if(status <= 0) return ResponseHelper.UnsuccessBuilder<AccessLevelDto>(ResponseMessage.UPDATE_RECORD_UNSUCCESS,errors);

            return ResponseHelper.SuccessBuilder(await repo.GetByIdAsync(dto.ComponentId));
        }


        public async Task<string> GetAcrName(string mac, short component)
        {
            return await repo.GetAcrNameByIdAndMacAsync(component,mac) ?? "";
        }

        public async Task<string> GetTzName(short component)
        {
            return await repo.GetTimezoneNameByIdAsync(component);
        }

            public async Task<ResponseDto<IEnumerable<AccessLevelDto>>> GetAsync()
            {
                  var res = await repo.GetAsync();
                  return ResponseHelper.SuccessBuilder(res);
            }

        public async Task<ResponseDto<Pagination<AccessLevelDto>>> GetPaginationAsync(PaginationParamsWithFilter param, short location)
        {
            var res = await repo.GetPaginationAsync(param,location);
            return ResponseHelper.SuccessBuilder(res);
        }
    }
}
