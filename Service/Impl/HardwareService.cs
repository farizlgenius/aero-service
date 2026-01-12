using AeroService.DTO.Hardware;
using HID.Aero.ScpdNet.Wrapper;
using AeroService.Aero.CommandService;
using AeroService.Aero.CommandService.Impl;
using AeroService.AeroLibrary;
using AeroService.Constant;
using AeroService.Constants;
using AeroService.Data;
using AeroService.DTO;
using AeroService.DTO.AccessLevel;
using AeroService.DTO.ControlPoint;
using AeroService.DTO.Hardware;
using AeroService.DTO.IdReport;
using AeroService.DTO.Module;
using AeroService.DTO.MonitorPoint;
using AeroService.DTO.Reader;
using AeroService.DTO.RequestExit;
using AeroService.DTO.Scp;
using AeroService.DTO.Sensor;
using AeroService.DTO.Strike;
using AeroService.Entity;
using AeroService.Enums;
using AeroService.Helpers;
using AeroService.Hubs;
using AeroService.Mapper;
using AeroService.Utility;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using MiNET.Entities.Passive;
using System.ComponentModel;
using System.Net;
using static AeroService.AeroLibrary.Description;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace AeroService.Service.Impl
{
    public class HardwareService(AppDbContext context, AeroCommandService command,ITimeZoneCommandService timeZoneCommandService, IHubContext<AeroHub> hub,ITimeZoneService timeZoneService,ICardFormatService cardFormatService,IAccessLevelService accessLevelService, IHelperService<Hardware> helperService, CommandService cmndService, ICredentialService credentialService) : IHardwareService
    {

        #region CRUD 

        public async Task<ResponseDto<IEnumerable<HardwareDto>>> GetAsync()
        {
            var dtos = await context.hardware
                .AsNoTracking()
                .Select(hardware => new HardwareDto
                {
                    // Base 
                    Uuid = hardware.uuid,
                    ComponentId = hardware.component_id,
                    Mac = hardware.mac,
                    LocationId = hardware.location_id,
                    IsActive = hardware.is_active,

                    // extend_desc
                    Name = hardware.name,
                    HardwareType = hardware.hardware_type,
                    HardwareTypeDescription = hardware.hardware_type_desc,
                    Firmware = hardware.firmware,
                    Ip = hardware.ip,
                    Port = hardware.port,
                    SerialNumber = hardware.serial_number,
                    IsReset = hardware.is_reset,
                    IsUpload = hardware.is_upload,
                    Modules = hardware.modules.Select(d => new ModuleDto
                    {
                        // Base 
                        Uuid = d.uuid,
                        ComponentId = d.component_id,
                        HardwareName = hardware.name,
                        Mac = hardware.mac,
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
                        Readers = null,
                        Sensors = null,
                        Strikes = null,
                        RequestExits = null,
                        MonitorPoints = null,
                        ControlPoints = null,
                        Address = d.address,
                        Port = d.port,
                        nInput = d.n_input,
                        nOutput = d.n_output,
                        nReader = d.n_reader,
                        Msp1No = d.msp1_no,
                        BaudRate = d.baudrate,
                        nProtocol = d.n_protocol,
                        nDialect = d.n_dialect,
                    }).ToList(),
                    PortOne = hardware.port_one,
                    ProtocolOne = hardware.protocol_one,
                    ProtocolOneDescription = hardware.protocol_one_desc,
                    PortTwo = hardware.port_two,
                    ProtocolTwoDescription = hardware.protocol_two_desc,
                    ProtocolTwo = hardware.protocol_two,
                    BaudRateOne = hardware.baudrate_one,
                    BaudRateTwo = hardware.baudrate_two,

                })
                .ToArrayAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<HardwareDto>>(dtos);
        }

        public async Task<ResponseDto<IEnumerable<HardwareDto>>> GetByLocationAsync(short location)
        {
            var dtos = await context.hardware
                 .AsNoTracking()
                .Where(s => s.location_id == location)
                .Select(hardware => new HardwareDto
                {
                    // Base 
                    Uuid = hardware.uuid,
                    ComponentId = hardware.component_id,
                    Mac = hardware.mac,
                    LocationId = hardware.location_id,
                    IsActive = hardware.is_active,

                    // extend_desc
                    Name = hardware.name,
                    HardwareType = hardware.hardware_type,
                    HardwareTypeDescription = hardware.hardware_type_desc,
                    Firmware = hardware.firmware,
                    Ip = hardware.ip,
                    Port = hardware.port,
                    SerialNumber = hardware.serial_number,
                    IsReset = hardware.is_reset,
                    IsUpload = hardware.is_upload,
                    Modules = hardware.modules.Select(d => new ModuleDto
                    {
                        // Base 
                        Uuid = d.uuid,
                        ComponentId = d.component_id,
                        HardwareName = hardware.name,
                        Mac = hardware.mac,
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
                        Readers = null,
                        Sensors = null,
                        Strikes = null,
                        RequestExits = null,
                        MonitorPoints = null,
                        ControlPoints = null,
                        Address = d.address,
                        Port = d.port,
                        nInput = d.n_input,
                        nOutput = d.n_output,
                        nReader = d.n_reader,
                        Msp1No = d.msp1_no,
                        BaudRate = d.baudrate,
                        nProtocol = d.n_protocol,
                        nDialect = d.n_dialect,
                    }).ToList(),
                    PortOne = hardware.port_one,
                    ProtocolOne = hardware.protocol_one,
                    ProtocolOneDescription = hardware.protocol_one_desc,
                    PortTwo = hardware.port_two,
                    ProtocolTwoDescription = hardware.protocol_two_desc,
                    ProtocolTwo = hardware.protocol_two,
                    BaudRateOne = hardware.baudrate_one,
                    BaudRateTwo = hardware.baudrate_two,

                })
                .ToArrayAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<HardwareDto>>(dtos);
        }

        public async Task<ResponseDto<bool>> DeleteAsync(string mac)
        {
            List<string> errors = new List<string>();
            var entity = await context.hardware.FirstOrDefaultAsync(x => x.mac == mac);
            if (entity == null) return ResponseHelper.NotFoundBuilder<bool>();
            var id = await helperService.GetIdFromMacAsync(mac);
            // CP

            // MP

            // ACR

            // Access Area

            // modules Check first 
            if (await context.hardware.AsNoTracking().Include(x => x.modules).AnyAsync(x => x.modules.Where(x => x.address != -1).Any())) return ResponseHelper.FoundReferenceBuilder<bool>();
            

            if (!command.DetachScp(id))
            {
                return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(mac, Command.C015));
            }

            context.hardware.Remove(entity);
            await context.SaveChangesAsync();


            return ResponseHelper.SuccessBuilder<bool>(true);

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

        public void TriggerIdReport(List<IdReportDto> IdReportDto)
        {
            hub.Clients.All.SendAsync("IdReport", IdReportDto);
        }


        #endregion







        public async Task<ResponseDto<bool>> ResetAsync(string mac)
        {
            List<string> errors = new List<string>();
            if (!await context.hardware.AnyAsync(x => x.mac == mac)) return ResponseHelper.NotFoundBuilder<bool>();
            var id = await helperService.GetIdFromMacAsync(mac);
            if(id == 0) return ResponseHelper.NotFoundBuilder<bool>();
            if (!command.ResetSCP(id))
            {
                return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.COMMAND_UNSUCCESS,MessageBuilder.Unsuccess(mac,Command.C301));
            }
            return ResponseHelper.SuccessBuilder(true);
        }

        public async Task<ResponseDto<bool>> ResetAsync(short id)
        {
            if (!command.ResetSCP(id))
            {
                return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess("", Command.C301));
            }
            return ResponseHelper.SuccessBuilder(true);
        }

        public async Task<bool> VerifyHardwareConnection(short ScpId)
        {
            var setting = await context.system_setting
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (setting is null) return false;

            if (!command.SCPDeviceSpecification(ScpId, setting)) return false;

            return true;
        }

        public async Task<bool> MappingHardwareAndAllocateMemory(short ScpId)
        {
            var data = await context.system_setting.AsNoTracking().FirstOrDefaultAsync();
            if (data is null) return false;

            if (!command.SCPDeviceSpecification(ScpId, data)) return false;

            if (!command.AccessDatabaseSpecification(ScpId, data)) return false;

            if (!command.TimeSet(ScpId)) return false;

            return true;
        }

        public async Task<ResponseDto<bool>> VerifyMemoryAllocateAsyncWithResponse(string Mac)
        {
            var ScpId = await helperService.GetIdFromMacAsync(Mac);
            if (ScpId == 0) return ResponseHelper.NotFoundBuilder<bool>();
            if (!command.ReadStructureStatus(ScpId))
            {
                return ResponseHelper.UnsuccessBuilder<bool>(MessageBuilder.Unsuccess(Mac, Command.C1853), []); 
            }
            return ResponseHelper.SuccessBuilder<bool>(true);
        }

        public async Task<bool> VerifyMemoryAllocateAsync(string Mac)
        {
            var ScpId = await helperService.GetIdFromMacAsync(Mac);
            if (ScpId == 0) return false;
            if (!command.ReadStructureStatus(ScpId))
            {
                return false;
            }
            return true;
        }


        public async Task<ResponseDto<bool>> UploadComponentConfigurationAsync(string mac)
        {
            List<string> errors = new List<string>();
            short id = await helperService.GetIdFromMacAsync(mac);
            var entity = await context.hardware
                .Where(x => x.mac == mac)
                .OrderBy(x => x.component_id)
                .FirstOrDefaultAsync();

            if (entity is null) return ResponseHelper.NotFoundBuilder<bool>();

            var ScpId = await helperService.GetIdFromMacAsync(mac);


            #region Module Upload

            // modules
            var modules = await context.module
                .AsNoTracking()
                .Where(x => x.hardware_mac == mac)
                .ToArrayAsync();

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
                if (!command.SioDriverConfiguration(ScpId, module.msp1_no, module.port, module.baudrate, module.n_protocol))
                {
                    errors.Add(MessageBuilder.Unsuccess(mac, Command.C108));
                };

                // Enums.model.HIDAeroX1100
                if (!command.SioPanelConfiguration(ScpId, (short)module.component_id, module.model, module.n_input, module.n_output, module.n_reader, module.address, module.msp1_no, true))
                {
                    errors.Add(MessageBuilder.Unsuccess(mac, Command.C109));
                };

                //if (!command.SioPanelConfiguration(hardware_id, (short)modules.component_id, modules.model, modules.n_input, modules.n_output, modules.n_reader, modules.address, modules.msp1_no, true))
                //{
                //    errors.Add(MessageBuilder.Unsuccess(mac, command.C109));
                //};


                // Setting Input for Alarm 
                for (short i = 0; i < module.n_input; i++)
                {
                    if (i + 1 >= module.n_input - 3)
                    {
                        if (!command.InputPointSpecification(ScpId, module.component_id, i, 0, 2, 5))
                        {
                            errors.Add(MessageBuilder.Unsuccess(mac, Command.C110));
                        }
                    }

                }
            }



            #endregion

            #region Time Zone Upload

            // Timezone

            var timezones = await context.timezone
                .AsNoTracking()
                .Include(x => x.timezone_intervals)
                .ThenInclude(x => x.interval)
                .ThenInclude(x => x.days)
                .ToArrayAsync();

            var intervals = timezones
                .SelectMany(x => x.timezone_intervals.Select(x => x.interval))
                .ToList();

            foreach (var tz in timezones)
            {
                if (!await timeZoneCommandService.ExtendedTimeZoneActSpecificationAsync(ScpId, tz, intervals, !string.IsNullOrEmpty(tz.active_time) ? (int)helperService.DateTimeToElapeSecond(tz.active_time) : 0, !string.IsNullOrEmpty(tz.deactive_time) ? (int)helperService.DateTimeToElapeSecond(tz.deactive_time) : 0))
                {
                    errors.Add(MessageBuilder.Unsuccess(mac, Command.C3103));
                }
            }


            #endregion

            #region Access Level Upload

            // Access Level

            var accessLevels = await context.accesslevel
           .AsNoTracking()
           .Include(x => x.accessleve_door_timezones)
           .ToArrayAsync();

            var tzs = accessLevels.SelectMany(x => x.accessleve_door_timezones).Select(x => new CreateUpdateAccessLevelDoorTimeZoneDto
            {
                TimeZoneId = x.timezone_id,
                DoorId = x.door_id,
            }).ToList();

            foreach (var a in accessLevels)
            {
                if (a.component_id == 1 || a.component_id == 2)
                {
                    if (!command.AccessLevelConfigurationExtended(ScpId, a.component_id, a.component_id == 1 ? (short)0 : (short)1))
                    {
                        errors.Add(MessageBuilder.Unsuccess(mac, Command.C2116));
                    };
                }
                else
                {
                    if (!command.AccessLevelConfigurationExtendedCreate(ScpId, a.component_id, tzs))
                    {
                        errors.Add(MessageBuilder.Unsuccess(mac, Command.C2116));
                    }
                }

            }


            #endregion

            #region Card Format Upload

            // Card format

            var formats = await context.card_format.AsNoTracking().ToArrayAsync();
            foreach (var format in formats)
            {
                if (!command.CardFormatterConfiguration(ScpId, format.component_id, format.facility, 0, 1, 0, format.bits, format.pe_ln, format.pe_loc, format.po_ln, format.po_loc, format.fc_ln, format.fc_loc, format.ch_ln, format.ch_loc, format.ic_ln, format.ic_loc))
                {
                    errors.Add(MessageBuilder.Unsuccess(mac, Command.C1102));
                };
            }

            #endregion

            #region Control Point

            // Control Point
            var cps = await context.control_point
                .AsNoTracking()
                .Where(x => x.module.hardware_mac == mac)
                .ToArrayAsync();

            foreach (var cp in cps)
            {
                // command place here
                short modeNo = await context.output_mode
                    .AsNoTracking()
                    .Where(x => x.offline_mode == cp.offline_mode && x.relay_mode == cp.relay_mode)
                    .Select(x => x.value).FirstOrDefaultAsync();

                if (!command.OutputPointSpecification(ScpId, cp.module_id, cp.output_no, modeNo))
                {
                    errors.Add(MessageBuilder.Unsuccess(mac, Command.C111));
                }


                if (!command.ControlPointConfiguration(ScpId, cp.module_id, cp.component_id, cp.output_no, cp.default_pulse))
                {
                    errors.Add(MessageBuilder.Unsuccess(mac, Command.C114));

                }

            }


            #endregion

            #region Monitor Point

            // Monitor Points
            var mps = await context.monitor_point
                .AsNoTracking()
                .Where(x => x.module.hardware_mac == mac)
                .ToArrayAsync();

            foreach (var mp in mps)
            {
                // command place here
                if (!command.InputPointSpecification(ScpId, mp.module_id, mp.input_no, mp.input_mode, mp.debounce, mp.holdtime))
                {
                    errors.Add(MessageBuilder.Unsuccess(mac, Command.C110));
                }


                if (!command.MonitorPointConfiguration(ScpId, mp.module_id, mp.input_no, mp.log_function, mp.monitor_point_mode, mp.delay_entry, mp.delay_exit, mp.component_id))
                {
                    errors.Add(MessageBuilder.Unsuccess(mac, Command.C113));
                }

            }


            #endregion

            #region Monitor Group

            // Monitor Group
            var mpgs = await context.monitor_group
                .AsNoTracking()
                .Include(x => x.n_mp_list)
                .Where(x => x.hardware_mac == mac)
                .ToArrayAsync();

            foreach (var mpg in mpgs)
            {
                if (!command.ConfigureMonitorPointGroup(ScpId, mpg.component_id, mpg.n_mp_count, mpg.n_mp_list.ToList()))
                {
                    errors.Add(MessageBuilder.Unsuccess(mac, Command.C120));
                }
            }


            #endregion

            #region Doors

            // door
            var doors = await context.door
                .Include(x => x.readers)
                .Include(x => x.request_exits)
                .Include(x => x.strike)
                .Include(x => x.sensor)
                .Where(x => x.hardware_mac == mac)
                .ToArrayAsync();

            foreach (var door in doors)
            {
                // command place here

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
                        errors.Add(MessageBuilder.Unsuccess(mac,Command.C112));
                    }
                }



                // Strike Strike Config
                var StrikeId = await helperService.GetIdFromMacAsync(door.strike.module.hardware_mac);
                if (!command.OutputPointSpecification(StrikeId, door.strike.module_id, door.strike.output_no, door.strike.relay_mode))
                {
                    errors.Add(MessageBuilder.Unsuccess(mac, Command.C111));
                };

                // door sensor Config
                var SensorId = await helperService.GetIdFromMacAsync(door.sensor.module.hardware_mac);
                if (!command.InputPointSpecification(SensorId, door.sensor.module_id, door.sensor.input_no, door.sensor.input_mode, door.sensor.debounce, door.sensor.holdtime))
                {
                    errors.Add(MessageBuilder.Unsuccess(mac, Command.C110));
                }

                foreach (var rex in door.request_exits)
                {
                    if (string.IsNullOrEmpty(rex.module.hardware_mac)) continue;
                    var Rex0Id = await helperService.GetIdFromMacAsync(rex.module.hardware_mac);
                    var rexComponentId = await helperService.GetLowestUnassignedNumberNoLimitAsync<RequestExit>(context);
                    rex.component_id = rexComponentId;
                    if (!command.InputPointSpecification(Rex0Id, rex.module_id, rex.input_no, rex.input_mode, rex.debounce, rex.holdtime))
                    {
                        errors.Add(MessageBuilder.Unsuccess(mac, Command.C110));
                    }
                }

                if (!command.AccessControlReaderConfiguration(ScpId, door.component_id, door))
                {
                    errors.Add(MessageBuilder.Unsuccess(mac,Command.C115));
                }

            }

            #endregion

            #region Card Holder

            var cards = await context.cardholder
                .AsNoTracking()
                .Include(x => x.credentials)
                .Include(x => x.access_levels)
                .ThenInclude(x => x.access_level)
                .ToArrayAsync();

            foreach(var card in cards)
            {
                var ScpIds = await context.hardware.Select(x => new { x.component_id, x.mac }).ToArrayAsync();
                foreach (var cred in card.credentials)
                {
                    foreach (var i in ScpIds)
                    {
                        if (!command.AccessDatabaseCardRecord(i.component_id, card.flag, cred.card_no, cred.issue_code, cred.pin, card.access_levels.Select(x => x.access_level).ToList(), (int)helperService.DateTimeToElapeSecond(cred.active_date), (int)helperService.DateTimeToElapeSecond(cred.deactive_date)))
                        {
                            errors.Add(MessageBuilder.Unsuccess(i.mac, Command.C8304));
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

            if (!command.SetTransactionLogIndex(ScpId, true))
            {
                errors.Add(MessageBuilder.Unsuccess(mac, Command.C208));
            }


            #endregion

            if (errors.Count > 0) return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.COMMAND_UNSUCCESS,errors);

            entity.updated_date = DateTime.UtcNow;
            entity.last_sync = DateTime.UtcNow;
            entity.is_upload = false;
            context.hardware.Update(entity);
            await context.SaveChangesAsync();

            return ResponseHelper.SuccessBuilder(true);
        }


      
        public async Task VerifyAllocateHardwareMemoryAsync(SCPReplyMessage message)
        {
            List<MemoryAllocateDto> mems = new List<MemoryAllocateDto>();

            var hw = await context.hardware
                .Where(d => d.component_id == message.SCPId)
                .FirstOrDefaultAsync();

            if (hw is null) return;

            var config = await context.system_setting.AsNoTracking().FirstOrDefaultAsync();

            if(config is null) return;

            foreach (var i in message.str_sts.sStrSpec)
            {
                switch ((ScpStructure)i.nStrType)
                {
                    case ScpStructure.SCPSID_TRAN:
                        // Handle transaction
                        mems.Add(new MemoryAllocateDto
                        {
                            nStrType = i.nStrType,
                            StrType = Description.ScpStructureToText(((ScpStructure)i.nStrType)),
                            nRecord = i.nRecords,
                            nRecSize = i.nRecSize,
                            nActive = i.nActive,
                            nSwAlloc = config.n_transaction,
                            nSwRecord= await context.transaction.AsNoTracking().CountAsync(),
                            IsSync = config.n_transaction > i.nRecords,
                        });
                        break;

                    case ScpStructure.SCPSID_TZ:
                        // Handle time zones
                        mems.Add(new MemoryAllocateDto
                        {
                            nStrType = i.nStrType,
                            StrType = Description.ScpStructureToText(((ScpStructure)i.nStrType)),
                            nRecord = i.nRecords,
                            nRecSize = i.nRecSize,
                            nActive = i.nActive,
                            nSwAlloc = config.n_tz,
                            nSwRecord = await context.timezone.AsNoTracking().CountAsync(),
                            IsSync = config.n_tz + 1 == i.nRecords,
                        });
                        break;

                    case ScpStructure.SCPSID_HOL:
                        // Handle holiday
                        mems.Add(new MemoryAllocateDto
                        {
                            nStrType = i.nStrType,
                            StrType = Description.ScpStructureToText(((ScpStructure)i.nStrType)),
                            nRecord = i.nRecords,
                            nRecSize = i.nRecSize,
                            nActive = i.nActive,
                            nSwAlloc = config.n_hol,
                            nSwRecord = await context.holiday.AsNoTracking().CountAsync(),
                            IsSync = config.n_hol == i.nRecords
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
                            nSwAlloc = config.m_msp1_port,
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
                            nSwAlloc = config.n_sio,
                            nSwRecord = await context.module.AsNoTracking().CountAsync(),
                            IsSync = config.n_sio == i.nRecords,
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
                            nSwAlloc = config.n_mp,
                            nSwRecord = await context.monitor_point.AsNoTracking().CountAsync(),
                            IsSync = config.n_mp == i.nRecords
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
                            nSwAlloc = config.n_cp,
                            nSwRecord = await context.control_point.AsNoTracking().CountAsync(),
                            IsSync = config.n_cp == i.nRecords
                        });
                        break;

                    case ScpStructure.SCPSID_ACR:
                        // Handle Access control reader
                        mems.Add(new MemoryAllocateDto
                        {
                            nStrType = i.nStrType,
                            StrType = Description.ScpStructureToText(((ScpStructure)i.nStrType)),
                            nRecord = i.nRecords,
                            nRecSize = i.nRecSize,
                            nActive = i.nActive,
                            nSwAlloc = config.n_acr,
                            nSwRecord = await context.door.AsNoTracking().CountAsync(),
                            IsSync = config.n_acr == i.nRecords
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
                            nSwAlloc = config.n_alvl,
                            nSwRecord = await context.accesslevel.AsNoTracking().CountAsync(),
                            IsSync = config.n_alvl == i.nRecords
                        });
                        break;

                    case ScpStructure.SCPSID_TRIG:
                        // Handle trigger
                        mems.Add(new MemoryAllocateDto
                        {
                            nStrType = i.nStrType,
                            StrType = Description.ScpStructureToText(((ScpStructure)i.nStrType)),
                            nRecord = i.nRecords,
                            nRecSize = i.nRecSize,
                            nActive = i.nActive,
                            nSwAlloc = config.n_trgr,
                            nSwRecord = await context.trigger.AsNoTracking().CountAsync(),
                            IsSync = config.n_trgr == i.nRecords
                        });
                        break;

                    case ScpStructure.SCPSID_PROC:
                        // Handle procedure
                        mems.Add(new MemoryAllocateDto
                        {
                            nStrType = i.nStrType,
                            StrType = Description.ScpStructureToText(((ScpStructure)i.nStrType)),
                            nRecord = i.nRecords,
                            nRecSize = i.nRecSize,
                            nActive = i.nActive,
                            nSwAlloc = config.n_proc,
                            nSwRecord = await context.procedure.AsNoTracking().CountAsync(),
                            IsSync = config.n_proc == i.nRecords
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
                            nSwAlloc = config.n_mpg,
                            nSwRecord = await context.monitor_group.AsNoTracking().CountAsync(),
                            IsSync = config.n_mpg == i.nRecords
                        });
                        break;

                    case ScpStructure.SCPSID_AREA:
                        // Handle Access area
                        mems.Add(new MemoryAllocateDto
                        {
                            nStrType = i.nStrType,
                            StrType = Description.ScpStructureToText(((ScpStructure)i.nStrType)),
                            nRecord = i.nRecords,
                            nRecSize = i.nRecSize,
                            nActive = i.nActive,
                            nSwAlloc = config.n_area,
                            nSwRecord = await context.area.AsNoTracking().CountAsync(),
                            IsSync = true
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
                            nSwAlloc = config.n_card,
                            nSwRecord = await context.credential.AsNoTracking().CountAsync(),
                            IsSync = config.n_card == i.nRecords
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

            hw.is_reset = mems.Any(x => x.IsSync == false);
            hw.updated_date = DateTime.UtcNow;
            context.hardware.Update(hw);
            await context.SaveChangesAsync();

            // Check mismatch device configuration
            //await VerifyDeviceConfigurationAsync(hw.mac,hw.location_id);
            TriggerSyncMemoryAllocate(hw.mac,mems);
        }


        private async Task<List<VerifyHardwareDeviceConfigDto>> VerifyDeviceConfigurationAsync(Hardware hw)
        {
            List<VerifyHardwareDeviceConfigDto> dev = new List<VerifyHardwareDeviceConfigDto>();

            if (hw is null) return dev;

            var hwSyn = hw.last_sync;

            // modules
            dev.Add(new VerifyHardwareDeviceConfigDto
            {
                ComponentName = "modules",
                nMismatchRecord = await context.module
                .AsNoTracking()
                .Where(m => m.hardware_mac == hw.mac && m.updated_date > hwSyn)
                .CountAsync(),
                IsUpload = await context.module
                .AsNoTracking()
                .Where(m => m.hardware_mac == hw.mac)
                .AnyAsync(m => m.updated_date > hwSyn)
            });

            // MP
            dev.Add(new VerifyHardwareDeviceConfigDto
            {
                ComponentName = "Monitor Point",
                nMismatchRecord = await context.monitor_point
                .AsNoTracking()
                .Where(m => m.module.hardware_mac == hw.mac && m.updated_date > hwSyn)
                .CountAsync(),
                IsUpload = await context.monitor_point
                .AsNoTracking()
                .Where(m => m.module.hardware_mac == hw.mac)
                .AnyAsync(m => m.updated_date > hwSyn)
            });

            // CP
            dev.Add(new VerifyHardwareDeviceConfigDto
            {
                ComponentName = "Control Point",
                nMismatchRecord = await context.control_point
                .AsNoTracking()
                .Where(m => m.module.hardware_mac == hw.mac && m.updated_date > hwSyn)
                .CountAsync(),
                IsUpload = await context.control_point
                .AsNoTracking()
                .Where(m => m.module.hardware_mac == hw.mac)
                .AnyAsync(m => m.updated_date > hwSyn)
            });

            // MPG
            dev.Add(new VerifyHardwareDeviceConfigDto
            {
                ComponentName = "Monitor Group",
                nMismatchRecord = await context.monitor_group
                .AsNoTracking()
                .Where(m => m.hardware_mac == hw.mac && m.updated_date > hwSyn)
                .CountAsync(),
                IsUpload = await context.monitor_group
                .AsNoTracking()
                .Where(m => m.hardware_mac == hw.mac)
                .AnyAsync(m => m.updated_date > hwSyn)
            });

            // ACR
            dev.Add(new VerifyHardwareDeviceConfigDto
            {
                ComponentName = "Access Control Reader",
                nMismatchRecord = await context.door
                .AsNoTracking()
                .Where(m => m.hardware_mac == hw.mac && m.updated_date > hwSyn)
                .CountAsync(),
                IsUpload = await context.door
                .AsNoTracking()
                .Where(m => m.hardware_mac == hw.mac)
                .AnyAsync(m => m.updated_date > hwSyn)
            });

            // Access Level
            dev.Add(new VerifyHardwareDeviceConfigDto
            {
                ComponentName = "Access Level",
                nMismatchRecord = await context.accesslevel
                .AsNoTracking()
                .Where(m => m.location_id == hw.location_id && m.updated_date > hwSyn)
                .CountAsync(),
                IsUpload = await context.accesslevel
                .AsNoTracking()
                .Where(m => m.location_id == hw.location_id)
                .AnyAsync(m => m.updated_date > hwSyn)
            });

            // Access Area
            dev.Add(new VerifyHardwareDeviceConfigDto
            {
                ComponentName = "Access Area",
                nMismatchRecord = await context.area
                .AsNoTracking()
                .Where(m => m.location_id == hw.location_id && m.updated_date > hwSyn)
                .CountAsync(),
                IsUpload = await context.area
                .AsNoTracking()
                .Where(m => m.location_id == hw.location_id)
                .AnyAsync(m => m.updated_date > hwSyn)
            });

            // time Zone
            dev.Add(new VerifyHardwareDeviceConfigDto
            {
                ComponentName = "time Zone",
                nMismatchRecord = await context.timezone
                .AsNoTracking()
                .Where(m => m.location_id == hw.location_id && m.updated_date > hwSyn)
                .CountAsync(),
                IsUpload = await context.timezone
                .AsNoTracking()
                .Where(m => m.location_id == hw.location_id)
                .AnyAsync(m => m.updated_date > hwSyn)
            });

            // Holiday
            dev.Add(new VerifyHardwareDeviceConfigDto
            {
                ComponentName = "Holiday",
                nMismatchRecord = await context.holiday
                .AsNoTracking()
                .Where(m => m.location_id == hw.location_id && m.updated_date > hwSyn)
                .CountAsync(),
                IsUpload = await context.holiday
                .AsNoTracking()
                .Where(m => m.location_id == hw.location_id)
                .AnyAsync(m => m.updated_date > hwSyn)
            });

            // interval
            dev.Add(new VerifyHardwareDeviceConfigDto
            {
                ComponentName = "interval",
                nMismatchRecord = await context.interval
                .AsNoTracking()
                .Where(m => m.location_id == hw.location_id && m.updated_date > hwSyn)
                .CountAsync(),
                IsUpload = await context.interval
                .AsNoTracking()
                .Where(m => m.location_id == hw.location_id)
                .AnyAsync(m => m.updated_date > hwSyn)
            });

            // trigger
            dev.Add(new VerifyHardwareDeviceConfigDto
            {
                ComponentName = "trigger",
                nMismatchRecord = await context.trigger
                .AsNoTracking()
                .Where(m => m.hardware_mac == hw.mac && m.updated_date > hwSyn)
                .CountAsync(),
                IsUpload = await context.trigger
                .AsNoTracking()
                .Where(m => m.hardware_mac == hw.mac)
                .AnyAsync(m => m.updated_date > hwSyn)
            });

            // Prcedure
            dev.Add(new VerifyHardwareDeviceConfigDto
            {
                ComponentName = "Prcedure",
                nMismatchRecord = await context.procedure
                .AsNoTracking()
                .Where(m => m.trigger.hardware_mac == hw.mac && m.updated_date > hwSyn)
                .CountAsync(),
                IsUpload = await context.procedure
                .AsNoTracking()
                .Where(m => m.trigger.hardware_mac == hw.mac)
                .AnyAsync(m => m.updated_date > hwSyn)
            });

            // action
            dev.Add(new VerifyHardwareDeviceConfigDto
            {
                ComponentName = "action",
                nMismatchRecord = await context.action
                .AsNoTracking()
                .Where(m => m.procedure.trigger.hardware_mac == hw.mac && m.updated_date > hwSyn)
                .CountAsync(),
                IsUpload = await context.action
                .AsNoTracking()
                .Where(m => m.procedure.trigger.hardware_mac == hw.mac)
                .AnyAsync(m => m.updated_date > hwSyn)
            });

            // card_format
            dev.Add(new VerifyHardwareDeviceConfigDto
            {
                ComponentName = "Card Format",
                nMismatchRecord = await context.card_format
                .AsNoTracking()
                .Where(m => m.updated_date > hwSyn)
                .CountAsync(),
                IsUpload = await context.card_format
                .AsNoTracking()
                .AnyAsync(m => m.updated_date > hwSyn)
            }
            );
            return dev;

        }

        public async Task<ResponseDto<IEnumerable<VerifyHardwareDeviceConfigDto>>> VerifyComponentConfigurationAsync(string mac)
        {
            var hardware = await context.hardware.Where(x => x.mac == mac)
                .FirstOrDefaultAsync();

            if (hardware is null) return ResponseHelper.NotFoundBuilder<IEnumerable<VerifyHardwareDeviceConfigDto>>();

            var dev = await VerifyDeviceConfigurationAsync(hardware);


            hardware.updated_date = DateTime.UtcNow;
            hardware.is_upload = dev.Any(s => s.IsUpload == true);

            context.hardware.Update(hardware);
            await context.SaveChangesAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<VerifyHardwareDeviceConfigDto>>(dev);
        }

        public async Task AssignIpAddress(SCPReplyMessage message)
        {
            if (!string.IsNullOrEmpty(await helperService.GetMacFromIdAsync((short)message.SCPId)))
            {
                var hw = context.hardware
                    .Where(x => x.component_id == message.SCPId)
                    .OrderBy(x => x.component_id)
                    .FirstOrDefault();

                if (hw is null) return;


                if (message.web_network is not null) hw.ip = UtilityHelper.IntegerToIp(message.web_network.cIpAddr);

                context.hardware.Update(hw);
                await context.SaveChangesAsync();

                command.GetWebConfigRead((short)message.SCPId, 3);

            }
            else
            {
                var report = await context.id_report
                    .Where(x => x.scp_id == message.SCPId)
                    .OrderBy(x => x.id)
                    .FirstOrDefaultAsync();

                if (report is null) return;

                if (message.web_network is not null) report.ip = UtilityHelper.IntegerToIp(message.web_network.cIpAddr);

                context.id_report.Update(report);
                await context.SaveChangesAsync();

                command.GetWebConfigRead((short)message.SCPId, 3);
            }


        }

        public async Task AssignPort(SCPReplyMessage message)
        {
            if(!string.IsNullOrEmpty(await helperService.GetMacFromIdAsync((short)message.SCPId)))
            {
                var hw = context.hardware
                    .Where(x => x.component_id == message.SCPId)
                    .OrderBy(x => x.component_id)
                    .FirstOrDefault();

                if (hw is null) return;

                if (message.web_host_comm_prim is not null)
                {
                    if(message.web_host_comm_prim.ipclient is not null)
                    {
                        hw.port = message.web_host_comm_prim.ipclient.nPort.ToString();
                    }
                    else if(message.web_host_comm_prim.ipserver is not null)
                    {
                        hw.port = message.web_host_comm_prim.ipserver.nPort.ToString();
                    }
                };

                context.hardware.Update(hw);
                await context.SaveChangesAsync();

                var dto = await context.id_report
                    .AsNoTracking()
                    .Select(x => MapperHelper.IdReportToDto(x))
                    .ToListAsync();

                TriggerIdReport(dto);

            }
            else
            {
                var report = await context.id_report
                    .Where(x => x.scp_id == message.SCPId)
                    .OrderBy(x => x.id)
                    .FirstOrDefaultAsync();

                if (report is null) return;

                if (message.web_host_comm_prim is not null)
                {
                    if (message.web_host_comm_prim.ipclient is not null)
                    {
                        report.port = message.web_host_comm_prim.ipclient.nPort.ToString();
                    }
                    else if (message.web_host_comm_prim.ipserver is not null)
                    {
                        report.port = message.web_host_comm_prim.ipserver.nPort.ToString();
                    }
                };

                context.id_report.Update(report);
                await context.SaveChangesAsync();

                var dto = await context.id_report
                    .AsNoTracking()
                    .Select(x => MapperHelper.IdReportToDto(x))
                    .ToListAsync();

                TriggerIdReport(dto);
            }

            
        }

        // Function for handle Detect IdReport
        public async Task HandleFoundHardware(SCPReplyMessage message)
        {
            if (await context.hardware.AnyAsync(x => x.mac.Equals(UtilityHelper.ByteToHexStr(message.id.mac_addr))))
            {
                var hardware = await context.hardware
                    .FirstOrDefaultAsync(d => d.mac.Equals(UtilityHelper.ByteToHexStr(message.id.mac_addr)));

                if (hardware is null) return;

                if (!await MappingHardwareAndAllocateMemory(message.id.scp_id))
                {
                    hardware.is_reset = true;
                }
                else
                {
                    hardware.is_reset = false;
                }

                if (!await VerifyMemoryAllocateAsync(hardware.mac))
                {
                    hardware.is_reset = true;
                }
                else
                {
                    hardware.is_reset = false;
                }

                hardware.firmware = UtilityHelper.ParseFirmware(message.id.sft_rev_major, message.id.sft_rev_minor);

                var component = await VerifyDeviceConfigurationAsync(hardware);

                hardware.updated_date = DateTime.UtcNow;
                hardware.is_upload = component.Any(s => s.IsUpload == true);

                context.hardware.Update(hardware);
                await context.SaveChangesAsync();

                // Call Get ip
                command.GetWebConfigRead(message.id.scp_id, 2);


            }
            else
            {
                if (!await VerifyHardwareConnection(message.id.scp_id)) return;

                

                if(await context.id_report.AnyAsync(x => x.scp_id == message.id.scp_id && x.mac.Equals(UtilityHelper.ByteToHexStr(message.id.mac_addr))))
                {
                    var iDReport = await context.id_report
                        .Where(x => x.scp_id == message.id.scp_id && x.mac.Equals(UtilityHelper.ByteToHexStr(message.id.mac_addr)))
                        .OrderBy(x => x.id)
                        .FirstOrDefaultAsync();

                    if(iDReport is null) return;
                    iDReport.device_id = message.id.device_id;
                    iDReport.device_ver = message.id.device_ver;
                    iDReport.software_rev_major = message.id.sft_rev_major;
                    iDReport.software_rev_minor = message.id.sft_rev_minor;
                    iDReport.firmware = UtilityHelper.ParseFirmware(message.id.sft_rev_major, message.id.sft_rev_minor);
                    iDReport.serial_number = message.id.serial_number;
                    iDReport.ram_size = message.id.ram_size;
                    iDReport.ram_free = message.id.ram_free;
                    iDReport.e_sec = UtilityHelper.UnixToDateTime(message.id.e_sec);
                    iDReport.db_max = message.id.db_max;
                    iDReport.db_active = message.id.db_active;
                    iDReport.dip_switch_powerup = message.id.dip_switch_pwrup;
                    iDReport.dip_switch_current = message.id.dip_switch_current;
                    //iDReport.hardware_id = command.SetScpId(message.id.scp_id, id) ? id : message.id.scp_id;
                    iDReport.firmware_advisory = message.id.firmware_advisory;
                    iDReport.scp_in1 = message.id.scp_in_1;
                    iDReport.scp_in2 = message.id.scp_in_2;
                    iDReport.n_oem_code = message.id.nOemCode;
                    iDReport.config_flag = message.id.config_flags;
                    //iDReport.mac = UtilityHelper.ByteToHexStr(message.id.mac_addr);
                    iDReport.tls_status = message.id.tls_status;
                    iDReport.oper_mode = message.id.oper_mode;
                    iDReport.scp_in3 = message.id.scp_in_3;
                    iDReport.cumulative_bld_cnt = message.id.cumulative_bld_cnt;
                    iDReport.port = "";
                    iDReport.ip = "";
                    context.id_report.Update(iDReport);
                }
                else
                {
                    short id = await helperService.GetLowestUnassignedNumberNoLimitAsync<Hardware>(context);
                    IdReport iDReport = new IdReport();
                    iDReport.device_id = message.id.device_id;
                    iDReport.device_ver = message.id.device_ver;
                    iDReport.software_rev_major = message.id.sft_rev_major;
                    iDReport.software_rev_minor = message.id.sft_rev_minor;
                    iDReport.firmware = UtilityHelper.ParseFirmware(message.id.sft_rev_major, message.id.sft_rev_minor);
                    iDReport.serial_number = message.id.serial_number;
                    iDReport.ram_size = message.id.ram_size;
                    iDReport.ram_free = message.id.ram_free;
                    iDReport.e_sec = UtilityHelper.UnixToDateTime(message.id.e_sec);
                    iDReport.db_max = message.id.db_max;
                    iDReport.db_active = message.id.db_active;
                    iDReport.dip_switch_powerup = message.id.dip_switch_pwrup;
                    iDReport.dip_switch_current = message.id.dip_switch_current;
                    iDReport.scp_id = command.SetScpId(message.id.scp_id, id) ? id : message.id.scp_id;
                    iDReport.firmware_advisory = message.id.firmware_advisory;
                    iDReport.scp_in1 = message.id.scp_in_1;
                    iDReport.scp_in2 = message.id.scp_in_2;
                    iDReport.n_oem_code = message.id.nOemCode;
                    iDReport.config_flag = message.id.config_flags;
                    iDReport.mac = UtilityHelper.ByteToHexStr(message.id.mac_addr);
                    iDReport.tls_status = message.id.tls_status;
                    iDReport.oper_mode = message.id.oper_mode;
                    iDReport.scp_in3 = message.id.scp_in_3;
                    iDReport.cumulative_bld_cnt = message.id.cumulative_bld_cnt;
                    iDReport.port = "";
                    iDReport.ip = "";
                    await context.id_report.AddAsync(iDReport);
                }
                
                
                await context.SaveChangesAsync();


                command.GetWebConfigRead(message.id.scp_id, 2);


            }
 
        }




        public async Task<ResponseDto<bool>> CreateAsync(CreateHardwareDto dto)
        {
            var hardware = MapperHelper.CreateToHardware(dto,DateTime.UtcNow);

            if (!await VerifyMemoryAllocateAsync(hardware.mac))
            {
                hardware.is_reset = true;
            }


            var component = await VerifyDeviceConfigurationAsync(hardware);

            hardware.is_upload = component.Any(s => s.IsUpload == true);

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
                if (!command.SioDriverConfiguration(dto.ComponentId, 1, 1, dto.BaudRateOne, dto.ProtocolOne))
                {
                    return ResponseHelper.UnsuccessBuilder<bool>(MessageBuilder.Unsuccess(dto.Mac, Command.C108), []);
                };

                // Send command for address 0-15 on port 1 if allow
                for(int i = 0;i < 16; i++)
                {
                    // model = -1 for allow every model
                    // n_input = 19 Maximum
                    // n_output = 12 Maximum
                    // n_reader = 4 Maximum
                    if (!command.SioPanelConfiguration(dto.ComponentId,(short)i,-1,19,12,4,(short)i,1,true))
                    {
                        return ResponseHelper.UnsuccessBuilder<bool>(MessageBuilder.Unsuccess(dto.Mac, Command.C109), []);
                    }
                }
            }

            if (dto.PortTwo)
            {
                if (!command.SioDriverConfiguration(dto.ComponentId, 2, 2, dto.BaudRateTwo, dto.ProtocolTwo))
                {
                    return ResponseHelper.UnsuccessBuilder<bool>(MessageBuilder.Unsuccess(dto.Mac, Command.C108), []);
                };

                // Send command for address 16-31 on port 2 if allow
                for (int i = 15; i < 31; i++)
                {
                    // model = -1 for allow every model
                    if (!command.SioPanelConfiguration(dto.ComponentId, (short)i, -1, 19, 12, 4, (short)i, 1, true))
                    {
                        return ResponseHelper.UnsuccessBuilder<bool>(MessageBuilder.Unsuccess(dto.Mac, Command.C109), []);
                    }
                }
            }

            var report = await context.id_report
               .Where(x => x.mac == dto.Mac && x.scp_id == dto.ComponentId)
               .OrderBy(x => x.id)
               .FirstOrDefaultAsync();

            if (report is null) return ResponseHelper.NotFoundBuilder<bool>();

            await context.hardware.AddAsync(hardware);
            await context.SaveChangesAsync();

           

            context.id_report.Remove(report);
            await context.SaveChangesAsync();

            return ResponseHelper.SuccessBuilder(true);
        }


        public async Task<ResponseDto<HardwareDto>> UpdateAsync(HardwareDto dto)
        {
            var en = await context.hardware
                .Where(x => x.mac == dto.Mac && x.component_id == dto.ComponentId)
                .OrderBy(x => x.id)
                .FirstOrDefaultAsync();

            if (en is null) return ResponseHelper.NotFoundBuilder<HardwareDto>();

            MapperHelper.UpdateHardware(en, dto);

            context.hardware.Update(en);
            await context.SaveChangesAsync();

            return ResponseHelper.SuccessBuilder<HardwareDto>(dto);
        }


        public async Task<ResponseDto<HardwareStatus>> GetStatusAsync(string mac)
        {
            var ScpId = await helperService.GetIdFromMacAsync(mac);
            if(ScpId == 0) return ResponseHelper.NotFoundBuilder<HardwareStatus>();
            if (!await context.hardware.AsNoTracking().AnyAsync(x => x.mac == mac && x.component_id == ScpId)) return ResponseHelper.NotFoundBuilder<HardwareStatus>();
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
            var dto = await context.hardware
                .AsNoTracking()
                .Where(x => x.mac == mac)
               .Select(hardware => new HardwareDto
               {
                   // Base 
                   Uuid = hardware.uuid,
                   ComponentId = hardware.component_id,
                   Mac = hardware.mac,
                   LocationId = hardware.location_id,
                   IsActive = hardware.is_active,

                   // extend_desc
                   Name = hardware.name,
                   HardwareType = hardware.hardware_type,
                   HardwareTypeDescription = hardware.hardware_type_desc,
                   Firmware = hardware.firmware,
                   Ip = hardware.ip,
                   Port = hardware.port,
                   SerialNumber = hardware.serial_number,
                   IsReset = hardware.is_reset,
                   IsUpload = hardware.is_upload,
                   Modules = hardware.modules.Select(d => new ModuleDto
                   {
                       // Base 
                       Uuid = d.uuid,
                       ComponentId = d.component_id,
                       Mac = hardware.mac,
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
                       Readers = null,
                       Sensors = null,
                       Strikes = null,
                       RequestExits = null,
                       MonitorPoints = null,
                       ControlPoints = null,
                       Address = d.address,
                       Port = d.port,
                       nInput = d.n_input,
                       nOutput = d.n_output,
                       nReader = d.n_reader,
                       Msp1No = d.msp1_no,
                       BaudRate = d.baudrate,
                       nProtocol = d.n_protocol,
                       nDialect = d.n_dialect,
                   }).ToList(),
                   PortOne = hardware.port_one,
                   ProtocolOne = hardware.protocol_one,
                   ProtocolOneDescription = hardware.protocol_one_desc,
                   PortTwo = hardware.port_two,
                   ProtocolTwoDescription = hardware.protocol_two_desc,
                   ProtocolTwo = hardware.protocol_two,
                   BaudRateOne = hardware.baudrate_one,
                   BaudRateTwo = hardware.baudrate_two,

               })
                .FirstOrDefaultAsync();

            if (dto == null) return ResponseHelper.NotFoundBuilder<HardwareDto>();
            return ResponseHelper.SuccessBuilder(dto);
        }

        public async Task<ResponseDto<bool>> SetTransactionAsync(string mac,short IsOn)
        {
            var ScpId = await helperService.GetIdFromMacAsync(mac);
            if (ScpId == 0) return ResponseHelper.NotFoundBuilder<bool>();
            if (!command.SetTransactionLogIndex(ScpId,IsOn == 1 ? true : false))
            {
                return ResponseHelper.UnsuccessBuilder<bool>(MessageBuilder.Unsuccess(mac,Command.C303),[]);
            }
            return ResponseHelper.SuccessBuilder(true);
        }

        public async Task<ResponseDto<bool>> GetTransactionLogStatusAsync(string mac)
        {
            var id = await helperService.GetIdFromMacAsync(mac);
            if (id == 0) return ResponseHelper.NotFoundBuilder<bool>();

            if(!command.GetTransactionLogStatus(id))
            {
                return ResponseHelper.UnsuccessBuilder<bool>(MessageBuilder.Unsuccess(mac,Command.C402),[]);
            }

            return ResponseHelper.SuccessBuilder(true);
        }

        public async void TriggerTranStatus(SCPReplyMessage message)
        {
            TranStatusDto tran = new TranStatusDto
            {
                MacAddress = await helperService.GetMacFromIdAsync((short)message.SCPId),
                Capacity = message.tran_sts.capacity,
                Oldest = message.tran_sts.oldest,
                LastLog = message.tran_sts.last_loggd,
                LastReport = message.tran_sts.last_rprtd,
                Disabled = message.tran_sts.disabled,
                Status = message.tran_sts.disabled == 0 ? "Enable" : "Disable"

            };
            await hub.Clients.All.SendAsync("TranStatus",tran);
        }

        public async Task<ResponseDto<IEnumerable<ModeDto>>> GetHardwareTypeAsync()
        {
            var dtos = await context.hardware_type
                .AsNoTracking()
                .Select(x => new ModeDto 
                {
                    Name = x.name,
                    Value = x.component_id,
                    Description = x.description
                })
                .ToArrayAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dtos);
        }

        public async Task<IEnumerable<IdReportDto>> RemoveIdReportAsync(SCPReplyMessage message)
        {
            var report = await context.id_report
                .Where(x => x.scp_id == message.SCPId)
                .OrderBy(x => x.id)
                .FirstOrDefaultAsync();

            if(report is null) return await context.id_report
                    .AsNoTracking()
                    .Select(x => MapperHelper.IdReportToDto(x))
                    .ToArrayAsync();

            context.id_report.Remove(report);
            await context.SaveChangesAsync();

            return await context.id_report
                    .AsNoTracking()
                    .Select(x => MapperHelper.IdReportToDto(x))
                    .ToArrayAsync();
        }

        public async Task<ResponseDto<IEnumerable<ResponseDto<bool>>>> SetRangeTransactionAsync(List<SetTranDto> dtos)
        {
            bool flag = true;
            List<ResponseDto<bool>> data = new List<ResponseDto<bool>>();
            foreach (var dto in dtos)
            {
                var re = await SetTransactionAsync(dto.MacAddress,dto.Param);
                if (re.code != HttpStatusCode.OK) flag = false;
                data.Add(re);
            }

            if (!flag) return ResponseHelper.UnsuccessBuilder<IEnumerable<ResponseDto<bool>>>(data);

            var res = ResponseHelper.SuccessBuilder<IEnumerable<ResponseDto<bool>>>(data);

            return res;
        }
    }
}
