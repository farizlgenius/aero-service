using Aero.Api.Constants;
using Aero.Application.Commands.Interfaces;
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
    public class DoorService(IAeroAdapter aero, IDeviceRepository hw, IDoorRepository repo, ISettingRepository setting) : IDoorService
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

        public async Task<ResponseDto<bool>> UnlockByIdAsync(int id)
        {
            var acr = await repo.GetByIdAsync(id);
            if (!aero.MomentaryUnlock((short)acr.ScpId, acr.AcrId))
            {
                return ResponseHelper.UnsuccessBuilderWithString<bool>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(await hw.GetMacFromComponentAsync((short)acr.ScpId), Command.MOMENT_UNLOCK));
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

        public async Task<ResponseDto<IEnumerable<short>>> AvailableReaderAsync(int module)
        {
            var reader = await repo.GetAvailableReaderFromModuleIddAsync(module);
            return ResponseHelper.SuccessBuilder<IEnumerable<short>>(reader);
        }


        public async Task<ResponseDto<bool>> ChangeModeAsync(ChangeDoorModeDto dto)
        {
            if (!await repo.IsAnyByIdAsync(dto.Id)) return ResponseHelper.NotFoundBuilder<bool>();

            if (!aero.AcrMode((short)dto.ScpId, (short)dto.AcrId, dto.Mode))
            {
                return ResponseHelper.UnsuccessBuilderWithString<bool>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(await hw.GetMacFromComponentAsync((short)dto.ScpId), Command.ACR_MODE));
            }

            var status = await repo.ChangeDoorModeAsync(dto.ScpId, (short)dto.AcrId, dto.Mode);
            if (status <= 0) return ResponseHelper.UnsuccessBuilder<bool>(ResponseMessage.UPDATE_DOOR_MODE_UNSUCCESS, []);
            return ResponseHelper.SuccessBuilder(true);

        }


        public async Task<ResponseDto<DoorDto>> CreateAsync(CreateDoorDto dto)
        {
            // Check value in license here 
            // ....to be implement

            var ScpSetting = await setting.GetScpSettingAsync();

            // var DriverId = await repo.GetLowestUnassignedNumberByDeviceIdAsync(ScpSetting.nAcr, dto.ScpId);
            // var DriverIdTwo = await repo.GetLowestUnassignedNumberByDeviceIdAsync(ScpSetting.nAcr, dto.ScpId);

            var d = await repo.GetLowestUnassignedNumberTwoByDeviceIdAsync(ScpSetting.nAcr,dto.ScpId);

            if (d.ElementAt(0) == -1) return ResponseHelper.ExceedLimit<DoorDto>();


            var domain = new Door(
                0,
                d.ElementAt(0),
                dto.Name,
                dto.AccessConfig,
                dto.AccessConfig == (int)Aero.Domain.Enums.DoorType.Dual ? d.ElementAt(1) : dto.PairDoorNo,
                DoorDirection.IN,
                dto.ScpId,
                dto.Readers is null ? new List<Reader>() : dto.Readers.Select(x => new Reader(
                    x.ModuleId,
                    x.ModuleDriverId,
                    x.DoorId,
                    x.Direction,
                    x.ReaderNo,
                    x.DataFormat,
                    x.KeypadMode,
                    x.LedDriveMode,
                    x.OsdpFlag,
                    x.OsdpBaudrate,
                    x.OsdpDiscover,
                    x.OsdpTracing,
                    x.OsdpAddress,
                    x.OsdpSecureChannel,
                    x.ScpId,
                    x.LocationId,
                    x.IsActive
                )
                ).ToList(),
                dto.ReaderOutConfiguration,
                    dto.Strk is null ? null : new Strike(dto.Strk.ScpId, dto.Strk.ModuleId, dto.Strk.ModuleDriverId, dto.Strk.DoorId, dto.Strk.OutputNo, dto.Strk.RelayMode, dto.Strk.OfflineMode, dto.Strk.StrkMax, dto.Strk.StrkMin, dto.Strk.StrkMode, dto.Strk.LocationId, dto.Strk.IsActive),
                    dto.Sensor is null ? null : new Sensor(dto.Sensor.ScpId, dto.Sensor.ModuleId, dto.Sensor.ModuleDriverId, dto.Sensor.DoorId, dto.Sensor.InputNo, dto.Sensor.InputMode, dto.Sensor.Debounce, dto.Sensor.HoldTime, dto.Sensor.DcHeld, dto.Sensor.LocationId, dto.Sensor.IsActive),
                    dto.RequestExits is null ? new List<RequestExit>() : dto.RequestExits.Select(x => new RequestExit(x.ScpId, x.ModuleId, x.ModuleDriverId, x.DoorId, x.InputNo, x.InputMode, x.Debounce, x.HoldTime, x.MaskTimeZone, x.LocationId, x.IsActive)).ToList()
                    , dto.CardFormat, dto.AntiPassbackMode, dto.AntiPassBackIn, dto.AntiPassBackOut, dto.SpareTags, dto.AccessControlFlags, dto.Mode, dto.ModeDesc,
                    dto.OfflineMode, dto.OfflineModeDesc, dto.DefaultMode, dto.DefaultModeDesc, dto.DefaultLEDMode, dto.PreAlarm, dto.AntiPassbackDelay,
                    dto.StrkT2, dto.DcHeld2, dto.StrkFollowPulse, dto.StrkFollowDelay, dto.nExtFeatureType, dto.IlPBSio, dto.IlPBNumber, dto.IlPBLongPress, dto.IlPBOutSio,
                    dto.IlPBOutNum, dto.DfOfFilterTime, dto.MaskHeldOpen, dto.MaskForceOpen
                    );

            foreach (var reader in domain.Readers)
            {
                if (reader.ScpId == 0) continue;
                short readerInOsdpFlag = 0x00;
                short readerLedDriveMode = 0;
                if (reader.OsdpFlag)
                {
                    readerInOsdpFlag += reader.OsdpBaudrate;
                    readerInOsdpFlag |= reader.OsdpDiscover;
                    readerInOsdpFlag |= reader.OsdpTracing;
                    readerInOsdpFlag |= (short)(reader.OsdpAddress << 5);
                    readerInOsdpFlag |= reader.OsdpSecureChannel;
                    readerLedDriveMode = 7;
                }
                else
                {
                    readerLedDriveMode = 1;
                }


                // Reader In Config
                if (!aero.ReaderSpecification((short)reader.ScpId, reader.ModuleDriverId, reader.ReaderNo, reader.DataFormat, reader.KeypadMode, readerLedDriveMode, readerInOsdpFlag))
                {
                    return ResponseHelper.UnsuccessBuilderWithString<DoorDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(await hw.GetMacFromComponentAsync((short)domain.ScpId), Command.READER_SPEC));
                }
            }

            // Strike Strike Config
            if (dto.Strk != null)
            {
                if (!aero.OutputPointSpecification((short)dto.Strk.ScpId, (short)domain.Strk.ModuleDriverId, domain.Strk.OutputNo, domain.Strk.RelayMode))
                {
                    return ResponseHelper.UnsuccessBuilderWithString<DoorDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(await hw.GetMacFromComponentAsync((short)domain.ScpId), Command.OUTPUT_SPEC));
                }

            }

            // door sensor Config
            if (dto.Sensor != null)
            {
                if (!aero.InputPointSpecification((short)domain.Sensor.ScpId, (short)domain.Sensor.ModuleDriverId, domain.Sensor.InputNo, domain.Sensor.InputMode, domain.Sensor.Debounce, domain.Sensor.HoldTime))
                {
                    return ResponseHelper.UnsuccessBuilderWithString<DoorDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(await hw.GetMacFromComponentAsync((short)domain.ScpId), Command.INPUT_SPEC));
                }
            }


            foreach (var rex in domain.RequestExits)
            {
                if (rex.ScpId == 0) continue;
                if (!aero.InputPointSpecification((short)rex.ScpId, (short)rex.ModuleDriverId, rex.InputNo, rex.InputMode, rex.Debounce, rex.HoldTime))
                {
                    return ResponseHelper.UnsuccessBuilderWithString<DoorDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(await hw.GetMacFromComponentAsync((short)domain.ScpId), Command.INPUT_SPEC));
                }
            }

            if (!aero.AccessControlReaderConfiguration(domain))
            {
                return ResponseHelper.UnsuccessBuilderWithString<DoorDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(await hw.GetMacFromComponentAsync((short)domain.ScpId), Command.ACR_CONFIG));
            }



            var status = await repo.AddAsync(domain);

            if (status <= 0) return ResponseHelper.UnsuccessBuilder<DoorDto>(ResponseMessage.SAVE_DATABASE_UNSUCCESS, []);

            if(dto.AccessConfig == (int)Aero.Domain.Enums.DoorType.Dual)
            {
                
                domain = new Door(
                0,
                d.ElementAt(1),
                dto.Name,
                dto.AccessConfig,
                d.ElementAt(0),
                DoorDirection.OUT,
                dto.ScpId,
                dto.Readers is null ? new List<Reader>() : new List<Reader>
                {
                    new Reader(
                    dto.Readers.ElementAt(1).ModuleId,
                    dto.Readers.ElementAt(1).ModuleDriverId,
                    dto.Readers.ElementAt(1).DoorId,
                    dto.Readers.ElementAt(1).Direction,
                    dto.Readers.ElementAt(1).ReaderNo,
                    dto.Readers.ElementAt(1).DataFormat,
                    dto.Readers.ElementAt(1).KeypadMode,
                    dto.Readers.ElementAt(1).LedDriveMode,
                    dto.Readers.ElementAt(1).OsdpFlag,
                    dto.Readers.ElementAt(1).OsdpBaudrate,
                    dto.Readers.ElementAt(1).OsdpDiscover,
                    dto.Readers.ElementAt(1).OsdpTracing,
                    dto.Readers.ElementAt(1).OsdpAddress,
                    dto.Readers.ElementAt(1).OsdpSecureChannel,
                    dto.Readers.ElementAt(1).ScpId,
                    dto.Readers.ElementAt(1).LocationId,
                    dto.Readers.ElementAt(1).IsActive
                    )
                }
                ,
                dto.ReaderOutConfiguration,
                    null,
                    null,
                    new List<RequestExit>()
                    , dto.CardFormat, dto.AntiPassbackMode, dto.AntiPassBackIn, dto.AntiPassBackOut, dto.SpareTags, dto.AccessControlFlags, dto.Mode, dto.ModeDesc,
                    dto.OfflineMode, dto.OfflineModeDesc, dto.DefaultMode, dto.DefaultModeDesc, dto.DefaultLEDMode, dto.PreAlarm, dto.AntiPassbackDelay,
                    dto.StrkT2, dto.DcHeld2, dto.StrkFollowPulse, dto.StrkFollowDelay, dto.nExtFeatureType, dto.IlPBSio, dto.IlPBNumber, dto.IlPBLongPress, dto.IlPBOutSio,
                    dto.IlPBOutNum, dto.DfOfFilterTime, dto.MaskHeldOpen, dto.MaskForceOpen
                    );

                    if (!aero.AccessControlReaderConfiguration(domain))
                {
                    return ResponseHelper.UnsuccessBuilderWithString<DoorDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(await hw.GetMacFromComponentAsync((short)domain.ScpId), Command.ACR_CONFIG));
                }

                status = await repo.AddAsync(domain);

                if (status <= 0) return ResponseHelper.UnsuccessBuilder<DoorDto>(ResponseMessage.SAVE_DATABASE_UNSUCCESS, []);
            }

            return ResponseHelper.SuccessBuilder(await repo.GetByIdAsync(status));


        }

        public async Task<ResponseDto<DoorDto>> CreateDualReaderDoorAsync(CreateDoorDto dto)
        {
             throw new NotImplementedException();
        }

        public async Task<ResponseDto<DoorDto>> CreateSingleReaderDoorAsync(CreateDoorDto dto)
        {
             throw new NotImplementedException();
        }


        // public async Task<ResponseDto<DoorDto>> CreateAsync(CreateDoorDto dto)
        // {
        //     // Check value in license here 
        //     // ....to be implement

        //     if (await repo.IsAnyByNameAsync(dto.Name.Trim())) return ResponseHelper.BadRequestName<DoorDto>();

        //     switch (dto.DoorType)
        //     {
        //         // Single In REX Out
        //         case 1:

        //             var ScpSetting = await setting.GetScpSettingAsync();

        //             var DriverId = await repo.GetLowestUnassignedNumberByDeviceIdAsync(ScpSetting.nAcr, dto.AcrId);

        //             if (DriverId == -1) return ResponseHelper.ExceedLimit<DoorDto>();


        //             var domain = new Door(0, DriverId, dto.Name, dto.DoorType, dto.PairDoorNo,
        //             dto.ScpId,
        //             dto.Readers is null || dto.Readers.Count == 0 ? new List<Reader>() :
        //             new List<Reader>
        //             {
        //                 new Reader(
        //                     dto.Readers.ElementAt(0).ModuleId,
        //                     dto.Readers.ElementAt(0).ModuleDriverId,
        //                     dto.Readers.ElementAt(0).DoorId,
        //                     DoorDirection.IN,
        //                     dto.Readers.ElementAt(0).ReaderNo,
        //                     dto.Readers.ElementAt(0).DataFormat,
        //                     dto.Readers.ElementAt(0).KeypadMode,
        //                     dto.Readers.ElementAt(0).LedDriveMode,
        //                     dto.Readers.ElementAt(0).OsdpFlag,
        //                     dto.Readers.ElementAt(0).OsdpBaudrate,
        //                     dto.Readers.ElementAt(0).OsdpDiscover,
        //                     dto.Readers.ElementAt(0).OsdpTracing,
        //                     dto.Readers.ElementAt(0).OsdpAddress,
        //                     dto.Readers.ElementAt(0).OsdpSecureChannel,
        //                     dto.Readers.ElementAt(0).ScpId,
        //                     dto.Readers.ElementAt(0).LocationId,
        //                     dto.Readers.ElementAt(0).IsActive
        //                 )
        //             },
        //             dto.ReaderOutConfiguration,
        //             dto.Strk is null ? null : new Strike(dto.Strk.ScpId, dto.Strk.ModuleId, dto.Strk.ModuleDriverId, dto.Strk.DoorId, dto.Strk.OutputNo, dto.Strk.RelayMode, dto.Strk.OfflineMode, dto.Strk.StrkMax, dto.Strk.StrkMin, dto.Strk.StrkMode, dto.Strk.LocationId, dto.Strk.IsActive),
        //             dto.Sensor is null ? null : new Sensor(dto.Sensor.ScpId, dto.Sensor.ModuleId, dto.Sensor.ModuleDriverId, dto.Sensor.DoorId, dto.Sensor.InputNo, dto.Sensor.InputMode, dto.Sensor.Debounce, dto.Sensor.HoldTime, dto.Sensor.DcHeld, dto.Sensor.LocationId, dto.Sensor.IsActive),
        //             dto.RequestExits is null ? new List<RequestExit>() : dto.RequestExits.Select(x => new RequestExit(x.ScpId, x.ModuleId, x.ModuleDriverId, x.DoorId, x.InputNo, x.InputMode, x.Debounce, x.HoldTime, x.MaskTimeZone, x.LocationId, x.IsActive)).ToList()
        //             , dto.CardFormat, dto.AntiPassbackMode, dto.AntiPassBackIn, dto.AntiPassBackOut, dto.SpareTags, dto.AccessControlFlags, dto.Mode, dto.ModeDesc,
        //             dto.OfflineMode, dto.OfflineModeDesc, dto.DefaultMode, dto.DefaultModeDesc, dto.DefaultLEDMode, dto.PreAlarm, dto.AntiPassbackDelay,
        //             dto.StrkT2, dto.DcHeld2, dto.StrkFollowPulse, dto.StrkFollowDelay, dto.nExtFeatureType, dto.IlPBSio, dto.IlPBNumber, dto.IlPBLongPress, dto.IlPBOutSio,
        //             dto.IlPBOutNum, dto.DfOfFilterTime, dto.MaskHeldOpen, dto.MaskForceOpen
        //             );

        //             foreach (var reader in domain.Readers)
        //             {
        //                 if (reader.ScpId == 0) continue;
        //                 short readerInOsdpFlag = 0x00;
        //                 short readerLedDriveMode = 0;
        //                 if (reader.OsdpFlag)
        //                 {
        //                     readerInOsdpFlag |= reader.OsdpBaudrate;
        //                     readerInOsdpFlag |= reader.OsdpDiscover;
        //                     readerInOsdpFlag |= reader.OsdpTracing;
        //                     readerInOsdpFlag |= reader.OsdpAddress;
        //                     readerInOsdpFlag |= reader.OsdpSecureChannel;
        //                     readerLedDriveMode = 7;
        //                 }
        //                 else
        //                 {
        //                     readerLedDriveMode = 1;
        //                 }


        //                 // Reader In Config
        //                 if (!aero.ReaderSpecification((short)reader.ScpId, reader.ModuleDriverId, reader.ReaderNo, reader.DataFormat, reader.KeypadMode, readerLedDriveMode, readerInOsdpFlag))
        //                 {
        //                     return ResponseHelper.UnsuccessBuilderWithString<DoorDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(await hw.GetMacFromComponentAsync((short)domain.ScpId), Command.READER_SPEC));
        //                 }
        //             }



        //             // Strike Strike Config
        //             if (dto.Strk != null)
        //             {
        //                 if (!aero.OutputPointSpecification((short)dto.Strk.ScpId, (short)domain.Strk.ModuleDriverId, domain.Strk.OutputNo, domain.Strk.RelayMode))
        //                 {
        //                     return ResponseHelper.UnsuccessBuilderWithString<DoorDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(await hw.GetMacFromComponentAsync((short)domain.ScpId), Command.OUTPUT_SPEC));
        //                 }

        //             }

        //             // door sensor Config
        //             if (dto.Sensor != null)
        //             {
        //                 if (!aero.InputPointSpecification((short)domain.Sensor.ScpId, (short)domain.Sensor.ModuleDriverId, domain.Sensor.InputNo, domain.Sensor.InputMode, domain.Sensor.Debounce, domain.Sensor.HoldTime))
        //                 {
        //                     return ResponseHelper.UnsuccessBuilderWithString<DoorDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(await hw.GetMacFromComponentAsync((short)domain.ScpId), Command.INPUT_SPEC));
        //                 }
        //             }


        //             foreach (var rex in domain.RequestExits)
        //             {
        //                 if (rex.ScpId == 0) continue;
        //                 if (!aero.InputPointSpecification((short)rex.ScpId, (short)rex.ModuleDriverId, rex.InputNo, rex.InputMode, rex.Debounce, rex.HoldTime))
        //                 {
        //                     return ResponseHelper.UnsuccessBuilderWithString<DoorDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(await hw.GetMacFromComponentAsync((short)domain.ScpId), Command.INPUT_SPEC));
        //                 }
        //             }

        //             if (!aero.AccessControlReaderConfiguration((short)domain.ScpId, domain.AcrId, domain, 0))
        //             {
        //                 return ResponseHelper.UnsuccessBuilderWithString<DoorDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(await hw.GetMacFromComponentAsync((short)domain.ScpId), Command.ACR_CONFIG));
        //             }



        //             var status = await repo.AddAsync(domain);

        //             if (status <= 0) return ResponseHelper.UnsuccessBuilder<DoorDto>(ResponseMessage.SAVE_DATABASE_UNSUCCESS, []);

        //             return ResponseHelper.SuccessBuilder(await repo.GetByIdAsync(status));
        //         // Reader In-Out (Wiegand)
        //         case 2:
        //             ScpSetting = await setting.GetScpSettingAsync();

        //             var DriverIdIn = await repo.GetLowestUnassignedNumberByDeviceIdAsync(ScpSetting.nAcr, dto.AcrId);
        //             var DriverIdOut = await repo.GetLowestUnassignedNumberByDeviceIdAsync(ScpSetting.nAcr, dto.AcrId);

        //             if (DriverIdIn == -1) return ResponseHelper.ExceedLimit<DoorDto>();
        //             if (DriverIdOut == -1) return ResponseHelper.ExceedLimit<DoorDto>();


        //             var domainIn = new Door(0, DriverIdIn, dto.Name, dto.DoorType, DriverIdOut,
        //             dto.ScpId,
        //             dto.Readers is null || dto.Readers.Count == 0 ? new List<Reader>() :
        //             new List<Reader>
        //             {
        //                 new Reader(
        //                     dto.Readers.ElementAt(0).ModuleId,
        //                     dto.Readers.ElementAt(0).ModuleDriverId,
        //                     dto.Readers.ElementAt(0).DoorId,
        //                     DoorDirection.IN,
        //                     dto.Readers.ElementAt(0).ReaderNo,
        //                     dto.Readers.ElementAt(0).DataFormat,
        //                     dto.Readers.ElementAt(0).KeypadMode,
        //                     dto.Readers.ElementAt(0).LedDriveMode,
        //                     dto.Readers.ElementAt(0).OsdpFlag,
        //                     dto.Readers.ElementAt(0).OsdpBaudrate,
        //                     dto.Readers.ElementAt(0).OsdpDiscover,
        //                     dto.Readers.ElementAt(0).OsdpTracing,
        //                     dto.Readers.ElementAt(0).OsdpAddress,
        //                     dto.Readers.ElementAt(0).OsdpSecureChannel,
        //                     dto.Readers.ElementAt(0).ScpId,
        //                     dto.Readers.ElementAt(0).LocationId,
        //                     dto.Readers.ElementAt(0).IsActive
        //                 )
        //             },
        //             dto.ReaderOutConfiguration,
        //             dto.Strk is null ? null : new Strike(dto.Strk.ScpId, dto.Strk.ModuleId, dto.Strk.ModuleDriverId, dto.Strk.DoorId, dto.Strk.OutputNo, dto.Strk.RelayMode, dto.Strk.OfflineMode, dto.Strk.StrkMax, dto.Strk.StrkMin, dto.Strk.StrkMode, dto.Strk.LocationId, dto.Strk.IsActive),
        //             dto.Sensor is null ? null : new Sensor(dto.Sensor.ScpId, dto.Sensor.ModuleId, dto.Sensor.ModuleDriverId, dto.Sensor.DoorId, dto.Sensor.InputNo, dto.Sensor.InputMode, dto.Sensor.Debounce, dto.Sensor.HoldTime, dto.Sensor.DcHeld, dto.Sensor.LocationId, dto.Sensor.IsActive),
        //             new List<RequestExit>(),
        //             dto.CardFormat, dto.AntiPassbackMode, dto.AntiPassBackIn, dto.AntiPassBackOut, dto.SpareTags, dto.AccessControlFlags, dto.Mode, dto.ModeDesc,
        //             dto.OfflineMode, dto.OfflineModeDesc, dto.DefaultMode, dto.DefaultModeDesc, dto.DefaultLEDMode, dto.PreAlarm, dto.AntiPassbackDelay,
        //             dto.StrkT2, dto.DcHeld2, dto.StrkFollowPulse, dto.StrkFollowDelay, dto.nExtFeatureType, dto.IlPBSio, dto.IlPBNumber, dto.IlPBLongPress, dto.IlPBOutSio,
        //             dto.IlPBOutNum, dto.DfOfFilterTime, dto.MaskHeldOpen, dto.MaskForceOpen
        //             );

        //             var domainOut = new Door(0, DriverIdOut, dto.Name, dto.DoorType, DriverIdIn,
        //             dto.ScpId,
        //             dto.Readers is null || dto.Readers.Count == 0 ? new List<Reader>() :
        //             new List<Reader>
        //             {
        //                 new Reader(
        //                     dto.Readers.ElementAt(1).ModuleId,
        //                     dto.Readers.ElementAt(1).ModuleDriverId,
        //                     dto.Readers.ElementAt(1).DoorId,
        //                     DoorDirection.OUT,
        //                     dto.Readers.ElementAt(1).ReaderNo,
        //                     dto.Readers.ElementAt(1).DataFormat,
        //                     dto.Readers.ElementAt(1).KeypadMode,
        //                     dto.Readers.ElementAt(1).LedDriveMode,
        //                     dto.Readers.ElementAt(1).OsdpFlag,
        //                     dto.Readers.ElementAt(1).OsdpBaudrate,
        //                     dto.Readers.ElementAt(1).OsdpDiscover,
        //                     dto.Readers.ElementAt(1).OsdpTracing,
        //                     dto.Readers.ElementAt(1).OsdpAddress,
        //                     dto.Readers.ElementAt(1).OsdpSecureChannel,
        //                     dto.Readers.ElementAt(1).ScpId,
        //                     dto.Readers.ElementAt(1).LocationId,
        //                     dto.Readers.ElementAt(1).IsActive
        //                 )
        //             },
        //             dto.ReaderOutConfiguration,
        //             null,
        //             null,
        //             new List<RequestExit>(),
        //             dto.CardFormat, dto.AntiPassbackMode, dto.AntiPassBackIn, dto.AntiPassBackOut, dto.SpareTags, dto.AccessControlFlags, dto.Mode, dto.ModeDesc,
        //             dto.OfflineMode, dto.OfflineModeDesc, dto.DefaultMode, dto.DefaultModeDesc, dto.DefaultLEDMode, dto.PreAlarm, dto.AntiPassbackDelay,
        //             dto.StrkT2, dto.DcHeld2, dto.StrkFollowPulse, dto.StrkFollowDelay, dto.nExtFeatureType, dto.IlPBSio, dto.IlPBNumber, dto.IlPBLongPress, dto.IlPBOutSio,
        //             dto.IlPBOutNum, dto.DfOfFilterTime, dto.MaskHeldOpen, dto.MaskForceOpen
        //             );

        //             foreach (var reader in domainIn.Readers)
        //             {
        //                 if (reader.ScpId == 0) continue;
        //                 short readerInOsdpFlag = 0x00;
        //                 short readerLedDriveMode = 0;
        //                 if (reader.OsdpFlag)
        //                 {
        //                     readerInOsdpFlag |= reader.OsdpBaudrate;
        //                     readerInOsdpFlag |= reader.OsdpDiscover;
        //                     readerInOsdpFlag |= reader.OsdpTracing;
        //                     readerInOsdpFlag |= reader.OsdpAddress;
        //                     readerInOsdpFlag |= reader.OsdpSecureChannel;
        //                     readerLedDriveMode = 7;
        //                 }
        //                 else
        //                 {
        //                     readerLedDriveMode = 1;
        //                 }


        //                 // Reader In Config
        //                 if (!aero.ReaderSpecification((short)reader.ScpId, reader.ModuleDriverId, reader.ReaderNo, reader.DataFormat, reader.KeypadMode, readerLedDriveMode, readerInOsdpFlag))
        //                 {
        //                     return ResponseHelper.UnsuccessBuilderWithString<DoorDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(await hw.GetMacFromComponentAsync((short)domainIn.ScpId), Command.READER_SPEC));
        //                 }
        //             }

        //             foreach (var reader in domainOut.Readers)
        //             {
        //                 if (reader.ScpId == 0) continue;
        //                 short readerInOsdpFlag = 0x00;
        //                 short readerLedDriveMode = 0;
        //                 if (reader.OsdpFlag)
        //                 {
        //                     readerInOsdpFlag |= reader.OsdpBaudrate;
        //                     readerInOsdpFlag |= reader.OsdpDiscover;
        //                     readerInOsdpFlag |= reader.OsdpTracing;
        //                     readerInOsdpFlag |= reader.OsdpAddress;
        //                     readerInOsdpFlag |= reader.OsdpSecureChannel;
        //                     readerLedDriveMode = 7;
        //                 }
        //                 else
        //                 {
        //                     readerLedDriveMode = 1;
        //                 }


        //                 // Reader In Config
        //                 if (!aero.ReaderSpecification((short)reader.ScpId, reader.ModuleDriverId, reader.ReaderNo, reader.DataFormat, reader.KeypadMode, readerLedDriveMode, readerInOsdpFlag))
        //                 {
        //                     return ResponseHelper.UnsuccessBuilderWithString<DoorDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(await hw.GetMacFromComponentAsync((short)domainOut.ScpId), Command.READER_SPEC));
        //                 }
        //             }



        //             // Strike Strike Config
        //             if (domainIn.Strk != null)
        //             {
        //                 if (!aero.OutputPointSpecification((short)domainIn.Strk.ScpId, (short)domainIn.Strk.ModuleDriverId, domainIn.Strk.OutputNo, domainIn.Strk.RelayMode))
        //                 {
        //                     return ResponseHelper.UnsuccessBuilderWithString<DoorDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(await hw.GetMacFromComponentAsync((short)domainIn.ScpId), Command.OUTPUT_SPEC));
        //                 }

        //             }

        //             // door sensor Config
        //             if (domainIn.Sensor != null)
        //             {
        //                 if (!aero.InputPointSpecification((short)domainIn.Sensor.ScpId, (short)domainIn.Sensor.ModuleDriverId, domainIn.Sensor.InputNo, domainIn.Sensor.InputMode, domainIn.Sensor.Debounce, domainIn.Sensor.HoldTime))
        //                 {
        //                     return ResponseHelper.UnsuccessBuilderWithString<DoorDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(await hw.GetMacFromComponentAsync((short)domainIn.ScpId), Command.INPUT_SPEC));
        //                 }
        //             }


        //             // foreach (var rex in domain.RequestExits)
        //             // {
        //             //     if (rex.ScpId == 0) continue;
        //             //     if (!aero.InputPointSpecification((short)rex.ScpId, (short)rex.ModuleDriverId, rex.InputNo, rex.InputMode, rex.Debounce, rex.HoldTime))
        //             //     {
        //             //         return ResponseHelper.UnsuccessBuilderWithString<DoorDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(await hw.GetMacFromComponentAsync((short)domain.ScpId), Command.INPUT_SPEC));
        //             //     }
        //             // }

        //             if (!aero.AccessControlReaderConfiguration((short)domainIn.ScpId, domainIn.AcrId, domainIn, 1))
        //             {
        //                 return ResponseHelper.UnsuccessBuilderWithString<DoorDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(await hw.GetMacFromComponentAsync((short)domainIn.ScpId), Command.ACR_CONFIG));
        //             }

        //             if (!aero.AccessControlReaderConfiguration((short)domainOut.ScpId, domainOut.AcrId, domainOut, 2))
        //             {
        //                 return ResponseHelper.UnsuccessBuilderWithString<DoorDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(await hw.GetMacFromComponentAsync((short)domainOut.ScpId), Command.ACR_CONFIG));
        //             }



        //             status = await repo.AddAsync(domainIn);
        //             if (status <= 0) return ResponseHelper.UnsuccessBuilder<DoorDto>(ResponseMessage.SAVE_DATABASE_UNSUCCESS, []);

        //             status = await repo.AddAsync(domainOut);
        //             if (status <= 0) return ResponseHelper.UnsuccessBuilder<DoorDto>(ResponseMessage.SAVE_DATABASE_UNSUCCESS, []);

        //             return ResponseHelper.SuccessBuilder(await repo.GetByIdAsync(status));
        //         // // Reader In-Out (OSDP)
        //         // case 3:
        //         // break;
        //         default:
        //             return ResponseHelper.UnsuccessBuilderWithString<DoorDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(await hw.GetMacFromComponentAsync((short)dto.ScpId), Command.ACR_CONFIG));
        //     }


        // }

        public async Task<ResponseDto<DoorDto>> DeleteAsync(int id)
        {
            var dto = await repo.GetByIdAsync(id);
            if (dto is null) return ResponseHelper.NotFoundBuilder<DoorDto>();

            // Send Command 
            var domain = new Door(
            dto.Id,
            dto.AcrId,
            dto.Name,
            0,
            dto.PairDoorNo,
           dto.Direction,
            (short)dto.ScpId,
            new List<Reader>(),
            dto.ReaderOutConfiguration,
            null,
            null,
            new List<RequestExit>(),
            dto.CardFormat, dto.AntiPassbackMode, dto.AreaInId,
           dto.AreaOutId, dto.SpareTags, dto.AccessControlFlags, dto.Mode, dto.ModeDesc,
            dto.OfflineMode, dto.OfflineModeDesc, dto.DefaultMode, dto.DefaultModeDesc, dto.DefaultLEDMode, dto.PreAlarm, dto.AntiPassbackDelay,
            dto.StrkT2, dto.DcHeld2, dto.StrkFollowPulse, dto.StrkFollowDelay, dto.nExtFeatureType, dto.IlPBSio, dto.IlPBNumber, dto.IlPBLongPress, dto.IlPBOutSio,
            dto.IlPBOutNum, dto.DfOfFilterTime, dto.MaskHeldOpen, dto.MaskForceOpen
            );


            if (!aero.AccessControlReaderConfiguration(domain))
            {
                return ResponseHelper.UnsuccessBuilderWithString<DoorDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(await hw.GetMacFromComponentAsync((short)domain.ScpId), Command.ACR_CONFIG));
            }

            // 
            var status = await repo.DeleteByIdAsync(id);
            if (status <= 0) return ResponseHelper.UnsuccessBuilder<DoorDto>(ResponseMessage.DELETE_DATABASE_UNSUCCESS, []);
            return ResponseHelper.SuccessBuilder<DoorDto>(dto);
        }

        public async Task<ResponseDto<DoorDto>> UpdateAsync(DoorDto dto)
        {

            var en = await repo.GetByIdAsync(dto.Id);
            
            if (en is null) return ResponseHelper.NotFoundBuilder<DoorDto>();

            var domain = new Door(0, dto.AcrId, dto.Name, (short)dto.DoorType, dto.PairDoorNo,dto.Direction,
            (short)dto.ScpId,
            dto.Readers.Select(x => new Reader(x.ModuleId, x.ModuleDriverId, x.DoorId, x.Direction, x.ReaderNo, x.DataFormat, x.KeypadMode, x.LedDriveMode, x.OsdpFlag, x.OsdpBaudrate, x.OsdpDiscover, x.OsdpTracing, x.OsdpAddress, x.OsdpSecureChannel, x.ScpId, x.LocationId, x.IsActive)).ToList(),
            dto.ReaderOutConfiguration,
            dto.Strk is null ? null : new Strike(dto.Strk.ScpId, dto.Strk.ModuleId, dto.Strk.ModuleDriverId, dto.Strk.DoorId, dto.Strk.OutputNo, dto.Strk.RelayMode, dto.Strk.OfflineMode, dto.Strk.StrkMax, dto.Strk.StrkMin, dto.Strk.StrkMode, dto.Strk.LocationId, dto.Strk.IsActive),
            dto.Sensor is null ? null : new Sensor(dto.Sensor.ScpId, dto.Sensor.ModuleId, dto.Sensor.ModuleDriverId, dto.Sensor.DoorId, dto.Sensor.InputNo, dto.Sensor.InputMode, dto.Sensor.Debounce, dto.Sensor.HoldTime, dto.Sensor.DcHeld, dto.Sensor.LocationId, dto.Sensor.IsActive),
            dto.RequestExits is null ? new List<RequestExit>() : dto.RequestExits.Select(x => new RequestExit(x.ScpId, x.ModuleId, x.ModuleDriverId, x.DoorId, x.InputNo, x.InputMode, x.Debounce, x.HoldTime, x.MaskTimeZone, x.LocationId, x.IsActive)).ToList()
            , dto.CardFormat, dto.AntiPassbackMode, dto.AreaInId,
            dto.AreaOutId, dto.SpareTags, dto.AccessControlFlags, dto.Mode, dto.ModeDesc,
            dto.OfflineMode, dto.OfflineModeDesc, dto.DefaultMode, dto.DefaultModeDesc, dto.DefaultLEDMode, dto.PreAlarm, dto.AntiPassbackDelay,
            dto.StrkT2, dto.DcHeld2, dto.StrkFollowPulse, dto.StrkFollowDelay, dto.nExtFeatureType, dto.IlPBSio, dto.IlPBNumber, dto.IlPBLongPress, dto.IlPBOutSio,
            dto.IlPBOutNum, dto.DfOfFilterTime, dto.MaskHeldOpen, dto.MaskForceOpen
            );

            foreach (var reader in domain.Readers)
            {
                if (reader.ScpId == 0) continue;
                short readerInOsdpFlag = 0x00;
                short readerLedDriveMode = 0;
                if (reader.OsdpFlag)
                {
                    readerInOsdpFlag += reader.OsdpBaudrate;
                    readerInOsdpFlag |= reader.OsdpDiscover;
                    readerInOsdpFlag |= reader.OsdpTracing;
                    readerInOsdpFlag |= (short)(reader.OsdpAddress << 5); 
                    readerInOsdpFlag |= reader.OsdpSecureChannel;
                    readerLedDriveMode = 7;
                }
                else
                {
                    readerLedDriveMode = 1;
                }


                // Reader In Config
                if (!aero.ReaderSpecification((short)reader.ScpId, reader.ModuleDriverId, reader.ReaderNo, reader.DataFormat, reader.KeypadMode, readerLedDriveMode, readerInOsdpFlag))
                {
                    return ResponseHelper.UnsuccessBuilderWithString<DoorDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(await hw.GetMacFromComponentAsync((short)domain.ScpId), Command.READER_SPEC));
                }
            }

            // Strike Strike Config
            if (!aero.OutputPointSpecification((short)domain.Strk.ScpId, (short)domain.Strk.ModuleDriverId, domain.Strk.OutputNo, domain.Strk.RelayMode))
            {
                return ResponseHelper.UnsuccessBuilderWithString<DoorDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(await hw.GetMacFromComponentAsync((short)domain.ScpId), Command.OUTPUT_SPEC));
            }

            // door sensor Config
            if (!aero.InputPointSpecification((short)domain.Sensor.ScpId, (short)domain.Sensor.ModuleDriverId, domain.Sensor.InputNo, domain.Sensor.InputMode, domain.Sensor.Debounce, domain.Sensor.HoldTime))
            {
                return ResponseHelper.UnsuccessBuilderWithString<DoorDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(await hw.GetMacFromComponentAsync((short)domain.ScpId), Command.INPUT_SPEC));
            }

            foreach (var rex in domain.RequestExits)
            {
                if (rex.ScpId == 0) continue;
                if (!aero.InputPointSpecification((short)rex.ScpId, (short)rex.ModuleDriverId, rex.InputNo, rex.InputMode, rex.Debounce, rex.HoldTime))
                {
                    return ResponseHelper.UnsuccessBuilderWithString<DoorDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(await hw.GetMacFromComponentAsync((short)domain.ScpId), Command.INPUT_SPEC));
                }
            }

            if (!aero.AccessControlReaderConfiguration((short)domain.ScpId, dto.AcrId, domain, 1))
            {
                return ResponseHelper.UnsuccessBuilderWithString<DoorDto>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(await hw.GetMacFromComponentAsync((short)domain.ScpId), Command.ACR_MODE));
            }

            var status = await repo.UpdateAsync(domain);

            if (status <= 0) return ResponseHelper.UnsuccessBuilder<DoorDto>(ResponseMessage.UPDATE_RECORD_UNSUCCESS, []);

            return ResponseHelper.SuccessBuilder(dto);
        }

        public async Task<ResponseDto<bool>> GetStatusAsync(int id)
        {
            var data = await repo.GetByIdAsync(id);
            if (!aero.GetAcrStatus((short)data.ScpId, data.AcrId, 1))
            {
                return ResponseHelper.UnsuccessBuilderWithString<bool>(ResponseMessage.COMMAND_UNSUCCESS, MessageBuilder.Unsuccess(await hw.GetMacFromComponentAsync((short)data.ScpId), Command.ACR_STATUS));
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
                case Aero.Domain.Enums.DoorServiceMode.DoorType:
                    return await GetDoorTypeAsync();
                default:
                    return ResponseHelper.UnsuccessBuilderWithString<IEnumerable<ModeDto>>(ResponseMessage.NOT_FOUND, ResponseMessage.NOT_FOUND);
            }

        }

        public async Task<ResponseDto<IEnumerable<ModeDto>>> GetSpareFlagAsync()
        {
            var dtos = await repo.GetDoorSpareFlagAsync();
            return ResponseHelper.SuccessBuilder<IEnumerable<ModeDto>>(dtos);
        }

        public async Task<ResponseDto<IEnumerable<ModeDto>>> GetDoorTypeAsync()
        {
            var dtos = await repo.GetDoorTypeAsync();
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

        public async Task<ResponseDto<IEnumerable<short>>> GetAvailableOsdpAddress(int moduleId)
        {
            var dtos = await repo.GetAvailableReaderFromModuleIddAsync(moduleId);
            return ResponseHelper.SuccessBuilder<IEnumerable<short>>(dtos);
        }

        public async Task<ResponseDto<DoorDto>> GetByComponentAsync(short component)
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
