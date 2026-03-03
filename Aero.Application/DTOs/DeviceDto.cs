
using Aero.Domain.Entities;

namespace Aero.Application.DTOs
{
    public sealed record DeviceDto(
        int Id,
        int DriverId,
    string Name,
    int HardwareType,
    string HardwareTypeDescription,
    string Mac,
    string Ip,
    string Firmware,
    string Port,
    List<ModuleDto> Modules,
    string SerialNumber,
    bool IsUpload,
    bool IsReset,
    bool PortOne,
    short ProtocolOne,
    string ProtocolOneDescription,
    short BaudRateOne,
    bool PortTwo,
    short ProtocolTwo,
    string ProtocolTwoDescription,
    short BaudRateTwo,
    DateTime LastSync,
    int LocationId,
    bool IsActive
) : BaseDto(LocationId,IsActive);
}
