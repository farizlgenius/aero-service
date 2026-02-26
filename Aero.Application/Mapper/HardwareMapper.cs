using System;
using Aero.Application.DTOs;
using Aero.Domain.Entities;
using Aero.Domain.Enums;

namespace Aero.Application.Mapper;

public class HardwareMapper
{
    public static Device ToDomain(CreateHardwareDto dto)
    {
        return new Device
        {
            ComponentId = dto.ComponentId,
            Mac = dto.Mac,
            LocationId = dto.LocationId,
            Name = dto.Name,
            HardwareType = dto.HardwareType,
            HardwareTypeDetail = dto.HardwareTypeDescription,
            Modules = new List<Module>
                {
                    new Module
                    {
                        // Base 
                        ComponentId = dto.ComponentId,
                        DriverId = 0,
                        Mac = dto.Mac,
                        LocationId = dto.LocationId,
                        IsActive = dto.IsActive,
                        // created_date =Created,
                        // updated_date = Created,

                        // extend_desc
                        ModelDetail = "Internal",
                        Model = (short)Domain.Enums.Model.AeroX1100,
                        Revision=dto.Firmware,
                        SerialNumber = dto.SerialNumber,
                        nHardwareId = 217,
                        nHardwareIdDetail = "HID Aero X1100",
                        Address = -1,
                        AddressDetail = "Internal",
                        Port = 3,
                        nInput = (short)InputComponents.HIDAeroX1100,
                        nOutput = (short)OutputComponents.HIDAeroX1100,
                        nReader = (short)ReaderComponents.HIDAeroX1100,
                        Msp1No = 0,
                        BaudRate = -1,
                        nProtocol = 0,
                        nDialect = 0,

                    }
                },
            Ip = dto.Ip,
            Port = dto.Port,
            Firmware = dto.Firmware,
            SerialNumber = dto.SerialNumber,
            IsUpload = false,
            IsReset = false,
            PortOne = dto.PortOne,
            ProtocolOne = dto.ProtocolOne,
            ProtocolOneDetail = dto.ProtocolOneDescription,
            PortTwo = dto.PortTwo,
            ProtocolTwoDetail = dto.ProtocolTwoDescription,
            ProtocolTwo = dto.ProtocolTwo,
            BaudRateOne = dto.BaudRateOne,
            BaudRateTwo = dto.BaudRateTwo,
            LastSync = DateTime.UtcNow,
            //     created_date = DateTime.UtcNow,
            //     updated_date = DateTime.UtcNow
        };
    }

    public static Device ToDomain(HardwareDto dto)
    {
        return new Device
        {
            ComponentId = dto.ComponentId,
            Mac = dto.Mac,
            LocationId = dto.LocationId,
            Name = dto.Name,
            HardwareType = dto.HardwareType,
            HardwareTypeDetail = dto.HardwareTypeDescription,
            Modules = new List<Module>
                {
                    new Module
                    {
                        // Base 
                        ComponentId = 1,
                        DriverId = 0,
                        Mac = dto.Mac,
                        LocationId = dto.LocationId,
                        IsActive = dto.IsActive,
                        // created_date =Created,
                        // updated_date = Created,

                        // extend_desc
                        ModelDetail = "Internal",
                        Model = (short)Domain.Enums.Model.AeroX1100,
                        Revision=dto.Firmware,
                        SerialNumber = dto.SerialNumber,
                        nHardwareId = 217,
                        nHardwareIdDetail = "HID Aero X1100",
                        Address = -1,
                        AddressDetail = "Internal",
                        Port = 3,
                        nInput = (short)InputComponents.HIDAeroX1100,
                        nOutput = (short)OutputComponents.HIDAeroX1100,
                        nReader = (short)ReaderComponents.HIDAeroX1100,
                        Msp1No = 0,
                        BaudRate = -1,
                        nProtocol = 0,
                        nDialect = 0,

                    }
                },
            Ip = dto.Ip,
            Port = dto.Port,
            Firmware = dto.Firmware,
            SerialNumber = dto.SerialNumber,
            IsUpload = false,
            IsReset = false,
            PortOne = dto.PortOne,
            ProtocolOne = dto.ProtocolOne,
            ProtocolOneDetail = dto.ProtocolOneDescription,
            PortTwo = dto.PortTwo,
            ProtocolTwoDetail = dto.ProtocolTwoDescription,
            ProtocolTwo = dto.ProtocolTwo,
            BaudRateOne = dto.BaudRateOne,
            BaudRateTwo = dto.BaudRateTwo,
            LastSync = DateTime.UtcNow,
            //     created_date = DateTime.UtcNow,
            //     updated_date = DateTime.UtcNow
        };
    }

}
