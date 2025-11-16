using HIDAeroService.DTO;
using HIDAeroService.DTO.Hardware;
using HIDAeroService.DTO.Interval;
using HIDAeroService.DTO.Scp;
using HIDAeroService.DTO.TimeZone;
using HIDAeroService.DTO.Module;
using HIDAeroService.Entity;
using HIDAeroService.DTO.Reader;
using HIDAeroService.DTO.MonitorPoint;
using HIDAeroService.DTO.ControlPoint;
using HIDAeroService.DTO.Acr;
using HIDAeroService.DTO.Strike;
using HIDAeroService.DTO.Sensor;
using HIDAeroService.DTO.RequestExit;
using MiNET.UI;
using HIDAeroService.DTO.CardFormat;
using MiNET.Utils;
using HIDAeroService.DTO.Location;
using HIDAeroService.DTO.AccessArea;
using HIDAeroService.DTO.CardHolder;
using HIDAeroService.DTO.Credential;
using HIDAeroService.DTO.AccessLevel;

namespace HIDAeroService.Mapper
{
    public static class MapperHelper
    {

        #region Hardware

        public static HardwareDto HardwareToHardwareDto(Hardware hardware)
        {
            return new HardwareDto
            {
                // Base 
                Uuid = hardware.Uuid,
                ComponentId = hardware.ComponentId,
                MacAddress = hardware.MacAddress,
                LocationId = hardware.LocationId,
                LocationName = hardware.LocationName,
                IsActive = hardware.IsActive,

                // Detail
                Name = hardware.Name,
                Model = hardware.Model,
                IpAddress = hardware.IpAddress,
                SerialNumber = hardware.SerialNumber,
                IsReset = hardware.IsReset,
                IsUpload = hardware.IsUpload,
                Modules = hardware.Module.Select(d => ModuleToDto(d)).ToList()

            };
        }

        public static Hardware CreateToHardware(CreateHardwareDto dto)
        {
            return new Hardware
            {
                ComponentId = dto.ComponentId,
                MacAddress = dto.MacAddress,
                LocationId = dto.LocationId,
                LocationName = dto.LocationName,
                Name = dto.Name,
                Model = dto.Model,
                Module = new List<Module>
                {
                    new Module
                    {
                        // Base 
                        ComponentId = 0,
                        MacAddress = dto.MacAddress,
                        LocationId = dto.LocationId,
                        LocationName = dto.LocationName,
                        IsActive = dto.IsActive,
                        CreatedDate = dto.CreatedDate,
                        UpdatedDate = dto.UpdatedDate,

                        // Detail
                        Model = Enum.Model.HIDAeroX1100.ToString(),
                        Address = -1,
                        Port = 3,
                        nInput = (short)Enum.InputComponents.HIDAeroX1100,
                        nOutput = (short)Enum.OutputComponents.HIDAeroX1100,
                        nReader = (short)Enum.ReaderComponents.HIDAeroX1100,
                        Msp1No = 0,
                        BaudRate = -1,
                        nProtocol = 0,
                        nDialect = 0,

                    }
                },
                IpAddress = dto.IpAddress,
                SerialNumber = dto.SerialNumber,
                IsUpload = false,
                IsReset = false,
                LastSync = DateTime.Now,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now
            };
        }

        #endregion

        #region Module

        public static Module DtoToModule(ModuleDto dto,short ComponentId,DateTime Create) 
        {
            return new Module
            {
                // Base
                ComponentId = ComponentId,
                MacAddress = dto.MacAddress,
                LocationId = dto.LocationId,
                LocationName = dto.LocationName,
                IsActive = dto.IsActive,
                CreatedDate = Create,
                UpdatedDate= Create,

                // Detail
                Model = dto.Model,
                //Readers = dto.Readers.Select(x => DtoToReader(x,CreateAsync)).ToList(),
                //Sensors = dto.Sensors.Select(x => DtoToSensor(x,CreateAsync)).ToList(),
                //Strikes = dto.Strikes.Select(x => DtotoStrike(x,CreateAsync)).ToList(),
                //RequestExits = dto.RequestExits.Select(x => DtoToRequestExit(x,CreateAsync)).ToList(),
                //MonitorPoints = dto.MonitorPoints.Select(x => DtoToMonitorPoint(x,CreateAsync)).ToList(),
                //ControlPoints = dto.ControlPoints.Select(x => DtoToControlPoint(x,CreateAsync)).ToList(),
                Address = dto.Address,
                Port = dto.Port,
                nInput = dto.nInput,
                nOutput = dto.nOutput,
                nReader = dto.nReader,
                Msp1No = dto.Msp1No,
                BaudRate = dto.BaudRate,
                nProtocol = dto.nProtocol,
                nDialect = dto.nDialect,


            };
        }
        public static ModuleDto ModuleToDto(Module d) 
        {
            return new ModuleDto
            {
                // Base 
                Uuid = d.Uuid,
                ComponentId = d.ComponentId,
                MacAddress = d.MacAddress,
                LocationName = d.LocationName,
                LocationId = d.LocationId,
                IsActive = d.IsActive,

                // Detail
                Model = d.Model,
                Readers = d.Readers is null ? null : d.Readers.Select(x => ReaderToDto(x)).ToList(),
                Sensors = d.Sensors is null ? null : d.Sensors.Select(x => SensorToDto(x)).ToList(),
                Strikes = d.Strikes is null ? null : d.Strikes.Select(x => StrikeToDto(x)).ToList(),
                RequestExits = d.RequestExits is null ? null : d.RequestExits.Select(x => RequestExitToDto(x)).ToList(),
                MonitorPoints = d.MonitorPoints is null ? null :  d.MonitorPoints.Select(x => MonitorPointToDto(x)).ToList(),
                ControlPoints = d.ControlPoints is null ? null : d.ControlPoints.Select(x => ControlPointToDto(x)).ToList(),
                Address = d.Address,
                Port = d.Port,
                nInput = d.nInput,
                nOutput = d.nOutput,
                nReader = d.nReader,
                Msp1No = d.Msp1No,
                BaudRate = d.BaudRate,
                nProtocol = d.nProtocol,
                nDialect = d.nDialect,
            };
        }

        #endregion

        #region MonitorPoint

        public static MonitorPointDto MonitorPointToDto(MonitorPoint x) 
        {
            return new MonitorPointDto
            {
                // Base 
                Uuid = x.Uuid,
                ComponentId = x.ComponentId,
                MacAddress = x.MacAddress,
                LocationId = x.LocationId,
                LocationName = x.LocationName,
                IsActive = x.IsActive,

                // Detail 
                Name = x.Name,
                ModuleId = x.ModuleId,
                InputNo = x.InputNo,
                InputMode = x.InputMode,
                Debounce = x.Debounce,
                HoldTime = x.HoldTime,
                LogFunction = x.LogFunction,
                MonitorPointMode = x.MonitorPointMode,
                DelayEntry = x.DelayEntry,
                DelayExit = x.DelayExit,
                IsMask = x.IsMask,

            };
        }

        public static MonitorPoint DtoToMonitorPoint(MonitorPointDto dto,short ComponentId,DateTime Create) 
        {
            return new MonitorPoint
            {
                // Base 
                ComponentId = ComponentId,
                MacAddress = dto.MacAddress,
                LocationId = dto.LocationId,
                LocationName = dto.LocationName,
                IsActive = true,
                CreatedDate = Create,
                UpdatedDate = Create,

                // Detail 
                Name = dto.Name,
                ModuleId = dto.ModuleId,
                InputNo = dto.InputNo,
                InputMode = dto.InputMode,
                Debounce = dto.Debounce,
                HoldTime = dto.HoldTime,
                MonitorPointMode = dto.MonitorPointMode,
                LogFunction = dto.LogFunction,
                DelayEntry = dto.DelayEntry,
                DelayExit = dto.DelayExit,
                IsMask = false
            };
        }

        public static void UpdateMonitorPoint(MonitorPoint input,MonitorPointDto dto)
        {

            // Base
            input.LocationId = dto.LocationId;
            input.LocationName = dto.LocationName;
            input.IsActive = dto.IsActive;
            input.UpdatedDate = DateTime.Now;

            // Detail
            input.Name = dto.Name;
            input.ModuleId = dto.ModuleId;
            input.InputNo = dto.InputNo;
            input.InputMode = dto.InputMode;
            input.Debounce = dto.Debounce;
            input.HoldTime = dto.HoldTime;
            input.LogFunction = dto.LogFunction;
            input.MonitorPointMode = dto.MonitorPointMode;
            input.DelayEntry = dto.DelayEntry;
            input.DelayExit = dto.DelayExit;
            input.IsMask = dto.IsMask;
        }

        #endregion

        #region ControlPoint

        public static ControlPointDto ControlPointToDto(ControlPoint x)
        {
            return new ControlPointDto
            {
                // Base
                Uuid = x.Uuid,
                ComponentId = x.ComponentId,
                MacAddress = x.MacAddress,
                LocationId = x.LocationId,
                LocationName = x.LocationName,
                IsActive = x.IsActive,

                // Detail


                Name = x.Name,
                ModuleId = x.ModuleId,
                OutputNo = x.OutputNo,
                RelayMode = x.RelayMode,
                DefaultPulse = x.DefaultPulse,
            };
        }

        public static ControlPoint DtoToControlPoint(ControlPointDto dto,short ComponentId,DateTime Create)
        {
            return new ControlPoint
            {
                // Base 
                ComponentId = ComponentId,
                MacAddress = dto.MacAddress,
                LocationId = dto.LocationId,
                LocationName = dto.LocationName,
                IsActive = true,
                CreatedDate = Create,
                UpdatedDate = Create,

                // Detail
                Name = dto.Name,
                ModuleId = dto.ModuleId,
                OutputNo = dto.OutputNo,
                RelayMode = dto.RelayMode,
                OfflineMode = dto.OfflineMode,
                DefaultPulse = dto.DefaultPulse,
            };

        }

        public static void UpdateControlPoint(ControlPoint output,ControlPointDto dto)
        {

            // Base
            output.LocationId = dto.LocationId;
            output.LocationName = dto.LocationName;
            output.IsActive = dto.IsActive;
            output.UpdatedDate = DateTime.Now;

            // Detail
            output.Name = dto.Name;
            output.ModuleId = dto.ModuleId;
            output.OutputNo = dto.OutputNo;
            output.RelayMode = dto.RelayMode;
            output.OfflineMode = dto.OfflineMode;
            output.DefaultPulse = dto.DefaultPulse;
        }

        #endregion

        #region Readers

        public static ReaderDto ReaderToDto(Reader x) 
        {
            return new ReaderDto 
            {
                // Base
                Uuid = x.Uuid,
                ComponentId = x.ComponentId,
                MacAddress = x.MacAddress,
                LocationId = x.LocationId,
                LocationName = x.LocationName,
                IsActive = x.IsActive,

                // Detail
                ModuleId= x.ModuleId,
                ReaderNo = x.ReaderNo,
                DataFormat = x.DataFormat,
                KeypadMode = x.KeypadMode,
                LedDriveMode = x.LedDriveMode,
                OsdpFlag = x.OsdpFlag,
                OsdpAddress = x.OsdpAddress,
                OsdpBaudrate = x.OsdpBaudrate,
                OsdpDiscover = x.OsdpDiscover,
                OsdpSecureChannel = x.OsdpSecureChannel,
                OsdpTracing = x.OsdpTracing,
            };
        }

        public static Reader DtoToReader(ReaderDto dto,DateTime Create) 
        {
            return new Reader
            {
                // Base 
                ComponentId = dto.ComponentId,
                MacAddress = dto.MacAddress,
                LocationId = dto.LocationId,
                LocationName = dto.LocationName,
                IsActive = dto.IsActive,
                CreatedDate = Create,
                UpdatedDate = Create,

                // Detail
                ModuleId = dto.ModuleId,
                ReaderNo = dto.ReaderNo,
                DataFormat = dto.DataFormat,
                KeypadMode = dto.KeypadMode,
                LedDriveMode= dto.LedDriveMode,
                OsdpFlag = dto.OsdpFlag,
                OsdpAddress = dto.OsdpAddress,
                OsdpBaudrate= dto.OsdpBaudrate,
                OsdpDiscover= dto.OsdpDiscover,
                OsdpSecureChannel= dto.OsdpSecureChannel,
                OsdpTracing= dto.OsdpTracing,
            };
        }

        public static void UpdateReader(Reader reader,ReaderDto dto) 
        {
            // Base
            reader.MacAddress = dto.MacAddress;
            reader.LocationId = dto.LocationId;
            reader.LocationName = dto.LocationName;
            reader.IsActive = dto.IsActive;
            reader.UpdatedDate = DateTime.Now;

            // Detail
            reader.ModuleId = dto.ModuleId;
            reader.ReaderNo = dto.ReaderNo;
            reader.DataFormat = dto.DataFormat;
            reader.KeypadMode = dto.KeypadMode;
            reader.LedDriveMode = dto.LedDriveMode;
            reader.OsdpFlag = dto.OsdpFlag;
            reader.OsdpAddress = dto.OsdpAddress;
            reader.OsdpBaudrate = dto.OsdpBaudrate;
            reader.OsdpDiscover = dto.OsdpDiscover;
            reader.OsdpSecureChannel = dto.OsdpSecureChannel;
            reader.OsdpTracing = dto.OsdpTracing;
        }

        #endregion

        #region Strike

        public static StrikeDto StrikeToDto(Strike x)
        {
            return new StrikeDto
            {
                // Base 
                Uuid = x.Uuid,
                ComponentId = x.ComponentId,
                MacAddress = x.MacAddress,
                LocationId = x.LocationId,
                LocationName = x.LocationName,
                IsActive = x.IsActive,

                // Detail
                ModuleId = x.ModuleId,
                OutputNo = x.OutputNo,
                RelayMode = x.RelayMode,
                OfflineMode = x.OfflineMode,
                StrkMax = x.StrkMax,
                StrkMin = x.StrkMin,
                StrkMode = x.StrkMode,
            };
        }

        public static Strike DtotoStrike(StrikeDto dto,DateTime Create)
        {
            return new Strike
            {
                // Base
                ComponentId= dto.ComponentId,
                MacAddress= dto.MacAddress,
                LocationId= dto.LocationId,
                LocationName= dto.LocationName,
                IsActive = dto.IsActive,
                CreatedDate = Create,
                UpdatedDate = Create,

                // Detail
                ModuleId= dto.ModuleId,
                OutputNo= dto.OutputNo,
                RelayMode= dto.RelayMode,
                OfflineMode= dto.OfflineMode,
                StrkMax= dto.StrkMax,
                StrkMin= dto.StrkMin,
                StrkMode= dto.StrkMode,
            };
        }

        #endregion

        #region RequestExit

        public static RequestExitDto RequestExitToDto(RequestExit x) 
        {
            return new RequestExitDto 
            {
                // Base
                Uuid = x.Uuid,
                ComponentId = x.ComponentId,
                MacAddress = x.MacAddress,
                LocationId = x.LocationId,
                LocationName = x.LocationName,
                IsActive = x.IsActive,

                // Detail
                ModuleId= x.ModuleId,
                InputNo = x.InputNo,
                InputMode = x.InputMode,
                Debounce = x.Debounce,
                HoldTime = x.HoldTime,
                MaskTimeZone = x.MaskTimeZone,
            };
        }

        public static RequestExit DtoToRequestExit(RequestExitDto dto,DateTime Create) 
        {
            return new RequestExit
            {
                // Base 
                ComponentId= dto.ComponentId,
                MacAddress= dto.MacAddress,
                LocationId= dto.LocationId,
                LocationName= dto.LocationName,
                IsActive= dto.IsActive,
                UpdatedDate = Create,
                CreatedDate = Create,

                // Detail
                ModuleId = dto.ModuleId,
                InputNo= dto.InputNo,
                InputMode= dto.InputMode,
                Debounce = dto.Debounce,
                HoldTime= dto.HoldTime,
                MaskTimeZone= dto.MaskTimeZone,
            };
        }

        #endregion

        #region Sensor

        public static SensorDto SensorToDto(Sensor x) 
        {
            return new SensorDto
            {

                // Base 
                Uuid = x.Uuid,
                ComponentId = x.ComponentId,
                MacAddress = x.MacAddress,
                LocationId = x.LocationId,
                LocationName = x.LocationName,
                IsActive = x.IsActive,

                // Detail
                ModuleId= x.ModuleId,
                InputNo = x.InputNo,
                InputMode = x.InputMode,
                Debounce = x.Debounce,
                HoldTime = x.HoldTime,
                DcHeld = x.DcHeld,

            };
        }

        public static Sensor DtoToSensor(SensorDto dto,DateTime Create) 
        {
            return new Sensor
            {
                // Base
                ComponentId = dto.ComponentId,
                MacAddress = dto.MacAddress,
                LocationId = dto.LocationId,
                LocationName = dto.LocationName,
                IsActive = dto.IsActive,
                CreatedDate = Create,
                UpdatedDate = Create,

                // Detail
                ModuleId = dto.ModuleId,
                InputNo = dto.InputNo,
                InputMode = dto.InputMode,
                Debounce = dto.Debounce,
                HoldTime = dto.HoldTime,
                DcHeld = dto.DcHeld,
            };
        }

        #endregion

        #region Doors

        public static DoorDto DoorToDto(Door x) 
        {
            return new DoorDto
            {
                // Base 
                Uuid = x.Uuid,
                ComponentId = x.ComponentId,
                MacAddress = x.MacAddress,
                LocationId = x.LocationId,
                LocationName = x.LocationName,
                IsActive = x.IsActive,

                // Detail
                Name = x.Name,
                AccessConfig = x.AccessConfig,
                PairDoorNo = x.PairDoorNo,

                // Reader
                Readers = x.Readers is null ? new List<ReaderDto>() : x.Readers
                .Select(x => ReaderToDto(x))
                .ToList(),
                ReaderOutConfiguration = x.ReaderOutConfiguration,

                // Strike
                StrkComponentId = x.StrkComponentId,
                Strk = x.Strk is null ? null : StrikeToDto(x.Strk),

                // Sensor
                SensorComponentId = x.SensorComponentId,
                Sensor = x.Sensor is null ? null : SensorToDto(x.Sensor),

                // Request Exit
                RequestExits = x.RequestExits is null ? new List<RequestExitDto>() :  x.RequestExits
                .Select(x => RequestExitToDto(x))
                .ToList(),


                CardFormat = x.CardFormat,
                AntiPassbackMode = x.AntiPassbackMode,
                AntiPassBackIn = x.AntiPassBackIn,
                AntiPassBackOut = x.AntiPassBackOut,
                SpareTags = x.SpareTags,
                AccessControlFlags = x.AccessControlFlags,
                Mode = x.Mode,
                ModeDesc = x.ModeDesc,
                OfflineMode = x.OfflineMode,
                OfflineModeDesc = x.OfflineModeDesc,
                DefaultMode = x.DefaultMode,
                DefaultModeDesc = x.DefaultModeDesc,
                DefaultLEDMode = x.DefaultLEDMode,
                PreAlarm = x.PreAlarm,
                AntiPassbackDelay = x.AntiPassbackDelay,
                StrkT2 = x.StrkT2,
                DcHeld2 = x.DcHeld2,
                StrkFollowPulse = x.StrkFollowPulse,
                StrkFollowDelay = x.StrkFollowDelay,
                nExtFeatureType = x.nExtFeatureType,
                IlPBSio = x.IlPBSio,
                IlPBNumber = x.IlPBNumber,
                IlPBLongPress = x.IlPBLongPress,
                IlPBOutSio = x.IlPBOutSio,
                IlPBOutNum = x.IlPBOutNum,
                DfOfFilterTime = x.DfOfFilterTime,
                MaskForceOpen = x.MaskForceOpen,
                MaskHeldOpen = x.MaskHeldOpen,

            };
        }

        public static Door DtoToDoor(
            DoorDto dto,
            short ComponentId,
            string ModeDesc,
            string OfflineModeDesc,
            string DefaultModeDesc,
            DateTime Create) 
        {
            return new Door
            {
                // Base
                ComponentId = ComponentId,
                MacAddress = dto.MacAddress,
                LocationId = dto.LocationId,
                LocationName = dto.LocationName,
                IsActive = true,
                CreatedDate = Create,
                UpdatedDate = Create,


                // Detail
                Name = dto.Name,
                AccessConfig = dto.AccessConfig,
                PairDoorNo = dto.PairDoorNo,
                Readers = dto.Readers
                .Where(x => !string.IsNullOrEmpty(x.MacAddress))
                .Select(x => DtoToReader(x,Create))
                .ToList(),

                Strk = DtotoStrike(dto.Strk,Create),
                Sensor = DtoToSensor(dto.Sensor,Create),
                RequestExits =  dto.RequestExits is null ? new List<RequestExit>() : dto.RequestExits
                .Where(x => !string.IsNullOrEmpty(x.MacAddress))
                .Select(x => DtoToRequestExit(x,Create))
                .ToList(),

                ReaderOutConfiguration = dto.ReaderOutConfiguration,
                CardFormat = dto.CardFormat,
                AntiPassbackMode = dto.AntiPassbackMode,
                AntiPassBackIn = dto.AntiPassBackIn,
                AntiPassBackOut = dto.AntiPassBackOut,
                SpareTags = dto.SpareTags,
                AccessControlFlags = dto.AccessControlFlags,
                Mode = dto.Mode,
                ModeDesc = ModeDesc,
                OfflineMode = dto.OfflineMode,
                OfflineModeDesc = OfflineModeDesc,
                DefaultMode = dto.DefaultMode,
                DefaultModeDesc = DefaultModeDesc,
                DefaultLEDMode = dto.DefaultLEDMode,
                PreAlarm = dto.PreAlarm,
                AntiPassbackDelay = dto.AntiPassbackDelay,
                StrkT2 = dto.StrkT2,
                DcHeld2 = dto.DcHeld2,
                StrkFollowPulse = dto.StrkFollowPulse,
                StrkFollowDelay = dto.StrkFollowDelay,
                nExtFeatureType = dto.nExtFeatureType,
                IlPBSio = dto.IlPBSio,
                IlPBNumber = dto.IlPBNumber,
                IlPBLongPress = dto.IlPBLongPress,
                IlPBOutSio = dto.IlPBOutSio,
                IlPBOutNum = dto.IlPBOutNum,
                DfOfFilterTime = dto.DfOfFilterTime,
                MaskHeldOpen = dto.MaskHeldOpen,
                MaskForceOpen = dto.MaskForceOpen,
                

            };
        }

        public static void UpdateDoor(Door door,DoorDto dto,string ModeDesc,string OfflineModeDesc,string DefaultModeDesc) 
        {
            DateTime time = DateTime.Now;
            // Base

            door.ComponentId = dto.ComponentId;
            door.MacAddress = dto.MacAddress;
            door.LocationId = dto.LocationId;
            door.LocationName = dto.LocationName;
            door.IsActive = true;
            door.UpdatedDate = time;

            // Detail
            door.Name = dto.Name;
            door.AccessConfig = dto.AccessConfig;
            door.PairDoorNo = dto.PairDoorNo;
            door.Readers = dto.Readers.Select(s => DtoToReader(s, time)).ToList();
            door.Strk = DtotoStrike(dto.Strk, time);
            door.Sensor = DtoToSensor(dto.Sensor, time);
            if(dto.RequestExits is not null && dto.RequestExits.Count > 0)
            {
                door.RequestExits = dto.RequestExits.Select(s => DtoToRequestExit(s,time)).ToList();
            }
            door.ReaderOutConfiguration = dto.ReaderOutConfiguration;
            door.CardFormat = dto.CardFormat;
            door.AntiPassbackMode = dto.AntiPassbackMode;
            door.AntiPassBackIn = dto.AntiPassBackIn;
            door.AntiPassBackOut = dto.AntiPassBackOut;
            door.SpareTags = dto.SpareTags;
            door.AccessControlFlags = dto.AccessControlFlags;
            door.Mode = dto.Mode;
            door.ModeDesc = ModeDesc;
            door.OfflineMode = dto.OfflineMode;
            door.OfflineModeDesc = OfflineModeDesc;
            door.DefaultMode = dto.DefaultMode;
            door.DefaultModeDesc = DefaultModeDesc;
            door.DefaultLEDMode = dto.DefaultLEDMode;
            door.PreAlarm = dto.PreAlarm;
            door.AntiPassbackDelay = dto.AntiPassbackDelay;
            door.StrkT2 = dto.StrkT2;
            door.DcHeld2 = dto.DcHeld2;
            door.StrkFollowPulse = dto.StrkFollowPulse;
            door.StrkFollowDelay = dto.StrkFollowDelay;
            door.nExtFeatureType = dto.nExtFeatureType;
            door.IlPBSio = dto.IlPBSio;
            door.IlPBNumber = dto.IlPBNumber;
            door.IlPBLongPress = dto.IlPBLongPress;
            door.IlPBOutSio = dto.IlPBOutSio;
            door.IlPBOutNum = dto.IlPBOutNum;
            door.DfOfFilterTime = dto.DfOfFilterTime;
            door.MaskForceOpen = dto.MaskForceOpen;
            door.MaskHeldOpen = dto.MaskHeldOpen;
        }


        #endregion

        #region CardHolder

        public static CardHolderDto CardHolderToDto(CardHolder entity)
        {
            return new CardHolderDto 
            {
                // Base
                Uuid = entity.Uuid,
                LocationId = entity.LocationId,
                LocationName = entity.LocationName,
                IsActive = entity.IsActive,

                // Detail
                UserId = entity.UserId,
                Title = entity.Title,
                FirstName = entity.FirstName,
                MiddleName = entity.MiddleName,
                LastName = entity.LastName,
                Sex = entity.Sex,
                Email = entity.Email,
                Phone = entity.Phone,
                Company = entity.Company,
                Position = entity.Position,
                Department = entity.Department,
                ImagePath = entity.ImagePath,
                Additionals = entity.Additional
                .Where(x => x.HolderId == entity.UserId)
                .Select(x => x.Additional).ToList(),
                Credentials = entity.Credentials
                .Select(x => CredentialToDto(x)).ToList(),
                
            };
        }

        public static CardHolder DtoToCardHolder(CardHolderDto dto,List<short> ComponentIds,DateTime Create)
        {
            return new CardHolder
            {
                // Base
                LocationId = dto.LocationId,
                LocationName = dto.LocationName,
                IsActive = true,
                CreatedDate = Create,
                UpdatedDate = Create,

                // Detail
                UserId = dto.UserId,
                Title = dto.Title,
                FirstName = dto.FirstName,
                MiddleName = dto.MiddleName,
                LastName = dto.LastName,
                Sex = dto.Sex,
                Email = dto.Email,
                Phone = dto.Phone,
                Company = dto.Company,
                Position = dto.Position,
                Department = dto.Department,
                ImagePath = dto.ImagePath,
                Additional = dto.Additionals
                .Select(x => DtoToAdditional(dto.UserId, x))
                .ToList(),
                Credentials = dto.Credentials.Select((x, i) => DtoToCredential(x, ComponentIds[i], Create)).ToList(),
                CardHolderAccessLevels = dto.AccessLevels is not null ? dto.AccessLevels.Select(x => DtoToCardHolderAccessLevel(x.ComponentId, dto.UserId)).ToList() : new List<CardHolderAccessLevel>()
            };
        }

        public static CardHolderAccessLevel DtoToCardHolderAccessLevel(short AccessLevelId,string UserId)
        {
            return new CardHolderAccessLevel
            {
                AccessLevelId = AccessLevelId,
                CardHolderId = UserId
            };
        }

        public static void UpdateCardHolder(CardHolder entity,CardHolderDto dto,List<short> ComponentId) 
        {
            // Detial
            entity.LocationId = dto.LocationId;
            entity.LocationName = dto.LocationName;
            entity.IsActive = dto.IsActive;
            entity.UpdatedDate = DateTime.Now;

            // Base
            entity.UserId = dto.UserId;
            entity.Title = dto.Title;
            entity.FirstName = dto.FirstName;
            entity.MiddleName = dto.MiddleName;
            entity.LastName = dto.LastName;
            entity.Sex = dto.Sex;
            entity.Email = dto.Email;
            entity.Phone = dto.Phone;
            entity.Company = dto.Company;
            entity.Position = dto.Position;
            entity.Department = dto.Department;
            entity.Additional = dto.Additionals
                .Select(x => MapperHelper.DtoToAdditional(dto.UserId, x))
                .ToArray();
            entity.ImagePath = dto.ImagePath;
            entity.Credentials = dto.Credentials
                .Select((x,i) => MapperHelper.DtoToCredential(x, ComponentId[i], DateTime.Now))
                .ToArray();
        }

        public static CardHolderAdditional DtoToAdditional(string HolderId,string Additional)
        {
            return new CardHolderAdditional
            {
                HolderId = HolderId,
                Additional = Additional
            };
        }

        #endregion

        #region Credential

        public static CredentialDto CredentialToDto(Credential entity) 
        {
            return new CredentialDto
            {
                // Base
                Uuid = entity.Uuid,
                LocationId = entity.LocationId,
                LocationName = entity.LocationName,
                IsActive = entity.IsActive,

                // Detail
                ComponentId = entity.ComponentId,
                Flag = entity.Flag,
                Bits = entity.Bits,
                IssueCode = entity.IssueCode,
                FacilityCode = entity.FacilityCode,
                CardNo = entity.CardNo,
                Pin = entity.Pin,
                ActiveDate = entity.ActiveDate,
                DeactiveDate = entity.DeactiveDate,
                //CardHolder = entity.CardHolder is not null ? CardHolderToDto(entity.CardHolder) : null,

            };
        }

        public static Credential DtoToCredential(CredentialDto dto,short ComponentId,DateTime Create)
        {
            return new Credential 
            {
                // Base
                LocationId = dto.LocationId,
                LocationName = dto.LocationName,
                IsActive = true,
                CreatedDate = Create,
                UpdatedDate = Create,

                // Detail
                Flag = dto.Flag,
                ComponentId = ComponentId,
                Bits = dto.Bits,
                IssueCode = dto.IssueCode,
                FacilityCode = dto.FacilityCode,
                CardNo = dto.CardNo,
                Pin = dto.Pin,
                ActiveDate = dto.ActiveDate,
                DeactiveDate = dto.DeactiveDate,
            };
        }

        public static ModeDto CredentialFlagToDto(CredentialFlag flag)
        {
            return new ModeDto
            {
                Name= flag.Name,
                Description = flag.Description,
                Value = flag.Value,
            };
        }



        #endregion

        #region AccessLevel

        public static AccessLevelDto AccessLevelToDto(AccessLevel entity)
        {
            return new AccessLevelDto
            {
                // Base
                Uuid = entity.Uuid,
                LocationId = entity.LocationId,
                LocationName = entity.LocationName,
                IsActive = entity.IsActive,

                // Detail
                Name = entity.Name,
                ComponentId = entity.ComponentId,
                AccessLevelDoorTimeZoneDto = entity.AccessLevelDoorTimeZones
                .Select(x => AccessLevelDoorTimeZoneToDto(x))
                .ToList()

            };
        }

        public static AccessLevel DtoToAccessLevel(CreateUpdateAccessLevelDto dto,short ComponentId,DateTime Create)
        {
            return new AccessLevel 
            {
                // Base 
                LocationId= dto.LocationId,
                LocationName= dto.LocationName,
                IsActive=true,
                CreatedDate= Create,
                UpdatedDate= Create,

                // Detail
                ComponentId= ComponentId,
                Name= dto.Name,
                AccessLevelDoorTimeZones=dto.CreateUpdateAccessLevelDoorTimeZoneDto
                .Select(x => DtoToAccessLevelDoorTimeZone(x,ComponentId))
                .ToArray()
            };
        }

        public static void UpdateAccessLeve(AccessLevel entity,CreateUpdateAccessLevelDto dto)
        {
            // Base
            entity.LocationId = dto.LocationId;
            entity.LocationName = dto.LocationName;
            entity.IsActive = dto.IsActive;
            entity.UpdatedDate = DateTime.Now;

            // Detail
            entity.Name = dto.Name;
            entity.AccessLevelDoorTimeZones = dto.CreateUpdateAccessLevelDoorTimeZoneDto
                .Select(x => DtoToAccessLevelDoorTimeZone(x,dto.ComponentId))
                .ToArray();
        }

        public static AccessLevelDoorTimeZone DtoToAccessLevelDoorTimeZone(CreateUpdateAccessLevelDoorTimeZoneDto dto,short ComponentId) 
        {
            return new AccessLevelDoorTimeZone
            {
                AccessLevelId = ComponentId,
                TimeZoneId = dto.TimeZoneId,
                DoorId=dto.DoorId
            };
        }

        public static AccessLevelDoorTimeZoneDto AccessLevelDoorTimeZoneToDto(AccessLevelDoorTimeZone acss) 
        {
            return new AccessLevelDoorTimeZoneDto
            {
                Doors = DoorToDto(acss.Door),
                TimeZone = TimeZoneToDto(acss.TimeZone),
            };
        }

        #endregion

        #region TimeZone

        public static TimeZoneDto TimeZoneToDto(Entity.TimeZone t)
        {
            return new TimeZoneDto 
            {
                // Base
                LocationId = t.LocationId,
                LocationName = t.LocationName,
                IsActive = t.IsActive,

                // Detail
                ComponentId = t.ComponentId,
                Name = t.Name,
                Mode = t.Mode,
                ActiveTime = t.ActiveTime,
                DeactiveTime = t.DeactiveTime,
                Intervals = t.TimeZoneIntervals is null ? null : t.TimeZoneIntervals
                .Select(s => s.Interval)
                .Select(s => IntervalToIntervalDto(s))
                .ToList(),

            };
        }

        public static Entity.TimeZone TimeZoneDtoToTimeZone(TimeZoneDto t)
        {
            return new Entity.TimeZone 
            {
                // Base
                LocationId = t.LocationId,
                LocationName = t.LocationName,
                IsActive = t.IsActive,

                // detail
                ComponentId= t.ComponentId,
                Name = t.Name,
                Mode = t.Mode,
                ActiveTime = t.ActiveTime,
                DeactiveTime = t.DeactiveTime,
            };
        }

        public static Entity.TimeZone CreateTimeZoneDtoToTimeZone(CreateTimeZoneDto t,short ComponentId)
        {
            return new Entity.TimeZone
            {
                // Base
                LocationId = t.LocationId,
                LocationName = t.LocationName,
                IsActive = t.IsActive,

                // detail
                ComponentId = ComponentId,
                Name = t.Name,
                Mode = t.Mode,
                ActiveTime = t.ActiveTime,
                DeactiveTime = t.DeactiveTime,
            };
        }

        public static Entity.TimeZone TimeZoneDtoMapTimeZone(TimeZoneDto dto,Entity.TimeZone entity)
        {
            entity.Name = dto.Name;
            entity.Mode = dto.Mode;
            entity.ActiveTime = dto.ActiveTime;
            entity.DeactiveTime = dto.DeactiveTime;
            entity.IsActive = dto.IsActive;
            entity.UpdatedDate = DateTime.Now;
            return entity;
          
        }

        #endregion

        #region Interval

        public static IntervalDto IntervalToIntervalDto(Interval p)
        {
            return new IntervalDto
            {
                // Base 
                LocationId = p.LocationId,
                LocationName = p.LocationName,
                IsActive = p.IsActive,

                // Detail
                ComponentId = p.ComponentId,
                DaysDesc = p.DaysDesc,
                StartTime = p.StartTime,
                EndTime = p.EndTime,
                Days = new DaysInWeekDto
                {
                    Sunday = p.Days.Sunday,
                    Monday = p.Days.Monday,
                    Tuesday = p.Days.Tuesday,
                    Wednesday = p.Days.Wednesday,
                    Thursday = p.Days.Thursday,
                    Friday = p.Days.Friday,
                    Saturday = p.Days.Saturday
                }

            };
        }

        public static Interval IntervalDtoToInterval(IntervalDto dto)
        {
            return new Interval 
            {
                // Base
                Uuid = dto.Uuid,
                LocationId = dto.LocationId,
                LocationName = dto.LocationName,
                IsActive = dto.IsActive,

                // Detail
                ComponentId = dto.ComponentId,
                DaysDesc = dto.DaysDesc,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                Days = new DaysInWeek 
                {
                    Sunday = dto.Days.Sunday,
                    Monday = dto.Days.Monday,
                    Tuesday = dto.Days.Tuesday,
                    Wednesday = dto.Days.Wednesday,
                    Thursday= dto.Days.Thursday,
                    Friday = dto.Days.Friday,
                    Saturday= dto.Days.Saturday,
                }
            };
        }

        public static Interval CreateToInterval(CreateIntervalDto dto,short componentId,string DaysDesc)
        {
            return new Interval
            {
                // Base 
                LocationId = dto.LocationId,
                LocationName = dto.LocationName,
                IsActive = true,
                UpdatedDate = DateTime.Now,

                // Detail
                ComponentId = componentId,
                DaysDesc = DaysDesc,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                Days = new DaysInWeek
                {
                    Sunday = dto.Days.Sunday,
                    Monday = dto.Days.Monday,
                    Tuesday = dto.Days.Tuesday,
                    Wednesday = dto.Days.Wednesday,
                    Thursday = dto.Days.Thursday,
                    Friday = dto.Days.Friday,
                    Saturday = dto.Days.Saturday,
                }
            };
        }

        public static Interval IntervalDtoMapInterval(IntervalDto dto,Interval interval) 
        {
            // Base 
            interval.LocationId = dto.LocationId;
            interval.LocationName = dto.LocationName;
            interval.IsActive = dto.IsActive;
            interval.UpdatedDate = DateTime.Now;

            // Detail
            interval.ComponentId = dto.ComponentId;
            interval.Days.Sunday = dto.Days.Sunday;
            interval.Days.Monday = dto.Days.Monday;
            interval.Days.Tuesday = dto.Days.Tuesday;
            interval.Days.Wednesday = dto.Days.Wednesday;
            interval.Days.Thursday = dto.Days.Thursday;
            interval.Days.Friday = dto.Days.Friday;
            interval.Days.Saturday = dto.Days.Saturday;
            interval.DaysDesc = dto.DaysDesc;
            interval.StartTime = dto.StartTime;
            interval.EndTime = dto.EndTime;

            return interval;
        }

        #endregion

        #region CardFormat

        public static CardFormat DtoToCardFormat(CardFormatDto dto,short ComponentId,DateTime Create) 
        {
            return new CardFormat
            {
                // Base 
                Uuid = dto.Uuid,
                LocationId = dto.LocationId,
                LocationName = dto.LocationName,
                IsActive = dto.IsActive,
                CreatedDate = Create,
                UpdatedDate = Create,

                // Detail
                Name = dto.Name,
                ComponentId = ComponentId,
                Facility = dto.Facility,
                Bits = dto.Bits,
                PeLn = dto.PeLn,
                PeLoc = dto.PeLoc,
                PoLn = dto.PoLn,
                PoLoc = dto.PoLoc,
                FcLn = dto.FcLn,
                FcLoc = dto.FcLoc,
                ChLn = dto.ChLn,
                ChLoc = dto.ChLoc,
                IcLn = dto.IcLn,
                IcLoc = dto.IcLoc,

            };

        }

        public static CardFormatDto CardFormatToDto(CardFormat x)
        {
            return new CardFormatDto
            {
                // Baes 
                Uuid = x.Uuid,
                LocationId = x.LocationId,
                LocationName = x.LocationName,
                IsActive = x.IsActive,

                // Detail
                Name = x.Name,
                ComponentId = x.ComponentId,
                Facility = x.Facility,
                Bits = x.Bits,
                PeLn = x.PeLn,
                PeLoc = x.PeLoc,
                PoLn = x.PoLn,
                PoLoc = x.PoLoc,
                FcLn = x.FcLn,
                FcLoc = x.FcLoc,
                ChLn = x.ChLn,
                ChLoc = x.ChLoc,
                IcLn = x.IcLn,
                IcLoc = x.IcLoc,

            };

        }

        public static void UpdateCardFormat(CardFormat card,CardFormatDto dto)
        {
            card.LocationId = dto.LocationId;
            card.LocationName = dto.LocationName;
            card.IsActive = dto.IsActive;
            card.UpdatedDate = DateTime.Now;

            card.Name = dto.Name;
            card.ComponentId = dto.ComponentId;
            card.Facility = dto.Facility;
            card.Bits = dto.Bits;
            card.PeLn = dto.PeLn;
            card.PeLoc = dto.PeLoc;
            card.PoLn = dto.PoLn;
            card.PoLoc = dto.PoLoc;
            card.FcLn = dto.FcLn;
            card.FcLoc = dto.FcLoc;
            card.ChLn = dto.ChLn;
            card.ChLoc = dto.ChLoc;
            card.IcLn = dto.IcLn;
            card.IcLoc = dto.IcLoc;
        }

        #endregion

        #region Event

        public static EventDto EventToEventDto(ArEvent even) 
        {
            EventDto dto = new EventDto();
            dto.Date = even.Date;
            dto.Time = even.Time;
            //dto.SerialNumber = even.serial_number;
            dto.Source = even.Source;
            dto.SourceNumber = even.SourceNo;
            //dto.Type = even.type;
            dto.Description = even.Description;
            dto.Additional = even.Additional;
            return dto;

        }

        #endregion

        #region Location

        public static Location DtoToLocation(LocationDto dto,short LocationId,DateTime Create)
        {
            return new Location
            {
                Uuid = dto.Uuid,
                ComponentId = LocationId,
                LocationName = dto.LocationName,
                Description = dto.Description,
                CreatedDate = Create,
                UpdatedDate = Create,
            };
        }

        public static LocationDto LocationToDto(Location location)
        {
            return new LocationDto
            {
                Uuid=location.Uuid,
                LocationId=location.ComponentId,
                LocationName=location.LocationName,
                Description=location.Description,
            };
        }

        public static void UpdateLocation(Location location,LocationDto dto) 
        {
            location.LocationName = dto.LocationName;
            location.Description = dto.Description;
            location.UpdatedDate = DateTime.Now;
        }

        #endregion

        #region Access Area

        public static AccessAreaDto AccessAreaToDto(AccessArea entity)
        {
            return new AccessAreaDto
            {
                // Base
                Uuid = entity.Uuid,
                LocationId = entity.LocationId,
                LocationName = entity.LocationName,
                IsActive = entity.IsActive,
                ComponentId = entity.ComponentId,
                MacAddress = entity.MacAddress,

                // Detail
                Name = entity.Name,
                MultiOccupancy = entity.MultiOccupancy,
                AccessControl = entity.AccessControl,
                OccControl = entity.OccControl,
                OccSet = entity.OccSet,
                OccMax = entity.OccMax,
                OccDown = entity.OccDown,
                OccUp = entity.OccUp,
                AreaFlag = entity.AreaFlag,
            };
        }

        public static AccessArea DtoToAccessArea(AccessAreaDto dto,short ComponentId,DateTime Create) 
        {
            return new AccessArea
            {
                // Base
                Uuid=dto.Uuid,
                LocationId=dto.LocationId,
                LocationName = dto.LocationName,
                IsActive = dto.IsActive,
                MacAddress = dto.MacAddress,
                ComponentId = ComponentId,
                CreatedDate = Create,
                UpdatedDate = Create,

                // Detail
                Name = dto.Name,
                MultiOccupancy = dto.MultiOccupancy,
                AccessControl = dto.AccessControl,
                OccControl = dto.OccControl,
                OccSet = dto.OccSet,
                OccMax = dto.OccMax,
                OccDown = dto.OccDown,
                OccUp = dto.OccUp,
                AreaFlag = dto.AreaFlag,
            };
        }

        public static void UpdateAccessArea(AccessArea entity,AccessAreaDto dto) 
        {
            // Base
            entity.LocationId = dto.LocationId;
            entity.LocationName = dto.LocationName;
            entity.IsActive = dto.IsActive;
            entity.MacAddress = dto.MacAddress;
            entity.UpdatedDate = DateTime.Now;

            // Detail
            entity.Name = dto.Name;
            entity.MultiOccupancy = dto.MultiOccupancy;
            entity.AccessControl = dto.AccessControl;
            entity.OccControl = dto.OccControl;
            entity.OccSet = dto.OccSet;
            entity.OccMax = dto.OccMax;
            entity.OccDown = dto.OccDown;
            entity.OccUp = dto.OccUp;
            entity.AreaFlag = dto.AreaFlag;
        }

        #endregion

    }
}
