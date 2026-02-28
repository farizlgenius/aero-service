using Aero.Api.Constants;
using Aero.Application.Constants;
using Aero.Application.DTOs;
using Aero.Application.Helpers;
using Aero.Application.Interface;
using Aero.Application.Interfaces;
using Aero.Domain.Entities;
using Aero.Domain.Helpers;
using Aero.Domain.Interface;
using Aero.Domain.Interfaces;
using System.Security.Cryptography;

namespace Aero.Application.Services
{
    public sealed class AccessLevelService(IAlvlRepository repo,IHwRepository hw,IAlvlCommand alvl,ISettingRepository setting) : IAccessLevelService
    {
        public async Task<ResponseDto<IEnumerable<AccessLevelDto>>> GetByLocationIdAsync(int location)
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
            // Check value in license here 
            // ....to be implement

            if(await repo.IsAnyByNameAsync(dto.Name.Trim())) return ResponseHelper.BadRequestName<AccessLevelDto>();

            var ScpSetting = await setting.GetScpSettingAsync();


            List<string> errors = new List<string>();

            var domain = new CreateAccessLevel(
                dto.Name,
                dto.Components.Select(x => new AccessLevelComponent(x.DriverId,x.Mac,x.DoorId,x.AcrId,x.TimeZoneId)).ToList(),
                dto.LocationId);

            var macs = domain.Components.Select(x => x.Mac).Distinct();

            for(int i = 0;i < macs.Count(); i++)
            {
                var DriverId = await repo.GetLowestUnassignedNumberByMacAsync(macs.ElementAt(i), ScpSetting.nAlvl);
                domain.Components.ElementAt(i).SetDriverId(DriverId); 
                if (!await alvl.AccessLevelConfigurationExtended(await hw.GetComponentIdFromMacAsync(macs.ElementAt(i)), DriverId, domain))
                {
                    errors.Add(MessageBuilder.Unsuccess(macs.ElementAt(i), Command.ALVL_CONFIG));

                }
            }


            if (errors.Count > 0) return ResponseHelper.UnsuccessBuilder<AccessLevelDto>(ResponseMessage.COMMAND_UNSUCCESS, errors);

            var status = await repo.AddAsync(domain);
            if(status <= 0) return ResponseHelper.UnsuccessBuilder<AccessLevelDto>(ResponseMessage.SAVE_DATABASE_UNSUCCESS,errors);
            return ResponseHelper.SuccessBuilder(await repo.GetByIdAsync(status));
        }

        public async Task<ResponseDto<AccessLevelDto>> DeleteAsync(int id)
        {
            List<string> errors = new List<string>();
            if (!await repo.IsAnyById(id)) return ResponseHelper.NotFoundBuilder<AccessLevelDto>();

            var domain = await repo.GetByIdAsync(id);

            foreach (var component in domain.Components)
            {
                var ScpId = await hw.GetComponentIdFromMacAsync(component.Mac);
                if (!await alvl.AccessLevelConfigurationExtended(ScpId,component.DriverId, 0))
                {
                    errors.Add(MessageBuilder.Unsuccess(component.Mac, Command.ALVL_CONFIG));
                }
            }
            if (errors.Count > 0) return ResponseHelper.UnsuccessBuilder<AccessLevelDto>(ResponseMessage.COMMAND_UNSUCCESS,errors);
            var status = await repo.DeleteByIdAsync(id);
            if(status <= 0) return ResponseHelper.UnsuccessBuilder<AccessLevelDto>(ResponseMessage.DELETE_DATABASE_UNSUCCESS,errors);
            return ResponseHelper.SuccessBuilder(domain);
        }

        public async Task<ResponseDto<AccessLevelDto>> UpdateAsync(AccessLevelDto dto)
        {

            List<string> errors = new List<string>();
            if (!await repo.IsAnyById(dto.Id)) return ResponseHelper.NotFoundBuilder<AccessLevelDto>();

            var domain = new AccessLevel(dto.Id,dto.Name,dto.Components.Select(c => new AccessLevelComponent(c.DriverId,c.Mac,c.DoorId,c.AcrId,c.TimeZoneId)).ToList(),dto.LocationId,dto.IsActive);
            var macs = domain.Components.Select(x => x.Mac).Distinct();

            for (int i = 0; i < macs.Count(); i++)
            {
                //var AlvlId = await qAlvl.GetLowestUnassignedNumberAsync(10, macs.ElementAt(i));
                //domain.Components.ElementAt(i).AlvlId = AlvlId;
                if (!await alvl.AccessLevelConfigurationExtended(await hw.GetComponentIdFromMacAsync(macs.ElementAt(i)),domain.Components.Where(x => x.Mac.Equals(macs.ElementAt(i))).Select(x => x.DriverId).FirstOrDefault(),domain))
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
            
            var status = await repo.UpdateAsync(domain);

            if(status <= 0) return ResponseHelper.UnsuccessBuilder<AccessLevelDto>(ResponseMessage.UPDATE_RECORD_UNSUCCESS,errors);

            return ResponseHelper.SuccessBuilder(await repo.GetByIdAsync(dto.Id));
        }


        public async Task<string> GetAcrName(string mac, int component)
        {
            return await repo.GetAcrNameByIdAndDeviceIdAsync(component,mac) ?? "";
        }

        public async Task<string> GetTzName(int component)
        {
            return await repo.GetTimezoneNameByIdAsync(component);
        }

            public async Task<ResponseDto<IEnumerable<AccessLevelDto>>> GetAsync()
            {
                  var res = await repo.GetAsync();
                  return ResponseHelper.SuccessBuilder(res);
            }

        public async Task<ResponseDto<Pagination<AccessLevelDto>>> GetPaginationAsync(PaginationParamsWithFilter param, int location)
        {
            var res = await repo.GetPaginationAsync(param,location);
            return ResponseHelper.SuccessBuilder(res);
        }
    }
}
