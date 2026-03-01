using Aero.Api.Constants;
using Aero.Application.Constants;
using Aero.Application.DTOs;
using Aero.Application.Helpers;
using Aero.Application.Interface;
using Aero.Application.Interfaces;
using Aero.Domain.Entities;
using Aero.Domain.Interface;
using Aero.Domain.Interfaces;
using System.Net;
using System.Security.Cryptography;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Aero.Application.Services
{
    public class HardwareService(
        IHwRepository hwRepo,
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
        INotificationPublisher publisher
        ) : IHardwareService
    {

        #region CRUD 

        public async Task<ResponseDto<IEnumerable<HardwareDto>>> GetAsync()
        {
            var res = await hwRepo.GetAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<HardwareDto>>(res);
        }

        public async Task<ResponseDto<IEnumerable<HardwareDto>>> GetByLocationAsync(short location)
        {
            var res = await hwRepo.GetByLocationIdAsync(location);
            return ResponseHelper.SuccessBuilder<IEnumerable<HardwareDto>>(res);
        }

        public async Task<ResponseDto<bool>> DeleteAsync(string mac)
        {
            List<string> errors = new List<string>();
            if (!await hwRepo.IsAnyByMac(mac)) return ResponseHelper.NotFoundBuilder<bool>();
            var id = await hwRepo.GetComponentIdFromMacAsync(mac);
            // CP

            // MP

            // ACR

            // Access Area

            // modules Check first 
            if (await hwRepo.IsAnyModuleReferenceByMacAsync(mac)) return ResponseHelper.FoundReferenceBuilder<bool>();


            if (!scp.DetachScp(id))
            {
                return ResponseHelper.UnsuccessBuilderWithString<bool>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(mac, Command.DELETE_SCP));
            }


            var res = await hwRepo.DeleteByMacAsync(mac);

            return ResponseHelper.SuccessBuilder<bool>(res > 0 ? true : false);

        }

        #endregion



        public async Task<ResponseDto<bool>> ResetByMacAsync(string mac)
        {
            if (!await hwRepo.IsAnyByMac(mac)) return ResponseHelper.NotFoundBuilder<bool>();
            var id = await hwRepo.GetComponentIdFromMacAsync(mac);
            if (id == 0) return ResponseHelper.NotFoundBuilder<bool>();
            if (!scp.ResetScp(id))
            {
                return ResponseHelper.UnsuccessBuilderWithString<bool>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(mac, Command.RESET_SCP));
            }
            return ResponseHelper.SuccessBuilder(true);
        }

        public async Task<ResponseDto<bool>> ResetByComponentAsync(short id)
        {
            string mac = await hwRepo.GetMacFromComponentAsync(id);
            if (string.IsNullOrEmpty(mac)) return ResponseHelper.NotFoundBuilder<bool>();
            if (!scp.ResetScp(id))
            {
                return ResponseHelper.UnsuccessBuilderWithString<bool>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(mac, Command.RESET_SCP));
            }
            return ResponseHelper.SuccessBuilder(true);
        }

        public async Task<bool> VerifyHardwareConnection(short ScpId)
        {
            var setting = await hwRepo.GetScpSettingAsync();

            if (setting is null) return false;

            if (!scp.ScpDeviceSpecification(ScpId, setting)) return false;

            return true;
        }

        public async Task<bool> MappingHardwareAndAllocateMemory(short ScpId)
        {
            var setting = await hwRepo.GetScpSettingAsync();
            if (setting is null) return false;

            if (!scp.ScpDeviceSpecification(ScpId, setting)) return false;

            if (!scp.AccessDatabaseSpecification(ScpId, setting)) return false;

            if (!scp.TimeSet(ScpId)) return false;

            return true;
        }

        public async Task<ResponseDto<bool>> VerifyMemoryAllocateAsyncWithResponse(string Mac)
        {
            var ScpId = await hwRepo.GetComponentIdFromMacAsync(Mac);
            if (ScpId == 0) return ResponseHelper.NotFoundBuilder<bool>();
            if (!scp.ReadStructureStatus(ScpId))
            {
                return ResponseHelper.UnsuccessBuilder<bool>(MessageBuilder.Unsuccess(Mac, Command.SCP_STRUCTURE_STATUS), []);
            }
            return ResponseHelper.SuccessBuilder<bool>(true);
        }

        public async Task<bool> VerifyMemoryAllocateAsync(string Mac)
        {
            var ScpId = await hwRepo.GetComponentIdFromMacAsync(Mac);
            if (ScpId == 0) return false;
            if (!scp.ReadStructureStatus(ScpId))
            {
                return false;
            }
            return true;
        }


        public async Task<ResponseDto<bool>> UploadComponentConfigurationAsync(string mac)
        {
            List<string> errors = new List<string>();

            if (!await hwRepo.IsAnyByMac(mac)) return ResponseHelper.NotFoundBuilder<bool>();

            var ScpId = await hwRepo.GetComponentIdFromMacAsync(mac);


            #region Module Upload

            // modules
            var modules = await moduleRepo.GetByMacAsync(mac);


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
                if (!sio.SioDriverConfiguration(ScpId, module.Msp1No, module.Port, module.BaudRate, module.nProtocol))
                {
                    errors.Add(MessageBuilder.Unsuccess(mac, Command.SIO_DRIVER));
                }
                ;

                // Enums.model.HIDAeroX1100
                if (!sio.SioPanelConfiguration(ScpId, module.DriverId, module.Model, module.nInput, module.nOutput, module.nReader, module.Address, module.Msp1No, true))
                {
                    errors.Add(MessageBuilder.Unsuccess(mac, Command.SIO_PANEL_CONFIG));
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
                        if (!mp.InputPointSpecification(ScpId, module.DriverId, i, 0, 2, 5))
                        {
                            errors.Add(MessageBuilder.Unsuccess(mac, Command.INPUT_SPEC));
                        }
                    }

                }
            }



            #endregion

            #region Time Zone Upload

            // Timezone

            var locationId = await hwRepo.GetLocationIdFromMacAsync(mac);

            var timezones = await tzRepo.GetByLocationIdAsync(locationId);

            var timezonesdomain = timezones.Select(x => TimezoneMapper.ToDomain(x)).ToList();

            var intervals = timezones
                .SelectMany(x => x.Intervals)
                .ToList();

            var intervalsdomain = intervals.Select(x => IntervalMapper.ToDomain(x)).ToList();

            foreach (var t in timezonesdomain)
            {
                if (!tz.ExtendedTimeZoneActSpecification(ScpId, t, intervalsdomain, !string.IsNullOrEmpty(t.ActiveTime) ? (int)UtilitiesHelper.DateTimeToElapeSecond(t.ActiveTime) : 0, !string.IsNullOrEmpty(t.DeactiveTime) ? (int)UtilitiesHelper.DateTimeToElapeSecond(t.DeactiveTime) : 0))
                {
                    errors.Add(MessageBuilder.Unsuccess(mac, Command.TIMEZONE_SPEC));
                }
            }


            #endregion

            #region Access Level Upload

            // Access Level

            var accessLevels = await alvlRepo.GetDomainAsync();


            foreach (var domain in accessLevels)
            {
                if (domain.ComponentId == 1 || domain.ComponentId == 2)
                {
                    if (!await alvl.AccessLevelConfigurationExtended(ScpId, domain.ComponentId, domain.ComponentId == 1 ? (short)0 : (short)1))
                    {
                        errors.Add(MessageBuilder.Unsuccess(mac, Command.ALVL_CONFIG));
                    }
                    ;
                }
                else
                {
                    var macs = domain.Components.Select(x => x.Mac).Distinct();

                    for (int i = 0; i < macs.Count(); i++)
                    {
                        //var AlvlId = await qAlvl.GetLowestUnassignedNumberAsync(10, macs.ElementAt(i));
                        //domain.Components.ElementAt(i).AlvlId = AlvlId;
                        if (!await alvl.AccessLevelConfigurationExtended(await hwRepo.GetComponentIdFromMacAsync(macs.ElementAt(i)), domain.Components.Where(x => x.Mac.Equals(macs.ElementAt(i))).Select(x => x.AlvlId).FirstOrDefault(), domain))
                        {
                            errors.Add(MessageBuilder.Unsuccess(macs.ElementAt(i), Command.ALVL_CONFIG));

                        }
                    }
                }

            }


            #endregion

            #region Control Point

            // Control Point
            var controls = await cpRepo.GetByMacAsync(mac);

            foreach (var control in controls)
            {
                // command place here
                short modeNo = await cpRepo.GetModeNoByOfflineAndRelayModeAsync(control.OfflineMode, control.RelayMode);

                if (!cp.OutputPointSpecification(ScpId, control.ModuleId, control.OutputNo, modeNo))
                {
                    errors.Add(MessageBuilder.Unsuccess(mac, Command.OUTPUT_SPEC));
                }


                if (!cp.ControlPointConfiguration(ScpId, control.ModuleId, control.ComponentId, control.OutputNo, control.DefaultPulse))
                {
                    errors.Add(MessageBuilder.Unsuccess(mac, Command.CONTROL_CONFIG));

                }

            }


            #endregion

            #region Monitor Point

            // Monitor Points
            var mps = await mpRepo.GetByMacAsync(mac);

            foreach (var monitors in mps)
            {
                // command place here
                if (!mp.InputPointSpecification(ScpId, monitors.ModuleId, monitors.InputNo, monitors.InputMode, monitors.Debounce, monitors.HoldTime))
                {
                    errors.Add(MessageBuilder.Unsuccess(mac, Command.INPUT_SPEC));
                }


                if (!mp.MonitorPointConfiguration(ScpId, monitors.ModuleId, monitors.InputNo, monitors.LogFunction, monitors.MonitorPointMode, monitors.DelayEntry, monitors.DelayExit, monitors.ComponentId))
                {
                    errors.Add(MessageBuilder.Unsuccess(mac, Command.MONITOR_CONFIG));
                }

            }


            #endregion

            #region Monitor Group

            // Monitor Group
            var mpgs = await mpgRepo.GetByMacAsync(mac);

            var mpgsdomain = mpgs.Select(x => MonitorGroupMapper.ToDomain(x)).ToList();

            foreach (var mpGroup in mpgsdomain)
            {
                if (!mpg.ConfigureMonitorPointGroup(ScpId, mpGroup.ComponentId, mpGroup.nMpCount, mpGroup.nMpList.ToList()))
                {
                    errors.Add(MessageBuilder.Unsuccess(mac, Command.CONFIG_MPG));
                }
            }


            #endregion

            #region Doors

            // door
            var doorss = await doorRepo.GetByDeviceIdAsync(mac);
            var doors = doorss.Select(x => DoorMapper.ToDomain(x)).ToList();

            foreach (var door in doors)
            {
                // command place here

                foreach (var reader in door.Readers)
                {
                    if (string.IsNullOrEmpty(reader.Mac)) continue;
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

                    var ReaderInId = await hwRepo.GetComponentIdFromMacAsync(reader.Mac);
                    if (!d.ReaderSpecification(ReaderInId, reader.ModuleId, reader.ReaderNo, reader.DataFormat, reader.KeypadMode, readerLedDriveMode, readerInOsdpFlag))
                    {
                        errors.Add(MessageBuilder.Unsuccess(mac, Command.READER_SPEC));
                    }
                }



                // Strike Strike Config
                var StrikeId = await hwRepo.GetComponentIdFromMacAsync(door.Strk.Mac);
                if (!cp.OutputPointSpecification(StrikeId, door.Strk.ModuleId, door.Strk.OutputNo, door.Strk.RelayMode))
                {
                    errors.Add(MessageBuilder.Unsuccess(mac, Command.OUTPUT_SPEC));
                }
                ;

                // door sensor Config
                var SensorId = await hwRepo.GetComponentIdFromMacAsync(door.Sensor.Mac);
                if (!mp.InputPointSpecification(SensorId, door.Sensor.ModuleId, door.Sensor.InputNo, door.Sensor.InputMode, door.Sensor.Debounce, door.Sensor.HoldTime))
                {
                    errors.Add(MessageBuilder.Unsuccess(mac, Command.INPUT_SPEC));
                }

                foreach (var rex in door.RequestExits)
                {
                    if (string.IsNullOrEmpty(rex.Mac)) continue;
                    var Rex0Id = await hwRepo.GetComponentIdFromMacAsync(rex.Mac);
                    var rexComponentId = await doorRepo.GetLowestUnassignedRexNumberAsync();
                    rex.ComponentId = rexComponentId;
                    if (!mp.InputPointSpecification(Rex0Id, rex.ModuleId, rex.InputNo, rex.InputMode, rex.Debounce, rex.HoldTime))
                    {
                        errors.Add(MessageBuilder.Unsuccess(mac, Command.INPUT_SPEC));
                    }
                }

                if (!d.AccessControlReaderConfiguration(ScpId, door.ComponentId, door))
                {
                    errors.Add(MessageBuilder.Unsuccess(mac, Command.ACR_CONFIG));
                }

            }

            #endregion

            #region Card Holder

            var cards = await userRepo.GetAsync();

            var cdomain = cards.Select(x => HolderMapper.ToDomain(x)).ToList();

            foreach (var card in cdomain)
            {
                //var ScpIds = await context.hardware.Select(x => new { x.component_id, x.mac }).ToArrayAsync();
                var ScpIds = await hwRepo.GetAsync();
                foreach (var cred in card.Credentials)
                {
                    foreach (var i in ScpIds)
                    {
                        if (!holder.AccessDatabaseCardRecord(i.ComponentId, card.Flag, cred.CardNo, cred.IssueCode, cred.Pin, card.AccessLevels.Select(x => x.ComponentId).ToList(), (int)UtilitiesHelper.DateTimeToElapeSecond(cred.ActiveDate), (int)UtilitiesHelper.DateTimeToElapeSecond(cred.DeactiveDate)))
                        {
                            errors.Add(MessageBuilder.Unsuccess(i.Mac, Command.CARD_RECORD));
                        }
                    }

                }
            }



            #endregion

            #region Card Format Upload

            // Card format

            var formats = await cfmtRepo.GetAsync();
            foreach (var format in formats)
            {
                if (!await cfmt.CardFormatterConfiguration(ScpId, format.ComponentId, format.Facility, 0, 1, 0, format.Bits, format.PeLn, format.PeLn, format.PoLn, format.PoLoc, format.FcLn, format.FcLoc, format.ChLn, format.ChLoc, format.IcLn, format.IcLoc))
                {
                    errors.Add(MessageBuilder.Unsuccess(mac, Command.CARDFORMAT_CONFIG));
                }

            }

            #endregion

            #region Transaction


            // Transction

            if (!scp.SetTransactionLogIndex(ScpId, true))
            {
                errors.Add(MessageBuilder.Unsuccess(mac, Command.C208));
            }


            #endregion

            if (errors.Count > 0) return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.COMMAND_UNSUCCESS, errors);

            if (await hwRepo.UpdateSyncStatusByMacAsync(mac) <= 0)
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
            {
                ComponentName = "modules",
                nMismatchRecord = await moduleRepo.CountByMacAndUpdateTimeAsync(hw.Mac, hwSyn),
                IsUpload = await moduleRepo.CountByMacAndUpdateTimeAsync(hw.Mac, hwSyn) != 0
            });

            // MP
            dev.Add(new VerifyHardwareDeviceConfigDto
            {
                ComponentName = "Monitor Point",
                nMismatchRecord = await mpRepo.CountByMacAndUpdateTimeAsync(hw.Mac, hwSyn),
                IsUpload = await mpRepo.CountByMacAndUpdateTimeAsync(hw.Mac, hwSyn) != 0
            });

            // CP
            dev.Add(new VerifyHardwareDeviceConfigDto
            {
                ComponentName = "Control Point",
                nMismatchRecord = await cpRepo.CountByMacAndUpdateTimeAsync(hw.Mac, hwSyn),
                IsUpload = await cpRepo.CountByMacAndUpdateTimeAsync(hw.Mac, hwSyn) != 0
            });

            // MPG
            dev.Add(new VerifyHardwareDeviceConfigDto
            {
                ComponentName = "Monitor Group",
                nMismatchRecord = await mpgRepo.CountByMacAndUpdateTimeAsync(hw.Mac, hwSyn),
                IsUpload = await mpgRepo.CountByMacAndUpdateTimeAsync(hw.Mac, hwSyn) != 0
            });

            // ACR
            dev.Add(new VerifyHardwareDeviceConfigDto
            {
                ComponentName = "Access Control Reader",
                nMismatchRecord = await doorRepo.CountByMacAndUpdateTimeAsync(hw.Mac, hwSyn),
                IsUpload = await doorRepo.CountByMacAndUpdateTimeAsync(hw.Mac, hwSyn) != 0
            });

            // Access Level
            dev.Add(new VerifyHardwareDeviceConfigDto
            {
                ComponentName = "Access Level",
                nMismatchRecord = await alvlRepo.CountByLocationIdAndUpdateTimeAsync(hw.LocationId, hwSyn),
                IsUpload = await alvlRepo.CountByLocationIdAndUpdateTimeAsync(hw.LocationId, hwSyn) != 0
            });

            // Access Area
            dev.Add(new VerifyHardwareDeviceConfigDto
            {
                ComponentName = "Access Area",
                nMismatchRecord = await areaRepo.CountByLocationIdAndUpdateTimeAsync(hw.LocationId, hwSyn),
                IsUpload = await areaRepo.CountByLocationIdAndUpdateTimeAsync(hw.LocationId, hwSyn) != 0
            });

            // time Zone
            dev.Add(new VerifyHardwareDeviceConfigDto
            {
                ComponentName = "Time Zone",
                nMismatchRecord = await tzRepo.CountByLocationIdAndUpdateTimeAsync(hw.LocationId, hwSyn),
                IsUpload = await tzRepo.CountByLocationIdAndUpdateTimeAsync(hw.LocationId, hwSyn) != 0
            });

            // Holiday
            dev.Add(new VerifyHardwareDeviceConfigDto
            {
                ComponentName = "Holiday",
                nMismatchRecord = await holRepo.CountByLocationIdAndUpdateTimeAsync(hw.LocationId, hwSyn),
                IsUpload = await holRepo.CountByLocationIdAndUpdateTimeAsync(hw.LocationId, hwSyn) != 0
            });

            // interval
            dev.Add(new VerifyHardwareDeviceConfigDto
            {
                ComponentName = "Interval",
                nMismatchRecord = await intervalRepo.CountByLocationIdAndUpdateTimeAsync(hw.LocationId, hwSyn),
                IsUpload = await intervalRepo.CountByLocationIdAndUpdateTimeAsync(hw.LocationId, hwSyn) != 0
            });

            // trigger
            dev.Add(new VerifyHardwareDeviceConfigDto
            {
                ComponentName = "Trigger",
                nMismatchRecord = await trigRepo.CountByMacAndUpdateTimeAsync(hw.Mac, hwSyn),
                IsUpload = await trigRepo.CountByMacAndUpdateTimeAsync(hw.Mac, hwSyn) != 0
            });

            // Prcedure
            dev.Add(new VerifyHardwareDeviceConfigDto
            {
                ComponentName = "Procedure",
                nMismatchRecord = await procRepo.CountByMacAndUpdateTimeAsync(hw.Mac, hwSyn),
                IsUpload = await procRepo.CountByMacAndUpdateTimeAsync(hw.Mac, hwSyn) != 0
            });

            // action
            dev.Add(new VerifyHardwareDeviceConfigDto
            {
                ComponentName = "Action",
                nMismatchRecord = await actionRepo.CountByMacAndUpdateTimeAsync(hw.Mac, hwSyn),
                IsUpload = await actionRepo.CountByMacAndUpdateTimeAsync(hw.Mac, hwSyn) != 0
            });

            // card_format
            dev.Add(new VerifyHardwareDeviceConfigDto
            {
                ComponentName = "Card Format",
                nMismatchRecord = await cfmtRepo.CountByUpdateTimeAsync(hwSyn),
                IsUpload = await cfmtRepo.CountByUpdateTimeAsync(hwSyn) != 0
            }
            );
            return dev;

        }

        public async Task<ResponseDto<IEnumerable<VerifyHardwareDeviceConfigDto>>> VerifyComponentConfigurationAsync(string mac)
        {
            var hardware = await hwRepo.GetDomainByMacAsync(mac);

            if (hardware is null) return ResponseHelper.NotFoundBuilder<IEnumerable<VerifyHardwareDeviceConfigDto>>();

            var dev = await VerifyDeviceConfigurationAsync(hardware);


            var status = dev.Any(s => s.IsUpload == true);

            await hwRepo.UpdateVerifyHardwareCofigurationMyMacAsync(mac, status);

            return ResponseHelper.SuccessBuilder<IEnumerable<VerifyHardwareDeviceConfigDto>>(dev);
        }


        public async Task<ResponseDto<bool>> CreateAsync(CreateDeviceDto dto)
        {
            var ComponentId = await moduleRepo.GetLowestUnassignedNumberAsync(10, "");
            if (ComponentId == -1) return ResponseHelper.ExceedLimit<bool>();
            var hardware = HardwareMapper.ToDomain(dto);
            hardware.Modules[0].ComponentId = ComponentId;

            if (!await VerifyMemoryAllocateAsync(hardware.Mac))
            {
                hardware.IsReset = true;
            }


            var component = await VerifyDeviceConfigurationAsync(hardware);

            hardware.IsUpload = component.Any(s => s.IsUpload == true);

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
                if (!sio.SioDriverConfiguration(dto.ComponentId, 1, 1, dto.BaudRateOne, dto.ProtocolOne))
                {
                    return ResponseHelper.UnsuccessBuilder<bool>(MessageBuilder.Unsuccess(dto.Mac, Command.SIO_DRIVER), []);
                }
                ;

                // Send command for address 0-15 on port 1 if allow
                for (int i = 0; i < 16; i++)
                {
                    // model = -1 for allow every model
                    // n_input = 19 Maximum
                    // n_output = 12 Maximum
                    // n_reader = 4 Maximum
                    if (!sio.SioPanelConfiguration(dto.ComponentId, (short)i, -1, 19, 12, 4, (short)i, 1, true))
                    {
                        return ResponseHelper.UnsuccessBuilder<bool>(MessageBuilder.Unsuccess(dto.Mac, Command.SIO_PANEL_CONFIG), []);
                    }
                }
            }

            if (dto.PortTwo)
            {
                if (!sio.SioDriverConfiguration(dto.ComponentId, 2, 2, dto.BaudRateTwo, dto.ProtocolTwo))
                {
                    return ResponseHelper.UnsuccessBuilder<bool>(MessageBuilder.Unsuccess(dto.Mac, Command.SIO_DRIVER), []);
                }
                ;

                // Send command for address 16-31 on port 2 if allow
                for (int i = 15; i < 31; i++)
                {
                    // model = -1 for allow every model
                    if (!sio.SioPanelConfiguration(dto.ComponentId, (short)i, -1, 19, 12, 4, (short)i, 1, true))
                    {
                        return ResponseHelper.UnsuccessBuilder<bool>(MessageBuilder.Unsuccess(dto.Mac, Command.SIO_PANEL_CONFIG), []);
                    }
                }
            }

            if (!scp.SetTransactionLogIndex(dto.ComponentId, true))
            {
                return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.TRANSACTION_ENABLE_FAIL, []);
            }

            var report = await idRepo.GetByMacAndScpIdAsync(dto.Mac, dto.ComponentId);

            if (report is null) return ResponseHelper.NotFoundBuilder<bool>();

            var status = await hwRepo.AddAsync(hardware);

            if (status <= 0) return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.SAVE_DATABASE_UNSUCCESS, []);

            status = await idRepo.DeleteByMacAndScpIdAsync(report.MacAddress, report.ComponentId);

            if (status <= 0) return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.DELETE_DATABASE_UNSUCCESS, []);


            return ResponseHelper.SuccessBuilder(true);
        }


        public async Task<ResponseDto<HardwareDto>> UpdateAsync(HardwareDto dto)
        {

            var en = await hwRepo.GetByMacAsync(dto.Mac);

            if (en is null) return ResponseHelper.NotFoundBuilder<HardwareDto>();

            var ens = HardwareMapper.ToDomain(dto);

            var status = await hwRepo.UpdateAsync(ens);

            if (status <= 0) return ResponseHelper.UnsuccessBuilder<HardwareDto>(ResponseMessage.UPDATE_RECORD_UNSUCCESS, []);

            return ResponseHelper.SuccessBuilder<HardwareDto>(dto);
        }


        public async Task<ResponseDto<HardwareStatusDto>> GetStatusAsync(string mac)
        {
            var ScpId = await hwRepo.GetComponentIdFromMacAsync(mac);
            if (ScpId == 0) return ResponseHelper.NotFoundBuilder<HardwareStatusDto>();
            if (!await hwRepo.IsAnyByMacAndComponent(mac, ScpId)) return ResponseHelper.NotFoundBuilder<HardwareStatusDto>();
            short status = scp.CheckSCPStatus(ScpId);
            return ResponseHelper.SuccessBuilder(new HardwareStatusDto()
            {
                Mac = mac,
                Status = status,
                ComponentId = ScpId

            });
        }




        public async Task<ResponseDto<bool>> SetTransactionAsync(string mac, short IsOn)
        {
            var ScpId = await hwRepo.GetComponentIdFromMacAsync(mac);
            if (ScpId == 0) return ResponseHelper.NotFoundBuilder<bool>();
            if (!scp.SetTransactionLogIndex(ScpId, IsOn == 1 ? true : false))
            {
                return ResponseHelper.UnsuccessBuilder<bool>(MessageBuilder.Unsuccess(mac, Command.TRAN_INDEX), []);
            }
            return ResponseHelper.SuccessBuilder(true);
        }

        public async Task<ResponseDto<bool>> GetTransactionLogStatusAsync(string mac)
        {
            var id = await hwRepo.GetComponentIdFromMacAsync(mac);
            if (id == 0) return ResponseHelper.NotFoundBuilder<bool>();

            if (!scp.GetTransactionLogStatus(id))
            {
                return ResponseHelper.UnsuccessBuilder<bool>(MessageBuilder.Unsuccess(mac, Command.C402), []);
            }

            return ResponseHelper.SuccessBuilder(true);
        }


        public async Task<ResponseDto<IEnumerable<Mode>>> GetHardwareTypeAsync()
        {
            var dtos = await hwRepo.GetHardwareTypeAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<Mode>>(dtos);
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

        public async Task<ResponseDto<HardwareDto>> GetByMacAsync(string mac)
        {
            var res = await hwRepo.GetByMacAsync(mac);
            return ResponseHelper.SuccessBuilder<HardwareDto>(res);
        }

        public async Task HandleFoundHardware(IScpReply message)
        {
            if (await hwRepo.IsAnyByMac(UtilitiesHelper.ByteToHexStr(message.id.mac_addr)))
            {
                var hardware = await hwRepo.GetDomainByMacAsync(UtilitiesHelper.ByteToHexStr(message.id.mac_addr));

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

                var status = await hwRepo.UpdateAsync(hardware);

                if (status <= 0) return;

                // Call Get ip
                scp.GetWebConfigRead(message.id.scp_id, 2);


            }
            else
            {
                if (!await VerifyHardwareConnection(message.id.scp_id)) return;


                if (await hwRepo.IsAnyByMacAndComponent(UtilitiesHelper.ByteToHexStr(message.id.mac_addr), message.id.scp_id))
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

            var scp = await hwRepo.GetByIdAsync(message.ScpId);

            if (scp is null) return;


            var mems = await hwRepo.CheckAllocateMemoryAsync(message);

            var res = await hwRepo.UpdateVerifyMemoryAllocateByComponentIdAsync((short)message.ScpId, mems.Any(x => x.IsSync == false));
            if (res <= 0) return;


            // Check mismatch device configuration
            //await VerifyDeviceConfigurationAsync(hw.mac,hw.location_id);
            var data = new MemoryAllocateDto(await hwRepo.GetMacFromComponentAsync((short)message.ScpId),mems);
            await publisher.ScpNotifyMemoryAllocate(data);
        }

        public async Task AssignIpAddressAsync(IScpReply message)
        {
            if (await hwRepo.IsAnyById((short)message.ScpId))
            {
                if (message.web_network is not null) await hwRepo.UpdateIpAddressAsync(message.ScpId, UtilitiesHelper.IntegerToIp(message.web_network.cIpAddr));

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
            if (await hwRepo.IsAnyById((short)message.ScpId))
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


                await hwRepo.UpdatePortAddressAsync((short)message.ScpId, port);

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

        public async Task<ResponseDto<Pagination<HardwareDto>>> GetPaginationAsync(PaginationParamsWithFilter param, short location)
        {
            var res = await hwRepo.GetPaginationAsync(param, location);
            return ResponseHelper.SuccessBuilder(res);
        }
    }
}
