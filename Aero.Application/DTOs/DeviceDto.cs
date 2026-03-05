
using Aero.Domain.Entities;

namespace Aero.Application.DTOs
{
    public sealed record DeviceDto(
        int Id,
        short DriverId,
    string Name,
    int HardwareType,
    string HardwareTypeDetail,
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
    string ProtocolOneDetail,
    short BaudRateOne,
    bool PortTwo,
    short ProtocolTwo,
    string ProtocolTwoDetail,
    short BaudRateTwo,
    DateTime LastSync,
    int LocationId,
    bool IsActive
) : BaseDto(LocationId,IsActive);
}
