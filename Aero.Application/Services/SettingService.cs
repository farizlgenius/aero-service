using System.Diagnostics;
using Aero.Api.Constants;
using Aero.Application.Commands.Interfaces;
using Aero.Application.Constants;
using Aero.Application.DTOs;
using Aero.Application.Helpers;
using Aero.Application.Interface;
using Aero.Application.Interfaces;
using Aero.Domain.Entities;
using Aero.Domain.Interface;

namespace Aero.Application.Services
{
    public sealed class SettingService(ISettingRepository repo,IAeroAdapter aero,IDeviceRepository device) : ISettingService
    {
        public async Task<ResponseDto<IEnumerable<LedDto>>> GetLedSettingAsync()
        {
            var dto = await repo.GetLedSettingAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<LedDto>>(dto);
        }

            public async Task<ResponseDto<LedDto>> GetLedSettingByIdAsync(int id)
            {
                 var dto = await repo.GetLedByIdAsync(id);
            return ResponseHelper.SuccessBuilder<LedDto>(dto);
            }

            public async Task<ResponseDto<PasswordRuleDto>> GetPasswordRuleAsync()
        {
            var dto = await repo.GetPasswordRuleAsync();
            return ResponseHelper.SuccessBuilder<PasswordRuleDto>(dto);
        }

        public async Task<ResponseDto<LedDto>> UpdateLedSettingAsync(LedDto dto)
        {
            var en = await repo.GetLedByIdAsync(dto.Id);
            if(en is null) return ResponseHelper.NotFoundBuilder<LedDto>();

            var ScpIds = await device.GetDriverIdsAsync(); 

            var domain = new Led(dto.Id,dto.LedMode,dto.Config.Select(x => new LedConfig(0,dto.Id,x.RLedId,x.OnColor,x.OffColor,x.OnTime,x.OffTime,x.RepeatCount,x.BeepCount)).ToList());

            List<string> errors = new List<string>();
            foreach(var id in ScpIds)
            {
                if (!aero.ReaderLedBuzzerFunctionSpec(id, domain))
                {
                    errors.Add(MessageBuilder.Unsuccess(await device.GetMacFromComponentAsync(id),Command.RLED_SPEC));
                }
            }

            var status = await repo.UpdateLedSettingAsync(domain);
            if(status <= 0) return ResponseHelper.UnsuccessBuilder<LedDto>(ResponseMessage.UPDATE_RECORD_UNSUCCESS,[]);

            return ResponseHelper.SuccessBuilder<LedDto>(en);

            
        }

        public async Task<ResponseDto<PasswordRuleDto>> UpdatePasswordRuleAsync(PasswordRuleDto dto)
        {

            if (!await repo.IsAnyPasswordRule()) return ResponseHelper.NotFoundBuilder<PasswordRuleDto>();

            var domain = new Domain.Entities.PasswordRule(dto.Len, dto.IsLower, dto.IsUpper, dto.IsDigit, dto.IsSymbol, dto.Weaks);

            var status = await repo.UpdatePasswordRuleAsync(domain);

            return ResponseHelper.SuccessBuilder<PasswordRuleDto>(dto);
        }
    }
}
