using System;
using Aero.Infrastructure.Data.Entities;
using Aero.Infrastructure.Enums;

namespace Aero.Infrastructure.Mapper;

public static class HardwareMapper
{
      public static Hardware ToEf(Aero.Domain.Entities.Hardware domain)
      {
            var date = DateTime.UtcNow;
            return new Hardware
            {
                  component_id = domain.ComponentId,
                  mac = domain.Mac,
                  location_id = domain.LocationId,
                  name = domain.Name,
                  hardware_type = domain.HardwareType,
                  hardware_type_desc = domain.HardwareTypeDescription,
                  modules = new List<Module>
                {
                    new Module
                    {
                        // Base 
                        component_id = 0,
                        hardware_mac = domain.Mac,
                        location_id = domain.LocationId,
                        is_active = domain.IsActive,
                        created_date =date,
                        updated_date = date,

                        // extend_desc
                        model_desc = "Internal",
                        model = (short)Model.AeroX1100,
                        revision=domain.Firmware,
                        serial_number = domain.SerialNumber,
                        n_hardware_id = 217,
                        n_hardware_id_desc = "HID Aero X1100",
                        address = -1,
                        address_desc = "Internal",
                        port = 3,
                        n_input = (short)InputComponents.HIDAeroX1100,
                        n_output = (short)OutputComponents.HIDAeroX1100,
                        n_reader = (short)ReaderComponents.HIDAeroX1100,
                        msp1_no = 0,
                        baudrate = -1,
                        n_protocol = 0,
                        n_dialect = 0,

                    }
                },
                  ip = domain.Ip,
                  port = domain.Port,
                  firmware = domain.Firmware,
                  serial_number = domain.SerialNumber,
                  is_upload = false,
                  is_reset = false,
                  port_one = domain.PortOne,
                  protocol_one = domain.ProtocolOne,
                  protocol_one_desc = domain.ProtocolOneDescription,
                  port_two = domain.PortTwo,
                  protocol_two_desc = domain.ProtocolTwoDescription,
                  protocol_two = domain.ProtocolTwo,
                  baudrate_one = domain.BaudRateOne,
                  baudrate_two = domain.BaudRateTwo,
                  last_sync = date,
                  created_date = date,
                  updated_date = date
            };
      }

      public static Aero.Domain.Entities.Hardware ToDomain(Hardware ef)
      {
            return new Aero.Domain.Entities.Hardware
            {
                  // Base 
                  Uuid = ef.uuid,
                  ComponentId = ef.component_id,
                  Mac = ef.mac,
                  LocationId = ef.location_id,
                  IsActive = ef.is_active,

                  // extend_desc
                  Name = ef.name,
                  HardwareType = ef.hardware_type,
                  HardwareTypeDescription = ef.hardware_type_desc,
                  Firmware = ef.firmware,
                  Ip = ef.ip,
                  Port = ef.port,
                  SerialNumber = ef.serial_number,
                  IsReset = ef.is_reset,
                  IsUpload = ef.is_upload,
                  Modules = ef.modules.Select(d => new Aero.Domain.Entities.Module
                  {
                        // Base 
                        Uuid = d.uuid,
                        ComponentId = d.component_id,
                        HardwareName = ef.name,
                        Mac = ef.mac,
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
                  PortOne = ef.port_one,
                  ProtocolOne = ef.protocol_one,
                  ProtocolOneDescription = ef.protocol_one_desc,
                  PortTwo = ef.port_two,
                  ProtocolTwoDescription = ef.protocol_two_desc,
                  ProtocolTwo = ef.protocol_two,
                  BaudRateOne = ef.baudrate_one,
                  BaudRateTwo = ef.baudrate_two,
            };
      }
}
