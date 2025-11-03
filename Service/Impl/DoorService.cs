using AutoMapper;
using HIDAeroService.AeroLibrary;
using HIDAeroService.Constant;
using HIDAeroService.Constants;
using HIDAeroService.Data;
using HIDAeroService.DTO;
using HIDAeroService.DTO.Acr;
using HIDAeroService.Entity;
using HIDAeroService.Helpers;
using HIDAeroService.Hubs;
using HIDAeroService.Mapper;
using HIDAeroService.Utility;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;


namespace HIDAeroService.Service.Impl
{
    public class DoorService(AppDbContext context, AeroMessage read, AeroCommand command, IHelperService<Door> helperService, IHubContext<AeroHub> hub, IMapper mapper) : IDoorService
    {
        public async Task<ResponseDto<IEnumerable<DoorDto>>> GetAsync()
        {
            var dtos = await context.Doors
                .AsNoTracking()
                .Include(x => x.Readers)
                .Include(x => x.Sensor)
                .Include(x=>x.RequestExits)
                .Include(x=>x.Strk)
                .Select(x => MapperHelper.DoorToDto(x))
                .ToArrayAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<DoorDto>>(dtos);
        }

        public async Task<ResponseDto<IEnumerable<DoorDto>>> GetByMacAsync(string mac)
        {
            var dtos = await context.Doors
                .AsNoTracking()
                .Include(x => x.Readers)
                .Include(x => x.Sensor)
                .Include(x => x.RequestExits)
                .Include(x => x.Strk)
                .Where(x => x.MacAddress == mac)
                .Select(x => MapperHelper.DoorToDto(x))
                .ToArrayAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<DoorDto>>(dtos);
        }

        public async Task<ResponseDto<DoorDto>> GetByComponentAsync(string mac, short component)
        {
            var dto = await context.Doors
                .AsNoTracking()
                .Include(x => x.Readers)
                .Include(x => x.Sensor)
                .Include(x => x.RequestExits)
                .Include(x => x.Strk)
                .Where(x => x.MacAddress == mac && x.ComponentId == component)
                .Select(x => MapperHelper.DoorToDto(x))
                .FirstOrDefaultAsync();
            if(dto is null) return ResponseHelper.NotFoundBuilder<DoorDto>();
            return ResponseHelper.SuccessBuilder(dto);
        }

        public async Task<ResponseDto<bool>> UnlockAsync(string mac, short component)
        {
            short id = await helperService.GetIdFromMacAsync(mac);
            if(!await command.MomentaryUnlockAsync(id,component))
            {
                return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.COMMAND_UNSUCCESS,MessageBuilder.Unsuccess(mac,Command.C311));
            }
            return ResponseHelper.SuccessBuilder(true);
        }

        private async Task<ResponseDto<IEnumerable<ModeDto>>> ReaderModeAsync()
        {
            var dtos = await context.ReaderConfigurationModes.Select(x => new ModeDto 
            {
                Name = x.Name,
                Value = x.Value,
                Description = x.Description

            }).ToArrayAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dtos);
        }

        private async Task<ResponseDto<IEnumerable<ModeDto>>> StrikeModeAsync()
        {
            var dtos = await context.StrikeModes.Select(x => new ModeDto 
            {
                Name = x.Name,
                Value = x.Value,
                Description = x.Description

            }).ToArrayAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dtos);
        }

        private async Task<ResponseDto<IEnumerable<ModeDto>>> AcrModeAsync()
        {
            var dtos = await context.DoorModes.Select(x => new ModeDto 
            {
                Name = x.Name,
                Value = x.Value,
                Description = x.Description

            }).ToArrayAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dtos);
        }

        public async Task<ResponseDto<IEnumerable<ModeDto>>> ApbModeAsync()
        {
            var dtos = await context.AntipassbackModes.Select(x => new ModeDto 
            {
                Name = x.Name,
                Value = x.Value,
                Description = x.Description

            }).ToArrayAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dtos);
        }


        public async Task<ResponseDto<IEnumerable<ModeDto>>> ReaderOutConfigurationAsync()
        {
            var dtos = await context.ReaderOutConfigurations
                .Select(x => new ModeDto
                {
                    Name = x.Name,
                    Value = x.Value,
                    Description = x.Description

                }).ToArrayAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dtos);
        }
        public async Task<ResponseDto<IEnumerable<short>>> AvailableReaderAsync(string mac, short component)
        {
            var reader = await context.Modules
                .AsNoTracking()
                .Where(cp => cp.ComponentId == component && cp.MacAddress == mac)
                .Select(cp => (short)cp.nReader)
                .FirstOrDefaultAsync();

            var rdrNos = await context.Readers
                .AsNoTracking()
                .Where(cp => cp.ModuleId == component && cp.MacAddress == mac)
                .Select(x => x.ReaderNo)
                .ToArrayAsync();


            List<short> all = Enumerable.Range(0, reader).Select(i => (short)i).ToList();
            return ResponseHelper.SuccessBuilder<IEnumerable<short>>(all.Except(rdrNos).ToList());
        }


        public async Task<ResponseDto<bool>> ChangeModeAsync(ChangeDoorModeDto dto)
        {
            var entity = await context.Doors.FirstOrDefaultAsync(x => x.MacAddress == dto.MacAddress && x.ComponentId == dto.ComponentId);
            if (entity == null) return ResponseHelper.NotFoundBuilder<bool>();

            var ScpId = await helperService.GetIdFromMacAsync(dto.MacAddress);
            if (!await command.ACRModeAsync(ScpId, dto.ComponentId, dto.Mode))
            {
                return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.COMMAND_UNSUCCESS,MessageBuilder.Unsuccess(dto.MacAddress,Command.C308));
            }

            entity.Mode = dto.Mode;
            entity.ModeDesc = await context.DoorModes
                .AsNoTracking()
                .Where(x => x.Value == entity.Mode)
                .Select(x => x.Name)
                .FirstOrDefaultAsync() ?? "";
            context.Update(entity);
            await context.SaveChangesAsync();
            return ResponseHelper.SuccessBuilder(true);

        }


        public async Task TriggerDeviceStatusAsync(int ScpId, short AcrNo, string AcrMode, string AccessPointStatus)
        {
            string ScpMac = await helperService.GetMacFromIdAsync((short)ScpId);
            await hub.Clients.All.SendAsync("AcrStatus", ScpMac, AcrNo, AcrMode, AccessPointStatus);
        }

        public void TriggerDeviceStatus(int ScpId, short AcrNo, string AcrMode, string AccessPointStatus)
        {
            string ScpMac = helperService.GetMacFromId((short)ScpId);
            hub.Clients.All.SendAsync("AcrStatus", ScpMac, AcrNo, AcrMode, AccessPointStatus);
        }



        public async Task<ResponseDto<bool>> CreateAsync(DoorDto dto)
        {
            var maxDoor = await context.SystemSettings
                .AsNoTracking()
                .Select(x => x.nAcr)
                .FirstOrDefaultAsync();

            short DoorId = await helperService
                .GetLowestUnassignedNumberAsync<Door>(context, dto.MacAddress, maxDoor);

            if (DoorId == -1) return ResponseHelper.ExceedLimit<bool>();

            short ScpId = await helperService.GetIdFromMacAsync(dto.MacAddress);

            foreach(var reader in dto.Readers)
            {
                if (string.IsNullOrEmpty(reader.MacAddress)) continue;
                short readerInOsdpFlag = 0x00;
                short readerLedDriveMode = 0;
                if (reader.OsdpFlag)
                {
                    readerInOsdpFlag |= reader.OsdpBaudrate;
                    readerInOsdpFlag |= reader.OsdpDiscover;
                    readerInOsdpFlag |= reader.OsdpTracing;
                    readerInOsdpFlag |= reader.OsdpAddress;
                    readerInOsdpFlag |= reader.OsdpSecureChannel;
                    readerLedDriveMode = 7;
                }
                else
                {
                    readerLedDriveMode = 1;
                }


                // Reader In Config

                var ReaderInId = await helperService.GetIdFromMacAsync(reader.MacAddress);
                var ReaderComponentId = await helperService.GetLowestUnassignedNumberNoLimitAsync<Reader>(context);
                reader.ComponentId = ReaderInId;
                if (!await command.ReaderSpecificationAsync(ReaderInId, reader.ModuleId, reader.ReaderNo, reader.DataFormat, reader.KeypadMode, readerLedDriveMode, readerInOsdpFlag))
                {
                    return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(dto.MacAddress, Command.C112));
                }
            }



            // Strike Strike Config
            var StrikeComponentId = await helperService.GetLowestUnassignedNumberNoLimitAsync<Strike>(context);
            dto.Strk.ComponentId = StrikeComponentId;
            var StrikeId = await helperService.GetIdFromMacAsync(dto.Strk.MacAddress);
            if (!await command.OutputPointSpecificationAsync(StrikeId, dto.Strk.ModuleId, dto.Strk.OutputNo, dto.Strk.RelayMode))
            {
                return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(dto.MacAddress, Command.C111));
            }

            // Door Sensor Config
            var SensorId = await helperService.GetIdFromMacAsync(dto.Sensor.MacAddress);
            var SensorComponentId = await helperService.GetLowestUnassignedNumberNoLimitAsync<Sensor>(context);
            dto.Sensor.ComponentId = SensorComponentId;
            if (!await command.InputPointSpecificationAsync(SensorId, dto.Sensor.ModuleId, dto.Sensor.InputNo, dto.Sensor.InputMode, dto.Sensor.Debounce, dto.Sensor.HoldTime))
            {
                return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(dto.MacAddress, Command.C110));
            }

            foreach (var rex in dto.RequestExits)
            {
                if (string.IsNullOrEmpty(rex.MacAddress)) continue;
                var Rex0Id = await helperService.GetIdFromMacAsync(rex.MacAddress);
                var rexComponentId = await helperService.GetLowestUnassignedNumberNoLimitAsync<RequestExit>(context);
                rex.ComponentId = rexComponentId;
                if (!await command.InputPointSpecificationAsync(Rex0Id, rex.ModuleId, rex.InputNo, rex.InputMode, rex.Debounce, rex.HoldTime))
                {
                    return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(dto.MacAddress, Command.C110));
                }
            }

            if (!await command.AccessControlReaderConfigurationAsync(ScpId, DoorId, dto))
            {
                return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(dto.MacAddress, Command.C115));
            }

            var door = MapperHelper.DtoToDoor(
                dto, 
                DoorId,
                await context.DoorModes
                .AsNoTracking()
                .Where(x => x.Value == dto.Mode)
                .Select(x => x.Name)
                .FirstOrDefaultAsync() ?? "",
                await context.DoorModes
                .AsNoTracking()
                .Where(x => x.Value == dto.OfflineMode)
                .Select(x => x.Name)
                .FirstOrDefaultAsync() ?? "",
                await context.DoorModes
                .AsNoTracking()
                .Where(x => x.Value == dto.DefaultMode)
                .Select(x => x.Name)
                .FirstOrDefaultAsync() ?? "",
                DateTime.Now);

            await context.Doors.AddAsync(door);
            await context.SaveChangesAsync();
            return ResponseHelper.SuccessBuilder(true);
        }

        public async Task<ResponseDto<bool>> DeleteAsync(string mac, short component)
        {
            var reader = await context.Doors.FirstOrDefaultAsync(x => x.MacAddress == mac && x.ComponentId == component);
            if(reader is null) return ResponseHelper.NotFoundBuilder<bool>();
            context.Doors.Remove(reader);
            await context.SaveChangesAsync();
            return ResponseHelper.SuccessBuilder<bool>(true);
        }

        public async Task<ResponseDto<DoorDto>> UpdateAsync(DoorDto dto)
        {

            var door = await context.Doors
                .Include(x => x.Readers)
                .Include(x => x.Sensor)
                .Include(x => x.RequestExits)
                .Include(x => x.Strk)
                .Where(x => x.ComponentId == dto.ComponentId && x.MacAddress == dto.MacAddress)
                .FirstOrDefaultAsync();

            if (door is null) return ResponseHelper.NotFoundBuilder<DoorDto>();
            short ScpId = await helperService.GetIdFromMacAsync(dto.MacAddress);

            foreach (var reader in dto.Readers)
            {
                if (string.IsNullOrEmpty(reader.MacAddress)) continue;
                short readerInOsdpFlag = 0x00;
                short readerLedDriveMode = 0;
                if (reader.OsdpFlag)
                {
                    readerInOsdpFlag |= reader.OsdpBaudrate;
                    readerInOsdpFlag |= reader.OsdpDiscover;
                    readerInOsdpFlag |= reader.OsdpTracing;
                    readerInOsdpFlag |= reader.OsdpAddress;
                    readerInOsdpFlag |= reader.OsdpSecureChannel;
                    readerLedDriveMode = 7;
                }
                else
                {
                    readerLedDriveMode = 1;
                }


                // Reader In Config
                var ReaderInId = await helperService.GetIdFromMacAsync(reader.MacAddress);
                if (!await command.ReaderSpecificationAsync(ReaderInId, reader.ModuleId, reader.ReaderNo, reader.DataFormat, reader.KeypadMode, readerLedDriveMode, readerInOsdpFlag))
                {
                    return ResponseHelper.UnsuccessBuilder<DoorDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(dto.MacAddress, Command.C112));
                }
            }

            // Strike Strike Config
            var StrkId = await helperService.GetIdFromMacAsync(dto.Strk.MacAddress);
            if (!await command.OutputPointSpecificationAsync(StrkId, dto.Strk.ModuleId, dto.Strk.OutputNo, dto.Strk.RelayMode))
            {
                return ResponseHelper.UnsuccessBuilder<DoorDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(dto.MacAddress, Command.C111));
            }

            // Door Sensor Config
            var SensorId = await helperService.GetIdFromMacAsync(dto.Sensor.MacAddress);
            if (!await command.InputPointSpecificationAsync(SensorId, dto.Sensor.ModuleId, dto.Sensor.InputNo, dto.Sensor.InputMode, dto.Sensor.Debounce, dto.Sensor.HoldTime))
            {
                return ResponseHelper.UnsuccessBuilder<DoorDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(dto.MacAddress, Command.C110));
            }

            foreach (var rex in dto.RequestExits)
            {
                if (string.IsNullOrEmpty(rex.MacAddress)) continue;
                var Rex0Id = await helperService.GetIdFromMacAsync(rex.MacAddress);
                if (!await command.InputPointSpecificationAsync(Rex0Id, rex.ModuleId, rex.InputNo, rex.InputMode, rex.Debounce, rex.HoldTime))
                {
                    return ResponseHelper.UnsuccessBuilder<DoorDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(dto.MacAddress, Command.C110));
                }
            }

            if (!await command.AccessControlReaderConfigurationAsync(ScpId, dto.ComponentId, dto))
            {
                return ResponseHelper.UnsuccessBuilder<DoorDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(dto.MacAddress, Command.C115));
            }

            // Delete old 
            context.Sensors.Remove(door.Sensor);
            if(door.RequestExits is not null)context.RequestExits.RemoveRange(door.RequestExits);
            context.Readers.RemoveRange(door.Readers);
            context.Strikes.Remove(door.Strk);


            MapperHelper.UpdateDoor(door,dto,
                 await context.DoorModes
                .AsNoTracking()
                .Where(x => x.Value == dto.Mode)
                .Select(x => x.Name)
                .FirstOrDefaultAsync() ?? "",
                await context.DoorModes
                .AsNoTracking()
                .Where(x => x.Value == dto.OfflineMode)
                .Select(x => x.Name)
                .FirstOrDefaultAsync() ?? "",
                await context.DoorModes
                .AsNoTracking()
                .Where(x => x.Value == dto.DefaultMode)
                .Select(x => x.Name)
                .FirstOrDefaultAsync() ?? "");

            context.Doors.Update(door);
            await context.SaveChangesAsync();
            return ResponseHelper.SuccessBuilder(dto);
        }

        public async Task<ResponseDto<bool>> GetStatusAsync(string mac, short component)
        {
            short id = await helperService.GetIdFromMacAsync(mac);
            if (!await command.GetAcrStatusAsync(id, component, 1))
            {
                return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.COMMAND_UNSUCCESS,MessageBuilder.Unsuccess(mac,Command.C407));
            }
            return ResponseHelper.SuccessBuilder(true);
        }

        public async Task<ResponseDto<IEnumerable<ModeDto>>> GetModeAsync(int param)
        {
            switch ((ComponentEnum.AcrServiceMode)param)
            {
                case ComponentEnum.AcrServiceMode.ReaderMode:
                    return await ReaderModeAsync();
                 case ComponentEnum.AcrServiceMode.StrikeMode:
                    return await StrikeModeAsync();
                 case ComponentEnum.AcrServiceMode.AcrMode:
                    return await AcrModeAsync();
                case ComponentEnum.AcrServiceMode.ApbMode:
                    return await ApbModeAsync();
                case ComponentEnum.AcrServiceMode.ReaderOut:
                    return await ReaderOutConfigurationAsync();
                case ComponentEnum.AcrServiceMode.SpareFlag:
                    return await GetSpareFlagAsync();
                case ComponentEnum.AcrServiceMode.AccessControlFlag:
                    return await GetAccessControlFlagAsync();
                default:
                    return ResponseHelper.UnsuccessBuilder<IEnumerable<ModeDto>>(ResponseMessage.NOT_FOUND,ResponseMessage.NOT_FOUND);
            }

        }

        public async Task<ResponseDto<IEnumerable<ModeDto>>> GetSpareFlagAsync()
        {
            var dtos = await context.DoorSpareFlags
                .Select(x => new ModeDto 
                {
                    Name = x.Name,
                    Value = x.Value,
                    Description = x.Description
                })
                .ToArrayAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dtos);
        }

        public async Task<ResponseDto<IEnumerable<ModeDto>>> GetAccessControlFlagAsync()
        {
            var dtos = await context.DoorAccessControlFlags
                .Select(x => new ModeDto 
                {
                    Name = x.Name,
                    Value = x.Value,
                    Description = x.Description
                }).ToArrayAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dtos);
        }


        public async Task<ResponseDto<IEnumerable<ModeDto>>> GetOsdpBaudRate()
        {
            var dtos = await context.OsdpBaudrates.Select(x => new ModeDto
            {
                Name = x.Name,
                Value = x.Value,
                Description = x.Description,
            }).ToArrayAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dtos);
        }

        public async Task<ResponseDto<IEnumerable<ModeDto>>> GetOsdpAddress()
        {
            var dtos = await context.OsdpAddresses.Select(x => new ModeDto 
            {
                Name = x.Name,
                Value = x.Value,
                Description = x.Description,
            }).ToArrayAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dtos);
        }

        public async Task<ResponseDto<IEnumerable<ModeDto>>> GetAvailableOsdpAddress(string mac,short component)
        {
            throw new NotImplementedException();
        }

 

    }
}
