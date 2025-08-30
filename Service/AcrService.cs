using HID.Aero.ScpdNet.Wrapper;
using HIDAeroService.Data;
using HIDAeroService.Dto;
using HIDAeroService.Dto.Acr;
using HIDAeroService.Entity;
using HIDAeroService.Hubs;
using HIDAeroService.Mapper;
using Microsoft.AspNetCore.SignalR;
using Org.BouncyCastle.Crypto.Macs;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Xml.Linq;

namespace HIDAeroService.Service
{
    public class AcrService
    {
        private readonly ILogger<AcrService> _logger;
        private readonly AppDbContext _context;
        private readonly AeroLibMiddleware _config;
        private readonly HelperService _helperService;
        private readonly IHubContext<AcrHub> _hub;

        public AcrService(AppDbContext context, AeroLibMiddleware config,ILogger<AcrService> logger,HelperService helperService,IHubContext<AcrHub> hub)
        {
            _hub = hub;
            _helperService = helperService;
            _logger = logger;
            _context = context;
            _config = config;
        }

        public bool MomentaryUnlock(string ScpMac,short AcrNo)
        {
            short ScpId = _helperService.GetScpIdFromMac(ScpMac);
            if (!_config.write.MomentaryUnlock(ScpId, AcrNo))
            {
                _logger.LogError("MomentaryUnlock : False");
                return false;
            }
            return true;
        }

        public List<AcrDto> GetACRList()
        {
            List<AcrDto> dtos = new List<AcrDto>();
            List<ArAcr> datas = _context.ArAcrs.ToList();
            int i = 1;
            foreach (var data in datas) 
            {
                string sio_name = _context.ArSios.Where(p => p.SioNumber == data.RdrSio).Select(p => p.Name).First();
                dtos.Add(MapperHelper.ACRToACRDto(data,i,sio_name));
                i+=1;
            }
            return dtos;
        }
        public List<AcrDto> GetAcrListByMac(string ScpMac)
        {
            try
            {
                List<AcrDto> dtos = new List<AcrDto>();
                List<ArAcr> datas = _context.ArAcrs.Where(p => p.ScpMac == ScpMac).ToList();
                
                int i = 1;
                foreach (var data in datas)
                {
                    string sio_name = _context.ArSios.Where(p => p.SioNumber == data.RdrSio).Select(p => p.Name).First();
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


        public short GetUniqueAcrNo(string ScpMac)
        {

            short highestAcrNumber;
            if (!_context.ArAcrNo.Any(p => p.ScpMac == ScpMac))
                return 0;

            if (_context.ArAcrNo.Any(p => p.IsAvailable == true && p.ScpMac == ScpMac))
            {
                highestAcrNumber = _context.ArAcrNo.Where(p => p.IsAvailable == true && p.ScpMac == ScpMac).Select(p => p.AcrNo).First();
                return highestAcrNumber;
            }
            else
            {
                highestAcrNumber = _context.ArAcrNo.Where(p => p.IsAvailable == false && p.ScpMac == ScpMac).Max(p => p.AcrNo);
                highestAcrNumber += 1;
                return highestAcrNumber;
            }

        }

        public bool Save(AddAcrDto dto,short AcrNo)
        {
            
            try
            {
                var isNewCpNo = _context.ArAcrNo.Where(p => p.AcrNo == AcrNo).FirstOrDefault();
                if (isNewCpNo == null)
                {
                    ArAcrNo ncp = MapperHelper.ACRDtoTonACR(dto, AcrNo);
                    _context.ArAcrNo.Add(ncp);
                }
                else
                {
                    isNewCpNo.IsAvailable = false;
                }
                
                ArAcr ar = MapperHelper.ACRDtoToACR(dto,AcrNo);
                _context.ArAcrs.Add(ar);
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

        public bool CreateAccessControlReaderForCreate(AddAcrDto dto)
        {
            short ScpId = _helperService.GetScpIdFromMac(dto.ScpMac);
            short AcrNo = GetUniqueAcrNo(dto.ScpMac);

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
            if (_config.write.OutputPointSpecification(ScpId, dto.StrikeSioNumber, dto.StrikeNumber, dto.RelayMode) == -1)
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


            if (!Save(dto,AcrNo))
            {
                Console.WriteLine($"Save Monitor Point to Database : False");
                return false;
            }

            return true;
        }

        public List<AcsRdrModeDto> GetAcsRdrModeList()
        {
            var datas = _context.ArReaderModes.ToList();
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
            var reader = _context.ArSios.Where(cp => cp.SioNumber == sio).Select(cp => cp.NReader).First();
            var unavailable = _context.ArAcrs.Where(cp => cp.RdrSio == sio).Select(cp => cp.ReaderNo).ToList();
            var unavailable2 = _context.ArAcrs.Where(cp => cp.RdrSio == sio).Select(cp => cp.AlternateReaderNo).ToList();
            unavailable.AddRange(unavailable2);
            List<short> all = Enumerable.Range(0, reader).Select(i => (short)i).ToList();
            return all.Except(unavailable).ToList();
        }

        public List<StrikeModeDto> GetStrikeMode()
        {
            List<StrikeModeDto> res = new List<StrikeModeDto>();
            var datas = _context.ArStrikeModes.ToList();
            foreach(var d in datas)
            {
                res.Add(MapperHelper.StrikeModeToStrikeModeDto(d));
            }
            return res;
        }

        public List<AcrModeDto> GetAcrModeList()
        {
            List<AcrModeDto> res = new List<AcrModeDto>();
            var datas = _context.ArAcrModes.ToList();
            foreach (var d in datas)
            {
                res.Add(MapperHelper.ACRModeToACRModeDto(d));
            }
            return res;
        }

        public List<ApbModeDto> GetApbModeList()
        {
            List<ApbModeDto> res = new List<ApbModeDto>();
            var datas = _context.ArApbModes.ToList();
            foreach (var d in datas)
            {
                res.Add(MapperHelper.ApbModeToApbModeDto(d));
            }
            return res;
        }


        #endregion

        public void TriggerDeviceStatus(int ScpId,short AcrNo, string AcrMode,string AccessPointStatus)
        {
            string ScpMac = _helperService.GetMacFromId((short)ScpId);
            _hub.Clients.All.SendAsync("AcrStatus", ScpMac, AcrNo, AcrMode, AccessPointStatus);
        }

        public bool GetAcrStatus(string ScpMac, short AcrNo)
        {
            short ScpId = _helperService.GetScpIdFromMac(ScpMac);
            if (!_config.write.GetAcrStatus(ScpId, AcrNo, 1))
            {
                return false;
            }
            return true;
        }

        public bool ChangeACRMode(string ScpMac,short AcrNo,short Mode)
        {
            short ScpId = _helperService.GetScpIdFromMac(ScpMac);
            if (!_config.write.ACRMode(ScpId, AcrNo, Mode))
            {
                return false;
            }

            if (!UpdateAcrModeInDatabase(ScpMac, AcrNo, Mode))
            {
                return false;
            }
            
            return true;
        }

        public bool UpdateAcrModeInDatabase(string ScpMac, short AcrNo, short Mode)
        {
            try
            {
                var data = _context.ArAcrs.FirstOrDefault(p => p.ScpMac == ScpMac && p.AcrNo == AcrNo);
                if (data != null) data.DoorMode = Mode;
                _context.SaveChanges();
                return true;
            }catch(Exception e)
            {
                _logger.LogError(e.Message);
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public bool RemoveAccessControlReader(string ScpMac,short AcrNo)
        {
            try
            {
                var acr = _context.ArAcrs.Where(d => d.ScpMac == ScpMac && d.AcrNo == AcrNo).FirstOrDefault();
                _context.ArAcrs.Remove(acr);
                var nacr = _context.ArAcrNo.Where(d => d.AcrNo == AcrNo && d.ScpMac == ScpMac).FirstOrDefault();
                if (nacr != null)
                {
                    nacr.IsAvailable = true;
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

        public int GetAcrRecAlloc(string ScpMac)
        {
            return _context.ArAcrs.Where(p => p.ScpMac == ScpMac).Count();
        }

    }
}
