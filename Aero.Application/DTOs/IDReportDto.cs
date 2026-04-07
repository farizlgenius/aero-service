namespace Aero.Application.DTOs
{
    public sealed record IdReportDto(
    short ScpId,
    int SerialNumber,
    string MacAddress,
    string Ip,
    string Port,
    string Firmware,
    short HardwareType,
    string HardwareTypeDescription
);
}
