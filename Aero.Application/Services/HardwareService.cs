using Aero.Api.Constants;
using Aero.Application.Constants;
using Aero.Application.DTOs;
using Aero.Application.Helpers;
using Aero.Application.Interface;
using Aero.Application.Interfaces;
using Aero.Domain.Entities;
using Aero.Domain.Enums;
using Aero.Domain.Interface;
using Aero.Domain.Interfaces;
using System.Net;
using System.Security.Cryptography;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Aero.Application.Services
{
    public class DeviceService(
        IDeviceRepository repo,
        IModuleRepository moduleRepo,
        ITzRepository tzRepo,
        IAlvlRepository alvlRepo,
        ICfmtRepository cfmtRepo,
        ICpRepository cpRepo,
        IMpgRepository mpgRepo,
        IMpRepository mpRepo,
        IDoorRepository doorRepo,
        IUserRepository userRepo,
        IAreaRepository areaRepo,
        IHolRepository holRepo,
        IIntervalRepository intervalRepo,
        ITriggerRepository trigRepo,
        IProcedureRepository procRepo,
        IActionRepository actionRepo,
        IIdReportRepository idRepo,
        IScpCommand scp,
        ISioCommand sio,
        IMpCommand mp,
        ITzCommand tz,
        IAlvlCommand alvl,
        ICfmtCommand cfmt,
        ICpCommand cp,
        IMpgCommand mpg,
        IDoorCommand d,
        IUserCommand holder,
        INotificationPublisher publisher,ISettingRepository setting
        ) : IDeviceService
    {

        #region CRUD 

        public async Task<ResponseDto<IEnumerable<DeviceDto>>> GetAsync()
        {
            var res = await repo.GetAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<DeviceDto>>(res);
        }
        public async Task<ResponseDto<IEnumerable<DeviceDto>>> GetByLocationAsync(short location)
        {
            var res = await repo.GetByLocationIdAsync(location);
            return ResponseHelper.SuccessBuilder<IEnumerable<DeviceDto>>(res);
        }

        public async Task<ResponseDto<DeviceDto>> DeleteAsync(int Id)
        {
            var en = await repo.GetByIdAsync(Id);

            if (en is null) return ResponseHelper.NotFoundBuilder<DeviceDto>();

            List<string> errors = new List<string>();
            // CP

            // MP

            // ACR

            // Access Area

            // modules Check first 
            if (await repo.IsAnyModuleReferenceByDriverIdAsync(en.DriverId)) return ResponseHelper.FoundReferenceBuilder<DeviceDto>();


            if (!scp.DetachScp((short)en.DriverId))
            {
                return ResponseHelper.UnsuccessBuilderWithString<DeviceDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(await repo.GetMacFromComponentAsync((short)en.DriverId), Command.DELETE_SCP));
            }


            var res = await repo.DeleteByMacAsync(en.Mac);

            return ResponseHelper.SuccessBuilder<DeviceDto>(en);

        }

        #endregion



        public async Task<ResponseDto<bool>> ResetByMacAsync(string mac)
        {
            if (!await repo.IsAnyByMac(mac)) return ResponseHelper.NotFoundBuilder<bool>();
            var id = await repo.GetComponentIdFromMacAsync(mac);
            if (id == 0) return ResponseHelper.NotFoundBuilder<bool>();
            if (!scp.ResetScp(id))
            {
                return ResponseHelper.UnsuccessBuilderWithString<bool>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(mac, Command.RESET_SCP));
            }
            return ResponseHelper.SuccessBuilder(true);
        }

        public async Task<ResponseDto<bool>> ResetByComponentAsync(short id)
        {
            string mac = await repo.GetMacFromComponentAsync(id);
            if (string.IsNullOrEmpty(mac)) return ResponseHelper.NotFoundBuilder<bool>();
            if (!scp.ResetScp(id))
            {
                return ResponseHelper.UnsuccessBuilderWithString<bool>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(mac, Command.RESET_SCP));
            }
            return ResponseHelper.SuccessBuilder(true);
        }

        public async Task<bool> VerifyHardwareConnection(short ScpId)
        {
            var setting = await repo.GetScpSettingAsync();

            if (setting is null) return false;

            if (!scp.ScpDeviceSpecification(ScpId, setting)) return false;

            return true;
        }

        public async Task<bool> MappingHardwareAndAllocateMemory(short ScpId)
        {
            var setting = await repo.GetScpSettingAsync();
            if (setting is null) return false;

            if (!scp.ScpDeviceSpecification(ScpId, setting)) return false;

            if (!scp.AccessDatabaseSpecification(ScpId, setting)) return false;

            if (!scp.TimeSet(ScpId)) return false;

            return true;
        }

        public async Task<ResponseDto<bool>> VerifyMemoryAllocateAsyncWithResponse(string Mac)
        {
            var ScpId = await repo.GetComponentIdFromMacAsync(Mac);
            if (ScpId == 0) return ResponseHelper.NotFoundBuilder<bool>();
            if (!scp.ReadStructureStatus(ScpId))
            {
                return ResponseHelper.UnsuccessBuilder<bool>(MessageBuilder.Unsuccess(Mac, Command.SCP_STRUCTURE_STATUS), []);
            }
            return ResponseHelper.SuccessBuilder<bool>(true);
        }

        public async Task<bool> VerifyMemoryAllocateAsync(string Mac)
        {
            var ScpId = await repo.GetComponentIdFromMacAsync(Mac);
            if (ScpId == 0) return false;
            if (!scp.ReadStructureStatus(ScpId))
            {
                return false;
            }
            return true;
        }


        public async Task<ResponseDto<bool>> UploadComponentConfigurationAsync(int id)
        {
            var en = await repo.GetByIdAsync(id);


            if (en is null) return ResponseHelper.NotFoundBuilder<bool>();

            List<string> errors = new List<string>();
            #region Module Upload

            // modules
            var modules = await moduleRepo.GetByDeviceIdAsync(en.DriverId);


            foreach (var module in modules)
            {
                //// command place here
                //if (!command.SioDriverConfiguration(hardware_id, modules.msp1_no, modules.port, modules.baudrate, modules.n_protocol))
                //{
                //    errors.Add(MessageBuilder.Unsuccess(mac, command.C108));
                //};

                //Enums.hardware_type.HIDAeroX1100
                //switch (modules.model)
                //{
                //    case (short)Enums.model.AeroX1100:

                //        break;
                //    case (short)Enums.model.AeroX100:
                //        break;
                //    default:
                //        break;
                //}
                // command place here
                if (!sio.SioDriverConfiguration((short)module.DeviceId, module.Msp1No, module.Port, module.BaudRate, module.nProtocol))
                {
                    errors.Add(MessageBuilder.Unsuccess(await repo.GetMacFromComponentAsync((short)module.DeviceId), Command.SIO_DRIVER));
                }
                ;

                // Enums.model.HIDAeroX1100
                if (!sio.SioPanelConfiguration((short)module.DeviceId, module.DriverId, module.Model, module.nInput, module.nOutput, module.nReader, module.Address, module.Msp1No, true))
                {
                    errors.Add(MessageBuilder.Unsuccess(await repo.GetMacFromComponentAsync((short)module.DeviceId), Command.SIO_PANEL_CONFIG));
                }
                ;

                //if (!command.SioPanelConfiguration(hardware_id, (short)modules.component_id, modules.model, modules.n_input, modules.n_output, modules.n_reader, modules.address, modules.msp1_no, true))
                //{
                //    errors.Add(MessageBuilder.Unsuccess(mac, command.C109));
                //};


                // Setting Input for Alarm 
                for (short i = 0; i < module.nInput; i++)
                {
                    if (i + 1 >= module.nInput - 3)
                    {
                        if (!mp.InputPointSpecification((short)module.DeviceId, module.DriverId, i, 0, 2, 5))
                        {
                            errors.Add(MessageBuilder.Unsuccess(await repo.GetMacFromComponentAsync((short)module.DeviceId), Command.INPUT_SPEC));
                        }
                    }

                }
            }



            #endregion

            #region Time Zone Upload

            // Timezone

            var locationId = await repo.GetLocationIdFromDriverIdAsync(en.DriverId);

            var timezones = await tzRepo.GetByLocationIdAsync(locationId);

            var timezonesdomain = timezones.Select(x => new Aero.Domain.Entities.TimeZone(
                x.Id,
                x.DriverId,
                x.Name,
                x.Mode,
                x.Active,
                x.Deactive,
                x.Intervals.Select(i => new Interval(
                    i.Id,
                    new DaysInWeek(
                        i.Days.Sunday,
                        i.Days.Monday,
                        i.Days.Tuesday,
                        i.Days.Wednesday,
                        i.Days.Thursday,
                        i.Days.Friday,
                        i.Days.Saturday
                        ),
                    i.DaysDetail,
                    i.Start,
                    i.End,
                    i.LocationId,
                    i.IsActive
                    )).ToList(),
                x.LocationId,
                x.IsActive
            )).ToList();


            foreach (var t in timezonesdomain)
            {
                if (!tz.ExtendedTimeZoneActSpecification((short)en.DriverId, t))
                {
                    errors.Add(MessageBuilder.Unsuccess(await repo.GetMacFromComponentAsync((short)en.DriverId), Command.TIMEZONE_SPEC));
                }
            }


            #endregion

            #region Access Level Upload

            // Access Level

            var accessLevels = await alvlRepo.GetByLocationIdAsync(locationId);
            var acl = accessLevels.Select(dto => new AccessLevel(
                0,
                dto.Name,
                dto.Components.Select(x => new AccessLevelComponent(x.DriverId,x.DeviceId,x.DoorId,x.AcrId,x.TimeZoneId)).ToList(),
                dto.LocationId,
                dto.IsActive
                )).ToList();

            foreach (var domain in acl)
            {
                
                if (domain.Id == 1 || domain.Id == 2)
                {
                    if (!await alvl.AccessLevelConfigurationExtended((short)en.DriverId, (short)domain.Id, domain.Id == 1 ? (short)0 : (short)1))
                    {
                        errors.Add(MessageBuilder.Unsuccess(await repo.GetMacFromComponentAsync((short)en.DriverId), Command.ALVL_CONFIG));
                    }
                    ;
                }
                else
                {
                    var device = domain.Components.Select(x => x.DeviceId).Distinct();

                    for (int i = 0; i < device.Count(); i++)
                    {
                        //var AlvlId = await qAlvl.GetLowestUnassignedNumberAsync(10, macs.ElementAt(i));
                        //domain.Components.ElementAt(i).AlvlId = AlvlId;
                        if (!await alvl.AccessLevelConfigurationExtended((short)device.ElementAt(i), domain.Components.Where(x => x.DeviceId == device.ElementAt(i)).Select(x => x.DriverId).FirstOrDefault(), domain))
                        {
                            errors.Add(MessageBuilder.Unsuccess(await repo.GetMacFromComponentAsync((short)device.ElementAt(i)), Command.ALVL_CONFIG));

                        }
                    }
                }

            }


            #endregion

            #region Control Point

            // Control Point
            var controls = await cpRepo.GetByDeviceId(en.DriverId);

            foreach (var control in controls)
            {
                // command place here
                short modeNo = await cpRepo.GetModeNoByOfflineAndRelayModeAsync(control.OfflineMode, control.RelayMode);

                if (!cp.OutputPointSpecification((short)control.DeviceId, (short)control.ModuleId, control.OutputNo, modeNo))
                {
                    errors.Add(MessageBuilder.Unsuccess(await repo.GetMacFromComponentAsync((short)control.DeviceId), Command.OUTPUT_SPEC));
                }


                if (!cp.ControlPointConfiguration((short)control.DeviceId, (short)control.ModuleId, control.DriverId, control.OutputNo, control.DefaultPulse))
                {
                    errors.Add(MessageBuilder.Unsuccess(await repo.GetMacFromComponentAsync((short)control.DeviceId), Command.CONTROL_CONFIG));

                }

            }


            #endregion

            #region Monitor Point

            // Monitor Points
            var mps = await mpRepo.GetByDeviceId(en.DriverId);

            foreach (var monitors in mps)
            {
                // command place here
                if (!mp.InputPointSpecification((short)monitors.DeviceId, monitors.ModuleId, monitors.InputNo, monitors.InputMode, monitors.Debounce, monitors.HoldTime))
                {
                    errors.Add(MessageBuilder.Unsuccess(await repo.GetMacFromComponentAsync((short)monitors.DeviceId), Command.INPUT_SPEC));
                }


                if (!mp.MonitorPointConfiguration((short)monitors.DeviceId, monitors.ModuleId, monitors.InputNo, monitors.LogFunction, monitors.MonitorPointMode, monitors.DelayEntry, monitors.DelayExit, monitors.DriverId))
                {
                    errors.Add(MessageBuilder.Unsuccess(await repo.GetMacFromComponentAsync((short)monitors.DeviceId), Command.MONITOR_CONFIG));
                }

            }


            #endregion

            #region Monitor Group

            // Monitor Group
            var mpgs = await mpgRepo.GetByDeviceIdAsync(en.DriverId);

            var mpgsdomain = mpgs.Where(x => x.DeviceId == en.DriverId).Select(dto =>  new MonitorGroup(
                dto.DeviceId,
                dto.DriverId,
                dto.Name,
                dto.nMpCount,
                dto.nMpList.Select(x => new MonitorGroupList(dto.DriverId,x.PointType,x.PointTypeDesc,x.PointNumber)).ToList(),
                dto.LocationId,
                dto.IsActive
                )).ToList();

            foreach (var mpGroup in mpgsdomain)
            {
                if (!mpg.ConfigureMonitorPointGroup((short)mpGroup.DeviceId, mpGroup.DriverId, mpGroup.nMpCount, mpGroup.nMpList.ToList()))
                {
                    errors.Add(MessageBuilder.Unsuccess(await repo.GetMacFromComponentAsync((short)mpGroup.DeviceId), Command.CONFIG_MPG));
                }
            }


            #endregion

            #region Doors

            // door
            var doors = await doorRepo.GetByDeviceIdAsync((short)en.DriverId);
            // var doors = doorss.Select(x => DoorMapper.ToDomain(x)).ToList();



            foreach (var dto in doors)
            {
                var door = new Door(dto.Id, dto.DriverId, dto.Name, dto.AccessConfig, dto.PairDoorNo,
            (DoorDirection)dto.Direction,
            dto.DeviceId,
            dto.Readers.Select(x => new Reader(x.ModuleId,x.ModuleDriverId, x.DoorId, x.ReaderNo, x.DataFormat, x.KeypadMode, x.LedDriveMode, x.OsdpFlag, x.OsdpBaudrate, x.OsdpDiscover, x.OsdpTracing, x.OsdpAddress, x.OsdpSecureChannel, x.DeviceId, x.LocationId, x.IsActive)).ToList(),
            dto.ReaderOutConfiguration,
            dto.Strk is null ? null : new Strike(dto.Strk.DeviceId, dto.Strk.ModuleId,dto.Strk.ModuleDriverId, dto.Strk.DoorId, dto.Strk.OutputNo, dto.Strk.RelayMode, dto.Strk.OfflineMode, dto.Strk.StrkMax, dto.Strk.StrkMin, dto.Strk.StrkMode, dto.Strk.LocationId, dto.Strk.IsActive),
            dto.Sensor is null ? null : new Sensor(dto.Sensor.DeviceId, dto.Sensor.ModuleId,dto.Sensor.ModuleDriverId,dto.Sensor.DoorId, dto.Sensor.InputNo, dto.Sensor.InputMode, dto.Sensor.Debounce, dto.Sensor.HoldTime, dto.Sensor.DcHeld, dto.Sensor.LocationId, dto.Sensor.IsActive),
            dto.RequestExits is null ? new List<RequestExit>() : dto.RequestExits.Select(x => new RequestExit(x.DeviceId, x.ModuleId, x.ModuleDriverId,x.DoorId, x.InputNo, x.InputMode, x.Debounce, x.HoldTime, x.MaskTimeZone, x.LocationId, x.IsActive)).ToList()
            , dto.CardFormat, dto.AntiPassbackMode, dto.AntiPassBackIn,
            dto.AreaInId, dto.AntiPassBackOut, dto.AreaOutId, dto.SpareTags, dto.AccessControlFlags, dto.Mode, dto.ModeDesc,
            dto.OfflineMode, dto.OfflineModeDesc, dto.DefaultMode, dto.DefaultModeDesc, dto.DefaultLEDMode, dto.PreAlarm, dto.AntiPassbackDelay,
            dto.StrkT2, dto.DcHeld2, dto.StrkFollowPulse, dto.StrkFollowDelay, dto.nExtFeatureType, dto.IlPBSio, dto.IlPBNumber, dto.IlPBLongPress, dto.IlPBOutSio,
            dto.IlPBOutNum, dto.DfOfFilterTime, dto.MaskHeldOpen, dto.MaskForceOpen
            );

                // command place here


                foreach (var reader in door.Readers)
                {
                    if (reader.DeviceId == 0) continue;
                    short readerInOsdpFlag = 0x00;
                    short readerLedDriveMode = 0;
                    if (reader.OsdpFlag)
                    {
                        readerInOsdpFlag |= reader.OsdpBaudrate;
                        readerInOsdpFlag |= reader.OsdpDiscover;
                        readerInOsdpFlag |= reader.OsdpTracing;
                        readerInOsdpFlag |= reader.OsdpAddress;
                        readerInOsdpFlag |= reader.OsdpSecureChannel;
                        readerLedDriveMode = 7;
                    }
                    else
                    {
                        readerLedDriveMode = 1;
                    }


                    // Reader In Config


                    if (!d.ReaderSpecification((short)reader.DeviceId,reader.ModuleDriverId, reader.ReaderNo, reader.DataFormat, reader.KeypadMode, readerLedDriveMode, readerInOsdpFlag))
                    {
                        errors.Add(MessageBuilder.Unsuccess(await repo.GetMacFromComponentAsync((short)en.DriverId), Command.READER_SPEC));
                    }
                }



                // Strike Strike Config
                if (door.Strk != null)
                {
                    if (!cp.OutputPointSpecification((short)door.Strk.DeviceId, (short)door.Strk.ModuleId, door.Strk.OutputNo, door.Strk.RelayMode))
                    {
                        errors.Add(MessageBuilder.Unsuccess(await repo.GetMacFromComponentAsync((short)en.DriverId), Command.OUTPUT_SPEC));
                    }
                }


                if (door.Sensor != null)
                {
                    if (!mp.InputPointSpecification((short)door.Sensor.DeviceId, door.Sensor.ModuleDriverId,door.Sensor.InputNo, door.Sensor.InputMode, door.Sensor.Debounce, door.Sensor.HoldTime))
                    {
                        errors.Add(MessageBuilder.Unsuccess(await repo.GetMacFromComponentAsync((short)en.DriverId), Command.INPUT_SPEC));
                    }
                }


                foreach (var rex in door.RequestExits)
                {
                    if (rex.DeviceId == 0) continue;
                    if (!mp.InputPointSpecification((short)rex.DeviceId, (short)rex.ModuleId, rex.InputNo, rex.InputMode, rex.Debounce, rex.HoldTime))
                    {
                        errors.Add(MessageBuilder.Unsuccess(await repo.GetMacFromComponentAsync((short)en.DriverId), Command.INPUT_SPEC));
                    }
                }

                if (!d.AccessControlReaderConfiguration((short)door.DeviceId, door.DriverId, door))
                {
                    errors.Add(MessageBuilder.Unsuccess(await repo.GetMacFromComponentAsync((short)en.DriverId), Command.ACR_CONFIG));
                }

            }

            #endregion

            #region Card Holder

            var cards = await userRepo.GetAsync();

            var cdomain = cards.Select(dto => new User(
                dto.UserId,
                dto.Title,
                dto.FirstName,
                dto.MiddleName,
                dto.LastName,
                (Gender)dto.Gender,
                dto.Email,
                dto.Phone,
                dto.CompanyId,
                dto.Company,
                dto.PositionId,
                dto.Position,
                dto.DepartmentId,
                dto.Department,
                dto.Image,
                dto.Flag,
                dto.Additionals,
                dto.Credentials.Select(c => new Credential(
                    c.Bits,
                    c.IssueCode,
                    c.FacilityCode,
                    c.CardNo,
                    c.Pin,
                    c.ActiveDate,
                    c.DeactiveDate,
                    dto.UserId
                )).ToList(),
                dto.AccessLevels.Select(a => new AccessLevel(
                    a.Id,
                    a.Name,
                    a.Components.Select(c => new AccessLevelComponent(
                        c.DriverId,
                        c.DeviceId,
                        c.DoorId,
                        c.AcrId,
                        c.TimeZoneId
                    )).ToList(),
                    a.LocationId,
                    a.IsActive
                )).ToList(),
                dto.LocationId,
                dto.IsActive
                )).ToList();

            foreach (var card in cdomain)
            {
                //var ScpIds = await context.hardware.Select(x => new { x.component_id, x.mac }).ToArrayAsync();
                // var ScpIds = await repo.GetAsync();
                // foreach (var cred in card.Credentials)
                // {
                //     foreach (var i in ScpIds)
                //     {
                //         if (!holder.AccessDatabaseCardRecord(i.DriverId, card.Flag, cred.CardNo, cred.IssueCode, cred.Pin, card.AccessLevels.Where(x => x.DeviceId == deviceId).Select(x => x.DriverId).ToList(), (int)UtilitiesHelper.DateTimeToElapeSecond(cred.ActiveDate), (int)UtilitiesHelper.DateTimeToElapeSecond(cred.DeactiveDate)))
                //         {
                //             errors.Add(MessageBuilder.Unsuccess(i.Mac, Command.CARD_RECORD));
                //         }
                //     }

                // }

                foreach (var alvl in card.AccessLevels)
                {
                    // Get unique deviceid
                    var deviceIds = alvl.Components.Select(x => x.DeviceId).Distinct().ToList();
                    foreach (var cred in card.Credentials)
                    {

                        foreach (var deviceId in deviceIds)
                        {

                            if (!holder.AccessDatabaseCardRecord((short)deviceId, card.Flag, cred.CardNo, cred.IssueCode, cred.Pin, alvl.Components.Where(x => x.DeviceId == deviceId).Select(x => x.DriverId).ToList(), (int)UtilitiesHelper.DateTimeToElapeSecond(cred.ActiveDate), (int)UtilitiesHelper.DateTimeToElapeSecond(cred.DeactiveDate)))
                            {
                                errors.Add(MessageBuilder.Unsuccess(await repo.GetMacFromComponentAsync((short)deviceId), Command.CARD_RECORD));
                            }

                        }

                    }
                }
            }





            #endregion

            #region Card Format Upload

            // Card format

            var formats = await cfmtRepo.GetByLocationIdAsync(locationId);

            foreach (var format in formats)
                {
                    if (!await cfmt.CardFormatterConfiguration((short)en.DriverId, format.DriverId, format.Fac, 0, 1, 0, format.Bits, format.PeLn, format.PeLn, format.PoLn, format.PoLoc, format.FcLn, format.FcLoc, format.ChLn, format.ChLoc, format.IcLn, format.IcLoc))
                    {
                        errors.Add(MessageBuilder.Unsuccess(await repo.GetMacFromComponentAsync((short)en.DriverId), Command.CARDFORMAT_CONFIG));
                    }

                }


            #endregion

            #region Transaction


            // Transction

            if (!scp.SetTransactionLogIndex((short)en.DriverId, true))
            {
                errors.Add(MessageBuilder.Unsuccess(await repo.GetMacFromComponentAsync((short)en.DriverId), Command.C208));
            }


            #endregion

            if (errors.Count > 0) return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.COMMAND_UNSUCCESS, errors);

            if (await repo.UpdateSyncStatusByIdAsync(en.Id) <= 0)
            {
                return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.UPLOAD_HW_CONFIG_FAIL, errors);
            }

            return ResponseHelper.SuccessBuilder(true);
        }



        public async Task<List<VerifyHardwareDeviceConfigDto>> VerifyDeviceConfigurationAsync(Device hw)
        {
            List<VerifyHardwareDeviceConfigDto> dev = new List<VerifyHardwareDeviceConfigDto>();

            if (hw is null) return dev;

            var hwSyn = hw.LastSync;


            // modules
            dev.Add(new VerifyHardwareDeviceConfigDto
            (
                 "modules",
                 await moduleRepo.CountByDeviceIdAndUpdateTimeAsync(hw.DriverId, hwSyn),
                 await moduleRepo.CountByDeviceIdAndUpdateTimeAsync(hw.DriverId, hwSyn) != 0
            ));
            

            // MP
            dev.Add(new VerifyHardwareDeviceConfigDto
            (
                 "Monitor Point",
                 await mpRepo.CountByDeviceIdAndUpdateTimeAsync(hw.DriverId, hwSyn),
                 await mpRepo.CountByDeviceIdAndUpdateTimeAsync(hw.DriverId, hwSyn) != 0
            ));

            // CP
            dev.Add(new VerifyHardwareDeviceConfigDto
            ("Control Point",
                 await cpRepo.CountByMacAndUpdateTimeAsync(hw.DriverId, hwSyn),
                 await cpRepo.CountByMacAndUpdateTimeAsync(hw.DriverId, hwSyn) != 0));

            // MPG
            dev.Add(new VerifyHardwareDeviceConfigDto(
                "Monitor Group",
                 await mpgRepo.CountByDriverIdAndUpdateTimeAsync(hw.DriverId, hwSyn),
                 await mpgRepo.CountByDriverIdAndUpdateTimeAsync(hw.DriverId, hwSyn) != 0
            ));

            // ACR
            dev.Add(new VerifyHardwareDeviceConfigDto(
                "Access Control Reader",
                 await doorRepo.CountByDeviceIdAndUpdateTimeAsync(hw.DriverId, hwSyn),
                 await doorRepo.CountByDeviceIdAndUpdateTimeAsync(hw.DriverId, hwSyn) != 0
            ));

            // Access Level
            dev.Add(new VerifyHardwareDeviceConfigDto(
                "Access Level",
                 await alvlRepo.CountByLocationIdAndUpdateTimeAsync(hw.LocationId, hwSyn),
                 await alvlRepo.CountByLocationIdAndUpdateTimeAsync(hw.LocationId, hwSyn) != 0
            ));

            // Access Area
            dev.Add(new VerifyHardwareDeviceConfigDto(
                "Access Area",
                 await areaRepo.CountByLocationIdAndUpdateTimeAsync(hw.LocationId, hwSyn),
                 await areaRepo.CountByLocationIdAndUpdateTimeAsync(hw.LocationId, hwSyn) != 0
            ));

            // time Zone
            dev.Add(new VerifyHardwareDeviceConfigDto(
                "Time Zone",
                 await tzRepo.CountByLocationIdAndUpdateTimeAsync((short)hw.LocationId, hwSyn),
                 await tzRepo.CountByLocationIdAndUpdateTimeAsync((short)hw.LocationId, hwSyn) != 0
            ));

            // Holiday
            dev.Add(new VerifyHardwareDeviceConfigDto(
                "Holiday",
                 await holRepo.CountByLocationIdAndUpdateTimeAsync(hw.LocationId, hwSyn),
                 await holRepo.CountByLocationIdAndUpdateTimeAsync(hw.LocationId, hwSyn) != 0
            ));

            // interval
            dev.Add(new VerifyHardwareDeviceConfigDto(
                "Interval",
                 await intervalRepo.CountByLocationIdAndUpdateTimeAsync((short)hw.LocationId, hwSyn),
                 await intervalRepo.CountByLocationIdAndUpdateTimeAsync((short)hw.LocationId, hwSyn) != 0
            ));

            // trigger
            dev.Add(new VerifyHardwareDeviceConfigDto(
                "Trigger",
                 await trigRepo.CountByDeviceIdAndUpdateTimeAsync(hw.DriverId, hwSyn),
                 await trigRepo.CountByDeviceIdAndUpdateTimeAsync(hw.DriverId, hwSyn) != 0
            ));

            // Prcedure
            dev.Add(new VerifyHardwareDeviceConfigDto(
                 "Procedure",
                 await procRepo.CountByDeviceIdAndUpdateTimeAsync(hw.DriverId, hwSyn),
                 await procRepo.CountByDeviceIdAndUpdateTimeAsync(hw.DriverId, hwSyn) != 0
            ));

            // action
            dev.Add(new VerifyHardwareDeviceConfigDto(
                 "Action",
                 await actionRepo.CountByMacAndUpdateTimeAsync(hw.Mac, hwSyn),
                 await actionRepo.CountByMacAndUpdateTimeAsync(hw.Mac, hwSyn) != 0
            ));

            // card_format
            dev.Add(new VerifyHardwareDeviceConfigDto(
                 "Card Format",
                 await cfmtRepo.CountByUpdateTimeAsync(hwSyn),
                 await cfmtRepo.CountByUpdateTimeAsync(hwSyn) != 0
            )
            );
            return dev;

        }

        public async Task<ResponseDto<IEnumerable<VerifyHardwareDeviceConfigDto>>> VerifyComponentConfigurationAsync(string mac)
        {
            var hardware = await repo.GetDomainByMacAsync(mac);

            if (hardware is null) return ResponseHelper.NotFoundBuilder<IEnumerable<VerifyHardwareDeviceConfigDto>>();

            var dev = await VerifyDeviceConfigurationAsync(hardware);


            var status = dev.Any(s => s.IsUpload == true);

            await repo.UpdateVerifyHardwareCofigurationMyMacAsync(mac, status);

            return ResponseHelper.SuccessBuilder<IEnumerable<VerifyHardwareDeviceConfigDto>>(dev);
        }


        public async Task<ResponseDto<DeviceDto>> CreateAsync(CreateDeviceDto dto)
        {
             // Check value in license here 
            // ....to be implement

            if (await repo.IsAnyByNameAsync(dto.Name.Trim())) return ResponseHelper.BadRequestName<DeviceDto>();

            var ScpSetting = await setting.GetScpSettingAsync();

            var DriverId = await repo.GetLowestUnassignedNumberAsync(10);
            if (DriverId == -1) return ResponseHelper.ExceedLimit<DeviceDto>();

            var dev = await repo.GetDeviceComponentByModelAsync(196); // Aero

            var hardware = new Aero.Domain.Entities.Device(
                0,
                dto.DriverId,
                dto.Name,
                dto.HardwareType,
                dto.HardwareTypeDetail,
                dto.Mac,
                dto.Ip,
                dto.Firmware,
                dto.Port,
                new List<Module>()
                {
                    new Aero.Domain.Entities.Module(
                        dto.DriverId,
                        1,
                        196,
                        "Internal",
                        dto.Firmware,
                        dto.SerialNumber,
                        1,
                        "X1100",
                        1,
                        1,
                        1,
                        1,
                        "",
                        1,
                        "",
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        0,
                        "0",
                        3,
                        (short)dev.nInput,
                        (short)dev.nOutput,
                         (short)dev.nReader,
                        3,
                        0,
                        0,
                        0,
                        dto.LocationId,
                        dto.IsActive
        )
                },
                dto.SerialNumber,
                false,
                false,
                dto.PortOne,
                dto.ProtocolOne,
                dto.ProtocolOneDetail,
                dto.BaudRateOne,
                dto.PortTwo,
                dto.ProtocolTwo,
                dto.ProtocolTwoDetail,
                dto.BaudRateTwo,
                DateTime.UtcNow

            );

            if (!await VerifyMemoryAllocateAsync(hardware.Mac))
            {
                hardware.SetReset(true);
            }


            var component = await VerifyDeviceConfigurationAsync(hardware);

            hardware.SetIsUpload(component.Any(s => s.IsUpload == true));

            //// Internal modules Config
            //if (!command.SioDriverConfiguration(dto.component_id,0, 0, -1, 0))
            //{
            //    return ResponseHelper.UnsuccessBuilder<bool>(MessageBuilder.Unsuccess(dto.mac, command.C108), []);
            //};

            //if (!command.SioPanelConfiguration(dto.component_id, 0, 196, 7, 4, 4, 0, 0, true))
            //{
            //    return ResponseHelper.UnsuccessBuilder<bool>(MessageBuilder.Unsuccess(dto.mac, command.C109), []);
            //}

            if (dto.PortOne)
            {
                if (!sio.SioDriverConfiguration(dto.DriverId, 1, 1, dto.BaudRateOne, dto.ProtocolOne))
                {
                    return ResponseHelper.UnsuccessBuilder<DeviceDto>(MessageBuilder.Unsuccess(dto.Mac, Command.SIO_DRIVER), []);
                }
                ;

                // Send command for address 0-15 on port 1 if allow
                for (int i = 0; i < 16; i++)
                {
                    // model = -1 for allow every model
                    // n_input = 19 Maximum
                    // n_output = 12 Maximum
                    // n_reader = 4 Maximum
                    if (!sio.SioPanelConfiguration(dto.DriverId, (short)i, -1, 19, 12, 4, (short)i, 1, true))
                    {
                        return ResponseHelper.UnsuccessBuilder<DeviceDto>(MessageBuilder.Unsuccess(dto.Mac, Command.SIO_PANEL_CONFIG), []);
                    }
                }
            }

            if (dto.PortTwo)
            {
                if (!sio.SioDriverConfiguration(dto.DriverId, 2, 2, dto.BaudRateTwo, dto.ProtocolTwo))
                {
                    return ResponseHelper.UnsuccessBuilder<DeviceDto>(MessageBuilder.Unsuccess(dto.Mac, Command.SIO_DRIVER), []);
                }
                ;

                // Send command for address 16-31 on port 2 if allow
                for (int i = 15; i < 31; i++)
                {
                    // model = -1 for allow every model
                    if (!sio.SioPanelConfiguration(dto.DriverId, (short)i, -1, 19, 12, 4, (short)i, 1, true))
                    {
                        return ResponseHelper.UnsuccessBuilder<DeviceDto>(MessageBuilder.Unsuccess(dto.Mac, Command.SIO_PANEL_CONFIG), []);
                    }
                }
            }

            if (!scp.SetTransactionLogIndex(dto.DriverId, true))
            {
                return ResponseHelper.UnsuccessBuilder<DeviceDto>(ResponseMessage.TRANSACTION_ENABLE_FAIL, []);
            }

            var report = await idRepo.GetByMacAndScpIdAsync(dto.Mac, dto.DriverId);

            if (report is null) return ResponseHelper.NotFoundBuilder<DeviceDto>();

            var status = await repo.AddAsync(hardware);

            if (status <= 0) return ResponseHelper.UnsuccessBuilder<DeviceDto>(ResponseMessage.SAVE_DATABASE_UNSUCCESS, []);

            status = await idRepo.DeleteByMacAndScpIdAsync(report.MacAddress, report.ScpId);

            if (status <= 0) return ResponseHelper.UnsuccessBuilder<DeviceDto>(ResponseMessage.DELETE_DATABASE_UNSUCCESS, []);


            return ResponseHelper.SuccessBuilder(await repo.GetByIdAsync(status));
        }


        public async Task<ResponseDto<DeviceDto>> UpdateAsync(DeviceDto dto)
        {

            var en = await repo.GetByMacAsync(dto.Mac);

            if (en is null) return ResponseHelper.NotFoundBuilder<DeviceDto>();

            var ens =  new Aero.Domain.Entities.Device(
                dto.Id,
                (short)dto.DriverId,
                dto.Name,
                dto.HardwareType,
                dto.HardwareTypeDetail,
                dto.Mac,
                dto.Ip,
                dto.Firmware,
                dto.Port,
                null,
                dto.SerialNumber,
                false,
                false,
                dto.PortOne,
                dto.ProtocolOne,
                dto.ProtocolOneDetail,
                dto.BaudRateOne,
                dto.PortTwo,
                dto.ProtocolTwo,
                dto.ProtocolTwoDetail,
                dto.BaudRateTwo,
                DateTime.UtcNow

            );

            var status = await repo.UpdateAsync(ens);

            if (status <= 0) return ResponseHelper.UnsuccessBuilder<DeviceDto>(ResponseMessage.UPDATE_RECORD_UNSUCCESS, []);

            return ResponseHelper.SuccessBuilder<DeviceDto>(dto);
        }


        public async Task<ResponseDto<DeviceStatusDto>> GetStatusAsync(int Id)
        {
            var en = await repo.GetByIdAsync(Id);
            if (en is null) return ResponseHelper.NotFoundBuilder<DeviceStatusDto>();
            short status = scp.CheckSCPStatus((short)en.DriverId);
            return ResponseHelper.SuccessBuilder(new DeviceStatusDto(en.DriverId,status));
        }




        public async Task<ResponseDto<bool>> SetTransactionAsync(string mac, short IsOn)
        {
            var ScpId = await repo.GetComponentIdFromMacAsync(mac);
            if (ScpId == 0) return ResponseHelper.NotFoundBuilder<bool>();
            if (!scp.SetTransactionLogIndex(ScpId, IsOn == 1 ? true : false))
            {
                return ResponseHelper.UnsuccessBuilder<bool>(MessageBuilder.Unsuccess(mac, Command.TRAN_INDEX), []);
            }
            return ResponseHelper.SuccessBuilder(true);
        }

        public async Task<ResponseDto<bool>> GetTransactionLogStatusAsync(string mac)
        {
            var id = await repo.GetComponentIdFromMacAsync(mac);
            if (id == 0) return ResponseHelper.NotFoundBuilder<bool>();

            if (!scp.GetTransactionLogStatus(id))
            {
                return ResponseHelper.UnsuccessBuilder<bool>(MessageBuilder.Unsuccess(mac, Command.C402), []);
            }

            return ResponseHelper.SuccessBuilder(true);
        }


        public async Task<ResponseDto<IEnumerable<Mode>>> GetHardwareTypeAsync()
        {
            throw new NotImplementedException();
            // return ResponseHelper.SuccessBuilder<IEnumerable<Mode>>(dtos);
        }



        public async Task<ResponseDto<IEnumerable<ResponseDto<bool>>>> SetRangeTransactionAsync(List<SetTranDto> dtos)
        {
            bool flag = true;
            List<ResponseDto<bool>> data = new List<ResponseDto<bool>>();
            foreach (var dto in dtos)
            {
                var re = await SetTransactionAsync(dto.MacAddress, dto.Param);
                if (re.code != HttpStatusCode.OK) flag = false;
                data.Add(re);
            }

            if (!flag) return ResponseHelper.UnsuccessBuilder<IEnumerable<ResponseDto<bool>>>(data);

            var res = ResponseHelper.SuccessBuilder<IEnumerable<ResponseDto<bool>>>(data);

            return res;
        }

        public async Task<ResponseDto<DeviceDto>> GetByMacAsync(string mac)
        {
            var res = await repo.GetByMacAsync(mac);
            return ResponseHelper.SuccessBuilder<DeviceDto>(res);
        }

        public async Task HandleFoundHardware(IScpReply message)
        {
            if (await repo.IsAnyByMac(UtilitiesHelper.ByteToHexStr(message.id.mac_addr)))
            {
                var hardware = await repo.GetDomainByMacAsync(UtilitiesHelper.ByteToHexStr(message.id.mac_addr));

                if (hardware is null) return;

                if (!await MappingHardwareAndAllocateMemory(message.id.scp_id))
                {
                    hardware.SetIsReset(true);
                }
                else
                {
                    hardware.SetIsReset(false);
                }

                if (!await VerifyMemoryAllocateAsync(hardware.Mac))
                {
                    hardware.SetIsReset(true);
                }
                else
                {
                    hardware.SetIsReset(false);
                }

                hardware.SetFirmware(UtilitiesHelper.ParseFirmware(message.id.sft_rev_major, message.id.sft_rev_minor));

                var component = await VerifyDeviceConfigurationAsync(hardware);

                hardware.SetIsUpload(component.Any(s => s.IsUpload == true));

                var status = await repo.UpdateAsync(hardware);

                if (status <= 0) return;

                // Call Get ip
                scp.GetWebConfigRead(message.id.scp_id, 2);


            }
            else
            {
                if (!await VerifyHardwareConnection(message.id.scp_id)) return;


                if (await repo.IsAnyByMacAndDriver(UtilitiesHelper.ByteToHexStr(message.id.mac_addr), message.id.scp_id))
                {

                    if (await idRepo.IsAnyByMacAndScpIdAsync(UtilitiesHelper.ByteToHexStr(message.id.mac_addr), message.id.scp_id))
                    {
                        // Delete id report
                        var status = await idRepo.DeleteByMacAndScpIdAsync(UtilitiesHelper.ByteToHexStr(message.id.mac_addr), message.id.scp_id);
                        if (status <= 0) throw new Exception("Delete Id report from database unsuccess.");
                    }
                    return;
                }
                else
                {
                    if (await idRepo.IsAnyByMacAndScpIdAsync(UtilitiesHelper.ByteToHexStr(message.id.mac_addr), message.id.scp_id))
                    {
                        // Update
                        var status = await idRepo.UpdateAsync(message);
                    }
                    else
                    {
                        // Create 
                        var status = await idRepo.AddAsync(message);
                    }
                }

                scp.GetWebConfigRead(message.id.scp_id, 2);


            }

        }

        public async Task VerifyAllocateHardwareMemoryAsync(IScpReply message)
        {

            var scp = await repo.GetByIdAsync(message.ScpId);

            if (scp is null) return;


            var mems = await repo.CheckAllocateMemoryAsync(message);

            var res = await repo.UpdateVerifyMemoryAllocateByIdAsync((short)message.ScpId, mems.Any(x => x.IsSync == false));
            if (res <= 0) return;


            // Check mismatch device configuration
            //await VerifyDeviceConfigurationAsync(hw.mac,hw.location_id);
            var data = new MemoryAllocateDto(await repo.GetMacFromComponentAsync((short)message.ScpId), mems);
            await publisher.ScpNotifyMemoryAllocate(data);
        }

        public async Task AssignIpAddressAsync(IScpReply message)
        {
            if (await repo.IsAnyByIdAsync((short)message.ScpId))
            {
                if (message.web_network is not null) await repo.UpdateIpAddressAsync(message.ScpId, UtilitiesHelper.IntegerToIp(message.web_network.cIpAddr));

                scp.GetWebConfigRead((short)message.ScpId, 3);

            }
            else
            {

                if (message.web_network is not null) await idRepo.UpdateIpAddressAsync(message.ScpId, UtilitiesHelper.IntegerToIp(message.web_network.cIpAddr));

                scp.GetWebConfigRead((short)message.ScpId, 3);
            }


        }

        public async Task AssignPortAsync(IScpReply message)
        {
            if (await repo.IsAnyByIdAsync((short)message.ScpId))
            {
                string port = "";

                if (message.web_host_comm_prim is not null)
                {
                    if (message.web_host_comm_prim.ipclient is not null)
                    {
                        port = message.web_host_comm_prim.ipclient.nPort.ToString();
                    }
                    else if (message.web_host_comm_prim.ipserver is not null)
                    {
                        port = message.web_host_comm_prim.ipserver.nPort.ToString();
                    }
                }
                ;


                await repo.UpdatePortAddressAsync((short)message.ScpId, port);

                var dto = await idRepo.GetAsync();

                await publisher.IdReportNotifyAsync(dto.ToList());


            }
            else
            {
                string port = "";

                if (message.web_host_comm_prim is not null)
                {
                    if (message.web_host_comm_prim.ipclient is not null)
                    {
                        port = message.web_host_comm_prim.ipclient.nPort.ToString();
                    }
                    else if (message.web_host_comm_prim.ipserver is not null)
                    {
                        port = message.web_host_comm_prim.ipserver.nPort.ToString();
                    }
                }
                ;

                await idRepo.UpdatePortAddressAsync((short)message.ScpId, port);

                var dto = await idRepo.GetAsync();

                await publisher.IdReportNotifyAsync(dto.ToList());
            }


        }

        public async Task<ResponseDto<Pagination<DeviceDto>>> GetPaginationAsync(PaginationParamsWithFilter param, short location)
        {
            var res = await repo.GetPaginationAsync(param, location);
            return ResponseHelper.SuccessBuilder(res);
        }
    }
}
