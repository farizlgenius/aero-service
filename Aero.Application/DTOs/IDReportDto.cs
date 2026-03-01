namespace Aero.Application.DTOs
{
    public sealed record IdReportDto(
    short ComponentId,
    int SerialNumber,
    string MacAddress,
    string Ip,
    string Port,
    string Firmware,
    short HardwareType,
    string HardwareTypeDescription
);
}
