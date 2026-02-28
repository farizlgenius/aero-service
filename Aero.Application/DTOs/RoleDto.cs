
using Aero.Application.Interfaces;
using Aero.Domain.Entities;

namespace Aero.Application.DTOs
{


    public sealed record RoleDto(int Id,short DriverId,string Name,List<FeatureDto> Features,int LocationId,bool Status) : BaseDto(LocationId,Status);
}
