using AeroService.Constant;
using AeroService.Constants;
using AeroService.Data;
using AeroService.DTO;
using AeroService.DTO.Module;
using AeroService.DTO.Reader;
using AeroService.DTO.Output;
using AeroService.Entity;
using AeroService.Helpers;
using AeroService.Hubs;
using AeroService.Mapper;
using AeroService.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using MiNET.Entities;
using System.Collections.Generic;
using System.Net;
using AeroService.Aero.CommandService.Impl;
using AeroService.Aero.CommandService;
using HID.Aero.ScpdNet.Wrapper;
using AeroService.Enums;
using Microsoft.AspNetCore.Http.HttpResults;
using MiNET.Items;
using System.Collections;
using System.Text;
using AeroService.DTO.ControlPoint;
using AeroService.DTO.MonitorPoint;
using AeroService.DTO.RequestExit;
using AeroService.DTO.Sensor;
using AeroService.DTO.Strike;

namespace AeroService.Service.Impl
{
    public class ModuleService(AppDbContext context, AeroCommandService command, IHubContext<AeroHub> hub, IHelperService<Module> helperService, ILogger<ModuleService> logger) : IModuleService
    {

        public async Task<ResponseDto<IEnumerable<ModuleDto>>> GetAsync()
        {
            var dtos = await context.module
                .AsNoTracking()
                .Select(d => new ModuleDto
                {
                    // Base 
                    Uuid = d.uuid,
                    ComponentId = d.component_id,
                    Mac = d.hardware_mac,
                    HardwareName = d.hardware.name,
                    LocationId = d.location_id,
                    IsActive = d.is_active,

                    // extend_desc
                    Model = d.model,
                    ModelDescription = d.model_desc,
                    Revision = d.revision,
                    SerialNumber = d.serial_number,
                    nHardwareId = d.n_hardware_id,
                    nHardwareIdDescription = d.n_hardware_id_desc,
                    nHardwareRev = d.n_hardware_rev,
                    nProductId = d.n_product_id,
                    nProductVer = d.n_product_ver,
                    nEncConfig = d.n_enc_config,
                    nEncConfigDescription = d.n_enc_config_desc,
                    nEncKeyStatus = d.n_enc_key_status,
                    nEncKeyStatusDescription = d.n_enc_key_status_desc,
                    Readers = d.readers == null ? null : d.readers.Select(x => new ReaderDto
                    {
                        // Base
                        Uuid = x.uuid,
                        ComponentId = x.component_id,
                        Mac = x.module.hardware_mac,
                        HardwareName = x.module.hardware.name,
                        LocationId = x.location_id,
                        IsActive = x.is_active,

                        // extend_desc
                        ModuleId = x.module_id,
                        ReaderNo = x.reader_no,
                        DataFormat = x.data_format,
                        KeypadMode = x.keypad_mode,
                        LedDriveMode = x.led_drive_mode,
                        OsdpFlag = x.osdp_flag,
                        OsdpAddress = x.osdp_address,
                        OsdpBaudrate = x.osdp_baudrate,
                        OsdpDiscover = x.osdp_discover,
                        OsdpSecureChannel = x.osdp_secure_channel,
                        OsdpTracing = x.osdp_tracing,
                    }).ToList(),
                    Sensors = d.sensors == null ? null : d.sensors.Select(x => new SensorDto
                    {

                        // Base 
                        Uuid = x.uuid,
                        ComponentId = x.component_id,
                        Mac = x.module.hardware_mac,
                        HardwareName = x.module.hardware.name,
                        LocationId = x.location_id,
                        IsActive = x.is_active,

                        // extend_desc
                        ModuleId = x.module_id,
                        InputNo = x.input_no,
                        InputMode = x.input_mode,
                        Debounce = x.debounce,
                        HoldTime = x.holdtime,
                        DcHeld = x.dc_held,

                    }).ToList(),
                    Strikes = d.strikes == null ? null : d.strikes.Select(x => new StrikeDto
                    {
                        // Base 
                        Uuid = x.uuid,
                        ComponentId = x.component_id,
                        Mac = x.module.hardware_mac,
                        HardwareName = x.module.hardware.name,
                        LocationId = x.location_id,
                        IsActive = x.is_active,

                        // extend_desc
                        ModuleId = x.module_id,
                        OutputNo = x.output_no,
                        RelayMode = x.relay_mode,
                        OfflineMode = x.offline_mode,
                        StrkMax = x.strike_max,
                        StrkMin = x.strike_min,
                        StrkMode = x.strike_mode,
                    }).ToList(),
                    RequestExits = d.request_exits == null ? null : d.request_exits.Select(x => new RequestExitDto
                    {
                        // Base
                        Uuid = x.uuid,
                        ComponentId = x.component_id,
                        Mac = x.module.hardware_mac,
                        HardwareName = x.module.hardware.name,
                        LocationId = x.location_id,
                        IsActive = x.is_active,

                        // extend_desc
                        ModuleId = x.module_id,
                        InputNo = x.input_no,
                        InputMode = x.input_mode,
                        Debounce = x.debounce,
                        HoldTime = x.holdtime,
                        MaskTimeZone = x.mask_timezone,
                    }).ToList(),
                    MonitorPoints = d.monitor_points == null ? null : d.monitor_points.Select(x => new MonitorPointDto
                    {
                        // Base 
                        Uuid = x.uuid,
                        ComponentId = x.component_id,
                        Mac = x.module.hardware_mac,
                        HardwareName = x.module.hardware.name,
                        LocationId = x.location_id,
                        IsActive = x.is_active,

                        // extend_desc 
                        Name = x.name,
                        ModuleId = x.module_id,
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

                    }).ToList(),
                    ControlPoints = d.control_points == null ? null : d.control_points.Select(x => new ControlPointDto
                    {
                        // Base
                        Uuid = x.uuid,
                        ComponentId = x.component_id,
                        Mac = x.module.hardware_mac,
                        HardwareName = x.module.hardware.name,
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
                    }).ToList(),
                    Address = d.address,
                    Port = d.port,
                    nInput = d.n_input,
                    nOutput = d.n_output,
                    nReader = d.n_reader,
                    Msp1No = d.msp1_no,
                    BaudRate = d.baudrate,
                    nProtocol = d.n_protocol,
                    nDialect = d.n_dialect,
                }).ToArrayAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<ModuleDto>>(dtos);
        }

        public void GetSioStatus(int ScpId, int SioNo)
        {
            command.GetSioStatus((short)ScpId, (short)SioNo);
        }

        public void TriggerDeviceStatus(int ScpId, short SioNo, string Status, string Tamper, string Ac, string Batt)
        {
            string mac = helperService.GetMacFromId((short)ScpId);
            //GetOnlineStatus()
            hub.Clients.All.SendAsync("SioStatus", mac, SioNo, Status, Tamper, Ac, Batt);
        }

        public async Task<ResponseDto<bool>> GetStatusAsync(string mac, short Id)
        {
            var entity = await context.module
                .AsNoTracking()
                .Where(x => x.hardware_mac == mac && x.component_id == Id)
                .FirstOrDefaultAsync();

            if (entity is null) return ResponseHelper.NotFoundBuilder<bool>();
            int id = await context.hardware.Where(d => d.mac == mac).Select(d => d.component_id).FirstOrDefaultAsync();
            if (!command.GetSioStatus((short)id, Id))
            {
                return ResponseHelper.UnsuccessBuilderWithString<bool>(ResponseMessage.COMMAND_UNSUCCESS,MessageBuilder.Unsuccess(mac,Command.C404));
            }
            return ResponseHelper.SuccessBuilder<bool>(true);

        }

        public async Task<ResponseDto<IEnumerable<ModuleDto>>> GetByMacAsync(string mac)
        {
            var dtos = await context.module
                .AsNoTracking()
                .Where(x => x.hardware_mac == mac)
                .Select(d => new ModuleDto
                {
                    // Base 
                    Uuid = d.uuid,
                    ComponentId = d.component_id,
                    Mac = d.hardware_mac,
                    HardwareName = d.hardware.name,
                    LocationId = d.location_id,
                    IsActive = d.is_active,

                    // extend_desc
                    Model = d.model,
                    ModelDescription = d.model_desc,
                    Revision = d.revision,
                    SerialNumber = d.serial_number,
                    nHardwareId = d.n_hardware_id,
                    nHardwareIdDescription = d.n_hardware_id_desc,
                    nHardwareRev = d.n_hardware_rev,
                    nProductId = d.n_product_id,
                    nProductVer = d.n_product_ver,
                    nEncConfig = d.n_enc_config,
                    nEncConfigDescription = d.n_enc_config_desc,
                    nEncKeyStatus = d.n_enc_key_status,
                    nEncKeyStatusDescription = d.n_enc_key_status_desc,
                    Readers = d.readers == null ? null : d.readers.Select(x => new ReaderDto
                    {
                        // Base
                        Uuid = x.uuid,
                        ComponentId = x.component_id,
                        Mac = x.module.hardware_mac,
                        HardwareName = x.module.hardware.name,
                        LocationId = x.location_id,
                        IsActive = x.is_active,

                        // extend_desc
                        ModuleId = x.module_id,
                        ReaderNo = x.reader_no,
                        DataFormat = x.data_format,
                        KeypadMode = x.keypad_mode,
                        LedDriveMode = x.led_drive_mode,
                        OsdpFlag = x.osdp_flag,
                        OsdpAddress = x.osdp_address,
                        OsdpBaudrate = x.osdp_baudrate,
                        OsdpDiscover = x.osdp_discover,
                        OsdpSecureChannel = x.osdp_secure_channel,
                        OsdpTracing = x.osdp_tracing,
                    }).ToList(),
                    Sensors = d.sensors == null ? null : d.sensors.Select(x => new SensorDto
                    {

                        // Base 
                        Uuid = x.uuid,
                        ComponentId = x.component_id,
                        Mac = x.module.hardware_mac,
                        HardwareName = x.module.hardware.name,
                        LocationId = x.location_id,
                        IsActive = x.is_active,

                        // extend_desc
                        ModuleId = x.module_id,
                        InputNo = x.input_no,
                        InputMode = x.input_mode,
                        Debounce = x.debounce,
                        HoldTime = x.holdtime,
                        DcHeld = x.dc_held,

                    }).ToList(),
                    Strikes = d.strikes == null ? null : d.strikes.Select(x => new StrikeDto
                    {
                        // Base 
                        Uuid = x.uuid,
                        ComponentId = x.component_id,
                        Mac = x.module.hardware_mac,
                        HardwareName = x.module.hardware.name,
                        LocationId = x.location_id,
                        IsActive = x.is_active,

                        // extend_desc
                        ModuleId = x.module_id,
                        OutputNo = x.output_no,
                        RelayMode = x.relay_mode,
                        OfflineMode = x.offline_mode,
                        StrkMax = x.strike_max,
                        StrkMin = x.strike_min,
                        StrkMode = x.strike_mode,
                    }).ToList(),
                    RequestExits = d.request_exits == null ? null : d.request_exits.Select(x => new RequestExitDto
                    {
                        // Base
                        Uuid = x.uuid,
                        ComponentId = x.component_id,
                        Mac = x.module.hardware_mac,
                        HardwareName = x.module.hardware.name,
                        LocationId = x.location_id,
                        IsActive = x.is_active,

                        // extend_desc
                        ModuleId = x.module_id,
                        InputNo = x.input_no,
                        InputMode = x.input_mode,
                        Debounce = x.debounce,
                        HoldTime = x.holdtime,
                        MaskTimeZone = x.mask_timezone,
                    }).ToList(),
                    MonitorPoints = d.monitor_points == null ? null : d.monitor_points.Select(x => new MonitorPointDto
                    {
                        // Base 
                        Uuid = x.uuid,
                        ComponentId = x.component_id,
                        Mac = x.module.hardware_mac,
                        HardwareName = x.module.hardware.name,
                        LocationId = x.location_id,
                        IsActive = x.is_active,

                        // extend_desc 
                        Name = x.name,
                        ModuleId = x.module_id,
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

                    }).ToList(),
                    ControlPoints = d.control_points == null ? null : d.control_points.Select(x => new ControlPointDto
                    {
                        // Base
                        Uuid = x.uuid,
                        ComponentId = x.component_id,
                        Mac = x.module.hardware_mac,
                        HardwareName = x.module.hardware.name,
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
                    }).ToList(),
                    Address = d.address,
                    Port = d.port,
                    nInput = d.n_input,
                    nOutput = d.n_output,
                    nReader = d.n_reader,
                    Msp1No = d.msp1_no,
                    BaudRate = d.baudrate,
                    nProtocol = d.n_protocol,
                    nDialect = d.n_dialect,
                })
                .ToArrayAsync();


            return ResponseHelper.SuccessBuilder<IEnumerable<ModuleDto>>(dtos);
        }

        public async Task<ResponseDto<ModuleDto>> CreateAsync(ModuleDto dto)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseDto<ModuleDto>> DeleteAsync(string mac, short component)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseDto<ModuleDto>> UpdateAsync(ModuleDto dto)
        {
            throw new NotImplementedException();
        }


        public async Task<ResponseDto<IEnumerable<ModeDto>>> GetModeAsync(int param)
        {
            throw new NotImplementedException();
        }


        public async Task<ResponseDto<ModuleDto>> GetByComponentAsync(string mac, short component)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseDto<IEnumerable<ModuleDto>>> GetByLocationAsync(short location)
        {
            var dtos = await context.module
                .AsNoTracking()
                .Select(d => new ModuleDto
                {
                    // Base 
                    Uuid = d.uuid,
                    ComponentId = d.component_id,
                    Mac = d.hardware_mac,
                    HardwareName = d.hardware.name,
                    LocationId = d.location_id,
                    IsActive = d.is_active,

                    // extend_desc
                    Model = d.model,
                    ModelDescription = d.model_desc,
                    Revision = d.revision,
                    SerialNumber = d.serial_number,
                    nHardwareId = d.n_hardware_id,
                    nHardwareIdDescription = d.n_hardware_id_desc,
                    nHardwareRev = d.n_hardware_rev,
                    nProductId = d.n_product_id,
                    nProductVer = d.n_product_ver,
                    nEncConfig = d.n_enc_config,
                    nEncConfigDescription = d.n_enc_config_desc,
                    nEncKeyStatus = d.n_enc_key_status,
                    nEncKeyStatusDescription = d.n_enc_key_status_desc,
                    Readers = d.readers == null ? null : d.readers.Select(x => new ReaderDto
                    {
                        // Base
                        Uuid = x.uuid,
                        ComponentId = x.component_id,
                        Mac = x.module.hardware_mac,
                        HardwareName = x.module.hardware.name,
                        LocationId = x.location_id,
                        IsActive = x.is_active,

                        // extend_desc
                        ModuleId = x.module_id,
                        ReaderNo = x.reader_no,
                        DataFormat = x.data_format,
                        KeypadMode = x.keypad_mode,
                        LedDriveMode = x.led_drive_mode,
                        OsdpFlag = x.osdp_flag,
                        OsdpAddress = x.osdp_address,
                        OsdpBaudrate = x.osdp_baudrate,
                        OsdpDiscover = x.osdp_discover,
                        OsdpSecureChannel = x.osdp_secure_channel,
                        OsdpTracing = x.osdp_tracing,
                    }).ToList(),
                    Sensors = d.sensors == null ? null : d.sensors.Select(x => new SensorDto
                    {

                        // Base 
                        Uuid = x.uuid,
                        ComponentId = x.component_id,
                        Mac = x.module.hardware_mac,
                        HardwareName = x.module.hardware.name,
                        LocationId = x.location_id,
                        IsActive = x.is_active,

                        // extend_desc
                        ModuleId = x.module_id,
                        InputNo = x.input_no,
                        InputMode = x.input_mode,
                        Debounce = x.debounce,
                        HoldTime = x.holdtime,
                        DcHeld = x.dc_held,

                    }).ToList(),
                    Strikes = d.strikes == null ? null : d.strikes.Select(x => new StrikeDto
                    {
                        // Base 
                        Uuid = x.uuid,
                        ComponentId = x.component_id,
                        Mac = x.module.hardware_mac,
                        HardwareName = x.module.hardware.name,
                        LocationId = x.location_id,
                        IsActive = x.is_active,

                        // extend_desc
                        ModuleId = x.module_id,
                        OutputNo = x.output_no,
                        RelayMode = x.relay_mode,
                        OfflineMode = x.offline_mode,
                        StrkMax = x.strike_max,
                        StrkMin = x.strike_min,
                        StrkMode = x.strike_mode,
                    }).ToList(),
                    RequestExits = d.request_exits == null ? null : d.request_exits.Select(x => new RequestExitDto
                    {
                        // Base
                        Uuid = x.uuid,
                        ComponentId = x.component_id,
                        Mac = x.module.hardware_mac,
                        HardwareName = x.module.hardware.name,
                        LocationId = x.location_id,
                        IsActive = x.is_active,

                        // extend_desc
                        ModuleId = x.module_id,
                        InputNo = x.input_no,
                        InputMode = x.input_mode,
                        Debounce = x.debounce,
                        HoldTime = x.holdtime,
                        MaskTimeZone = x.mask_timezone,
                    }).ToList(),
                    MonitorPoints = d.monitor_points == null ? null : d.monitor_points.Select(x => new MonitorPointDto
                    {
                        // Base 
                        Uuid = x.uuid,
                        ComponentId = x.component_id,
                        Mac = x.module.hardware_mac,
                        HardwareName = x.module.hardware.name,
                        LocationId = x.location_id,
                        IsActive = x.is_active,

                        // extend_desc 
                        Name = x.name,
                        ModuleId = x.module_id,
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

                    }).ToList(),
                    ControlPoints = d.control_points == null ? null : d.control_points.Select(x => new ControlPointDto
                    {
                        // Base
                        Uuid = x.uuid,
                        ComponentId = x.component_id,
                        Mac = x.module.hardware_mac,
                        HardwareName = x.module.hardware.name,
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
                    }).ToList(),
                    Address = d.address,
                    Port = d.port,
                    nInput = d.n_input,
                    nOutput = d.n_output,
                    nReader = d.n_reader,
                    Msp1No = d.msp1_no,
                    BaudRate = d.baudrate,
                    nProtocol = d.n_protocol,
                    nDialect = d.n_dialect,
                }).ToArrayAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<ModuleDto>>(dtos);
        }

        public async Task<ResponseDto<IEnumerable<ModeDto>>> GetBaudrateAsync()
        {
            var dtos = await context.module_baudrate
                .AsNoTracking()
                .Select(x => new ModeDto 
                {
                    Value = x.value,
                    Name = x.name,
                    Description = x.description,
                })
                .ToArrayAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dtos);
        }

        public async Task<ResponseDto<IEnumerable<ModeDto>>> GetProtocolAsync()
        {
            var dtos = await context.module_protocol
                .AsNoTracking()
                .Select(x => new ModeDto
                {
                    Value = x.value,
                    Name = x.name,
                    Description = x.description,
                })
                .ToArrayAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dtos);
        }

        public async Task HandleFoundModuleAsync(SCPReplyMessage message)
        {
            if(message.tran.s_comm.comm_sts == 3)
            {
                if (await context.module.AsNoTracking().AnyAsync(x => x.serial_number.Equals(message.tran.s_comm.ser_num.ToString()))) return;
                var id = await helperService.GetLowestUnassignedNumberAsync<Module>(context, 16);
                if (id == 0) return;
                var mac = await helperService.GetMacFromIdAsync((short)message.SCPId);
                var module = new Module
                {
                    // Base 
                    component_id = id,
                    hardware_mac = mac,
                    location_id = await context.hardware.AsNoTracking().Where(x => x.mac.Equals(mac)).OrderBy(x => x.id).Select(x => x.location_id).FirstOrDefaultAsync(),
                    is_active = true,
                    created_date = DateTime.UtcNow,
                    updated_date = DateTime.UtcNow,

                    // extend_desc
                    model = message.tran.s_comm.model,
                    model_desc = Enums.Model.AeroX1100.ToString(),
                    revision = ((int)message.tran.s_comm.revision).ToString(),
                    address = -1,
                    port = 3,
                    n_input = (short)InputComponents.HIDAeroX1100,
                    n_output = (short)OutputComponents.HIDAeroX1100,
                    n_reader = (short)ReaderComponents.HIDAeroX1100,
                    msp1_no = 0,
                    baudrate = -1,
                    n_protocol = 0,
                    n_dialect = 0,

                };
            }else if(message.tran.s_comm.comm_sts == 2)
            {

            }
            
        }
    }
}
