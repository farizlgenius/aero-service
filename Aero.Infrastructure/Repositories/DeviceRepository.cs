using Aero.Application.DTOs;
using Aero.Application.Helpers;
using Aero.Application.Interfaces;
using Aero.Domain.Entities;
using Aero.Domain.Interface;
using Aero.Infrastructure.Persistences;
using Aero.Infrastructure.Helpers;
using Aero.Infrastructure.Mapper;
using HID.Aero.ScpdNet.Wrapper;
using Microsoft.EntityFrameworkCore;
using System;
using static Aero.Infrastructure.Helpers.DescriptionHelper;
using Aero.Application.Interface;

namespace Aero.Infrastructure.Repositories;

public sealed class DeviceRepository(AppDbContext context) : IDeviceRepository
{
    public async Task<int> AddAsync(Device data)
    {
        var en = new Aero.Infrastructure.Persistences.Entities.Device(data);
        await context.device.AddAsync(en);
        var rec = await context.SaveChangesAsync();
        if (rec <= 0) return -1;
        return en.id;
    }

    public async Task<int> DeleteByIdAsync(int id)
    {
        var ef = await context.device
        .Where(x => x.id == id)
        .OrderBy(x => x.id)
        .FirstOrDefaultAsync();

        if (ef is null) return 0;

        context.device.Remove(ef);
        return await context.SaveChangesAsync();
    }

    public async Task<int> DeleteByMacAsync(string mac)
    {
        var ef = await context.device
        .Where(x => x.mac.Equals(mac))
        .OrderBy(x => x.id)
        .FirstOrDefaultAsync();

        if (ef is null) return 0;

        context.device.Remove(ef);
        return await context.SaveChangesAsync();
    }

    public async Task<Device> GetDomainByMacAsync(string mac)
    {
        var res = await context.device
        .Where(x => x.mac.Equals(mac))
        .OrderBy(x => x.id)
        .Select(x => new Device(
            x.id,
            x.driver_id,
            x.name,
            x.hardware_type,
            x.hardware_type_detail,
            x.mac,
            x.ip,
            x.firmware,
            x.port,
            x.modules.Select(m => new Module(
                m.device_id,
                m.driver_id,
                m.model,
                m.model_detail,
                m.revision,
                m.serial_number,
                m.n_hardware_id,
                m.n_hardware_id_detail,
                m.n_hardware_rev,
                m.n_product_id,
                m.n_product_ver,
                m.n_enc_config,
                m.n_enc_config_detail,
                m.n_enc_key_status,
                m.n_enc_key_status_detail,
                null,
                null,
                null,
                null,
                null,
                null,
                m.address,
                m.address_detail,
                m.port,
                m.n_input,
                m.n_output,
                m.n_reader,
                m.msp1_no,
                m.baudrate,
                m.n_protocol,
                m.n_dialect,
                m.location_id,
                m.is_active
            )).ToList(),
            x.serial_number,
            x.is_upload,
            x.is_reset,
            x.port_one,
            x.protocol_one,
            x.protocol_one_detail,
            x.baudrate_one,
            x.port_two,
            x.protocol_two,
            x.protocol_two_detail,
            x.baudrate_two,
            x.last_sync
            ))
        .FirstOrDefaultAsync();

        return res;

    }

    public async Task<int> UpdateSyncStatusByIdAsync(int id)
    {
        var res = await context.device.
        Where(x => x.id == id)
        .OrderBy(x => x.id)
        .FirstOrDefaultAsync();

        if (res is null) return 0;

        res.updated_date = DateTime.UtcNow;
        res.last_sync = DateTime.UtcNow;
        res.is_upload = false;
        context.device.Update(res);
        return await context.SaveChangesAsync();
    }

    public async Task<int> UpdateVerifyHardwareCofigurationMyMacAsync(string mac, bool status)
    {
        var hardware = await context.device
        .Where(x => x.mac == mac)
        .FirstOrDefaultAsync();

        if (hardware is null) return 0;

        hardware.updated_date = DateTime.UtcNow;
        hardware.is_upload = status;

        context.device.Update(hardware);
        return await context.SaveChangesAsync();
    }

    public async Task<int> UpdateVerifyMemoryAllocateByIdAsync(int id, bool isSync)
    {
        var hw = await context.device.
        Where(x => x.id == id)
        .OrderBy(x => x.id)
        .FirstOrDefaultAsync();

        if (hw is null) return 0;
        hw.is_reset = isSync;
        hw.updated_date = DateTime.UtcNow;
        context.device.Update(hw);
        return await context.SaveChangesAsync();
    }



    public async Task<int> UpdateAsync(Device data)
    {
        var en = await context.device
        .Where(x => x.mac.Equals(data.Mac) && x.id == data.Id)
        .OrderBy(x => x.id)
        .FirstOrDefaultAsync();


        if (en is null) return 0;

        en.Update(data);

        context.device.Update(en);
        return await context.SaveChangesAsync();
    }

    public async Task UpdateIpAddressAsync(int ScpId, string ip)
    {
        var hw = await context.device
        .Where(x => x.id == (short)ScpId)
        .FirstOrDefaultAsync();

        if (hw is null) return;

        hw.ip = ip;

        context.device.Update(hw);
        await context.SaveChangesAsync();
    }

    public async Task UpdatePortAddressAsync(int ScpId, string port)
    {
        var hw = await context.device
        .Where(x => x.id == (short)ScpId)
        .FirstOrDefaultAsync();

        if (hw is null) return;

        hw.port = port;

        context.device.Update(hw);
        await context.SaveChangesAsync();
    }

    public async Task<IEnumerable<DeviceDto>> GetAsync()
    {
        var dtos = await context.device
            .AsNoTracking()
            .Select(x => new DeviceDto(
            x.id,
            x.driver_id,
            x.name,
            x.hardware_type,
            x.hardware_type_detail,
            x.mac,
            x.ip,
            x.firmware,
            x.port,
            x.modules.Select(m => new ModuleDto(
                m.device_id,
                m.driver_id,
                m.model,
                m.model_detail,
                m.revision,
                m.serial_number,
                m.n_hardware_id,
                m.n_hardware_id_detail,
                m.n_hardware_rev,
                m.n_product_id,
                m.n_product_ver,
                m.n_enc_config,
                m.n_enc_config_detail,
                m.n_enc_key_status,
                m.n_enc_key_status_detail,
                null,
                null,
                null,
                null,
                null,
                null,
                m.address,
                m.address_detail,
                m.port,
                m.n_input,
                m.n_output,
                m.n_reader,
                m.msp1_no,
                m.baudrate,
                m.n_protocol,
                m.n_dialect,
                m.location_id,
                m.is_active
            )).ToList(),
            x.serial_number,
            x.is_upload,
            x.is_reset,
            x.port_one,
            x.protocol_one,
            x.protocol_one_detail,
            x.baudrate_one,
            x.port_two,
            x.protocol_two,
            x.protocol_two_detail,
            x.baudrate_two,
            x.last_sync,
            x.location_id,
            x.is_active
            ))
            .ToArrayAsync();

        return dtos;
    }

    public async Task<DeviceDto> GetByIdAsync(int id)
    {
        var dto = await context.device
            .AsNoTracking()
            .Where(x => x.id == id)
            .OrderBy(x => x.id)
            .Select(x => new DeviceDto(
            x.id,
            x.driver_id,
            x.name,
            x.hardware_type,
            x.hardware_type_detail,
            x.mac,
            x.ip,
            x.firmware,
            x.port,
            x.modules.Select(m => new ModuleDto(
                m.device_id,
                m.driver_id,
                m.model,
                m.model_detail,
                m.revision,
                m.serial_number,
                m.n_hardware_id,
                m.n_hardware_id_detail,
                m.n_hardware_rev,
                m.n_product_id,
                m.n_product_ver,
                m.n_enc_config,
                m.n_enc_config_detail,
                m.n_enc_key_status,
                m.n_enc_key_status_detail,
                null,
                null,
                null,
                null,
                null,
                null,
                m.address,
                m.address_detail,
                m.port,
                m.n_input,
                m.n_output,
                m.n_reader,
                m.msp1_no,
                m.baudrate,
                m.n_protocol,
                m.n_dialect,
                m.location_id,
                m.is_active
            )).ToList(),
            x.serial_number,
            x.is_upload,
            x.is_reset,
            x.port_one,
            x.protocol_one,
            x.protocol_one_detail,
            x.baudrate_one,
            x.port_two,
            x.protocol_two,
            x.protocol_two_detail,
            x.baudrate_two,
            x.last_sync,
            x.location_id,
            x.is_active
            ))
            .FirstOrDefaultAsync();

        return dto;
    }

    public async Task<IEnumerable<DeviceDto>> GetByLocationIdAsync(int locationId)
    {
        var dto = await context.device
            .AsNoTracking()
            .Where(x => x.location_id == locationId || x.location_id == 1)
            .OrderBy(x => x.id)
            .Select(x => new DeviceDto(
            x.id,
            x.driver_id,
            x.name,
            x.hardware_type,
            x.hardware_type_detail,
            x.mac,
            x.ip,
            x.firmware,
            x.port,
            x.modules.Select(m => new ModuleDto(
                m.device_id,
                m.driver_id,
                m.model,
                m.model_detail,
                m.revision,
                m.serial_number,
                m.n_hardware_id,
                m.n_hardware_id_detail,
                m.n_hardware_rev,
                m.n_product_id,
                m.n_product_ver,
                m.n_enc_config,
                m.n_enc_config_detail,
                m.n_enc_key_status,
                m.n_enc_key_status_detail,
                null,
                null,
                null,
                null,
                null,
                null,
                m.address,
                m.address_detail,
                m.port,
                m.n_input,
                m.n_output,
                m.n_reader,
                m.msp1_no,
                m.baudrate,
                m.n_protocol,
                m.n_dialect,
                m.location_id,
                m.is_active
            )).ToList(),
            x.serial_number,
            x.is_upload,
            x.is_reset,
            x.port_one,
            x.protocol_one,
            x.protocol_one_detail,
            x.baudrate_one,
            x.port_two,
            x.protocol_two,
            x.protocol_two_detail,
            x.baudrate_two,
            x.last_sync,
            x.location_id,
            x.is_active
            ))
            .ToArrayAsync();

        return dto;
    }

    public async Task<DeviceDto> GetByMacAsync(string mac)
    {
        var dto = await context.device
            .AsNoTracking()
            .Where(x => x.mac.Equals(mac))
            .OrderBy(x => x.id)
            .Select(x => new DeviceDto(
            x.id,
            x.driver_id,
            x.name,
            x.hardware_type,
            x.hardware_type_detail,
            x.mac,
            x.ip,
            x.firmware,
            x.port,
            x.modules.Select(m => new ModuleDto(
                m.device_id,
                m.driver_id,
                m.model,
                m.model_detail,
                m.revision,
                m.serial_number,
                m.n_hardware_id,
                m.n_hardware_id_detail,
                m.n_hardware_rev,
                m.n_product_id,
                m.n_product_ver,
                m.n_enc_config,
                m.n_enc_config_detail,
                m.n_enc_key_status,
                m.n_enc_key_status_detail,
                null,
                null,
                null,
                null,
                null,
                null,
                m.address,
                m.address_detail,
                m.port,
                m.n_input,
                m.n_output,
                m.n_reader,
                m.msp1_no,
                m.baudrate,
                m.n_protocol,
                m.n_dialect,
                m.location_id,
                m.is_active
            )).ToList(),
            x.serial_number,
            x.is_upload,
            x.is_reset,
            x.port_one,
            x.protocol_one,
            x.protocol_one_detail,
            x.baudrate_one,
            x.port_two,
            x.protocol_two,
            x.protocol_two_detail,
            x.baudrate_two,
            x.last_sync,
            x.location_id,
            x.is_active
            ))
            .FirstOrDefaultAsync();

        return dto;
    }

    public async Task<short> GetComponentIdFromMacAsync(string mac)
    {
        var res = await context.device
        .AsNoTracking()
        .Where(x => x.mac.Equals(mac))
        .OrderBy(x => x.id)
        .Select(x => x.driver_id)
        .FirstOrDefaultAsync();

        return res;
    }

    public async Task<string> GetMacFromComponentAsync(short component)
    {
        var res = await context.device
        .AsNoTracking()
        .Where(x => x.driver_id == component)
        .OrderBy(x => x.id)
        .Select(x => x.mac)
        .FirstOrDefaultAsync() ?? "";

        return res;
    }

    public async Task<ScpSetting> GetScpSettingAsync()
    {
        var res = await context.scp_setting
        .AsNoTracking()
        .OrderBy(x => x.id)
        .Select(x => new ScpSetting(x.n_msp1_port,x.n_transaction,x.n_sio,x.n_mp,x.n_cp,x.n_acr,x.n_alvl,x.n_trgr,x.n_proc,x.gmt_offset,x.n_tz,x.n_hol,x.n_mpg,x.n_card,x.n_area,x.n_cfmt))
        .FirstOrDefaultAsync();

        return res;
    }

    public async Task<bool> IsAnyByIdAsync(int id)
    {
        return await context.device
        .AsNoTracking()
        .Where(x => x.id == id)
        .AnyAsync();
    }

    public async Task<bool> IsAnyByMac(string mac)
    {
        return await context.device
        .AsNoTracking()
        .Where(x => x.mac.Equals(mac))
        .AnyAsync();
    }

    public async Task<bool> IsAnyModuleReferenceByDriverIdAsync(int driver)
    {
        return await context.device
        .AsNoTracking()
        .Include(x => x.modules)
        .Where(x => x.driver_id == driver)
        .AnyAsync(x => x.modules.Where(x => x.device_id != -1).Any());
    }

    public async Task<IEnumerable<(short DriverId, string Mac)>> GetDriverAndMacAsync()
    {
        var res = await context.device
            .AsNoTracking()
            .Select(x => new { x.driver_id, x.mac })
            .ToArrayAsync();

        return res.Select(x => (x.driver_id, x.mac));
    }

    public async Task<bool> IsAnyByMacAndDriver(string mac, int driver)
    {
        return await context.device.AnyAsync(x => x.mac.Equals(mac) && x.driver_id == driver);
    }



    public async Task<IEnumerable<short>> GetDriverIdByLocationIdAsync(int locationId)
    {
        return await context.device.AsNoTracking()
        .Where(x => x.location_id == locationId)
        .Select(x => x.driver_id)
        .ToArrayAsync();
    }

    public async Task<IEnumerable<string>> GetMacsAsync()
    {
        var res = await context.device
        .AsNoTracking()
        .Select(x => x.mac)
        .ToArrayAsync();

        return res;
    }

    public async Task<IEnumerable<short>> GetDriverIdsAsync()
    {
        var res = await context.device.AsNoTracking()
        .Select(x => x.driver_id)
        .ToArrayAsync();

        return res;
    }


    public async Task<List<MemoryDto>> CheckAllocateMemoryAsync(IScpReply message)
    {
        var mems = new List<MemoryDto>();
        var config = await GetScpSettingAsync();

        foreach (var i in message.str_sts.sStrSpec)
        {
            var structure = (ScpStructure)i.nStrType;
            var structureText = DescriptionHelper.ScpStructureToText(structure);

            switch (structure)
            {
                case ScpStructure.SCPSID_TRAN:
                    mems.Add(new MemoryDto(i.nStrType, structureText, i.nRecords, i.nRecSize, i.nActive, config.nTransaction, await context.transaction.AsNoTracking().CountAsync(), config.nTransaction > i.nRecords));
                    break;
                case ScpStructure.SCPSID_TZ:
                    mems.Add(new MemoryDto(i.nStrType, structureText, i.nRecords, i.nRecSize, i.nActive, config.nTz, await context.timezone.AsNoTracking().CountAsync(), config.nTz + 1 == i.nRecords));
                    break;
                case ScpStructure.SCPSID_HOL:
                    mems.Add(new MemoryDto(i.nStrType, structureText, i.nRecords, i.nRecSize, i.nActive, config.nHol, await context.holiday.AsNoTracking().CountAsync(), config.nHol == i.nRecords));
                    break;
                case ScpStructure.SCPSID_MSP1:
                    mems.Add(new MemoryDto(i.nStrType, structureText, i.nRecords, i.nRecSize, i.nActive, config.nMsp1Port, 0, true));
                    break;
                case ScpStructure.SCPSID_SIO:
                    mems.Add(new MemoryDto(i.nStrType, structureText, i.nRecords, i.nRecSize, i.nActive, config.nSio, await context.module.AsNoTracking().CountAsync(), config.nSio == i.nRecords));
                    break;
                case ScpStructure.SCPSID_MP:
                    mems.Add(new MemoryDto(i.nStrType, structureText, i.nRecords, i.nRecSize, i.nActive, config.nMp, await context.monitor_point.AsNoTracking().CountAsync(), config.nMp == i.nRecords));
                    break;
                case ScpStructure.SCPSID_CP:
                    mems.Add(new MemoryDto(i.nStrType, structureText, i.nRecords, i.nRecSize, i.nActive, config.nCp, await context.control_point.AsNoTracking().CountAsync(), config.nCp == i.nRecords));
                    break;
                case ScpStructure.SCPSID_ACR:
                    mems.Add(new MemoryDto(i.nStrType, structureText, i.nRecords, i.nRecSize, i.nActive, config.nAcr, await context.door.AsNoTracking().CountAsync(), config.nAcr == i.nRecords));
                    break;
                case ScpStructure.SCPSID_ALVL:
                    mems.Add(new MemoryDto(i.nStrType, structureText, i.nRecords, i.nRecSize, i.nActive, config.nAlvl, await context.access_level.AsNoTracking().CountAsync(), config.nAlvl == i.nRecords));
                    break;
                case ScpStructure.SCPSID_TRIG:
                    mems.Add(new MemoryDto(i.nStrType, structureText, i.nRecords, i.nRecSize, i.nActive, config.nTrgr, await context.trigger.AsNoTracking().CountAsync(), config.nTrgr == i.nRecords));
                    break;
                case ScpStructure.SCPSID_PROC:
                    mems.Add(new MemoryDto(i.nStrType, structureText, i.nRecords, i.nRecSize, i.nActive, config.nProc, await context.procedure.AsNoTracking().CountAsync(), config.nProc == i.nRecords));
                    break;
                case ScpStructure.SCPSID_MPG:
                    mems.Add(new MemoryDto(i.nStrType, structureText, i.nRecords, i.nRecSize, i.nActive, config.nMpg, await context.monitor_group.AsNoTracking().CountAsync(), config.nMpg == i.nRecords));
                    break;
                case ScpStructure.SCPSID_AREA:
                    mems.Add(new MemoryDto(i.nStrType, structureText, i.nRecords, i.nRecSize, i.nActive, config.nArea, await context.area.AsNoTracking().CountAsync(), true));
                    break;
                case ScpStructure.SCPSID_EAL:
                case ScpStructure.SCPSID_FLASH:
                case ScpStructure.SCPSID_BSQN:
                case ScpStructure.SCPSID_SAVE_STAT:
                case ScpStructure.SCPSID_MAB1_FREE:
                case ScpStructure.SCPSID_MAB2_FREE:
                case ScpStructure.SCPSID_ARQ_BUFFER:
                case ScpStructure.SCPSID_PART_FREE_CNT:
                case ScpStructure.SCPSID_LOGIN_STANDARD:
                case ScpStructure.SCPSID_FILE_SYSTEM:
                    mems.Add(new MemoryDto(i.nStrType, structureText, i.nRecords, i.nRecSize, i.nActive, 0, 0, true));
                    break;
                case ScpStructure.SCPSID_CRDB:
                    mems.Add(new MemoryDto(i.nStrType, structureText, i.nRecords, i.nRecSize, i.nActive, config.nCard, await context.credential.AsNoTracking().CountAsync(), config.nCard == i.nRecords));
                    break;
                default:
                    break;
            }
        }

        return mems;
    }

    public async Task<short> GetLowestUnassignedNumberAsync(int max)
    {
        if (max <= 0) return -1;

        var query = context.device
            .AsNoTracking()
            .Select(x => x.driver_id);

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

    public async Task<string> GetNameByDriverIdAsync(int driver)
    {
        var res = await context.device.AsNoTracking()
        .Where(x => x.driver_id == driver)
        .OrderBy(x => x.id)
        .Select(x => x.name)
        .FirstOrDefaultAsync() ?? "";

        return res;
    }

    public async Task<IEnumerable<short>> GetDriverIdsByLocationIdAsync(int locationid)
    {
        return await context.device.AsNoTracking()
            .Where(x => x.location_id == locationid)
            .Select(x => x.driver_id)
            .ToArrayAsync();
    }

    public async Task<IEnumerable<string>> GetMacsByLocationIdAsync(int locationid)
    {
        return await context.device.AsNoTracking()
           .Where(x => x.location_id == locationid)
           .Select(x => x.mac)
           .ToArrayAsync();
    }

    public async Task<int> GetLocationIdFromDriverIdAsync(int driver)
    {
        return await context.device
            .AsNoTracking()
            .OrderBy(x => x.id)
            .Where(x => x.driver_id == driver)
            .Select(x => x.location_id)
            .FirstOrDefaultAsync();
    }

    public async Task<Pagination<DeviceDto>> GetPaginationAsync(PaginationParamsWithFilter param, int location)
    {

        var query = context.device.AsNoTracking().AsQueryable();


        if (!string.IsNullOrWhiteSpace(param.Search))
        {
            if (!string.IsNullOrWhiteSpace(param.Search))
            {
                var search = param.Search.Trim();

                if (context.Database.IsNpgsql())
                {
                    var pattern = $"%{search}%";

                    query = query.Where(x =>
                        EF.Functions.ILike(x.name, pattern) ||
                        EF.Functions.ILike(x.hardware_type_detail, pattern) ||
                        EF.Functions.ILike(x.ip, pattern) ||
                        EF.Functions.ILike(x.mac, pattern) ||
                        EF.Functions.ILike(x.port, pattern) ||
                        EF.Functions.ILike(x.firmware, pattern) ||
                        EF.Functions.ILike(x.serial_number, pattern)

                    );
                }
                else // SQL Server
                {
                    query = query.Where(x =>
                        x.name.Contains(search) ||
                        x.hardware_type_detail.Contains(search) ||
                        x.ip.Contains(search) ||
                        x.mac.Contains(search) ||
                        x.port.Contains(search) ||
                        x.firmware.Contains(search) ||
                        x.serial_number.Contains(search)
                    );
                }
            }
        }

        query = query.Where(x => x.location_id == location || x.location_id == 1);

        if (param.StartDate != null)
        {
            var startUtc = DateTime.SpecifyKind(param.StartDate.Value, DateTimeKind.Utc);
            query = query.Where(x => x.created_date >= startUtc);
        }

        if (param.EndDate != null)
        {
            var endUtc = DateTime.SpecifyKind(param.EndDate.Value, DateTimeKind.Utc);
            query = query.Where(x => x.created_date <= endUtc);
        }

        var count = await query.CountAsync();


        var data = await query
            .AsNoTracking()
            .OrderByDescending(t => t.created_date)
            .Skip((param.PageNumber - 1) * param.PageSize)
            .Take(param.PageSize)
            .Select(x => new DeviceDto(
            x.id,
            x.driver_id,
            x.name,
            x.hardware_type,
            x.hardware_type_detail,
            x.mac,
            x.ip,
            x.firmware,
            x.port,
            x.modules.Select(m => new ModuleDto(
                m.device_id,
                m.driver_id,
                m.model,
                m.model_detail,
                m.revision,
                m.serial_number,
                m.n_hardware_id,
                m.n_hardware_id_detail,
                m.n_hardware_rev,
                m.n_product_id,
                m.n_product_ver,
                m.n_enc_config,
                m.n_enc_config_detail,
                m.n_enc_key_status,
                m.n_enc_key_status_detail,
                null,
                null,
                null,
                null,
                null,
                null,
                m.address,
                m.address_detail,
                m.port,
                m.n_input,
                m.n_output,
                m.n_reader,
                m.msp1_no,
                m.baudrate,
                m.n_protocol,
                m.n_dialect,
                m.location_id,
                m.is_active
            )).ToList(),
            x.serial_number,
            x.is_upload,
            x.is_reset,
            x.port_one,
            x.protocol_one,
            x.protocol_one_detail,
            x.baudrate_one,
            x.port_two,
            x.protocol_two,
            x.protocol_two_detail,
            x.baudrate_two,
            x.last_sync,
            x.location_id,
            x.is_active
            ))
            .ToListAsync();


        return new Pagination<DeviceDto>
        {
            Data = data,
            Page = new PaginationData
            {
                TotalCount = count,
                PageNumber = param.PageNumber,
                PageSize = param.PageSize,
                TotalPage = (int)Math.Ceiling(count / (double)param.PageSize)
            }
        };
    }

      public async Task<bool> IsAnyByNameAsync(string name)
      {
            return await context.device.AsNoTracking().AnyAsync(x => x.name.Equals(name));
      }

      public async Task<DeviceComponent> GetDeviceComponentByModelAsync(short model)
      {
            var res = await context.device_component
            .AsNoTracking()
            .Where(x => x.model_no == model)
            .OrderBy(x => x.id)
            .Select(x => new DeviceComponent(x.id,x.model_no,x.name,x.n_input,x.n_output,x.n_reader))
            .FirstOrDefaultAsync();

            return res;
   
      }
}
