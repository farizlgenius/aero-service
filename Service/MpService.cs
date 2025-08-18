using HID.Aero.ScpdNet.Wrapper;
using HIDAeroService.Data;
using HIDAeroService.Dto;
using HIDAeroService.Entity;
using HIDAeroService.Hubs;
using HIDAeroService.Mapper;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace HIDAeroService.Service
{
    public sealed class MpService
    {
        private readonly AppDbContext _context;
        private readonly HelperService _helperService;
        private readonly AppConfigData _config;
        private readonly IHubContext<MpHub> _hub;
        private readonly ILogger<MpService> _logger;
        public MpService(HelperService helperService,AppConfigData config,AppDbContext context,IHubContext<MpHub> hub,ILogger<MpService> logger) 
        {
            _logger = logger;
            _hub = hub;
            _config = config;
            _context = context;
            _helperService = helperService;
        }

        public short GetUniqueMpNo(string ScpIp)
        {

            short highestMpNumber;
            if (!_context.ar_mp_no.Any(p => p.scp_ip == ScpIp))
                return 0;

            if (_context.ar_mp_no.Any(p => p.is_available == true && p.scp_ip == ScpIp))
            {
                highestMpNumber = _context.ar_mp_no.Where(p => p.is_available == true && p.scp_ip == ScpIp).Select(p => p.mp_number).First();
                return highestMpNumber;
            }
            else
            {
                highestMpNumber = _context.ar_mp_no.Where(p => p.is_available == false && p.scp_ip == ScpIp).Max(p => p.mp_number);
                highestMpNumber += 1;
                return highestMpNumber;
            }

        }

        public bool SaveMpToDb(string name,short scp_id,short sio_number,short mp_number,short ip_number,short icvt_mode,short lf_code,short delay_entry,short delay_exit)
        {
            try
            {
                ar_monitor_point m = new ar_monitor_point();
                m.name = name;
                m.scp_ip = _helperService.GetScpIpFromId(scp_id); 
                m.sio_number = sio_number;
                m.mp_number = mp_number;
                m.ip_number = ip_number;
                m.icvt_num = icvt_mode;
                m.lf_code = lf_code;
                m.delay_entry = delay_entry;
                m.delay_exit = delay_exit;
                _context.ar_monitor_point.Add(m);

                ar_n_mp n = new ar_n_mp();
                n.scp_ip = _helperService.GetScpIpFromId(scp_id);
                n.sio_number = sio_number;
                n.mp_number = mp_number;
                n.ip_number = ip_number;
                n.is_available = false;
                _context.ar_mp_no.Add(n);
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
            var input = _context.ar_sios.Where(mp => mp.sio_number == sio).Select(mp => mp.n_inputs).First();
            var unavailable = _context.ar_monitor_point.Where(mp => mp.sio_number == sio).Select(mp => mp.ip_number).ToList();
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

        public bool GetMpStatus(string ScpIp, short MpNo)
        {
            short ScpId = _helperService.GetScpIdFromIp(ScpIp);
            if (!_config.write.GetMpStatus(ScpId, MpNo, 1))
            {
                return false;
            }
            return true;
        }

        public bool RemoveMonitorPoint(string ScpIp, short MpNo)
        {
            try
            {
                var mp = _context.ar_monitor_point.Where(d => d.scp_ip == ScpIp && d.mp_number == MpNo).FirstOrDefault();
                _context.ar_monitor_point.Remove(mp);
                var nmp = _context.ar_mp_no.Where(d => d.mp_number == MpNo && d.scp_ip == ScpIp).FirstOrDefault();
                if (nmp != null)
                {
                    nmp.is_available = true;
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
            var mps = _context.ar_monitor_point.Join(
                _context.ar_sios,
                mp => mp.sio_number,
                s => s.sio_number,
                (mp, s) => new
                {
                    mp,
                    s.model_desc,
                    s.name
                }
                ).ToList();
            int i = 1;
            foreach (var c in mps)
            {
                dtos.Add(MapperHelper.MpToMpDto(i, c.mp, c.model_desc, c.name));
                i++;
            }
            return dtos;

        }



        public bool CreateMonitorPoint(string name,string ScpIp, short SioNo, short InputNo, short IcvtMode,short Debounce,short HoldTime,short LfCode,short Mode,short DelayEntry,short DelayExit,MpService mp)
        {
            short ScpID = _helperService.GetScpIdFromIp(ScpIp);
            short nmp = mp.GetUniqueMpNo(ScpIp);
            if (!_config.write.InputPointSpecification(ScpID, SioNo, InputNo, IcvtMode, Debounce, HoldTime))
            {
                Console.WriteLine($"InputPointSpecification : False");
                return false;
            }

            if (!_config.write.MonitorPointConfiguration(ScpID, SioNo, InputNo,LfCode,Mode,DelayEntry,DelayExit,nmp))
            {
                Console.WriteLine($"MonitorPointConfiguration : False");
                return false;
            }

            if (!SaveMpToDb(name,ScpID,SioNo,nmp,InputNo, IcvtMode,LfCode,DelayEntry,DelayExit))
            {
                Console.WriteLine($"Save Monitor Point to Database : False");
                return false;
            }

            return true;

        }

        public void TriggerDeviceStatus(int ScpId, short first, short count, short[] status)
        {
            string ScpIp = _helperService.GetScpIpFromId((short)ScpId);
            //GetOnlineStatus()
            var result = _hub.Clients.All.SendAsync("MpStatus", ScpIp, first, count, status);
        }

        public List<IpModeDto> GetIpModeList()
        {
            List<IpModeDto> dtos = new List<IpModeDto>();
            var datas = _context.ar_ip_modes.ToList();
            foreach (var data in datas) 
            {
                dtos.Add(MapperHelper.IpModeToIpModeDto(data));
            }
            return dtos;
        }

        public int GetMpRecAlloc(string ScpIp)
        {
            return _context.ar_monitor_point.Where(p => p.scp_ip == ScpIp).Count();
        }


        #endregion
    }
}
