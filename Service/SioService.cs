using HIDAeroService.AeroLibrary;
using HIDAeroService.Data;
using HIDAeroService.Dto;
using HIDAeroService.Entity;
using HIDAeroService.Hubs;
using HIDAeroService.Mapper;
using Microsoft.AspNetCore.SignalR;

namespace HIDAeroService.Service
{
    public class SioService
    {
        private AppDbContext _context;
        private AppConfigData _config;
        private HelperService _helperService;
        private IHubContext<SioHub> _hub;
        public SioService(AppDbContext context,AppConfigData config,IHubContext<SioHub> hub,HelperService helperService) 
        {
            _helperService = helperService;
            _hub = hub;
            _config = config;
            _context = context;
        }

        public List<SioDto> GetSioList()
        {
            List<SioDto> dtos = new List<SioDto>();
            var sios = _context.ar_sios.ToList();
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


        public bool SaveToDataBase(string scp_name,string sio_name,string scp_ip,short sio_number,short model,short address,short msp1_number,short port_number,short baud_rate,short n_protocol,short n_dialect)
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
                ar_n_sio a = new ar_n_sio();
                a.scp_ip = scp_ip;
                a.sio_number = sio_number;
                a.port = 0;
                a.is_available = false;
                _context.ar_sio_no.Add(a);
                ar_sio s = new ar_sio();
                s.name = sio_name;
                s.scp_name = scp_name;
                s.scp_ip = scp_ip;
                s.sio_number = sio_number;
                s.n_inputs = nInput;
                s.n_outputs = nOutput;
                s.n_readers = nReaders;
                s.model = model;
                s.model_desc = Description.GetSioModelDesc(model);
                s.address = address;
                s.msp1_number = msp1_number;
                s.port_number = port_number;
                s.baud_rate = baud_rate;
                s.n_protocol = n_protocol;
                s.n_dialect = n_dialect;
                _context.ar_sios.Add(s);
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

        public List<SioDto> GetSioListFromIp(string ip)
        {
            List<SioDto> d = new List<SioDto>();
            var datas = _context.ar_sios.Where(p => p.scp_ip == ip).ToList();
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
            return _context.ar_sios.Where(p => p.scp_ip == ScpIp).Count();
        }
    }
}
