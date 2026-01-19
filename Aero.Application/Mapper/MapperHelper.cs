using Aero.Application.DTOs;

namespace Aero.Application.Mapper
{
    public static class MapperHelper
    {

        #region IdReport

        public static IdReportDto IdReportToDto(IdReport en)
        {
            return new IdReportDto()
            {
                ComponentId = en.scp_id,
                SerialNumber = en.serial_number,
                MacAddress = en.mac,
                Ip = en.ip,
                Port = en.port,
                Firmware = en.firmware,
                HardwareTypeDescription = "HID Aero",
                HardwareType = 1,
            };
        }

        #endregion

        #region Hardware


        public static Hardware CreateToHardware(CreateHardwareDto dto,DateTime Created)
        {
            return new Hardware
            {
                component_id = dto.ComponentId,
                mac = dto.Mac,
                location_id = dto.LocationId,
                name = dto.Name,
                hardware_type = dto.HardwareType,
                hardware_type_desc = dto.HardwareTypeDescription,
                modules = new List<Module>
                {
                    new Module
                    {
                        // Base 
                        component_id = 0,
                        hardware_mac = dto.Mac,
                        location_id = dto.LocationId,
                        is_active = dto.IsActive,
                        created_date =Created,
                        updated_date = Created,

                        // extend_desc
                        model_desc = "Internal",
                        model = (short)Enums.Model.AeroX1100,
                        revision=dto.Firmware,
                        serial_number = dto.SerialNumber,
                        n_hardware_id = 217,
                        n_hardware_id_desc = "HID Aero X1100",
                        address = -1,
                        address_desc = "Internal",
                        port = 3,
                        n_input = (short)InputComponents.HIDAeroX1100,
                        n_output = (short)OutputComponents.HIDAeroX1100,
                        n_reader = (short)ReaderComponents.HIDAeroX1100,
                        msp1_no = 0,
                        baudrate = -1,
                        n_protocol = 0,
                        n_dialect = 0,

                    }
                },
                ip = dto.Ip,
                port = dto.Port,
                firmware = dto.Firmware,
                serial_number = dto.SerialNumber,
                is_upload = false,
                is_reset = false,
                port_one = dto.PortOne,
                protocol_one = dto.ProtocolOne,
                protocol_one_desc = dto.ProtocolOneDescription,
                port_two = dto.PortTwo,
                protocol_two_desc = dto.ProtocolTwoDescription,
                protocol_two = dto.ProtocolTwo,
                baudrate_one = dto.BaudRateOne,
                baudrate_two = dto.BaudRateTwo,
                last_sync = DateTime.UtcNow,
                created_date = DateTime.UtcNow,
                updated_date = DateTime.UtcNow
            };
        }

        public static void UpdateHardware(Hardware hw,HardwareDto dto) 
        {
            // Base 
            hw.mac = dto.Mac;
            hw.updated_date = DateTime.UtcNow;

            // Detail 
            hw.name = dto.Name;
            hw.hardware_type = dto.HardwareType;
            hw.hardware_type_desc = dto.HardwareTypeDescription;
            hw.ip = dto.Ip;
            hw.port = dto.Port;
            hw.firmware = dto.Firmware;
            hw.serial_number = dto.SerialNumber;
            hw.port_one = dto.PortOne;
            hw.protocol_one = dto.ProtocolOne;
            hw.protocol_one_desc = dto.ProtocolOneDescription;
            hw.port_two = dto.PortTwo;
            hw.protocol_two_desc = dto.ProtocolTwoDescription;
            hw.protocol_two = dto.ProtocolTwo;
            hw.baudrate_one = dto.BaudRateOne;
            hw.baudrate_two = dto.BaudRateTwo;
        }

        #endregion

        #region Module

       
       

        #endregion

        #region MonitorPoint

       

        public static MonitorPoint DtoToMonitorPoint(MonitorPointDto dto,short ComponentId,DateTime Create) 
        {
            return new MonitorPoint
            {
                // Base 
                component_id = ComponentId,
                location_id = dto.LocationId,
                is_active = true,
                created_date = Create,
                updated_date = Create,

                // extend_desc 
                name = dto.Name,
                module_id = dto.ModuleId,
                input_no = dto.InputNo,
                input_mode = dto.InputMode,
                input_mode_desc = dto.InputModeDescription,
                debounce = dto.Debounce,
                holdtime = dto.HoldTime,
                monitor_point_mode = dto.MonitorPointMode,
                monitor_point_mode_desc = dto.MonitorPointModeDescription,
                log_function = dto.LogFunction,
                log_function_desc = dto.LogFunctionDescription,
                delay_entry = dto.DelayEntry,
                delay_exit = dto.DelayExit,
                is_mask = false
            };
        }

        public static void UpdateMonitorPoint(MonitorPoint input,MonitorPointDto dto)
        {

            // Base
            input.location_id = dto.LocationId;
            input.is_active = dto.IsActive;
            input.updated_date = DateTime.UtcNow;

            // extend_desc
            input.name = dto.Name;
            input.module_id = dto.ModuleId;
            input.input_no = dto.InputNo;
            input.input_mode = dto.InputMode;
            input.input_mode_desc = dto.InputModeDescription;
            input.debounce = dto.Debounce;
            input.holdtime = dto.HoldTime;
            input.log_function = dto.LogFunction;
            input.log_function_desc = dto.LogFunctionDescription;
            input.monitor_point_mode = dto.MonitorPointMode;
            input.monitor_point_mode_desc = dto.MonitorPointModeDescription;
            input.delay_entry = dto.DelayEntry;
            input.delay_exit = dto.DelayExit;
            input.is_mask = dto.IsMask;
        }

        #endregion

        #region Monitor Group

        public static MonitorGroup DtoToMonitorGroup(MonitorGroupDto dto,short ComponentId,DateTime Create)
        {
            return new MonitorGroup
            {
                // Base 
                component_id = ComponentId,
                hardware_mac = dto.Mac,
                location_id = dto.LocationId,
                is_active = true,
                created_date = Create,
                updated_date = Create,

                // Detail
                name = dto.Name,
                n_mp_count = dto.nMpCount,
                n_mp_list = dto.nMpList.Select(x => DtoToMonitorGroupList(x,ComponentId)).ToList()
            };
        }

        public static MonitorGroupList DtoToMonitorGroupList(MonitorGroupListDto dto,short MonitorGroupId)
        {
            return new MonitorGroupList
            {
                point_type = dto.PointType,
                point_number = dto.PointNumber,
                point_type_desc = dto.PointTypeDesc,
                monitor_group_id = MonitorGroupId
            };
        }

        public static void UpdateMonitorGroup(MonitorGroup en,MonitorGroupDto dto)
        {
            // Base 
            en.location_id = dto.LocationId;
            en.updated_date = DateTime.UtcNow;

            // Detail
            en.name = dto.Name;
            en.n_mp_count = dto.nMpCount;
            en.n_mp_list = dto.nMpList.Select(x => DtoToMonitorGroupList(x,dto.ComponentId)).ToList();
        }

        #endregion

        #region ControlPoint


        public static ControlPoint DtoToControlPoint(ControlPointDto dto,short ComponentId,DateTime Create)
        {
            return new ControlPoint
            {
                // Base 
                component_id = ComponentId,
                location_id = dto.LocationId,
                is_active = true,
                created_date = Create,
                updated_date = Create,

                // extend_desc
                name = dto.Name,
                module_id = dto.ModuleId,
                module_desc = dto.ModuleDescription,
                output_no = dto.OutputNo,
                relay_mode = dto.RelayMode,
                relay_mode_desc = dto.RelayModeDescription,
                offline_mode = dto.OfflineMode,
                offline_mode_desc = dto.OfflineModeDescription,
                default_pulse = dto.DefaultPulse,
            };

        }

        public static void UpdateControlPoint(ControlPoint output,ControlPointDto dto)
        {

            // Base
            output.location_id = dto.LocationId;
            output.is_active = dto.IsActive;
            output.updated_date = DateTime.UtcNow;

            // extend_desc
            output.name = dto.Name;
            output.module_id = dto.ModuleId;
            output.module_desc = dto.ModuleDescription;
            output.output_no = dto.OutputNo;
            output.relay_mode = dto.RelayMode;
            output.relay_mode_desc = dto.RelayModeDescription;
            output.offline_mode = dto.OfflineMode;
            output.offline_mode_desc = dto.OfflineModeDescription;
            output.default_pulse = dto.DefaultPulse;
        }

        #endregion

        #region Readers



        public static Reader DtoToReader(ReaderDto dto,DateTime Create) 
        {
            return new Reader
            {
                // Base 
                component_id = dto.ComponentId,
                location_id = dto.LocationId,
                is_active = dto.IsActive,
                created_date = Create,
                updated_date = Create,

                // extend_desc
                module_id = dto.ModuleId,
                reader_no = dto.ReaderNo,
                data_format = dto.DataFormat,
                keypad_mode = dto.KeypadMode,
                led_drive_mode= dto.LedDriveMode,
                osdp_flag = dto.OsdpFlag,
                osdp_address = dto.OsdpAddress,
                osdp_baudrate= dto.OsdpBaudrate,
                osdp_discover= dto.OsdpDiscover,
                osdp_secure_channel= dto.OsdpSecureChannel,
                osdp_tracing= dto.OsdpTracing,
            };
        }

        public static void UpdateReader(Reader reader,ReaderDto dto) 
        {
            // Base
            reader.location_id = dto.LocationId;
            reader.is_active = dto.IsActive;
            reader.updated_date = DateTime.UtcNow;

            // extend_desc
            reader.module_id = dto.ModuleId;
            reader.reader_no = dto.ReaderNo;
            reader.data_format = dto.DataFormat;
            reader.keypad_mode = dto.KeypadMode;
            reader.led_drive_mode = dto.LedDriveMode;
            reader.osdp_flag = dto.OsdpFlag;
            reader.osdp_address = dto.OsdpAddress;
            reader.osdp_baudrate = dto.OsdpBaudrate;
            reader.osdp_discover = dto.OsdpDiscover;
            reader.osdp_secure_channel = dto.OsdpSecureChannel;
            reader.osdp_tracing = dto.OsdpTracing;
        }

        #endregion

        #region Strike


        public static Strike DtotoStrike(StrikeDto dto,DateTime Create)
        {
            return new Strike
            {
                // Base
                component_id= dto.ComponentId,
                location_id= dto.LocationId,
                is_active = dto.IsActive,
                created_date = Create,
                updated_date = Create,

                // extend_desc
                module_id= dto.ModuleId,
                output_no= dto.OutputNo,
                relay_mode= dto.RelayMode,
                offline_mode= dto.OfflineMode,
                strike_max= dto.StrkMax,
                strike_min= dto.StrkMin,
                strike_mode= dto.StrkMode,
            };
        }

        #endregion

        #region RequestExit


        public static RequestExit DtoToRequestExit(RequestExitDto dto,DateTime Create) 
        {
            return new RequestExit
            {
                // Base 
                component_id= dto.ComponentId,
                location_id= dto.LocationId,
                is_active= dto.IsActive,
                updated_date = Create,
                created_date = Create,

                // extend_desc
                module_id = dto.ModuleId,
                input_no= dto.InputNo,
                input_mode= dto.InputMode,
                debounce = dto.Debounce,
                holdtime= dto.HoldTime,
                mask_timezone= dto.MaskTimeZone,
            };
        }

        #endregion

        #region Sensor


        public static Sensor DtoToSensor(SensorDto dto,DateTime Create) 
        {
            return new Sensor
            {
                // Base
                component_id = dto.ComponentId,
                location_id = dto.LocationId,
                is_active = dto.IsActive,
                created_date = Create,
                updated_date = Create,

                // extend_desc
                module_id = dto.ModuleId,
                input_no = dto.InputNo,
                input_mode = dto.InputMode,
                debounce = dto.Debounce,
                holdtime = dto.HoldTime,
                dc_held = dto.DcHeld,
            };
        }

        #endregion

        #region Doors

     

        public static Door DtoToDoor(
            DoorDto dto,
            short ComponentId,
            string ModeDesc,
            string OfflineModeDesc,
            string DefaultModeDesc,
            DateTime Create) 
        {
            var door = new Door
            {
                // Base
                component_id = ComponentId,
                hardware_mac = dto.Mac,
                location_id = dto.LocationId,
                
                is_active = true,
                created_date = Create,
                updated_date = Create,


                // extend_desc
                name = dto.Name,
                access_config = dto.AccessConfig,
                pair_door_no = dto.PairDoorNo,
                readers = dto.Readers
                .Where(x => !string.IsNullOrEmpty(x.Mac))
                .Select(x => DtoToReader(x,Create))
                .ToList(),

                strike = DtotoStrike(dto.Strk,Create),
                sensor = DtoToSensor(dto.Sensor,Create),
                request_exits =  dto.RequestExits is null ? new List<RequestExit>() : dto.RequestExits
                .Where(x => !string.IsNullOrEmpty(x.Mac))
                .Select(x => DtoToRequestExit(x,Create))
                .ToList(),

                reader_out_config = dto.ReaderOutConfiguration,
                card_format = dto.CardFormat,
                antipassback_mode = dto.AntiPassbackMode,
                //antipassback_in = dto.antipassback_in,
                //antipassback_out = dto.antipassback_out,
                spare_tag = dto.SpareTags,
                access_control_flag = dto.AccessControlFlags,
                mode = dto.Mode,
                mode_desc = ModeDesc,
                offline_mode = dto.OfflineMode,
                offline_mode_desc = OfflineModeDesc,
                default_mode = dto.DefaultMode,
                default_mode_desc = DefaultModeDesc,
                default_led_mode = dto.DefaultLEDMode,
                pre_alarm = dto.PreAlarm,
                antipassback_delay = dto.AntiPassbackDelay,
                strike_t2 = dto.StrkT2,
                dc_held2 = dto.DcHeld2,
                strike_follow_pulse = dto.StrkFollowPulse,
                strike_follow_delay = dto.StrkFollowDelay,
                n_ext_feature_type = dto.nExtFeatureType,
                i_lpb_sio = dto.IlPBSio,
                i_lpb_number = dto.IlPBNumber,
                i_lpb_long_press = dto.IlPBLongPress,
                i_lpb_out_sio = dto.IlPBOutSio,
                i_lpb_out_num = dto.IlPBOutNum,
                df_filter_time = dto.DfOfFilterTime,
                is_held_mask = dto.MaskHeldOpen,
                is_force_mask = dto.MaskForceOpen,     

            };

            if (dto.AntiPassBackIn > 0) door.antipassback_in = dto.AntiPassBackIn;
            if (dto.AntiPassBackOut > 0) door.antipassback_out = dto.AntiPassBackOut;
            return door;
        }

        public static void UpdateDoor(Door door,DoorDto dto,string ModeDesc,string OfflineModeDesc,string DefaultModeDesc) 
        {
            DateTime time = DateTime.UtcNow;
            // Base

            door.component_id = dto.ComponentId;
            door.hardware_mac = dto.Mac;
            door.location_id = dto.LocationId;
            door.is_active = true;
            door.updated_date = time;

            // extend_desc
            door.name = dto.Name;
            door.access_config = dto.AccessConfig;
            door.pair_door_no = dto.PairDoorNo;
            door.readers = dto.Readers.Select(s => DtoToReader(s, time)).ToList();
            door.strike = DtotoStrike(dto.Strk, time);
            door.sensor = DtoToSensor(dto.Sensor, time);
            if(dto.RequestExits is not null && dto.RequestExits.Count > 0)
            {
                door.request_exits = dto.RequestExits.Select(s => DtoToRequestExit(s,time)).ToList();
            }
            door.reader_out_config = dto.ReaderOutConfiguration;
            door.card_format = dto.CardFormat;
            door.antipassback_mode = dto.AntiPassbackMode;
            door.antipassback_in = dto.AntiPassBackIn;
            door.antipassback_out = dto.AntiPassBackOut;
            door.spare_tag = dto.SpareTags;
            door.access_control_flag = dto.AccessControlFlags;
            door.mode = dto.Mode;
            door.mode_desc = ModeDesc;
            door.offline_mode = dto.OfflineMode;
            door.offline_mode_desc = OfflineModeDesc;
            door.default_mode = dto.DefaultMode;
            door.default_mode_desc = DefaultModeDesc;
            door.default_led_mode = dto.DefaultLEDMode;
            door.pre_alarm = dto.PreAlarm;
            door.antipassback_delay = dto.AntiPassbackDelay;
            door.strike_t2 = dto.StrkT2;
            door.dc_held2 = dto.DcHeld2;
            door.strike_follow_pulse = dto.StrkFollowPulse;
            door.strike_follow_delay = dto.StrkFollowDelay;
            door.n_ext_feature_type = dto.nExtFeatureType;
            door.i_lpb_sio = dto.IlPBSio;
            door.i_lpb_number = dto.IlPBNumber;
            door.i_lpb_long_press = dto.IlPBLongPress;
            door.i_lpb_out_sio = dto.IlPBOutSio;
            door.i_lpb_out_num = dto.IlPBOutNum;
            door.df_filter_time = dto.DfOfFilterTime;
            door.is_force_mask = dto.MaskForceOpen;
            door.is_held_mask = dto.MaskHeldOpen;
        }


        #endregion

        #region CardHolder

        public static CardHolderDto CardHolderToDto(CardHolder entity)
        {
            return new CardHolderDto 
            {
                // Base
                Uuid = entity.uuid,
                LocationId = entity.location_id,
                IsActive = entity.is_active,

                // extend_desc
                Flag = entity.flag,
                UserId = entity.user_id,
                Title = entity.title,
                FirstName = entity.first_name,
                MiddleName = entity.middle_name,
                LastName = entity.last_name,
                Sex = entity.sex,
                Email = entity.email,
                Phone = entity.phone,
                Company = entity.company,
                Position = entity.position,
                Department = entity.department,
                ImagePath = entity.image_path,
                Additionals = entity.additional
                .Where(x => x.holder_id == entity.user_id)
                .Select(x => x.additional).ToList(),
                Credentials = entity.credentials
                .Select(x => CredentialToDto(x)).ToList(),
                
            };
        }

        public static CardHolder DtoToCardHolder(CardHolderDto dto,List<short> ComponentIds,DateTime Create)
        {
            return new CardHolder
            {
                // Base
                location_id = dto.LocationId,
                
                is_active = true,
                created_date = Create,
                updated_date = Create,

                // extend_desc
                flag = dto.Flag,
                user_id = dto.UserId,
                title = dto.Title,
                first_name = dto.FirstName,
                middle_name = dto.MiddleName,
                last_name = dto.LastName,
                sex = dto.Sex,
                email = dto.Email,
                phone = dto.Phone,
                company = dto.Company,
                position = dto.Position,
                department = dto.Department,
                image_path = dto.ImagePath,
                additional = dto.Additionals
                .Select(x => DtoToAdditional(dto.UserId, x))
                .ToList(),
                credentials = dto.Credentials.Select((x, i) => DtoToCredential(x, ComponentIds[i], Create)).ToList(),
                access_levels = dto.AccessLevels is not null ? dto.AccessLevels.Select(x => DtoToCardHolderAccessLevel(x.component_id, dto.UserId)).ToList() : new List<CardHolderAccessLevel>()
            };
        }

        public static CardHolderAccessLevel DtoToCardHolderAccessLevel(short AccessLevelId,string UserId)
        {
            return new CardHolderAccessLevel
            {
                access_level_id = AccessLevelId,
                cardholder_id = UserId
            };
        }

        public static void UpdateCardHolder(CardHolder entity,CardHolderDto dto,List<short> ComponentId) 
        {
            // Detial
            entity.location_id = dto.LocationId;
            entity.is_active = dto.IsActive;
            entity.updated_date = DateTime.UtcNow;

            // Base
            entity.user_id = dto.UserId;
            entity.title = dto.Title;
            entity.first_name = dto.FirstName;
            entity.middle_name = dto.MiddleName;
            entity.last_name = dto.LastName;
            entity.sex = dto.Sex;
            entity.email = dto.Email;
            entity.phone = dto.Phone;
            entity.company = dto.Company;
            entity.position = dto.Position;
            entity.department = dto.Department;
            entity.additional = dto.Additionals
                .Select(x => MapperHelper.DtoToAdditional(dto.UserId, x))
                .ToArray();
            entity.image_path = dto.ImagePath;
            entity.credentials = dto.Credentials
                .Select((x,i) => MapperHelper.DtoToCredential(x, ComponentId[i], DateTime.UtcNow))
                .ToArray();
        }

        public static CardHolderAdditional DtoToAdditional(string HolderId,string Additional)
        {
            return new CardHolderAdditional
            {
                holder_id = HolderId,
                additional = Additional
            };
        }

        #endregion

        #region Credential

        public static CredentialDto CredentialToDto(Credential entity) 
        {
            return new CredentialDto
            {
                // Base
                Uuid = entity.uuid,
                LocationId = entity.location_id,
                IsActive = entity.is_active,

                // extend_desc
                component_id = entity.component_id,
                Bits = entity.bits,
                IssueCode = entity.issue_code,
                FacilityCode = entity.fac_code,
                CardNo = entity.card_no,
                Pin = entity.pin,
                ActiveDate = entity.active_date,
                DeactiveDate = entity.deactive_date,
                //card_holder = entity.card_holder is not null ? CardHolderToDto(entity.card_holder) : null,

            };
        }

        public static Credential DtoToCredential(CredentialDto dto,short ComponentId,DateTime Create)
        {
            return new Credential 
            {
                // Base
                location_id = dto.LocationId,
                is_active = true,
                created_date = Create,
                updated_date = Create,

                // extend_desc
                component_id = ComponentId,
                bits = dto.Bits,
                issue_code = dto.IssueCode,
                fac_code = dto.FacilityCode,
                card_no = dto.CardNo,
                pin = dto.Pin,
                active_date = dto.ActiveDate,
                deactive_date = dto.DeactiveDate,
            };
        }

        public static ModeDto CredentialFlagToDto(CredentialFlag flag)
        {
            return new ModeDto
            {
                Name= flag.name,
                Description = flag.description,
                Value = flag.value,
            };
        }



        #endregion

        #region AccessLevel


        public static AccessLevel DtoToAccessLevel(CreateUpdateAccessLevelDto dto,short ComponentId,DateTime Create)
        {
            return new AccessLevel 
            {
                // Base 
                location_id= dto.LocationId,
                is_active=true,
                created_date= Create,
                updated_date= Create,

                // extend_desc
                component_id= ComponentId,
                name= dto.Name,
                accessleve_door_timezones=dto.CreateUpdateAccessLevelDoorTimeZoneDto
                .Select(x => DtoToAccessLevelDoorTimeZone(x,ComponentId))
                .ToArray()
            };
        }

        public static void UpdateAccessLeve(AccessLevel entity,CreateUpdateAccessLevelDto dto)
        {
            // Base
            entity.location_id = dto.LocationId;
            entity.is_active = dto.IsActive;
            entity.updated_date = DateTime.UtcNow;

            // extend_desc
            entity.name = dto.Name;
            entity.accessleve_door_timezones = dto.CreateUpdateAccessLevelDoorTimeZoneDto
                .Select(x => DtoToAccessLevelDoorTimeZone(x,dto.component_id))
                .ToArray();
        }

        public static AccessLevelDoorTimeZone DtoToAccessLevelDoorTimeZone(CreateUpdateAccessLevelDoorTimeZoneDto dto,short ComponentId) 
        {
            return new AccessLevelDoorTimeZone
            {
                accesslevel_id = ComponentId,
                timezone_id = dto.TimeZoneId,
                door_id=dto.DoorId
            };
        }


        #endregion

        #region TimeZone

        public static TimeZoneDto TimeZoneToDto(Entity.TimeZone t)
        {
            return new TimeZoneDto 
            {
                // Base
                IsActive = t.is_active,

                // extend_desc
                ComponentId = t.component_id,
                Name = t.name,
                Mode = t.mode,
                ActiveTime = t.active_time,
                DeactiveTime = t.deactive_time,
                Intervals = t.timezone_intervals is null ? null : t.timezone_intervals
                .Select(s => s.interval)
                .Select(s => IntervalToDto(s))
                .ToList(),

            };
        }

        public static Entity.TimeZone TimeZoneDtoToTimeZone(TimeZoneDto t)
        {
            return new Entity.TimeZone 
            {
                // Base
                is_active = t.IsActive,

                // detail
                component_id= t.ComponentId,
                name = t.Name,
                mode = t.Mode,
                active_time = t.ActiveTime,
                deactive_time = t.DeactiveTime,
            };
        }

        public static Entity.TimeZone CreateTimeZoneDtoToTimeZone(CreateTimeZoneDto t,short ComponentId)
        {
            return new Entity.TimeZone
            {
                // Base
                is_active = t.IsActive,

                // detail
                component_id = ComponentId,
                name = t.Name,
                mode = t.Mode,
                active_time = t.ActiveTime,
                deactive_time = t.DeactiveTime,
            };
        }

        public static Entity.TimeZone TimeZoneDtoMapTimeZone(TimeZoneDto dto,Entity.TimeZone entity)
        {
            entity.name = dto.Name;
            entity.mode = dto.Mode;
            entity.active_time = dto.ActiveTime;
            entity.deactive_time = dto.DeactiveTime;
            entity.is_active = dto.IsActive;
            entity.updated_date = DateTime.UtcNow;
            return entity;
          
        }

        #endregion

        #region Interval

        public static IntervalDto IntervalToDto(Interval p)
        {
            return new IntervalDto
            {
                // Base 
                IsActive = p.is_active,
                LocationId = p.location_id,

                // extend_desc
                ComponentId = p.component_id,
                DaysDesc = p.days_desc,
                StartTime = p.start_time,
                EndTime = p.end_time,
                Days = new DaysInWeekDto
                {
                    Sunday = p.days.sunday,
                    Monday = p.days.monday,
                    Tuesday = p.days.tuesday,
                    Wednesday = p.days.wednesday,
                    Thursday = p.days.thursday,
                    Friday = p.days.friday,
                    Saturday = p.days.saturday
                }

            };
        }

        public static Interval DtoToInterval(IntervalDto dto)
        {
            return new Interval 
            {
                // Base
                uuid = dto.Uuid,
                
                is_active = dto.IsActive,

                // extend_desc
                component_id = dto.ComponentId,
                days_desc = dto.DaysDesc,
                start_time = dto.StartTime,
                end_time = dto.EndTime,
                days = new DaysInWeek 
                {
                    sunday = dto.Days.Sunday,
                    monday = dto.Days.Monday,
                    tuesday = dto.Days.Tuesday,
                    wednesday = dto.Days.Wednesday,
                    thursday= dto.Days.Thursday,
                    friday = dto.Days.Friday,
                    saturday= dto.Days.Saturday,
                }
            };
        }

        public static Interval CreateToInterval(CreateIntervalDto dto,short componentId,string DaysDesc)
        {
            return new Interval
            {
                // Base 
                
                is_active = true,
                updated_date = DateTime.UtcNow,
                location_id = dto.LocationId,

                // extend_desc
                component_id = componentId,
                days_desc = DaysDesc,
                start_time = dto.StartTime,
                end_time = dto.EndTime,
                days = new DaysInWeek
                {
                    sunday = dto.Days.Sunday,
                    monday = dto.Days.Monday,
                    tuesday = dto.Days.Tuesday,
                    wednesday = dto.Days.Wednesday,
                    thursday = dto.Days.Thursday,
                    friday = dto.Days.Friday,
                    saturday = dto.Days.Saturday,
                }
            };
        }

        public static Interval IntervalDtoMapInterval(IntervalDto dto,Interval interval) 
        {
            // Base 
            interval.is_active = dto.IsActive;
            interval.updated_date = DateTime.UtcNow;

            // extend_desc
            interval.component_id = dto.ComponentId;
            interval.days.sunday = dto.Days.Sunday;
            interval.days.monday = dto.Days.Monday;
            interval.days.tuesday = dto.Days.Tuesday;
            interval.days.wednesday = dto.Days.Wednesday;
            interval.days.thursday = dto.Days.Thursday;
            interval.days.friday = dto.Days.Friday;
            interval.days.saturday = dto.Days.Saturday;
            interval.days_desc = dto.DaysDesc;
            interval.start_time = dto.StartTime;
            interval.end_time = dto.EndTime;

            return interval;
        }

        public static void UpdateInterval(Interval en,IntervalDto dto) 
        {
            // Base
            en.updated_date = DateTime.UtcNow;

            // Detail
            en.days_desc = dto.DaysDesc;
            en.start_time = dto.StartTime;
            en.end_time = dto.EndTime;
            en.days.sunday = dto.Days.Sunday;
            en.days.monday = dto.Days.Monday;
            en.days.tuesday = dto.Days.Tuesday;
            en.days.wednesday = dto.Days.Wednesday;
            en.days.thursday = dto.Days.Thursday;
            en.days.friday = dto.Days.Friday;
            en.days.saturday = dto.Days.Saturday;

        }

        #endregion

        #region CardFormat

        public static CardFormat DtoToCardFormat(CardFormatDto dto,short ComponentId,DateTime Create) 
        {
            return new CardFormat
            {
                // Base 
                uuid = dto.Uuid,           
                is_active = dto.IsActive,
                created_date = Create,
                updated_date = Create,

                // extend_desc
                name = dto.Name,
                component_id = ComponentId,
                facility = dto.Facility,
                bits = dto.Bits,
                pe_ln = dto.PeLn,
                pe_loc = dto.PeLoc,
                po_ln = dto.PoLn,
                po_loc = dto.PoLoc,
                fc_ln = dto.FcLn,
                fc_loc = dto.FcLoc,
                ch_ln = dto.ChLn,
                ch_loc = dto.ChLoc,
                ic_ln = dto.IcLn,
                ic_loc = dto.IcLoc,

            };

        }

        public static CardFormatDto CardFormatToDto(CardFormat x)
        {
            return new CardFormatDto
            {
                // Baes 
                Uuid = x.uuid,
                IsActive = x.is_active,

                // extend_desc
                Name = x.name,
                ComponentId = x.component_id,
                Facility = x.facility,
                Bits = x.bits,
                PeLn = x.pe_ln,
                PeLoc = x.pe_loc,
                PoLn = x.po_ln,
                PoLoc = x.po_loc,
                FcLn = x.fc_ln,
                FcLoc = x.fc_loc,
                ChLn = x.ch_ln,
                ChLoc = x.ch_loc,
                IcLn = x.ic_ln,
                IcLoc = x.ic_loc,

            };

        }

        public static void UpdateCardFormat(CardFormat card,CardFormatDto dto)
        {
            card.is_active = dto.IsActive;
            card.updated_date = DateTime.UtcNow;

            card.name = dto.Name;
            card.component_id = dto.ComponentId;
            card.facility = dto.Facility;
            card.bits = dto.Bits;
            card.pe_ln = dto.PeLn;
            card.pe_loc = dto.PeLoc;
            card.po_ln = dto.PoLn;
            card.po_loc = dto.PoLoc;
            card.fc_ln = dto.FcLn;
            card.fc_loc = dto.FcLoc;
            card.ch_ln = dto.ChLn;
            card.ch_loc = dto.ChLoc;
            card.ic_ln = dto.IcLn;
            card.ic_loc = dto.IcLoc;
        }

        #endregion

        #region Location

        public static Location DtoToLocation(LocationDto dto,short LocationId,DateTime Create)
        {
            return new Location
            {
                uuid = dto.Uuid,
                component_id = LocationId,
                operator_locations = new List<OperatorLocation> { new OperatorLocation { location_id = LocationId, operator_id = 1 } },
                
                location_name = dto.LocationName,
                description = dto.Description,
                created_date = Create,
                updated_date = Create,
            };
        }

        //public static LocationDto LocationToDto(location location)
        //{
        //    return new LocationDto
        //    {
        //        uuid=location.uuid,
        //        component_id=location.component_id,
        //        location_name=location.location_name,
        //        description=location.description,
        //    };
        //}

        public static void UpdateLocation(Location location,LocationDto dto) 
        {
            location.location_name = dto.LocationName;
            location.description = dto.Description;
            location.updated_date = DateTime.UtcNow;
        }

        #endregion

        #region Access Area

        public static AccessAreaDto AccessAreaToDto(Area entity)
        {
            return new AccessAreaDto
            {
                // Base
                Uuid = entity.uuid,
                LocationId = entity.location_id,
                IsActive = entity.is_active,
                component_id = entity.component_id,

                // extend_desc
                Name = entity.name,
                MultiOccupancy = entity.multi_occ,
                AccessControl = entity.access_control,
                OccControl = entity.occ_control,
                OccSet = entity.occ_set,
                OccMax = entity.occ_max,
                OccDown = entity.occ_down,
                OccUp = entity.occ_up,
                AreaFlag = entity.area_flag,
            };
        }

        public static Area DtoToAccessArea(AccessAreaDto dto,short ComponentId,DateTime Create) 
        {
            return new Area
            {
                // Base
                uuid=dto.Uuid,
                location_id=dto.LocationId,
                
                is_active = dto.IsActive,
                component_id = ComponentId,
                created_date = Create,
                updated_date = Create,

                // extend_desc
                name = dto.Name,
                multi_occ = dto.MultiOccupancy,
                access_control = dto.AccessControl,
                occ_control = dto.OccControl,
                occ_set = dto.OccSet,
                occ_max = dto.OccMax,
                occ_down = dto.OccDown,
                occ_up = dto.OccUp,
                area_flag = dto.AreaFlag,
            };
        }

        public static void UpdateAccessArea(Area entity,AccessAreaDto dto) 
        {
            // Base
            entity.location_id = dto.LocationId;
            entity.is_active = dto.IsActive;
            entity.updated_date = DateTime.UtcNow;

            // extend_desc
            entity.name = dto.Name;
            entity.multi_occ = dto.MultiOccupancy;
            entity.access_control = dto.AccessControl;
            entity.occ_control = dto.OccControl;
            entity.occ_set = dto.OccSet;
            entity.occ_max = dto.OccMax;
            entity.occ_down = dto.OccDown;
            entity.occ_up = dto.OccUp;
            entity.area_flag = dto.AreaFlag;
        }


        #endregion

        #region Operator

        //public static OperatorDto OperatorToDto(operator entity) 
        //{
        //    return new OperatorDto
        //    {
        //        uuid = entity.uuid,
        //        LocationIds = entity.operator_location.Select(x => x.location.component_id).ToList(),
        //        is_active = entity.is_active,

        //        // extend_desc 
        //        component_id = entity.component_id,
        //        user_name = entity.user_name,
        //        email = entity.email,
        //        title = entity.title,
        //        first_name = entity.first_name,
        //        middle_name = entity.middle_name,
        //        last_name = entity.last_name,
        //        phone = entity.phone,
        //        Image = entity.image_path,
        //        role_id = entity.role_id,
        //    };
        //}

        public static Operator DtoToOperator(CreateOperatorDto dto,short ComponentId,DateTime Created)
        {
            return new Operator
            {
                operator_locations = dto.LocationIds.Select(x => new OperatorLocation { location_id=x,operator_id=ComponentId }).ToArray(),
                is_active = true,
                created_date = Created,
                updated_date = Created,

                // extend_desc
                user_id = dto.UserId,
                component_id = ComponentId,
                user_name= dto.Username,
                password= EncryptHelper.HashPassword(dto.Password),
                email= dto.Email,
                title= dto.Title,
                first_name= dto.FirstName,
                middle_name= dto.MiddleName,
                last_name= dto.LastName,
                phone= dto.Phone,
                image_path= dto.ImagePath,
                role_id= dto.RoleId,
            };
        }

        public static void UpdateOperator(Operator en,CreateOperatorDto dto) 
        {
            foreach(var id in dto.LocationIds)
            {
                en.operator_locations.Add(new OperatorLocation { location_id = id, operator_id = en.component_id });
            }
            en.updated_date = DateTime.UtcNow;

            // extend_desc
            en.component_id = dto.ComponentId;
            en.user_name = dto.Username;
            en.password = dto.Password;
            en.email = dto.Email;
            en.title = dto.Title;
            en.first_name = dto.FirstName;
            en.middle_name = dto.MiddleName;
            en.last_name = dto.LastName;
            en.phone = dto.Phone;
            en.image_path = dto.ImagePath;
            en.role_id = dto.RoleId;
        }

        public static PasswordRuleDto PasswordRuleToDto(PasswordRule en)
        {
            return new PasswordRuleDto
            {
                Len = en.len,
                IsDigit = en.is_digit,
                IsLower = en.is_lower,
                IsSymbol = en.is_symbol,
                IsUpper = en.is_upper,
                Weaks = en.weaks.Select(x => x.pattern).ToList()
            };
        }

        public static void UpdatePasswordRule(PasswordRule en,PasswordRuleDto dto)
        {
            en.len = dto.Len;
            en.is_lower = dto.IsLower;
            en.is_digit = dto.IsDigit;
            en.is_symbol = dto.IsSymbol;
            en.is_upper = dto.IsUpper;
            if (en.weaks is not null && en.weaks.Any())
            {
                en.weaks.Clear();
            }
            else
            {
                en.weaks = new List<WeakPassword>();
            }
            foreach(var w in dto.Weaks)
            {
                en.weaks.Add(new WeakPassword {pattern = w });
            }
        } 

        #endregion

        #region Role

        //public static RoleDto RoleToDto(role en) 
        //{
        //    return new RoleDto
        //    {
        //        component_id = en.component_id,
        //        name = en.name,
        //        feature = en.feature_role is not null && en.feature_role.Count > 0 ? en.feature_role.Select(x => MapperHelper.FeatureToDto(x.feature,x.is_allow,x.is_create,x.is_modify,x.is_delete,x.is_action)).ToList() : new List<FeatureDto>()
        //    };
        //}

        public static Role DtoToRole(RoleDto dto,short ComponentId,DateTime Create)
        {
            return new Role
            {
                component_id = ComponentId,
                name = dto.Name,
                feature_roles = dto.Features.Select(fr => DtoToFeatureRole(fr,ComponentId)).ToArray(),
                created_date = Create,
                updated_date = Create

            };
        }

        public static FeatureRole DtoToFeatureRole(FeatureDto dto,short RoleId) 
        {
            return new FeatureRole
            {
                feature_id = dto.ComponentId,
                role_id = RoleId,
                is_create = dto.IsCreate,
                is_delete = dto.IsDelete,
                is_modify = dto.IsModify,
                is_allow = dto.IsAllow,
                is_action = dto.IsAction,
            };
        }

        public static void UpdateRole(Role en,RoleDto dto)
        {
            en.name = dto.Name;
            en.updated_date = DateTime.UtcNow;
            en.feature_roles.Clear();
            foreach(var id in dto.Features)
            {
                en.feature_roles.Add(DtoToFeatureRole(id,dto.component_id));
            }
        }

        #endregion

        #region Feature

        public static FeatureDto FeatureToDto(Feature fn,bool isAllow, bool isCreate,bool isModify,bool isDelete,bool isAction)
        {
            return new FeatureDto
            {
                ComponentId = fn.component_id,
                Name = fn.name,
                Path = fn.path,
                SubItems = fn.sub_feature is null ? new List<SubFeatureDto>() : fn.sub_feature.Select(x => SubFeatureToDto(x)).ToList(),
                IsAllow = isAllow,
                IsCreate = isCreate,
                IsModify = isModify,
                IsDelete = isDelete,
                IsAction = isAction
            };
        }

        public static SubFeatureDto SubFeatureToDto(SubFeature s) 
        {
            return new SubFeatureDto
            {
                Path = s.path,
                Name = s.name
            };
        }

        #endregion

        #region Transaction




        #endregion

        #region Procedure

       
        
        public static Procedure DtoToProcedure(ProcedureDto dto,short ComponentId,DateTime Create)
        {
            return new Procedure
            {
                // Base
                component_id = ComponentId,
                location_id = dto.LocationId,
                is_active = true,
                updated_date = Create,
                created_date = Create,

                // Detail
                name = dto.Name,
                actions = dto.Actions
                .Select(x => DtoToAction(x,ComponentId,DateTime.UtcNow))
                .ToArray(),    
            };
        }

        public static Entity.Action DtoToAction(ActionDto dto,short ComponentId,DateTime Create)
        {
            return new Entity.Action 
            {
                // Base
                component_id = ComponentId,
                location_id = dto.LocationId,
                is_active = true,
                updated_date = Create,
                created_date = Create,

                // Detail
                hardware_id = dto.ScpId,
                procedure_id = ComponentId,
                action_type = dto.ActionType,
                action_type_desc = dto.ActionTypeDesc,
                arg1 = dto.Arg1,
                arg2 = dto.Arg2,
                arg3 = dto.Arg3,
                arg4 = dto.Arg4,
                arg5 = dto.Arg5,
                arg6 = dto.Arg6,
                arg7 = dto.Arg7,
                str_arg = dto.StrArg,
                delay_time = dto.DelayTime,

            };
        }


        #endregion

        #region Trigger

        public static TriggerDto TriggerToDto(Entity.Trigger en)
        {
            return new TriggerDto
            {
                // Base
                Uuid = en.uuid,
                LocationId = en.location_id,
                Mac = en.hardware_mac,
                HardwareName = en.hardware.name,
                IsActive = en.is_active,

                // Detail
                Name = en.name,
                Command = en.command,
                ProcedureId = en.procedure_id,
                SourceNumber = en.source_number,
                SourceType = en.source_type,
                TranType = en.tran_type,
                CodeMap = en.code_map.Select(x => new TransactionCodeDto 
                {
                    Name = x.name,
                    Value = x.value,
                    Description = x.description
                }).ToList(),
                TimeZone = en.timezone,
            };
        }

        public static Trigger DtoToTrigger(TriggerDto dto,short ComponentId,DateTime Create)
        {
            return new Trigger
            {
                // Base
                location_id = dto.LocationId,
                hardware_mac = dto.Mac,
                is_active = true,
                created_date = Create,
                updated_date = Create,

                // Detail
                component_id = ComponentId,
                name = dto.Name,
                command = dto.Command,
                procedure_id = dto.ProcedureId,
                source_number = dto.SourceNumber,
                source_type = dto.SourceType,
                tran_type = dto.TranType,
                code_map = dto.CodeMap.Select(x => new TriggerTranCode 
                {
                    trigger_id = ComponentId,
                    name = x.Name,
                    description = x.Description,
                    value = x.Value,
                }).ToArray(),
                timezone = dto.TimeZone,
            };
        }


        #endregion

    }
}
