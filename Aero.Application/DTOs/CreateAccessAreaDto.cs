using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aero.Application.DTOs
{
    public sealed record CreateAccessAreaDto
    (
        string Name,
        short DriverId,
        short MultiOccupancy,
        short AccessControl,
        short OccControl,
        short OccSet,
        short OccMax,
        short OccUp,
        short OccDown,
        short AreaFlag,
        int LocationId,
        bool Status
        ) : BaseDto(LocationId,Status);
}
