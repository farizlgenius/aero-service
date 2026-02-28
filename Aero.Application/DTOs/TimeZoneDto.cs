

using Aero.Domain.Entities;

namespace Aero.Application.DTOs
{

    public sealed record TimeZoneDto(int Id,short DriverId,string Name,short Mode,string Active,string Deactive,List<IntervalDto> Intervals,int LocationId,bool Status) : BaseDto(LocationId,Status);
}
