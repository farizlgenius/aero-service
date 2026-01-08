using HIDAeroService.Aero.CommandService;
using HIDAeroService.Aero.CommandService.Impl;
using HIDAeroService.AeroLibrary;
using HIDAeroService.Constant;
using HIDAeroService.Constants;
using HIDAeroService.Data;
using HIDAeroService.DTO;
using HIDAeroService.DTO.Acr;
using HIDAeroService.DTO.Reader;
using HIDAeroService.DTO.RequestExit;
using HIDAeroService.DTO.Sensor;
using HIDAeroService.DTO.Strike;
using HIDAeroService.Entity;
using HIDAeroService.Helpers;
using HIDAeroService.Hubs;
using HIDAeroService.Mapper;
using HIDAeroService.Utility;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;


namespace HIDAeroService.Service.Impl
{
    public class DoorService(AppDbContext context, AeroMessage read, AeroCommandService command, IHelperService<Door> helperService, IHubContext<AeroHub> hub) : IDoorService
    {
        public async Task<ResponseDto<IEnumerable<DoorDto>>> GetAsync()
        {
            var dtos = await context.door
                .AsNoTracking()
                .Select(x => new DoorDto
                {
                    // Base 
                    Uuid = x.uuid,
                    ComponentId = x.component_id,
                    Mac = x.hardware_mac,
                    LocationId = x.location_id,
                    IsActive = x.is_active,

                    // extend_desc
                    Name = x.name,
                    AccessConfig = x.access_config,
                    PairDoorNo = x.pair_door_no,

                    // Reader
                    Readers = x.readers == null ? new List<ReaderDto>() : x.readers
                    .Select(x => new ReaderDto
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
                    })
                    .ToList(),
                    ReaderOutConfiguration = x.reader_out_config,

                    // Strike
                    StrkComponentId = x.strike_id,
                    Strk = x.strike == null ? null : new StrikeDto
                    {
                        // Base 
                        Uuid = x.strike.uuid,
                        ComponentId = x.strike.component_id,
                        Mac = x.strike.module.hardware_mac,
                        HardwareName = x.strike.module.hardware.name,
                        LocationId = x.location_id,
                        IsActive = x.is_active,

                        // extend_desc
                        ModuleId = x.strike.module_id,
                        OutputNo = x.strike.output_no,
                        RelayMode = x.strike.relay_mode,
                        OfflineMode = x.strike.offline_mode,
                        StrkMax = x.strike.strike_max,
                        StrkMin = x.strike.strike_min,
                        StrkMode = x.strike.strike_mode,
                    },

                    // sensor
                    SensorComponentId = x.sensor_id,
                    Sensor = x.sensor == null ? null : new SensorDto
                    {

                        // Base 
                        Uuid = x.sensor.uuid,
                        ComponentId = x.sensor.component_id,
                        Mac = x.sensor.module.hardware_mac,
                        HardwareName = x.sensor.module.hardware.name,
                        LocationId = x.sensor.location_id,
                        IsActive = x.sensor.is_active,

                        // extend_desc
                        ModuleId = x.sensor.module_id,
                        InputNo = x.sensor.input_no,
                        InputMode = x.sensor.input_mode,
                        Debounce = x.sensor.debounce,
                        HoldTime = x.sensor.holdtime,
                        DcHeld = x.sensor.dc_held,

                    },

                    // Request Exit
                    RequestExits = x.request_exits == null ? new List<RequestExitDto>() : x.request_exits
                    .Select(x => new RequestExitDto
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
                    })
                    .ToList(),


                    CardFormat = x.card_format,
                    AntiPassbackMode = x.antipassback_mode,
                    AntiPassBackIn = (short)(x.antipassback_in == null ? 0 : (short)x.antipassback_in),
                    AntiPassBackOut = (short)(x.antipassback_out == null ? 0 : (short)x.antipassback_out),
                    SpareTags = x.spare_tag,
                    AccessControlFlags = x.access_control_flag,
                    Mode = x.mode,
                    ModeDesc = x.mode_desc,
                    OfflineMode = x.offline_mode,
                    OfflineModeDesc = x.offline_mode_desc,
                    DefaultMode = x.default_mode,
                    DefaultModeDesc = x.default_mode_desc,
                    DefaultLEDMode = x.default_led_mode,
                    PreAlarm = x.pre_alarm,
                    AntiPassbackDelay = x.antipassback_delay,
                    StrkT2 = x.strike_t2,
                    DcHeld2 = x.dc_held2,
                    StrkFollowPulse = x.strike_follow_pulse,
                    StrkFollowDelay = x.strike_follow_delay,
                    nExtFeatureType = x.n_ext_feature_type,
                    IlPBSio = x.i_lpb_sio,
                    IlPBNumber = x.i_lpb_number,
                    IlPBLongPress = x.i_lpb_long_press,
                    IlPBOutSio = x.i_lpb_out_sio,
                    IlPBOutNum = x.i_lpb_out_num,
                    DfOfFilterTime = x.df_filter_time,
                    MaskForceOpen = x.is_force_mask,
                    MaskHeldOpen = x.is_held_mask,

                })
                .ToArrayAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<DoorDto>>(dtos);
        }
        public async Task<ResponseDto<IEnumerable<DoorDto>>> GetByLocationIdAsync(short location)
        {
            var dtos = await context.door
                .AsNoTracking()
                .Where(x => x.location_id == location)
                .Select(x => new DoorDto
                {
                    // Base 
                    Uuid = x.uuid,
                    ComponentId = x.component_id,
                    Mac = x.hardware_mac,
                    LocationId = x.location_id,
                    IsActive = x.is_active,

                    // extend_desc
                    Name = x.name,
                    AccessConfig = x.access_config,
                    PairDoorNo = x.pair_door_no,

                    // Reader
                    Readers = x.readers == null ? new List<ReaderDto>() : x.readers
                    .Select(x => new ReaderDto
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
                    })
                    .ToList(),
                    ReaderOutConfiguration = x.reader_out_config,

                    // Strike
                    StrkComponentId = x.strike_id,
                    Strk = x.strike == null ? null : new StrikeDto
                    {
                        // Base 
                        Uuid = x.strike.uuid,
                        ComponentId = x.strike.component_id,
                        Mac = x.strike.module.hardware_mac,
                        HardwareName = x.strike.module.hardware.name,
                        LocationId = x.location_id,
                        IsActive = x.is_active,

                        // extend_desc
                        ModuleId = x.strike.module_id,
                        OutputNo = x.strike.output_no,
                        RelayMode = x.strike.relay_mode,
                        OfflineMode = x.strike.offline_mode,
                        StrkMax = x.strike.strike_max,
                        StrkMin = x.strike.strike_min,
                        StrkMode = x.strike.strike_mode,
                    },

                    // sensor
                    SensorComponentId = x.sensor_id,
                    Sensor = x.sensor == null ? null : new SensorDto
                    {

                        // Base 
                        Uuid = x.sensor.uuid,
                        ComponentId = x.sensor.component_id,
                        Mac = x.sensor.module.hardware_mac,
                        HardwareName = x.sensor.module.hardware.name,
                        LocationId = x.sensor.location_id,
                        IsActive = x.sensor.is_active,

                        // extend_desc
                        ModuleId = x.sensor.module_id,
                        InputNo = x.sensor.input_no,
                        InputMode = x.sensor.input_mode,
                        Debounce = x.sensor.debounce,
                        HoldTime = x.sensor.holdtime,
                        DcHeld = x.sensor.dc_held,

                    },

                    // Request Exit
                    RequestExits = x.request_exits == null ? new List<RequestExitDto>() : x.request_exits
                    .Select(x => new RequestExitDto
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
                    })
                    .ToList(),


                    CardFormat = x.card_format,
                    AntiPassbackMode = x.antipassback_mode,
                    AntiPassBackIn = (short)(x.antipassback_in == null ? 0 : (short)x.antipassback_in),
                    AntiPassBackOut = (short)(x.antipassback_out == null ? 0 : (short)x.antipassback_out),
                    SpareTags = x.spare_tag,
                    AccessControlFlags = x.access_control_flag,
                    Mode = x.mode,
                    ModeDesc = x.mode_desc,
                    OfflineMode = x.offline_mode,
                    OfflineModeDesc = x.offline_mode_desc,
                    DefaultMode = x.default_mode,
                    DefaultModeDesc = x.default_mode_desc,
                    DefaultLEDMode = x.default_led_mode,
                    PreAlarm = x.pre_alarm,
                    AntiPassbackDelay = x.antipassback_delay,
                    StrkT2 = x.strike_t2,
                    DcHeld2 = x.dc_held2,
                    StrkFollowPulse = x.strike_follow_pulse,
                    StrkFollowDelay = x.strike_follow_delay,
                    nExtFeatureType = x.n_ext_feature_type,
                    IlPBSio = x.i_lpb_sio,
                    IlPBNumber = x.i_lpb_number,
                    IlPBLongPress = x.i_lpb_long_press,
                    IlPBOutSio = x.i_lpb_out_sio,
                    IlPBOutNum = x.i_lpb_out_num,
                    DfOfFilterTime = x.df_filter_time,
                    MaskForceOpen = x.is_force_mask,
                    MaskHeldOpen = x.is_held_mask,

                })
                .ToArrayAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<DoorDto>>(dtos);
        }

        public async Task<ResponseDto<IEnumerable<DoorDto>>> GetByMacAsync(string mac)
        {
            var dtos = await context.door
                .AsNoTracking()
                .Where(x => x.hardware_mac == mac)
                .Select(x => new DoorDto
                {
                    // Base 
                    Uuid = x.uuid,
                    ComponentId = x.component_id,
                    Mac = x.hardware_mac,
                    LocationId = x.location_id,
                    IsActive = x.is_active,

                    // extend_desc
                    Name = x.name,
                    AccessConfig = x.access_config,
                    PairDoorNo = x.pair_door_no,

                    // Reader
                    Readers = x.readers == null ? new List<ReaderDto>() : x.readers
                    .Select(x => new ReaderDto
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
                    })
                    .ToList(),
                    ReaderOutConfiguration = x.reader_out_config,

                    // Strike
                    StrkComponentId = x.strike_id,
                    Strk = x.strike == null ? null : new StrikeDto
                    {
                        // Base 
                        Uuid = x.strike.uuid,
                        ComponentId = x.strike.component_id,
                        Mac = x.strike.module.hardware_mac,
                        HardwareName = x.strike.module.hardware.name,
                        LocationId = x.location_id,
                        IsActive = x.is_active,

                        // extend_desc
                        ModuleId = x.strike.module_id,
                        OutputNo = x.strike.output_no,
                        RelayMode = x.strike.relay_mode,
                        OfflineMode = x.strike.offline_mode,
                        StrkMax = x.strike.strike_max,
                        StrkMin = x.strike.strike_min,
                        StrkMode = x.strike.strike_mode,
                    },

                    // sensor
                    SensorComponentId = x.sensor_id,
                    Sensor = x.sensor == null ? null : new SensorDto
                    {

                        // Base 
                        Uuid = x.sensor.uuid,
                        ComponentId = x.sensor.component_id,
                        Mac = x.sensor.module.hardware_mac,
                        HardwareName = x.sensor.module.hardware.name,
                        LocationId = x.sensor.location_id,
                        IsActive = x.sensor.is_active,

                        // extend_desc
                        ModuleId = x.sensor.module_id,
                        InputNo = x.sensor.input_no,
                        InputMode = x.sensor.input_mode,
                        Debounce = x.sensor.debounce,
                        HoldTime = x.sensor.holdtime,
                        DcHeld = x.sensor.dc_held,

                    },

                    // Request Exit
                    RequestExits = x.request_exits == null ? new List<RequestExitDto>() : x.request_exits
                    .Select(x => new RequestExitDto
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
                    })
                    .ToList(),


                    CardFormat = x.card_format,
                    AntiPassbackMode = x.antipassback_mode,
                    AntiPassBackIn = (short)(x.antipassback_in == null ? 0 : (short)x.antipassback_in),
                    AntiPassBackOut = (short)(x.antipassback_out == null ? 0 : (short)x.antipassback_out),
                    SpareTags = x.spare_tag,
                    AccessControlFlags = x.access_control_flag,
                    Mode = x.mode,
                    ModeDesc = x.mode_desc,
                    OfflineMode = x.offline_mode,
                    OfflineModeDesc = x.offline_mode_desc,
                    DefaultMode = x.default_mode,
                    DefaultModeDesc = x.default_mode_desc,
                    DefaultLEDMode = x.default_led_mode,
                    PreAlarm = x.pre_alarm,
                    AntiPassbackDelay = x.antipassback_delay,
                    StrkT2 = x.strike_t2,
                    DcHeld2 = x.dc_held2,
                    StrkFollowPulse = x.strike_follow_pulse,
                    StrkFollowDelay = x.strike_follow_delay,
                    nExtFeatureType = x.n_ext_feature_type,
                    IlPBSio = x.i_lpb_sio,
                    IlPBNumber = x.i_lpb_number,
                    IlPBLongPress = x.i_lpb_long_press,
                    IlPBOutSio = x.i_lpb_out_sio,
                    IlPBOutNum = x.i_lpb_out_num,
                    DfOfFilterTime = x.df_filter_time,
                    MaskForceOpen = x.is_force_mask,
                    MaskHeldOpen = x.is_held_mask,

                })
                .ToArrayAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<DoorDto>>(dtos);
        }

        public async Task<ResponseDto<DoorDto>> GetByComponentAsync(string mac, short component)
        {
            var dto = await context.door
                .AsNoTracking()
                .Where(x => x.hardware_mac == mac && x.component_id == component)
                .Select(x => new DoorDto
                {
                    // Base 
                    Uuid = x.uuid,
                    ComponentId = x.component_id,
                    Mac = x.hardware_mac,
                    LocationId = x.location_id,
                    IsActive = x.is_active,

                    // extend_desc
                    Name = x.name,
                    AccessConfig = x.access_config,
                    PairDoorNo = x.pair_door_no,

                    // Reader
                    Readers = x.readers == null ? new List<ReaderDto>() : x.readers
                    .Select(x => new ReaderDto
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
                    })
                    .ToList(),
                    ReaderOutConfiguration = x.reader_out_config,

                    // Strike
                    StrkComponentId = x.strike_id,
                    Strk = x.strike == null ? null : new StrikeDto
                    {
                        // Base 
                        Uuid = x.strike.uuid,
                        ComponentId = x.strike.component_id,
                        Mac = x.strike.module.hardware_mac,
                        HardwareName = x.strike.module.hardware.name,
                        LocationId = x.location_id,
                        IsActive = x.is_active,

                        // extend_desc
                        ModuleId = x.strike.module_id,
                        OutputNo = x.strike.output_no,
                        RelayMode = x.strike.relay_mode,
                        OfflineMode = x.strike.offline_mode,
                        StrkMax = x.strike.strike_max,
                        StrkMin = x.strike.strike_min,
                        StrkMode = x.strike.strike_mode,
                    },

                    // sensor
                    SensorComponentId = x.sensor_id,
                    Sensor = x.sensor == null ? null : new SensorDto
                    {

                        // Base 
                        Uuid = x.sensor.uuid,
                        ComponentId = x.sensor.component_id,
                        Mac = x.sensor.module.hardware_mac,
                        HardwareName = x.sensor.module.hardware.name,
                        LocationId = x.sensor.location_id,
                        IsActive = x.sensor.is_active,

                        // extend_desc
                        ModuleId = x.sensor.module_id,
                        InputNo = x.sensor.input_no,
                        InputMode = x.sensor.input_mode,
                        Debounce = x.sensor.debounce,
                        HoldTime = x.sensor.holdtime,
                        DcHeld = x.sensor.dc_held,

                    },

                    // Request Exit
                    RequestExits = x.request_exits == null ? new List<RequestExitDto>() : x.request_exits
                    .Select(x => new RequestExitDto
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
                    })
                    .ToList(),


                    CardFormat = x.card_format,
                    AntiPassbackMode = x.antipassback_mode,
                    AntiPassBackIn = (short)(x.antipassback_in == null ? 0 : (short)x.antipassback_in),
                    AntiPassBackOut = (short)(x.antipassback_out == null ? 0 : (short)x.antipassback_out),
                    SpareTags = x.spare_tag,
                    AccessControlFlags = x.access_control_flag,
                    Mode = x.mode,
                    ModeDesc = x.mode_desc,
                    OfflineMode = x.offline_mode,
                    OfflineModeDesc = x.offline_mode_desc,
                    DefaultMode = x.default_mode,
                    DefaultModeDesc = x.default_mode_desc,
                    DefaultLEDMode = x.default_led_mode,
                    PreAlarm = x.pre_alarm,
                    AntiPassbackDelay = x.antipassback_delay,
                    StrkT2 = x.strike_t2,
                    DcHeld2 = x.dc_held2,
                    StrkFollowPulse = x.strike_follow_pulse,
                    StrkFollowDelay = x.strike_follow_delay,
                    nExtFeatureType = x.n_ext_feature_type,
                    IlPBSio = x.i_lpb_sio,
                    IlPBNumber = x.i_lpb_number,
                    IlPBLongPress = x.i_lpb_long_press,
                    IlPBOutSio = x.i_lpb_out_sio,
                    IlPBOutNum = x.i_lpb_out_num,
                    DfOfFilterTime = x.df_filter_time,
                    MaskForceOpen = x.is_force_mask,
                    MaskHeldOpen = x.is_held_mask,

                })
                .FirstOrDefaultAsync();
            if(dto is null) return ResponseHelper.NotFoundBuilder<DoorDto>();
            return ResponseHelper.SuccessBuilder(dto);
        }

        public async Task<ResponseDto<bool>> UnlockAsync(string mac, short component)
        {
            short id = await helperService.GetIdFromMacAsync(mac);
            if(!command.MomentaryUnlock(id,component))
            {
                return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.COMMAND_UNSUCCESS,MessageBuilder.Unsuccess(mac,Command.C311));
            }
            return ResponseHelper.SuccessBuilder(true);
        }

        private async Task<ResponseDto<IEnumerable<ModeDto>>> ReaderModeAsync()
        {
            var dtos = await context.reader_configuration_mode.Select(x => new ModeDto 
            {
                Name = x.name,
                Value = x.value,
                Description = x.description

            }).ToArrayAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dtos);
        }

        private async Task<ResponseDto<IEnumerable<ModeDto>>> StrikeModeAsync()
        {
            var dtos = await context.strike_mode.Select(x => new ModeDto 
            {
                Name = x.name,
                Value = x.value,
                Description = x.description

            }).ToArrayAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dtos);
        }

        private async Task<ResponseDto<IEnumerable<ModeDto>>> AcrModeAsync()
        {
            var dtos = await context.door_mode.Select(x => new ModeDto 
            {
                Name = x.name,
                Value = x.value,
                Description = x.description

            }).ToArrayAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dtos);
        }

        public async Task<ResponseDto<IEnumerable<ModeDto>>> ApbModeAsync()
        {
            var dtos = await context.antipassback_mode.Select(x => new ModeDto 
            {
                Name = x.name,
                Value = x.value,
                Description = x.description

            }).ToArrayAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dtos);
        }


        public async Task<ResponseDto<IEnumerable<ModeDto>>> ReaderOutConfigurationAsync()
        {
            var dtos = await context.reader_out_configuration
                .Select(x => new ModeDto
                {
                    Name = x.name,
                    Value = x.value,
                    Description = x.description

                }).ToArrayAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dtos);
        }
        public async Task<ResponseDto<IEnumerable<short>>> AvailableReaderAsync(string mac, short component)
        {
            var reader = await context.module
                .AsNoTracking()
                .Where(cp => cp.component_id == component && cp.hardware_mac == mac)
                .Select(cp => (short)cp.n_reader)
                .FirstOrDefaultAsync();

            var rdrNos = await context.reader
                .AsNoTracking()
                .Where(cp => cp.module_id == component && cp.module.hardware_mac == mac)
                .Select(x => x.reader_no)
                .ToArrayAsync();


            List<short> all = Enumerable.Range(0, reader).Select(i => (short)i).ToList();
            return ResponseHelper.SuccessBuilder<IEnumerable<short>>(all.Except(rdrNos).ToList());
        }


        public async Task<ResponseDto<bool>> ChangeModeAsync(ChangeDoorModeDto dto)
        {
            var entity = await context.door.FirstOrDefaultAsync(x => x.hardware_mac == dto.MacAddress && x.component_id == dto.ComponentId);
            if (entity == null) return ResponseHelper.NotFoundBuilder<bool>();

            var ScpId = await helperService.GetIdFromMacAsync(dto.MacAddress);
            if (!command.AcrMode(ScpId, dto.ComponentId, dto.Mode))
            {
                return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.COMMAND_UNSUCCESS,MessageBuilder.Unsuccess(dto.MacAddress,Command.C308));
            }

            entity.mode = dto.Mode;
            entity.mode_desc = await context.door_mode
                .AsNoTracking()
                .Where(x => x.value == entity.mode)
                .Select(x => x.name)
                .FirstOrDefaultAsync() ?? "";
            context.Update(entity);
            await context.SaveChangesAsync();
            return ResponseHelper.SuccessBuilder(true);

        }


        public async Task TriggerDeviceStatusAsync(int ScpId, short AcrNo, string AcrMode, string AccessPointStatus)
        {
            string ScpMac = await helperService.GetMacFromIdAsync((short)ScpId);
            await hub.Clients.All.SendAsync("AcrStatus", ScpMac, AcrNo, AcrMode, AccessPointStatus);
        }

        public void TriggerDeviceStatus(int ScpId, short AcrNo, string AcrMode, string AccessPointStatus)
        {
            string ScpMac = helperService.GetMacFromId((short)ScpId);
            hub.Clients.All.SendAsync("AcrStatus", ScpMac, AcrNo, AcrMode, AccessPointStatus);
        }



        public async Task<ResponseDto<bool>> CreateAsync(DoorDto dto)
        {
            var maxDoor = await context.system_setting
                .AsNoTracking()
                .Select(x => x.n_acr)
                .FirstOrDefaultAsync();

            short DoorId = await helperService
                .GetLowestUnassignedNumberAsync<Door>(context, maxDoor);

            if (DoorId == -1) return ResponseHelper.ExceedLimit<bool>();

            short ScpId = await helperService.GetIdFromMacAsync(dto.Mac);

            var door = MapperHelper.DtoToDoor(
                dto,
                DoorId,
                await context.door_mode
                .AsNoTracking()
                .Where(x => x.value == dto.Mode)
                .Select(x => x.name)
                .FirstOrDefaultAsync() ?? "",
                await context.door_mode
                .AsNoTracking()
                .Where(x => x.value == dto.OfflineMode)
                .Select(x => x.name)
                .FirstOrDefaultAsync() ?? "",
                await context.door_mode
                .AsNoTracking()
                .Where(x => x.value == dto.DefaultMode)
                .Select(x => x.name)
                .FirstOrDefaultAsync() ?? "",
                DateTime.Now
            );

            foreach (var reader in door.readers)
            {
                if (string.IsNullOrEmpty(reader.module.hardware_mac)) continue;
                short readerInOsdpFlag = 0x00;
                short readerLedDriveMode = 0;
                if (reader.osdp_flag)
                {
                    readerInOsdpFlag |= reader.osdp_baudrate;
                    readerInOsdpFlag |= reader.osdp_discover;
                    readerInOsdpFlag |= reader.osdp_tracing;
                    readerInOsdpFlag |= reader.osdp_address;
                    readerInOsdpFlag |= reader.osdp_secure_channel;
                    readerLedDriveMode = 7;
                }
                else
                {
                    readerLedDriveMode = 1;
                }


                // Reader In Config

                var ReaderInId = await helperService.GetIdFromMacAsync(reader.module.hardware_mac);
                var ReaderComponentId = await helperService.GetLowestUnassignedNumberNoLimitAsync<Reader>(context);
                reader.component_id = ReaderComponentId;
                if (!command.ReaderSpecification(ReaderInId, reader.module_id, reader.reader_no, reader.data_format, reader.keypad_mode, readerLedDriveMode, readerInOsdpFlag))
                {
                    return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(door.hardware_mac, Command.C112));
                }
            }



            // Strike Strike Config
            var StrikeComponentId = await helperService.GetLowestUnassignedNumberNoLimitAsync<Strike>(context);
            door.strike.component_id = StrikeComponentId;
            var StrikeId = await helperService.GetIdFromMacAsync(door.strike.module.hardware_mac);
            if (!command.OutputPointSpecification(StrikeId, door.strike.module_id, door.strike.output_no, door.strike.relay_mode))
            {
                return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(door.hardware_mac, Command.C111));
            }

            // door sensor Config
            var SensorId = await helperService.GetIdFromMacAsync(door.sensor.module.hardware_mac);
            var SensorComponentId = await helperService.GetLowestUnassignedNumberNoLimitAsync<Sensor>(context);
            door.sensor.component_id = SensorComponentId;
            if (!command.InputPointSpecification(SensorId, door.sensor.module_id, door.sensor.input_no, door.sensor.input_mode, door.sensor.debounce, door.sensor.holdtime))
            {
                return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(door.hardware_mac, Command.C110));
            }

            foreach (var rex in door.request_exits)
            {
                if (string.IsNullOrEmpty(rex.module.hardware_mac)) continue;
                var Rex0Id = await helperService.GetIdFromMacAsync(rex.module.hardware_mac);
                var rexComponentId = await helperService.GetLowestUnassignedNumberNoLimitAsync<RequestExit>(context);
                rex.component_id = rexComponentId;
                if (!command.InputPointSpecification(Rex0Id, rex.module_id, rex.input_no, rex.input_mode, rex.debounce, rex.holdtime))
                {
                    return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(door.hardware_mac, Command.C110));
                }
            }

            if (!command.AccessControlReaderConfiguration(ScpId, DoorId, door))
            {
                return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(door.hardware_mac, Command.C115));
            }



            await context.door.AddAsync(door);
            await context.SaveChangesAsync();
            return ResponseHelper.SuccessBuilder(true);
        }

        public async Task<ResponseDto<bool>> DeleteAsync(string mac, short component)
        {
            var reader = await context.door.FirstOrDefaultAsync(x => x.hardware_mac == mac && x.component_id == component);
            if(reader is null) return ResponseHelper.NotFoundBuilder<bool>();
            context.door.Remove(reader);
            await context.SaveChangesAsync();
            return ResponseHelper.SuccessBuilder<bool>(true);
        }

        public async Task<ResponseDto<DoorDto>> UpdateAsync(DoorDto dto)
        {

            var door = await context.door
                .Include(x => x.readers)
                //.ThenInclude(x => x.module)
                .Include(x => x.sensor)
                //.ThenInclude(x => x.module)
                .Include(x => x.request_exits)
                //.ThenInclude(x => x.module)
                .Include(x => x.strike)
                //.ThenInclude(x => x.module)
                .Where(x => x.component_id == dto.ComponentId && x.hardware_mac == dto.Mac)
                .FirstOrDefaultAsync();

            if (door is null) return ResponseHelper.NotFoundBuilder<DoorDto>();
            short ScpId = await helperService.GetIdFromMacAsync(dto.Mac);

            MapperHelper.UpdateDoor(door, dto,
                 await context.door_mode
                .AsNoTracking()
                .Where(x => x.value == dto.Mode)
                .Select(x => x.name)
                .FirstOrDefaultAsync() ?? "",
                await context.door_mode
                .AsNoTracking()
                .Where(x => x.value == dto.OfflineMode)
                .Select(x => x.name)
                .FirstOrDefaultAsync() ?? "",
                await context.door_mode
                .AsNoTracking()
                .Where(x => x.value == dto.DefaultMode)
                .Select(x => x.name)
                .FirstOrDefaultAsync() ?? ""
             );

            foreach (var reader in door.readers)
            {
                if (string.IsNullOrEmpty(reader.module.hardware_mac)) continue;
                short readerInOsdpFlag = 0x00;
                short readerLedDriveMode = 0;
                if (reader.osdp_flag)
                {
                    readerInOsdpFlag |= reader.osdp_baudrate;
                    readerInOsdpFlag |= reader.osdp_discover;
                    readerInOsdpFlag |= reader.osdp_tracing;
                    readerInOsdpFlag |= reader.osdp_address;
                    readerInOsdpFlag |= reader.osdp_secure_channel;
                    readerLedDriveMode = 7;
                }
                else
                {
                    readerLedDriveMode = 1;
                }


                // Reader In Config
                var ReaderInId = await helperService.GetIdFromMacAsync(reader.module.hardware_mac);
                if (!command.ReaderSpecification(ReaderInId, reader.module_id, reader.reader_no, reader.data_format, reader.keypad_mode, readerLedDriveMode, readerInOsdpFlag))
                {
                    return ResponseHelper.UnsuccessBuilder<DoorDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(door.hardware_mac, Command.C112));
                }
            }

            // Strike Strike Config
            var StrkId = await helperService.GetIdFromMacAsync(door.strike.module.hardware_mac);
            if (!command.OutputPointSpecification(StrkId, door.strike.module_id, door.strike.output_no, door.strike.relay_mode))
            {
                return ResponseHelper.UnsuccessBuilder<DoorDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(door.hardware_mac, Command.C111));
            }

            // door sensor Config
            var SensorId = await helperService.GetIdFromMacAsync(door.sensor.module.hardware_mac);
            if (!command.InputPointSpecification(SensorId, door.sensor.module_id, door.sensor.input_no, door.sensor.input_mode, door.sensor.debounce, door.sensor.holdtime))
            {
                return ResponseHelper.UnsuccessBuilder<DoorDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(door.hardware_mac, Command.C110));
            }

            foreach (var rex in door.request_exits)
            {
                if (string.IsNullOrEmpty(rex.module.hardware_mac)) continue;
                var Rex0Id = await helperService.GetIdFromMacAsync(rex.module.hardware_mac);
                if (!command.InputPointSpecification(Rex0Id, rex.module_id, rex.input_no, rex.input_mode, rex.debounce, rex.holdtime))
                {
                    return ResponseHelper.UnsuccessBuilder<DoorDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(dto.Mac, Command.C110));
                }
            }

            if (!command.AccessControlReaderConfiguration(ScpId, dto.ComponentId, door))
            {
                return ResponseHelper.UnsuccessBuilder<DoorDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(door.hardware_mac, Command.C115));
            }

            // DeleteAsync old 
            context.sensor.Remove(door.sensor);
            if(door.request_exits is not null)context.request_exit.RemoveRange(door.request_exits);
            context.reader.RemoveRange(door.readers);
            context.strike.Remove(door.strike);

            context.door.Update(door);
            await context.SaveChangesAsync();
            return ResponseHelper.SuccessBuilder(dto);
        }

        public async Task<ResponseDto<bool>> GetStatusAsync(string mac, short component)
        {
            short id = await helperService.GetIdFromMacAsync(mac);
            if (!command.GetAcrStatus(id, component, 1))
            {
                return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.COMMAND_UNSUCCESS,MessageBuilder.Unsuccess(mac,Command.C407));
            }
            return ResponseHelper.SuccessBuilder(true);
        }

        public async Task<ResponseDto<IEnumerable<ModeDto>>> GetModeAsync(int param)
        {
            switch ((ComponentEnum.AcrServiceMode)param)
            {
                case ComponentEnum.AcrServiceMode.ReaderMode:
                    return await ReaderModeAsync();
                 case ComponentEnum.AcrServiceMode.StrikeMode:
                    return await StrikeModeAsync();
                 case ComponentEnum.AcrServiceMode.AcrMode:
                    return await AcrModeAsync();
                case ComponentEnum.AcrServiceMode.ApbMode:
                    return await ApbModeAsync();
                case ComponentEnum.AcrServiceMode.ReaderOut:
                    return await ReaderOutConfigurationAsync();
                case ComponentEnum.AcrServiceMode.SpareFlag:
                    return await GetSpareFlagAsync();
                case ComponentEnum.AcrServiceMode.AccessControlFlag:
                    return await GetAccessControlFlagAsync();
                default:
                    return ResponseHelper.UnsuccessBuilder<IEnumerable<ModeDto>>(ResponseMessage.NOT_FOUND,ResponseMessage.NOT_FOUND);
            }

        }

        public async Task<ResponseDto<IEnumerable<ModeDto>>> GetSpareFlagAsync()
        {
            var dtos = await context.door_access_control_flag
                .Select(x => new ModeDto 
                {
                    Name = x.name,
                    Value = x.value,
                    Description = x.description
                })
                .ToArrayAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dtos);
        }

        public async Task<ResponseDto<IEnumerable<ModeDto>>> GetAccessControlFlagAsync()
        {
            var dtos = await context.door_access_control_flag
                .Select(x => new ModeDto 
                {
                    Name = x.name,
                    Value = x.value,
                    Description = x.description
                }).ToArrayAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dtos);
        }


        public async Task<ResponseDto<IEnumerable<ModeDto>>> GetOsdpBaudRate()
        {
            var dtos = await context.osdp_baudrate.Select(x => new ModeDto
            {
                Name = x.name,
                Value = x.value,
                Description = x.description,
            }).ToArrayAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dtos);
        }

        public async Task<ResponseDto<IEnumerable<ModeDto>>> GetOsdpAddress()
        {
            var dtos = await context.osdp_address.Select(x => new ModeDto 
            {
                Name = x.name,
                Value = x.value,
                Description = x.description,
            }).ToArrayAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dtos);
        }

        public async Task<ResponseDto<IEnumerable<ModeDto>>> GetAvailableOsdpAddress(string mac,short component)
        {
            throw new NotImplementedException();
        }

 

    }
}
