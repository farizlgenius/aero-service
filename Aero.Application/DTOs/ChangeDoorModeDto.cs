namespace Aero.Application.DTOs
{


    public sealed record ChangeDoorModeDto(
        short DriverId,
        string Mac,
        short Mode
        );
}
