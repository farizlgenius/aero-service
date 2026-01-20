using System;
using Aero.Application.DTOs;
using Aero.Application.Interfaces;
using Aero.Domain.Entities;
using Aero.Domain.Interface;
using Aero.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Aero.Infrastructure.Repositories;

public sealed class QueryHardwareRepository(AppDbContext context) : IQueryHardwareRepository
{
    public async Task<IEnumerable<HardwareDto>> GetAsync()
    {
        var dtos = await context.hardware
            .AsNoTracking()
            .Select(hardware => new HardwareDto
            {
                // Base 
                Uuid = hardware.uuid,
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
                    Uuid = d.uuid,
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
                Uuid = hardware.uuid,
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
                    Uuid = d.uuid,
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
                Uuid = hardware.uuid,
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
                    Uuid = d.uuid,
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
                Uuid = hardware.uuid,
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
                    Uuid = d.uuid,
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

    public async Task<short> GetComponentFromMacAsync(string mac)
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

    public async Task<bool> IsAnyModuleReferenceByMacAsync(string mac)
    {
        return await context.hardware
        .AsNoTracking()
        .Include(x => x.modules)
        .Where(x => x.mac.Equals(mac))
        .AnyAsync(x => x.modules.Where(x => x.component_id != -1).Any());
    }


}
