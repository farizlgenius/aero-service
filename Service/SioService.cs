using HIDAeroService.AeroLibrary;
using HIDAeroService.Data;
using HIDAeroService.Dto.Sio;
using HIDAeroService.Entity;
using HIDAeroService.Hubs;
using HIDAeroService.Mapper;
using Microsoft.AspNetCore.SignalR;

namespace HIDAeroService.Service
{
    public class SioService
    {
        private AppDbContext _context;
        private AeroLibMiddleware _config;
        private HelperService _helperService;
        private IHubContext<SioHub> _hub;
        public SioService(AppDbContext context,AeroLibMiddleware config,IHubContext<SioHub> hub,HelperService helperService) 
        {
            _helperService = helperService;
            _hub = hub;
            _config = config;
            _context = context;
        }

        public List<SioDto> GetSioList()
        {
            List<SioDto> dtos = new List<SioDto>();
            var sios = _context.ArSios.ToList();
            int i = 1;
            foreach (var sio in sios) 
            {
                dtos.Add(MapperHelper.SioToSioDto(i,sio));
                i++;
            }
            return dtos;
        }

        public void GetSioStatus(string ScpIp,short SioNo)
        {
            short ScpId = _helperService.GetScpIdFromIp(ScpIp);
            _config.write.GetSioStatus(ScpId,SioNo);
        }

        public void GetSioStatus(int ScpId, short SioNo)
        {
            _config.write.GetSioStatus((short)ScpId, SioNo);
        }


        public bool Save(string ScpMac,string scp_name,string sio_name,string scp_ip,short sio_number,short model,short address,short msp1_number,short port_number,short baud_rate,short n_protocol,short n_dialect)
        {
            short nInput, nOutput, nReaders;
            switch (model)
            {
                case 196:
                    nInput = 7;
                    nOutput = 4;
                    nReaders = 4;
                    break;
                case 193:
                    nInput = 7;
                    nOutput = 4;
                    nReaders = 4;
                    break;
                case 194:
                    nInput = 19;
                    nOutput = 2;
                    nReaders = 0;
                    break;
                case 195:
                    nInput = 5;
                    nOutput = 12;
                    nReaders = 0;
                    break;
                case 190:
                    nInput = 7;
                    nOutput = 4;
                    nReaders = 2;
                    break;
                case 191:
                    nInput = 19;
                    nOutput = 2;
                    nReaders = 0;
                    break;
                case 192:
                    nInput = 5;
                    nOutput = 12;
                    nReaders = 0;
                    break;
                default:
                    nInput = 0;
                    nOutput = 0;
                    nReaders = 0;
                    break;
            }

            try
            {
                ArSioNo a = new ArSioNo();
                a.ScpIp = scp_ip;
                a.SioNo = sio_number;
                a.ScpMac = ScpMac;
                a.Port = 0;
                a.IsAvailable = false;
                _context.ArSioNo.Add(a);
                ArSio s = new ArSio();
                s.Name = sio_name;
                s.ScpName = scp_name;
                s.ScpIp = scp_ip;
                s.SioNumber = sio_number;
                s.NInput = nInput;
                s.NOutput = nOutput;
                s.NReader = nReaders;
                s.Model = model;
                s.ModeDescription = Description.GetSioModelDesc(model);
                s.Address = address;
                s.Msp1No = msp1_number;
                s.PortNo = port_number;
                s.BaudRate = baud_rate;
                s.NProtocol = n_protocol;
                s.NDialect = n_dialect;
                s.ScpMac = ScpMac;
                s.CreatedDate = DateTime.Now;
                s.UpdatedDate = DateTime.Now;
                _context.ArSios.Add(s);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.ToString());
                return false;
            }

        }

        public void TriggerDeviceStatus(int ScpId,short SioNo,short Status, short Tamper, short Ac,short Batt )
        {
            string ScpIp = _helperService.GetScpIpFromId((short)ScpId);
            //GetOnlineStatus()
            var result = _hub.Clients.All.SendAsync("SioStatus",ScpIp,SioNo,Status, Tamper, Ac, Batt);
        }

        public List<SioDto> GetSioListFromMac(string mac)
        {
            List<SioDto> d = new List<SioDto>();
            var datas = _context.ArSios.Where(p => p.ScpMac == mac).ToList();
            int i = 1;
            foreach (var data in datas) 
            {
                d.Add(MapperHelper.SioToSioDto(i,data));
                i++;
            }
            return d;
        }

        public int GetRecAllocSio(string ScpIp)
        {
            return _context.ArSios.Where(p => p.ScpIp == ScpIp).Count();
        }
    }
}
