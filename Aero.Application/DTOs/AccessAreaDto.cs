using Aero.Application.Interfaces;
using Aero.Domain.Entities;
using Aero.Domain.Interface;


namespace Aero.Application.DTOs
{


    public sealed record AccessAreaDto(
        int Id,
        int DeviceId,
        short DriverId,
        string Name,
        short MultiOccupancy,
        short AccessControl,
        short OccControl,
        short OccSet,
        short OccMax,
        short OccUp,
        short OccDown,
        short AreaFlag,
        int LocationId,
        bool IsActive
        ) : BaseDto(LocationId,IsActive);


}
