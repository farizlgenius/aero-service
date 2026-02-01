

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
    public sealed class MonitorGroupService(IQMpgRepository qMpg,IMpgCommand mpg,IQHwRepository qHw,IMpgRepository rMpg) : IMonitorGroupService
    {
        public async Task<ResponseDto<bool>> CreateAsync(MonitorGroupDto dto)
        {

            var ComponentId = await qMpg.GetLowestUnassignedNumberAsync(10);
            if (ComponentId == -1) return ResponseHelper.ExceedLimit<bool>();

            var domain = MonitorGroupMapper.ToDomain(dto);

            var ScpId = await qHw.GetComponentIdFromMacAsync(dto.Mac);

            if (!mpg.ConfigureMonitorPointGroup(ScpId, ComponentId, dto.nMpCount, domain.nMpList.ToList()))
            {
                return ResponseHelper.UnsuccessBuilderWithString<bool>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(dto.Mac, Command.CONFIG_MPG));
            }

            var status = await rMpg.AddAsync(domain);

            if(status <= 0) return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.SAVE_DATABASE_UNSUCCESS,[]);

            return ResponseHelper.SuccessBuilder<bool>(true);
        }

        public async Task<ResponseDto<bool>> DeleteAsync(string mac, short Component)
        {
            if (!await qMpg.IsAnyByMacAndComponentIdAsync(mac,Component)) return ResponseHelper.NotFoundBuilder<bool>();

            var ScpId = await qHw.GetComponentIdFromMacAsync(mac);

            if (!mpg.ConfigureMonitorPointGroup(ScpId, Component, 0, []))
            {
                return ResponseHelper.UnsuccessBuilderWithString<bool>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(mac, Command.CONFIG_MPG));
            }

            var status = await rMpg.DeleteByComponentIdAsync(Component);

            if(status <= 0) return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.SAVE_DATABASE_UNSUCCESS,[]);

            return ResponseHelper.SuccessBuilder(true);
        }

        public async Task<ResponseDto<IEnumerable<MonitorGroupDto>>> GetAsync()
        {
            var dto = await qMpg.GetAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<MonitorGroupDto>>(dto);
        }

        public async Task<ResponseDto<IEnumerable<MonitorGroupDto>>> GetByLocationAsync(short location)
        {
            var dto = await qMpg.GetByLocationIdAsync(location);

            return ResponseHelper.SuccessBuilder<IEnumerable<MonitorGroupDto>>(dto);
        }

        public async Task<ResponseDto<IEnumerable<Mode>>> GetCommandAsync()
        {
            var dtos = await qMpg.GetCommandAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<Mode>>(dtos);

        }


        public async Task<ResponseDto<IEnumerable<Mode>>> GetTypeAsync()
        {
            var dtos = await qMpg.GetTypeAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<Mode>>(dtos);
        }

        public async Task<ResponseDto<bool>> MonitorGroupCommandAsync(MonitorGroupCommandDto dto)
        {
            if (!await qMpg.IsAnyByMacAndComponentIdAsync(dto.Mac,dto.ComponentId)) return ResponseHelper.NotFoundBuilder<bool>();

            var ScpId = await qHw.GetComponentIdFromMacAsync(dto.Mac);

            if (!mpg.MonitorPointGroupArmDisarm(ScpId, dto.ComponentId, dto.Command, dto.Arg))
            {
                return ResponseHelper.UnsuccessBuilderWithString<bool>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(dto.Mac, Command.MPG_ARM_DISARM));
            }

            return ResponseHelper.SuccessBuilder<bool>(true);
        }

        public async Task<ResponseDto<MonitorGroupDto>> UpdateAsync(MonitorGroupDto dto)
        {

            if (!await qMpg.IsAnyByMacAndComponentIdAsync(dto.Mac,dto.ComponentId)) return ResponseHelper.NotFoundBuilder<MonitorGroupDto>();

            var ScpId = await qHw.GetComponentIdFromMacAsync(dto.Mac);

            // Delete relate table first 
            var status = await rMpg.DeleteReferenceByMacAnsComponentIdAsync(dto.Mac,dto.ComponentId);

            if(status <= 0) return ResponseHelper.UnsuccessBuilder<MonitorGroupDto>(ResponseMessage.REMOVE_OLD_REF_UNSUCCESS,[]);

            var domain = MonitorGroupMapper.ToDomain(dto);

            if (!mpg.ConfigureMonitorPointGroup(ScpId, dto.ComponentId, dto.nMpCount, domain.nMpList.ToList()))
            {
                return ResponseHelper.UnsuccessBuilderWithString<MonitorGroupDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(dto.Mac, Command.CONFIG_MPG));
            }


            status = await rMpg.UpdateAsync(domain);

            if(status <= 0) return ResponseHelper.UnsuccessBuilder<MonitorGroupDto>(ResponseMessage.SAVE_DATABASE_UNSUCCESS,[]);

            return ResponseHelper.SuccessBuilder<MonitorGroupDto>(dto);

        }
    }
}
