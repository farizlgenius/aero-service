using Aero.Api.Constants;
using Aero.Application.Constants;
using Aero.Application.DTOs;
using Aero.Application.Helpers;
using Aero.Application.Interface;
using Aero.Application.Interfaces;
using Aero.Application.Mapper;
using Aero.Domain.Interface;
using Aero.Domain.Interfaces;

namespace Aero.Application.Services
{
    public class DoorService(IQDoorRepository qDoor,IDoorCommand door,IQHwRepository qHw,IDoorRepository rDoor,ICpCommand cp,IMpCommand mp) : IDoorService
    {
        public async Task<ResponseDto<IEnumerable<DoorDto>>> GetAsync()
        {
            var dtos = await qDoor.GetAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<DoorDto>>(dtos);
        }
        public async Task<ResponseDto<IEnumerable<DoorDto>>> GetByLocationIdAsync(short location)
        {
            var dtos = await qDoor.GetByLocationIdAsync(location);
            return ResponseHelper.SuccessBuilder<IEnumerable<DoorDto>>(dtos);
        }

        public async Task<ResponseDto<IEnumerable<DoorDto>>> GetByMacAsync(string mac)
        {
            var dtos = await qDoor.GetByMacAsync(mac);
            return ResponseHelper.SuccessBuilder(dtos);
        }

        public async Task<ResponseDto<bool>> UnlockAsync(string mac, short component)
        {
            short id = await qHw.GetComponentFromMacAsync(mac);
            if(!door.MomentaryUnlock(id,component))
            {
                return ResponseHelper.UnsuccessBuilderWithString<bool>(ResponseMessage.COMMAND_UNSUCCESS,MessageBuilder.Unsuccess(mac,Command.MOMENT_UNLOCK));
            }
            return ResponseHelper.SuccessBuilder(true);
        }

        private async Task<ResponseDto<IEnumerable<ModeDto>>> ReaderModeAsync()
        {
            var dtos = await qDoor.GetReaderModeAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dtos);
        }

        private async Task<ResponseDto<IEnumerable<ModeDto>>> StrikeModeAsync()
        {
            var dtos = await qDoor.GetStrikeModeAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dtos);
        }

        private async Task<ResponseDto<IEnumerable<ModeDto>>> AcrModeAsync()
        {
            var dtos = await qDoor.GetDoorModeAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dtos);
        }

        public async Task<ResponseDto<IEnumerable<ModeDto>>> ApbModeAsync()
        {
            var dtos = await qDoor.GetApbModeAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dtos);
        }


        public async Task<ResponseDto<IEnumerable<ModeDto>>> ReaderOutConfigurationAsync()
        {
            var dtos = await qDoor.GetReaderOutModeAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dtos);
        }

        public async Task<ResponseDto<IEnumerable<short>>> AvailableReaderAsync(string mac, short component)
        {
            var reader = await qDoor.GetAvailableReaderFromMacAndComponentIdAsync(mac,component);
            return ResponseHelper.SuccessBuilder<IEnumerable<short>>(reader);
        }


        public async Task<ResponseDto<bool>> ChangeModeAsync(ChangeDoorModeDto dto)
        {
            if (!await qDoor.IsAnyByComponentId(dto.ComponentId)) return ResponseHelper.NotFoundBuilder<bool>();

            var ScpId = await qHw.GetComponentFromMacAsync(dto.Mac);
            if (!door.AcrMode(ScpId, dto.ComponentId, dto.Mode))
            {
                return ResponseHelper.UnsuccessBuilderWithString<bool>(ResponseMessage.COMMAND_UNSUCCESS,MessageBuilder.Unsuccess(dto.Mac,Command.ACR_MODE));
            }

            var status = await rDoor.ChangeDoorModeAsync(dto.Mac,dto.ComponentId,dto.AcrId,dto.Mode);
            if(status <= 0) return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.UPDATE_DOOR_MODE_UNSUCCESS,[]);
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
            if(!await qHw.IsAnyByMac(dto.Mac)) return ResponseHelper.NotFoundBuilder<bool>();

            short DoorId = await qDoor.GetLowestUnassignedNumberAsync(10);
            short AcrId = await qDoor.GetLowestUnassignedNumberByMacAsync(dto.Mac,10);

            if (DoorId == -1 || AcrId == -1) return ResponseHelper.ExceedLimit<bool>();

            var domain = DoorMapper.ToDomain(dto);

            short ScpId = await qHw.GetComponentFromMacAsync(dto.Mac);


            foreach (var reader in domain.Readers)
            {
                if (string.IsNullOrEmpty(reader.Mac)) continue;
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

                var ReaderInId = await qHw.GetComponentFromMacAsync(reader.Mac);
                var ReaderComponentId = await qDoor.GetLowestUnassignedReaderNumberNoLimitAsync();
                reader.ComponentId = ReaderComponentId;
                if (!door.ReaderSpecification(ReaderInId, reader.ModuleId, reader.ReaderNo, reader.DataFormat, reader.KeypadMode, readerLedDriveMode, readerInOsdpFlag))
                {
                    return ResponseHelper.UnsuccessBuilderWithString<bool>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(domain.Mac, Command.READER_SPEC));
                }
            }



            // Strike Strike Config
            var StrikeComponentId = await qDoor.GetLowestUnassignedStrikeNumberNoLimitAsync();
            domain.Strk.ComponentId = StrikeComponentId;
            var StrikeId = await qHw.GetComponentFromMacAsync(domain.Strk.Mac);
            if (!cp.OutputPointSpecification(StrikeId, domain.Strk.ModuleId, domain.Strk.OutputNo, domain.Strk.RelayMode))
            {
                return ResponseHelper.UnsuccessBuilderWithString<bool>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(domain.Mac, Command.OUTPUT_SPEC));
            }

            // door sensor Config
            var SensorId = await qHw.GetComponentFromMacAsync(domain.Sensor.Mac);
            var SensorComponentId = await qDoor.GetLowestUnassignedSensorNumberNoLimitAsync();
            domain.Sensor.ComponentId = SensorComponentId;
            if (!mp.InputPointSpecification(SensorId, domain.Sensor.ModuleId, domain.Sensor.InputNo, domain.Sensor.InputMode, domain.Sensor.Debounce, domain.Sensor.HoldTime))
            {
                return ResponseHelper.UnsuccessBuilderWithString<bool>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(domain.Mac, Command.INPUT_SPEC));
            }

            foreach (var rex in domain.RequestExits)
            {
                if (string.IsNullOrEmpty(rex.Mac)) continue;
                var Rex0Id = await qHw.GetComponentFromMacAsync(rex.Mac);
                var rexComponentId = await qDoor.GetLowestUnassignedRexNumberAsync();
                rex.ComponentId = rexComponentId;
                if (!mp.InputPointSpecification(Rex0Id, rex.ModuleId, rex.InputNo, rex.InputMode, rex.Debounce, rex.HoldTime))
                {
                    return ResponseHelper.UnsuccessBuilderWithString<bool>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(domain.Mac, Command.INPUT_SPEC));
                }
            }

            if (!door.AccessControlReaderConfiguration(ScpId, DoorId, domain))
            {
                return ResponseHelper.UnsuccessBuilderWithString<bool>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(domain.Mac, Command.ACR_CONFIG));
            }

            var status = await rDoor.AddAsync(domain);

            if(status <= 0) return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.SAVE_DATABASE_UNSUCCESS,[]);

            return ResponseHelper.SuccessBuilder(true);
        }

        public async Task<ResponseDto<bool>> DeleteAsync(short component)
        {
            if(!await qDoor.IsAnyByComponentId(component)) return ResponseHelper.NotFoundBuilder<bool>();
            var status = await rDoor.DeleteByComponentIdAsync(component);
            if(status <= 0) return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.DELETE_DATABASE_UNSUCCESS,[]);
            return ResponseHelper.SuccessBuilder<bool>(true);
        }

        public async Task<ResponseDto<DoorDto>> UpdateAsync(DoorDto dto)
        {

            if (!await qDoor.IsAnyByComponentId(dto.ComponentId)) return ResponseHelper.NotFoundBuilder<DoorDto>();

            var domain = DoorMapper.ToDomain(dto);

            foreach (var reader in domain.Readers)
            {
                if (string.IsNullOrEmpty(reader.Mac)) continue;
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
                var ReaderInId = await helperService.GetIdFromMacAsync(reader.module.hardware_mac);
                if (!command.ReaderSpecification(ReaderInId, reader.module_id, reader.reader_no, reader.data_format, reader.keypad_mode, readerLedDriveMode, readerInOsdpFlag))
                {
                    return ResponseHelper.UnsuccessBuilderWithString<DoorDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(door.hardware_mac, Command.C112));
                }
            }

            // Strike Strike Config
            var StrkId = await helperService.GetIdFromMacAsync(door.strike.module.hardware_mac);
            if (!command.OutputPointSpecification(StrkId, door.strike.module_id, door.strike.output_no, door.strike.relay_mode))
            {
                return ResponseHelper.UnsuccessBuilderWithString<DoorDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(door.hardware_mac, Command.C111));
            }

            // door sensor Config
            var SensorId = await helperService.GetIdFromMacAsync(door.sensor.module.hardware_mac);
            if (!command.InputPointSpecification(SensorId, door.sensor.module_id, door.sensor.input_no, door.sensor.input_mode, door.sensor.debounce, door.sensor.holdtime))
            {
                return ResponseHelper.UnsuccessBuilderWithString<DoorDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(door.hardware_mac, Command.C110));
            }

            foreach (var rex in door.request_exits)
            {
                if (string.IsNullOrEmpty(rex.module.hardware_mac)) continue;
                var Rex0Id = await helperService.GetIdFromMacAsync(rex.module.hardware_mac);
                if (!command.InputPointSpecification(Rex0Id, rex.module_id, rex.input_no, rex.input_mode, rex.debounce, rex.holdtime))
                {
                    return ResponseHelper.UnsuccessBuilderWithString<DoorDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(dto.Mac, Command.C110));
                }
            }

            if (!command.AccessControlReaderConfiguration(ScpId, dto.ComponentId, door))
            {
                return ResponseHelper.UnsuccessBuilderWithString<DoorDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(door.hardware_mac, Command.C115));
            }

            // DeleteAsync old 
            context.sensor.Remove(door.sensor);
            if(door.request_exits is not null)context.request_exit.RemoveRange(door.request_exits);
            context.reader.RemoveRange(door.readers);
            context.strike.Remove(door.strike);

            context.door.Update(door);
            await context.SaveChangesAsync();
            return ResponseHelper.SuccessBuilder(dto);
        }

        public async Task<ResponseDto<bool>> GetStatusAsync(string mac, short component)
        {
            short id = await helperService.GetIdFromMacAsync(mac);
            if (!command.GetAcrStatus(id, component, 1))
            {
                return ResponseHelper.UnsuccessBuilderWithString<bool>(ResponseMessage.COMMAND_UNSUCCESS,MessageBuilder.Unsuccess(mac,Command.C407));
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
                    return ResponseHelper.UnsuccessBuilderWithString<IEnumerable<ModeDto>>(ResponseMessage.NOT_FOUND,ResponseMessage.NOT_FOUND);
            }

        }

        public async Task<ResponseDto<IEnumerable<ModeDto>>> GetSpareFlagAsync()
        {
            var dtos = await context.door_access_control_flag
                .Select(x => new ModeDto 
                {
                    Name = x.name,
                    Value = x.value,
                    Description = x.description
                })
                .ToArrayAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dtos);
        }

        public async Task<ResponseDto<IEnumerable<ModeDto>>> GetAccessControlFlagAsync()
        {
            var dtos = await context.door_access_control_flag
                .Select(x => new ModeDto 
                {
                    Name = x.name,
                    Value = x.value,
                    Description = x.description
                }).ToArrayAsync();

            return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dtos);
        }


        public async Task<ResponseDto<IEnumerable<ModeDto>>> GetOsdpBaudRate()
        {
            var dtos = await context.osdp_baudrate.Select(x => new ModeDto
            {
                Name = x.name,
                Value = x.value,
                Description = x.description,
            }).ToArrayAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dtos);
        }

        public async Task<ResponseDto<IEnumerable<ModeDto>>> GetOsdpAddress()
        {
            var dtos = await context.osdp_address.Select(x => new ModeDto 
            {
                Name = x.name,
                Value = x.value,
                Description = x.description,
            }).ToArrayAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dtos);
        }

        public async Task<ResponseDto<IEnumerable<ModeDto>>> GetAvailableOsdpAddress(string mac,short component)
        {
            throw new NotImplementedException();
        }

 

    }
}
