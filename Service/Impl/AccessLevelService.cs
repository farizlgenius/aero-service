using AeroService.Aero.CommandService;
using AeroService.Aero.CommandService.Impl;
using AeroService.Constant;
using AeroService.Constants;
using AeroService.Data;
using AeroService.DTO;
using AeroService.DTO.AccessLevel;
using AeroService.DTO.Interval;
using AeroService.DTO.Reader;
using AeroService.DTO.TimeZone;
using AeroService.Entity;
using AeroService.Helpers;
using AeroService.Mapper;
using AeroService.Utility;
using Microsoft.EntityFrameworkCore;
using MiNET.Entities;


namespace AeroService.Service.Impl
{
    public sealed class AccessLevelService(AeroCommandService command, AppDbContext context, IHelperService<AccessLevel> helperService) : IAccessLevelService
    {

        public async Task<ResponseDto<IEnumerable<AccessLevelDto>>> GetAsync()
        {
            var dtos = await context.accesslevel
                .AsNoTracking()
                .Select(x => new AccessLevelDto
                {
                    // Base
                    Uuid = x.uuid,
                    LocationId = x.location_id,
                    IsActive = x.is_active,

                    // extend_desc
                    Name = x.name,
                    component_id = x.component_id,
                    AccessLevelDoorTimeZoneDto = x.accessleve_door_timezones
                    .Select(x => new AccessLevelDoorTimeZoneDto
                    {
                        Doors = new DTO.Acr.DoorDto 
                        {
                            // Base 
                            Uuid = x.door.uuid,
                            ComponentId = x.door.component_id,
                            Mac = x.door.hardware_mac,
                            LocationId = x.door.location_id,
                            IsActive = x.door.is_active,

                            // extend_desc
                            Name = x.door.name,
                            AccessConfig = x.door.access_config,
                            PairDoorNo = x.door.pair_door_no,

                            Readers = new List<ReaderDto>(),
                            ReaderOutConfiguration = x.door.reader_out_config,
                            StrkComponentId = x.door.strike_id,
                            Strk = null,
                            SensorComponentId = x.door.sensor_id,
                            RequestExits = new List<DTO.RequestExit.RequestExitDto>(),

                            CardFormat = x.door.card_format,
                            AntiPassbackMode = x.door.antipassback_mode,
                            AntiPassBackIn = (short)(x.door.antipassback_in == null ? 0 : (short)x.door.antipassback_in),
                            AntiPassBackOut = (short)(x.door.antipassback_out == null ? 0 : (short)x.door.antipassback_out),
                            SpareTags = x.door.spare_tag,
                            AccessControlFlags = x.door.access_control_flag,
                            Mode = x.door.mode,
                            ModeDesc = x.door.mode_desc,
                            OfflineMode = x.door.offline_mode,
                            OfflineModeDesc = x.door.offline_mode_desc,
                            DefaultMode = x.door.default_mode,
                            DefaultModeDesc = x.door.default_mode_desc,
                            DefaultLEDMode = x.door.default_led_mode,
                            PreAlarm = x.door.pre_alarm,
                            AntiPassbackDelay = x.door.antipassback_delay,
                            StrkT2 = x.door.strike_t2,
                            DcHeld2 = x.door.dc_held2,
                            StrkFollowPulse = x.door.strike_follow_pulse,
                            StrkFollowDelay = x.door.strike_follow_delay,
                            nExtFeatureType = x.door.n_ext_feature_type,
                            IlPBSio = x.door.i_lpb_sio,
                            IlPBNumber = x.door.i_lpb_number,
                            IlPBLongPress = x.door.i_lpb_long_press,
                            IlPBOutSio = x.door.i_lpb_out_sio,
                            IlPBOutNum = x.door.i_lpb_out_num,
                            DfOfFilterTime = x.door.df_filter_time,
                            MaskForceOpen = x.door.is_force_mask,
                            MaskHeldOpen = x.door.is_held_mask,
                        },
                        TimeZone = new TimeZoneDto
                        {
                            // Base
                            IsActive = x.timezone.is_active,

                            // extend_desc
                            ComponentId = x.timezone.component_id,
                            Name = x.timezone.name,
                            Mode = x.timezone.mode,
                            ActiveTime = x.timezone.active_time,
                            DeactiveTime = x.timezone.deactive_time,
                            Intervals = new List<IntervalDto>()
                        },
                    })
                    .ToList()
                })
                .ToArrayAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<AccessLevelDto>>(dtos);
        }

        public async Task<ResponseDto<IEnumerable<AccessLevelDto>>> GetByLocationIdAsync(short location)
        {
            var dtos = await context.accesslevel
                .AsNoTracking()
                .Where(x => x.location_id == location)
                .Select(x => new AccessLevelDto
                {
                    // Base
                    Uuid = x.uuid,
                    LocationId = x.location_id,
                    IsActive = x.is_active,

                    // extend_desc
                    Name = x.name,
                    component_id = x.component_id,
                    AccessLevelDoorTimeZoneDto = x.accessleve_door_timezones
                    .Select(x => new AccessLevelDoorTimeZoneDto
                    {
                        Doors = new DTO.Acr.DoorDto
                        {
                            // Base 
                            Uuid = x.door.uuid,
                            ComponentId = x.door.component_id,
                            Mac = x.door.hardware_mac,
                            LocationId = x.door.location_id,
                            IsActive = x.door.is_active,

                            // extend_desc
                            Name = x.door.name,
                            AccessConfig = x.door.access_config,
                            PairDoorNo = x.door.pair_door_no,

                            Readers = new List<ReaderDto>(),
                            ReaderOutConfiguration = x.door.reader_out_config,
                            StrkComponentId = x.door.strike_id,
                            Strk = null,
                            SensorComponentId = x.door.sensor_id,
                            RequestExits = new List<DTO.RequestExit.RequestExitDto>(),

                            CardFormat = x.door.card_format,
                            AntiPassbackMode = x.door.antipassback_mode,
                            AntiPassBackIn = (short)(x.door.antipassback_in == null ? 0 : (short)x.door.antipassback_in),
                            AntiPassBackOut = (short)(x.door.antipassback_out == null ? 0 : (short)x.door.antipassback_out),
                            SpareTags = x.door.spare_tag,
                            AccessControlFlags = x.door.access_control_flag,
                            Mode = x.door.mode,
                            ModeDesc = x.door.mode_desc,
                            OfflineMode = x.door.offline_mode,
                            OfflineModeDesc = x.door.offline_mode_desc,
                            DefaultMode = x.door.default_mode,
                            DefaultModeDesc = x.door.default_mode_desc,
                            DefaultLEDMode = x.door.default_led_mode,
                            PreAlarm = x.door.pre_alarm,
                            AntiPassbackDelay = x.door.antipassback_delay,
                            StrkT2 = x.door.strike_t2,
                            DcHeld2 = x.door.dc_held2,
                            StrkFollowPulse = x.door.strike_follow_pulse,
                            StrkFollowDelay = x.door.strike_follow_delay,
                            nExtFeatureType = x.door.n_ext_feature_type,
                            IlPBSio = x.door.i_lpb_sio,
                            IlPBNumber = x.door.i_lpb_number,
                            IlPBLongPress = x.door.i_lpb_long_press,
                            IlPBOutSio = x.door.i_lpb_out_sio,
                            IlPBOutNum = x.door.i_lpb_out_num,
                            DfOfFilterTime = x.door.df_filter_time,
                            MaskForceOpen = x.door.is_force_mask,
                            MaskHeldOpen = x.door.is_held_mask,
                        },
                        TimeZone = new TimeZoneDto
                        {
                            // Base
                            IsActive = x.timezone.is_active,

                            // extend_desc
                            ComponentId = x.timezone.component_id,
                            Name = x.timezone.name,
                            Mode = x.timezone.mode,
                            ActiveTime = x.timezone.active_time,
                            DeactiveTime = x.timezone.deactive_time,
                            Intervals = new List<IntervalDto>()
                        },
                    })
                    .ToList()
                })
                .ToArrayAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<AccessLevelDto>>(dtos);
        }



        public async Task<ResponseDto<AccessLevelDto>> GetByComponentIdAsync(short component)
        {
            var dtos = await context.accesslevel
                .AsNoTracking()
                .OrderBy(x => x.component_id)
                .Where(x => x.component_id == component)
                .Select(x => new AccessLevelDto
                {
                    // Base
                    Uuid = x.uuid,
                    LocationId = x.location_id,
                    IsActive = x.is_active,

                    // extend_desc
                    Name = x.name,
                    component_id = x.component_id,
                    AccessLevelDoorTimeZoneDto = x.accessleve_door_timezones
                    .Select(x => new AccessLevelDoorTimeZoneDto
                    {
                        Doors = new DTO.Acr.DoorDto
                        {
                            // Base 
                            Uuid = x.door.uuid,
                            ComponentId = x.door.component_id,
                            Mac = x.door.hardware_mac,
                            LocationId = x.door.location_id,
                            IsActive = x.door.is_active,

                            // extend_desc
                            Name = x.door.name,
                            AccessConfig = x.door.access_config,
                            PairDoorNo = x.door.pair_door_no,

                            Readers = new List<ReaderDto>(),
                            ReaderOutConfiguration = x.door.reader_out_config,
                            StrkComponentId = x.door.strike_id,
                            Strk = null,
                            SensorComponentId = x.door.sensor_id,
                            RequestExits = new List<DTO.RequestExit.RequestExitDto>(),

                            CardFormat = x.door.card_format,
                            AntiPassbackMode = x.door.antipassback_mode,
                            AntiPassBackIn = (short)(x.door.antipassback_in == null ? 0 : (short)x.door.antipassback_in),
                            AntiPassBackOut = (short)(x.door.antipassback_out == null ? 0 : (short)x.door.antipassback_out),
                            SpareTags = x.door.spare_tag,
                            AccessControlFlags = x.door.access_control_flag,
                            Mode = x.door.mode,
                            ModeDesc = x.door.mode_desc,
                            OfflineMode = x.door.offline_mode,
                            OfflineModeDesc = x.door.offline_mode_desc,
                            DefaultMode = x.door.default_mode,
                            DefaultModeDesc = x.door.default_mode_desc,
                            DefaultLEDMode = x.door.default_led_mode,
                            PreAlarm = x.door.pre_alarm,
                            AntiPassbackDelay = x.door.antipassback_delay,
                            StrkT2 = x.door.strike_t2,
                            DcHeld2 = x.door.dc_held2,
                            StrkFollowPulse = x.door.strike_follow_pulse,
                            StrkFollowDelay = x.door.strike_follow_delay,
                            nExtFeatureType = x.door.n_ext_feature_type,
                            IlPBSio = x.door.i_lpb_sio,
                            IlPBNumber = x.door.i_lpb_number,
                            IlPBLongPress = x.door.i_lpb_long_press,
                            IlPBOutSio = x.door.i_lpb_out_sio,
                            IlPBOutNum = x.door.i_lpb_out_num,
                            DfOfFilterTime = x.door.df_filter_time,
                            MaskForceOpen = x.door.is_force_mask,
                            MaskHeldOpen = x.door.is_held_mask,
                        },
                        TimeZone = new TimeZoneDto
                        {
                            // Base
                            IsActive = x.timezone.is_active,

                            // extend_desc
                            ComponentId = x.timezone.component_id,
                            Name = x.timezone.name,
                            Mode = x.timezone.mode,
                            ActiveTime = x.timezone.active_time,
                            DeactiveTime = x.timezone.deactive_time,
                            Intervals = new List<IntervalDto>()
                        },
                    })
                    .ToList()
                })
                .FirstOrDefaultAsync();

            if(dtos is null) return ResponseHelper.NotFoundBuilder<AccessLevelDto>();

            return ResponseHelper.SuccessBuilder<AccessLevelDto>(dtos);
        }



        public async Task<ResponseDto<bool>> CreateAsync(CreateUpdateAccessLevelDto dto)
        {
            List<string> errors = new List<string>();
            var max = await context.system_setting.Select(x => x.n_alvl).FirstOrDefaultAsync();
            var ComponentId = await helperService.GetLowestUnassignedNumberAsync<AccessLevel>(context,max);
            if (ComponentId == -1) return ResponseHelper.ExceedLimit<bool>();

            var macs = dto.CreateUpdateAccessLevelDoorTimeZoneDto
                .Select(x => x.DoorMacAddress)
                .Distinct()
                .ToList();


            foreach (var mac in macs)
            {
                var id = await helperService.GetIdFromMacAsync(mac);
                if(id == 0) errors.Add(MessageBuilder.Notfound());
                //if (!await command.AccessLevelConfigurationExtendedCreate(id, component_id, dto.CreateUpdateAccessLevelDoorTimeZoneDto.Where(x => x.DoorMacAddress == mac).ToList()))
                //{
                //    errors.Add(MessageBuilder.Unsuccess(mac, command.C2116));

                //}

            }


            if (errors.Count > 0) return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.COMMAND_UNSUCCESS, errors);
            var entity = MapperHelper.DtoToAccessLevel(dto,ComponentId,DateTime.UtcNow);
            await context.accesslevel.AddAsync(entity);
            await context.SaveChangesAsync();
            return ResponseHelper.SuccessBuilder(true);
        }

        public async Task<ResponseDto<bool>> DeleteAsync(short ComponentId)
        {
            List<string> errors = new List<string>();
            var entity = await context.accesslevel
                .Include(x => x.accessleve_door_timezones)
                .ThenInclude(x => x.door)
                .FirstOrDefaultAsync(x => x.component_id == ComponentId);
            if (entity is null) return ResponseHelper.NotFoundBuilder<bool>();
            foreach (var d in entity.accessleve_door_timezones)
            {
                var ScpId = await helperService.GetIdFromMacAsync(d.door.hardware_mac);
                if (!command.AccessLevelConfigurationExtended(ScpId,ComponentId, 0))
                {
                    errors.Add(MessageBuilder.Unsuccess(d.door.hardware_mac, Command.C2116));
                }
            }
            if (errors.Count > 0) return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.COMMAND_UNSUCCESS,errors);
            context.accesslevel.Remove(entity);
            await context.SaveChangesAsync();
            return ResponseHelper.SuccessBuilder(true);
        }

        public async Task<ResponseDto<AccessLevelDto>> UpdateAsync(CreateUpdateAccessLevelDto dto)
        {

            var entity = await context.accesslevel
                .Include(x => x.accessleve_door_timezones)
                .ThenInclude(x => x.accesslevel)
                .Include(x => x.accessleve_door_timezones)
                .ThenInclude(x => x.timezone)
                .Include(x => x.accessleve_door_timezones)  
                .ThenInclude(x => x.door)
                .OrderBy(x => x.component_id)
                .FirstOrDefaultAsync(x => x.component_id == dto.component_id);

            if (entity is null) return ResponseHelper.NotFoundBuilder<AccessLevelDto>();
            List<string> errors = new List<string>();
            var macs = dto.CreateUpdateAccessLevelDoorTimeZoneDto
                .Select(x => x.DoorMacAddress)
                .Distinct()
                .ToList();


            foreach (var mac in macs)
            {
                var id = await helperService.GetIdFromMacAsync(mac);
                if (id == 0) errors.Add(MessageBuilder.Notfound());
                if (!command.AccessLevelConfigurationExtendedCreate(id, dto.component_id, dto.CreateUpdateAccessLevelDoorTimeZoneDto
                    .Where(x => x.DoorMacAddress == mac).ToList()))
                {
                    errors.Add(MessageBuilder.Unsuccess(mac, Command.C2116));

                }

            }

            if (errors.Count > 0) return ResponseHelper.UnsuccessBuilder<AccessLevelDto>(ResponseMessage.COMMAND_UNSUCCESS, errors);
            MapperHelper.DtoToAccessLevel(dto, dto.component_id, DateTime.UtcNow);
            context.accesslevel.Update(entity);
            await context.SaveChangesAsync();

            var res = await context.accesslevel
                .AsNoTracking()
                .Where(x => x.component_id == dto.component_id)
                .OrderBy(x => x.component_id)
                .Select(x => new AccessLevelDto
                {
                    // Base
                    Uuid = x.uuid,
                    LocationId = x.location_id,
                    IsActive = x.is_active,

                    // extend_desc
                    Name = x.name,
                    component_id = x.component_id,
                    AccessLevelDoorTimeZoneDto = x.accessleve_door_timezones
                    .Select(x => new AccessLevelDoorTimeZoneDto
                    {
                        Doors = new DTO.Acr.DoorDto
                        {
                            // Base 
                            Uuid = x.door.uuid,
                            ComponentId = x.door.component_id,
                            Mac = x.door.hardware_mac,
                            LocationId = x.door.location_id,
                            IsActive = x.door.is_active,

                            // extend_desc
                            Name = x.door.name,
                            AccessConfig = x.door.access_config,
                            PairDoorNo = x.door.pair_door_no,

                            Readers = new List<ReaderDto>(),
                            ReaderOutConfiguration = x.door.reader_out_config,
                            StrkComponentId = x.door.strike_id,
                            Strk = null,
                            SensorComponentId = x.door.sensor_id,
                            RequestExits = new List<DTO.RequestExit.RequestExitDto>(),

                            CardFormat = x.door.card_format,
                            AntiPassbackMode = x.door.antipassback_mode,
                            AntiPassBackIn = (short)(x.door.antipassback_in == null ? 0 : (short)x.door.antipassback_in),
                            AntiPassBackOut = (short)(x.door.antipassback_out == null ? 0 : (short)x.door.antipassback_out),
                            SpareTags = x.door.spare_tag,
                            AccessControlFlags = x.door.access_control_flag,
                            Mode = x.door.mode,
                            ModeDesc = x.door.mode_desc,
                            OfflineMode = x.door.offline_mode,
                            OfflineModeDesc = x.door.offline_mode_desc,
                            DefaultMode = x.door.default_mode,
                            DefaultModeDesc = x.door.default_mode_desc,
                            DefaultLEDMode = x.door.default_led_mode,
                            PreAlarm = x.door.pre_alarm,
                            AntiPassbackDelay = x.door.antipassback_delay,
                            StrkT2 = x.door.strike_t2,
                            DcHeld2 = x.door.dc_held2,
                            StrkFollowPulse = x.door.strike_follow_pulse,
                            StrkFollowDelay = x.door.strike_follow_delay,
                            nExtFeatureType = x.door.n_ext_feature_type,
                            IlPBSio = x.door.i_lpb_sio,
                            IlPBNumber = x.door.i_lpb_number,
                            IlPBLongPress = x.door.i_lpb_long_press,
                            IlPBOutSio = x.door.i_lpb_out_sio,
                            IlPBOutNum = x.door.i_lpb_out_num,
                            DfOfFilterTime = x.door.df_filter_time,
                            MaskForceOpen = x.door.is_force_mask,
                            MaskHeldOpen = x.door.is_held_mask,
                        },
                        TimeZone = new TimeZoneDto
                        {
                            // Base
                            IsActive = x.timezone.is_active,

                            // extend_desc
                            ComponentId = x.timezone.component_id,
                            Name = x.timezone.name,
                            Mode = x.timezone.mode,
                            ActiveTime = x.timezone.active_time,
                            DeactiveTime = x.timezone.deactive_time,
                            Intervals = new List<IntervalDto>()
                        },
                    })
                    .ToList()
                })
                .FirstOrDefaultAsync();

            if(res is null) return ResponseHelper.NotFoundBuilder<AccessLevelDto>();

            return ResponseHelper.SuccessBuilder(res);
        }


        public string GetAcrName(string mac, short component)
        {
            return context.door
                .Where(x => x.hardware_mac == mac && x.component_id == component).Select(x => x.name).FirstOrDefault() ?? "";
        }

        public string GetTzName(short component)
        {
            return context.timezone.Where(x => x.component_id == component).Select(x => x.name).FirstOrDefault() ?? "";
        }
    }
}
