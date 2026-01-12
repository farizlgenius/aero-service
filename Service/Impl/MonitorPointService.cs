using HID.Aero.ScpdNet.Wrapper;
using AeroService.Aero.CommandService;
using AeroService.Aero.CommandService.Impl;
using AeroService.Constant;
using AeroService.Constants;
using AeroService.Data;
using AeroService.DTO;
using AeroService.DTO.MonitorPoint;
using AeroService.Entity;
using AeroService.Helpers;
using AeroService.Hubs;
using AeroService.Mapper;
using AeroService.Utility;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using MiNET.Entities;
using MiNET.Entities.Passive;
using System.Net;


namespace AeroService.Service.Impl
{
    public sealed class MonitorPointService(IHelperService<Sensor> helperService, AeroCommandService command, AppDbContext context, IHubContext<AeroHub> hub, ILogger<MonitorPointService> logger) : IMonitorPointService
    {

        public async Task<ResponseDto<IEnumerable<MonitorPointDto>>> GetAsync()
        {
            var dtos = await context.monitor_point
                .AsNoTracking()
                .Select(x => new MonitorPointDto
                {
                    // Base 
                    Uuid = x.uuid,
                    ComponentId = x.component_id,
                    Mac = x.module.hardware.mac,
                    HardwareName = x.module.hardware.name,
                    LocationId = x.location_id,
                    IsActive = x.is_active,

                    // extend_desc 
                    Name = x.name,
                    ModuleId = x.module_id,
                    ModuleDescription = x.module.model_desc,
                    InputNo = x.input_no,
                    InputMode = x.input_mode,
                    InputModeDescription = x.input_mode_desc,
                    Debounce = x.debounce,
                    HoldTime = x.holdtime,
                    LogFunction = x.log_function,
                    LogFunctionDescription = x.log_function_desc,
                    MonitorPointMode = x.monitor_point_mode,
                    MonitorPointModeDescription = x.monitor_point_mode_desc,
                    DelayEntry = x.delay_entry,
                    DelayExit = x.delay_exit,
                    IsMask = x.is_mask,

                })
                .ToArrayAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<MonitorPointDto>>(dtos);
        }


        public async Task<ResponseDto<IEnumerable<MonitorPointDto>>> GetByLocationAsync(short location)
        {
            var dtos = await context.monitor_point
                .AsNoTracking()
                .Where(x => x.location_id == location)
                .Select(x => new MonitorPointDto
                {
                    // Base 
                    Uuid = x.uuid,
                    ComponentId = x.component_id,
                    Mac = x.module.hardware.mac,
                    HardwareName = x.module.hardware.name,
                    LocationId = x.location_id,
                    IsActive = x.is_active,

                    // extend_desc 
                    Name = x.name,
                    ModuleId = x.module_id,
                    ModuleDescription = x.module.model_desc,
                    InputNo = x.input_no,
                    InputMode = x.input_mode,
                    InputModeDescription = x.input_mode_desc,
                    Debounce = x.debounce,
                    HoldTime = x.holdtime,
                    LogFunction = x.log_function,
                    LogFunctionDescription = x.log_function_desc,
                    MonitorPointMode = x.monitor_point_mode,
                    MonitorPointModeDescription = x.monitor_point_mode_desc,
                    DelayEntry = x.delay_entry,
                    DelayExit = x.delay_exit,
                    IsMask = x.is_mask,

                })
                .ToArrayAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<MonitorPointDto>>(dtos);
        }


        public async Task<ResponseDto<IEnumerable<short>>> GetAvailableIp(string mac, short sio)
        {
            var input = await context.module
                .AsNoTracking()
                .Where(mp => mp.component_id == sio && mp.hardware_mac == mac)
                .Select(mp => mp.n_input)
                .FirstOrDefaultAsync();

            var sensors = await context.sensor
                .AsNoTracking()
                .Where(x => x.module_id == sio && x.module.hardware_mac == mac)
                .Select(x => x.input_no)
                .ToArrayAsync();

            var rex = await context.request_exit
                .AsNoTracking()
                .Where(x => x.module_id == sio && x.module.hardware_mac == mac)
                .Select(x => x.input_no)
                .ToArrayAsync();

            var mp = await context.monitor_point
                .AsNoTracking()
                .Where(x => x.module_id == sio && x.module.hardware_mac == mac)
                .Select(x => x.input_no)
                .ToArrayAsync();


            var unavailable = sensors
                .Union(rex)
                .Union(mp)
                .Distinct()
                .ToList();


            List<short> all = Enumerable.Range(0, input - 3).Select(i => (short)i).ToList();
            List<short> av = all.Except(unavailable).ToList();
            return ResponseHelper.SuccessBuilder<IEnumerable<short>>(av);
        }


        public async void TriggerDeviceStatus(int ScpId, short first, string status)
        {
            string ScpMac = await helperService.GetMacFromIdAsync((short)ScpId);
            //GetOnlineStatus()
            var result = hub.Clients.All.SendAsync("MpStatus", ScpMac, first, status);
        }



        public async Task<ResponseDto<bool>> MaskAsync(MonitorPointDto dto, bool IsMask)
        {
            var input = await context.monitor_point
                .Where(x => x.component_id == dto.ComponentId && x.module.hardware_mac == dto.Mac)
                .FirstOrDefaultAsync();

            if (input is null) return ResponseHelper.NotFoundBuilder<bool>();

            var scpId = await context.hardware
                .AsNoTracking().Where(d => d.mac == dto.Mac)
                .Select(x => x.component_id)
                .FirstOrDefaultAsync();

            if (!command.MonitorPointMask((short)scpId, (short)input.component_id, IsMask ? 1 : 0))
            {
                return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(dto.Mac, Command.C306));
            }

            if (IsMask) input.is_mask = true;
            input.updated_date = DateTime.UtcNow;
            input.is_mask = IsMask ? true : false;
            context.monitor_point.Update(input);
            await context.SaveChangesAsync();
            return ResponseHelper.SuccessBuilder(true);
        }


        public async Task<ResponseDto<bool>> CreateAsync(MonitorPointDto dto)
        {
            
            var max = await context.system_setting
                .AsNoTracking()
                .Select(x => x.n_mp)
                .FirstOrDefaultAsync();

            var componentId = await helperService.GetLowestUnassignedNumberAsync<MonitorPoint>(context, max);
            if (componentId == -1) return ResponseHelper.ExceedLimit<bool>();

            var scpId = await helperService.GetIdFromMacAsync(dto.Mac);

            if (!command.InputPointSpecification(scpId, dto.ModuleId, dto.InputNo, dto.InputMode, dto.Debounce, dto.HoldTime))
            {
                return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(dto.Mac, Command.C110));
            }


            if (!command.MonitorPointConfiguration(scpId, dto.ModuleId, dto.InputNo, dto.LogFunction, dto.MonitorPointMode, dto.DelayEntry, dto.DelayExit, componentId))
            {
                return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(dto.Mac, Command.C113));
            }


            var entity = MapperHelper.DtoToMonitorPoint(dto, componentId,DateTime.UtcNow);
            await context.monitor_point.AddAsync(entity);
            await context.SaveChangesAsync();
            return ResponseHelper.SuccessBuilder(true);
        }


        public async Task<ResponseDto<bool>> DeleteAsync(short ComponentId)
        {
            var input = await context.monitor_point
                .Include(x => x.module)
                .Where(x => x.component_id == ComponentId)
                .FirstOrDefaultAsync();

            if (input is null) return ResponseHelper.NotFoundBuilder<bool>();

            var scpId = await helperService.GetIdFromMacAsync(input.module.hardware_mac);

            if (!command.MonitorPointConfiguration(scpId,-1, input.input_no, input.log_function, input.monitor_point_mode, input.delay_entry, input.delay_exit, ComponentId))
            {
                return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(input.module.hardware_mac, Command.C113));
            }

            context.monitor_point.Remove(input);
            await context.SaveChangesAsync();
            return ResponseHelper.SuccessBuilder(true);
        }

        public async Task<ResponseDto<MonitorPointDto>> UpdateAsync(MonitorPointDto dto)
        {
            var input = await context.monitor_point
                .Where(x => x.module.hardware_mac == dto.Mac && x.id == dto.ComponentId)
                .FirstOrDefaultAsync();

            if (input is null) return ResponseHelper.NotFoundBuilder<MonitorPointDto>();

            var scpId = await helperService.GetIdFromMacAsync(dto.Mac);
            if (!command.InputPointSpecification(scpId, dto.ModuleId, dto.InputNo, dto.InputMode, dto.Debounce, dto.HoldTime))
            {
                return ResponseHelper.UnsuccessBuilder<MonitorPointDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(dto.Mac, Command.C110));
            }

            if (!command.MonitorPointConfiguration(scpId, dto.ModuleId, dto.InputNo, dto.LogFunction, dto.MonitorPointMode, dto.DelayEntry, dto.DelayExit, dto.ComponentId))
            {
                return ResponseHelper.UnsuccessBuilder<MonitorPointDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(dto.Mac, Command.C113));
            }

            MapperHelper.UpdateMonitorPoint(input,dto);

            context.monitor_point.Update(input);
            await context.SaveChangesAsync();
            return ResponseHelper.SuccessBuilder<MonitorPointDto>(dto);
        }

        public async Task<ResponseDto<bool>> GetStatusAsync(string mac, short component)
        {
            var id = await helperService.GetIdFromMacAsync(mac);
            if(id == 0) return ResponseHelper.NotFoundBuilder<bool>();
            if (!command.GetMpStatus(id, component, 1))
            {
                return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.COMMAND_UNSUCCESS,MessageBuilder.Unsuccess(mac,Command.C405));
            }
            return ResponseHelper.SuccessBuilder<bool>(true);
        }

        public async Task<ResponseDto<IEnumerable<ModeDto>>> GetModeAsync(int param)
        {
            switch (param)
            {
                case 0:
                    var dtos = await context.input_mode.AsNoTracking().Select(x => new ModeDto
                    {
                        Name = x.name,
                        Value = x.value,
                        Description = x.description,
                    }).ToArrayAsync();
                    return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dtos);
                case 1:
                    var d = await context.monitor_point_mode.AsNoTracking().Select(x => new ModeDto
                    {
                        Name = x.name,
                        Value = x.value,
                        Description = x.description,
                    }).ToArrayAsync();
                    return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(d);
                default:
                    return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>([]);

            }

        }

        public async Task<ResponseDto<MonitorPointDto>> GetByIdAndMacAsync(string mac, short Id)
        {
            var dto = await context.monitor_point
                .AsNoTracking()
                .Where(x => x.module.hardware_mac == mac && x.component_id == Id)
                .Select(x => new MonitorPointDto
                {
                    // Base 
                    Uuid = x.uuid,
                    ComponentId = x.component_id,
                    Mac = x.module.hardware.mac,
                    HardwareName = x.module.hardware.name,
                    LocationId = x.location_id,
                    IsActive = x.is_active,

                    // extend_desc 
                    Name = x.name,
                    ModuleId = x.module_id,
                    ModuleDescription = x.module.model_desc,
                    InputNo = x.input_no,
                    InputMode = x.input_mode,
                    InputModeDescription = x.input_mode_desc,
                    Debounce = x.debounce,
                    HoldTime = x.holdtime,
                    LogFunction = x.log_function,
                    LogFunctionDescription = x.log_function_desc,
                    MonitorPointMode = x.monitor_point_mode,
                    MonitorPointModeDescription = x.monitor_point_mode_desc,
                    DelayEntry = x.delay_entry,
                    DelayExit = x.delay_exit,
                    IsMask = x.is_mask,

                })
                .FirstOrDefaultAsync();

            if (dto is null) return ResponseHelper.NotFoundBuilder<MonitorPointDto>();

            return ResponseHelper.SuccessBuilder(dto);
        }

        public async Task<ResponseDto<IEnumerable<MonitorPointDto>>> GetByIdAndMacAsync(string mac)
        {
            var dtos = await context.monitor_point
                .AsNoTracking()
                .Where(x => x.module.hardware_mac == mac)
                .Select(x => new MonitorPointDto
                {
                    // Base 
                    Uuid = x.uuid,
                    ComponentId = x.component_id,
                    Mac = x.module.hardware.mac,
                    HardwareName = x.module.hardware.name,
                    LocationId = x.location_id,
                    IsActive = x.is_active,

                    // extend_desc 
                    Name = x.name,
                    ModuleId = x.module_id,
                    ModuleDescription = x.module.model_desc,
                    InputNo = x.input_no,
                    InputMode = x.input_mode,
                    InputModeDescription = x.input_mode_desc,
                    Debounce = x.debounce,
                    HoldTime = x.holdtime,
                    LogFunction = x.log_function,
                    LogFunctionDescription = x.log_function_desc,
                    MonitorPointMode = x.monitor_point_mode,
                    MonitorPointModeDescription = x.monitor_point_mode_desc,
                    DelayEntry = x.delay_entry,
                    DelayExit = x.delay_exit,
                    IsMask = x.is_mask,

                })
                .ToArrayAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<MonitorPointDto>>(dtos);
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

        public async Task<ResponseDto<IEnumerable<ModeDto>>> GetLogFunctionAsync()
        {
            var dtos = await context.monitor_point_log_function
                .AsNoTracking()
                .Select(x => new ModeDto 
                {
                    Name = x.name,
                    Value = x.value,
                    Description = x.description,
                })
                .ToArrayAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dtos);
        }
    }
}
