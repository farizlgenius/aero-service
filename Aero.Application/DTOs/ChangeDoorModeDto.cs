namespace Aero.Application.DTOs
{


    public sealed record ChangeDoorModeDto(
        int Id,
        int ScpId,
        int AcrId,
        short Mode
        );
}
