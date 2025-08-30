using HID.Aero.ScpdNet.Wrapper;
using HIDAeroService.AeroLibrary;
using HIDAeroService.Constants;
using HIDAeroService.Data;
using HIDAeroService.Dto.Cp;
using HIDAeroService.Dto.Scp;
using HIDAeroService.Entity;
using HIDAeroService.Hubs;
using HIDAeroService.Mapper;
using HIDAeroService.Models;
using HIDAeroService.Service.Impl;
using HIDAeroService.Service.Interface;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto.Macs;
using static HIDAeroService.AeroLibrary.Description;

namespace HIDAeroService.Service
{
    public class ScpService 
    {
        private readonly HelperService _helperService;
        private readonly SysService _sysService;
        private readonly AppDbContext _context;
        private readonly AeroLibMiddleware _config;
        private readonly MpService _mpService;
        private readonly AcrService _acrService;
        private readonly CmndService _cmndService;
        private readonly CredentialService _credentialService;
        private readonly IHubContext<ScpHub> _hub;
        private readonly ILogger<ScpService> _logger;
        public ScpService(AppDbContext appDbContext,AeroLibMiddleware config,IHubContext<ScpHub> hub,MpService mpService,AcrService acrService,ILogger<ScpService> logger,HelperService helperService,SysService sysService,CmndService cmndService,CredentialService credentialService)
        {
            _cmndService = cmndService;
            _credentialService = credentialService;
            _sysService = sysService;
            _helperService = helperService;
            _logger = logger;
            _acrService = acrService;
            _mpService = mpService;
            _hub = hub;
            _context = appDbContext;
            _config = config;
        }

        public IDReportDto GetIDReport(short id)
        {
            if (_config.write.GetIDReport(id))
            {
                return MapperHelper.IDReportToIDReportDto(_config.read.iDReport);
            }
            return null;
        }

        public ScpStatusDto GetOnlineStatus(string ScpMac)
        {
            short ScpId = _helperService.GetScpIdFromMac(ScpMac);
            ScpStatusDto dto = new ScpStatusDto();
            dto.ScpMac = ScpMac;
            dto.ScpId = ScpId;
            dto.Status = _config.write.CheckSCPStatus(ScpId);
            return dto;
        }

        public int IDReportListCount()
        {
            _logger.LogInformation("Get IdReport List Count");
            if (_config.read.iDReports.Count != 0) 
            {
                int count = _config.read.iDReports.Count;
                _logger.LogInformation("IdReport List Count : {0}",count);
                return count;
            }
            return 0;
            
        }

        public async Task<List<IDReportDto>> IDReportList()
        {
            List<IDReportDto> data = new List<IDReportDto>();
            foreach (IDReport i in _config.read.iDReports)
            {
                if (!IsRegister(i.MacAddress))
                {
                    data.Add(MapperHelper.IDReportToIDReportDto(i));
                }

            }
            return data;
        }

        public bool RegisterSCP(ScpRegisDto scpDto)
        {
            try
            {
                ArScp scp = MapperHelper.ScpRegisDtoToSCP(scpDto);
                _context.ArScps.Add(scp);
                return _context.SaveChanges() > 0;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }


        }

        public List<ScpDto> GetScpList() 
        {
            List<ScpDto> scpDtos = new List<ScpDto>();
            var scps = _context.ArScps.ToList();
            if(scps.Count != 0)
            {
                int i = 1;
                foreach (ArScp scp in scps)
                {
                    scpDtos.Add(MapperHelper.SCPToScpDto(i,scp,0));
                    i++;
                }
                return scpDtos;
            }
            return new List<ScpDto>();

        }

        public bool IsRegister(string mac) 
        {
            return _context.ArScps.Any(s => s.Mac == mac);
        }

        public bool IsIpRegister(string ipaddress)
        {
            return _context.ArScps.Any(s => s.Ip == ipaddress);
        }



        public void TriggerDeviceStatus(string ScpMac,int CommStatus)
        {
            //GetOnlineStatus()
            var result = _hub.Clients.All.SendAsync("CommStatus", ScpMac, CommStatus);
        }

        public void TriggerSyncStatus()
        {
            //GetOnlineStatus()
            var result = _hub.Clients.All.SendAsync("SyncStatus");
        }

        public void TriggerUploadMessage(string message,bool isFinish = false)
        {
            //GetOnlineStatus()
            var result = _hub.Clients.All.SendAsync("UploadStatus", message,isFinish);
        }


        public bool ResetScp(ResetScpDto dto)
        {
            short ScpId = _helperService.GetScpIdFromMac(dto.ScpMac);

            if (!_config.write.ResetSCP(ScpId))
            {
                _logger.LogError("ScpReset : False");
                Console.WriteLine("ScpReset : False");
                return false;
            }
            return true;
        }

        public bool ReadMemoryStorage(short id)
        {
            return _config.write.ReadMemoryStorage(id);
        }

        public bool ReadStructureStatus(short id)
        {
            return _config.write.ReadStructureStatus(id);
        }

        public bool GetWenConfig(short id)
        {
            return _config.write.GetWebConfig(id);
        }

        #region Command Group

        public string ValidateScp(short ScpId)
        {
            var data = _context.ArScpSettings.AsNoTracking().First();
            if (data == null)
            {
                _logger.LogError(Constants.ConstantsHelper.NO_SYSTEM_CONFIG_IN_DB);
                return Constants.ConstantsHelper.NO_SYSTEM_CONFIG_IN_DB;
            }

            if (!_config.write.SCPDeviceSpecification(ScpId, data))
            {
                _logger.LogError(Constants.ConstantsHelper.SCP_DEVICE_SPECIFICATION_UNSUCCESS);
                return Constants.ConstantsHelper.SCP_DEVICE_SPECIFICATION_UNSUCCESS;
            }

            return Constants.ConstantsHelper.COMMAND_SUCCESS;
        }

        public string ScpConfiguration(short ScpId, TimeZoneService tzService, ICardFormatService cfmtService, IAccessLevelService alvlService)
        {

            /*
                Command 108: Driver Configuration
                Command 109: SIO Panel Configuration
                Command 110: Input Point Specification
                Command 111: Output Point Specification
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

            short SioNo = 0;
            short model = 196;
            short address = 0;
            short msp1_number = 0;
            short internal_port_number = 3;
            short internal_baud_rate = -1;
            short protocol = 0;
            short n_dialect = 0;

            var data = _context.ArScpSettings.AsNoTracking().First();
            if(data == null)
            {
                _logger.LogError(Constants.ConstantsHelper.NO_SYSTEM_CONFIG_IN_DB);
                return Constants.ConstantsHelper.NO_SYSTEM_CONFIG_IN_DB;
            }

            if (!_config.write.AccessDatabaseSpecification(ScpId,data))
            {
                _logger.LogError(Constants.ConstantsHelper.ACCESS_DATABASE_SPECIFICATION_UNSUCCESS);
                return Constants.ConstantsHelper.ACCESS_DATABASE_SPECIFICATION_UNSUCCESS;
            }

            if (!_config.write.TimeSet(ScpId))
            {
                _logger.LogError(Constants.ConstantsHelper.TIME_SET_UNSUCCESS);
                return Constants.ConstantsHelper.TIME_SET_UNSUCCESS;
            }

            if (!_config.write.SioDriverConfiguration(ScpId, msp1_number, internal_port_number, internal_baud_rate, protocol))
            {
                _logger.LogError(Constants.ConstantsHelper.SIO_DRIVER_CONFIGURATION_UNSUCCESS);
                return Constants.ConstantsHelper.SIO_DRIVER_CONFIGURATION_UNSUCCESS;
            }

            if (!_config.write.SioPanelConfiguration(ScpId, SioNo, model, address, msp1_number, true))
            {
                _logger.LogError(Constants.ConstantsHelper.SIO_PANEL_CONFIGURATION_UNSUCCESS);
                return Constants.ConstantsHelper.SIO_PANEL_CONFIGURATION_UNSUCCESS;
            }


            IEnumerable<ArCardFormat> formats = cfmtService.GetAllSetting();
            foreach (var format in formats)
            {
                if (!_config.write.CardFormatterConfiguration(ScpId, format.ComponentNo, format.Facility, format.Offset, format.FunctionId, format.Flags, format.Bits, format.PeLn, format.PeLoc, format.PoLn, format.PoLoc, format.FcLn, format.FcLoc, format.ChLn, format.ChLoc, format.IcLn, format.IcLoc))
                {
                    _logger.LogError(Constants.ConstantsHelper.CARD_FORMATTER_COMMAND_UNSUCCESS);
                }
            }


            //List<ArTimeZone> timezones = tzService.GetTimeZoneList();
            //foreach (var tz in timezones)
            //{
            //    if (!_config.write.ExtendedTimeZoneActSpecification(ScpId, tz.TzNo, tz.Mode, (int)_helperService.DateTimeToElapeSecond(tz.ActTime), (int)_helperService.DateTimeToElapeSecond(tz.DeactTime), tz.Intervals, []))
            //    {
            //        _logger.LogError(Constants.Constant.EXTEND_TIME_ZONE_SPECIFICATION_UNSUCCESS);
            //        //return AppContants.EXTEND_TIME_ZONE_SPECIFICATION_UNSUCCESS;
            //    }
            //}


            // Create Access Level All
            IEnumerable<ArAccessLevel> acls = alvlService.GetAllSetting();
            foreach (var a in acls)
            {
                if (!_config.write.AccessLevelConfigurationExtended(ScpId, a.ComponentNo,a.TzAcr1))
                {
                    _logger.LogError(Constants.ConstantsHelper.ACCESS_LEVEL_CONFIGURATION_UNSUCCESS);
                    //return AppContants.ACCESS_LEVEL_CONFIGURATION_UNSUCCESS;
                }
            }

            var component = _context.ArScpComponents.Where(p => p.ModelNo == model).First();
            if (component == null)
            {
                _logger.LogError(Constants.ConstantsHelper.NO_SCP_COMPONENT_IN_DB);
                return Constants.ConstantsHelper.NO_SCP_COMPONENT_IN_DB;
            }

            for(short i = 0; i < component.NInput; i++)
            {
                if(i + 1 >= component.NInput - 3)
                {
                    if (!_config.write.InputPointSpecification(ScpId, SioNo, i, 0, 2, 5))
                    {
                        _logger.LogError(Constants.ConstantsHelper.INPUT_SPECIFICATION_UNSUCCESS);
                        return Constants.ConstantsHelper.INPUT_SPECIFICATION_UNSUCCESS;
                    }
                }

            }

            if (!_config.write.SetTransactionLogIndex(ScpId,true))
            {
                return Constants.ConstantsHelper.COMMAND_UNSUCCESS;
            }

            return Constants.ConstantsHelper.COMMAND_SUCCESS;
        }



        public string RemoveScp(RemoveScpDto dto)
        {
            short ScpId = _helperService.GetScpIdFromMac(dto.ScpMac);
            try
            {
                //if (!_config.write.DeleteScp(ScpId))
                //{
                //    return AppContants.DELETE_SCP_FAIL;
                //}

                if (!_config.write.DetachScp(ScpId))
                {
                    return Constants.ConstantsHelper.DETACH_SCP_FAIL;
                }

                // Delete Scp
                var scp = _context.ArScps.First(p => p.Mac == dto.ScpMac);
                if(scp != null)
                {
                    _context.ArScps.Remove(scp);
                }
                else
                {
                    return Constants.ConstantsHelper.NOT_FOUND_RECORD;
                }

                // Delete Sio
                var sios = _context.ArSios.Where(p => p.ScpMac == dto.ScpMac).ToList();
                if (sios.Count > 0)
                {
                    _context.ArSios.RemoveRange(sios);
                }

                // Delete Cp
                //var cps = _context.ArControlPoints.Where(p => p.ScpIp)
                _context.SaveChanges();

                return Constants.ConstantsHelper.REMOVE_SUCCESS;
            }
            catch (Exception ex) 
            {
                return ex.Message;
            }
        }

        public string VerifyScpConfiguration(VerifyScpDto dto)
        {
            short ScpId = _helperService.GetScpIdFromMac(dto.ScpMac);
            if (!_config.write.ReadStructureStatus(ScpId))
            {
                return Constants.ConstantsHelper.COMMAND_UNSUCCESS;
            }
            return Constants.ConstantsHelper.COMMAND_SUCCESS;
        }

        public string UploadScpConfig(UploadScpConfigDto dto)
        {
            TriggerUploadMessage(Constants.ConstantsHelper.UPLOAD_COMPARE_SCP_DATA);
            short ScpId = _helperService.GetScpIdFromIp(dto.ScpMac);
            var scp = _context.ArScps.AsNoTracking().FirstOrDefault(p => p.Mac == dto.ScpMac);
            if(scp == null)
            {
                return Constants.ConstantsHelper.NOT_FOUND_RECORD;
            }

            // CP
            TriggerUploadMessage(Constants.ConstantsHelper.UPLOAD_CONTROL_POINT);
            List<ArControlPoint> cps = _context.ArControlPoints.Where(p => p.CreatedDate > scp.LastSync).ToList();
            if(cps.Count > 0)
            {
                foreach (var cp in cps)
                {
                    var status = _config.write.OutputPointSpecification(ScpId, cp.SioNo, cp.OpNo, cp.Mode);
                    if (status == -1)
                    {
                        return Constants.ConstantsHelper.COMMAND_UNSUCCESS; 
                    }
                    _config.write.UploadCommandTags.Add(status);
                    _config.write.UploadMessage = Constants.ConstantsHelper.UPLOAD_CONTROL_POINT;

                    status = _config.write.ControlPointConfiguration(ScpId, cp.SioNo, cp.CpNo, cp.OpNo, cp.DefaultPulseTime);
                    if (status == -1)
                    {
                        return Constants.ConstantsHelper.COMMAND_UNSUCCESS;
                    }
                    _config.write.UploadCommandTags.Add(status);
                }
            }
            else
            {
                TriggerUploadMessage(Constants.ConstantsHelper.UPLOAD_SUCCESS, true);
                _config.write.UploadCommandTags.Clear();
            }


            return Constants.ConstantsHelper.COMMAND_SUCCESS;
        }
        public void HandleUploadCommand(WriteAeroDriver write, SCPReplyMessage message)
        {
            if (write.UploadCommandTags.Contains(message.cmnd_sts.sequence_number) && message.cmnd_sts.status == 1)
            {
                write.UploadCommandTags.Remove(message.cmnd_sts.sequence_number);
            }

            if (write.UploadCommandTags.Count == 0)
            {
                try
                {
                    var scp = _context.ArScps.FirstOrDefault(p => p.ScpId == message.SCPId);
                    if(scp != null)
                    {
                        scp.IsUpload = false;
                        scp.LastSync = DateTime.Now;
                        _context.SaveChanges();
                        TriggerUploadMessage(Constants.ConstantsHelper.UPLOAD_SUCCESS, true);
                    }

                }
                catch(Exception e)
                {
                    TriggerUploadMessage("", false);
                    _logger.LogError(e.Message);
                }
                
            }
        }

        public void UpdateScpStatus(VerifyScpConfigDto rec)
        {
            try
            {
                _context.ArScps.ToList().ForEach(record => {
                    if(rec.Ip == record.Ip && rec.Mac == record.Mac)
                    {
                        record.IsReset = rec.IsReset;
                        record.IsUpload = rec.IsUpload;
                    }
                }
                );
                _context.SaveChanges();
                _logger.LogInformation("Update status success");
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex.Message);
            }
        }

        public void VerifyScpConfiguration(SCPReplyMessage message)
        {
            VerifyScpConfigDto verifyScpConfigDto = new VerifyScpConfigDto();
            string ScpIp = _helperService.GetScpIpFromId((short)message.SCPId);
            string ScpMac = _helperService.GetMacFromId((short)message.SCPId);
            var config = _sysService.GetScpSetting();
            VerifyScpConfigDto rec = new VerifyScpConfigDto();
            rec.Ip = ScpIp;
            rec.Mac = ScpMac;
            foreach (var i in message.str_sts.sStrSpec)
            {
                switch ((ScpStructure)i.nStrType)
                {
                    case ScpStructure.SCPSID_TRAN:
                        // Handle Transactions
                        //rec.RecAllocTransaction = i.nRecords < config.NTransaction ? 1 : i.nRecords > config.NTransaction ? -1 : 0;
                        //rec.IsReset = rec.RecAllocTransaction == 0 ? false : true;
                        break;

                    case ScpStructure.SCPSID_TZ:
                        // Handle Time zones
                        rec.RecAllocTimezone = i.nActive - 1 < config.NTz ? 1 : i.nActive - 1 > config.NTz ? -1 : 0;
                        break;

                    case ScpStructure.SCPSID_HOL:
                        // Handle Holidays
                        rec.RecAllocHoliday = i.nActive < config.NHol ? 1 : i.nActive > config.NHol ? -1 : 0;
                        break;

                    case ScpStructure.SCPSID_MSP1:
                        // Handle Msp1 ports (SIO drivers)
                        //rec.RecAllocMsp1 = i.nActive < config.NMsp1Port ? 1 : i.nActive > config.NMsp1Port ? -1 : 0;
                        break;

                    case ScpStructure.SCPSID_SIO:
                        // Handle SIOs
                        rec.RecAllocSio = i.nActive < config.NSio ? 1 : i.nActive > config.NSio ? -1 : 0;
                        break;

                    case ScpStructure.SCPSID_MP:
                        // Handle Monitor points
                        rec.RecAllocMp = i.nActive < config.NMp ? 1 : i.nActive > config.NMp ? -1 : 0;
                        break;

                    case ScpStructure.SCPSID_CP:
                        rec.RecAllocCp = i.nActive < config.NCp ? 1 : i.nActive > config.NCp ? -1 : 0;
                        // Handle Control points
                        break;

                    case ScpStructure.SCPSID_ACR:
                        // Handle Access control readers
                        rec.RecAllocAcr = i.nActive < config.NAcr ? 1 : i.nActive > config.NAcr ? -1 : 0;
                        break;

                    case ScpStructure.SCPSID_ALVL:
                        // Handle Access levels
                        rec.RecAllocAlvl = i.nActive < config.NAlvl ? 1 : i.nActive > config.NAlvl ? -1 : 0;
                        break;

                    case ScpStructure.SCPSID_TRIG:
                        // Handle Triggers
                        rec.RecAllocTrig = i.nActive < config.NTrgr ? 1 : i.nActive > config.NTrgr ? -1 : 0;
                        break;

                    case ScpStructure.SCPSID_PROC:
                        // Handle Procedures
                        rec.RecAllocProc = i.nActive < config.NProc ? 1 : i.nActive > config.NProc ? -1 : 0;
                        break;

                    case ScpStructure.SCPSID_MPG:
                        // Handle Monitor point groups
                        rec.RecAllocMpg = i.nActive < config.NMpg ? 1 : i.nActive > config.NMpg ? -1 : 0;
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
                        int ncard = _credentialService.GetCredentialRecAlloc();
                        rec.RecAllocCrdb = i.nRecords < config.NCard ? 1 : i.nRecords > config.NCard ? -1 : 0;
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
                bool hasMisMatch = rec.GetType()
                       .GetProperties()
                       .Where(p => p.PropertyType == typeof(int)) // only int properties
                       .Select(p => (int)p.GetValue(rec))
                       .Any(value => value == -1 || value == 1);
                rec.IsReset = hasMisMatch;
                // Check mismatch device configuration
                rec.IsUpload = CheckMisMatchDeviceConfiguration(message.SCPId);
                UpdateScpStatus(rec);
                _cmndService.TriggerVerifyScpConfiguration(rec);
                TriggerSyncStatus();
            }
        }

        private bool CheckMisMatchDeviceConfiguration(int sCPId)
        {
            string _scpMac = _helperService.GetMacFromId((short)sCPId);
            try
            {
                var scp = _context.ArScps.FirstOrDefault(p => p.Mac == _scpMac);
                if (scp == null)
                {
                    _logger.LogWarning("No SCP found for MAC {Mac}", _scpMac);
                    return false;
                }

                // Check if any control points created after last sync
                return _context.ArControlPoints.Any(p => p.CreatedDate > scp.LastSync) ||
                    _context.ArMonitorPoints.Any(p => p.CreatedDate > scp.LastSync) ||
                    _context.ArAcrs.Any(p => p.CreatedDate > scp.LastSync);
            }
            catch(Exception e)
            {
                _logger.LogError(e.Message);
                return false;
            }
            

        }

        public void AssignIpToIdReport(SCPReplyMessage message, List<IDReport> iDReports)
        {
            foreach (var i in iDReports)
            {
                if (i.ScpID == message.SCPId)
                {
                    i.Port = message.web_network.cPortTnl;
                    i.Ip = message.web_network.cIpAddr;
                }
            }
            TriggerIdReport(iDReports);
        }

        public IDReport handleInCommingScp(SCPReplyMessage message,TimeZoneService tz, ICardFormatService cfmt,IAccessLevelService alvl)
        {
            if (ValidateScp(message.id.scp_id).Equals(Constants.ConstantsHelper.COMMAND_SUCCESS))
            {

                if (IsRegister(Utility.ByteToHexStr(message.id.mac_addr)))
                {
                    ScpConfiguration(message.id.scp_id, tz, cfmt, alvl);
                    ReadStructureStatus(message.id.scp_id);
                    return null;
                }
                else
                {
                    _config.write.GetWebConfig(message.id.scp_id);
                    IDReport iDReport = new IDReport();
                    iDReport.DeviceID = message.id.device_id;
                    iDReport.DeviceVer = message.id.device_ver;
                    iDReport.SoftwareRevMajor = message.id.sft_rev_major;
                    iDReport.SoftwareRevMinor = message.id.sft_rev_minor;
                    iDReport.SerialNumber = message.id.serial_number;
                    iDReport.RamSize = message.id.ram_size;
                    iDReport.RamFree = message.id.ram_free;
                    iDReport.ESec = Utility.UnixToDateTime(message.id.e_sec);
                    iDReport.DatabaseMax = message.id.db_max;
                    iDReport.DatabaseActive = message.id.db_active;
                    iDReport.DipSwitchPowerUp = message.id.dip_switch_pwrup;
                    iDReport.DipSwitchCurrent = message.id.dip_switch_current;
                    iDReport.ScpID = message.id.scp_id;
                    iDReport.FirmWareAdvisory = message.id.firmware_advisory;
                    iDReport.ScpIn1 = message.id.scp_in_1;
                    iDReport.ScpIn2 = message.id.scp_in_2;
                    iDReport.NOemCode = message.id.nOemCode;
                    iDReport.ConfigFlag = message.id.config_flags;
                    iDReport.MacAddress = Utility.ByteToHexStr(message.id.mac_addr);
                    iDReport.TlsStatus = message.id.tls_status;
                    iDReport.OperMode = message.id.oper_mode;
                    iDReport.ScpIn3 = message.id.scp_in_3;
                    iDReport.CumulativeBldCnt = message.id.cumulative_bld_cnt;
                    return iDReport;
                }
            }
            return null;
        }

        public void TriggerIdReport(List<IDReport> IdReports)
        {
            _hub.Clients.All.SendAsync("IdReport", IdReports);
        }





        #endregion



    }
}
