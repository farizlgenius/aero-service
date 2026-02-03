using System.Net;
using Aero.Api.Constants;
using Aero.Application.Constants;
using Aero.Application.DTOs;
using Aero.Application.Helpers;
using Aero.Application.Interface;
using Aero.Application.Interfaces;
using Aero.Application.Mapper;
using Aero.Domain.Entities;
using Aero.Domain.Interface;
using Aero.Domain.Interfaces;

namespace Aero.Application.Services
{
    public class HardwareService(
        IQHwRepository qHw,
        IHwRepository rHw,
        IQModuleRepository qModule,
        IQTzRepository qTz,
        IQAlvlRepository qAlvl,
        IQCfmtRepository qCfmt,
        IQCpRepository qCp,
        IQMpgRepository qMpg,
        IQMpRepository qMp,
        IQDoorRepository qDoor,
        IQHolderRepository qHolder,
        IQAreaRepository qArea,
        IQHolRepository qHol,
        IQIntervalRepository qInterval,
        IQTrigRepository qTrig,
        IQProcRepository qProc,
        IQActionRepository qAction,
        IQIdReportRepository qId,
        IScpCommand scp,
        ISioCommand sio,
        IMpCommand mp,
        ITzCommand tz,
        IAlvlCommand alvl,
        ICfmtCommand cfmt,
        ICpCommand cp,
        IMpgCommand mpg,
        IDoorCommand d,
        IHolderCommand holder,
        INotificationPublisher publisher
        ) : IHardwareService
    {

        #region CRUD 

        public async Task<ResponseDto<IEnumerable<HardwareDto>>> GetAsync()
        {
            var res = await qHw.GetAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<HardwareDto>>(res);
        }

        public async Task<ResponseDto<IEnumerable<HardwareDto>>> GetByLocationAsync(short location)
        {
            var res = await qHw.GetByLocationIdAsync(location);
            return ResponseHelper.SuccessBuilder<IEnumerable<HardwareDto>>(res);
        }

        public async Task<ResponseDto<bool>> DeleteAsync(string mac)
        {
            List<string> errors = new List<string>();
            if (!await qHw.IsAnyByMac(mac)) return ResponseHelper.NotFoundBuilder<bool>();
            var id = await qHw.GetComponentIdFromMacAsync(mac);
            // CP

            // MP

            // ACR

            // Access Area

            // modules Check first 
            if (await qHw.IsAnyModuleReferenceByMacAsync(mac)) return ResponseHelper.FoundReferenceBuilder<bool>();


            if (!scp.DetachScp(id))
            {
                return ResponseHelper.UnsuccessBuilderWithString<bool>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(mac, Command.DELETE_SCP));
            }


            var res = await rHw.DeleteByMacAsync(mac);

            return ResponseHelper.SuccessBuilder<bool>(res > 0 ? true : false);

        }

        #endregion



        public async Task<ResponseDto<bool>> ResetByMacAsync(string mac)
        {
            if (!await qHw.IsAnyByMac(mac)) return ResponseHelper.NotFoundBuilder<bool>();
            var id = await qHw.GetComponentIdFromMacAsync(mac);
            if (id == 0) return ResponseHelper.NotFoundBuilder<bool>();
            if (!scp.ResetScp(id))
            {
                return ResponseHelper.UnsuccessBuilderWithString<bool>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(mac, Command.RESET_SCP));
            }
            return ResponseHelper.SuccessBuilder(true);
        }

        public async Task<ResponseDto<bool>> ResetByComponentAsync(short id)
        {
            string mac = await qHw.GetMacFromComponentAsync(id);
            if (string.IsNullOrEmpty(mac)) return ResponseHelper.NotFoundBuilder<bool>();
            if (!scp.ResetScp(id))
            {
                return ResponseHelper.UnsuccessBuilderWithString<bool>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(mac, Command.RESET_SCP));
            }
            return ResponseHelper.SuccessBuilder(true);
        }

        public async Task<bool> VerifyHardwareConnection(short ScpId)
        {
            var setting = await qHw.GetScpSettingAsync();

            if (setting is null) return false;

            if (!scp.ScpDeviceSpecification(ScpId, setting)) return false;

            return true;
        }

        public async Task<bool> MappingHardwareAndAllocateMemory(short ScpId)
        {
            var setting = await qHw.GetScpSettingAsync();
            if (setting is null) return false;

            if (!scp.ScpDeviceSpecification(ScpId, setting)) return false;

            if (!scp.AccessDatabaseSpecification(ScpId, setting)) return false;

            if (!scp.TimeSet(ScpId)) return false;

            return true;
        }

        public async Task<ResponseDto<bool>> VerifyMemoryAllocateAsyncWithResponse(string Mac)
        {
            var ScpId = await qHw.GetComponentIdFromMacAsync(Mac);
            if (ScpId == 0) return ResponseHelper.NotFoundBuilder<bool>();
            if (!scp.ReadStructureStatus(ScpId))
            {
                return ResponseHelper.UnsuccessBuilder<bool>(MessageBuilder.Unsuccess(Mac, Command.SCP_STRUCTURE_STATUS), []);
            }
            return ResponseHelper.SuccessBuilder<bool>(true);
        }

        public async Task<bool> VerifyMemoryAllocateAsync(string Mac)
        {
            var ScpId = await qHw.GetComponentIdFromMacAsync(Mac);
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

            if (!await qHw.IsAnyByMac(mac)) return ResponseHelper.NotFoundBuilder<bool>();

            var ScpId = await qHw.GetComponentIdFromMacAsync(mac);


            #region Module Upload

            // modules
            var modules = await qModule.GetByMacAsync(mac);


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
                if (!sio.SioPanelConfiguration(ScpId, module.ComponentId, module.Model, module.nInput, module.nOutput, module.nReader, module.Address, module.Msp1No, true))
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
                        if (!mp.InputPointSpecification(ScpId, module.ComponentId, i, 0, 2, 5))
                        {
                            errors.Add(MessageBuilder.Unsuccess(mac, Command.INPUT_SPEC));
                        }
                    }

                }
            }



            #endregion

            #region Time Zone Upload

            // Timezone

            var timezones = await qTz.GetAsync();

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

            var accessLevels = await qAlvl.GetAsync();


            foreach (var a in accessLevels)
            {
                if (a.ComponentId == 1 || a.ComponentId == 2)
                {
                    if (!alvl.AccessLevelConfigurationExtended(ScpId, a.ComponentId, a.ComponentId == 1 ? (short)0 : (short)1))
                    {
                        errors.Add(MessageBuilder.Unsuccess(mac, Command.ALVL_CONFIG));
                    }
                    ;
                }
                else
                {
                    var accss = a.Components.Where(x => x.Mac.Equals(mac)).SelectMany(x => x.DoorComponent.Select(x => new CreateUpdateAccessLevelDoorComponent
                    {
                        AcrId = x.AcrId,
                        TimezoneId = x.TimezoneId
                    })).ToList();

                    if (!alvl.AccessLevelConfigurationExtendedCreate(ScpId, a.ComponentId, accss))
                    {
                        errors.Add(MessageBuilder.Unsuccess(mac, Command.ALVL_CONFIG));
                    }
                }

            }


            #endregion

            #region Card Format Upload

            // Card format

            var formats = await qCfmt.GetAsync();
            foreach (var format in formats)
            {
                if (!cfmt.CardFormatterConfiguration(ScpId, format.ComponentId, format.Facility, 0, 1, 0, format.Bits, format.PeLn, format.PeLn, format.PoLn, format.PoLoc, format.FcLn, format.FcLoc, format.ChLn, format.ChLoc, format.IcLn, format.IcLoc))
                {
                    errors.Add(MessageBuilder.Unsuccess(mac, Command.CARDFORMAT_CONFIG));
                }

            }

            #endregion

            #region Control Point

            // Control Point
            var controls = await qCp.GetByMacAsync(mac);

            foreach (var control in controls)
            {
                // command place here
                short modeNo = await qCp.GetModeNoByOfflineAndRelayModeAsync(control.OfflineMode, control.RelayMode);

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
            var mps = await qMp.GetByMacAsync(mac);

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
            var mpgs = await qMpg.GetByMacAsync(mac);

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
            var doorss = await qDoor.GetByMacAsync(mac);
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

                    var ReaderInId = await qHw.GetComponentIdFromMacAsync(reader.Mac);
                    if (!d.ReaderSpecification(ReaderInId, reader.ModuleId, reader.ReaderNo, reader.DataFormat, reader.KeypadMode, readerLedDriveMode, readerInOsdpFlag))
                    {
                        errors.Add(MessageBuilder.Unsuccess(mac, Command.READER_SPEC));
                    }
                }



                // Strike Strike Config
                var StrikeId = await qHw.GetComponentIdFromMacAsync(door.Strk.Mac);
                if (!cp.OutputPointSpecification(StrikeId, door.Strk.ModuleId, door.Strk.OutputNo, door.Strk.RelayMode))
                {
                    errors.Add(MessageBuilder.Unsuccess(mac, Command.OUTPUT_SPEC));
                }
                ;

                // door sensor Config
                var SensorId = await qHw.GetComponentIdFromMacAsync(door.Sensor.Mac);
                if (!mp.InputPointSpecification(SensorId, door.Sensor.ModuleId, door.Sensor.InputNo, door.Sensor.InputMode, door.Sensor.Debounce, door.Sensor.HoldTime))
                {
                    errors.Add(MessageBuilder.Unsuccess(mac, Command.INPUT_SPEC));
                }

                foreach (var rex in door.RequestExits)
                {
                    if (string.IsNullOrEmpty(rex.Mac)) continue;
                    var Rex0Id = await qHw.GetComponentIdFromMacAsync(rex.Mac);
                    var rexComponentId = await qDoor.GetLowestUnassignedRexNumberAsync();
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

            var cards = await qHolder.GetAsync();

            var cdomain = cards.Select(x => HolderMapper.ToDomain(x)).ToList(); 

            foreach (var card in cdomain)
            {
                //var ScpIds = await context.hardware.Select(x => new { x.component_id, x.mac }).ToArrayAsync();
                var ScpIds = await qHw.GetAsync();
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

            #region Transaction


            // Transction

            if (!scp.SetTransactionLogIndex(ScpId, true))
            {
                errors.Add(MessageBuilder.Unsuccess(mac, Command.C208));
            }


            #endregion

            if (errors.Count > 0) return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.COMMAND_UNSUCCESS, errors);

            if (await rHw.UpdateSyncStatusByMacAsync(mac) <= 0)
            {
                return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.UPLOAD_HW_CONFIG_FAIL, errors);
            }

            return ResponseHelper.SuccessBuilder(true);
        }



        public async Task<List<VerifyHardwareDeviceConfigDto>> VerifyDeviceConfigurationAsync(Hardware hw)
        {
            List<VerifyHardwareDeviceConfigDto> dev = new List<VerifyHardwareDeviceConfigDto>();

            if (hw is null) return dev;

            var hwSyn = hw.LastSync;


            // modules
            dev.Add(new VerifyHardwareDeviceConfigDto
            {
                ComponentName = "modules",
                nMismatchRecord = await qModule.CountByMacAndUpdateTimeAsync(hw.Mac, hwSyn),
                IsUpload = await qModule.CountByMacAndUpdateTimeAsync(hw.Mac, hwSyn) != 0
            });

            // MP
            dev.Add(new VerifyHardwareDeviceConfigDto
            {
                ComponentName = "Monitor Point",
                nMismatchRecord = await qMp.CountByMacAndUpdateTimeAsync(hw.Mac, hwSyn),
                IsUpload = await qMp.CountByMacAndUpdateTimeAsync(hw.Mac, hwSyn) != 0
            });

            // CP
            dev.Add(new VerifyHardwareDeviceConfigDto
            {
                ComponentName = "Control Point",
                nMismatchRecord = await qCp.CountByMacAndUpdateTimeAsync(hw.Mac, hwSyn),
                IsUpload = await qCp.CountByMacAndUpdateTimeAsync(hw.Mac, hwSyn) != 0
            });

            // MPG
            dev.Add(new VerifyHardwareDeviceConfigDto
            {
                ComponentName = "Monitor Group",
                nMismatchRecord = await qMpg.CountByMacAndUpdateTimeAsync(hw.Mac, hwSyn),
                IsUpload = await qMpg.CountByMacAndUpdateTimeAsync(hw.Mac, hwSyn) != 0
            });

            // ACR
            dev.Add(new VerifyHardwareDeviceConfigDto
            {
                ComponentName = "Access Control Reader",
                nMismatchRecord = await qDoor.CountByMacAndUpdateTimeAsync(hw.Mac, hwSyn),
                IsUpload = await qDoor.CountByMacAndUpdateTimeAsync(hw.Mac, hwSyn) != 0
            });

            // Access Level
            dev.Add(new VerifyHardwareDeviceConfigDto
            {
                ComponentName = "Access Level",
                nMismatchRecord = await qAlvl.CountByLocationIdAndUpdateTimeAsync(hw.LocationId, hwSyn),
                IsUpload = await qAlvl.CountByLocationIdAndUpdateTimeAsync(hw.LocationId, hwSyn) != 0
            });

            // Access Area
            dev.Add(new VerifyHardwareDeviceConfigDto
            {
                ComponentName = "Access Area",
                nMismatchRecord = await qArea.CountByLocationIdAndUpdateTimeAsync(hw.LocationId, hwSyn),
                IsUpload = await qArea.CountByLocationIdAndUpdateTimeAsync(hw.LocationId, hwSyn) != 0
            });

            // time Zone
            dev.Add(new VerifyHardwareDeviceConfigDto
            {
                ComponentName = "Time Zone",
                nMismatchRecord = await qTz.CountByLocationIdAndUpdateTimeAsync(hw.LocationId, hwSyn),
                IsUpload = await qTz.CountByLocationIdAndUpdateTimeAsync(hw.LocationId, hwSyn) != 0
            });

            // Holiday
            dev.Add(new VerifyHardwareDeviceConfigDto
            {
                ComponentName = "Holiday",
                nMismatchRecord = await qHol.CountByLocationIdAndUpdateTimeAsync(hw.LocationId, hwSyn),
                IsUpload = await qHol.CountByLocationIdAndUpdateTimeAsync(hw.LocationId, hwSyn) != 0
            });

            // interval
            dev.Add(new VerifyHardwareDeviceConfigDto
            {
                ComponentName = "Interval",
                nMismatchRecord = await qInterval.CountByLocationIdAndUpdateTimeAsync(hw.LocationId, hwSyn),
                IsUpload = await qInterval.CountByLocationIdAndUpdateTimeAsync(hw.LocationId, hwSyn) != 0
            });

            // trigger
            dev.Add(new VerifyHardwareDeviceConfigDto
            {
                ComponentName = "Trigger",
                nMismatchRecord = await qTrig.CountByMacAndUpdateTimeAsync(hw.Mac, hwSyn),
                IsUpload = await qTrig.CountByMacAndUpdateTimeAsync(hw.Mac, hwSyn) != 0
            });

            // Prcedure
            dev.Add(new VerifyHardwareDeviceConfigDto
            {
                ComponentName = "Procedure",
                nMismatchRecord = await qProc.CountByMacAndUpdateTimeAsync(hw.Mac, hwSyn),
                IsUpload = await qProc.CountByMacAndUpdateTimeAsync(hw.Mac, hwSyn) != 0
            });

            // action
            dev.Add(new VerifyHardwareDeviceConfigDto
            {
                ComponentName = "Action",
                nMismatchRecord = await qAction.CountByMacAndUpdateTimeAsync(hw.Mac, hwSyn),
                IsUpload = await qAction.CountByMacAndUpdateTimeAsync(hw.Mac, hwSyn) != 0
            });

            // card_format
            dev.Add(new VerifyHardwareDeviceConfigDto
            {
                ComponentName = "Card Format",
                nMismatchRecord = await qCfmt.CountByUpdateTimeAsync(hwSyn),
                IsUpload = await qCfmt.CountByUpdateTimeAsync(hwSyn) != 0
            }
            );
            return dev;

        }

        public async Task<ResponseDto<IEnumerable<VerifyHardwareDeviceConfigDto>>> VerifyComponentConfigurationAsync(string mac)
        {
            var hardware = await rHw.GetByMacAsync(mac);

            if (hardware is null) return ResponseHelper.NotFoundBuilder<IEnumerable<VerifyHardwareDeviceConfigDto>>();

            var dev = await VerifyDeviceConfigurationAsync(hardware);


            var status = dev.Any(s => s.IsUpload == true);

            await rHw.UpdateVerifyHardwareCofigurationMyMacAsync(mac, status);

            return ResponseHelper.SuccessBuilder<IEnumerable<VerifyHardwareDeviceConfigDto>>(dev);
        }


        public async Task<ResponseDto<bool>> CreateAsync(CreateHardwareDto dto)
        {
            var hardware = HardwareMapper.ToHardware(dto);

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

            var report = await qId.GetByMacAndScpIdAsync(dto.Mac, dto.ComponentId);

            if (report is null) return ResponseHelper.NotFoundBuilder<bool>();

            var status = await rHw.AddAsync(hardware);

            if (status <= 0) return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.SAVE_DATABASE_UNSUCCESS, []);

            status = await qId.DeleteByMacAndScpIdAsync(report.MacAddress, report.ComponentId);

            if (status <= 0) return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.DELETE_DATABASE_UNSUCCESS, []);


            return ResponseHelper.SuccessBuilder(true);
        }


        public async Task<ResponseDto<HardwareDto>> UpdateAsync(HardwareDto dto)
        {

            var en = await qHw.GetByMacAsync(dto.Mac);

            if (en is null) return ResponseHelper.NotFoundBuilder<HardwareDto>();

            var ens = HardwareMapper.ToHardware(dto);

            var status = await rHw.UpdateAsync(ens);

            if (status <= 0) return ResponseHelper.UnsuccessBuilder<HardwareDto>(ResponseMessage.UPDATE_RECORD_UNSUCCESS, []);

            return ResponseHelper.SuccessBuilder<HardwareDto>(dto);
        }


        public async Task<ResponseDto<HardwareStatus>> GetStatusAsync(string mac)
        {
            var ScpId = await qHw.GetComponentIdFromMacAsync(mac);
            if (ScpId == 0) return ResponseHelper.NotFoundBuilder<HardwareStatus>();
            if (!await qHw.IsAnyByMacAndComponent(mac, ScpId)) return ResponseHelper.NotFoundBuilder<HardwareStatus>();
            short status = scp.CheckSCPStatus(ScpId);
            return ResponseHelper.SuccessBuilder(new HardwareStatus()
            {
                Mac = mac,
                Status = status,
                ComponentId = ScpId

            });
        }




        public async Task<ResponseDto<bool>> SetTransactionAsync(string mac, short IsOn)
        {
            var ScpId = await qHw.GetComponentIdFromMacAsync(mac);
            if (ScpId == 0) return ResponseHelper.NotFoundBuilder<bool>();
            if (!scp.SetTransactionLogIndex(ScpId, IsOn == 1 ? true : false))
            {
                return ResponseHelper.UnsuccessBuilder<bool>(MessageBuilder.Unsuccess(mac, Command.TRAN_INDEX), []);
            }
            return ResponseHelper.SuccessBuilder(true);
        }

        public async Task<ResponseDto<bool>> GetTransactionLogStatusAsync(string mac)
        {
            var id = await qHw.GetComponentIdFromMacAsync(mac);
            if (id == 0) return ResponseHelper.NotFoundBuilder<bool>();

            if (!scp.GetTransactionLogStatus(id))
            {
                return ResponseHelper.UnsuccessBuilder<bool>(MessageBuilder.Unsuccess(mac, Command.C402), []);
            }

            return ResponseHelper.SuccessBuilder(true);
        }


        public async Task<ResponseDto<IEnumerable<Mode>>> GetHardwareTypeAsync()
        {
            var dtos = await qHw.GetHardwareTypeAsync();
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
            var res = await qHw.GetByMacAsync(mac);
            return ResponseHelper.SuccessBuilder<HardwareDto>(res);
        }

        public async Task HandleFoundHardware(IScpReply message)
        {
            if (await qHw.IsAnyByMac(UtilitiesHelper.ByteToHexStr(message.id.mac_addr)))
            {
                var hardware = await rHw.GetByMacAsync(UtilitiesHelper.ByteToHexStr(message.id.mac_addr));

                if (hardware is null) return;

                if (!await MappingHardwareAndAllocateMemory(message.id.scp_id))
                {
                    hardware.IsReset = true;
                }
                else
                {
                    hardware.IsReset = false;
                }

                if (!await VerifyMemoryAllocateAsync(hardware.Mac))
                {
                    hardware.IsReset = true;
                }
                else
                {
                    hardware.IsReset = false;
                }

                hardware.Firmware = UtilitiesHelper.ParseFirmware(message.id.sft_rev_major, message.id.sft_rev_minor);

                var component = await VerifyDeviceConfigurationAsync(hardware);

                hardware.IsUpload = component.Any(s => s.IsUpload == true);

                var status = await rHw.UpdateAsync(hardware);

                if (status <= 0) return;

                // Call Get ip
                scp.GetWebConfigRead(message.id.scp_id, 2);


            }
            else
            {
                if (!await VerifyHardwareConnection(message.id.scp_id)) return;


                if (await qHw.IsAnyByMacAndComponent(UtilitiesHelper.ByteToHexStr(message.id.mac_addr), message.id.scp_id))
                {

                    if (await qId.IsAnyByMacAndScpIdAsync(UtilitiesHelper.ByteToHexStr(message.id.mac_addr), message.id.scp_id))
                    {
                        // Delete id report
                        var status = await qId.DeleteByMacAndScpIdAsync(UtilitiesHelper.ByteToHexStr(message.id.mac_addr), message.id.scp_id);
                        if (status <= 0) throw new Exception("Delete Id report from database unsuccess.");
                    }
                    return;
                }
                else
                {
                    if (await qId.IsAnyByMacAndScpIdAsync(UtilitiesHelper.ByteToHexStr(message.id.mac_addr), message.id.scp_id))
                    {
                        // Update
                        var status = await qId.UpdateAsync(message);
                    }
                    else
                    {
                        // Create 
                        var status = await qId.AddAsync(message);
                    }
                }

                scp.GetWebConfigRead(message.id.scp_id, 2);


            }

        }

        public async Task VerifyAllocateHardwareMemoryAsync(IScpReply message)
        {

            var scp = await qHw.GetByComponentIdAsync((short)message.ScpId);

            if (scp is null) return;


            var mems = await qHw.CheckAllocateMemoryAsync(message);

            var res = await rHw.UpdateVerifyMemoryAllocateByComponentIdAsync((short)message.ScpId, mems.Any(x => x.IsSync == false));
            if (res <= 0) return;


            // Check mismatch device configuration
            //await VerifyDeviceConfigurationAsync(hw.mac,hw.location_id);
            var data = new MemoryAllocateDto
            {
                Mac = await qHw.GetMacFromComponentAsync((short)message.ScpId),
                Memories = mems,
            };
            await publisher.ScpNotifyMemoryAllocate(data);
        }

        public async Task AssignIpAddressAsync(IScpReply message)
        {
            if(!await qHw.IsAnyByComponentId((short)message.ScpId))
            {


                if (message.web_network is not null) await rHw.UpdateIpAddressAsync(message.ScpId,UtilitiesHelper.IntegerToIp(message.web_network.cIpAddr));

                scp.GetWebConfigRead((short)message.ScpId, 3);

            }
            else
            {

                if(message.web_network is not null) await qId.UpdateIpAddressAsync(message.ScpId,UtilitiesHelper.IntegerToIp(message.web_network.cIpAddr));

                scp.GetWebConfigRead((short)message.ScpId, 3);
            }


        }

        public async Task AssignPortAsync(IScpReply message)
        {
            if (await qHw.IsAnyByComponentId((short)message.ScpId))
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
                };


                await rHw.UpdatePortAddressAsync((short)message.ScpId,port);

                var dto = await qId.GetAsync();

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

                await qId.UpdatePortAddressAsync((short)message.ScpId,port);

                var dto = await qId.GetAsync();

                await publisher.IdReportNotifyAsync(dto.ToList());
            }


        }




    }
}
