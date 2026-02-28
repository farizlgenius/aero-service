using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aero.Application.DTOs
{
    public sealed record CreateTimeZoneDto( string Name, short Mode, string Active, string Deactive, List<IntervalDto> Intervals, int LocationId, bool Status) : BaseDto(LocationId, Status);
}
