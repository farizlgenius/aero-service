using Aero.Api.Constants;
using Aero.Application.Constants;
using Aero.Application.DTOs;
using Aero.Application.Helpers;
using Aero.Application.Interface;
using Aero.Application.Interfaces;
using Aero.Application.Mapper;
using Aero.Domain.Entities;
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
            short id = await qHw.GetComponentIdFromMacAsync(mac);
            if(!door.MomentaryUnlock(id,component))
            {
                return ResponseHelper.UnsuccessBuilderWithString<bool>(ResponseMessage.COMMAND_UNSUCCESS,MessageBuilder.Unsuccess(mac,Command.MOMENT_UNLOCK));
            }
            return ResponseHelper.SuccessBuilder(true);
        }

        private async Task<ResponseDto<IEnumerable<Mode>>> ReaderModeAsync()
        {
            var dtos = await qDoor.GetReaderModeAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<Mode>>(dtos);
        }

        private async Task<ResponseDto<IEnumerable<Mode>>> StrikeModeAsync()
        {
            var dtos = await qDoor.GetStrikeModeAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<Mode>>(dtos);
        }

        private async Task<ResponseDto<IEnumerable<Mode>>> AcrModeAsync()
        {
            var dtos = await qDoor.GetDoorModeAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<Mode>>(dtos);
        }

        public async Task<ResponseDto<IEnumerable<Mode>>> ApbModeAsync()
        {
            var dtos = await qDoor.GetApbModeAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<Mode>>(dtos);
        }


        public async Task<ResponseDto<IEnumerable<Mode>>> ReaderOutConfigurationAsync()
        {
            var dtos = await qDoor.GetReaderOutModeAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<Mode>>(dtos);
        }

        public async Task<ResponseDto<IEnumerable<short>>> AvailableReaderAsync(string mac, short component)
        {
            var reader = await qDoor.GetAvailableReaderFromMacAndComponentIdAsync(mac,component);
            return ResponseHelper.SuccessBuilder<IEnumerable<short>>(reader);
        }


        public async Task<ResponseDto<bool>> ChangeModeAsync(ChangeDoorModeDto dto)
        {
            if (!await qDoor.IsAnyByComponentId(dto.ComponentId)) return ResponseHelper.NotFoundBuilder<bool>();

            var ScpId = await qHw.GetComponentIdFromMacAsync(dto.Mac);
            if (!door.AcrMode(ScpId, dto.ComponentId, dto.Mode))
            {
                return ResponseHelper.UnsuccessBuilderWithString<bool>(ResponseMessage.COMMAND_UNSUCCESS,MessageBuilder.Unsuccess(dto.Mac,Command.ACR_MODE));
            }

            var status = await rDoor.ChangeDoorModeAsync(dto.Mac,dto.ComponentId,dto.AcrId,dto.Mode);
            if(status <= 0) return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.UPDATE_DOOR_MODE_UNSUCCESS,[]);
            return ResponseHelper.SuccessBuilder(true);

        }



        public async Task<ResponseDto<bool>> CreateAsync(DoorDto dto)
        {
            if(!await qHw.IsAnyByMac(dto.Mac)) return ResponseHelper.NotFoundBuilder<bool>();

            short DoorId = await qDoor.GetLowestUnassignedNumberAsync(10,"");
            short AcrId = await qDoor.GetLowestUnassignedNumberByMacAsync(dto.Mac,10);

            if (DoorId == -1 || AcrId == -1) return ResponseHelper.ExceedLimit<bool>();

            var domain = DoorMapper.ToDomain(dto);

            short ScpId = await qHw.GetComponentIdFromMacAsync(dto.Mac);


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

                var ReaderInId = await qHw.GetComponentIdFromMacAsync(reader.Mac);
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
            var StrikeId = await qHw.GetComponentIdFromMacAsync(domain.Strk.Mac);
            if (!cp.OutputPointSpecification(StrikeId, domain.Strk.ModuleId, domain.Strk.OutputNo, domain.Strk.RelayMode))
            {
                return ResponseHelper.UnsuccessBuilderWithString<bool>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(domain.Mac, Command.OUTPUT_SPEC));
            }

            // door sensor Config
            var SensorId = await qHw.GetComponentIdFromMacAsync(domain.Sensor.Mac);
            var SensorComponentId = await qDoor.GetLowestUnassignedSensorNumberNoLimitAsync();
            domain.Sensor.ComponentId = SensorComponentId;
            if (!mp.InputPointSpecification(SensorId, domain.Sensor.ModuleId, domain.Sensor.InputNo, domain.Sensor.InputMode, domain.Sensor.Debounce, domain.Sensor.HoldTime))
            {
                return ResponseHelper.UnsuccessBuilderWithString<bool>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(domain.Mac, Command.INPUT_SPEC));
            }

            foreach (var rex in domain.RequestExits)
            {
                if (string.IsNullOrEmpty(rex.Mac)) continue;
                var Rex0Id = await qHw.GetComponentIdFromMacAsync(rex.Mac);
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
                if (!door.ReaderSpecification(reader.ComponentId, reader.ModuleId, reader.ReaderNo, reader.DataFormat, reader.KeypadMode, readerLedDriveMode, readerInOsdpFlag))
                {
                    return ResponseHelper.UnsuccessBuilderWithString<DoorDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(domain.Mac, Command.READER_SPEC));
                }
            }

            // Strike Strike Config
            if (!cp.OutputPointSpecification(domain.StrkComponentId, domain.Strk.ModuleId, domain.Strk.OutputNo, domain.Strk.RelayMode))
            {
                return ResponseHelper.UnsuccessBuilderWithString<DoorDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(domain.Mac, Command.OUTPUT_SPEC));
            }

            // door sensor Config
            if (!mp.InputPointSpecification(domain.SensorComponentId,domain.Sensor.ModuleId,domain.Sensor.InputNo, domain.Sensor.InputMode, domain.Sensor.Debounce, domain.Sensor.HoldTime))
            {
                return ResponseHelper.UnsuccessBuilderWithString<DoorDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(domain.Mac, Command.INPUT_SPEC));
            }

            foreach (var rex in domain.RequestExits)
            {
                if (string.IsNullOrEmpty(rex.Mac)) continue;
                if (!mp.InputPointSpecification(rex.ComponentId, rex.ModuleId, rex.InputNo, rex.InputMode, rex.Debounce, rex.HoldTime))
                {
                    return ResponseHelper.UnsuccessBuilderWithString<DoorDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(domain.Mac, Command.INPUT_SPEC));
                }
            }

            if (!door.AccessControlReaderConfiguration(await qHw.GetComponentIdFromMacAsync(domain.Mac), dto.AcrId, domain))
            {
                return ResponseHelper.UnsuccessBuilderWithString<DoorDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(domain.Mac, Command.ACR_MODE));
            }

            var status = await rDoor.UpdateAsync(domain);

            if(status <= 0) return ResponseHelper.UnsuccessBuilder<DoorDto>(ResponseMessage.UPDATE_RECORD_UNSUCCESS,[]);

            return ResponseHelper.SuccessBuilder(dto);
        }

        public async Task<ResponseDto<bool>> GetStatusAsync(string mac, short component)
        {
            short id = await qHw.GetComponentIdFromMacAsync(mac);
            if (!door.GetAcrStatus(id, component, 1))
            {
                return ResponseHelper.UnsuccessBuilderWithString<bool>(ResponseMessage.COMMAND_UNSUCCESS,MessageBuilder.Unsuccess(mac,Command.C407));
            }
            return ResponseHelper.SuccessBuilder(true);
        }

        public async Task<ResponseDto<IEnumerable<Mode>>> GetModeAsync(int param)
        {
            switch ((Aero.Domain.Enums.DoorServiceMode)param)
            {
                case Aero.Domain.Enums.DoorServiceMode.ReaderMode:
                    return await ReaderModeAsync();
                 case Aero.Domain.Enums.DoorServiceMode.StrikeMode:
                    return await StrikeModeAsync();
                 case Aero.Domain.Enums.DoorServiceMode.AcrMode:
                    return await AcrModeAsync();
                case Aero.Domain.Enums.DoorServiceMode.ApbMode:
                    return await ApbModeAsync();
                case Aero.Domain.Enums.DoorServiceMode.ReaderOut:
                    return await ReaderOutConfigurationAsync();
                case Aero.Domain.Enums.DoorServiceMode.SpareFlag:
                    return await GetSpareFlagAsync();
                case Aero.Domain.Enums.DoorServiceMode.AccessControlFlag:
                    return await GetAccessControlFlagAsync();
                default:
                    return ResponseHelper.UnsuccessBuilderWithString<IEnumerable<Mode>>(ResponseMessage.NOT_FOUND,ResponseMessage.NOT_FOUND);
            }

        }

        public async Task<ResponseDto<IEnumerable<Mode>>> GetSpareFlagAsync()
        {
            var dtos = await qDoor.GetDoorSpareFlagAsync();    
            return ResponseHelper.SuccessBuilder<IEnumerable<Mode>>(dtos);
        }

        public async Task<ResponseDto<IEnumerable<Mode>>> GetAccessControlFlagAsync()
        {
            var dtos = await qDoor.GetDoorAccessControlFlagAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<Mode>>(dtos);
        }


        public async Task<ResponseDto<IEnumerable<Mode>>> GetOsdpBaudRate()
        {
            var dtos = await qDoor.GetOsdpBaudrateAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<Mode>>(dtos);
        }

        public async Task<ResponseDto<IEnumerable<Mode>>> GetOsdpAddress()
        {
             var dtos = await qDoor.GetOsdpAddressAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<Mode>>(dtos);
        }

        public async Task<ResponseDto<IEnumerable<Mode>>> GetAvailableOsdpAddress(string mac,short component)
        {
            throw new NotImplementedException();
        }

            public async Task<ResponseDto<DoorDto>> GetByComponentAsync( short component)
            {
                  var dtos = await qDoor.GetByComponentIdAsync(component);
                  return ResponseHelper.SuccessBuilder<DoorDto>(dtos);
            }
      }
}
