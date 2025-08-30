using HID.Aero.ScpdNet.Wrapper;
using HIDAeroService.Data;
using HIDAeroService.Dto.Cp;
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
        private readonly AeroLibMiddleware _config;
        private readonly IHubContext<CpHub> _hub;

        public CpService(AppDbContext context, AeroLibMiddleware config,HelperService helperService,ILogger<CpService> logger,IHubContext<CpHub> hub)
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
            var cps = _context.ArControlPoints.Join(
                _context.ArSios,
                cp => cp.SioNo,
                s => s.SioNumber,
                (cp,s) =>new
                {
                    cp,
                    s.ModeDescription,
                    s.Name
                }
                ).ToList(); 
            int i = 1;
            foreach(var c in cps)
            {
                dtos.Add(MapperHelper.CpToCpDto(i,c.cp,c.ModeDescription,c.Name));
                i++;
            }
            return dtos;
        }


  
        public bool Save(AddCpDto cpDto,short cp_number)
        {
            try
            {
                var isNewCpNo = _context.ArCpNo.Where(p => p.CpNo == cp_number).FirstOrDefault();
                if (isNewCpNo == null)
                {
                    ArCpNo ncp = MapperHelper.AddCpDtoTonCp(cpDto, cp_number);
                    _context.ArCpNo.Add(ncp);
                }
                else
                {
                    isNewCpNo.IsAvailable = false;
                }
                
                ArControlPoint cp = MapperHelper.AddCpDtoToCp(cpDto, cp_number);
                _context.ArControlPoints.Add(cp);
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
            short _scpId = _helperService.GetScpIdFromMac(cpTriggerDto.ScpMac);
            if (!TriggerControlPoint(_scpId, cpTriggerDto.CpNo, cpTriggerDto.Command))
            {
                return false;
            }
            return true;
        }

        public List<OpModeDto> GetOpModeList()
        {
            var op = _context.ArOpModes.ToList();
            List < OpModeDto > dtos = new List<OpModeDto> ();
            foreach (var o in op)
            {
                dtos.Add(MapperHelper.OpModeToOpModeDto(o));
            }
            return dtos;
        }

        public List<short> GetAvailableOp(short sio)
        {
            var output = _context.ArSios.Where(cp => cp.SioNumber == sio).Select(cp => cp.NOutput).First();
            var unavailable = _context.ArControlPoints.Where(cp => cp.SioNo == sio).Select(cp => cp.OpNo).ToList();
            List<short> all = Enumerable.Range(0, output).Select(i => (short)i).ToList();
            return all.Except(unavailable).ToList();
        }


        public short GetUniqueCpNo(string ScpMac)
        {

            short highestCpNumber;
            if (!_context.ArCpNo.Any(p => p.ScpMac == ScpMac))
                return 0;

            if (_context.ArCpNo.Any(p => p.IsAvailable == true && p.ScpMac == ScpMac))
            {
                highestCpNumber = _context.ArCpNo.Where(p => p.IsAvailable == true && p.ScpMac == ScpMac).Select(p => p.CpNo).First();
                return highestCpNumber;
            }
            else
            {
                highestCpNumber = _context.ArCpNo.Where(p => p.IsAvailable == false && p.ScpMac == ScpMac).Max(p => p.CpNo);
                highestCpNumber+=1;
                return highestCpNumber;
            }

        }

        public bool GetCpStatus(GetCpStatusDto dto)
        {
            short _scpId = _helperService.GetScpIdFromMac(dto.ScpMac);
            if (!_config.write.GetCpStatus(_scpId, dto.CpNo,1))
            {
                return false;
            }
            return true;
        }


        public void TriggerDeviceStatus(string ScpMac,short first, short[] status)
        {
            //GetOnlineStatus()
            var result = _hub.Clients.All.SendAsync("CpStatus", ScpMac, first,status);
        }




        #region Command Group

        public bool CreateControlPoint(AddCpDto cpDto)
        {
            short ScpId = _helperService.GetScpIdFromMac(cpDto.ScpMac);
            short cp_number = GetUniqueCpNo(cpDto.ScpMac);

            if (_config.write.OutputPointSpecification(ScpId, cpDto.SioNumber, cpDto.OpNumber, cpDto.Mode) == -1)
            {
                return false;
            }

            if (_config.write.ControlPointConfiguration(ScpId, cpDto.SioNumber, cp_number, cpDto.OpNumber,cpDto.DefaultPulseTime) == -1)
            {
                return false;
            }

            if (!Save(cpDto, cp_number))
            {
                _logger.LogError("Save Cp to database : False");
                return false;
            }

            return true;
        }

        public bool RemoveControlPoint(RemoveCpDto dto)
        {
            try
            {
                var cp = _context.ArControlPoints.Where(d => d.ScpMac == dto.ScpMac && d.CpNo == dto.CpNo).FirstOrDefault();
                _context.ArControlPoints.Remove(cp);
                var ncp = _context.ArCpNo.Where(d => d.CpNo == dto.CpNo && d.ScpMac == dto.ScpMac).FirstOrDefault();
                if(ncp != null)
                {
                    ncp.IsAvailable = true;
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

        public int GetCpRecAlloc(string ScpMac)
        {
            return _context.ArControlPoints.Where(p => p.ScpMac == ScpMac).Count();
        }

        #endregion


    }
}
