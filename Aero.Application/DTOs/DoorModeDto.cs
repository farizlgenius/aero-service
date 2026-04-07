namespace Aero.Application.DTOs
{


    public sealed record DoorModeDto(string Name,short Value,string Description,int Location,bool IsActive) : BaseDto(Location,IsActive);
}
