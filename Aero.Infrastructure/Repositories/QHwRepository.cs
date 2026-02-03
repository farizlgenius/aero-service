using System;
using Aero.Application.DTOs;
using Aero.Application.Helpers;
using Aero.Application.Interfaces;
using Aero.Domain.Entities;
using Aero.Domain.Interface;
using Aero.Domain.Interfaces;
using Aero.Infrastructure.Data;
using Aero.Infrastructure.Helpers;
using Microsoft.EntityFrameworkCore;
using static Aero.Infrastructure.Helpers.DescriptionHelper;

namespace Aero.Infrastructure.Repositories;

public sealed class QHwRepository(AppDbContext context) : IQHwRepository
{
    
    public async Task<IEnumerable<HardwareDto>> GetAsync()
    {
        var dtos = await context.hardware
            .AsNoTracking()
            .Select(hardware => new HardwareDto
            {
                // Base 
                ComponentId = hardware.component_id,
                Mac = hardware.mac,
                LocationId = hardware.location_id,
                IsActive = hardware.is_active,

                // extend_desc
                Name = hardware.name,
                HardwareType = hardware.hardware_type,
                HardwareTypeDescription = hardware.hardware_type_desc,
                Firmware = hardware.firmware,
                Ip = hardware.ip,
                Port = hardware.port,
                SerialNumber = hardware.serial_number,
                IsReset = hardware.is_reset,
                IsUpload = hardware.is_upload,
                Modules = hardware.modules.Select(d => new ModuleDto
                {
                    // Base 
                    ComponentId = d.component_id,
                    HardwareName = hardware.name,
                    Mac = hardware.mac,
                    LocationId = d.location_id,
                    IsActive = d.is_active,

                    // extend_desc
                    Model = d.model,
                    ModelDescription = d.model_desc,
                    Revision = d.revision,
                    SerialNumber = d.serial_number,
                    nHardwareId = d.n_hardware_id,
                    nHardwareIdDescription = d.n_hardware_id_desc,
                    nHardwareRev = d.n_hardware_rev,
                    nProductId = d.n_product_id,
                    nProductVer = d.n_product_ver,
                    nEncConfig = d.n_enc_config,
                    nEncConfigDescription = d.n_enc_config_desc,
                    nEncKeyStatus = d.n_enc_key_status,
                    nEncKeyStatusDescription = d.n_enc_key_status_desc,
                    Readers = null,
                    Sensors = null,
                    Strikes = null,
                    RequestExits = null,
                    MonitorPoints = null,
                    ControlPoints = null,
                    Address = d.address,
                    Port = d.port,
                    nInput = d.n_input,
                    nOutput = d.n_output,
                    nReader = d.n_reader,
                    Msp1No = d.msp1_no,
                    BaudRate = d.baudrate,
                    nProtocol = d.n_protocol,
                    nDialect = d.n_dialect,
                }).ToList(),
                PortOne = hardware.port_one,
                ProtocolOne = hardware.protocol_one,
                ProtocolOneDescription = hardware.protocol_one_desc,
                PortTwo = hardware.port_two,
                ProtocolTwoDescription = hardware.protocol_two_desc,
                ProtocolTwo = hardware.protocol_two,
                BaudRateOne = hardware.baudrate_one,
                BaudRateTwo = hardware.baudrate_two,

            })
            .ToArrayAsync();

        return dtos;
    }

    public async Task<HardwareDto> GetByComponentIdAsync(short componentId)
    {
        var dto = await context.hardware
            .AsNoTracking()
            .Where(x => x.component_id == componentId)
            .Select(hardware => new HardwareDto
            {
                // Base 
                ComponentId = hardware.component_id,
                Mac = hardware.mac,
                LocationId = hardware.location_id,
                IsActive = hardware.is_active,

                // extend_desc
                Name = hardware.name,
                HardwareType = hardware.hardware_type,
                HardwareTypeDescription = hardware.hardware_type_desc,
                Firmware = hardware.firmware,
                Ip = hardware.ip,
                Port = hardware.port,
                SerialNumber = hardware.serial_number,
                IsReset = hardware.is_reset,
                IsUpload = hardware.is_upload,
                Modules = hardware.modules.Select(d => new ModuleDto
                {
                    // Base 
                    ComponentId = d.component_id,
                    HardwareName = hardware.name,
                    Mac = hardware.mac,
                    LocationId = d.location_id,
                    IsActive = d.is_active,

                    // extend_desc
                    Model = d.model,
                    ModelDescription = d.model_desc,
                    Revision = d.revision,
                    SerialNumber = d.serial_number,
                    nHardwareId = d.n_hardware_id,
                    nHardwareIdDescription = d.n_hardware_id_desc,
                    nHardwareRev = d.n_hardware_rev,
                    nProductId = d.n_product_id,
                    nProductVer = d.n_product_ver,
                    nEncConfig = d.n_enc_config,
                    nEncConfigDescription = d.n_enc_config_desc,
                    nEncKeyStatus = d.n_enc_key_status,
                    nEncKeyStatusDescription = d.n_enc_key_status_desc,
                    Readers = null,
                    Sensors = null,
                    Strikes = null,
                    RequestExits = null,
                    MonitorPoints = null,
                    ControlPoints = null,
                    Address = d.address,
                    Port = d.port,
                    nInput = d.n_input,
                    nOutput = d.n_output,
                    nReader = d.n_reader,
                    Msp1No = d.msp1_no,
                    BaudRate = d.baudrate,
                    nProtocol = d.n_protocol,
                    nDialect = d.n_dialect,
                }).ToList(),
                PortOne = hardware.port_one,
                ProtocolOne = hardware.protocol_one,
                ProtocolOneDescription = hardware.protocol_one_desc,
                PortTwo = hardware.port_two,
                ProtocolTwoDescription = hardware.protocol_two_desc,
                ProtocolTwo = hardware.protocol_two,
                BaudRateOne = hardware.baudrate_one,
                BaudRateTwo = hardware.baudrate_two,

            })
            .OrderBy(x => x.ComponentId)
            .FirstOrDefaultAsync();

        return dto;
    }

    public async Task<IEnumerable<HardwareDto>> GetByLocationIdAsync(short locationId)
    {
        var dto = await context.hardware
            .AsNoTracking()
            .Where(x => x.location_id == locationId)
            .Select(hardware => new HardwareDto
            {
                // Base 
                ComponentId = hardware.component_id,
                Mac = hardware.mac,
                LocationId = hardware.location_id,
                IsActive = hardware.is_active,

                // extend_desc
                Name = hardware.name,
                HardwareType = hardware.hardware_type,
                HardwareTypeDescription = hardware.hardware_type_desc,
                Firmware = hardware.firmware,
                Ip = hardware.ip,
                Port = hardware.port,
                SerialNumber = hardware.serial_number,
                IsReset = hardware.is_reset,
                IsUpload = hardware.is_upload,
                Modules = hardware.modules.Select(d => new ModuleDto
                {
                    // Base 
                    ComponentId = d.component_id,
                    HardwareName = hardware.name,
                    Mac = hardware.mac,
                    LocationId = d.location_id,
                    IsActive = d.is_active,

                    // extend_desc
                    Model = d.model,
                    ModelDescription = d.model_desc,
                    Revision = d.revision,
                    SerialNumber = d.serial_number,
                    nHardwareId = d.n_hardware_id,
                    nHardwareIdDescription = d.n_hardware_id_desc,
                    nHardwareRev = d.n_hardware_rev,
                    nProductId = d.n_product_id,
                    nProductVer = d.n_product_ver,
                    nEncConfig = d.n_enc_config,
                    nEncConfigDescription = d.n_enc_config_desc,
                    nEncKeyStatus = d.n_enc_key_status,
                    nEncKeyStatusDescription = d.n_enc_key_status_desc,
                    Readers = null,
                    Sensors = null,
                    Strikes = null,
                    RequestExits = null,
                    MonitorPoints = null,
                    ControlPoints = null,
                    Address = d.address,
                    Port = d.port,
                    nInput = d.n_input,
                    nOutput = d.n_output,
                    nReader = d.n_reader,
                    Msp1No = d.msp1_no,
                    BaudRate = d.baudrate,
                    nProtocol = d.n_protocol,
                    nDialect = d.n_dialect,
                }).ToList(),
                PortOne = hardware.port_one,
                ProtocolOne = hardware.protocol_one,
                ProtocolOneDescription = hardware.protocol_one_desc,
                PortTwo = hardware.port_two,
                ProtocolTwoDescription = hardware.protocol_two_desc,
                ProtocolTwo = hardware.protocol_two,
                BaudRateOne = hardware.baudrate_one,
                BaudRateTwo = hardware.baudrate_two,

            })
            .OrderBy(x => x.ComponentId)
            .ToArrayAsync();

        return dto;
    }

    public async Task<HardwareDto> GetByMacAsync(string mac)
    {
        var dto = await context.hardware
            .AsNoTracking()
            .Where(x => x.mac.Equals(mac))
            .Select(hardware => new HardwareDto
            {
                // Base 
                ComponentId = hardware.component_id,
                Mac = hardware.mac,
                LocationId = hardware.location_id,
                IsActive = hardware.is_active,

                // extend_desc
                Name = hardware.name,
                HardwareType = hardware.hardware_type,
                HardwareTypeDescription = hardware.hardware_type_desc,
                Firmware = hardware.firmware,
                Ip = hardware.ip,
                Port = hardware.port,
                SerialNumber = hardware.serial_number,
                IsReset = hardware.is_reset,
                IsUpload = hardware.is_upload,
                Modules = hardware.modules.Select(d => new ModuleDto
                {
                    // Base 
                    ComponentId = d.component_id,
                    HardwareName = hardware.name,
                    Mac = hardware.mac,
                    LocationId = d.location_id,
                    IsActive = d.is_active,

                    // extend_desc
                    Model = d.model,
                    ModelDescription = d.model_desc,
                    Revision = d.revision,
                    SerialNumber = d.serial_number,
                    nHardwareId = d.n_hardware_id,
                    nHardwareIdDescription = d.n_hardware_id_desc,
                    nHardwareRev = d.n_hardware_rev,
                    nProductId = d.n_product_id,
                    nProductVer = d.n_product_ver,
                    nEncConfig = d.n_enc_config,
                    nEncConfigDescription = d.n_enc_config_desc,
                    nEncKeyStatus = d.n_enc_key_status,
                    nEncKeyStatusDescription = d.n_enc_key_status_desc,
                    Readers = null,
                    Sensors = null,
                    Strikes = null,
                    RequestExits = null,
                    MonitorPoints = null,
                    ControlPoints = null,
                    Address = d.address,
                    Port = d.port,
                    nInput = d.n_input,
                    nOutput = d.n_output,
                    nReader = d.n_reader,
                    Msp1No = d.msp1_no,
                    BaudRate = d.baudrate,
                    nProtocol = d.n_protocol,
                    nDialect = d.n_dialect,
                }).ToList(),
                PortOne = hardware.port_one,
                ProtocolOne = hardware.protocol_one,
                ProtocolOneDescription = hardware.protocol_one_desc,
                PortTwo = hardware.port_two,
                ProtocolTwoDescription = hardware.protocol_two_desc,
                ProtocolTwo = hardware.protocol_two,
                BaudRateOne = hardware.baudrate_one,
                BaudRateTwo = hardware.baudrate_two,

            })
            .OrderBy(x => x.ComponentId)
            .FirstOrDefaultAsync();

        return dto;
    }

    public async Task<short> GetComponentIdFromMacAsync(string mac)
    {
        var res = await context.hardware
        .AsNoTracking()
        .Where(x => x.mac.Equals(mac))
        .OrderBy(x => x.component_id)
        .Select(x => x.component_id)
        .FirstOrDefaultAsync();

        return res;
    }

    public async Task<string> GetMacFromComponentAsync(short component)
    {
        var res = await context.hardware
        .AsNoTracking()
        .Where(x => x.component_id == component)
        .OrderBy(x => x.component_id)
        .Select(x => x.mac)
        .FirstOrDefaultAsync() ?? "";

        return res;
    }

    public async Task<ScpSetting> GetScpSettingAsync()
    {
        var res = await context.scp_setting
        .AsNoTracking()
        .OrderBy(x => x.id)
        .Select(x => new Aero.Domain.Entities.ScpSetting
        {
            nMsp1Port = x.n_msp1_port,
            nTransaction = x.n_transaction,
            nSio = x.n_sio,
            nMp = x.n_mp,
            nCp = x.n_cp,
            nAcr = x.n_acr,
            nAlvl = x.n_alvl,
            nTrgr = x.n_trgr,
            nProc = x.n_proc,
            gmtOffset = x.gmt_offset,
            nTz = x.n_tz,
            nHol = x.n_hol,
            nMpg = x.n_mpg,
            nCard = x.n_card,
            nArea = x.n_area,
        })
        .FirstOrDefaultAsync();

        return res;
    }

    public async Task<bool> IsAnyByComponentId(short component)
    {
        return await context.hardware
        .AsNoTracking()
        .Where(x => x.component_id == component)
        .AnyAsync();
    }

    public async Task<bool> IsAnyByMac(string mac)
    {
        return await context.hardware
        .AsNoTracking()
        .Where(x => x.mac.Equals(mac))
        .AnyAsync();
    }

    public async Task<bool> IsAnyModuleReferenceByMacAsync(string mac)
    {
        return await context.hardware
        .AsNoTracking()
        .Include(x => x.modules)
        .Where(x => x.mac.Equals(mac))
        .AnyAsync(x => x.modules.Where(x => x.component_id != -1).Any());
    }

    public async Task<IEnumerable<(short ComponentId, string Mac)>> GetComponentAndMacAsync()
    {
        var res = await context.hardware
            .AsNoTracking()
            .Select(x => new { x.component_id, x.mac })
            .ToArrayAsync();

        return res.Select(x => (x.component_id, x.mac));
    }

    public async Task<bool> IsAnyByMacAndComponent(string mac, short component)
    {
        return await context.hardware.AnyAsync(x => x.mac.Equals(mac) && x.component_id == component);
    }

    public async Task<IEnumerable<Mode>> GetHardwareTypeAsync()
    {
        var res = await context.hardware_type
        .AsNoTracking()
        .Select(x => new Mode
        {
            Name = x.name,
            Description = x.description,
            Value = x.component_id
        })
        .ToArrayAsync();

        return res;
    }

    public async Task<IEnumerable<short>> GetComponentIdByLocationIdAsync(short locationId)
    {
        return await context.hardware.AsNoTracking()
        .Where(x => x.location_id == locationId)
        .Select(x => x.component_id)
        .ToArrayAsync();
    }

    public async Task<IEnumerable<string>> GetMacsAsync()
    {
        var res = await context.hardware
        .AsNoTracking()
        .Select(x => x.mac)
        .ToArrayAsync();

        return res;
    }

    public async Task<IEnumerable<short>> GetComponentIdsAsync()
    {
        var res = await context.hardware.AsNoTracking()
        .Select(x => x.component_id)
        .ToArrayAsync();

        return res;
    }

    Task<IEnumerable<Mode>> IQHwRepository.GetHardwareTypeAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<List<MemoryDto>> CheckAllocateMemoryAsync(IScpReply message)
    {
        List<MemoryDto> mems = new List<MemoryDto>();
        var config = await GetScpSettingAsync();
        foreach (var i in message.str_sts.sStrSpec)
        {
            switch ((ScpStructure)i.nStrType)
            {
                case ScpStructure.SCPSID_TRAN:
                    // Handle transaction
                    mems.Add(new MemoryDto
                    {
                        nStrType = i.nStrType,
                        StrType = DescriptionHelper.ScpStructureToText(((ScpStructure)i.nStrType)),
                        nRecord = i.nRecords,
                        nRecSize = i.nRecSize,
                        nActive = i.nActive,
                        nSwAlloc = config.nTransaction,
                        nSwRecord = await context.transaction.AsNoTracking().CountAsync(),
                        IsSync = config.nTransaction > i.nRecords,
                    });
                    break;

                case ScpStructure.SCPSID_TZ:
                    // Handle time zones
                    mems.Add(new MemoryDto
                    {
                        nStrType = i.nStrType,
                        StrType = DescriptionHelper.ScpStructureToText(((ScpStructure)i.nStrType)),
                        nRecord = i.nRecords,
                        nRecSize = i.nRecSize,
                        nActive = i.nActive,
                        nSwAlloc = config.nTz,
                        nSwRecord = await context.timezone.AsNoTracking().CountAsync(),
                        IsSync = config.nTz + 1 == i.nRecords,
                    });
                    break;

                case ScpStructure.SCPSID_HOL:
                    // Handle holiday
                    mems.Add(new MemoryDto
                    {
                        nStrType = i.nStrType,
                        StrType = DescriptionHelper.ScpStructureToText(((ScpStructure)i.nStrType)),
                        nRecord = i.nRecords,
                        nRecSize = i.nRecSize,
                        nActive = i.nActive,
                        nSwAlloc = config.nHol,
                        nSwRecord = await context.holiday.AsNoTracking().CountAsync(),
                        IsSync = config.nHol == i.nRecords
                    });
                    break;

                case ScpStructure.SCPSID_MSP1:
                    // Handle Msp1 ports (SIO drivers)
                    mems.Add(new MemoryDto
                    {
                        nStrType = i.nStrType,
                        StrType = DescriptionHelper.ScpStructureToText(((ScpStructure)i.nStrType)),
                        nRecord = i.nRecords,
                        nRecSize = i.nRecSize,
                        nActive = i.nActive,
                        nSwAlloc = config.nMsp1Port,
                        nSwRecord = 0,
                        IsSync = true,
                    });
                    break;

                case ScpStructure.SCPSID_SIO:
                    // Handle SIOs
                    mems.Add(new MemoryDto
                    {
                        nStrType = i.nStrType,
                        StrType = DescriptionHelper.ScpStructureToText(((ScpStructure)i.nStrType)),
                        nRecord = i.nRecords,
                        nRecSize = i.nRecSize,
                        nActive = i.nActive,
                        nSwAlloc = config.nSio,
                        nSwRecord = await context.module.AsNoTracking().CountAsync(),
                        IsSync = config.nSio == i.nRecords,
                    });
                    break;

                case ScpStructure.SCPSID_MP:
                    // Handle Monitor points
                    mems.Add(new MemoryDto
                    {
                        nStrType = i.nStrType,
                        StrType = DescriptionHelper.ScpStructureToText(((ScpStructure)i.nStrType)),
                        nRecord = i.nRecords,
                        nRecSize = i.nRecSize,
                        nActive = i.nActive,
                        nSwAlloc = config.nMp,
                        nSwRecord = await context.monitor_point.AsNoTracking().CountAsync(),
                        IsSync = config.nMp == i.nRecords
                    });
                    break;

                case ScpStructure.SCPSID_CP:
                    // Handle Control points
                    mems.Add(new MemoryDto
                    {
                        nStrType = i.nStrType,
                        StrType = DescriptionHelper.ScpStructureToText(((ScpStructure)i.nStrType)),
                        nRecord = i.nRecords,
                        nRecSize = i.nRecSize,
                        nActive = i.nActive,
                        nSwAlloc = config.nCp,
                        nSwRecord = await context.control_point.AsNoTracking().CountAsync(),
                        IsSync = config.nCp == i.nRecords
                    });
                    break;

                case ScpStructure.SCPSID_ACR:
                    // Handle Access control reader
                    mems.Add(new MemoryDto
                    {
                        nStrType = i.nStrType,
                        StrType = DescriptionHelper.ScpStructureToText(((ScpStructure)i.nStrType)),
                        nRecord = i.nRecords,
                        nRecSize = i.nRecSize,
                        nActive = i.nActive,
                        nSwAlloc = config.nAcr,
                        nSwRecord = await context.door.AsNoTracking().CountAsync(),
                        IsSync = config.nAcr == i.nRecords
                    });
                    break;

                case ScpStructure.SCPSID_ALVL:
                    // Handle Access levels
                    mems.Add(new MemoryDto
                    {
                        nStrType = i.nStrType,
                        StrType = DescriptionHelper.ScpStructureToText(((ScpStructure)i.nStrType)),
                        nRecord = i.nRecords,
                        nRecSize = i.nRecSize,
                        nActive = i.nActive,
                        nSwAlloc = config.nAlvl,
                        nSwRecord = await context.access_level.AsNoTracking().CountAsync(),
                        IsSync = config.nAlvl == i.nRecords
                    });
                    break;

                case ScpStructure.SCPSID_TRIG:
                    // Handle trigger
                    mems.Add(new MemoryDto
                    {
                        nStrType = i.nStrType,
                        StrType = DescriptionHelper.ScpStructureToText(((ScpStructure)i.nStrType)),
                        nRecord = i.nRecords,
                        nRecSize = i.nRecSize,
                        nActive = i.nActive,
                        nSwAlloc = config.nTrgr,
                        nSwRecord = await context.trigger.AsNoTracking().CountAsync(),
                        IsSync = config.nTrgr == i.nRecords
                    });
                    break;

                case ScpStructure.SCPSID_PROC:
                    // Handle procedure
                    mems.Add(new MemoryDto
                    {
                        nStrType = i.nStrType,
                        StrType = DescriptionHelper.ScpStructureToText(((ScpStructure)i.nStrType)),
                        nRecord = i.nRecords,
                        nRecSize = i.nRecSize,
                        nActive = i.nActive,
                        nSwAlloc = config.nProc,
                        nSwRecord = await context.procedure.AsNoTracking().CountAsync(),
                        IsSync = config.nProc == i.nRecords
                    });
                    break;

                case ScpStructure.SCPSID_MPG:
                    // Handle Monitor point groups
                    mems.Add(new MemoryDto
                    {
                        nStrType = i.nStrType,
                        StrType = DescriptionHelper.ScpStructureToText(((ScpStructure)i.nStrType)),
                        nRecord = i.nRecords,
                        nRecSize = i.nRecSize,
                        nActive = i.nActive,
                        nSwAlloc = config.nMpg,
                        nSwRecord = await context.monitor_group.AsNoTracking().CountAsync(),
                        IsSync = config.nMpg == i.nRecords
                    });
                    break;

                case ScpStructure.SCPSID_AREA:
                    // Handle Access area
                    mems.Add(new MemoryDto
                    {
                        nStrType = i.nStrType,
                        StrType = DescriptionHelper.ScpStructureToText(((ScpStructure)i.nStrType)),
                        nRecord = i.nRecords,
                        nRecSize = i.nRecSize,
                        nActive = i.nActive,
                        nSwAlloc = config.nArea,
                        nSwRecord = await context.area.AsNoTracking().CountAsync(),
                        IsSync = true
                    });
                    break;

                case ScpStructure.SCPSID_EAL:
                    // Handle Elevator access levels
                    mems.Add(new MemoryDto
                    {
                        nStrType = i.nStrType,
                        StrType = DescriptionHelper.ScpStructureToText(((ScpStructure)i.nStrType)),
                        nRecord = i.nRecords,
                        nRecSize = i.nRecSize,
                        nActive = i.nActive,
                        nSwAlloc = 0,
                        nSwRecord = 0,
                        IsSync = true
                    });
                    break;

                case ScpStructure.SCPSID_CRDB:
                    // Handle Cardholder database
                    mems.Add(new MemoryDto
                    {
                        nStrType = i.nStrType,
                        StrType = DescriptionHelper.ScpStructureToText(((ScpStructure)i.nStrType)),
                        nRecord = i.nRecords,
                        nRecSize = i.nRecSize,
                        nActive = i.nActive,
                        nSwAlloc = config.nCard,
                        nSwRecord = await context.credential.AsNoTracking().CountAsync(),
                        IsSync = config.nCard == i.nRecords
                    });
                    break;

                case ScpStructure.SCPSID_FLASH:
                    // Handle FLASH specs
                    mems.Add(new MemoryDto
                    {
                        nStrType = i.nStrType,
                        StrType = DescriptionHelper.ScpStructureToText(((ScpStructure)i.nStrType)),
                        nRecord = i.nRecords,
                        nRecSize = i.nRecSize,
                        nActive = i.nActive,
                        nSwAlloc = 0,
                        nSwRecord = 0,
                        IsSync = true
                    });
                    break;

                case ScpStructure.SCPSID_BSQN:
                    // Handle Build sequence number
                    mems.Add(new MemoryDto
                    {
                        nStrType = i.nStrType,
                        StrType = DescriptionHelper.ScpStructureToText(((ScpStructure)i.nStrType)),
                        nRecord = i.nRecords,
                        nRecSize = i.nRecSize,
                        nActive = i.nActive,
                        nSwAlloc = 0,
                        nSwRecord = 0,
                        IsSync = true
                    });
                    break;

                case ScpStructure.SCPSID_SAVE_STAT:
                    // Handle Flash save status
                    mems.Add(new MemoryDto
                    {
                        nStrType = i.nStrType,
                        StrType = DescriptionHelper.ScpStructureToText(((ScpStructure)i.nStrType)),
                        nRecord = i.nRecords,
                        nRecSize = i.nRecSize,
                        nActive = i.nActive,
                        nSwAlloc = 0,
                        nSwRecord = 0,
                        IsSync = true
                    });
                    break;

                case ScpStructure.SCPSID_MAB1_FREE:
                    // Handle Memory alloc block 1 free memory
                    mems.Add(new MemoryDto
                    {
                        nStrType = i.nStrType,
                        StrType = DescriptionHelper.ScpStructureToText(((ScpStructure)i.nStrType)),
                        nRecord = i.nRecords,
                        nRecSize = i.nRecSize,
                        nActive = i.nActive,
                        nSwAlloc = 0,
                        nSwRecord = 0,
                        IsSync = true
                    });
                    break;

                case ScpStructure.SCPSID_MAB2_FREE:
                    // Handle Memory alloc block 2 free memory
                    mems.Add(new MemoryDto
                    {
                        nStrType = i.nStrType,
                        StrType = DescriptionHelper.ScpStructureToText(((ScpStructure)i.nStrType)),
                        nRecord = i.nRecords,
                        nRecSize = i.nRecSize,
                        nActive = i.nActive,
                        nSwAlloc = 0,
                        nSwRecord = 0,
                        IsSync = true
                    });
                    break;

                case ScpStructure.SCPSID_ARQ_BUFFER:
                    // Handle Access request buffers
                    mems.Add(new MemoryDto
                    {
                        nStrType = i.nStrType,
                        StrType = DescriptionHelper.ScpStructureToText(((ScpStructure)i.nStrType)),
                        nRecord = i.nRecords,
                        nRecSize = i.nRecSize,
                        nActive = i.nActive,
                        nSwAlloc = 0,
                        nSwRecord = 0,
                        IsSync = true
                    });
                    break;

                case ScpStructure.SCPSID_PART_FREE_CNT:
                    // Handle Partition memory free info
                    mems.Add(new MemoryDto
                    {
                        nStrType = i.nStrType,
                        StrType = DescriptionHelper.ScpStructureToText(((ScpStructure)i.nStrType)),
                        nRecord = i.nRecords,
                        nRecSize = i.nRecSize,
                        nActive = i.nActive,
                        nSwAlloc = 0,
                        nSwRecord = 0,
                        IsSync = true
                    });
                    break;

                case ScpStructure.SCPSID_LOGIN_STANDARD:
                    // Handle Web logins - standard
                    mems.Add(new MemoryDto
                    {
                        nStrType = i.nStrType,
                        StrType = DescriptionHelper.ScpStructureToText(((ScpStructure)i.nStrType)),
                        nRecord = i.nRecords,
                        nRecSize = i.nRecSize,
                        nActive = i.nActive,
                        nSwAlloc = 0,
                        nSwRecord = 0,
                        IsSync = true
                    });
                    break;
                case ScpStructure.SCPSID_FILE_SYSTEM:
                    mems.Add(new MemoryDto
                    {
                        nStrType = i.nStrType,
                        StrType = DescriptionHelper.ScpStructureToText(((ScpStructure)i.nStrType)),
                        nRecord = i.nRecords,
                        nRecSize = i.nRecSize,
                        nActive = i.nActive,
                        nSwAlloc = 0,
                        nSwRecord = 0,
                        IsSync = true
                    });
                    break;

                default:
                    // Handle unknown/unsupported types
                    break;
            }
        }
        return mems;
    }

    public async Task<short> GetLowestUnassignedNumberAsync(int max, string mac)
    {
        if (max <= 0) return -1;

        var query = context.hardware
            .AsNoTracking()
            .Select(x => x.component_id);

        // Handle empty table case quickly
        var hasAny = await query.AnyAsync();
        if (!hasAny)
            return 1; // start at 1 if table is empty

        // Load all numbers into memory (only the column, so it's lightweight)
        var numbers = await query.Distinct().OrderBy(x => x).ToListAsync();

        short expected = 1;
        foreach (var num in numbers)
        {
            if (num != expected)
                return expected; // found the lowest missing number
            expected++;
        }

        // If none missing in sequence, return next number
        if (expected > max) return -1;
        return expected;
    }

    public Task AssignPortAsync(IScpReply message)
    {
        throw new NotImplementedException();
    }

    public Task AssignIpAddressAsync(IScpReply message)
    {
        throw new NotImplementedException();
    }

    public async Task<string> GetNameByComponentIdAsync(short component)
    {
        var res = await context.hardware.AsNoTracking()
        .Where(x => x.component_id == component)
        .OrderBy(x => x.component_id)
        .Select(x => x.name)
        .FirstOrDefaultAsync() ?? "";

        return res;
    }
}
