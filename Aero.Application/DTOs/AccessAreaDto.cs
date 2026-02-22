using Aero.Application.Interfaces;
using Aero.Domain.Entities;
using Aero.Domain.Interface;


namespace Aero.Application.DTOs
{


    public sealed record AccessAreaDto(
        string Name,
        short DriverId,
        short MultiOccupancy,
        short AccessControl,
        short OccControl,
        short OccSet,
        short OccMax,
        short OccUp,
        short OccDown,
        short AreaFlag
        );


}
