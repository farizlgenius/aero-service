using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aero.Application.DTOs
{
    public sealed record CreateIntervalDto(DaysInWeekDto Days, string DaysDetail, string Start, string End, int LocationId, bool Status) : BaseDto(LocationId, Status);
}
