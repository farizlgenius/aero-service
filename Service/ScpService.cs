using HIDAeroService.AeroLibrary;
using HIDAeroService.Constants;
using HIDAeroService.Data;
using HIDAeroService.Dto;
using HIDAeroService.Dto.Scp;
using HIDAeroService.Entity;
using HIDAeroService.Hubs;
using HIDAeroService.Mapper;
using HIDAeroService.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace HIDAeroService.Service
{
    public class ScpService 
    {
        private readonly HelperService _helperService;
        private readonly TZService _tzService;
        private readonly AppDbContext _context;
        private readonly AppConfigData _config;
        private readonly MpService _mpService;
        private readonly AcrService _acrService;
        private readonly IHubContext<ScpHub> _hub;
        private readonly ILogger<ScpService> _logger;
        public ScpService(AppDbContext appDbContext,AppConfigData config,IHubContext<ScpHub> hub,MpService mpService,AcrService acrService,ILogger<ScpService> logger,HelperService helperService,TZService tzService)
        {
            _tzService = tzService;
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

        public ScpStatusDto GetOnlineStatus(short scp_id)
        {
            ScpStatusDto dto = new ScpStatusDto();
            dto.ScpIp = _helperService.GetScpIpFromId(scp_id);
            dto.ScpId = scp_id;
            dto.Status = _config.write.CheckSCPStatus(scp_id);
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
                if (!(await IsRegisterAsync(i.MacAddress)))
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
                ar_scp scp = MapperHelper.ScpRegisDtoToSCP(scpDto);
                _context.ar_scps.Add(scp);
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
            var scps = _context.ar_scps.ToList();
            if(scps.Count != 0)
            {
                int i = 1;
                foreach (ar_scp _scp in scps)
                {
                    scpDtos.Add(MapperHelper.SCPToScpDto(i,_scp,0));
                    i++;
                }
                return scpDtos;
            }
            return new List<ScpDto>();

        }

        public async Task<bool> IsRegisterAsync(string mac) 
        {
            return await _context.ar_scps.AnyAsync(s => s.mac == mac);
        }

        public bool IsIpRegister(string ipaddress)
        {
            return _context.ar_scps.Any(s => s.ip_address == ipaddress);
        }



        public void TriggerDeviceStatus(int Status,int ScpId)
        {
            //GetOnlineStatus()
            var result = _hub.Clients.All.SendAsync("CommStatus", Status, ScpId);
        }


        public bool ResetScp(ResetScpDto dto)
        {
            short ScpId = _helperService.GetScpIdFromIp(dto.ScpIp);

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
        public bool ValidateSCPConnection(short ScpID, TZService tzService, CardFormatService cfmtService, AlvlService alvlService)
        {
            if (!_config.write.SCPDeviceSpecification(ScpID))
            {
                Console.WriteLine("SCPDeviceSpecification : False");
                _logger.LogError("SCPDeviceSpecification : False");
                return false;
            }


            if (!_config.write.AccessDatabaseSpecification(ScpID))
            {
                Console.WriteLine("AccessDatabaseSpecification : False");
                return false;
            }

            if (!_config.write.TimeSet(ScpID))
            {
                Console.WriteLine("TimeSet : False");
                return false;
            }

            List<ar_card_format> formats = cfmtService.GetCardFormatList();
            foreach (var format in formats)
            {
                if (!_config.write.CardFormatterConfiguration(ScpID, format.number, format.facility, format.offset, format.function_id, format.flags, format.bits, format.pe_ln, format.pe_loc, format.po_ln, format.po_loc, format.fc_ln, format.fc_loc, format.ch_ln, format.ch_loc, format.ic_ln, format.ic_loc))
                {
                    Console.WriteLine("CardFormatterConfiguration : False");
                }
            }


            List<ar_tz> timezones = tzService.GetTimeZoneList();
            foreach (var tz in timezones)
            {
                if (!_config.write.ExtendedTimeZoneActSpecification(ScpID, tz.tz_number, tz.mode, tz.act_time, tz.deact_time, tz.intervals, []))
                {
                    Console.WriteLine($"ExtendedTimeZoneActSpecification : False");
                    return false;
                }
            }


            // Create Access Level All
            List<ar_access_lv> acls = alvlService.GetAccessLevelList();
            foreach (var a in acls)
            {
                if (!_config.write.AccessLevelConfigurationExtended(ScpID, a.access_lv_number))
                {
                    Console.WriteLine("AccessLevelConfigurationExtended : False");
                    return false;
                }
            }


            return true;
        }
        public bool InitialScpConfiguration(short ScpID, CpService cp, MpService mp, string mac)
        {
            short SioNo = 0;
            short model = 196;
            short address = 0;
            short msp1_number = 0;
            short internal_port_number = 3;
            short internal_baud_rate = -1;
            short protocol = 0;
            short n_dialect = 0;

            Console.WriteLine(ScpID);
            if (!_config.write.SetTransactionLogIndex(ScpID, true))
            {
                Console.WriteLine("SetTransactionLogIndex : False 1");
                if (!_config.write.SetTransactionLogIndex(ScpID, false))
                {
                    Console.WriteLine("SetTransactionLogIndex : False 2");
                    if (_config.write.SetTransactionLogIndex(ScpID, true))
                    {
                        Console.WriteLine("SetTransactionLogIndex : False 3");
                        return true;
                    }
                    return false;
                }
                return false;
            }

            if (!_config.write.SIODriverConfiguration(ScpID, msp1_number, internal_port_number, internal_baud_rate, protocol))
            {
                Console.WriteLine("InternalSIODriverConfiguration : False");
                return false;
            }

            if (!_config.write.SIOPanelConfiguration(ScpID, SioNo, model, address, msp1_number, true))
            {
                Console.WriteLine("InternalSIOPanelConfiguration : False");
                return false;
            }

            short[] component = Utility.GetSCPComponent(model);
            short nInput = component[0];
            short nOutput = component[1];
            short nReader = component[2];


            // Create Monitor Point for Tamper / AC Fail / BATT Fail

            for (short i = 0; i < nInput; i++)
            {
                if (i + 1 >= nInput - 3)
                {
                    _mpService.InputPointSpecification(ScpID, SioNo, i, 0);
                }

            }


            return true;
        }

        public string RemoveScp(RemoveScpDto dto)
        {
            short ScpId = _helperService.GetScpIdFromIp(dto.ScpIp);
            try
            {
                if (!_config.write.DeleteScp(ScpId))
                {
                    return AppContants.DELETE_SCP_FAIL;
                }

                if (!_config.write.DetachScp(ScpId))
                {
                    return AppContants.DETACH_SCP_FAIL;
                }

                var record = _context.ar_scps.First(p => p.ip_address == dto.ScpIp);
                //var nrecord = _context.ar_scp
                if(record != null)
                {
                    _context.ar_scps.Remove(record);
                }
                else
                {
                    return AppContants.NOT_FOUND_RECORD;
                }

                return AppContants.REMOVE_SUCCESS;
            }
            catch (Exception ex) 
            {
                return ex.Message;
            }
        }

        public string VerifyScpConfiguration(VerifyScpDto dto)
        {
            short ScpId = _helperService.GetScpIdFromIp(dto.ScpIp);
            if (!_config.write.ReadStructureStatus(ScpId))
            {
                return AppContants.COMMAND_UNSUCCESS;
            }
            return AppContants.COMMAND_SUCCESS;
        }

        public string UploadScpConfig(UploadScpConfigDto dto)
        {
            short ScpId = _helperService.GetScpIdFromIp(dto.ScpIp);
            if (!_config.write.UploadScpConfig(ScpId))
            {
                return AppContants.COMMAND_SUCCESS;
            }
            return AppContants.COMMAND_UNSUCCESS;
        }



        #endregion



    }
}
