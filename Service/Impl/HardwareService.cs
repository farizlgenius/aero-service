using HID.Aero.ScpdNet.Wrapper;
using HIDAeroService.AeroLibrary;
using HIDAeroService.Constant;
using HIDAeroService.Constants;
using HIDAeroService.Data;
using HIDAeroService.DTO;
using HIDAeroService.DTO.AccessLevel;
using HIDAeroService.DTO.Hardware;
using HIDAeroService.DTO.Scp;
using HIDAeroService.Entity;
using HIDAeroService.Helpers;
using HIDAeroService.Hubs;
using HIDAeroService.Mapper;
using HIDAeroService.Model;
using HIDAeroService.Utility;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using MiNET.Entities.Passive;
using System.ComponentModel;
using static HIDAeroService.AeroLibrary.Description;


namespace HIDAeroService.Service.Impl
{
    public class HardwareService(AppDbContext context, AeroCommand command, IHubContext<AeroHub> hub,ITimeZoneService timeZoneService,ICardFormatService cardFormatService,IAccessLevelService accessLevelService, IHelperService<Hardware> helperService, CommandService cmndService, ICredentialService credentialService) : IHardwareService
    {

        #region CRUD 

        public async Task<ResponseDto<IEnumerable<HardwareDto>>> GetAsync()
        {
            var dtos = await context.Hardwares
                .AsNoTracking()
                .Include(s => s.Module)
                .ThenInclude(s => s.Readers)
                .Include(s => s.Module)
                .ThenInclude(s => s.Sensors)
                .Include(s => s.Module)
                .ThenInclude(s => s.Strikes)
                .Select(s => MapperHelper.HardwareToHardwareDto(s))
                .ToArrayAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<HardwareDto>>(dtos);
        }

        public async Task<ResponseDto<IEnumerable<HardwareDto>>> GetByLocationAsync(short location)
        {
            var dtos = await context.Hardwares
                 .AsNoTracking()
                .Include(s => s.Module)
                .ThenInclude(s => s.Readers)
                .Include(s => s.Module)
                .ThenInclude(s => s.Sensors)
                .Include(s => s.Module)
                .ThenInclude(s => s.Strikes)
                .Where(s => s.LocationId == location)
                .Select(s => MapperHelper.HardwareToHardwareDto(s))
                .ToArrayAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<HardwareDto>>(dtos);
        }

        public async Task<ResponseDto<HardwareDto>> DeleteAsync(string mac)
        {
            List<string> errors = new List<string>();
            var entity = await context.Hardwares.FirstOrDefaultAsync(x => x.MacAddress == mac);
            if (entity == null) return ResponseHelper.NotFoundBuilder<HardwareDto>();
            var id = await helperService.GetIdFromMacAsync(mac);
            // CP

            // MP

            // ACR

            // Access Area

            // Module

            //if (!await command.DeleteScpAsync(id))
            //{
            //    _logger.LogError(Helper.ResponseCommandUnsuccessMessageBuilder(mac, id, ResponseMessage.C208));
            //    errors.Add(Helper.ResponseCommandUnsuccessMessageBuilder(mac, id, ResponseMessage.C208));
            //}

            if (!await command.DetachScpAsync(id))
            {
                return ResponseHelper.UnsuccessBuilder<HardwareDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(mac, Command.C015));
            }

            context.Hardwares.Remove(entity);
            await context.SaveChangesAsync();

            var dto = MapperHelper.HardwareToHardwareDto(entity);

            return ResponseHelper.SuccessBuilder<HardwareDto>(dto);

        }

        #endregion

        #region Web Socket

        public void TriggerDeviceStatus(string ScpMac, int CommStatus)
        {
            //GetOnlineStatus()
            var result = hub.Clients.All.SendAsync("CommStatus", ScpMac, CommStatus);
        }

        public void TriggerSyncMemoryAllocate(string mac, List<MemoryAllocateDto> mem)
        {
            //GetOnlineStatus()
            var result = hub.Clients.All.SendAsync("MemoryAllocate", mac, mem);
        }

        public void TriggerSyncDeviceConfiguration(string mac, short location, List<VerifyHardwareDeviceConfigDto> dev)
        {
            var result = hub.Clients.All.SendAsync("DeviceConfiguration", mac, location, dev);
        }

        public void TriggerUploadMessage(string message, bool isFinish = false)
        {
            //GetOnlineStatus()
            var result = hub.Clients.All.SendAsync("UploadStatus", message, isFinish);
        }

        public void TriggerIdReport(List<IdReport> IdReports)
        {
            hub.Clients.All.SendAsync("IdReport", IdReports);
        }


        #endregion




        public async Task<bool> IsHardwareRegister(string mac)
        {
            return await context.Hardwares.AnyAsync(s => s.MacAddress == mac);
        }




        public async Task<ResponseDto<bool>> ResetAsync(string mac)
        {
            List<string> errors = new List<string>();
            if (!await context.Hardwares.AnyAsync(x => x.MacAddress == mac)) return ResponseHelper.NotFoundBuilder<bool>();
            var id = await helperService.GetIdFromMacAsync(mac);
            if(id == 0) return ResponseHelper.NotFoundBuilder<bool>();
            if (!await command.ResetSCP(id))
            {
                return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.COMMAND_UNSUCCESS,MessageBuilder.Unsuccess(mac,Command.C301));
            }
            return ResponseHelper.SuccessBuilder(true);
        }

        public async Task<ResponseDto<bool>> ResetAsync(short id)
        {
            if (!await command.ResetSCP(id))
            {
                return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess("", Command.C301));
            }
            return ResponseHelper.SuccessBuilder(true);
        }

        public async Task<bool> VerifyHardwareConnection(short ScpId)
        {
            var setting = await context.SystemSettings
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (setting is null) return false;

            if (!await command.SCPDeviceSpecification(ScpId, setting)) return false;

            return true;
        }

        public async Task<bool> MappingHardwareAndAllocateMemory(short ScpId)
        {
            var data = await context.SystemSettings.AsNoTracking().FirstOrDefaultAsync();
            if (data is null) return false;

            if (!await command.SCPDeviceSpecification(ScpId, data)) return false;

            if (!await command.AccessDatabaseSpecificationAsync(ScpId, data)) return false;

            if (!await command.TimeSetAsync(ScpId)) return false;

            return true;
        }

        public async Task<ResponseDto<bool>> VerifyMemoryAllocateAsyncWithResponse(string Mac)
        {
            var ScpId = await helperService.GetIdFromMacAsync(Mac);
            if (ScpId == 0) return ResponseHelper.NotFoundBuilder<bool>();
            if (!await command.ReadStructureStatusAsync(ScpId))
            {
                return ResponseHelper.UnsuccessBuilder<bool>(MessageBuilder.Unsuccess(Mac, Command.C1853), []); 
            }
            return ResponseHelper.SuccessBuilder<bool>(true);
        }

        public async Task<bool> VerifyMemoryAllocateAsync(string Mac)
        {
            var ScpId = await helperService.GetIdFromMacAsync(Mac);
            if (ScpId == 0) return false;
            if (!await command.ReadStructureStatusAsync(ScpId))
            {
                return false;
            }
            return true;
        }


        public async Task<ResponseDto<bool>> UploadComponentConfigurationAsync(string mac)
        {
            List<string> errors = new List<string>();
            short id = await helperService.GetIdFromMacAsync(mac);
            var entity = await context.Hardwares
                .Where(x => x.MacAddress == mac)
                .OrderBy(x => x.ComponentId)
                .FirstOrDefaultAsync();

            if (entity is null) return ResponseHelper.NotFoundBuilder<bool>();

            var ScpId = await helperService.GetIdFromMacAsync(mac);


            #region Module Upload

            // Modules
            var modules = await context.Modules
                .AsNoTracking()
                .Where(x => x.MacAddress == mac)
                .ToArrayAsync();

            foreach (var module in modules)
            {
                // Command place here
                if (!await command.SioDriverConfigurationAsync(ScpId, module.Msp1No, module.Port, module.BaudRate, module.nProtocol))
                {
                    errors.Add(MessageBuilder.Unsuccess(mac, Command.C108));
                };

                // Enums.Model.HIDAeroX1100
                if (!await command.SioPanelConfigurationAsync(ScpId, (short)module.ComponentId, module.ModelNo, module.nInput, module.nOutput, module.nReader, module.Address, module.Msp1No, true))
                {
                    errors.Add(MessageBuilder.Unsuccess(mac, Command.C109));
                };

                // Setting Input for Alarm 
                for (short i = 0; i < module.nInput; i++)
                {
                    if (i + 1 >= module.nInput - 3)
                    {
                        if (!await command.InputPointSpecificationAsync(ScpId, module.ComponentId, i, 0, 2, 5))
                        {
                            errors.Add(MessageBuilder.Unsuccess(mac, Command.C110));
                        }
                    }

                }
            }



            #endregion

            #region Time Zone Upload

            // Timezone

            var timezones = await context.TimeZones
                .AsNoTracking()
                .Include(x => x.TimeZoneIntervals)
                .ThenInclude(x => x.Interval)
                .ThenInclude(x => x.Days)
                .ToArrayAsync();

            var intervals = timezones
                .SelectMany(x => x.TimeZoneIntervals.Select(x => x.Interval))
                .ToList();

            foreach (var tz in timezones)
            {
                if (!await command.ExtendedTimeZoneActSpecificationAsync(ScpId, tz, intervals, !string.IsNullOrEmpty(tz.ActiveTime) ? (int)helperService.DateTimeToElapeSecond(tz.ActiveTime) : 0, !string.IsNullOrEmpty(tz.DeactiveTime) ? (int)helperService.DateTimeToElapeSecond(tz.DeactiveTime) : 0))
                {
                    errors.Add(MessageBuilder.Unsuccess(mac, Command.C3103));
                }
            }


            #endregion

            #region Access Level Upload

            // Access Level

            var accessLevels = await context.AccessLevels
           .AsNoTracking()
           .Include(x => x.AccessLevelDoorTimeZones)
           .ToArrayAsync();

            var tzs = accessLevels.SelectMany(x => x.AccessLevelDoorTimeZones).Select(x => new CreateUpdateAccessLevelDoorTimeZoneDto
            {
                TimeZoneId = x.TimeZoneId,
                DoorId = x.DoorId,
            }).ToList();

            foreach (var a in accessLevels)
            {
                if (a.ComponentId == 1 || a.ComponentId == 2)
                {
                    if (!await command.AccessLevelConfigurationExtendedAsync(ScpId, a.ComponentId, a.ComponentId == 1 ? (short)0 : (short)1))
                    {
                        errors.Add(MessageBuilder.Unsuccess(mac, Command.C2116));
                    };
                }
                else
                {
                    if (!await command.AccessLevelConfigurationExtendedCreateAsync(ScpId, a.ComponentId, tzs))
                    {
                        errors.Add(MessageBuilder.Unsuccess(mac, Command.C2116));
                    }
                }

            }


            #endregion

            #region Card Format Upload

            // Card format

            var formats = await context.CardFormats.AsNoTracking().ToArrayAsync();
            foreach (var format in formats)
            {
                if (!await command.CardFormatterConfigurationAsync(ScpId, format.ComponentId, format.Facility, 0, 1, 0, format.Bits, format.PeLn, format.PeLoc, format.PoLn, format.PoLoc, format.FcLn, format.FcLoc, format.ChLn, format.ChLoc, format.IcLn, format.IcLoc))
                {
                    errors.Add(MessageBuilder.Unsuccess(mac, Command.C1102));
                };
            }

            #endregion

            #region Control Point

            // Control Point
            var cps = await context.ControlPoints
                .AsNoTracking()
                .Where(x => x.MacAddress == mac)
                .ToArrayAsync();

            foreach (var cp in cps)
            {
                // Command place here
                short modeNo = await context.OutputModes
                    .AsNoTracking()
                    .Where(x => x.OfflineMode == cp.OfflineMode && x.RelayMode == cp.RelayMode)
                    .Select(x => x.Value).FirstOrDefaultAsync();

                if (!await command.OutputPointSpecificationAsync(ScpId, cp.ModuleId, cp.OutputNo, modeNo))
                {
                    errors.Add(MessageBuilder.Unsuccess(mac, Command.C111));
                }


                if (!await command.ControlPointConfigurationAsync(ScpId, cp.ModuleId, cp.ComponentId, cp.OutputNo, cp.DefaultPulse))
                {
                    errors.Add(MessageBuilder.Unsuccess(mac, Command.C114));

                }

            }


            #endregion

            #region Monitor Point

            // Monitor Points
            var mps = await context.MonitorPoints
                .AsNoTracking()
                .Where(x => x.MacAddress == mac)
                .ToArrayAsync();

            foreach (var mp in mps)
            {
                // Command place here
                if (!await command.InputPointSpecificationAsync(ScpId, mp.ModuleId, mp.InputNo, mp.InputMode, mp.Debounce, mp.HoldTime))
                {
                    errors.Add(MessageBuilder.Unsuccess(mac, Command.C110));
                }


                if (!await command.MonitorPointConfigurationAsync(ScpId, mp.ModuleId, mp.InputNo, mp.LogFunction, mp.MonitorPointMode, mp.DelayEntry, mp.DelayExit, mp.ComponentId))
                {
                    errors.Add(MessageBuilder.Unsuccess(mac, Command.C113));
                }

            }


            #endregion

            #region Monitor Group

            // Monitor Group
            var mpgs = await context.MonitorGroups
                .AsNoTracking()
                .Include(x => x.nMpList)
                .Where(x => x.MacAddress == mac)
                .ToArrayAsync();

            foreach (var mpg in mpgs)
            {
                if (!await command.ConfigureMonitorPointGroup(ScpId, mpg.ComponentId, mpg.nMpCount, mpg.nMpList.ToList()))
                {
                    errors.Add(MessageBuilder.Unsuccess(mac, Command.C120));
                }
            }


            #endregion

            #region Doors

            // Doors
            var doors = await context.Doors
                .AsNoTracking()
                .Include(x => x.Readers)
                .Include(x => x.RequestExits)
                .Include(x => x.Strk)
                .Include(x => x.Sensor)
                .Where(x => x.MacAddress == mac)
                .ToArrayAsync();

            foreach (var door in doors)
            {
                // Command place here

                foreach (var reader in door.Readers)
                {
                    if (string.IsNullOrEmpty(reader.MacAddress)) continue;
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

                    var ReaderInId = await helperService.GetIdFromMacAsync(reader.MacAddress);
                    if (!await command.ReaderSpecificationAsync(ReaderInId, reader.ModuleId, reader.ReaderNo, reader.DataFormat, reader.KeypadMode, readerLedDriveMode, readerInOsdpFlag))
                    {
                        errors.Add(MessageBuilder.Unsuccess(mac,Command.C112));
                    }
                }



                // Strike Strike Config
                var StrikeId = await helperService.GetIdFromMacAsync(door.Strk.MacAddress);
                if (!await command.OutputPointSpecificationAsync(StrikeId, door.Strk.ModuleId, door.Strk.OutputNo, door.Strk.RelayMode))
                {
                    errors.Add(MessageBuilder.Unsuccess(mac, Command.C111));
                };

                // Door Sensor Config
                var SensorId = await helperService.GetIdFromMacAsync(door.Sensor.MacAddress);
                if (!await command.InputPointSpecificationAsync(SensorId, door.Sensor.ModuleId, door.Sensor.InputNo, door.Sensor.InputMode, door.Sensor.Debounce, door.Sensor.HoldTime))
                {
                    errors.Add(MessageBuilder.Unsuccess(mac, Command.C110));
                }

                foreach (var rex in door.RequestExits)
                {
                    if (string.IsNullOrEmpty(rex.MacAddress)) continue;
                    var Rex0Id = await helperService.GetIdFromMacAsync(rex.MacAddress);
                    var rexComponentId = await helperService.GetLowestUnassignedNumberNoLimitAsync<RequestExit>(context);
                    rex.ComponentId = rexComponentId;
                    if (!await command.InputPointSpecificationAsync(Rex0Id, rex.ModuleId, rex.InputNo, rex.InputMode, rex.Debounce, rex.HoldTime))
                    {
                        errors.Add(MessageBuilder.Unsuccess(mac, Command.C110));
                    }
                }

                if (!await command.AccessControlReaderConfigurationAsync(ScpId, door.ComponentId, door))
                {
                    errors.Add(MessageBuilder.Unsuccess(mac,Command.C115));
                }

            }

            #endregion

            #region Card Holder

            var cards = await context.CardHolders
                .AsNoTracking()
                .Include(x => x.Credentials)
                .Include(x => x.AccessLevels)
                .ThenInclude(x => x.AccessLevel)
                .ToArrayAsync();

            foreach(var card in cards)
            {
                var ScpIds = await context.Hardwares.Select(x => new { x.ComponentId, x.MacAddress }).ToArrayAsync();
                foreach (var cred in card.Credentials)
                {
                    foreach (var i in ScpIds)
                    {
                        if (!await command.AccessDatabaseCardRecordAsync(i.ComponentId, card.Flag, cred.CardNo, cred.IssueCode, cred.Pin, card.AccessLevels.Select(x => x.AccessLevel).ToList(), (int)helperService.DateTimeToElapeSecond(cred.ActiveDate), (int)helperService.DateTimeToElapeSecond(cred.DeactiveDate)))
                        {
                            errors.Add(MessageBuilder.Unsuccess(i.MacAddress, Command.C8304));
                        }
                    }

                }
            }



            #endregion

            #region Procedure



            #endregion

            #region Trigger


            #endregion

            #region Transaction


            // Transction

            if (!await command.SetTransactionLogIndexAsync(ScpId, true))
            {
                errors.Add(MessageBuilder.Unsuccess(mac, Command.C208));
            }


            #endregion

            if (errors.Count > 0) return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.COMMAND_UNSUCCESS,errors);

            entity.UpdatedDate = DateTime.Now;
            entity.LastSync = DateTime.Now;
            entity.IsUpload = false;
            context.Hardwares.Update(entity);
            await context.SaveChangesAsync();

            return ResponseHelper.SuccessBuilder(true);
        }

        public void HandleUploadCommand(AeroCommand command, SCPReplyMessage message)
        {
            if (command.UploadCommandTags.Contains(message.cmnd_sts.sequence_number) && message.cmnd_sts.status == 1)
            {
                command.UploadCommandTags.Remove(message.cmnd_sts.sequence_number);
            }

            if (command.UploadCommandTags.Count == 0)
            {
                try
                {
                    var scp = context.Hardwares.FirstOrDefault(p => p.Id == message.SCPId);
                    if (scp != null)
                    {
                        scp.IsUpload = false;
                        scp.LastSync = DateTime.Now;
                        context.SaveChanges();
                        TriggerUploadMessage(ResponseMessage.UPLOAD_SUCCESS, true);
                    }

                }
                catch (Exception e)
                {
                    TriggerUploadMessage("", false);
                }

            }
        }

      
        public async Task VerifyAllocateHardwareMemoryAsync(SCPReplyMessage message)
        {
            List<MemoryAllocateDto> mems = new List<MemoryAllocateDto>();

            var hw = await context.Hardwares
                .Where(d => d.ComponentId == message.SCPId)
                .FirstOrDefaultAsync();

            if (hw is null) return;

            var config = await context.SystemSettings.AsNoTracking().FirstOrDefaultAsync();
            foreach (var i in message.str_sts.sStrSpec)
            {
                switch ((ScpStructure)i.nStrType)
                {
                    case ScpStructure.SCPSID_TRAN:
                        // Handle Transactions
                        mems.Add(new MemoryAllocateDto
                        {
                            nStrType = i.nStrType,
                            StrType = Description.ScpStructureToText(((ScpStructure)i.nStrType)),
                            nRecord = i.nRecords,
                            nRecSize = i.nRecSize,
                            nActive = i.nActive,
                            nSwAlloc = config.nTransaction,
                            nSwRecord= await context.Transactions.AsNoTracking().CountAsync(),
                            IsSync = config.nTransaction > i.nRecords,
                        });
                        break;

                    case ScpStructure.SCPSID_TZ:
                        // Handle Time zones
                        mems.Add(new MemoryAllocateDto
                        {
                            nStrType = i.nStrType,
                            StrType = Description.ScpStructureToText(((ScpStructure)i.nStrType)),
                            nRecord = i.nRecords,
                            nRecSize = i.nRecSize,
                            nActive = i.nActive,
                            nSwAlloc = config.nTz,
                            nSwRecord = await context.TimeZones.AsNoTracking().CountAsync(),
                            IsSync = config.nTz + 1 == i.nRecords,
                        });
                        break;

                    case ScpStructure.SCPSID_HOL:
                        // Handle Holidays
                        mems.Add(new MemoryAllocateDto
                        {
                            nStrType = i.nStrType,
                            StrType = Description.ScpStructureToText(((ScpStructure)i.nStrType)),
                            nRecord = i.nRecords,
                            nRecSize = i.nRecSize,
                            nActive = i.nActive,
                            nSwAlloc = config.nHol,
                            nSwRecord = await context.Holidays.AsNoTracking().CountAsync(),
                            IsSync = config.nHol == i.nRecords
                        });
                        break;

                    case ScpStructure.SCPSID_MSP1:
                        // Handle Msp1 ports (SIO drivers)
                        mems.Add(new MemoryAllocateDto
                        {
                            nStrType = i.nStrType,
                            StrType = Description.ScpStructureToText(((ScpStructure)i.nStrType)),
                            nRecord = i.nRecords,
                            nRecSize = i.nRecSize,
                            nActive = i.nActive,
                            nSwAlloc = config.nMsp1Port,
                            nSwRecord = 0,
                            IsSync = true,
                        });
                        break;

                    case ScpStructure.SCPSID_SIO:
                        // Handle SIOs
                        mems.Add(new MemoryAllocateDto
                        {
                            nStrType = i.nStrType,
                            StrType = Description.ScpStructureToText(((ScpStructure)i.nStrType)),
                            nRecord = i.nRecords,
                            nRecSize = i.nRecSize,
                            nActive = i.nActive,
                            nSwAlloc = config.nSio,
                            nSwRecord = await context.Modules.AsNoTracking().CountAsync(),
                            IsSync = config.nSio == i.nRecords,
                        });
                        break;

                    case ScpStructure.SCPSID_MP:
                        // Handle Monitor points
                        mems.Add(new MemoryAllocateDto
                        {
                            nStrType = i.nStrType,
                            StrType = Description.ScpStructureToText(((ScpStructure)i.nStrType)),
                            nRecord = i.nRecords,
                            nRecSize = i.nRecSize,
                            nActive = i.nActive,
                            nSwAlloc = config.nMp,
                            nSwRecord = await context.MonitorPoints.AsNoTracking().CountAsync(),
                            IsSync = config.nMp == i.nRecords
                        });
                        break;

                    case ScpStructure.SCPSID_CP:
                        // Handle Control points
                        mems.Add(new MemoryAllocateDto
                        {
                            nStrType = i.nStrType,
                            StrType = Description.ScpStructureToText(((ScpStructure)i.nStrType)),
                            nRecord = i.nRecords,
                            nRecSize = i.nRecSize,
                            nActive = i.nActive,
                            nSwAlloc = config.nCp,
                            nSwRecord = await context.ControlPoints.AsNoTracking().CountAsync(),
                            IsSync = config.nCp == i.nRecords
                        });
                        break;

                    case ScpStructure.SCPSID_ACR:
                        // Handle Access control readers
                        mems.Add(new MemoryAllocateDto
                        {
                            nStrType = i.nStrType,
                            StrType = Description.ScpStructureToText(((ScpStructure)i.nStrType)),
                            nRecord = i.nRecords,
                            nRecSize = i.nRecSize,
                            nActive = i.nActive,
                            nSwAlloc = config.nAcr,
                            nSwRecord = await context.Doors.AsNoTracking().CountAsync(),
                            IsSync = config.nAcr == i.nRecords
                        });
                        break;

                    case ScpStructure.SCPSID_ALVL:
                        // Handle Access levels
                        mems.Add(new MemoryAllocateDto
                        {
                            nStrType = i.nStrType,
                            StrType = Description.ScpStructureToText(((ScpStructure)i.nStrType)),
                            nRecord = i.nRecords,
                            nRecSize = i.nRecSize,
                            nActive = i.nActive,
                            nSwAlloc = config.nAlvl,
                            nSwRecord = await context.AccessLevels.AsNoTracking().CountAsync(),
                            IsSync = config.nAlvl == i.nRecords
                        });
                        break;

                    case ScpStructure.SCPSID_TRIG:
                        // Handle Triggers
                        mems.Add(new MemoryAllocateDto
                        {
                            nStrType = i.nStrType,
                            StrType = Description.ScpStructureToText(((ScpStructure)i.nStrType)),
                            nRecord = i.nRecords,
                            nRecSize = i.nRecSize,
                            nActive = i.nActive,
                            nSwAlloc = config.nTrgr,
                            nSwRecord = await context.Triggers.AsNoTracking().CountAsync(),
                            IsSync = config.nTrgr == i.nRecords
                        });
                        break;

                    case ScpStructure.SCPSID_PROC:
                        // Handle Procedures
                        mems.Add(new MemoryAllocateDto
                        {
                            nStrType = i.nStrType,
                            StrType = Description.ScpStructureToText(((ScpStructure)i.nStrType)),
                            nRecord = i.nRecords,
                            nRecSize = i.nRecSize,
                            nActive = i.nActive,
                            nSwAlloc = config.nProc,
                            nSwRecord = await context.Procedures.AsNoTracking().CountAsync(),
                            IsSync = config.nProc == i.nRecords
                        });
                        break;

                    case ScpStructure.SCPSID_MPG:
                        // Handle Monitor point groups
                        mems.Add(new MemoryAllocateDto
                        {
                            nStrType = i.nStrType,
                            StrType = Description.ScpStructureToText(((ScpStructure)i.nStrType)),
                            nRecord = i.nRecords,
                            nRecSize = i.nRecSize,
                            nActive = i.nActive,
                            nSwAlloc = config.nMpg,
                            nSwRecord = await context.MonitorGroups.AsNoTracking().CountAsync(),
                            IsSync = config.nMpg == i.nRecords
                        });
                        break;

                    case ScpStructure.SCPSID_AREA:
                        // Handle Access areas
                        mems.Add(new MemoryAllocateDto
                        {
                            nStrType = i.nStrType,
                            StrType = Description.ScpStructureToText(((ScpStructure)i.nStrType)),
                            nRecord = i.nRecords,
                            nRecSize = i.nRecSize,
                            nActive = i.nActive,
                            nSwAlloc = config.nArea,
                            nSwRecord = await context.AccessAreas.AsNoTracking().CountAsync(),
                            IsSync = config.nArea == i.nRecords
                        });
                        break;

                    case ScpStructure.SCPSID_EAL:
                        // Handle Elevator access levels
                        mems.Add(new MemoryAllocateDto
                        {
                            nStrType = i.nStrType,
                            StrType = Description.ScpStructureToText(((ScpStructure)i.nStrType)),
                            nRecord = i.nRecords,
                            nRecSize = i.nRecSize,
                            nActive = i.nActive,
                            nSwAlloc = 0,
                            nSwRecord = 0,
                            IsSync = true
                        });
                        break;

                    case ScpStructure.SCPSID_CRDB:
                        // Handle Cardholder database
                        mems.Add(new MemoryAllocateDto
                        {
                            nStrType = i.nStrType,
                            StrType = Description.ScpStructureToText(((ScpStructure)i.nStrType)),
                            nRecord = i.nRecords,
                            nRecSize = i.nRecSize,
                            nActive = i.nActive,
                            nSwAlloc = config.nCard,
                            nSwRecord = await context.Credentials.AsNoTracking().CountAsync(),
                            IsSync = config.nCard == i.nRecords
                        });
                        break;

                    case ScpStructure.SCPSID_FLASH:
                        // Handle FLASH specs
                        mems.Add(new MemoryAllocateDto
                        {
                            nStrType = i.nStrType,
                            StrType = Description.ScpStructureToText(((ScpStructure)i.nStrType)),
                            nRecord = i.nRecords,
                            nRecSize = i.nRecSize,
                            nActive = i.nActive,
                            nSwAlloc = 0,
                            nSwRecord = 0,
                            IsSync = true
                        });
                        break;

                    case ScpStructure.SCPSID_BSQN:
                        // Handle Build sequence number
                        mems.Add(new MemoryAllocateDto
                        {
                            nStrType = i.nStrType,
                            StrType = Description.ScpStructureToText(((ScpStructure)i.nStrType)),
                            nRecord = i.nRecords,
                            nRecSize = i.nRecSize,
                            nActive = i.nActive,
                            nSwAlloc = 0,
                            nSwRecord = 0,
                            IsSync = true
                        });
                        break;

                    case ScpStructure.SCPSID_SAVE_STAT:
                        // Handle Flash save status
                        mems.Add(new MemoryAllocateDto
                        {
                            nStrType = i.nStrType,
                            StrType = Description.ScpStructureToText(((ScpStructure)i.nStrType)),
                            nRecord = i.nRecords,
                            nRecSize = i.nRecSize,
                            nActive = i.nActive,
                            nSwAlloc = 0,
                            nSwRecord = 0,
                            IsSync = true
                        });
                        break;

                    case ScpStructure.SCPSID_MAB1_FREE:
                        // Handle Memory alloc block 1 free memory
                        mems.Add(new MemoryAllocateDto
                        {
                            nStrType = i.nStrType,
                            StrType = Description.ScpStructureToText(((ScpStructure)i.nStrType)),
                            nRecord = i.nRecords,
                            nRecSize = i.nRecSize,
                            nActive = i.nActive,
                            nSwAlloc = 0,
                            nSwRecord = 0,
                            IsSync=true
                        });
                        break;

                    case ScpStructure.SCPSID_MAB2_FREE:
                        // Handle Memory alloc block 2 free memory
                        mems.Add(new MemoryAllocateDto
                        {
                            nStrType = i.nStrType,
                            StrType = Description.ScpStructureToText(((ScpStructure)i.nStrType)),
                            nRecord = i.nRecords,
                            nRecSize = i.nRecSize,
                            nActive = i.nActive,
                            nSwAlloc = 0,
                            nSwRecord = 0,
                            IsSync=true
                        });
                        break;

                    case ScpStructure.SCPSID_ARQ_BUFFER:
                        // Handle Access request buffers
                        mems.Add(new MemoryAllocateDto
                        {
                            nStrType = i.nStrType,
                            StrType = Description.ScpStructureToText(((ScpStructure)i.nStrType)),
                            nRecord = i.nRecords,
                            nRecSize = i.nRecSize,
                            nActive = i.nActive,
                            nSwAlloc = 0,
                            nSwRecord = 0,
                            IsSync = true
                        });
                        break;

                    case ScpStructure.SCPSID_PART_FREE_CNT:
                        // Handle Partition memory free info
                        mems.Add(new MemoryAllocateDto
                        {
                            nStrType = i.nStrType,
                            StrType = Description.ScpStructureToText(((ScpStructure)i.nStrType)),
                            nRecord = i.nRecords,
                            nRecSize = i.nRecSize,
                            nActive = i.nActive,
                            nSwAlloc = 0,
                            nSwRecord = 0,
                            IsSync = true
                        });
                        break;

                    case ScpStructure.SCPSID_LOGIN_STANDARD:
                        // Handle Web logins - standard
                        mems.Add(new MemoryAllocateDto
                        {
                            nStrType = i.nStrType,
                            StrType = Description.ScpStructureToText(((ScpStructure)i.nStrType)),
                            nRecord = i.nRecords,
                            nRecSize = i.nRecSize,
                            nActive = i.nActive,
                            nSwAlloc = 0,
                            nSwRecord = 0,
                            IsSync = true
                        });
                        break;
                    case ScpStructure.SCPSID_FILE_SYSTEM:
                        mems.Add(new MemoryAllocateDto
                        {
                            nStrType = i.nStrType,
                            StrType = Description.ScpStructureToText(((ScpStructure)i.nStrType)),
                            nRecord = i.nRecords,
                            nRecSize = i.nRecSize,
                            nActive = i.nActive,
                            nSwAlloc = 0,
                            nSwRecord = 0,
                            IsSync = true
                        });
                        break;

                    default:
                        // Handle unknown/unsupported types
                        break;
                }
            }

            hw.IsReset = mems.Any(x => x.IsSync == false);
            hw.UpdatedDate = DateTime.Now;
            context.Hardwares.Update(hw);
            await context.SaveChangesAsync();

            // Check mismatch device configuration
            //await VerifyDeviceConfigurationAsync(hw.MacAddress,hw.LocationId);
            TriggerSyncMemoryAllocate(hw.MacAddress,mems);
        }


        private async Task<List<VerifyHardwareDeviceConfigDto>> VerifyDeviceConfigurationAsync(Hardware hw)
        {
            List<VerifyHardwareDeviceConfigDto> dev = new List<VerifyHardwareDeviceConfigDto>();

            if (hw is null) return dev;

            var hwSyn = hw.LastSync;

            // Module
            dev.Add(new VerifyHardwareDeviceConfigDto
            {
                ComponentName = "Module",
                nMismatchRecord = await context.Modules
                .AsNoTracking()
                .Where(m => m.MacAddress == hw.MacAddress && m.UpdatedDate > hwSyn)
                .CountAsync(),
                IsUpload = await context.Modules
                .AsNoTracking()
                .Where(m => m.MacAddress == hw.MacAddress)
                .AnyAsync(m => m.UpdatedDate > hwSyn)
            });

            // MP
            dev.Add(new VerifyHardwareDeviceConfigDto
            {
                ComponentName = "Monitor Point",
                nMismatchRecord = await context.MonitorPoints
                .AsNoTracking()
                .Where(m => m.MacAddress == hw.MacAddress && m.UpdatedDate > hwSyn)
                .CountAsync(),
                IsUpload = await context.MonitorPoints
                .AsNoTracking()
                .Where(m => m.MacAddress == hw.MacAddress)
                .AnyAsync(m => m.UpdatedDate > hwSyn)
            });

            // CP
            dev.Add(new VerifyHardwareDeviceConfigDto
            {
                ComponentName = "Control Point",
                nMismatchRecord = await context.ControlPoints
                .AsNoTracking()
                .Where(m => m.MacAddress == hw.MacAddress && m.UpdatedDate > hwSyn)
                .CountAsync(),
                IsUpload = await context.ControlPoints
                .AsNoTracking()
                .Where(m => m.MacAddress == hw.MacAddress)
                .AnyAsync(m => m.UpdatedDate > hwSyn)
            });

            // MPG
            dev.Add(new VerifyHardwareDeviceConfigDto
            {
                ComponentName = "Monitor Group",
                nMismatchRecord = await context.MonitorGroups
                .AsNoTracking()
                .Where(m => m.MacAddress == hw.MacAddress && m.UpdatedDate > hwSyn)
                .CountAsync(),
                IsUpload = await context.MonitorGroups
                .AsNoTracking()
                .Where(m => m.MacAddress == hw.MacAddress)
                .AnyAsync(m => m.UpdatedDate > hwSyn)
            });

            // ACR
            dev.Add(new VerifyHardwareDeviceConfigDto
            {
                ComponentName = "Access Control Reader",
                nMismatchRecord = await context.Doors
                .AsNoTracking()
                .Where(m => m.MacAddress == hw.MacAddress && m.UpdatedDate > hwSyn)
                .CountAsync(),
                IsUpload = await context.Doors
                .AsNoTracking()
                .Where(m => m.MacAddress == hw.MacAddress)
                .AnyAsync(m => m.UpdatedDate > hwSyn)
            });

            // Access Level
            dev.Add(new VerifyHardwareDeviceConfigDto
            {
                ComponentName = "Access Level",
                nMismatchRecord = await context.AccessLevels
                .AsNoTracking()
                .Where(m => m.LocationId == hw.LocationId && m.UpdatedDate > hwSyn)
                .CountAsync(),
                IsUpload = await context.AccessLevels
                .AsNoTracking()
                .Where(m => m.LocationId == hw.LocationId)
                .AnyAsync(m => m.UpdatedDate > hwSyn)
            });

            // Access Area
            dev.Add(new VerifyHardwareDeviceConfigDto
            {
                ComponentName = "Access Area",
                nMismatchRecord = await context.AccessAreas
                .AsNoTracking()
                .Where(m => m.LocationId == hw.LocationId && m.UpdatedDate > hwSyn)
                .CountAsync(),
                IsUpload = await context.AccessAreas
                .AsNoTracking()
                .Where(m => m.LocationId == hw.LocationId)
                .AnyAsync(m => m.UpdatedDate > hwSyn)
            });

            // Time Zone
            dev.Add(new VerifyHardwareDeviceConfigDto
            {
                ComponentName = "Time Zone",
                nMismatchRecord = await context.TimeZones
                .AsNoTracking()
                .Where(m => m.LocationId == hw.LocationId && m.UpdatedDate > hwSyn)
                .CountAsync(),
                IsUpload = await context.TimeZones
                .AsNoTracking()
                .Where(m => m.LocationId == hw.LocationId)
                .AnyAsync(m => m.UpdatedDate > hwSyn)
            });

            // Holiday
            dev.Add(new VerifyHardwareDeviceConfigDto
            {
                ComponentName = "Holiday",
                nMismatchRecord = await context.Holidays
                .AsNoTracking()
                .Where(m => m.LocationId == hw.LocationId && m.UpdatedDate > hwSyn)
                .CountAsync(),
                IsUpload = await context.Holidays
                .AsNoTracking()
                .Where(m => m.LocationId == hw.LocationId)
                .AnyAsync(m => m.UpdatedDate > hwSyn)
            });

            // Interval
            dev.Add(new VerifyHardwareDeviceConfigDto
            {
                ComponentName = "Interval",
                nMismatchRecord = await context.Intervals
                .AsNoTracking()
                .Where(m => m.LocationId == hw.LocationId && m.UpdatedDate > hwSyn)
                .CountAsync(),
                IsUpload = await context.Intervals
                .AsNoTracking()
                .Where(m => m.LocationId == hw.LocationId)
                .AnyAsync(m => m.UpdatedDate > hwSyn)
            });

            // Trigger
            dev.Add(new VerifyHardwareDeviceConfigDto
            {
                ComponentName = "Trigger",
                nMismatchRecord = await context.Triggers
                .AsNoTracking()
                .Where(m => m.MacAddress == hw.MacAddress && m.UpdatedDate > hwSyn)
                .CountAsync(),
                IsUpload = await context.Triggers
                .AsNoTracking()
                .Where(m => m.MacAddress == hw.MacAddress)
                .AnyAsync(m => m.UpdatedDate > hwSyn)
            });

            // Prcedure
            dev.Add(new VerifyHardwareDeviceConfigDto
            {
                ComponentName = "Prcedure",
                nMismatchRecord = await context.Procedures
                .AsNoTracking()
                .Where(m => m.MacAddress == hw.MacAddress && m.UpdatedDate > hwSyn)
                .CountAsync(),
                IsUpload = await context.Procedures
                .AsNoTracking()
                .Where(m => m.MacAddress == hw.MacAddress)
                .AnyAsync(m => m.UpdatedDate > hwSyn)
            });

            // Action
            dev.Add(new VerifyHardwareDeviceConfigDto
            {
                ComponentName = "Action",
                nMismatchRecord = await context.Actions
                .AsNoTracking()
                .Where(m => m.MacAddress == hw.MacAddress && m.UpdatedDate > hwSyn)
                .CountAsync(),
                IsUpload = await context.Actions
                .AsNoTracking()
                .Where(m => m.MacAddress == hw.MacAddress)
                .AnyAsync(m => m.UpdatedDate > hwSyn)
            });

            // CardFormat
            dev.Add(new VerifyHardwareDeviceConfigDto
            {
                ComponentName = "Card Format",
                nMismatchRecord = await context.CardFormats
                .AsNoTracking()
                .Where(m => m.UpdatedDate > hwSyn)
                .CountAsync(),
                IsUpload = await context.CardFormats
                .AsNoTracking()
                .AnyAsync(m => m.UpdatedDate > hwSyn)
            }
            );
            return dev;

        }

        public async Task<ResponseDto<IEnumerable<VerifyHardwareDeviceConfigDto>>> VerifyComponentConfigurationAsync(string mac)
        {
            var hardware = await context.Hardwares.Where(x => x.MacAddress == mac)
                .FirstOrDefaultAsync();

            if (hardware is null) return ResponseHelper.NotFoundBuilder<IEnumerable<VerifyHardwareDeviceConfigDto>>();

            var dev = await VerifyDeviceConfigurationAsync(hardware);


            hardware.UpdatedDate = DateTime.Now;
            hardware.IsUpload = dev.Any(s => s.IsUpload == true);

            context.Hardwares.Update(hardware);
            await context.SaveChangesAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<VerifyHardwareDeviceConfigDto>>(dev);
        }

        public void AssignIpToIdReport(SCPReplyMessage message, List<IdReport> iDReports)
        {
            foreach (var i in iDReports)
            {
                if (i.ScpId == message.SCPId)
                {
                    i.Port = message.web_network.cPortTnl;
                    i.Ip = UtilityHelper.IntegerToIp(message.web_network.cIpAddr);
                }
            }
            TriggerIdReport(iDReports);
        }

        // Function for handle Detect IdReport
        public async Task<IdReport> HandleFoundHardware(SCPReplyMessage message)
        {

            if (await IsHardwareRegister(UtilityHelper.ByteToHexStr(message.id.mac_addr)))
            {
                var hardware = await context.Hardwares
                    .FirstOrDefaultAsync(d => d.MacAddress == UtilityHelper.ByteToHexStr(message.id.mac_addr));

                if (!await MappingHardwareAndAllocateMemory(message.id.scp_id))
                {
                    hardware.IsReset = true;
                }
                else
                {
                    hardware.IsReset = false;
                }

                if (!await VerifyMemoryAllocateAsync(hardware.MacAddress))
                {
                    hardware.IsReset = true;
                }
                else
                {
                    hardware.IsReset = false;
                }

                var component = await VerifyDeviceConfigurationAsync(hardware);

                hardware.UpdatedDate = DateTime.Now;
                hardware.IsUpload = component.Any(s => s.IsUpload == true);

                context.Hardwares.Update(hardware);
                await context.SaveChangesAsync();

                return null;
            }
            else
            {
                if(!await VerifyHardwareConnection(message.id.scp_id)) return null;

                short id = await helperService.GetLowestUnassignedNumberNoLimitAsync<Hardware>(context);
                command.GetWebConfig(message.id.scp_id);
                IdReport iDReport = new IdReport();
                iDReport.DeviceId = message.id.device_id;
                iDReport.DeviceVer = message.id.device_ver;
                iDReport.SoftwareRevMajor = message.id.sft_rev_major;
                iDReport.SoftwareRevMinor = message.id.sft_rev_minor;
                iDReport.SerialNumber = message.id.serial_number;
                iDReport.RamSize = message.id.ram_size;
                iDReport.RamFree = message.id.ram_free;
                iDReport.ESec = UtilityHelper.UnixToDateTime(message.id.e_sec);
                iDReport.DatabaseMax = message.id.db_max;
                iDReport.DatabaseActive = message.id.db_active;
                iDReport.DipSwitchPowerUp = message.id.dip_switch_pwrup;
                iDReport.DipSwitchCurrent = message.id.dip_switch_current;
                iDReport.ScpId = command.SetScpId(message.id.scp_id, id) ? id : message.id.scp_id;
                iDReport.FirmWareAdvisory = message.id.firmware_advisory;
                iDReport.ScpIn1 = message.id.scp_in_1;
                iDReport.ScpIn2 = message.id.scp_in_2;
                iDReport.NOemCode = message.id.nOemCode;
                iDReport.ConfigFlag = message.id.config_flags;
                iDReport.MacAddress = UtilityHelper.ByteToHexStr(message.id.mac_addr);
                iDReport.TlsStatus = message.id.tls_status;
                iDReport.OperMode = message.id.oper_mode;
                iDReport.ScpIn3 = message.id.scp_in_3;
                iDReport.CumulativeBldCnt = message.id.cumulative_bld_cnt;
                iDReport.Model = Enums.Model.HIDAeroX1100.ToString();
                return iDReport;
            }
 
        }




        public async Task<ResponseDto<bool>> CreateAsync(CreateHardwareDto dto)
        {

            var hardware = MapperHelper.CreateToHardware(dto,DateTime.Now);

            if (!await VerifyMemoryAllocateAsync(hardware.MacAddress))
            {
                hardware.IsReset = true;
            }


            var component = await VerifyDeviceConfigurationAsync(hardware);

            hardware.IsUpload = component.Any(s => s.IsUpload == true);

            await context.Hardwares.AddAsync(hardware);
            await context.SaveChangesAsync();

            return ResponseHelper.SuccessBuilder(true);
        }


        public Task<ResponseDto<HardwareDto>> UpdateAsync(HardwareDto dto)
        {
            throw new NotImplementedException();
        }


        public async Task<ResponseDto<HardwareStatus>> GetStatusAsync(string mac)
        {
            var ScpId = await helperService.GetIdFromMacAsync(mac);
            if(ScpId == 0) return ResponseHelper.NotFoundBuilder<HardwareStatus>();
            if (!await context.Hardwares.AsNoTracking().AnyAsync(x => x.MacAddress == mac && x.ComponentId == ScpId)) return ResponseHelper.NotFoundBuilder<HardwareStatus>();
            short status = command.CheckSCPStatus(ScpId);
            return ResponseHelper.SuccessBuilder(new HardwareStatus()
            {
                MacAddress = mac,
                Status = status,
                ComponentId = ScpId

            });
        }

        public Task<ResponseDto<IEnumerable<ModeDto>>> GetModeAsync(int param)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseDto<HardwareDto>> GetByMacAsync(string mac)
        {
            var dto = await context.Hardwares
                .AsNoTracking()
                .Where(x => x.MacAddress == mac)
                .Select(d => MapperHelper.HardwareToHardwareDto(d))
                .FirstOrDefaultAsync();

            if (dto == null) return ResponseHelper.NotFoundBuilder<HardwareDto>();
            return ResponseHelper.SuccessBuilder(dto);
        }

        public async Task<ResponseDto<bool>> SetTransactionAsync(string mac,short IsOn)
        {
            var ScpId = helperService.GetIdFromMac(mac);
            if(await command.SetTransactionLogIndexAsync(ScpId,IsOn == 1 ? true : false))
            {
                return ResponseHelper.UnsuccessBuilder<bool>("", "");
            }
            return ResponseHelper.SuccessBuilder(true);
        }


    }
}
