using AeroService.Aero.CommandService;
using AeroService.Aero.CommandService.Impl;
using AeroService.Constant;
using AeroService.Constants;
using AeroService.Data;
using AeroService.DTO;
using AeroService.DTO.ControlPoint;
using AeroService.DTO.Output;
using AeroService.Entity;
using AeroService.Entity.Interface;
using AeroService.Helpers;
using AeroService.Hubs;
using AeroService.Mapper;
using AeroService.Model;
using AeroService.Utility;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using MiNET.Entities;
using System.ComponentModel;
using System.Net;

namespace AeroService.Service.Impl
{
    public sealed class ControlPointService(AppDbContext context,IHelperService<Strike> helperService,AeroCommandService command,ILogger<ControlPointService> logger,IHubContext<AeroHub> hub) : IControlPointService 
    {
        public async Task<ResponseDto<IEnumerable<ControlPointDto>>> GetAsync()
        {
            var dtos = await context.control_point
                .AsNoTracking()
                .Select(x => new ControlPointDto
                {
                    // Base
                    Uuid = x.uuid,
                    ComponentId = x.component_id,
                    HardwareName = x.module.hardware.name,
                    Mac = x.module.hardware_mac,
                    LocationId = x.location_id,
                    IsActive = x.is_active,

                    // extend_desc
                    Name = x.name,
                    ModuleId = x.module_id,
                    ModuleDescription = x.module.model_desc,
                    //module_desc = x.module_desc,
                    OutputNo = x.output_no,
                    RelayMode = x.relay_mode,
                    RelayModeDescription = x.relay_mode_desc,
                    OfflineMode = x.offline_mode,
                    OfflineModeDescription = x.offline_mode_desc,
                    DefaultPulse = x.default_pulse,
                })
                .ToArrayAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<ControlPointDto>>(dtos);
        }

        public async Task<ResponseDto<IEnumerable<ControlPointDto>>> GetByLocationAsync(short location)
        {
            var dtos = await context.control_point
                .AsNoTracking()
                .Where(x => x.location_id == location)
                .Select(x => new ControlPointDto
                {
                    // Base
                    Uuid = x.uuid,
                    ComponentId = x.component_id,
                    HardwareName = x.module.hardware.name,
                    Mac = x.module.hardware_mac,
                    LocationId = x.location_id,
                    IsActive = x.is_active,

                    // extend_desc
                    Name = x.name,
                    ModuleId = x.module_id,
                    ModuleDescription = x.module.model_desc,
                    //module_desc = x.module_desc,
                    OutputNo = x.output_no,
                    RelayMode = x.relay_mode,
                    RelayModeDescription = x.relay_mode_desc,
                    OfflineMode = x.offline_mode,
                    OfflineModeDescription = x.offline_mode_desc,
                    DefaultPulse = x.default_pulse,
                })
                .ToArrayAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<ControlPointDto>>(dtos);
        }

        private async Task<ResponseDto<IEnumerable<ModeDto>>> GetOfflineModeAsync()
        {
            var dtos = await context.relay_offline_mode.AsNoTracking().Select(x => new ModeDto 
            {
                Name = x.name,
                Value = x.value,
                Description = x.description,
            }).ToArrayAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dtos);
        }


        private async Task<ResponseDto<IEnumerable<ModeDto>>> GetRelayModeAsync()
        {
            var dtos = await context.relay_mode.AsNoTracking().Select(x => new ModeDto 
            {
                Name = x.name,
                Value = x.value,
                Description = x.description,
            }).ToArrayAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dtos);
        }


        public async Task<ResponseDto<bool>> ToggleAsync(ToggleControlPointDto dto)
        {
            List<string> errors = new List<string>();
            var id = await helperService.GetIdFromMacAsync(dto.macAddress);
            if(id == 0) return ResponseHelper.NotFoundBuilder<bool>();
            if (!command.ControlPointCommand(id, dto.ComponentId, dto.Command))
            {
                return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.COMMAND_UNSUCCESS,MessageBuilder.Unsuccess(dto.macAddress,Command.C307));
            }
            return ResponseHelper.SuccessBuilder(true);
        }

        public async Task<ResponseDto<IEnumerable<short>>> GetAvailableOpAsync(string mac, short ModuleId)
        {
            var ops = await context.module
                .AsNoTracking()
                .Where(sio => sio.hardware_mac == mac && sio.component_id == ModuleId)
                .Select(cp => cp.n_output)
                .FirstOrDefaultAsync();

            var strk = await context.strike
                .AsNoTracking()
                .Where(x => x.module_id == ModuleId && x.module.hardware_mac == mac)
                .Select(x => x.output_no)
                .ToArrayAsync();

            var cp = await context.control_point
                .AsNoTracking()
                .Where(x => x.module_id == ModuleId && x.module.hardware_mac == mac)
                .Select(x => x.output_no)
                .ToArrayAsync();


            var unavailable = strk
                .Union(cp)
                .Distinct()
                .ToList();

            List<short> all = Enumerable.Range(0, ops).Select(x => (short)x).ToList();
            return ResponseHelper.SuccessBuilder<IEnumerable<short>>(all.Except(unavailable).ToList());
        }


        public void TriggerDeviceStatus(string ScpMac, int first, string status)
        {
            //GetOnlineStatus()
            var result = hub.Clients.All.SendAsync("CpStatus", ScpMac, first, status);
        }



        public async Task<ResponseDto<bool>> CreateAsync(ControlPointDto dto)
        {
            short scpId = await helperService.GetIdFromMacAsync(dto.Mac);

            var max = await context.system_setting.AsNoTracking().Select(x => x.n_cp).FirstOrDefaultAsync();
            var componentId = await helperService.GetLowestUnassignedNumberAsync<ControlPoint>(context, max);

            short modeNo = await context.output_mode
                .AsNoTracking()
                .Where(x => x.offline_mode == dto.OfflineMode && x.relay_mode == dto.RelayMode)
                .Select(x => x.value).FirstOrDefaultAsync();

            if (!command.OutputPointSpecification(scpId, dto.ModuleId, dto.OutputNo, modeNo))
            {
                return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(dto.Mac, Command.C111));
            }


            if (componentId == -1) return ResponseHelper.ExceedLimit<bool>();

            if (!command.ControlPointConfiguration(scpId, dto.ModuleId, (short)componentId, dto.OutputNo, dto.DefaultPulse))
            {
                return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(dto.Mac, Command.C114));

            }


            var output = MapperHelper.DtoToControlPoint(dto,componentId,DateTime.Now);
            await context.control_point.AddAsync(output);
            await context.SaveChangesAsync();
            return ResponseHelper.SuccessBuilder(true);
        }

        public async Task<ResponseDto<bool>> DeleteAsync(short Id)
        {
            var output = await context.control_point
                .Include(x => x.module)
                .FirstOrDefaultAsync(x => x.component_id == Id);

            if (output is null) return ResponseHelper.NotFoundBuilder<bool>();

            var scpId = await helperService.GetIdFromMacAsync(output.module.hardware_mac);

            if (!command.ControlPointConfiguration(scpId, -1, (short)output.component_id, output.output_no, output.default_pulse))
            {
                return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.COMMAND_UNSUCCESS,MessageBuilder.Unsuccess(output.module.hardware_mac,Command.C114));
            }

            context.control_point.Remove(output);
            await context.SaveChangesAsync();
            return ResponseHelper.SuccessBuilder(true);
        }

        public async Task<ResponseDto<ControlPointDto>> UpdateAsync(ControlPointDto dto)
        {
            var output = await context.control_point
                .FirstOrDefaultAsync(x => x.module.hardware_mac == dto.Mac && x.component_id == dto.ComponentId);

            if (output is null) return ResponseHelper.NotFoundBuilder<ControlPointDto>();

            var scpId = await helperService.GetIdFromMacAsync(dto.Mac);
            short modeNo = await context.output_mode.AsNoTracking().Where(x => x.offline_mode == dto.OfflineMode && x.relay_mode == dto.RelayMode).Select(x => x.value).FirstOrDefaultAsync();
            if (!command.OutputPointSpecification(scpId, dto.ModuleId, dto.OutputNo, modeNo))
            {
                return ResponseHelper.UnsuccessBuilder<ControlPointDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(dto.Mac, Command.C111));
            }

            if (!command.ControlPointConfiguration(scpId, dto.ModuleId, (short)dto.ComponentId, dto.OutputNo, dto.DefaultPulse))
            {
                return ResponseHelper.UnsuccessBuilder<ControlPointDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(dto.Mac, Command.C114));
            }


            MapperHelper.UpdateControlPoint(output,dto);

            context.control_point.Update(output);
            await context.SaveChangesAsync();
            return ResponseHelper.SuccessBuilder(dto);
        }

        public async Task<ResponseDto<bool>> GetStatusAsync(string mac, short component)
        {
            var id = await helperService.GetIdFromMacAsync(mac);
            if (!command.GetCpStatus(id, component, 1))
            {
                return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.COMMAND_UNSUCCESS,MessageBuilder.Unsuccess(mac,Command.C406));
            }
            return ResponseHelper.SuccessBuilder(true);
        }

        public async Task<ResponseDto<IEnumerable<ModeDto>>> GetModeAsync(int param)
        {
            switch (param)
            {
                case 0:
                    return await GetOfflineModeAsync();
                case 1:
                    return await GetRelayModeAsync();
                default:
                    return ResponseHelper.NotFoundBuilder<IEnumerable<ModeDto>>();
            }
        }

        public async Task<ResponseDto<ControlPointDto>> GetByMacAndIdAsync(string Mac, short ComponentId)
        {
            var dto = await context.control_point
                .Where(x => x.module.hardware_mac == Mac && x.component_id == ComponentId)
               .Select(x => new ControlPointDto
               {
                   // Base
                   Uuid = x.uuid,
                   ComponentId = x.component_id,
                   HardwareName = x.module.hardware.name,
                   Mac = x.module.hardware_mac,
                   LocationId = x.location_id,
                   IsActive = x.is_active,

                   // extend_desc
                   Name = x.name,
                   ModuleId = x.module_id,
                   ModuleDescription = x.module.model_desc,
                   //module_desc = x.module_desc,
                   OutputNo = x.output_no,
                   RelayMode = x.relay_mode,
                   RelayModeDescription = x.relay_mode_desc,
                   OfflineMode = x.offline_mode,
                   OfflineModeDescription = x.offline_mode_desc,
                   DefaultPulse = x.default_pulse,
               })
               .FirstOrDefaultAsync();

            if (dto is null) return ResponseHelper.NotFoundBuilder<ControlPointDto>();

            return ResponseHelper.SuccessBuilder<ControlPointDto>(dto);
        }

        public async Task<ResponseDto<IEnumerable<ResponseDto<bool>>>> DeleteRangeAsync(List<short> components)
        {
            bool flag = true;
            List<ResponseDto<bool>> data = new List<ResponseDto<bool>>();
            foreach (var dto in components)
            {
                var re = await DeleteAsync(dto);
                if (re.code != HttpStatusCode.OK) flag = false;
                data.Add(re);
            }

            if (!flag) return ResponseHelper.UnsuccessBuilder<IEnumerable<ResponseDto<bool>>>(data);

            var res = ResponseHelper.SuccessBuilder<IEnumerable<ResponseDto<bool>>>(data);

            return res;
        }
    }
}
