using Aero.Api.Constants;
using Aero.Application.Constants;
using Aero.Application.DTOs;
using Aero.Application.Helpers;
using Aero.Application.Interface;
using Aero.Application.Interfaces;
using Aero.Domain.Entities;
using Aero.Domain.Enums;
using Aero.Domain.Interface;
using Aero.Domain.Interfaces;

namespace Aero.Application.Services
{
    public class DoorService(IDoorCommand door,IHwRepository hw,IDoorRepository repo,ICpCommand cp,IMpCommand mp,ISettingRepository setting) : IDoorService
    {
        public async Task<ResponseDto<IEnumerable<DoorDto>>> GetAsync()
        {
            var dtos = await repo.GetAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<DoorDto>>(dtos);
        }
        public async Task<ResponseDto<IEnumerable<DoorDto>>> GetByLocationIdAsync(short location)
        {
            var dtos = await repo.GetByLocationIdAsync(location);
            return ResponseHelper.SuccessBuilder<IEnumerable<DoorDto>>(dtos);
        }

        public async Task<ResponseDto<IEnumerable<DoorDto>>> GetByDeviceIdAsync(int device)
        {
            var dtos = await repo.GetByDeviceIdAsync(device);
            return ResponseHelper.SuccessBuilder(dtos);
        }

        public async Task<ResponseDto<bool>> UnlockAsync(string mac, short component)
        {
            short id = await hw.GetComponentIdFromMacAsync(mac);
            if(!door.MomentaryUnlock(id,component))
            {
                return ResponseHelper.UnsuccessBuilderWithString<bool>(ResponseMessage.COMMAND_UNSUCCESS,MessageBuilder.Unsuccess(mac,Command.MOMENT_UNLOCK));
            }
            return ResponseHelper.SuccessBuilder(true);
        }

        private async Task<ResponseDto<IEnumerable<ModeDto>>> ReaderModeAsync()
        {
            var dtos = await repo.GetReaderModeAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dtos);
        }

        private async Task<ResponseDto<IEnumerable<ModeDto>>> StrikeModeAsync()
        {
            var dtos = await repo.GetStrikeModeAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dtos);
        }

        private async Task<ResponseDto<IEnumerable<ModeDto>>> AcrModeAsync()
        {
            var dtos = await repo.GetDoorModeAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dtos);
        }

        public async Task<ResponseDto<IEnumerable<ModeDto>>> ApbModeAsync()
        {
            var dtos = await repo.GetApbModeAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dtos);
        }


        public async Task<ResponseDto<IEnumerable<ModeDto>>> ReaderOutConfigurationAsync()
        {
            var dtos = await repo.GetReaderOutModeAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dtos);
        }

        public async Task<ResponseDto<IEnumerable<short>>> AvailableReaderAsync(int device, int driver)
        {
            var reader = await repo.GetAvailableReaderFromDeviceIdAndDriverIdAsync(device,driver);
            return ResponseHelper.SuccessBuilder<IEnumerable<short>>(reader);
        }


        public async Task<ResponseDto<bool>> ChangeModeAsync(ChangeDoorModeDto dto)
        {
            if (!await repo.IsAnyById(dto.DriverId)) return ResponseHelper.NotFoundBuilder<bool>();

            if (!door.AcrMode((short)dto.DeviceId, (short)dto.DriverId, dto.Mode))
            {
                return ResponseHelper.UnsuccessBuilderWithString<bool>(ResponseMessage.COMMAND_UNSUCCESS,MessageBuilder.Unsuccess(await hw.GetMacFromComponentAsync((short)dto.DeviceId),Command.ACR_MODE));
            }

            var status = await repo.ChangeDoorModeAsync(dto.DeviceId,(short)dto.DriverId,dto.Mode);
            if(status <= 0) return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.UPDATE_DOOR_MODE_UNSUCCESS,[]);
            return ResponseHelper.SuccessBuilder(true);

        }



        public async Task<ResponseDto<DoorDto>> CreateAsync(CreateDoorDto dto)
        {
            // Check value in license here 
            // ....to be implement

            if(await repo.IsAnyByNameAsync(dto.Name.Trim())) return ResponseHelper.BadRequestName<DoorDto>();

            var ScpSetting = await setting.GetScpSettingAsync();

            var DriverId = await repo.GetLowestUnassignedNumberByDeviceIdAsync(ScpSetting.nAcr,dto.DeviceId);

            if (DriverId == -1 ) return ResponseHelper.ExceedLimit<DoorDto>();


            var domain = new Door(0,DriverId,dto.Name,dto.AccessConfig,dto.PairDoorNo,
            (DoorDirection)dto.direction,
            dto.DeviceId,
            dto.Readers.Select(x => new Reader(x.ModuleId,x.DoorId,x.ReaderNo,x.DataFormat,x.KeypadMode,x.LedDriveMode,x.OsdpFlag,x.OsdpBaudrate,x.OsdpDiscover,x.OsdpTracing,x.OsdpAddress,x.OsdpSecureChannel,x.DeviceId,x.LocationId,x.IsActive)).ToList(),
            dto.ReaderOutConfiguration,
            dto.Strk is null ? null : new Strike(dto.Strk.DeviceId,dto.Strk.ModuleId,dto.Strk.DoorId,dto.Strk.OutputNo,dto.Strk.RelayMode,dto.Strk.OfflineMode,dto.Strk.StrkMax,dto.Strk.StrkMin,dto.Strk.StrkMode,dto.Strk.LocationId,dto.Strk.IsActive),
            dto.Sensor is null ? null : new Sensor(dto.Sensor.DeviceId,dto.Sensor.ModuleId,dto.Sensor.DoorId,dto.Sensor.InputNo,dto.Sensor.InputMode,dto.Sensor.Debounce,dto.Sensor.HoldTime,dto.Sensor.DcHeld,dto.Sensor.LocationId,dto.Sensor.IsActive),
            dto.RequestExits is null ? new List<RequestExit>() : dto.RequestExits.Select(x => new RequestExit(x.DeviceId,x.ModuleId,x.DoorId,x.InputNo,x.InputMode,x.Debounce,x.HoldTime,x.MaskTimeZone,x.LocationId,x.IsActive)).ToList()
            ,dto.CardFormat,dto.AntiPassbackMode,dto.AntiPassBackIn,
            dto.AreaInId,dto.AntiPassBackOut,dto.AreaOutId,dto.SpareTags,dto.AccessControlFlags,dto.Mode,dto.ModeDesc,
            dto.OfflineMode,dto.OfflineModeDesc,dto.DefaultMode,dto.DefaultModeDesc,dto.DefaultLEDMode,dto.PreAlarm,dto.AntiPassbackDelay,
            dto.StrkT2,dto.DcHeld2,dto.StrkFollowPulse,dto.StrkFollowDelay,dto.nExtFeatureType,dto.IlPBSio,dto.IlPBNumber,dto.IlPBLongPress,dto.IlPBOutSio,
            dto.IlPBOutNum,dto.DfOfFilterTime,dto.MaskHeldOpen,dto.MaskForceOpen
            );

            foreach (var reader in domain.Readers)
            {
                if (reader.DeviceId == 0) continue;
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
                if (!door.ReaderSpecification((short)reader.DeviceId, reader.ModuleId, reader.ReaderNo, reader.DataFormat, reader.KeypadMode, readerLedDriveMode, readerInOsdpFlag))
                {
                    return ResponseHelper.UnsuccessBuilderWithString<DoorDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(await hw.GetMacFromComponentAsync((short)domain.DeviceId), Command.READER_SPEC));
                }
            }



            // Strike Strike Config
            if(dto.Strk != null)
            {
                if (!cp.OutputPointSpecification((short)dto.Strk.DeviceId, domain.Strk.ModuleId, domain.Strk.OutputNo, domain.Strk.RelayMode))
            {
                return ResponseHelper.UnsuccessBuilderWithString<DoorDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(await hw.GetMacFromComponentAsync((short)domain.DeviceId), Command.OUTPUT_SPEC));
            }

            }
            
            // door sensor Config
            if(dto.Sensor != null)
            {
                if (!mp.InputPointSpecification((short)domain.Sensor.DeviceId, (short)domain.Sensor.ModuleId, domain.Sensor.InputNo, domain.Sensor.InputMode, domain.Sensor.Debounce, domain.Sensor.HoldTime))
            {
                return ResponseHelper.UnsuccessBuilderWithString<DoorDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(await hw.GetMacFromComponentAsync((short)domain.DeviceId), Command.INPUT_SPEC));
            }
            }
            

            foreach (var rex in domain.RequestExits)
            {
                if (rex.DeviceId == 0) continue;
                if (!mp.InputPointSpecification((short)rex.DeviceId, rex.ModuleId, rex.InputNo, rex.InputMode, rex.Debounce, rex.HoldTime))
                {
                    return ResponseHelper.UnsuccessBuilderWithString<DoorDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(await hw.GetMacFromComponentAsync((short)domain.DeviceId), Command.INPUT_SPEC));
                }
            }

            if (!door.AccessControlReaderConfiguration((short)domain.DeviceId, domain.DriverId, domain))
            {
                return ResponseHelper.UnsuccessBuilderWithString<DoorDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(await hw.GetMacFromComponentAsync((short)domain.DeviceId), Command.ACR_CONFIG));
            }

            var status = await repo.AddAsync(domain);

            if(status <= 0) return ResponseHelper.UnsuccessBuilder<DoorDto>(ResponseMessage.SAVE_DATABASE_UNSUCCESS,[]);

            return ResponseHelper.SuccessBuilder(await repo.GetByIdAsync(status));
        }

        public async Task<ResponseDto<DoorDto>> DeleteAsync(int id)
        {
            var dto = await repo.GetByIdAsync(id);
            if(dto is null) return ResponseHelper.NotFoundBuilder<DoorDto>();

            // Send Command 
            var domain = new Door(0,dto.DriverId,dto.Name,dto.AccessConfig,dto.PairDoorNo,
            (DoorDirection)dto.Direction,
            dto.DeviceId,
            dto.Readers.Select(x => new Reader(x.ModuleId,x.DoorId,x.ReaderNo,x.DataFormat,x.KeypadMode,x.LedDriveMode,x.OsdpFlag,x.OsdpBaudrate,x.OsdpDiscover,x.OsdpTracing,x.OsdpAddress,x.OsdpSecureChannel,x.DeviceId,x.LocationId,x.IsActive)).ToList(),
            dto.ReaderOutConfiguration,
            dto.Strk is null ? null : new Strike(dto.Strk.DeviceId,dto.Strk.ModuleId,dto.Strk.DoorId,dto.Strk.OutputNo,dto.Strk.RelayMode,dto.Strk.OfflineMode,dto.Strk.StrkMax,dto.Strk.StrkMin,dto.Strk.StrkMode,dto.Strk.LocationId,dto.Strk.IsActive),
            dto.Sensor is null ? null : new Sensor(dto.Sensor.DeviceId,dto.Sensor.ModuleId,dto.Sensor.DoorId,dto.Sensor.InputNo,dto.Sensor.InputMode,dto.Sensor.Debounce,dto.Sensor.HoldTime,dto.Sensor.DcHeld,dto.Sensor.LocationId,dto.Sensor.IsActive),
            dto.RequestExits is null ? new List<RequestExit>() : dto.RequestExits.Select(x => new RequestExit(x.DeviceId,x.ModuleId,x.DoorId,x.InputNo,x.InputMode,x.Debounce,x.HoldTime,x.MaskTimeZone,x.LocationId,x.IsActive)).ToList()
            ,dto.CardFormat,dto.AntiPassbackMode,dto.AntiPassBackIn,
            dto.AreaInId,dto.AntiPassBackOut,dto.AreaOutId,dto.SpareTags,dto.AccessControlFlags,dto.Mode,dto.ModeDesc,
            dto.OfflineMode,dto.OfflineModeDesc,dto.DefaultMode,dto.DefaultModeDesc,dto.DefaultLEDMode,dto.PreAlarm,dto.AntiPassbackDelay,
            dto.StrkT2,dto.DcHeld2,dto.StrkFollowPulse,dto.StrkFollowDelay,dto.nExtFeatureType,dto.IlPBSio,dto.IlPBNumber,dto.IlPBLongPress,dto.IlPBOutSio,
            dto.IlPBOutNum,dto.DfOfFilterTime,dto.MaskHeldOpen,dto.MaskForceOpen
            );


            if (!door.AccessControlReaderConfiguration((short)domain.DeviceId, domain.DriverId, domain))
            {
                return ResponseHelper.UnsuccessBuilderWithString<DoorDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(await hw.GetMacFromComponentAsync((short)domain.DeviceId), Command.ACR_CONFIG));
            }

            // 
            var status = await repo.DeleteByIdAsync(id);
            if(status <= 0) return ResponseHelper.UnsuccessBuilder<DoorDto>(ResponseMessage.DELETE_DATABASE_UNSUCCESS,[]);
            return ResponseHelper.SuccessBuilder<DoorDto>(dto);
        }

        public async Task<ResponseDto<DoorDto>> UpdateAsync(DoorDto dto)
        {

            if (!await repo.IsAnyById(dto.Id)) return ResponseHelper.NotFoundBuilder<DoorDto>();

            var domain = new Door(0,dto.DriverId,dto.Name,dto.AccessConfig,dto.PairDoorNo,
            (DoorDirection)dto.Direction,
            dto.DeviceId,
            dto.Readers.Select(x => new Reader(x.ModuleId,x.DoorId,x.ReaderNo,x.DataFormat,x.KeypadMode,x.LedDriveMode,x.OsdpFlag,x.OsdpBaudrate,x.OsdpDiscover,x.OsdpTracing,x.OsdpAddress,x.OsdpSecureChannel,x.DeviceId,x.LocationId,x.IsActive)).ToList(),
            dto.ReaderOutConfiguration,
            dto.Strk is null ? null : new Strike(dto.Strk.DeviceId,dto.Strk.ModuleId,dto.Strk.DoorId,dto.Strk.OutputNo,dto.Strk.RelayMode,dto.Strk.OfflineMode,dto.Strk.StrkMax,dto.Strk.StrkMin,dto.Strk.StrkMode,dto.Strk.LocationId,dto.Strk.IsActive),
            dto.Sensor is null ? null : new Sensor(dto.Sensor.DeviceId,dto.Sensor.ModuleId,dto.Sensor.DoorId,dto.Sensor.InputNo,dto.Sensor.InputMode,dto.Sensor.Debounce,dto.Sensor.HoldTime,dto.Sensor.DcHeld,dto.Sensor.LocationId,dto.Sensor.IsActive),
            dto.RequestExits is null ? new List<RequestExit>() : dto.RequestExits.Select(x => new RequestExit(x.DeviceId,x.ModuleId,x.DoorId,x.InputNo,x.InputMode,x.Debounce,x.HoldTime,x.MaskTimeZone,x.LocationId,x.IsActive)).ToList()
            ,dto.CardFormat,dto.AntiPassbackMode,dto.AntiPassBackIn,
            dto.AreaInId,dto.AntiPassBackOut,dto.AreaOutId,dto.SpareTags,dto.AccessControlFlags,dto.Mode,dto.ModeDesc,
            dto.OfflineMode,dto.OfflineModeDesc,dto.DefaultMode,dto.DefaultModeDesc,dto.DefaultLEDMode,dto.PreAlarm,dto.AntiPassbackDelay,
            dto.StrkT2,dto.DcHeld2,dto.StrkFollowPulse,dto.StrkFollowDelay,dto.nExtFeatureType,dto.IlPBSio,dto.IlPBNumber,dto.IlPBLongPress,dto.IlPBOutSio,
            dto.IlPBOutNum,dto.DfOfFilterTime,dto.MaskHeldOpen,dto.MaskForceOpen
            );

            foreach (var reader in domain.Readers)
            {
                if (reader.DeviceId == 0) continue;
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
                if (!door.ReaderSpecification((short)reader.DeviceId, reader.ModuleId, reader.ReaderNo, reader.DataFormat, reader.KeypadMode, readerLedDriveMode, readerInOsdpFlag))
                {
                    return ResponseHelper.UnsuccessBuilderWithString<DoorDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(await hw.GetMacFromComponentAsync((short)domain.DeviceId), Command.READER_SPEC));
                }
            }

            // Strike Strike Config
            if (!cp.OutputPointSpecification((short)domain.Strk.DeviceId, domain.Strk.ModuleId, domain.Strk.OutputNo, domain.Strk.RelayMode))
            {
                return ResponseHelper.UnsuccessBuilderWithString<DoorDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(await hw.GetMacFromComponentAsync((short)domain.DeviceId), Command.OUTPUT_SPEC));
            }

            // door sensor Config
            if (!mp.InputPointSpecification((short)domain.Sensor.DeviceId,(short)domain.Sensor.ModuleId,domain.Sensor.InputNo, domain.Sensor.InputMode, domain.Sensor.Debounce, domain.Sensor.HoldTime))
            {
                return ResponseHelper.UnsuccessBuilderWithString<DoorDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(await hw.GetMacFromComponentAsync((short)domain.DeviceId), Command.INPUT_SPEC));
            }

            foreach (var rex in domain.RequestExits)
            {
                if (rex.DeviceId == 0) continue;
                if (!mp.InputPointSpecification((short)rex.DeviceId, rex.ModuleId, rex.InputNo, rex.InputMode, rex.Debounce, rex.HoldTime))
                {
                    return ResponseHelper.UnsuccessBuilderWithString<DoorDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(await hw.GetMacFromComponentAsync((short)domain.DeviceId), Command.INPUT_SPEC));
                }
            }

            if (!door.AccessControlReaderConfiguration((short)domain.DeviceId, dto.DriverId, domain))
            {
                return ResponseHelper.UnsuccessBuilderWithString<DoorDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(await hw.GetMacFromComponentAsync((short)domain.DeviceId), Command.ACR_MODE));
            }

            var status = await repo.UpdateAsync(domain);

            if(status <= 0) return ResponseHelper.UnsuccessBuilder<DoorDto>(ResponseMessage.UPDATE_RECORD_UNSUCCESS,[]);

            return ResponseHelper.SuccessBuilder(dto);
        }

        public async Task<ResponseDto<bool>> GetStatusAsync(string mac, short component)
        {
            short id = await hw.GetComponentIdFromMacAsync(mac);
            if (!door.GetAcrStatus(id, component, 1))
            {
                return ResponseHelper.UnsuccessBuilderWithString<bool>(ResponseMessage.COMMAND_UNSUCCESS,MessageBuilder.Unsuccess(mac,Command.C407));
            }
            return ResponseHelper.SuccessBuilder(true);
        }

        public async Task<ResponseDto<IEnumerable<ModeDto>>> GetModeAsync(int param)
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
                    return ResponseHelper.UnsuccessBuilderWithString<IEnumerable<ModeDto>>(ResponseMessage.NOT_FOUND,ResponseMessage.NOT_FOUND);
            }

        }

        public async Task<ResponseDto<IEnumerable<ModeDto>>> GetSpareFlagAsync()
        {
            var dtos = await repo.GetDoorSpareFlagAsync();    
            return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dtos);
        }

        public async Task<ResponseDto<IEnumerable<ModeDto>>> GetAccessControlFlagAsync()
        {
            var dtos = await repo.GetDoorAccessControlFlagAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dtos);
        }


        public async Task<ResponseDto<IEnumerable<ModeDto>>> GetOsdpBaudRate()
        {
            var dtos = await repo.GetOsdpBaudrateAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dtos);
        }

        public async Task<ResponseDto<IEnumerable<ModeDto>>> GetOsdpAddress()
        {
             var dtos = await repo.GetOsdpAddressAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dtos);
        }

        public async Task<ResponseDto<IEnumerable<ModeDto>>> GetAvailableOsdpAddress(string mac,short component)
        {
            throw new NotImplementedException();
        }

            public async Task<ResponseDto<DoorDto>> GetByComponentAsync( short component)
            {
                  var dtos = await repo.GetByIdAsync(component);
                  return ResponseHelper.SuccessBuilder<DoorDto>(dtos);
            }

        public async Task<ResponseDto<Pagination<DoorDto>>> GetPaginationAsync(PaginationParamsWithFilter param, short location)
        {
            var res = await repo.GetPaginationAsync(param, location);
            return ResponseHelper.SuccessBuilder(res);
        }
    }
}
