using HID.Aero.ScpdNet.Wrapper;
using HIDAeroService.AeroLibrary;
using HIDAeroService.Constant;
using HIDAeroService.Constants;
using HIDAeroService.Data;
using HIDAeroService.DTO;
using HIDAeroService.DTO.MonitorPoint;
using HIDAeroService.Entity;
using HIDAeroService.Helpers;
using HIDAeroService.Hubs;
using HIDAeroService.Mapper;
using HIDAeroService.Utility;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using MiNET.Entities;
using MiNET.Entities.Passive;


namespace HIDAeroService.Service.Impl
{
    public sealed class MonitorPointService(IHelperService<Sensor> helperService, AeroCommand command, AppDbContext context, IHubContext<AeroHub> hub, ILogger<MonitorPointService> logger) : IMonitorPointService
    {

        public async Task<ResponseDto<IEnumerable<MonitorPointDto>>> GetAsync()
        {
            var dtos = await context.MonitorPoints
                .AsNoTracking()
                .Select(x => MapperHelper.MonitorPointToDto(x))
                .ToArrayAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<MonitorPointDto>>(dtos);
        }


        public async Task<ResponseDto<IEnumerable<MonitorPointDto>>> GetByLocationAsync(short location)
        {
            var dtos = await context.MonitorPoints
                .AsNoTracking()
                .Where(x => x.LocationId == location)
                .Select(x => MapperHelper.MonitorPointToDto(x))
                .ToArrayAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<MonitorPointDto>>(dtos);
        }


        public async Task<ResponseDto<IEnumerable<short>>> GetAvailableIp(string mac, short sio)
        {
            var input = await context.Modules
                .AsNoTracking()
                .Where(mp => mp.ComponentId == sio && mp.MacAddress == mac)
                .Select(mp => mp.nInput)
                .FirstOrDefaultAsync();

            var sensors = await context.Sensors
                .AsNoTracking()
                .Where(x => x.ComponentId == sio && x.MacAddress == mac)
                .Select(x => x.InputNo)
                .ToArrayAsync();

            var rex = await context.RequestExits
                .AsNoTracking()
                .Where(x => x.ComponentId == sio && x.MacAddress == mac)
                .Select(x => x.InputNo)
                .ToArrayAsync();

            var mp = await context.MonitorPoints
                .AsNoTracking()
                .Where(x => x.ComponentId == sio && x.MacAddress == mac)
                .Select(x => x.InputNo)
                .ToArrayAsync();


            var unavailable = sensors
                .Union(rex)
                .Union(mp)
                .Distinct()
                .ToList();


            List<short> all = Enumerable.Range(0, input - 3).Select(i => (short)i).ToList();
            List<short> av = all.Except(unavailable).ToList();
            return ResponseHelper.SuccessBuilder<IEnumerable<short>>(av);
        }


        public async void TriggerDeviceStatus(int ScpId, short first, string status)
        {
            string ScpMac = await helperService.GetMacFromIdAsync((short)ScpId);
            //GetOnlineStatus()
            var result = hub.Clients.All.SendAsync("MpStatus", ScpMac, first, status);
        }



        public async Task<ResponseDto<bool>> MaskAsync(MonitorPointDto dto, bool IsMask)
        {
            var input = await context.MonitorPoints
                .Where(x => x.ComponentId == dto.ComponentId && x.MacAddress == dto.MacAddress)
                .FirstOrDefaultAsync();

            if (input is null) return ResponseHelper.NotFoundBuilder<bool>();

            var scpId = await context.Hardwares
                .AsNoTracking().Where(d => d.MacAddress == dto.MacAddress)
                .Select(x => x.ComponentId)
                .FirstOrDefaultAsync();

            if (!await command.MonitorPointMaskAsync((short)scpId, (short)input.ComponentId, IsMask ? 1 : 0))
            {
                return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(dto.MacAddress, Command.C306));
            }

            if (IsMask) input.IsMask = true;
            input.UpdatedDate = DateTime.Now;
            input.IsMask = IsMask ? true : false;
            context.MonitorPoints.Update(input);
            await context.SaveChangesAsync();
            return ResponseHelper.SuccessBuilder(true);
        }


        public async Task<ResponseDto<bool>> CreateAsync(MonitorPointDto dto)
        {
            
            var max = await context.SystemSettings
                .AsNoTracking()
                .Select(x => x.nMp)
                .FirstOrDefaultAsync();

            var componentId = await helperService.GetLowestUnassignedNumberAsync<MonitorPoint>(context, dto.MacAddress, max);
            if (componentId == -1) return ResponseHelper.ExceedLimit<bool>();

            var scpId = await helperService.GetIdFromMacAsync(dto.MacAddress);

            if (!await command.InputPointSpecificationAsync(scpId, dto.ModuleId, dto.InputNo, dto.InputMode, dto.Debounce, dto.HoldTime))
            {
                return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(dto.MacAddress, Command.C110));
            }


            if (!await command.MonitorPointConfigurationAsync(scpId, dto.ModuleId, dto.InputNo, dto.LogFunction, dto.MonitorPointMode, dto.DelayEntry, dto.DelayExit, componentId))
            {
                return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(dto.MacAddress, Command.C113));
            }


            var entity = MapperHelper.DtoToMonitorPoint(dto, componentId,DateTime.Now);
            await context.MonitorPoints.AddAsync(entity);
            await context.SaveChangesAsync();
            return ResponseHelper.SuccessBuilder(true);
        }


        public async Task<ResponseDto<bool>> DeleteAsync(string MacAddress, short ComponentId)
        {
            var input = await context.MonitorPoints
                .Where(x => x.MacAddress == MacAddress && x.ComponentId == ComponentId)
                .FirstOrDefaultAsync();

            if (input is null) return ResponseHelper.NotFoundBuilder<bool>();

            var scpId = await helperService.GetIdFromMacAsync(MacAddress);

            if (!await command.MonitorPointConfigurationAsync(scpId,-1, input.InputNo, input.LogFunction, input.MonitorPointMode, input.DelayEntry, input.DelayExit, ComponentId))
            {
                return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(input.MacAddress, Command.C113));
            }

            context.MonitorPoints.Remove(input);
            await context.SaveChangesAsync();
            return ResponseHelper.SuccessBuilder(true);
        }

        public async Task<ResponseDto<MonitorPointDto>> UpdateAsync(MonitorPointDto dto)
        {
            var input = await context.MonitorPoints
                .Where(x => x.MacAddress == dto.MacAddress && x.Id == dto.ComponentId)
                .FirstOrDefaultAsync();

            if (input is null) return ResponseHelper.NotFoundBuilder<MonitorPointDto>();

            var scpId = await helperService.GetIdFromMacAsync(dto.MacAddress);
            if (!await command.InputPointSpecificationAsync(scpId, dto.ModuleId, dto.InputNo, dto.InputMode, dto.Debounce, dto.HoldTime))
            {
                return ResponseHelper.UnsuccessBuilder<MonitorPointDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(dto.MacAddress, Command.C110));
            }

            if (!await command.MonitorPointConfigurationAsync(scpId, dto.ModuleId, dto.InputNo, dto.LogFunction, dto.MonitorPointMode, dto.DelayEntry, dto.DelayExit, dto.ComponentId))
            {
                return ResponseHelper.UnsuccessBuilder<MonitorPointDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(dto.MacAddress, Command.C113));
            }

            MapperHelper.UpdateMonitorPoint(input,dto);

            context.MonitorPoints.Update(input);
            await context.SaveChangesAsync();
            return ResponseHelper.SuccessBuilder<MonitorPointDto>(dto);
        }

        public async Task<ResponseDto<bool>> GetStatusAsync(string mac, short component)
        {
            var id = await helperService.GetIdFromMacAsync(mac);
            if(id == 0) return ResponseHelper.NotFoundBuilder<bool>();
            if (!await command.GetMpStatusAsync(id, component, 1))
            {
                return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.COMMAND_UNSUCCESS,MessageBuilder.Unsuccess(mac,Command.C405));
            }
            return ResponseHelper.SuccessBuilder<bool>(true);
        }

        public async Task<ResponseDto<IEnumerable<ModeDto>>> GetModeAsync(int param)
        {
            switch (param)
            {
                case 0:
                    var dtos = await context.InputModes.AsNoTracking().Select(x => new ModeDto
                    {
                        Name = x.Name,
                        Value = x.Value,
                        Description = x.Description,
                    }).ToArrayAsync();
                    return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dtos);
                case 1:
                    var d = await context.MonitorPointModes.AsNoTracking().Select(x => new ModeDto
                    {
                        Name = x.Name,
                        Value = x.Value,
                        Description = x.Description,
                    }).ToArrayAsync();
                    return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(d);
                default:
                    return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>([]);

            }

        }

        public async Task<ResponseDto<MonitorPointDto>> GetByIdAndMacAsync(string mac, short Id)
        {
            var dto = await context.MonitorPoints
                .AsNoTracking()
                .Where(x => x.MacAddress == mac && x.ComponentId == Id)
                .Select(x => MapperHelper.MonitorPointToDto(x))
                .FirstOrDefaultAsync();

            return ResponseHelper.SuccessBuilder(dto);
        }

        public async Task<ResponseDto<IEnumerable<MonitorPointDto>>> GetByIdAndMacAsync(string mac)
        {
            var dtos = await context.MonitorPoints
                .AsNoTracking()
                .Where(x => x.MacAddress == mac)
                .Select(x => MapperHelper.MonitorPointToDto(x))
                .ToArrayAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<MonitorPointDto>>(dtos);
        }
    }
}
