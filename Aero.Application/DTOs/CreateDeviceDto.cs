using Aero.Domain.Entities;

namespace Aero.Application.DTOs
{

    public sealed record    CreateDeviceDto(
        short DriverId,
        string Name,
        int HardwareType,
        string HardwareTypeDetail,
        string Mac,
        string Port,
        string Ip,
        string Firmware,
        string SerialNumber,
        bool PortOne,
        short ProtocolOne,
        string ProtocolOneDetail,
        short BaudRateOne,
        bool PortTwo,
        short ProtocolTwo,
        string ProtocolTwoDetail,
        short BaudRateTwo,
        int LocationId,
        bool IsActive) : BaseDto(LocationId,IsActive);
}
