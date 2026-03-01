using Aero.Domain.Entities;

namespace Aero.Application.DTOs
{

    public sealed record CreateDeviceDto(string Name,int HardwareType,string HardwareTypeDetail,string Port,
    string Ip,string Firmware,string SerialNumber,bool PortOne,short ProtocolOne,string ProtocolOneDescription,
    short BaudRateOne,bool PortTwo,short ProtocolTwo,string ProtocolTwoDescription,short BaudRateTwo,int LocationId,bool IsActive) : BaseDto(LocationId,IsActive);
}
