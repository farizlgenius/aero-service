namespace Aero.Application.DTOs
{


    public sealed record ChangeDoorModeDto(
        int DeviceId,
        int DriverId,
        short Mode
        );
}
