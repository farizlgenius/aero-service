using AutoMapper;
using HIDAeroService.AeroLibrary;
using HIDAeroService.Constant;
using HIDAeroService.Constants;
using HIDAeroService.Data;
using HIDAeroService.DTO;
using HIDAeroService.DTO.ControlPoint;
using HIDAeroService.DTO.Output;
using HIDAeroService.Entity;
using HIDAeroService.Entity.Interface;
using HIDAeroService.Helpers;
using HIDAeroService.Hubs;
using HIDAeroService.Mapper;
using HIDAeroService.Model;
using HIDAeroService.Utility;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using MiNET.Entities;
using System.ComponentModel;
using System.Net;

namespace HIDAeroService.Service.Impl
{
    public sealed class ControlPointService(AppDbContext context,IHelperService<Strike> helperService,AeroCommand command,ILogger<ControlPointService> logger,IMapper mapper,IHubContext<AeroHub> hub) : IControlPointService 
    {
        public async Task<ResponseDto<IEnumerable<ControlPointDto>>> GetAsync()
        {
            var dtos = await context.ControlPoints
                .AsNoTracking()
                .Select(x => MapperHelper.ControlPointToDto(x))
                .ToArrayAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<ControlPointDto>>(dtos);
        }

        public async Task<ResponseDto<IEnumerable<ControlPointDto>>> GetByLocationAsync(short location)
        {
            var dtos = await context.ControlPoints
                .AsNoTracking()
                .Where(x => x.LocationId == location)
                .Select(x => MapperHelper.ControlPointToDto(x))
                .ToArrayAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<ControlPointDto>>(dtos);
        }

        private async Task<ResponseDto<IEnumerable<ModeDto>>> GetOfflineModeAsync()
        {
            var dtos = await context.OutputOfflineModes.AsNoTracking().Select(x => new ModeDto 
            {
                Name = x.Name,
                Value = x.Value,
                Description = x.Description,
            }).ToArrayAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dtos);
        }


        private async Task<ResponseDto<IEnumerable<ModeDto>>> GetRelayModeAsync()
        {
            var dtos = await context.RelayModes.AsNoTracking().Select(x => new ModeDto 
            {
                Name = x.Name,
                Value = x.Value,
                Description = x.Description,
            }).ToArrayAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dtos);
        }


        public async Task<ResponseDto<bool>> ToggleAsync(ToggleControlPointDto dto)
        {
            List<string> errors = new List<string>();
            var id = await helperService.GetIdFromMacAsync(dto.macAddress);
            if(id == 0) return ResponseHelper.NotFoundBuilder<bool>();
            if (!await command.ControlPointCommandAsync(id, dto.ComponentId, dto.Command))
            {
                return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.COMMAND_UNSUCCESS,MessageBuilder.Unsuccess(dto.macAddress,Command.C307));
            }
            return ResponseHelper.SuccessBuilder(true);
        }

        public async Task<ResponseDto<IEnumerable<short>>> GetAvailableOpAsync(string mac, short ModuleId)
        {
            var ops = await context.Modules
                .AsNoTracking()
                .Where(sio => sio.MacAddress == mac && sio.ComponentId == ModuleId)
                .Select(cp => cp.nOutput)
                .FirstOrDefaultAsync();

            var strk = await context.Strikes
                .AsNoTracking()
                .Where(x => x.ModuleId == ModuleId && x.MacAddress == mac)
                .Select(x => x.OutputNo)
                .ToArrayAsync();

            var cp = await context.ControlPoints
                .AsNoTracking()
                .Where(x => x.ModuleId == ModuleId && x.MacAddress == mac)
                .Select(x => x.OutputNo)
                .ToArrayAsync();


            var unavailable = strk
                .Union(cp)
                .Distinct()
                .ToList();

            List<short> all = Enumerable.Range(0, ops).Select(x => (short)x).ToList();
            return ResponseHelper.SuccessBuilder<IEnumerable<short>>(all.Except(unavailable).ToList());
        }


        public void TriggerDeviceStatus(string ScpMac, int first, string status)
        {
            //GetOnlineStatus()
            var result = hub.Clients.All.SendAsync("CpStatus", ScpMac, first, status);
        }



        public async Task<ResponseDto<bool>> CreateOutputAsync(ControlPointDto dto)
        {
            short scpId = await helperService.GetIdFromMacAsync(dto.MacAddress);

            var max = await context.SystemSettings.AsNoTracking().Select(x => x.nCp).FirstOrDefaultAsync();
            var componentId = await helperService.GetLowestUnassignedNumberAsync<MonitorPoint>(context, dto.MacAddress, max);

            short modeNo = await context.OutputModes
                .AsNoTracking()
                .Where(x => x.OfflineMode == dto.OfflineMode && x.RelayMode == dto.RelayMode)
                .Select(x => x.Value).FirstOrDefaultAsync();

            if (!await command.OutputPointSpecificationAsync(scpId, dto.ModuleId, dto.OutputNo, modeNo))
            {
                return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.COMMAND_UNSUCCESS,MessageBuilder.Unsuccess(dto.MacAddress, Command.C111));
            }


            if (componentId == -1) return ResponseHelper.ExceedLimit<bool>();

            if (!await command.ControlPointConfigurationAsync(scpId, dto.ModuleId, (short)componentId, dto.OutputNo, dto.DefaultPulse))
            {
                return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(dto.MacAddress, Command.C114));

            }


            var output = MapperHelper.DtoToControlPoint(dto,componentId,DateTime.Now);
            await context.ControlPoints.AddAsync(output);
            await context.SaveChangesAsync();
            return ResponseHelper.SuccessBuilder(true);
        }

        public async Task<ResponseDto<bool>> DeleteAsync(string mac, short Id)
        {
            var output = await context.ControlPoints
                .FirstOrDefaultAsync(x => x.ComponentId == Id && x.MacAddress == mac);

            if (output is null) return ResponseHelper.NotFoundBuilder<bool>();

            var scpId = await helperService.GetIdFromMacAsync(mac);

            if (!await command.ControlPointConfigurationAsync(scpId, -1, (short)output.ComponentId, output.OutputNo, output.DefaultPulse))
            {
                return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.COMMAND_UNSUCCESS,MessageBuilder.Unsuccess(mac,Command.C114));
            }

            context.ControlPoints.Remove(output);
            await context.SaveChangesAsync();
            return ResponseHelper.SuccessBuilder(true);
        }

        public async Task<ResponseDto<ControlPointDto>> UpdateAsync(ControlPointDto dto)
        {
            var output = await context.ControlPoints
                .FirstOrDefaultAsync(x => x.MacAddress == dto.MacAddress && x.ComponentId == dto.ComponentId);

            if (output is null) return ResponseHelper.NotFoundBuilder<ControlPointDto>();

            var scpId = await helperService.GetIdFromMacAsync(dto.MacAddress);
            short modeNo = await context.OutputModes.AsNoTracking().Where(x => x.OfflineMode == dto.OfflineMode && x.RelayMode == dto.RelayMode).Select(x => x.Value).FirstOrDefaultAsync();
            if (!await command.OutputPointSpecificationAsync(scpId, dto.ModuleId, dto.OutputNo, modeNo))
            {
                return ResponseHelper.UnsuccessBuilder<ControlPointDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(dto.MacAddress, Command.C111));
            }

            if (!await command.ControlPointConfigurationAsync(scpId, dto.ModuleId, (short)dto.ComponentId, dto.OutputNo, dto.DefaultPulse))
            {
                return ResponseHelper.UnsuccessBuilder<ControlPointDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(dto.MacAddress, Command.C114));
            }


            MapperHelper.UpdateControlPoint(output,dto);

            context.ControlPoints.Update(output);
            await context.SaveChangesAsync();
            return ResponseHelper.SuccessBuilder(dto);
        }

        public async Task<ResponseDto<bool>> GetStatusAsync(string mac, short component)
        {
            var id = await helperService.GetIdFromMacAsync(mac);
            if (!await command.GetCpStatus(id, component, 1))
            {
                return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.COMMAND_UNSUCCESS,MessageBuilder.Unsuccess(mac,Command.C406));
            }
            return ResponseHelper.SuccessBuilder(true);
        }

        public async Task<ResponseDto<IEnumerable<ModeDto>>> GetModeAsync(int param)
        {
            switch (param)
            {
                case 0:
                    return await GetOfflineModeAsync();
                case 1:
                    return await GetRelayModeAsync();
                default:
                    return ResponseHelper.NotFoundBuilder<IEnumerable<ModeDto>>();
            }
        }

        public async Task<ResponseDto<ControlPointDto>> GetByMacAndIdAsync(string Mac, short ComponentId)
        {
            var dto = await context.ControlPoints
                .Where(x => x.MacAddress == Mac && x.ComponentId == ComponentId)
                .Select(x => MapperHelper.ControlPointToDto(x)).FirstOrDefaultAsync();

            return ResponseHelper.SuccessBuilder(dto);
        }
    }
}
