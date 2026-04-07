using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aero.Application.DTOs
{
    public sealed record CreateAccessLevelDto(
        string Name,
        List<AccessLevelComponentDto> Components,
        int LocationId,
        bool IsActive
        ) : BaseDto(LocationId, IsActive);
}
