using HID.Aero.ScpdNet.Wrapper;
using HIDAeroService.Data;
using HIDAeroService.Dto;
using HIDAeroService.Entity;
using HIDAeroService.Hubs;
using HIDAeroService.Mapper;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Xml.Linq;

namespace HIDAeroService.Service
{
    public class AcrService
    {
        private readonly ILogger<AcrService> _logger;
        private readonly AppDbContext _context;
        private readonly AppConfigData _config;
        private readonly HelperService _helperService;
        private readonly IHubContext<AcrHub> _hub;

        public AcrService(AppDbContext context, AppConfigData config,ILogger<AcrService> logger,HelperService helperService,IHubContext<AcrHub> hub)
        {
            _hub = hub;
            _helperService = helperService;
            _logger = logger;
            _context = context;
            _config = config;
        }

        public bool MomentaryUnlock(string ScpIp,short AcrNo)
        {
            short ScpId = _helperService.GetScpIdFromIp(ScpIp);
            if (!_config.write.MomentaryUnlock(ScpId, AcrNo))
            {
                _logger.LogError("MomentaryUnlock : False");
                return false;
            }
            return true;
        }

        public List<ACRDto> GetACRList()
        {
            List<ACRDto> dtos = new List<ACRDto>();
            List<ar_acr> datas = _context.ar_acrs.ToList();
            int i = 1;
            foreach (var data in datas) 
            {
                string sio_name = _context.ar_sios.Where(p => p.sio_number == data.rdr_sio).Select(p => p.name).First();
                dtos.Add(MapperHelper.ACRToACRDto(data,i,sio_name));
                i+=1;
            }
            return dtos;
        }
        public List<ACRDto> GetACRListByIp(string ip)
        {
            try
            {
                List<ACRDto> dtos = new List<ACRDto>();
                List<ar_acr> datas = _context.ar_acrs.Where(p => p.scp_ip == ip).ToList();
                
                int i = 1;
                foreach (var data in datas)
                {
                    string sio_name = _context.ar_sios.Where(p => p.sio_number == data.rdr_sio).Select(p => p.name).First();
                    dtos.Add(MapperHelper.ACRToACRDto(data, i,sio_name));
                    i += 1;
                }
                return dtos;
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.Message);
                return [];
            }

        }


        public short GetUniqueAcrNo(string ScpIp)
        {

            short highestAcrNumber;
            if (!_context.ar_acr_no.Any(p => p.scp_ip == ScpIp))
                return 0;

            if (_context.ar_acr_no.Any(p => p.is_available == true && p.scp_ip == ScpIp))
            {
                highestAcrNumber = _context.ar_acr_no.Where(p => p.is_available == true && p.scp_ip == ScpIp).Select(p => p.acr_number).First();
                return highestAcrNumber;
            }
            else
            {
                highestAcrNumber = _context.ar_acr_no.Where(p => p.is_available == false && p.scp_ip == ScpIp).Max(p => p.acr_number);
                highestAcrNumber += 1;
                return highestAcrNumber;
            }

        }

        public bool SaveAcrToDatabase(AddACRDto dto,short AcrNo)
        {
            
            try
            {
                var isNewCpNo = _context.ar_acr_no.Where(p => p.acr_number == AcrNo).FirstOrDefault();
                if (isNewCpNo == null)
                {
                    ar_n_acr ncp = MapperHelper.ACRDtoTonACR(dto, AcrNo);
                    _context.ar_acr_no.Add(ncp);
                }
                else
                {
                    isNewCpNo.is_available = false;
                }
                
                ar_acr ar = MapperHelper.ACRDtoToACR(dto,AcrNo);
                _context.ar_acrs.Add(ar);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            
        }

        //public bool CreateAccessControlReader(AddACRDto dto)
        //{
        //    short ScpId = _helperService.GetScpIdFromIp(dto.ScpIp);
        //    short AcrNo = GetUniqueAcrNo(dto.ScpIp);
        //    if (!_config.write.AccessControlReaderConfiguration(ScpId, AcrNo, dto))
        //    {
        //        _logger.LogError("AccessControlReaderConfiguration : False");
        //        return false;
        //    }

        //    if (!SaveAcrToDatabase(dto, AcrNo))
        //    {
        //        Console.WriteLine($"Save Monitor Point to Database : False");
        //        return false;
        //    }

        //    //if (!_config.write.SIOPanelConfiguration(ScpID, SioNo, 196, 0, 0, true))
        //    //{
        //    //    _logger.LogError("InternalSIOPanelConfiguration : False");
        //    //    return false;
        //    //}


        //    return true;
        //}

        #region Command Group

        public bool CreateAccessControlReaderForCreate(AddACRDto dto)
        {
            short ScpId = _helperService.GetScpIdFromIp(dto.ScpIp);
            short AcrNo = GetUniqueAcrNo(dto.ScpIp);

            byte readerInOsdpFlag = 0x00;
            short readerLedDriveMode = 0;
            if (dto.IsReaderOsdp)
            {
                readerInOsdpFlag |= dto.OsdpBaudRate;
                readerInOsdpFlag |= dto.OsdpNoDiscover;
                readerInOsdpFlag |= dto.OsdpTracing;
                readerInOsdpFlag |= dto.OsdpAddress;
                readerInOsdpFlag |= dto.OsdpSecureChannel;
                readerLedDriveMode = 7;
            }
            else
            {
                readerLedDriveMode = 1;
            }


            // Reader In Config
            if (!_config.write.ReaderSpecification(ScpId, dto.ReaderSioNumber, dto.ReaderNumber, dto.ReaderDataFormat, dto.KeyPadMode, readerLedDriveMode, readerInOsdpFlag))
            {
                _logger.LogError($"Reader In Specification : False");
                Console.WriteLine($"Reader In Specification : False");
                return false;
            }

            // Strike Output Config
            if (!_config.write.OutputPointSpecification(ScpId, dto.StrikeSioNumber, dto.StrikeNumber, dto.RelayMode))
            {
                Console.WriteLine($"Strike Output Specification : False");
                return false;
            }

            // Door Sensor Config

            if (!_config.write.InputPointSpecification(ScpId, dto.SensorSioNumber, dto.SensorNumber, dto.SensorMode, 4, 7))
            {
                Console.WriteLine($"Sensor Input Specification : False");
                return false;
            }

            // Rex0 Input Config
            Console.WriteLine("##################################### > " + dto.IsREX0Used + "  " + dto.IsREX1Used);
            if (dto.IsREX0Used)
            {
                if (!_config.write.InputPointSpecification(ScpId, dto.REX0SioNumber, dto.REX0Number, dto.REX0SensorMode, 2, 7))
                {
                    Console.WriteLine($"Rex 0 Input Specification : False");
                    return false;
                }
            }
            else
            {
                dto.REX0SioNumber = -1;
                dto.REX0Number = -1;
            }


            // Rex1 Input Config
            if (dto.IsREX1Used)
            {
                if (!_config.write.InputPointSpecification(ScpId, dto.REX1SioNumber, dto.REX1Number, dto.REX1SensorMode, 2, 7))
                {
                    Console.WriteLine($"Rex 1 Input Specification : False");
                    return false;
                }
            }
            else
            {
                dto.REX1SioNumber = -1;
                dto.REX1Number = -1;
            }

            // Alternate Reader Specification
            if (dto.IsAlternateReaderUsed)
            {
                byte alternateReaderInOsdpFlag = 0x00;
                short alternateLedDriveMode = 0;
                if (dto.IsAlternateReaderOsdp)
                {
                    alternateReaderInOsdpFlag |= dto.AlternateOsdpBaudRate;
                    alternateReaderInOsdpFlag |= dto.AlternateOsdpNoDiscover;
                    alternateReaderInOsdpFlag |= dto.AlternateOsdpAddress;
                    alternateReaderInOsdpFlag |= dto.AlternateOsdpTracing;
                    alternateReaderInOsdpFlag |= dto.AlternateOsdpSecureChannel;
                    alternateLedDriveMode = 7;
                }
                else
                {
                    alternateLedDriveMode = 1;
                }


                if (!_config.write.ReaderSpecification(ScpId, dto.AlternateReaderSioNumber, dto.AlternateReaderNumber, dto.AlternateReaderDataFormat, dto.AlternateKeyPadMode, alternateLedDriveMode, alternateReaderInOsdpFlag))
                {
                    _logger.LogError($"Reader Out Specification : False");
                    Console.WriteLine($"Reader Out Specification : False");
                    return false;
                }
            }


            if (!_config.write.AccessControlReaderConfigurationForCreate(ScpId, AcrNo, dto))
            {
                _logger.LogError("AccessControlReaderConfiguration : False");
                return false;
            }


            if (!SaveAcrToDatabase(dto,AcrNo))
            {
                Console.WriteLine($"Save Monitor Point to Database : False");
                return false;
            }

            return true;
        }

        public List<AcsRdrModeDto> GetAcsRdrModeList()
        {
            var datas = _context.ar_rdr_modes.ToList();
            List<AcsRdrModeDto> mode = new List<AcsRdrModeDto>();
            if(datas.Count > 0)
            {
                foreach (var data in datas) 
                {
                    mode.Add(MapperHelper.AcsRdrModeToAcsRdrModeDto(data));
                }
                return mode;
                
            }
            return new List<AcsRdrModeDto>();
        }

        public List<short> GetAvailableReader(short sio)
        {
            var reader = _context.ar_sios.Where(cp => cp.sio_number == sio).Select(cp => cp.n_readers).First();
            var unavailable = _context.ar_acrs.Where(cp => cp.rdr_sio == sio).Select(cp => cp.reader_number).ToList();
            var unavailable2 = _context.ar_acrs.Where(cp => cp.rdr_sio == sio).Select(cp => cp.altrdr_number).ToList();
            unavailable.AddRange(unavailable2);
            List<short> all = Enumerable.Range(0, reader).Select(i => (short)i).ToList();
            return all.Except(unavailable).ToList();
        }

        public List<StrikeModeDto> GetStrikeMode()
        {
            List<StrikeModeDto> res = new List<StrikeModeDto>();
            var datas = _context.ar_strk_modes.ToList();
            foreach(var d in datas)
            {
                res.Add(MapperHelper.StrikeModeToStrikeModeDto(d));
            }
            return res;
        }

        public List<ACRModeDto> GetAcrModeList()
        {
            List<ACRModeDto> res = new List<ACRModeDto>();
            var datas = _context.ar_acr_modes.ToList();
            foreach (var d in datas)
            {
                res.Add(MapperHelper.ACRModeToACRModeDto(d));
            }
            return res;
        }

        public List<ApbModeDto> GetApbModeList()
        {
            List<ApbModeDto> res = new List<ApbModeDto>();
            var datas = _context.ar_apb_modes.ToList();
            foreach (var d in datas)
            {
                res.Add(MapperHelper.ApbModeToApbModeDto(d));
            }
            return res;
        }


        #endregion

        public void TriggerDeviceStatus(int ScpId,short AcrNumber,string AcrMode,string AccessPointStatus)
        {
            string ScpIp = _helperService.GetScpIpFromId((short)ScpId);
            _hub.Clients.All.SendAsync("AcrStatus", ScpIp, AcrNumber, AcrMode, AccessPointStatus);
        }

        public bool GetAcrStatus(string ScpIp, short AcrNo)
        {
            short ScpId = _helperService.GetScpIdFromIp(ScpIp);
            if (!_config.write.GetAcrStatus(ScpId, AcrNo, 1))
            {
                return false;
            }
            return true;
        }

        public bool ChangeACRMode(string ScpIp,short AcrNo,short Mode)
        {
            short ScpId = _helperService.GetScpIdFromIp(ScpIp);
            if (!_config.write.ACRMode(ScpId, AcrNo, Mode))
            {
                return false;
            }
            return true;
        }

        public bool RemoveAccessControlReader(string ScpIp,short AcrNo)
        {
            try
            {
                var acr = _context.ar_acrs.Where(d => d.scp_ip == ScpIp && d.acr_number == AcrNo).FirstOrDefault();
                _context.ar_acrs.Remove(acr);
                var nacr = _context.ar_acr_no.Where(d => d.acr_number == AcrNo && d.scp_ip == ScpIp).FirstOrDefault();
                if (nacr != null)
                {
                    nacr.is_available = true;
                }
                _context.SaveChanges();
                _logger.LogInformation("Delete ACR {0} Success",AcrNo);
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return false;
            }
        }

        public int GetAcrRecAlloc(string ScpIp)
        {
            return _context.ar_acrs.Where(p => p.scp_ip == ScpIp).Count();
        }

    }
}
