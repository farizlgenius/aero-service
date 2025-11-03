using AutoMapper;
using HID.Aero.ScpdNet.Wrapper;
using HIDAeroService.AeroLibrary;
using HIDAeroService.Constant;
using HIDAeroService.Constants;
using HIDAeroService.Data;
using HIDAeroService.DTO;
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
using static HIDAeroService.AeroLibrary.Description;


namespace HIDAeroService.Service.Impl
{
    public class HardwareService(AppDbContext context, AeroCommand command, IHubContext<AeroHub> hub,ITimeZoneService timeZoneService,ICardFormatService cardFormatService,IAccessLevelService accessLevelService, IHelperService<Hardware> helperService, CmndService cmndService, ICredentialService credentialService, IMapper mapper) : IHardwareService
    {

        public virtual async Task<ResponseDto<IEnumerable<HardwareDto>>> GetAsync()
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
                return ResponseHelper.UnsuccessBuilder<HardwareDto>(ResponseMessage.COMMAND_UNSUCCESS,MessageBuilder.Unsuccess(mac,Command.C015));
            }

            context.Hardwares.Remove(entity);
            await context.SaveChangesAsync();

            var dto = mapper.Map<HardwareDto>(entity);

            return ResponseHelper.SuccessBuilder<HardwareDto>(dto);

        }



        public async Task<bool> IsHardwareRegister(string mac)
        {
            return await context.Hardwares.AnyAsync(s => s.MacAddress == mac);
        }


        public void TriggerDeviceStatus(string ScpMac, int CommStatus)
        {
            //GetOnlineStatus()
            var result = hub.Clients.All.SendAsync("CommStatus", ScpMac, CommStatus);
        }

        public void TriggerSyncStatus()
        {
            //GetOnlineStatus()
            var result = hub.Clients.All.SendAsync("SyncStatus");
        }

        public void TriggerUploadMessage(string message, bool isFinish = false)
        {
            //GetOnlineStatus()
            var result = hub.Clients.All.SendAsync("UploadStatus", message, isFinish);
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


        public async Task<bool> ReadStructureStatus(short id)
        {
            return await command.ReadStructureStatusAsync(id);
        }


        public async Task<bool> MappingHardware(short ScpId)
        {
            var data = await context.SystemSettings.AsNoTracking().FirstOrDefaultAsync();
            if (data is null) return false;

            if (!command.SCPDeviceSpecification(ScpId, data)) return false;

            return true;
        }


        public async Task<bool> HardwareConfigurationAsync(Hardware hardware,ITimeZoneService tzService, ICardFormatService cfmtService, IAccessLevelService alvlService)
        {

            /*
                Command 108: Driver Configuration
                Command 109: SIO Panel Configuration
                Command 110: Sensor Point Specification
                Command 111: Strike Point Specification
                Command 112: Reader Specification
                Command 113: Monitor Point Configuration
                Command 114: Control Point Configuration
                Command 115: Access Control Reader Configuration
                Note: Issue Command 109: SIO Panel Configuration twice, the first time as specified above with the enable
                parameter “off.” Issue it again after Command 115: Access Control Reader Configuration with the enable
                parameter “on” to avoid extraneous change-of-state transactions.
                Next configure access levels, time zones, and holidays using the following commands:
                Command 2116: Access Level Configuration Extended
                Command 1104: Holiday Configuration
                At this point, issue the configuration commands required for your application.
                Note: Sample configuration files are supplied with the initial demo kit. If you would like sample configuration files,
                please contact technical support.
             
             */

            //short SioNo = 0;
            //short model = 196;
            //short address = 0;
            //short msp1_number = 0;
            //short internal_port_number = 3;
            //short internal_baud_rate = -1;
            //short protocol = 0;
            //short n_dialect = 0;

            var systemSettings = context.SystemSettings.AsNoTracking().First();
            if (systemSettings is null) return false;

            if (!await command.AccessDatabaseSpecificationAsync(hardware.ComponentId, systemSettings)) return false;

            if (!await command.TimeSetAsync(hardware.ComponentId)) return false;

            foreach(var module in hardware.Module)
            {
                if (!await command.SioDriverConfigurationAsync(hardware.ComponentId, module.Msp1No, module.Port, module.BaudRate, module.nProtocol)) return false;
                if (!await command.SioPanelConfigurationAsync(hardware.ComponentId, (short)module.ComponentId,Enum.Model.HIDAeroX1100,module.nInput,module.nOutput,module.nReader, module.Address, module.Msp1No, true)) return false;
            }



            var formats = await context.CardFormats.ToArrayAsync();
            foreach (var format in formats)
            {
                if (!await command.CardFormatterConfigurationAsync(hardware.ComponentId, format.ComponentId, format.Facility, 0, 1, 0, format.Bits, format.PeLn, format.PeLoc, format.PoLn, format.PoLoc, format.FcLn, format.FcLoc, format.ChLn, format.ChLoc, format.IcLn, format.IcLoc)) return false;
            }


            //var timezones = await context.ArTimeZones.ToArrayAsync();
            //foreach (var tz in timezones)
            //{
            //    if (!command.ExtendedTimeZoneActSpecification(ScpId, tz.TzNo, tz.Mode, (int)helperService.DateTimeToElapeSecond(tz.ActTime), (int)helperService.DateTimeToElapeSecond(tz.DeactTime), tz.Intervals, []))
            //    {
            //        _logger.LogError(Constants.Constant.EXTEND_TIME_ZONE_SPECIFICATION_UNSUCCESS);
            //        //return AppContants.EXTEND_TIME_ZONE_SPECIFICATION_UNSUCCESS;
            //    
            //}


            // Create Access Level All
            //IEnumerable<AccessLevel> acls = alvlService.GetAllSetting();
            //foreach (var a in acls)
            //{
            //    if (!command.AccessLevelConfigurationExtended(ScpId, a.ComponentNo,a))
            //    {
            //        _logger.LogError(Constants.ResponseMessage.ACCESS_LEVEL_CONFIGURATION_UNSUCCESS);
            //        //return AppContants.ACCESS_LEVEL_CONFIGURATION_UNSUCCESS;
            //    }
            //}

            foreach(var module in hardware.Module)
            {
                for (short i = 0; i < module.nInput; i++)
                {
                    if (i + 1 >= module.nInput - 3)
                    {
                        if (!await command.InputPointSpecificationAsync(hardware.ComponentId, module.ComponentId, i, 0, 2, 5))
                        {
                            return false;
                        }
                    }

                }

            }



            if (!await command.SetTransactionLogIndexAsync(hardware.ComponentId, true))
            {
                return false;
            }

            return true;
        }



        public async Task<bool> VerifySystemConfigurationAsync(short Id)
        {
            List<string> errors = new List<string>();
            if (!await command.ReadStructureStatusAsync(Id))
            {
                return false;
            }
            return true;
        }


        public async Task<ResponseDto<bool>> UploadConfigAsync(string mac)
        {
            List<string> errors = new List<string>();
            short id = await helperService.GetIdFromMacAsync(mac);
            var entity = await context.Hardwares.AsNoTracking().FirstOrDefaultAsync(x => x.MacAddress == mac);
            if (entity == null) return ResponseHelper.NotFoundBuilder<bool>();


           // // CP
           // //TriggerUploadMessage(ResponseMessage.UPLOAD_CONTROL_POINT);
           //var cps = await context.Strikes.AsNoTracking().Where(p => p.CreatedDate > entity.LastSync && p.MacAddress == mac).ToListAsync();
           // foreach (var cp in cps)
           // {
           //     if(!await command.OutputPointSpecificationAsync(id, cp.SioNo, cp.OpNo, cp.ModeNo))
           //     {
           //         errors.Add(MessageBuilder.Unsuccess(mac, Command.C111));
           //     }

           //     if(!await command.ControlPointConfigurationAsync(id, cp.SioNo, cp.ComponentNo, cp.OpNo, cp.DefaultPulseTime))
           //     {
           //         errors.Add(MessageBuilder.Unsuccess(mac, Command.C114));
           //     }

           // }

           // // MP
           // var mps = await context.Sensors.AsNoTracking().Where(p => p.CreatedDate > entity.LastSync && p.MacAddress == mac).ToListAsync();
           // foreach (var mp in mps)
           // {

           // }

            if (errors.Count > 0) return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.COMMAND_UNSUCCESS,errors);

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

        public void UpdateScpStatus(VerifyScpConfigDto rec)
        {
            context.Hardwares.ToList().ForEach(record =>
            {
                if (rec.Mac == record.MacAddress)
                {
                    record.IsReset = rec.IsReset;
                    record.IsUpload = rec.IsUpload;
                }
            }
            );
            context.SaveChanges();
        }

      

        public async Task VerifyAllocateHardwareMemoryAsync(SCPReplyMessage message)
        {
            VerifyScpConfigDto verifyScpConfigDto = new VerifyScpConfigDto();
            string mac = await context.Hardwares.AsNoTracking().Where(d => d.ComponentId == message.SCPId).Select(d => d.MacAddress).FirstOrDefaultAsync() ?? "";
            var config = await context.SystemSettings.AsNoTracking().FirstOrDefaultAsync();
            VerifyScpConfigDto rec = new VerifyScpConfigDto();
            rec.Mac = mac;
            foreach (var i in message.str_sts.sStrSpec)
            {
                switch ((ScpStructure)i.nStrType)
                {
                    case ScpStructure.SCPSID_TRAN:
                        // Handle Transactions
                        //rec.RecAllocTransaction = i.nRecords < config.nTransaction ? 1 : i.nRecords > config.nTransaction ? -1 : 0;
                        //rec.IsReset = rec.RecAllocTransaction == 0 ? false : true;
                        break;

                    case ScpStructure.SCPSID_TZ:
                        // Handle Time zones
                        rec.RecAllocTimezone = i.nActive - 1 < config.nTz ? 1 : i.nActive - 1 > config.nTz ? -1 : 0;
                        break;

                    case ScpStructure.SCPSID_HOL:
                        // Handle Holidays
                        rec.RecAllocHoliday = i.nActive < config.nHol ? 1 : i.nActive > config.nHol ? -1 : 0;
                        break;

                    case ScpStructure.SCPSID_MSP1:
                        // Handle Msp1 ports (SIO drivers)
                        //rec.RecAllocMsp1 = i.nActive < config.nMsp1Port ? 1 : i.nActive > config.nMsp1Port ? -1 : 0;
                        break;

                    case ScpStructure.SCPSID_SIO:
                        // Handle SIOs
                        rec.RecAllocSio = i.nActive < config.nSio ? 1 : i.nActive > config.nSio ? -1 : 0;
                        break;

                    case ScpStructure.SCPSID_MP:
                        // Handle Monitor points
                        rec.RecAllocMp = i.nActive < config.nMp ? 1 : i.nActive > config.nMp ? -1 : 0;
                        break;

                    case ScpStructure.SCPSID_CP:
                        rec.RecAllocCp = i.nActive < config.nCp ? 1 : i.nActive > config.nCp ? -1 : 0;
                        // Handle Control points
                        break;

                    case ScpStructure.SCPSID_ACR:
                        // Handle Access control readers
                        rec.RecAllocAcr = i.nActive < config.nAcr ? 1 : i.nActive > config.nAcr ? -1 : 0;
                        break;

                    case ScpStructure.SCPSID_ALVL:
                        // Handle Access levels
                        rec.RecAllocAlvl = i.nActive < config.nAlvl ? 1 : i.nActive > config.nAlvl ? -1 : 0;
                        break;

                    case ScpStructure.SCPSID_TRIG:
                        // Handle Triggers
                        rec.RecAllocTrig = i.nActive < config.nTrgr ? 1 : i.nActive > config.nTrgr ? -1 : 0;
                        break;

                    case ScpStructure.SCPSID_PROC:
                        // Handle Procedures
                        rec.RecAllocProc = i.nActive < config.nProc ? 1 : i.nActive > config.nProc ? -1 : 0;
                        break;

                    case ScpStructure.SCPSID_MPG:
                        // Handle Monitor point groups
                        rec.RecAllocMpg = i.nActive < config.nMpg ? 1 : i.nActive > config.nMpg ? -1 : 0;
                        break;

                    case ScpStructure.SCPSID_AREA:
                        // Handle Access areas
                        //rec.RecAllocArea = i.nActive < config.n_area ? 1 : i.nActive > config.n_area ? -1 : 0;
                        break;

                    case ScpStructure.SCPSID_EAL:
                        // Handle Elevator access levels
                        break;

                    case ScpStructure.SCPSID_CRDB:
                        // Handle Cardholder database
                        //int ncard = credentialService.GetCredentialRecAlloc();
                        rec.RecAllocCrdb = i.nRecords < config.nCard ? 1 : i.nRecords > config.nCard ? -1 : 0;
                        //int ncardac = _credentialService.GetActiveCredentialRecAlloc();
                        //rec.RecAllocCardActive = i.nActive < ncardac ? 1 : i.nActive > ncardac ? -1 : 0;
                        break;

                    case ScpStructure.SCPSID_FLASH:
                        // Handle FLASH specs
                        break;

                    case ScpStructure.SCPSID_BSQN:
                        // Handle Build sequence number
                        break;

                    case ScpStructure.SCPSID_SAVE_STAT:
                        // Handle Flash save status
                        break;

                    case ScpStructure.SCPSID_MAB1_FREE:
                        // Handle Memory alloc block 1 free memory
                        break;

                    case ScpStructure.SCPSID_MAB2_FREE:
                        // Handle Memory alloc block 2 free memory
                        break;

                    case ScpStructure.SCPSID_ARQ_BUFFER:
                        // Handle Access request buffers
                        break;

                    case ScpStructure.SCPSID_PART_FREE_CNT:
                        // Handle Partition memory free info
                        break;

                    case ScpStructure.SCPSID_LOGIN_STANDARD:
                        // Handle Web logins - standard
                        break;
                    case ScpStructure.SCPSID_FILE_SYSTEM:
                        break;

                    default:
                        // Handle unknown/unsupported types
                        break;
                }
            }
            bool hasMisMatch = rec.GetType()
                .GetProperties()
                .Where(p => p.PropertyType == typeof(int)) // only int properties
                .Select(p => (int)p.GetValue(rec))
                .Any(value => value == -1 || value == 1);
            rec.IsReset = hasMisMatch;
            // Check mismatch device configuration
            rec.IsUpload = await VerifyDeviceConfigurationAsync(mac);
            UpdateScpStatus(rec);
            cmndService.TriggerVerifyScpConfiguration(rec);
            TriggerSyncStatus();
        }


        private async Task<bool> VerifyDeviceConfigurationAsync(string mac)
        {
            bool result = true;
            var scp = await context.Hardwares
                .AsNoTracking().Where(s => s.MacAddress == mac)
                .Include(m => m.Module)
                .ThenInclude(i => i.Sensors)
                .Include(m => m.Module)
                .ThenInclude(o => o.Strikes)
                .Include(m => m.Module)
                .ThenInclude(r => r.Readers)
                .FirstOrDefaultAsync();

            if (scp is null) return false;


            // Check if any control points created after last sync
            
            foreach(var module in scp.Module)
            {
                result = module.Sensors.Any(d => d.CreatedDate > scp.LastSync) || module.Strikes.Any(d => d.CreatedDate > scp.LastSync) || module.Readers.Any(d => d.CreatedDate > scp.LastSync);
            }

            return result;

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
        public async Task<IdReport> HandleFoundHardware(SCPReplyMessage message, ITimeZoneService tz, ICardFormatService cfmt, IAccessLevelService alvl)
        {
            if (await MappingHardware(message.id.scp_id))
            {

                if (await IsHardwareRegister(UtilityHelper.ByteToHexStr(message.id.mac_addr)))
                {
                    var hardware = await context.Hardwares
                        .Include(s => s.Module)
                        .FirstOrDefaultAsync(d => d.MacAddress == UtilityHelper.ByteToHexStr(message.id.mac_addr));
                    await HardwareConfigurationAsync(hardware, tz, cfmt, alvl);
                    await ReadStructureStatus(message.id.scp_id);
                    return null;
                }
                else
                {
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
                    iDReport.ScpId = command.SetScpId(message.id.scp_id,id) ? id : message.id.scp_id;
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
                    iDReport.Model = Enum.Model.HIDAeroX1100.ToString();
                    return iDReport;
                }
            }
            return null;
        }

        public void TriggerIdReport(List<IdReport> IdReports)
        {
            hub.Clients.All.SendAsync("IdReport", IdReports);
        }


        public async Task<ResponseDto<bool>> CreateAsync(CreateHardwareDto dto)
        {

            var hardware = MapperHelper.CreateToHardware(dto);

            await context.Hardwares.AddAsync(hardware);
            await context.SaveChangesAsync();
            if (!await HardwareConfigurationAsync(hardware,timeZoneService,cardFormatService, accessLevelService))
            {
                hardware.IsReset = true;
            }

            if (!await VerifySystemConfigurationAsync((short)hardware.ComponentId))
            {
                hardware.IsReset = true;
            }

            if (!await VerifyDeviceConfigurationAsync(hardware.MacAddress))
            {
                hardware.IsUpload = true;
            }

            context.Hardwares.Update(hardware);
            await context.SaveChangesAsync();

            return ResponseHelper.SuccessBuilder(true);
        }


        public Task<ResponseDto<HardwareDto>> UpdateAsync(HardwareDto dto)
        {
            throw new NotImplementedException();
        }


        public async Task<ResponseDto<HardwareStatus>> GetStatusAsync(string mac,short id)
        {
            if (!await context.Hardwares.AsNoTracking().AnyAsync(x => x.MacAddress == mac && x.ComponentId == id)) return ResponseHelper.NotFoundBuilder<HardwareStatus>();
            short status = command.CheckSCPStatus(id);
            return ResponseHelper.SuccessBuilder(new HardwareStatus()
            {
                MacAddress = mac,
                Status = status,
                ComponentId = id

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
