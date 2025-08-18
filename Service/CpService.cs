using HID.Aero.ScpdNet.Wrapper;
using HIDAeroService.Data;
using HIDAeroService.Dto;
using HIDAeroService.Entity;
using HIDAeroService.Hubs;
using HIDAeroService.Mapper;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Linq;

namespace HIDAeroService.Service
{
    public sealed class CpService
    {
        private readonly ILogger<CpService> _logger;
        private readonly AppDbContext _context;
        private readonly HelperService _helperService;
        private readonly AppConfigData _config;
        private readonly IHubContext<CpHub> _hub;

        public CpService(AppDbContext context, AppConfigData config,HelperService helperService,ILogger<CpService> logger,IHubContext<CpHub> hub)
        {
            _hub = hub;
            _logger = logger;
            _helperService = helperService;
            _context = context;
            _config = config;
        }

        public List<CpDto> GetControlPointList()
        {
            List<CpDto> dtos = new List<CpDto>();
            var cps = _context.ar_control_point.Join(
                _context.ar_sios,
                cp => cp.sio_number,
                s => s.sio_number,
                (cp,s) =>new
                {
                    cp,
                    s.model_desc,
                    s.name
                }
                ).ToList(); 
            int i = 1;
            foreach(var c in cps)
            {
                dtos.Add(MapperHelper.CpToCpDto(i,c.cp,c.model_desc,c.name));
                i++;
            }
            return dtos;
        }


  
        public bool SaveToDatabase(AddCpDto cpDto,short cp_number)
        {
            try
            {
                var isNewCpNo = _context.ar_cp_no.Where(p => p.cp_number == cp_number).FirstOrDefault();
                if (isNewCpNo == null)
                {
                    ar_n_cp ncp = MapperHelper.AddCpDtoTonCp(cpDto, cp_number);
                    _context.ar_cp_no.Add(ncp);
                }
                else
                {
                    isNewCpNo.is_available = false;
                }
                
                ar_control_point cp = MapperHelper.AddCpDtoToCp(cpDto, cp_number);
                _context.ar_control_point.Add(cp);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            
        }

        public bool TriggerCP(CpTriggerDto cpTriggerDto)
        {
            short scp_id = _helperService.GetScpIdFromIp(cpTriggerDto.ScpIp);
            if (!TriggerControlPoint(scp_id, cpTriggerDto.CpNumber, cpTriggerDto.Command))
            {
                return false;
            }
            return true;
        }

        public List<OpModeDto> GetOpModeList()
        {
            var op = _context.ar_op_modes.ToList();
            List < OpModeDto > dtos = new List<OpModeDto> ();
            foreach (var o in op)
            {
                dtos.Add(MapperHelper.OpModeToOpModeDto(o));
            }
            return dtos;
        }

        public List<short> GetAvailableOp(short sio)
        {
            var output = _context.ar_sios.Where(cp => cp.sio_number == sio).Select(cp => cp.n_outputs).First();
            var unavailable = _context.ar_control_point.Where(cp => cp.sio_number == sio).Select(cp => cp.op_number).ToList();
            List<short> all = Enumerable.Range(0, output).Select(i => (short)i).ToList();
            return all.Except(unavailable).ToList();
        }


        public short GetUniqueCpNo(string ScpIp)
        {

            short highestCpNumber;
            if (!_context.ar_cp_no.Any(p => p.scp_ip == ScpIp))
                return 0;

            if (_context.ar_cp_no.Any(p => p.is_available == true && p.scp_ip == ScpIp))
            {
                highestCpNumber = _context.ar_cp_no.Where(p => p.is_available == true && p.scp_ip == ScpIp).Select(p => p.cp_number).First();
                return highestCpNumber;
            }
            else
            {
                highestCpNumber = _context.ar_cp_no.Where(p => p.is_available == false && p.scp_ip == ScpIp).Max(p => p.cp_number);
                highestCpNumber+=1;
                return highestCpNumber;
            }

        }

        public bool GetCpStatus(string ScpIp,short CpNo)
        {
            short ScpId = _helperService.GetScpIdFromIp(ScpIp);
            if (!_config.write.GetCpStatus(ScpId, CpNo,1))
            {
                return false;
            }
            return true;
        }


        public void TriggerDeviceStatus(int ScpId,short first,short count, short[] status)
        {
            string ScpIp = _helperService.GetScpIpFromId((short)ScpId);
            //GetOnlineStatus()
            var result = _hub.Clients.All.SendAsync("CpStatus", ScpIp,first,count,status);
        }




        #region Command Group

        public bool CreateControlPoint(AddCpDto cpDto)
        {
            short scp_id = _helperService.GetScpIdFromIp(cpDto.ScpIp);
            short cp_number = GetUniqueCpNo(cpDto.ScpIp);

            if (!_config.write.OutputPointSpecification(scp_id, cpDto.SioNumber, cpDto.OpNumber, cpDto.Mode))
            {
                return false;
            }

            if (!_config.write.ControlPointConfiguration(scp_id, cpDto.SioNumber, cp_number, cpDto.OpNumber,cpDto.DefaultPulseTime))
            {
                return false;
            }

            if (!SaveToDatabase(cpDto, cp_number))
            {
                _logger.LogError("Save Cp to database : False");
                return false;
            }

            return true;
        }

        public bool RemoveControlPoint(string ScpIp,short CpNo)
        {
            try
            {
                var cp = _context.ar_control_point.Where(d => d.scp_ip == ScpIp && d.cp_number == CpNo).FirstOrDefault();
                _context.ar_control_point.Remove(cp);
                var ncp = _context.ar_cp_no.Where(d => d.cp_number == CpNo && d.scp_ip == ScpIp).FirstOrDefault();
                if(ncp != null)
                {
                    ncp.is_available = true;
                }
                _context.SaveChanges();
                return true;
            }catch(Exception e)
            {
                _logger.LogError(e.Message);
                return false;
            }
        }


        public bool TriggerControlPoint(short scp_id, short cp_number, short command)
        {

            if (!_config.write.ControlPointCommand(scp_id, cp_number, command))
            {
                _logger.LogError("ControlPointCommand : False");
                return false;
            }
            _logger.LogInformation("ControlPointCommand : True");
            return true;
        }

        public int GetCpRecAlloc(string ScpIp)
        {
            return _context.ar_control_point.Where(p => p.scp_ip == ScpIp).Count();
        }

        #endregion


    }
}
