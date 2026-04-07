using Aero.Api.Constants;
using Aero.Application.Commands.Interfaces;
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
        IAeroAdapter aero,
        INotificationPublisher publisher, ISettingRepository setting
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
            if (await repo.IsAnyModuleReferenceByDriverIdAsync(en.ScpId)) return ResponseHelper.FoundReferenceBuilder<DeviceDto>();


            if (!aero.DetachScp((short)en.ScpId))
            {
                return ResponseHelper.UnsuccessBuilderWithString<DeviceDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(await repo.GetMacFromComponentAsync((short)en.ScpId), Command.DELETE_SCP));
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
            if (!aero.ResetScp(id))
            {
                return ResponseHelper.UnsuccessBuilderWithString<bool>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(mac, Command.RESET_SCP));
            }
            return ResponseHelper.SuccessBuilder(true);
        }

        public async Task<ResponseDto<bool>> ResetByComponentAsync(short id)
        {
            string mac = await repo.GetMacFromComponentAsync(id);
            if (string.IsNullOrEmpty(mac)) return ResponseHelper.NotFoundBuilder<bool>();
            if (!aero.ResetScp(id))
            {
                return ResponseHelper.UnsuccessBuilderWithString<bool>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(mac, Command.RESET_SCP));
            }
            return ResponseHelper.SuccessBuilder(true);
        }

        public async Task<bool> VerifyHardwareConnection(short ScpId)
        {
            var setting = await repo.GetScpSettingAsync();

            if (setting is null) return false;

            if (!aero.ScpDeviceSpecification(ScpId, setting)) return false;

            return true;
        }

        public async Task<bool> MappingHardwareAndAllocateMemory(short ScpId)
        {
            var setting = await repo.GetScpSettingAsync();
            if (setting is null) return false;

            if (!aero.ScpDeviceSpecification(ScpId, setting)) return false;

            if (!aero.AccessDatabaseSpecification(ScpId, setting)) return false;

            if (!aero.TimeSet(ScpId)) return false;

            return true;
        }

        public async Task<ResponseDto<bool>> VerifyMemoryAllocateAsyncWithResponse(string Mac)
        {
            var ScpId = await repo.GetComponentIdFromMacAsync(Mac);
            if (ScpId == 0) return ResponseHelper.NotFoundBuilder<bool>();
            if (!aero.ReadStructureStatus(ScpId))
            {
                return ResponseHelper.UnsuccessBuilder<bool>(MessageBuilder.Unsuccess(Mac, Command.SCP_STRUCTURE_STATUS), []);
            }
            return ResponseHelper.SuccessBuilder<bool>(true);
        }

        public async Task<bool> VerifyMemoryAllocateAsync(string Mac)
        {
            var ScpId = await repo.GetComponentIdFromMacAsync(Mac);
            if (ScpId == 0) return false;
            if (!aero.ReadStructureStatus(ScpId))
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
            var modules = await moduleRepo.GetByDeviceIdAsync(en.ScpId);


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
                if (!aero.SioDriverConfiguration((short)module.DeviceId, module.Msp1No, module.Port, module.BaudRate, module.nProtocol))
                {
                    errors.Add(MessageBuilder.Unsuccess(await repo.GetMacFromComponentAsync((short)module.DeviceId), Command.SIO_DRIVER));
                }
                ;

                // Enums.model.HIDAeroX1100
                if (!aero.SioPanelConfiguration((short)module.DeviceId, module.DriverId, module.Model, module.nInput, module.nOutput, module.nReader, module.Address, module.Msp1No, true))
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
                        if (!aero.InputPointSpecification((short)module.DeviceId, module.DriverId, i, 0, 2, 5))
                        {
                            errors.Add(MessageBuilder.Unsuccess(await repo.GetMacFromComponentAsync((short)module.DeviceId), Command.INPUT_SPEC));
                        }
                    }

                }
            }



            #endregion

            #region Time Zone Upload

            // Timezone

            var locationId = await repo.GetLocationIdFromDriverIdAsync(en.ScpId);

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
                if (!aero.ExtendedTimeZoneActSpecification((short)en.ScpId, t))
                {
                    errors.Add(MessageBuilder.Unsuccess(await repo.GetMacFromComponentAsync((short)en.ScpId), Command.TIMEZONE_SPEC));
                }
            }


            #endregion

            #region Access Level Upload

            // Access Level

            var accessLevels = await alvlRepo.GetByLocationIdAsync(locationId);
            var acl = accessLevels.Select(dto => new AccessLevel(
                0,
                dto.Name,
                dto.Components.Select(x => new AccessLevelComponent(x.AlvlId, x.DeviceId, x.DoorId, x.AcrId, x.TimeZoneId)).ToList(),
                dto.LocationId,
                dto.IsActive
                )).ToList();

            foreach (var domain in acl)
            {

                if (domain.Id == 1 || domain.Id == 2)
                {
                    if (!await aero.AccessLevelConfigurationExtended((short)en.ScpId, (short)domain.Id, domain.Id == 1 ? (short)0 : (short)1))
                    {
                        errors.Add(MessageBuilder.Unsuccess(await repo.GetMacFromComponentAsync((short)en.ScpId), Command.ALVL_CONFIG));
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
                        if (!await aero.AccessLevelConfigurationExtended((short)device.ElementAt(i), domain.Components.Where(x => x.DeviceId == device.ElementAt(i)).Select(x => x.AlvlId).FirstOrDefault(), domain))
                        {
                            errors.Add(MessageBuilder.Unsuccess(await repo.GetMacFromComponentAsync((short)device.ElementAt(i)), Command.ALVL_CONFIG));

                        }
                    }
                }

            }


            #endregion

            #region Control Point

            // Control Point
            var controls = await cpRepo.GetByDeviceId(en.ScpId);

            foreach (var control in controls)
            {
                // command place here
                short modeNo = await cpRepo.GetModeNoByOfflineAndRelayModeAsync(control.OfflineMode, control.RelayMode);

                if (!aero.OutputPointSpecification((short)control.ScpId, control.ModuleDriverId, control.OutputNo, modeNo))
                {
                    errors.Add(MessageBuilder.Unsuccess(await repo.GetMacFromComponentAsync((short)control.ScpId), Command.OUTPUT_SPEC));
                }


                if (!aero.ControlPointConfiguration((short)control.ScpId, control.ModuleDriverId, control.CpId, control.OutputNo, control.DefaultPulse))
                {
                    errors.Add(MessageBuilder.Unsuccess(await repo.GetMacFromComponentAsync((short)control.ScpId), Command.CONTROL_CONFIG));

                }

            }


            #endregion

            #region Monitor Point

            // Monitor Points
            var mps = await mpRepo.GetByDeviceId(en.ScpId);

            foreach (var monitors in mps)
            {
                // command place here
                if (!aero.InputPointSpecification((short)monitors.ScpId, monitors.ModuleDriverId, monitors.InputNo, monitors.InputMode, monitors.Debounce, monitors.HoldTime))
                {
                    errors.Add(MessageBuilder.Unsuccess(await repo.GetMacFromComponentAsync((short)monitors.ScpId), Command.INPUT_SPEC));
                }


                if (!aero.MonitorPointConfiguration((short)monitors.ScpId, monitors.ModuleDriverId, monitors.InputNo, monitors.LogFunction, monitors.MonitorPointMode, monitors.DelayEntry, monitors.DelayExit, monitors.MpId))
                {
                    errors.Add(MessageBuilder.Unsuccess(await repo.GetMacFromComponentAsync((short)monitors.ScpId), Command.MONITOR_CONFIG));
                }

            }


            #endregion

            #region Monitor Group

            // Monitor Group
            var mpgs = await mpgRepo.GetByDeviceIdAsync(en.ScpId);

            var mpgsdomain = mpgs.Where(x => x.ScpId == en.ScpId).Select(dto => new MonitorGroup(
                dto.ScpId,
                dto.MpgId,
                dto.Name,
                dto.nMpCount,
                dto.nMpList.Select(x => new MonitorGroupList(dto.MpgId, x.PointType, x.PointTypeDesc, x.PointNumber)).ToList(),
                dto.LocationId,
                dto.IsActive
                )).ToList();

            foreach (var mpGroup in mpgsdomain)
            {
                if (!aero.ConfigureMonitorPointGroup((short)mpGroup.ScpId, mpGroup.MpgId, mpGroup.nMpCount, mpGroup.nMpList.ToList()))
                {
                    errors.Add(MessageBuilder.Unsuccess(await repo.GetMacFromComponentAsync((short)mpGroup.ScpId), Command.CONFIG_MPG));
                }
            }


            #endregion

            #region Doors

            // door
            var doors = await doorRepo.GetByDeviceIdAsync((short)en.ScpId);
            // var doors = doorss.Select(x => DoorMapper.ToDomain(x)).ToList();



            foreach (var dto in doors)
            {
                // var door = new Door(dto.Id, dto.AcrId, dto.Name, dto.DoorType, dto.PairDoorNo,
                // (DoorDirection)dto.Direction,
                // dto.ScpId,
                // dto.Readers.Select(x => new Reader(x.ModuleId,x.ModuleDriverId, x.DoorId, x.ReaderNo, x.DataFormat, x.KeypadMode, x.LedDriveMode, x.OsdpFlag, x.OsdpBaudrate, x.OsdpDiscover, x.OsdpTracing, x.OsdpAddress, x.OsdpSecureChannel, x.ScpId, x.LocationId, x.IsActive)).ToList(),
                // dto.ReaderOutConfiguration,
                // dto.Strk is null ? null : new Strike(dto.Strk.ScpId, dto.Strk.ModuleId,dto.Strk.ModuleDriverId, dto.Strk.DoorId, dto.Strk.OutputNo, dto.Strk.RelayMode, dto.Strk.OfflineMode, dto.Strk.StrkMax, dto.Strk.StrkMin, dto.Strk.StrkMode, dto.Strk.LocationId, dto.Strk.IsActive),
                // dto.Sensor is null ? null : new Sensor(dto.Sensor.ScpId, dto.Sensor.ModuleId,dto.Sensor.ModuleDriverId,dto.Sensor.DoorId, dto.Sensor.InputNo, dto.Sensor.InputMode, dto.Sensor.Debounce, dto.Sensor.HoldTime, dto.Sensor.DcHeld, dto.Sensor.LocationId, dto.Sensor.IsActive),
                // dto.RequestExits is null ? new List<RequestExit>() : dto.RequestExits.Select(x => new RequestExit(x.ScpId, x.ModuleId, x.ModuleDriverId,x.DoorId, x.InputNo, x.InputMode, x.Debounce, x.HoldTime, x.MaskTimeZone, x.LocationId, x.IsActive)).ToList()
                // , dto.CardFormat,  dto.AreaInId,
                // dto.AreaInId, dto.AreaOutId, dto.SpareTags, dto.AccessControlFlags, dto.Mode, dto.ModeDesc,
                // dto.OfflineMode, dto.OfflineModeDesc, dto.DefaultMode, dto.DefaultModeDesc, dto.DefaultLEDMode, dto.PreAlarm, dto.AntiPassbackDelay,
                // dto.StrkT2, dto.DcHeld2, dto.StrkFollowPulse, dto.StrkFollowDelay, dto.nExtFeatureType, dto.IlPBSio, dto.IlPBNumber, dto.IlPBLongPress, dto.IlPBOutSio,
                // dto.IlPBOutNum, dto.DfOfFilterTime, dto.MaskHeldOpen, dto.MaskForceOpen
                // );

                // // command place here


                // foreach (var reader in door.Readers)
                // {
                //     if (reader.ScpId == 0) continue;
                //     short readerInOsdpFlag = 0x00;
                //     short readerLedDriveMode = 0;
                //     if (reader.OsdpFlag)
                //     {
                //         readerInOsdpFlag |= reader.OsdpBaudrate;
                //         readerInOsdpFlag |= reader.OsdpDiscover;
                //         readerInOsdpFlag |= reader.OsdpTracing;
                //         readerInOsdpFlag |= reader.OsdpAddress;
                //         readerInOsdpFlag |= reader.OsdpSecureChannel;
                //         readerLedDriveMode = 7;
                //     }
                //     else
                //     {
                //         readerLedDriveMode = 1;
                //     }


                //     // Reader In Config


                //     if (!aero.ReaderSpecification((short)reader.ScpId,reader.ModuleDriverId, reader.ReaderNo, reader.DataFormat, reader.KeypadMode, readerLedDriveMode, readerInOsdpFlag))
                //     {
                //         errors.Add(MessageBuilder.Unsuccess(await repo.GetMacFromComponentAsync((short)en.ScpId), Command.READER_SPEC));
                //     }
                // }



                // // Strike Strike Config
                // if (door.Strk != null)
                // {
                //     if (!aero.OutputPointSpecification((short)door.Strk.ScpId, (short)door.Strk.ModuleDriverId, door.Strk.OutputNo, door.Strk.RelayMode))
                //     {
                //         errors.Add(MessageBuilder.Unsuccess(await repo.GetMacFromComponentAsync((short)en.ScpId), Command.OUTPUT_SPEC));
                //     }
                // }


                // if (door.Sensor != null)
                // {
                //     if (!aero.InputPointSpecification((short)door.Sensor.ScpId, door.Sensor.ModuleDriverId,door.Sensor.InputNo, door.Sensor.InputMode, door.Sensor.Debounce, door.Sensor.HoldTime))
                //     {
                //         errors.Add(MessageBuilder.Unsuccess(await repo.GetMacFromComponentAsync((short)en.ScpId), Command.INPUT_SPEC));
                //     }
                // }


                // foreach (var rex in door.RequestExits)
                // {
                //     if (rex.ScpId == 0) continue;
                //     if (!aero.InputPointSpecification((short)rex.ScpId, (short)rex.ModuleDriverId, rex.InputNo, rex.InputMode, rex.Debounce, rex.HoldTime))
                //     {
                //         errors.Add(MessageBuilder.Unsuccess(await repo.GetMacFromComponentAsync((short)en.ScpId), Command.INPUT_SPEC));
                //     }
                // }

                // if (!aero.AccessControlReaderConfiguration((short)door.ScpId, door.AcrId, door))
                // {
                //     errors.Add(MessageBuilder.Unsuccess(await repo.GetMacFromComponentAsync((short)en.ScpId), Command.ACR_CONFIG));
                // }

                // switch (dto.DoorType)
                // {
                //     // Single In REX Out
                //     case 1:


                //         var domain = new Door(0, dto.AcrId, dto.Name, dto.DoorType, dto.PairDoorNo,
                //         dto.ScpId,
                //         dto.Readers is null || dto.Readers.Count == 0 ? new List<Reader>() :
                //         new List<Reader>
                //         {
                //         new Reader(
                //             dto.Readers.ElementAt(0).ModuleId,
                //             dto.Readers.ElementAt(0).ModuleDriverId,
                //             dto.Readers.ElementAt(0).DoorId,
                //             DoorDirection.IN,
                //             dto.Readers.ElementAt(0).ReaderNo,
                //             dto.Readers.ElementAt(0).DataFormat,
                //             dto.Readers.ElementAt(0).KeypadMode,
                //             dto.Readers.ElementAt(0).LedDriveMode,
                //             dto.Readers.ElementAt(0).OsdpFlag,
                //             dto.Readers.ElementAt(0).OsdpBaudrate,
                //             dto.Readers.ElementAt(0).OsdpDiscover,
                //             dto.Readers.ElementAt(0).OsdpTracing,
                //             dto.Readers.ElementAt(0).OsdpAddress,
                //             dto.Readers.ElementAt(0).OsdpSecureChannel,
                //             dto.Readers.ElementAt(0).ScpId,
                //             dto.Readers.ElementAt(0).LocationId,
                //             dto.Readers.ElementAt(0).IsActive
                //         )
                //         },
                //         dto.ReaderOutConfiguration,
                //         dto.Strk is null ? null : new Strike(dto.Strk.ScpId, dto.Strk.ModuleId, dto.Strk.ModuleDriverId, dto.Strk.DoorId, dto.Strk.OutputNo, dto.Strk.RelayMode, dto.Strk.OfflineMode, dto.Strk.StrkMax, dto.Strk.StrkMin, dto.Strk.StrkMode, dto.Strk.LocationId, dto.Strk.IsActive),
                //         dto.Sensor is null ? null : new Sensor(dto.Sensor.ScpId, dto.Sensor.ModuleId, dto.Sensor.ModuleDriverId, dto.Sensor.DoorId, dto.Sensor.InputNo, dto.Sensor.InputMode, dto.Sensor.Debounce, dto.Sensor.HoldTime, dto.Sensor.DcHeld, dto.Sensor.LocationId, dto.Sensor.IsActive),
                //         dto.RequestExits is null ? new List<RequestExit>() : dto.RequestExits.Select(x => new RequestExit(x.ScpId, x.ModuleId, x.ModuleDriverId, x.DoorId, x.InputNo, x.InputMode, x.Debounce, x.HoldTime, x.MaskTimeZone, x.LocationId, x.IsActive)).ToList()
                //         , dto.CardFormat, dto.AntiPassbackMode, dto.AreaInId, dto.AreaOutId, dto.SpareTags, dto.AccessControlFlags, dto.Mode, dto.ModeDesc,
                //         dto.OfflineMode, dto.OfflineModeDesc, dto.DefaultMode, dto.DefaultModeDesc, dto.DefaultLEDMode, dto.PreAlarm, dto.AntiPassbackDelay,
                //         dto.StrkT2, dto.DcHeld2, dto.StrkFollowPulse, dto.StrkFollowDelay, dto.nExtFeatureType, dto.IlPBSio, dto.IlPBNumber, dto.IlPBLongPress, dto.IlPBOutSio,
                //         dto.IlPBOutNum, dto.DfOfFilterTime, dto.MaskHeldOpen, dto.MaskForceOpen
                //         );

                //         foreach (var reader in domain.Readers)
                //         {
                //             if (reader.ScpId == 0) continue;
                //             short readerInOsdpFlag = 0x00;
                //             short readerLedDriveMode = 0;
                //             if (reader.OsdpFlag)
                //             {
                //                 readerInOsdpFlag |= reader.OsdpBaudrate;
                //                 readerInOsdpFlag |= reader.OsdpDiscover;
                //                 readerInOsdpFlag |= reader.OsdpTracing;
                //                 readerInOsdpFlag |= reader.OsdpAddress;
                //                 readerInOsdpFlag |= reader.OsdpSecureChannel;
                //                 readerLedDriveMode = 7;
                //             }
                //             else
                //             {
                //                 readerLedDriveMode = 1;
                //             }


                //             // Reader In Config
                //             if (!aero.ReaderSpecification((short)reader.ScpId, reader.ModuleDriverId, reader.ReaderNo, reader.DataFormat, reader.KeypadMode, readerLedDriveMode, readerInOsdpFlag))
                //             {
                //                 errors.Add(MessageBuilder.Unsuccess(await repo.GetMacFromComponentAsync((short)domain.ScpId), Command.READER_SPEC));
                //             }
                //         }



                //         // Strike Strike Config
                //         if (dto.Strk != null)
                //         {
                //             if (!aero.OutputPointSpecification((short)dto.Strk.ScpId, (short)domain.Strk.ModuleDriverId, domain.Strk.OutputNo, domain.Strk.RelayMode))
                //             {
                //                 errors.Add(MessageBuilder.Unsuccess(await repo.GetMacFromComponentAsync((short)domain.ScpId), Command.OUTPUT_SPEC));
                //             }

                //         }

                //         // door sensor Config
                //         if (dto.Sensor != null)
                //         {
                //             if (!aero.InputPointSpecification((short)domain.Sensor.ScpId, (short)domain.Sensor.ModuleDriverId, domain.Sensor.InputNo, domain.Sensor.InputMode, domain.Sensor.Debounce, domain.Sensor.HoldTime))
                //             {
                //                 errors.Add(MessageBuilder.Unsuccess(await repo.GetMacFromComponentAsync((short)domain.ScpId), Command.INPUT_SPEC));
                //             }
                //         }


                //         foreach (var rex in domain.RequestExits)
                //         {
                //             if (rex.ScpId == 0) continue;
                //             if (!aero.InputPointSpecification((short)rex.ScpId, (short)rex.ModuleDriverId, rex.InputNo, rex.InputMode, rex.Debounce, rex.HoldTime))
                //             {
                //                 errors.Add(MessageBuilder.Unsuccess(await repo.GetMacFromComponentAsync((short)domain.ScpId), Command.INPUT_SPEC));
                //             }
                //         }

                //         if (!aero.AccessControlReaderConfiguration((short)domain.ScpId, domain.AcrId, domain, 0))
                //         {
                //             errors.Add(MessageBuilder.Unsuccess(await repo.GetMacFromComponentAsync((short)domain.ScpId), Command.ACR_CONFIG));
                //         }

                //     // Reader In-Out (Wiegand)
                //     case 2:

                //         var domainIn = new Door(0, dto.AcrId, dto.Name, dto.DoorType, dto.PairDoorNo,
                //         dto.ScpId,
                //         dto.Readers is null || dto.Readers.Count == 0 ? new List<Reader>() :
                //         new List<Reader>
                //         {
                //         new Reader(
                //             dto.Readers.ElementAt(0).ModuleId,
                //             dto.Readers.ElementAt(0).ModuleDriverId,
                //             dto.Readers.ElementAt(0).DoorId,
                //             DoorDirection.IN,
                //             dto.Readers.ElementAt(0).ReaderNo,
                //             dto.Readers.ElementAt(0).DataFormat,
                //             dto.Readers.ElementAt(0).KeypadMode,
                //             dto.Readers.ElementAt(0).LedDriveMode,
                //             dto.Readers.ElementAt(0).OsdpFlag,
                //             dto.Readers.ElementAt(0).OsdpBaudrate,
                //             dto.Readers.ElementAt(0).OsdpDiscover,
                //             dto.Readers.ElementAt(0).OsdpTracing,
                //             dto.Readers.ElementAt(0).OsdpAddress,
                //             dto.Readers.ElementAt(0).OsdpSecureChannel,
                //             dto.Readers.ElementAt(0).ScpId,
                //             dto.Readers.ElementAt(0).LocationId,
                //             dto.Readers.ElementAt(0).IsActive
                //         )
                //         },
                //         dto.ReaderOutConfiguration,
                //         dto.Strk is null ? null : new Strike(dto.Strk.ScpId, dto.Strk.ModuleId, dto.Strk.ModuleDriverId, dto.Strk.DoorId, dto.Strk.OutputNo, dto.Strk.RelayMode, dto.Strk.OfflineMode, dto.Strk.StrkMax, dto.Strk.StrkMin, dto.Strk.StrkMode, dto.Strk.LocationId, dto.Strk.IsActive),
                //         dto.Sensor is null ? null : new Sensor(dto.Sensor.ScpId, dto.Sensor.ModuleId, dto.Sensor.ModuleDriverId, dto.Sensor.DoorId, dto.Sensor.InputNo, dto.Sensor.InputMode, dto.Sensor.Debounce, dto.Sensor.HoldTime, dto.Sensor.DcHeld, dto.Sensor.LocationId, dto.Sensor.IsActive),
                //         new List<RequestExit>(),
                //         dto.CardFormat, dto.AntiPassbackMode, dto.AntiPassBackIn, dto.AntiPassBackOut, dto.SpareTags, dto.AccessControlFlags, dto.Mode, dto.ModeDesc,
                //         dto.OfflineMode, dto.OfflineModeDesc, dto.DefaultMode, dto.DefaultModeDesc, dto.DefaultLEDMode, dto.PreAlarm, dto.AntiPassbackDelay,
                //         dto.StrkT2, dto.DcHeld2, dto.StrkFollowPulse, dto.StrkFollowDelay, dto.nExtFeatureType, dto.IlPBSio, dto.IlPBNumber, dto.IlPBLongPress, dto.IlPBOutSio,
                //         dto.IlPBOutNum, dto.DfOfFilterTime, dto.MaskHeldOpen, dto.MaskForceOpen
                //         );

                //         var domainOut = new Door(0, DriverIdOut, dto.Name, dto.DoorType, DriverIdIn,
                //         dto.ScpId,
                //         dto.Readers is null || dto.Readers.Count == 0 ? new List<Reader>() :
                //         new List<Reader>
                //         {
                //         new Reader(
                //             dto.Readers.ElementAt(1).ModuleId,
                //             dto.Readers.ElementAt(1).ModuleDriverId,
                //             dto.Readers.ElementAt(1).DoorId,
                //             DoorDirection.OUT,
                //             dto.Readers.ElementAt(1).ReaderNo,
                //             dto.Readers.ElementAt(1).DataFormat,
                //             dto.Readers.ElementAt(1).KeypadMode,
                //             dto.Readers.ElementAt(1).LedDriveMode,
                //             dto.Readers.ElementAt(1).OsdpFlag,
                //             dto.Readers.ElementAt(1).OsdpBaudrate,
                //             dto.Readers.ElementAt(1).OsdpDiscover,
                //             dto.Readers.ElementAt(1).OsdpTracing,
                //             dto.Readers.ElementAt(1).OsdpAddress,
                //             dto.Readers.ElementAt(1).OsdpSecureChannel,
                //             dto.Readers.ElementAt(1).ScpId,
                //             dto.Readers.ElementAt(1).LocationId,
                //             dto.Readers.ElementAt(1).IsActive
                //         )
                //         },
                //         dto.ReaderOutConfiguration,
                //         null,
                //         null,
                //         new List<RequestExit>(),
                //         dto.CardFormat, dto.AntiPassbackMode, dto.AntiPassBackIn, dto.AntiPassBackOut, dto.SpareTags, dto.AccessControlFlags, dto.Mode, dto.ModeDesc,
                //         dto.OfflineMode, dto.OfflineModeDesc, dto.DefaultMode, dto.DefaultModeDesc, dto.DefaultLEDMode, dto.PreAlarm, dto.AntiPassbackDelay,
                //         dto.StrkT2, dto.DcHeld2, dto.StrkFollowPulse, dto.StrkFollowDelay, dto.nExtFeatureType, dto.IlPBSio, dto.IlPBNumber, dto.IlPBLongPress, dto.IlPBOutSio,
                //         dto.IlPBOutNum, dto.DfOfFilterTime, dto.MaskHeldOpen, dto.MaskForceOpen
                //         );

                //         foreach (var reader in domainIn.Readers)
                //         {
                //             if (reader.ScpId == 0) continue;
                //             short readerInOsdpFlag = 0x00;
                //             short readerLedDriveMode = 0;
                //             if (reader.OsdpFlag)
                //             {
                //                 readerInOsdpFlag |= reader.OsdpBaudrate;
                //                 readerInOsdpFlag |= reader.OsdpDiscover;
                //                 readerInOsdpFlag |= reader.OsdpTracing;
                //                 readerInOsdpFlag |= reader.OsdpAddress;
                //                 readerInOsdpFlag |= reader.OsdpSecureChannel;
                //                 readerLedDriveMode = 7;
                //             }
                //             else
                //             {
                //                 readerLedDriveMode = 1;
                //             }


                //             // Reader In Config
                //             if (!aero.ReaderSpecification((short)reader.ScpId, reader.ModuleDriverId, reader.ReaderNo, reader.DataFormat, reader.KeypadMode, readerLedDriveMode, readerInOsdpFlag))
                //             {
                //                 return ResponseHelper.UnsuccessBuilderWithString<DoorDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(await hw.GetMacFromComponentAsync((short)domainIn.ScpId), Command.READER_SPEC));
                //             }
                //         }

                //         foreach (var reader in domainOut.Readers)
                //         {
                //             if (reader.ScpId == 0) continue;
                //             short readerInOsdpFlag = 0x00;
                //             short readerLedDriveMode = 0;
                //             if (reader.OsdpFlag)
                //             {
                //                 readerInOsdpFlag |= reader.OsdpBaudrate;
                //                 readerInOsdpFlag |= reader.OsdpDiscover;
                //                 readerInOsdpFlag |= reader.OsdpTracing;
                //                 readerInOsdpFlag |= reader.OsdpAddress;
                //                 readerInOsdpFlag |= reader.OsdpSecureChannel;
                //                 readerLedDriveMode = 7;
                //             }
                //             else
                //             {
                //                 readerLedDriveMode = 1;
                //             }


                //             // Reader In Config
                //             if (!aero.ReaderSpecification((short)reader.ScpId, reader.ModuleDriverId, reader.ReaderNo, reader.DataFormat, reader.KeypadMode, readerLedDriveMode, readerInOsdpFlag))
                //             {
                //                 return ResponseHelper.UnsuccessBuilderWithString<DoorDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(await hw.GetMacFromComponentAsync((short)domainOut.ScpId), Command.READER_SPEC));
                //             }
                //         }



                //         // Strike Strike Config
                //         if (domainIn.Strk != null)
                //         {
                //             if (!aero.OutputPointSpecification((short)domainIn.Strk.ScpId, (short)domainIn.Strk.ModuleDriverId, domainIn.Strk.OutputNo, domainIn.Strk.RelayMode))
                //             {
                //                 return ResponseHelper.UnsuccessBuilderWithString<DoorDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(await hw.GetMacFromComponentAsync((short)domainIn.ScpId), Command.OUTPUT_SPEC));
                //             }

                //         }

                //         // door sensor Config
                //         if (domainIn.Sensor != null)
                //         {
                //             if (!aero.InputPointSpecification((short)domainIn.Sensor.ScpId, (short)domainIn.Sensor.ModuleDriverId, domainIn.Sensor.InputNo, domainIn.Sensor.InputMode, domainIn.Sensor.Debounce, domainIn.Sensor.HoldTime))
                //             {
                //                 return ResponseHelper.UnsuccessBuilderWithString<DoorDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(await hw.GetMacFromComponentAsync((short)domainIn.ScpId), Command.INPUT_SPEC));
                //             }
                //         }


                //         // foreach (var rex in domain.RequestExits)
                //         // {
                //         //     if (rex.ScpId == 0) continue;
                //         //     if (!aero.InputPointSpecification((short)rex.ScpId, (short)rex.ModuleDriverId, rex.InputNo, rex.InputMode, rex.Debounce, rex.HoldTime))
                //         //     {
                //         //         return ResponseHelper.UnsuccessBuilderWithString<DoorDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(await hw.GetMacFromComponentAsync((short)domain.ScpId), Command.INPUT_SPEC));
                //         //     }
                //         // }

                //         if (!aero.AccessControlReaderConfiguration((short)domainIn.ScpId, domainIn.AcrId, domainIn, 1))
                //         {
                //             return ResponseHelper.UnsuccessBuilderWithString<DoorDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(await hw.GetMacFromComponentAsync((short)domainIn.ScpId), Command.ACR_CONFIG));
                //         }

                //         if (!aero.AccessControlReaderConfiguration((short)domainOut.ScpId, domainOut.AcrId, domainOut, 2))
                //         {
                //             return ResponseHelper.UnsuccessBuilderWithString<DoorDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(await hw.GetMacFromComponentAsync((short)domainOut.ScpId), Command.ACR_CONFIG));
                //         }



                //         status = await repo.AddAsync(domainIn);
                //         if (status <= 0) return ResponseHelper.UnsuccessBuilder<DoorDto>(ResponseMessage.SAVE_DATABASE_UNSUCCESS, []);

                //         status = await repo.AddAsync(domainOut);
                //         if (status <= 0) return ResponseHelper.UnsuccessBuilder<DoorDto>(ResponseMessage.SAVE_DATABASE_UNSUCCESS, []);

                //         return ResponseHelper.SuccessBuilder(await repo.GetByIdAsync(status));
                //     // // Reader In-Out (OSDP)
                //     // case 3:
                //     // break;
                //     default:
                //         return ResponseHelper.UnsuccessBuilderWithString<DoorDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(await hw.GetMacFromComponentAsync((short)dto.ScpId), Command.ACR_CONFIG));
                // }

            }

            #endregion

            #region Card Holder

            var cards = await userRepo.GetAsync();

            var cdomain = cards.Select(dto => new User(
                dto.UserId,
                dto.Identification,
                dto.Title,
                dto.FirstName,
                dto.MiddleName,
                dto.LastName,
                (Gender)dto.Gender,
                UtilitiesHelper.StringToDate(dto.DateOfBirth),
                dto.Email,
                dto.Phone,
                dto.CompanyId,
                dto.Company,
                dto.PositionId,
                dto.Position,
                dto.DepartmentId,
                dto.Department,
                dto.Address,
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
                        c.AlvlId,
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

                            if (!aero.AccessDatabaseCardRecord((short)deviceId, card.Flag, cred.CardNo, cred.IssueCode, cred.Pin, alvl.Components.Where(x => x.DeviceId == deviceId).Select(x => x.AlvlId).ToList(), (int)UtilitiesHelper.DateTimeToElapeSecond(cred.ActiveDate), (int)UtilitiesHelper.DateTimeToElapeSecond(cred.DeactiveDate)))
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
                if (!await aero.CardFormatterConfiguration((short)en.ScpId, format.CfmtId, format.Fac, 0, 1, 0, format.Bits, format.PeLn, format.PeLn, format.PoLn, format.PoLoc, format.FcLn, format.FcLoc, format.ChLn, format.ChLoc, format.IcLn, format.IcLoc))
                {
                    errors.Add(MessageBuilder.Unsuccess(await repo.GetMacFromComponentAsync((short)en.ScpId), Command.CARDFORMAT_CONFIG));
                }

            }


            #endregion

            #region Transaction


            // Transction

            if (!aero.SetTransactionLogIndex((short)en.ScpId, true))
            {
                errors.Add(MessageBuilder.Unsuccess(await repo.GetMacFromComponentAsync((short)en.ScpId), Command.C208));
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
                 await moduleRepo.CountByDeviceIdAndUpdateTimeAsync(hw.ScpId, hwSyn),
                 await moduleRepo.CountByDeviceIdAndUpdateTimeAsync(hw.ScpId, hwSyn) != 0
            ));


            // MP
            dev.Add(new VerifyHardwareDeviceConfigDto
            (
                 "Monitor Point",
                 await mpRepo.CountByDeviceIdAndUpdateTimeAsync(hw.ScpId, hwSyn),
                 await mpRepo.CountByDeviceIdAndUpdateTimeAsync(hw.ScpId, hwSyn) != 0
            ));

            // CP
            dev.Add(new VerifyHardwareDeviceConfigDto
            ("Control Point",
                 await cpRepo.CountByMacAndUpdateTimeAsync(hw.ScpId, hwSyn),
                 await cpRepo.CountByMacAndUpdateTimeAsync(hw.ScpId, hwSyn) != 0));

            // MPG
            dev.Add(new VerifyHardwareDeviceConfigDto(
                "Monitor Group",
                 await mpgRepo.CountByDriverIdAndUpdateTimeAsync(hw.ScpId, hwSyn),
                 await mpgRepo.CountByDriverIdAndUpdateTimeAsync(hw.ScpId, hwSyn) != 0
            ));

            // ACR
            dev.Add(new VerifyHardwareDeviceConfigDto(
                "Access Control Reader",
                 await doorRepo.CountByDeviceIdAndUpdateTimeAsync(hw.ScpId, hwSyn),
                 await doorRepo.CountByDeviceIdAndUpdateTimeAsync(hw.ScpId, hwSyn) != 0
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
                 await trigRepo.CountByDeviceIdAndUpdateTimeAsync(hw.ScpId, hwSyn),
                 await trigRepo.CountByDeviceIdAndUpdateTimeAsync(hw.ScpId, hwSyn) != 0
            ));

            // Prcedure
            dev.Add(new VerifyHardwareDeviceConfigDto(
                 "Procedure",
                 await procRepo.CountByDeviceIdAndUpdateTimeAsync(hw.ScpId, hwSyn),
                 await procRepo.CountByDeviceIdAndUpdateTimeAsync(hw.ScpId, hwSyn) != 0
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
                dto.ScpId,
                dto.Name,
                dto.HardwareType,
                dto.HardwareTypeDetail,
                dto.Mac,
                dto.Ip,
                dto.Firmware,
                dto.Port,
                new List<Aero.Domain.Entities.Module>()
                {
                    new Aero.Domain.Entities.Module(
                        dto.ScpId,
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
                if (!aero.SioDriverConfiguration(dto.ScpId, 1, 1, dto.BaudRateOne, dto.ProtocolOne))
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
                    if (!aero.SioPanelConfiguration(dto.ScpId, (short)i, -1, 19, 12, 4, (short)i, 1, true))
                    {
                        return ResponseHelper.UnsuccessBuilder<DeviceDto>(MessageBuilder.Unsuccess(dto.Mac, Command.SIO_PANEL_CONFIG), []);
                    }
                }
            }

            if (dto.PortTwo)
            {
                if (!aero.SioDriverConfiguration(dto.ScpId, 2, 2, dto.BaudRateTwo, dto.ProtocolTwo))
                {
                    return ResponseHelper.UnsuccessBuilder<DeviceDto>(MessageBuilder.Unsuccess(dto.Mac, Command.SIO_DRIVER), []);
                }
                ;

                // Send command for address 16-31 on port 2 if allow
                for (int i = 15; i < 31; i++)
                {
                    // model = -1 for allow every model
                    if (!aero.SioPanelConfiguration(dto.ScpId, (short)i, -1, 19, 12, 4, (short)i, 1, true))
                    {
                        return ResponseHelper.UnsuccessBuilder<DeviceDto>(MessageBuilder.Unsuccess(dto.Mac, Command.SIO_PANEL_CONFIG), []);
                    }
                }
            }

            if (!aero.SetTransactionLogIndex(dto.ScpId, true))
            {
                return ResponseHelper.UnsuccessBuilder<DeviceDto>(ResponseMessage.TRANSACTION_ENABLE_FAIL, []);
            }

            var report = await idRepo.GetByMacAndScpIdAsync(dto.Mac, dto.ScpId);

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

            var ens = new Aero.Domain.Entities.Device(
                dto.Id,
                (short)dto.ScpId,
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
            short status = aero.CheckSCPStatus((short)en.ScpId);
            return ResponseHelper.SuccessBuilder(new DeviceStatusDto(en.ScpId, status));
        }




        public async Task<ResponseDto<bool>> SetTransactionAsync(string mac, short IsOn)
        {
            var ScpId = await repo.GetComponentIdFromMacAsync(mac);
            if (ScpId == 0) return ResponseHelper.NotFoundBuilder<bool>();
            if (!aero.SetTransactionLogIndex(ScpId, IsOn == 1 ? true : false))
            {
                return ResponseHelper.UnsuccessBuilder<bool>(MessageBuilder.Unsuccess(mac, Command.TRAN_INDEX), []);
            }
            return ResponseHelper.SuccessBuilder(true);
        }

        public async Task<ResponseDto<bool>> GetTransactionLogStatusAsync(string mac)
        {
            var id = await repo.GetComponentIdFromMacAsync(mac);
            if (id == 0) return ResponseHelper.NotFoundBuilder<bool>();

            if (!aero.GetTransactionLogStatus(id))
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
                aero.GetWebConfigRead(message.id.scp_id, 2);


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

                aero.GetWebConfigRead(message.id.scp_id, 2);


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

                aero.GetWebConfigRead((short)message.ScpId, 3);

            }
            else
            {

                if (message.web_network is not null) await idRepo.UpdateIpAddressAsync(message.ScpId, UtilitiesHelper.IntegerToIp(message.web_network.cIpAddr));

                aero.GetWebConfigRead((short)message.ScpId, 3);
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
