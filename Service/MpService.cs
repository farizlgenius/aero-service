using HID.Aero.ScpdNet.Wrapper;
using HIDAeroService.Data;
using HIDAeroService.Dto;
using HIDAeroService.Dto.Cp;
using HIDAeroService.Dto.Mp;
using HIDAeroService.Entity;
using HIDAeroService.Hubs;
using HIDAeroService.Mapper;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto.Macs;

namespace HIDAeroService.Service
{
    public sealed class MpService
    {
        private readonly AppDbContext _context;
        private readonly HelperService _helperService;
        private readonly AeroLibMiddleware _config;
        private readonly IHubContext<MpHub> _hub;
        private readonly ILogger<MpService> _logger;
        public MpService(HelperService helperService,AeroLibMiddleware config,AppDbContext context,IHubContext<MpHub> hub,ILogger<MpService> logger) 
        {
            _logger = logger;
            _hub = hub;
            _config = config;
            _context = context;
            _helperService = helperService;
        }

        public short GetUniqueMpNo(string scpMac)
        {

            short highestMpNumber;
            if (!_context.ArMpNo.Any(p => p.ScpMac == scpMac))
                return 0;

            if (_context.ArMpNo.Any(p => p.IsAvailable == true && p.ScpMac == scpMac))
            {
                highestMpNumber = _context.ArMpNo.Where(p => p.IsAvailable == true && p.ScpMac == scpMac).Select(p => p.MpNo).First();
                return highestMpNumber;
            }
            else
            {
                highestMpNumber = _context.ArMpNo.Where(p => p.IsAvailable == false && p.ScpMac == scpMac).Max(p => p.MpNo);
                highestMpNumber += 1;
                return highestMpNumber;
            }


        }



        public bool Save(string name,string scpIp,string scpMac,short sio_number,short mp_number,short ip_number,short icvt_mode,short lf_code,short delay_entry,short delay_exit)
        {
            try
            {
                ArMonitorPoint m = new ArMonitorPoint();
                m.Name = name;
                m.ScpIp = scpIp; 
                m.ScpMac = scpMac;
                m.SioNo = sio_number;
                m.MpNo = mp_number;
                m.IpNo = ip_number;
                m.IcvtNo = icvt_mode;
                m.LfCode = lf_code;
                m.DelayEntry = delay_entry;
                m.DelayExit = delay_exit;
                m.CreatedDate = DateTime.Now;
                m.UpdatedDate = DateTime.Now;
                _context.ArMonitorPoints.Add(m);

                var isNewMpNo = _context.ArMpNo.Where(p => p.MpNo == mp_number).FirstOrDefault();
                if(isNewMpNo == null)
                {
                    ArMpNo n = new ArMpNo();
                    n.ScpMac = scpMac;
                    n.SioNo = sio_number;
                    n.MpNo = mp_number;
                    n.IsAvailable = false;
                    _context.ArMpNo.Add(n);
                }
                else
                {
                    isNewMpNo.IsAvailable = false;
                }

                return _context.SaveChanges() > 0;

            }
            catch (Exception e) 
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public List<short> GetAvailableIp(short sio)
        {
            var input = _context.ArSios.Where(mp => mp.SioNumber == sio).Select(mp => mp.NInput).First();
            var unavailable = _context.ArMonitorPoints.Where(mp => mp.SioNo == sio).Select(mp => mp.IpNo).ToList();
            List<short> all = Enumerable.Range(0, input-3).Select(i => (short)i).ToList();
            return all.Except(unavailable).ToList();
        }

        public bool InputPointSpecification(short ScpId,short SioNo,short InputNo,short IcvtMode,short Debounce = 2,short HoldTime = 5)
        {
            if (!_config.write.InputPointSpecification(ScpId, SioNo, InputNo, IcvtMode, Debounce, HoldTime))
            {
                Console.WriteLine($"InputPointSpecification : False");
                return false;
            }
            return true;
        }

        public bool GetMpStatus(GetMpStatusDto dto)
        {
            short ScpId = _helperService.GetScpIdFromMac(dto.ScpMac);
            if (!_config.write.GetMpStatus(ScpId, (short)dto.MpNo, 1))
            {
                return false;
            }
            return true;
        }

        public bool RemoveMonitorPoint(RemoveMpDto dto)
        {
            try
            {
                var mp = _context.ArMonitorPoints.Where(d => d.ScpMac == dto.ScpMac && d.MpNo == dto.MpNo).FirstOrDefault();
                _context.ArMonitorPoints.Remove(mp);
                var nmp = _context.ArMpNo.Where(d => d.MpNo == dto.MpNo && d.ScpMac == dto.ScpMac).FirstOrDefault();
                if (nmp != null)
                {
                    nmp.IsAvailable = true;
                }
                _context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return false;
            }
        }

        #region Command Group

        public List<MpDto> GetMonitorPointList()
        {
            List<MpDto> dtos = new List<MpDto>();
            var mps = _context.ArMonitorPoints.Join(
                _context.ArSios,
                mp => mp.SioNo,
                s => s.SioNumber,
                (mp, s) => new
                {
                    mp,
                    s.ModeDescription,
                    s.Name
                }
                ).ToList();
            int i = 1;
            foreach (var c in mps)
            {
                dtos.Add(MapperHelper.MpToMpDto(i, c.mp, c.ModeDescription, c.Name));
                i++;
            }
            return dtos;

        }



        public bool CreateMonitorPoint(AddMpDto dto)
        {
            short ScpId = _helperService.GetScpIdFromMac(dto.ScpMac);
            short nmp = GetUniqueMpNo(dto.ScpMac);
            if (!_config.write.InputPointSpecification(ScpId, dto.SioNo, dto.IpNo, dto.LcvtMode, dto.Debounce, dto.HoldTime))
            {
                Console.WriteLine($"InputPointSpecification : False");
                return false;
            }

            if (!_config.write.MonitorPointConfiguration(ScpId, dto.SioNo, dto.IpNo, dto.LfCode,dto.Mode,dto.DelayEntry,dto.DelayExit,nmp))
            {
                Console.WriteLine($"MonitorPointConfiguration : False");
                return false;
            }

            if (!Save(dto.Name, _helperService.GetScpIpFromId(ScpId), dto.ScpMac, dto.SioNo, nmp,dto.IpNo, dto.LcvtMode, dto.LfCode, dto.DelayEntry, dto.DelayExit))
            {
                Console.WriteLine($"Save Monitor Point to Database : False");
                return false;
            }

            return true;

        }

        public void TriggerDeviceStatus(int ScpId, short first, short[] status)
        {
            string ScpMac = _helperService.GetMacFromId((short)ScpId);
            //GetOnlineStatus()
            var result = _hub.Clients.All.SendAsync("MpStatus", ScpMac, first, status);
        }

        public List<IpModeDto> GetIpModeList()
        {
            List<IpModeDto> dtos = new List<IpModeDto>();
            var datas = _context.ArIpModes.ToList();
            foreach (var data in datas) 
            {
                dtos.Add(MapperHelper.IpModeToIpModeDto(data));
            }
            return dtos;
        }

        public int GetMpRecAlloc(string ScpIp)
        {
            return _context.ArMonitorPoints.Where(p => p.ScpIp == ScpIp).Count();
        }


        #endregion
    }
}
