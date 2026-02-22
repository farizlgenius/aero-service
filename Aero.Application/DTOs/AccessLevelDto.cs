
using Aero.Application.DTOs;
using Aero.Application.Interfaces;
using Aero.Domain.Entities;

namespace Aero.Application.DTOs
{

   
    public sealed record AccessLevelDto(
        string Name,
        List<AccessLevelComponentDto> Components,
        int LocationId,
        bool IsActive
        ) : BaseDto(LocationId,IsActive);
    
}
