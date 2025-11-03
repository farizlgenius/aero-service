using AutoMapper;
using HIDAeroService.AeroLibrary;
using HIDAeroService.Constant;
using HIDAeroService.Constants;
using HIDAeroService.Data;
using HIDAeroService.DTO;
using HIDAeroService.DTO.Module;
using HIDAeroService.DTO.Reader;
using HIDAeroService.DTO.Output;
using HIDAeroService.Entity;
using HIDAeroService.Helpers;
using HIDAeroService.Hubs;
using HIDAeroService.Mapper;
using HIDAeroService.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using MiNET.Entities;
using System.Collections.Generic;
using System.Net;

namespace HIDAeroService.Service.Impl
{
    public class ModuleService(AppDbContext context, AeroCommand command, IHubContext<SioHub> hub, IHelperService<Module> helperService, IMapper mapper, ILogger<ModuleService> logger) : IModuleService
    {

        public async Task<ResponseDto<IEnumerable<ModuleDto>>> GetAsync()
        {
            var dtos = await context.Modules
                .AsNoTracking()
                .Include(d => d.Readers)
                .Include(d => d.Sensors)
                .Include(d => d.Strikes)
                .Include(d => d.ControlPoints)
                .Include(d => d.MonitorPoints)
                .Include(d => d.RequestExits)
                .Select(d => MapperHelper.ModuleToDto(d)).ToArrayAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<ModuleDto>>(dtos);
        }

        public void GetSioStatus(int ScpId, int SioNo)
        {
            command.GetSioStatus((short)ScpId, (short)SioNo);
        }

        public void TriggerDeviceStatus(int ScpId, short SioNo, string Status, string Tamper, string Ac, string Batt)
        {
            string mac = helperService.GetMacFromId((short)ScpId);
            //GetOnlineStatus()
            hub.Clients.All.SendAsync("SioStatus", mac, SioNo, Status, Tamper, Ac, Batt);
        }

        public async Task<ResponseDto<bool>> GetStatusAsync(string mac, short Id)
        {
            var entity = await context.Modules.AsNoTracking().Where(x => x.MacAddress == mac && x.Id == Id).FirstOrDefaultAsync();
            if (entity is null) return ResponseHelper.NotFoundBuilder<bool>();
            int id = await context.Hardwares.Where(d => d.MacAddress == mac).Select(d => d.Id).FirstOrDefaultAsync();
            if (!await command.GetSioStatusAsync((short)id, Id))
            {
                return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.COMMAND_UNSUCCESS,MessageBuilder.Unsuccess(mac,Command.C404));
            }
            return ResponseHelper.SuccessBuilder<bool>(true);

        }

        public async Task<ResponseDto<IEnumerable<ModuleDto>>> GetByMacAsync(string mac)
        {
            var entities = await context.Modules.Where(x => x.MacAddress == mac).ToArrayAsync();
            if (entities.Count() == 0) return ResponseHelper.NotFoundBuilder<IEnumerable < ModuleDto>> ();
            List<ModuleDto> dtos = new List<ModuleDto>();
            foreach(var entity in entities)
            {
                dtos.Add(mapper.Map<ModuleDto>(entity));
            }
            return ResponseHelper.SuccessBuilder<IEnumerable<ModuleDto>>(dtos);
        }

        public async Task<ResponseDto<ModuleDto>> CreateAsync(ModuleDto dto)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseDto<ModuleDto>> DeleteAsync(string mac, short component)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseDto<ModuleDto>> UpdateAsync(ModuleDto dto)
        {
            throw new NotImplementedException();
        }


        public async Task<ResponseDto<IEnumerable<ModeDto>>> GetModeAsync(int param)
        {
            throw new NotImplementedException();
        }


        public async Task<ResponseDto<ModuleDto>> GetByComponentAsync(string mac, short component)
        {
            throw new NotImplementedException();
        }
    }
}
